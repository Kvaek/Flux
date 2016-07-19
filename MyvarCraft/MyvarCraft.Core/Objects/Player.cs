using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects
{
    public class Player
    {
        public string Name { get; set; }
        public Guid OwnerID { get; set; } = Guid.NewGuid();
        public int GameMode { get; set; } = 1;

        public double X { get; set; } = 0;
        public double Y { get; set; } = 50;
        public double Z { get; set; } = 0;

        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;

        public bool Spawned { get; set; } = true;
    }
}
