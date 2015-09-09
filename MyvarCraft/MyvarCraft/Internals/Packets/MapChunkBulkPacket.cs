using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class MapChunkBulkPacket : Packet
    {
        public byte SkyLightSent { get; set; } = 0x01;
        public int ChunkColumnCount { get; set; } = 49;

        public MapChunkBulkPacket()
        {
            ID = 0x26;
            IlegalState = 2;
        }

        

        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            read.WriteByte(SkyLightSent);
            read.WriteVarInt(ChunkColumnCount);
            //flat land
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; x++)
                {
                    read.WriteInt(x);//X
                    read.WriteInt(y);//Y
                    read.WriteUShort(ushort.MaxValue);//bitmask
                }
            }

            return read.Flush(ID);
        }

    }
}
