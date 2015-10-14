using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class Pong : Packet
    {
        public long Payload { get; set; }

        public Pong()
        {
            ID = 0x01;
        }

        public override void Write(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();
            read.WriteLong(Payload);
            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }

    }
}
