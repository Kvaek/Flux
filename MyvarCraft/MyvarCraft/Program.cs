using MyvarCraft.Core;
using MyvarCraft.Core.Internals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!File.Exists("settings.json"))
            {
                File.WriteAllText("settings.json", JsonConvert.SerializeObject(new Settings() { }, Formatting.Indented));
            }

            var mc = new MyvarCraftServer(JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json")));
            mc.Start();

            while(true)
            {

            }
        }
    }
}
