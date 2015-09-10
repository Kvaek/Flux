using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Api.Map
{
    public class Block
    {
        public int ID { get; set; } = 0;
        public int Metta { get; set; } = 0;
        public byte Light { get; set; } = 0xff;
        public byte Sky { get; set; } = 0xff;
        public string Name { get; set; } = "minecraft:air";
    }
}
