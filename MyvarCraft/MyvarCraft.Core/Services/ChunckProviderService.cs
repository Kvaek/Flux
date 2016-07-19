using MyvarCraft.Core.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Services
{
    public class ChunckProviderService : IService
    {
        public string Name { get; set; } = "ChunckProviderService";

        public void Start()
        {
            
        }

        public void Stop()
        {
           
        }

        public void Tick()
        {
           lock(MinecraftServer.Worlds)
            {
                foreach(var i in MinecraftServer.Worlds)
                {
                    foreach(var p in i.Players)
                    {
                        if(p.Spawned && !p.SpawnedCunckLoaded)
                        {
                            ChunckData cd = new ChunckData() { Owner = p.OwnerID };
                            cd.Data = i.WorldGenerator.GetChunck(0,0);

                            NetworkService.EnqueuePacket(cd);

                            p.SpawnedCunckLoaded = true;
                        }
                    }
                }
            }
        }
    }
}
