using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Api.World
{
    public abstract class WorldProvider
    {
        public abstract Chunk GetChunk(int x, int y);
    }
}
