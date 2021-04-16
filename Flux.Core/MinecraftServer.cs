using Flux.Core.Objects;
using Flux.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core {
	public class MinecraftServer {
		public static List<World> Worlds { get; set; } = new List<World> { new World() };

		public void Start() {
			ServiceManager.Reset();

			//register all our services
			ServiceManager.AddService(new NetworkService());
			ServiceManager.AddService(new ServerListService());
			ServiceManager.AddService(new LoginService());
			ServiceManager.AddService(new ChunkProviderService());

			ServiceManager.Start();
		}
	}
}