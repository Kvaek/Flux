using System;
using System.Net.Sockets;

namespace Flux.Network {
	public class Client {
		public TcpClient TcpClient { get; set; }
		public NetworkStream Stream { get; set; }
		public Guid ClientId { get; set; } = Guid.NewGuid();
		public bool Connected { get; set; } = false;
		public object Player { get; set; }

		public Client(TcpClient client) {
			TcpClient = client;
			Stream = client.GetStream();
		}
	}
}