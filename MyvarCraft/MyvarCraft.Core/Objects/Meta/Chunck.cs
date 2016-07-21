using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects.Meta
{
    public class Chunk
    {
        public BlockStorage RawData { get; set; } = new BlockStorage();

        public Chunk()
        {
            
        }


        public Block GetBlock(int x, int y, int z)
        {
            var d = RawData.Get(z, y, z);
            return new Block() { ID =  (d | d >> 1) };
        }

        public void SetBlock(int x, int y, int z, int id, int damage)
        {
            RawData.Set(z, y, z, 1 << id | damage);
        }
    }
}
