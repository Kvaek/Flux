using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class Ping : Packet
    {
        public long Payload { get; set; }

        public Ping()
        {
            IDs.Add(-1);
            IDs.Add(0x01);
            IDs.Add(-1);
        }

        public override Packet Parse(byte[] data)
        {
            var re = new Ping();
            MinecraftStream ms = new MinecraftStream();
            ms.ReadVarInt(data);
            re.ID = ms.ReadVarInt(data);
            re.Payload = ms.ReadLong(data);
            return re;
        }
    }
}
