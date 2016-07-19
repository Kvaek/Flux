using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects.Meta
{
    public class Chunck
    {
        public List<List<Block>> RawData { get; set; } = new List<List<Block>>();

        public Chunck()
        {
            for (int i = 0; i < 16; i++)
            {
                var arr = new List<Block>(16 * 16);
                for (int t = 0; t < 16 * 16; t++)
                {
                    arr.Add(new Block());
                }
                RawData.Add(arr);
            }
        }

        public void SetBlock(Block b, Location l)
        {
            RawData[(int)l.Y][(int)l.X + ((int)l.Z * 16)] = b;
        }

        public Block GetBlock(Location l)
        {
            return RawData[(int)l.Y][(int)l.X + ((int)l.Z * 16)];
        }
        public Block GetBlock(int x, int y, int z)
        {
            return RawData[y][x + (z * 16)];
        }

        public byte[] FlushData()
        {
            var re = new List<byte>();

            return re.ToArray();
        }
    }
}
