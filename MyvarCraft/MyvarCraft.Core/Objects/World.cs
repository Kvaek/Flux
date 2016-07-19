using MyvarCraft.Core.Objects.WorldGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects
{
    public class World
    {
        public List<Player> Players { get; set; } = new List<Player>();
        public IWorldGenerator WorldGenerator { get; set; } = new FlatLand();
    }
}
