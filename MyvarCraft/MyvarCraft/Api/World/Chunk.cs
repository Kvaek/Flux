using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Api.World
{
    public class Chunk
    {
        public List<BlockStorage> Blocks { get; set; } = new List<BlockStorage>()
        {
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
            new BlockStorage(),
        };

        public byte[] ToPacketFormat()
        {
            List<byte> Chunks = new List<byte>();

            foreach(var i in Blocks)
            {
                Chunks.AddRange(i.Write());
            }

            return Chunks.ToArray();
        }
    }
}
