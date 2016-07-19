using MyvarCraft.Core.Objects.Meta;
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

        public Location Posistion { get; set; }
        public Look Look { get; set; }

        public bool Spawned { get; set; } = true;
    }
}
