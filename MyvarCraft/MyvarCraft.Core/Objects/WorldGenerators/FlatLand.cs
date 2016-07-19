using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyvarCraft.Core.Objects.Meta;

namespace MyvarCraft.Core.Objects.WorldGenerators
{
    public class FlatLand : IWorldGenerator
    {
        public Chunck GetChunck(int ax, int ay)
        {
            var c = new Chunck();

            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        c.SetBlock(new Block() { ID = 1, Damage = 0 }, new Location() { X = z, Y = y, Z = z });
                    }
                }
            }

            return c;
        }
    }
}
