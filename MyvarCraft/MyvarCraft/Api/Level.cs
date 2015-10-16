using MyvarCraft.Api.World;
using MyvarCraft.Api.World.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft.Api
{
    public class Level
    {
        public List<Player> Players { get; set; } = new List<Player>();

        public WorldProvider World { get; set; } = new FlatLand();

        public void BroadCastMessage(MCChatMessage m, int loc)
        {
            try
            {
                foreach (var i in Players.ToArray())
                {
                    i.SendChat(m, loc);
                }
            }
            catch
            {

            }
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem((x) =>
            {
                while (true)
                {
                    try
                    {
                        foreach (var i in Players.ToArray())
                        {
                            i.Update();
                        }
                    }
                    catch
                    {

                    }
                }
            });
        }
    }
}
