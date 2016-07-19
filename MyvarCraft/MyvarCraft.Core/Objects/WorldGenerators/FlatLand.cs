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
            int id = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        c.SetBlock(new Block() { ID = 3, Damage = 0 }, new Location() { X = x, Y = y, Z = z });
                    }
                }
            }

            return c;
        }
    }
}
