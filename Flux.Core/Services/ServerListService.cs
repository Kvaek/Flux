using Flux.Core;
using Flux.Core.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flux.Core.Objects;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Flux.Core.Services {
	public class ServerListService : IService {
		public string Name { get; set; } = "ServerList";

		public void Start() { }

		public void Stop() { }

		public void Tick() {
			if (NetworkService.IsAvalible(new Ping())) {
				Ping p = NetworkService.GetPacket<Ping>() as Ping;
				NetworkService.EnqueuePacket(new Pong {
					Owner = p.Owner, 
					Payload = p.Payload,
					Send = true, 
					KillSwitch = true
				});
			}

			if (NetworkService.IsAvalible(new Request())) {
				Request p = NetworkService.GetPacket<Request>() as Request;
				Response resp = new Response {
					Owner = p.Owner, 
					Json = JsonSerializer.ToJsonString(new SResponse())
				};
				NetworkService.EnqueuePacket(resp);
			}

			if (NetworkService.IsAvalible(new HandShake())) {
				HandShake p = NetworkService.GetPacket<HandShake>() as HandShake;
				foreach (ConnectionWrapper i in NetworkService._cw)
					if (i.OwnerID == p.Owner) {
						i.State = p.NextState;
						break;
					}
			}
		}
	}

	public class SResponse {
		public Version version { get; set; } = new Version();
		public Description description { get; set; } = new Description();
		public Players players { get; set; } = new Players();
	}

	public class Version {
		public string name { get; set; } = "1.12.2";
		public int protocol { get; set; } = 340;
	}

	public class Description {
		public string text { get; set; } = "A .NET server";
	}

	public class Players {
		public int max { get; set; } = 100;
		public int online { get; set; } = 0;

		public Players() {
			foreach (World i in MinecraftServer.Worlds) online += i.Players.Count;
		}
	}
}