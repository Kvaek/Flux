using MyvarCraft.Networking;
using MyvarCraft.Networking.Packets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft.Api
{
    public class Player
    {

        public TcpClient _tcp { get; set; }

        public int State { get; set; } = 0;



        public Player(TcpClient c)
        {
            _tcp = c;

            ThreadPool.QueueUserWorkItem((x) =>
            {

                var tcp = x as TcpClient;
                var stream = tcp.GetStream();

                while (tcp.Connected)
                {
                    if (stream.DataAvailable)
                    {
                        try
                        {
                            byte[] buffer = new byte[4096];
                            int bytesread = stream.Read(buffer, 0, buffer.Length);
                            Array.Resize(ref buffer, bytesread);

                            HandlePacket(PacketManager.GetPacket(buffer, State), stream);

                        }
                        catch
                        { }
                    }
                }

                stream.Close();
                stream.Dispose();
                tcp.Close();
                Thread.CurrentThread.Abort();

            }, _tcp);

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
            }
        }

        public void Update()
        {

        }
    }
}
