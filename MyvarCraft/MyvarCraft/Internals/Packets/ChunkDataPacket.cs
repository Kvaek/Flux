using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class ChunkDataPacket : Packet
    {
        public int ChunkX { get; set; } = 0;
        public int ChunkY { get; set; } = 0;
        public byte GroundUpContinuous { get; set; } = 1;
        public ushort PrimaryBitMask { get; set; } = 0xffff;
        public int Size { get; set; } = (16 * 256 * 16);


        public ChunkDataPacket()
        {
            ID = 0x21;
            IlegalState = 2;
        }



        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            //Chuck Colum Meta
            read.WriteInt(ChunkX);
            read.WriteInt(ChunkY);
            read.WriteByte(GroundUpContinuous);
            read.WriteUShort(PrimaryBitMask);
            //data size = ((16 * 256 * 16) * 3) + 256 
            int si = ((Size * 3) + 256);
            read.WriteVarInt(si);

            for (int y = 0; y < 256; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {

                        if (y < 2)//Flat land gen
                        {
                            read.WriteUShort((ushort)((1 << 4) | 0));//stone
                        }
                        else
                        {
                            read.WriteByte(0);//air block
                            read.WriteByte(0);
                        }

                    }
                }
            }
            for (int y = 0; y < (Size); y++)
            {

                read.WriteByte(0xff);//sky = 15, block light = 15      
            }

            for (int y = 0; y < 256; y++)
            {
                read.WriteByte(1);//plains biome

            }
            return read.Flush(ID);
        }

    }
}
