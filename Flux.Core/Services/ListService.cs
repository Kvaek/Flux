using System.Threading.Tasks;

namespace Flux.Core.Services {
	public class ListService : Service {
		public ListService() => Name = "ServerList";

		public override async Task Start() { }

		public override async Task Stop() { }

		public override async Task Tick() { }
	}
}