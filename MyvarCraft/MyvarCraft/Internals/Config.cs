using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals
{
    public class Config
    {
        public string Description { get; set; } = "A MyvarCraft Server";
        public int MaxPlayers { get; set; } = 100;
        public int Port { get; set; } = 25565;
        public int Protical { get; set; } = 78;
        public string Version { get; set; } = "15w41b";
    }
}
