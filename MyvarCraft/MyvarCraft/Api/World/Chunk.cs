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
        public BlockStorage Blocks { get; set; } = new BlockStorage();

        public byte[] ToPacketFormat()
        {
            return Blocks.Write();
        }
    }
}
