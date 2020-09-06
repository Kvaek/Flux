using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Flux.Network;

namespace Flux.Services {
	public class NetworkService : Service {
		public NetworkService() {
			Name = "Network";
		}
		
		private TcpListener _serverListener = new TcpListener(IPAddress.Any, 25565);

		public override void Start() {
			_serverListener.Start();
			Console.WriteLine("Ready for connections...");
			Console.WriteLine("To shutdown the server safely press CTRL+C");
		}

		public override void Stop() {
			_serverListener.Stop();
		}
		
		public override void Tick() {
			if (!_serverListener.Server.IsBound) return;
			TcpClient client = _serverListener.AcceptTcpClient();
			new Task(() => SetupClient(client)).Start();
		}

		private void SetupClient(TcpClient tcp) {
			NetworkStream clientStream = tcp.GetStream();
			Client client = new Client(tcp);
			if(!ClientManagerService.ConnectedClients.Contains(client)) ClientManagerService.AddClient(ref client);
		}
	}
}