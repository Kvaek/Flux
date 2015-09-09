using MyvarCraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            MyvarCraftServer mcs = new MyvarCraftServer();
            mcs.Start();

            while (true) { }
        }
    }
}
