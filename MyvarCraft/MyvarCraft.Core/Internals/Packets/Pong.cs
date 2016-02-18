using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Internals.Packets
{
    public class Pong : Packet
    {
        public long Payload { get; set; }

        public Pong()
        {
            IDs.Add(-1);
            IDs.Add(0x01);
            IDs.Add(-1);
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
