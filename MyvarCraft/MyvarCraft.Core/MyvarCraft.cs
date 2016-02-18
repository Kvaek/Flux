using MyvarCraft.Core.Internals;
using MyvarCraft.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core
{
    public class MyvarCraft
    {
        public static List<Level> Levels { get; set; } = new List<Level>()
        {
            new Level()
        };
        public static Config Config { get; set; } = new Config();

        private NetworkManager _nm = new NetworkManager();

        public void Start()
        {
            _nm.Listen();
        }
    }
}
