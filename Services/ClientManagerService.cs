using System;
using System.Collections.Generic;
using System.Linq;
using Flux.Network;

namespace Flux.Services {
	public class ClientManagerService : Service {
		public ClientManagerService() {
			Name = "ClientManager";
		}
		private static List<Client> _connectedClients = new List<Client>();
		
		public static List<Client> ConnectedClients => _connectedClients;
		public override void Tick() {
			foreach (Client client in _connectedClients) {
				
			}
		}

		public override void Start() {
			
		}

		public override void Stop() {
			
		}

		public static void AddClient(ref Client client) {
			if (_connectedClients.Contains(client)) return;
			
			_connectedClients.Add(client);
		}

		public static void RemoveClient(Client client) {
			if (!_connectedClients.Contains(client)) return;
			
			_connectedClients.Remove(client);
			GC.Collect();
		}
	}
}