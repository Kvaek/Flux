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
        public void Start()
        {
            ServiceManager.Reset();

            //register all our serveces
            ServiceManager.AddServece(new NetworkService());
            ServiceManager.AddServece(new ServerListService());

            ServiceManager.Start();
        }
    }
}
