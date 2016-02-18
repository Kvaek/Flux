using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Internals
{
    public class Config
    {
        public int DefaultLevel { get; set; } = 0;
        public string Description { get; internal set; } = "A MyvarCraft Server";
        public int MaxPlayers { get; internal set; } = 100;
        public int Port { get; set; } = 25565;
        public int Protical { get; internal set; } = 94;
        public string Version { get; internal set; } = "15w51b";
    }
}
