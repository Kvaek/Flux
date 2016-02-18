using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects.ServerList
{
    public class Responce
    {
        public Version version { get; set; } = new Version();
        public Description description { get; set; } = new Description();
        public Players players { get; set; } = new Players();
    }

    public class Version
    {
        public string name { get; set; } = MyvarCraft.Config.Version;
        public int protocol { get; set; } = MyvarCraft.Config.Protical;
    }

    public class Description
    {
        public string text { get; set; } = MyvarCraft.Config.Description;
    }

    public class Players
    {
        public int max { get; set; } = MyvarCraft.Config.MaxPlayers;
        public int online { get; set; } = 0;
    }
}
