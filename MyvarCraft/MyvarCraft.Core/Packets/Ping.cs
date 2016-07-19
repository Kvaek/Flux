using MyvarCraft.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Packets
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

        public override Packet Read(byte[] data)
        {
            var re = new Ping();
            MinecraftStream ms = new MinecraftStream(data);
            re.ID = ms.ReadVarInt();
            re.Payload = ms.ReadLong();
            return re;
        }
    }
}
