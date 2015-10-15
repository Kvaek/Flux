
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Api.World.Providers
{
    public class FlatLand : WorldProvider
    {
        public Dictionary<string, Chunk> Chunks = new Dictionary<string, Chunk>();


        public override Chunk GetChunk(int x, int y)
        {
            if(Chunks.ContainsKey(x + "," + y))
            {
                return Chunks[x + "," + y];
            }

            var ch = new Chunk();

            for (int y1 = 0; y1 < 4; y1++)
            {
                for (int x1 = 0; x1 < 16; x1++)
                {
                    for (int z1 = 0; z1 < 16; z1++)
                    {
                        ch.Blocks.Set(x1, y1, z1, 1 << 4 | 0);
                    }
                }
            }

            Chunks.Add(x + "," + y, ch);
            return ch;
        }
    }
}
