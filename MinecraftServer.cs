using System.Collections.Generic;
using Flux.Services;

namespace Flux {
	public class MinecraftServer {
		public ServiceManager sm = new ServiceManager();
		public void Start() {
			sm.Init();
			
			sm.AddService(new NetworkService());
			
			sm.Start();
		}

		public void Stop() {
			sm.Stop();
		}
		
		public static Dictionary<string, object> Version { get; } = new Dictionary<string, object> {
			{"Name", "1.8"},
			{"Protocol", 47}
		};
		public static Dictionary<string, object> Players { get; set; } = new Dictionary<string, object> {
			{"Current", ClientManagerService.ConnectedClients.Count},
			{"Max", 20}
		};
		public static string MOTD { get; set; } = "A .NET Server";
	}
}