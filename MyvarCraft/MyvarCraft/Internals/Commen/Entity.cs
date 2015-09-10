using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Commen
{
    public class Entity
    {
        public static int GUID = 0;

        public double X { get; set; } = 10;
        public double Y { get; set; } = 30;
        public double Z { get; set; } = 10;
        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;
        public byte OnGround { get; set; } = 0;
        public int UID { get; set; } = Entity.GUID++;
    }
}
