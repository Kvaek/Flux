using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Internals
{
    public class Settings
    {
        public string ProticalJsonFile { get; set; } = "1.9-pre1.json";
        public string VertionDllFile { get; set; } = "1.9-pre1.dll";

        public int Port { get; set; } = 25565;

        public string Description { get; set; } = "A MyvarCraft Server";
        public int MaxPlayers { get; set; } = 100;
        public int ViewDistance { get; set; } = 15;
        public int Gamemode { get; set; } = 1;
        public int Protical { get; set; } = 103;
        public string Version { get; set; } = "1.9-pre1";
    }
}
