using MyvarCraft.Networking;
using MyvarCraft.Networking.Packets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft.Api
{
    public class Player
    {

        public NetworkStream _ns { get; set; }

        public int State { get; set; } = 0;
        public string Name { get; set; }


        public Player(TcpClient c)
        {
            
            var stream = c.GetStream();
            _ns = stream;

            ThreadPool.QueueUserWorkItem((x) =>
            {
                var tcp = x as TcpClient;
                

                while (tcp.Connected)
                {
                    if (_ns.DataAvailable)
                    {
                        try
                        {
                            byte[] buffer = new byte[4096];
                            int bytesread = _ns.Read(buffer, 0, buffer.Length);
                            Array.Resize(ref buffer, bytesread);

                            HandlePacket(PacketManager.GetPacket(buffer, State), _ns);

                        }
                        catch
                        {
                            _ns.Close();
                            _ns.Dispose();
                            tcp.Close();
                            Thread.CurrentThread.Abort();
                        }
                    }
                }

                _ns.Close();
                _ns.Dispose();
                tcp.Close();
                Thread.CurrentThread.Abort();

            }, c);

        }

        public void HandlePacket(Packet c, NetworkStream ns)
        {
            if (c == null)
            {
                return;
            }

            switch (State)
            {
                case 0:

                    if (c is HandShake)
                    {
                        var x = c as HandShake;
                        State = x.NextState;
                    }

                    break;
                case 1:

                    if (c is Request)
                    {
                        var x = c as Request;
                        var resp = new Response();
                        resp.Json = JsonConvert.SerializeObject(new ServerList.Responce(), Formatting.Indented);
                        resp.Write(ns);
                    }

                    if (c is Ping)
                    {
                        var x = c as Ping;
                        var png = new Pong();
                        png.Payload = x.Payload;
                        png.Write(ns);
                    }

                    break;
                case 2:

                   

                    if (c is LoginStart)
                    {
                        //login
                        var x = c as LoginStart;
                        Name = x.Name;

                        var ls = new LoginSuccess();
                        ls.Username = Name;
                        ls.UUID = GetUuid(Name);
                        ls.Write(ns);

                        //join game
                        var jg = new JoinGame();
                        jg.EntityID = 0;
                        jg.Gamemode = 0;
                        jg.Dimension = 0;
                        jg.Difficulty = 0;
                        jg.MaxPlayers = 255;
                        jg.LevelType = "default";
                        jg.ReducedDebugInfo = 0;
                        jg.Write(ns);

                        //send terain


                    }


                    break;
            }
        }


        private string GetUuid(string username)
        {
            try
            {
                var wc = new WebClient();
                var result = wc.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username);
                var _result = result.Split('"');
                if (_result.Length > 1)
                {
                    var uuid = _result[3];
                    return new Guid(uuid).ToString();
                }
                return Guid.NewGuid().ToString();
            }
            catch
            {
                return Guid.NewGuid().ToString();
            }
        }

        public void Update()
        {
            if (_ns.CanWrite)
            {
                var ka = new KeepAlive();
                ka.KeepAliveID = new Random().Next();
                ka.Write(_ns);
            }
        }
    }
}
