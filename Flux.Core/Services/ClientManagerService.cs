using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flux.Core.Entities;

namespace Flux.Core.Services {
	public class ClientManagerService : Service {
		public ClientManagerService() => Name = "ClientManager";

		public static List<Client> ConnectedClients { get; } = new List<Client>();
		public static int ConnectedCount = ConnectedClients.Count;

		public override async Task Tick() {
			foreach (Client client in ConnectedClients) { }
		}

		public override async Task Start() { }

		public override async Task Stop() { }

		public static void AddClient(ref Client client) {
			if (ConnectedClients.Contains(client)) return;

			ConnectedClients.Add(client);
		}

		public static void RemoveClient(Client client) {
			if (!ConnectedClients.Contains(client)) return;

			ConnectedClients.Remove(client);
			GC.Collect();
		}
	}
}