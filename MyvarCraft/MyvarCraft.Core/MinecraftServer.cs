using MyvarCraft.Core.Objects;
using MyvarCraft.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core
{
    public class MinecraftServer
    {
        public static List<World> Worlds { get; set; } = new List<World>()
        {
            new World()
        };

        public void Start()
        {
            ServiceManager.Reset();

            //register all our serveces
            ServiceManager.AddServece(new NetworkService());
            ServiceManager.AddServece(new ServerListService());
            ServiceManager.AddServece(new LoginService());

            ServiceManager.Start();
        }
    }
}
