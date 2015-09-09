using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class SpawnPositionPacket : Packet
    {
        public int X { get; set; } = 10;       
        public int Y { get; set; } = 10;
        public int Z { get; set; } = 10;

        public SpawnPositionPacket()
        {
            ID = 0x05;
            IlegalState = 1;
        }

        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            read.WriteInt64(((X & 0x3FFFFFF) << 38) | ((Y & 0xFFF) << 26) | (Z & 0x3FFFFFF));           
            return read.Flush(ID);
        }

    }
}
