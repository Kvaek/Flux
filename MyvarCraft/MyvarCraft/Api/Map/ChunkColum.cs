using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Api.Map
{
    public class ChunkColum
    {
        public List<Block> Blocks { get; set; } = new List<Block>();
        public Biome Biome { get; set; } = Biome.Planes;

        public ChunkColum()
        {
            for (int i = 0; i < (16 * 256 * 16); i++)
            {
                Blocks.Add(new Block());//Fill Chunck with air
            }
        }

        public void AddBlock(Block b)
        {
            Blocks.Add(b);
        }

        public void SetBlock(int x, int y, int z, Block b)
        {
            Blocks[(y * (256 * 2) + (z * 16) + x)] = b;//That is confusing as shit, basicly its the way i do double buffers in my os dev stuff
        }

        public byte[] Serlize(int x, int y)
        {
            StreamHelper Cnk = new StreamHelper();
            //Meta Data
            Cnk.WriteInt(x);
            Cnk.WriteInt(y);
            Cnk.WriteByte(1);
            Cnk.WriteUShort(0XFFFF);

            Cnk.WriteVarInt(((16 * 256 * 16) * 3) + 256);//Chunck DataSize

            //Blocks Data
            foreach (var i in Blocks)
            {
                Cnk.WriteUShort((ushort)((i.ID << 4) | i.Metta));//write block
            }

            foreach (var i in Blocks)
            {
                Cnk.WriteByte((byte)(i.Light << 4 | i.Sky));
            }

            for (int i = 0; i < 256; i++)
            {
                Cnk.WriteByte((byte)Biome);//Write Biome

            }
            return Cnk._buffer.ToArray();
        }

    }
}
