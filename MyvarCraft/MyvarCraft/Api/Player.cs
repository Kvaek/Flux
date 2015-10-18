using MyvarCraft.Networking;
using MyvarCraft.Networking.Packets;
using Newtonsoft.Json;
using System;
using System.Collections;
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

        public int LevelID { get; set; } = 0;

        public int State { get; set; } = 0;
        public int GameMode { get; set; } = Globals.Config.Gamemode;
        public string Name { get; set; }

        

        public double X { get; set; } = 0;
        public double Y { get; set; } = 50;
        public double Z { get; set; } = 0;

        public bool Spawned { get; set; } = false;

        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;

        public List<string> LoadedChunks = new List<string>();

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
                        catch(Exception ee)
                        {
                            Console.WriteLine(ee.ToString());
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
                        jg.Gamemode = (byte)GameMode;
                        jg.Dimension = 0;
                        jg.Difficulty = 0;
                        jg.MaxPlayers = 255;
                        jg.LevelType = "default";
                        jg.ReducedDebugInfo = 0;
                        jg.Write(ns);

                        //send player details
                        var ppal = new PlayerPositionAndLook();
                        ppal.X = X;
                        ppal.Y = Y;
                        ppal.Z = Z;
                        ppal.Yaw = Yaw;
                        ppal.Pitch = Pitch;
                        ppal.Flags = 255;
                        ppal.Write(ns);

                        //TODO: player abilitys

                        //send chuncks
                        var cd = new ChunkData();
                        cd.X = 0;
                        cd.Y = 0;
                        cd.GroundUpContinuous = 1;
                        cd.PrimaryBitMask = 1;
                        cd.Data = MyvarCraft.Levels[LevelID].World.GetChunk(0, 0).ToPacketFormat().ToArray();
                        cd.Write(ns);

                        LoadedChunks.Add("0,0");

                        Spawned = true;
                    }

                    if (c is PlayerPositionAndLook && Spawned)
                    {
                        //update player pos
                        var x = c as PlayerPositionAndLook;
                        X = x.X;
                        Y = x.Y;
                        Z = x.Z;
                       // Yaw = x.Yaw;
                       // Pitch = x.Pitch;
                    }


                    if (c is ChatMessage)
                    {                        
                        var x = c as ChatMessage;
                        if (!x.Message.StartsWith("/"))
                        {
                            MyvarCraft.Levels[LevelID].BroadCastMessage(new MCChatMessage("<" + Name + "> " + x.Message), 0);
                        }
                    }
                    break;
            }
        }

        public void SendChat(MCChatMessage m, int loc)
        {
            var cm = new ChatMessage();
            cm.JSONData = JsonConvert.SerializeObject(m);
            cm.Position = (byte)loc;
            cm.Write(_ns);
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

        public static int DivideRoundingUp(int x, int y)
        {
            // TODO: Define behaviour for negative numbers
            int remainder;
            int quotient = Math.DivRem(x, y, out remainder);
            return remainder == 0 ? quotient : quotient + 1;
        }

        public void UpdateChunks()
        {
            var h = Globals.Config.ViewDistance;
            var l = Globals.Config.ViewDistance - (Globals.Config.ViewDistance * 2);

            var X1 = DivideRoundingUp((int)X, 16);
            var Z1 = DivideRoundingUp((int)Z, 16);

            for (int x = l  ; x < h; x++)
            {
                for (int z = l ; z < h; z++)
                {
                    if(!LoadedChunks.Contains((x) + "," + (z)))
                    {
                        //send chunk
                        var cd = new ChunkData();
                        cd.X = x;
                        cd.Y = z;
                        cd.GroundUpContinuous = 1;
                        cd.PrimaryBitMask = 1;
                        cd.Data = MyvarCraft.Levels[LevelID].World.GetChunk(x ,  z ).ToPacketFormat().ToArray();
                        cd.Write(_ns);

                        LoadedChunks.Add((x) + "," + (z ));
                    }
                }
            }

        }


        public void Update()
        {
            if (_ns.CanWrite && State == 2 && Spawned)
            {
                var ka = new KeepAlive();
                ka.KeepAliveID = new Random().Next();
                ka.Write(_ns);

                UpdateChunks();
            }
        }
    }
}
