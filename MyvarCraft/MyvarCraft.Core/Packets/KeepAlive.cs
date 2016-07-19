using MyvarCraft.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Packets
{
    public class KeepAlive : Packet
    {
        public int KeepAliveID { get; set; }

        public KeepAlive()
        {
            ID = 0x1F;
            Send = true;
        }


        public override void Flush(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();
            read.WriteVarInt(KeepAliveID);
            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }
    }
}
