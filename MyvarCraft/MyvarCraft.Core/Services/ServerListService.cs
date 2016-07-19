using MyvarCraft.Core;
using MyvarCraft.Core.Packets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Services
{
    public class ServerListService : IService
    {
        public string Name { get; set; } = "ServerList";

        public void Start()
        {

        }

        public void Stop()
        {

        }

        public void Tick()
        {
            if (NetworkService.IsAvalible(new Ping()))
            {
                var p = NetworkService.GetPacket<Ping>() as Ping;
                NetworkService.EnqueuePacket(new Pong() { Owner = p.Owner, Payload = p.Payload, Send = true, KillSwitch = true });
            }

            if (NetworkService.IsAvalible(new Request()))
            {
                var p = NetworkService.GetPacket<Request>() as Request;
                var resp = new Response() { Owner = p.Owner };
                resp.Json = JsonConvert.SerializeObject(new SResponce(), Formatting.Indented);
                NetworkService.EnqueuePacket(resp);
            }

            if (NetworkService.IsAvalible(new HandShake()))
            {
                var p = NetworkService.GetPacket<HandShake>() as HandShake;
                foreach (var i in NetworkService._cw)
                {
                    if (i.OwnerID == p.Owner)
                    {
                        i.State = p.NextState;
                        break;
                    }
                }
            }
        }
    }

    public class SResponce
    {
        public Version version { get; set; } = new Version();
        public Description description { get; set; } = new Description();
        public Players players { get; set; } = new Players();
    }

    public class Version
    {
        public string name { get; set; } = "1.10";
        public int protocol { get; set; } = 210;
    }

    public class Description
    {
        public string text { get; set; } = "A MyvarCraft server";
    }

    public class Players
    {
        public int max { get; set; } = 100;
        public int online { get; set; } = 0;

        public Players()
        {
            foreach(var i in MinecraftServer.Worlds)
            {
                online += i.Players.Count();
            }
        }
    }
}
