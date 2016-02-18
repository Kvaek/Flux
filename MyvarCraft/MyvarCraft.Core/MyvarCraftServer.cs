using MyvarCraft.Core.Api;
using MyvarCraft.Core.Internals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft.Core
{
    public class MyvarCraftServer
    {
        public static Settings Settings { get; set; } = new Settings();

        public List<World> Worlds { get; set; } = new List<World>() { new World() };

        public Assembly Implentation { get; set; }
        public Protical Protical { get; set; }

        private Type HookClass { get; set; }
        private Dictionary<string, MethodInfo> MethodsIndex { get; set; } = new Dictionary<string, MethodInfo>();

        private object WorldsLock = new object();

        public MyvarCraftServer(Settings s)
        {
            Settings = s;
        }

        public void Start()
        {
            Implentation = Assembly.LoadFile(Path.GetFullPath(Settings.VertionDllFile));
            Protical = JsonConvert.DeserializeObject<Protical>(File.ReadAllText(Settings.ProticalJsonFile));

            Hook();

            ThreadPool.QueueUserWorkItem((d) =>
            {

                TcpListener _tcp = new TcpListener(IPAddress.Any, Settings.Port);
                _tcp.Start();
                while (true)
                {
                    var c = _tcp.AcceptTcpClient();
                    lock(Worlds)
                    {
                        Worlds[0].Players.Add(new Player(c));
                    }
                }

            });

            ThreadPool.QueueUserWorkItem((d) =>
            {

                while (true)
                {
                    lock (Worlds)
                    {
                        foreach (var i in Worlds)
                        {
                            List<Player> removequeue = new List<Player>();

                            try
                            {

                                foreach (var x in i.Players)
                                {
                                    
                                   
                                            try
                                            {
                                                x.Step();

                                                if (x.RecivedPacketQueue.Count != 0)
                                                {
                                                    var packet = x.RecivedPacketQueue.Dequeue();
                                                    MinecraftStream ms = new MinecraftStream();

                                                    int Size = ms.ReadVarInt(packet);
                                                    int ID = ms.ReadVarInt(packet);
                                                 
                                                    foreach (var c in Protical.Packets)
                                                    {
                                                        if (c.ID == ID && c.State == x.State && c.IsServerBound)
                                                        {
                                                            dynamic context = new ExpandoObject();
                                                            context.Player = x;
                                                            context.Packet = new ExpandoObject();
                                                            context.Server = new ExpandoObject();
                                                            context.Server.ServerList = JsonConvert.SerializeObject(new Responce());
                                                            var gg = context.Packet as IDictionary<string, Object>;
                                                           
                                                            foreach (var g in c.Fields)
                                                            {

                                                                var meth = ms.GetType().GetMethod("Read" + g.Value);
                                                                gg.Add(g.Key.Replace(" ", ""), meth.Invoke(ms, new object[] { packet }));

                                                            }

                                                            HandlePacket(c.Name + "." + x.State, context);


                                                        }
                                                    }
                                                }

                                                if (x.SendPacketQueue.Count != 0)
                                                {
                                                    var f = x.SendPacketQueue.Dequeue();
                                                    foreach (var c in Protical.Packets)
                                                    {
                                                        var hh = f as IDictionary<string, Object>;
                                                        if (c.ID == ((int)hh["ID"]) && c.Name == hh["Name"].ToString())
                                                        {
                                                            MinecraftStream read = new MinecraftStream();


                                                            foreach (var g in c.Fields)
                                                            {
                                                                var meth = read.GetType().GetMethod("Write" + g.Value);
                                                                meth.Invoke(read, new object[] { hh[g.Key] });
                                                            }

                                                            var buf = read.Flush(((int)hh["ID"]));
                                                            x._ns.Write(buf, 0, buf.Length);
                                                        }
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                removequeue.Add(x);
                                            }
                                     


                                }

                                foreach(var ii in removequeue)
                                {
                                    i.Players.RemoveAt(i.Players.FindIndex((x) => { return ii.InternalGUID == x.InternalGUID; }));
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }

            });
            /*      dynamic obj = new ExpandoObject();
                  obj.Player = new Player();
                  obj.Packet = new ExpandoObject();
                  obj.Packet.NextState = 1;

                  HandlePacket("Handshake", obj);*/
        }

        private void HandlePacket(string name, dynamic Context)
        {
            if(MethodsIndex.ContainsKey(name))
            {
                MethodsIndex[name].Invoke(null, new object[] { Context });
            }
        }

        private void Hook()
        {
            HookClass = Implentation.Modules.ToList()[0].Assembly.ExportedTypes.Where((x) => { return x.Name == Protical.HookClass; }).First();

            foreach(var i in HookClass.GetMethods())
            {
                var att = i.GetCustomAttribute<Hook>();
                if (att != null)
                {
                    MethodsIndex.Add(att.PacketName + "." + att.State, i);
                }
            }
        }
    }
}
