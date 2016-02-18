using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Internals
{
    public class Protical
    {
        public string HookClass { get; set; }

        public List<Packet> Packets { get; set; } = new List<Packet>();
    }

    public class Packet
    {
        public int ID { get; set; } = -1;
        public int State { get; set; } = -1;
        public string Name { get; set; } = "";

        public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
        public bool IsServerBound { get;  set; }
    }
}
