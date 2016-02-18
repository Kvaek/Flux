using MyvarCraft.Core.Internals;
using MyvarCraft.Core.Internals.Packets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects
{
    public class Player
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int State { get; set; } = 0;

        public TcpClient _tcp = new TcpClient();

        public NetworkStream _ns { get; set; }

        public Player()
        {


            ThreadPool.QueueUserWorkItem((x) =>
            {
                try
                {
                    while (true)
                    {

                        Run();

                    }
                }
                catch (Exception ee)
                {
                    //TODO: send disconect packet

                    Die();
                }
            });
        }

        public void Die()
        {
           
            _tcp.Close();
            _ns.Dispose();
            MyvarCraft.Levels[Level].RemovePlayer(this);
        }

        private Packet GetPacket()
        {
            byte[] buffer = new byte[4096 * 4];
            int bytesread = _ns.Read(buffer, 0, buffer.Length);
            Array.Resize(ref buffer, bytesread);
            return PacketManager.GetPacket(buffer, State);
        }

        public void Run()
        {
            if(_ns.DataAvailable)
            {
                var pck = GetPacket();
                HandlePacket(pck);
            }
        }

        public void HandlePacket(Packet c)
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
                        resp.Json = JsonConvert.SerializeObject(new  ServerList.Responce(), Formatting.Indented);
                        resp.Write(_ns);
                       

                    }

                    if (c is Ping)
                    {
                        var x = c as Ping;
                        var png = new Pong();
                        png.Payload = x.Payload;
                        png.Write(_ns);
                        Die();
                    }

                    break;
                case 2:

                    break;
            }
        }
    }
}
