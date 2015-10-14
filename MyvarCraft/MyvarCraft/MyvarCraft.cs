using MyvarCraft.Api;
using MyvarCraft.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft
{
    public class MyvarCraft
    {
        public object PlayerLock = new object();
     

        public MyvarCraft()
        {

        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem((x) =>
            {
                //Init server
                List<Level> Levels = new List<Level>();
                Levels.Add(new Level()); // Levels[0] = spawn world
                Levels[0].Start(); // Start, Spawn World

                var nm = new NetworkingManager();

                while (true)
                {
                    Levels[0].Players.Add(nm.Listen());                    
                }

            });

           
            while (true)
            {

            }
        }

        public void Stop()
        {

        }
    }
}
