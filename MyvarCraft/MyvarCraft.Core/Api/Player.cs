using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Api
{
    public class Player
    {
        public Guid InternalGUID = Guid.NewGuid();

        public int State { get; set; } = 0;
        public string Name { get; set; } = "";

        public TcpClient _tcp { get; set; }
        public NetworkStream _ns { get; set; }
        public Queue<dynamic> SendPacketQueue { get; set; } = new Queue<dynamic>();
        public Queue<byte[]> RecivedPacketQueue { get; set; } = new Queue<byte[]>();

        public void SendPacket(dynamic Packet)
        {
            SendPacketQueue.Enqueue(Packet);
        }

        public void Step()
        {
            try
            {
                if (_ns.DataAvailable)
                {
                    byte[] buffer = new byte[4096];
                    int bytesread = _ns.Read(buffer, 0, buffer.Length);
                    Array.Resize(ref buffer, bytesread);

                    RecivedPacketQueue.Enqueue(buffer);
                }



            }
            catch
            {

            }
        }

        public Player(TcpClient c)
        {
            _tcp = c;
            _ns = _tcp.GetStream();
        }
    }
}
