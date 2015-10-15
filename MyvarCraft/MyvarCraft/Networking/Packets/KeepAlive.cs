using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class KeepAlive : Packet
    {
        public int KeepAliveID { get; set; }

        public KeepAlive()
        {
            IDs.Add(-1);
            IDs.Add(-1);
            IDs.Add(-1);

            ID = 0x1F;
        }

        public override Packet Parse(byte[] data)
        {
            var re = new KeepAlive();
            MinecraftStream ms = new MinecraftStream();
            ms.ReadVarInt(data);
            re.ID = ms.ReadVarInt(data);
            re.KeepAliveID = ms.ReadVarInt(data);
            return re;
        }

        public override void Write(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();
            read.WriteVarInt(KeepAliveID);
            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }

    }
}
