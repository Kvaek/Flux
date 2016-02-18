using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Api
{
    public class Responce
    {
        public Version version { get; set; } = new Version();
        public Description description { get; set; } = new Description();
        public Players players { get; set; } = new Players();
    }

    public class Version
    {
        public string name { get; set; } = MyvarCraftServer.Settings.Version;
        public int protocol { get; set; } = MyvarCraftServer.Settings.Protical;
    }

    public class Description
    {
        public string text { get; set; } = MyvarCraftServer.Settings.Description;
    }

    public class Players
    {
        public int max { get; set; } = MyvarCraftServer.Settings.MaxPlayers;
        public int online { get; set; } = 0;
    }
}
