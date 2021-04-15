using Flux.Core.Objects;
using Flux.Core.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Services {
	public class LoginService : IService {
		public string Name { get; set; } = "LoginService";


		public static void Disconnected(Guid pl) {
			lock (MinecraftServer.Worlds) {
				foreach (World i in MinecraftServer.Worlds)
				foreach (Player p in i.Players.ToArray())
					if (p.OwnerID == pl) {
						i.Players.Remove(p);
						return;
					}
			}
		}

		public void Start() { }

		public void Stop() { }


		public void Tick() {
			lock (MinecraftServer.Worlds) {
				foreach (World i in MinecraftServer.Worlds)
				foreach (Player p in i.Players) {
					KeepAlive ka = new KeepAlive() { Owner = p.OwnerID };
					ka.KeepAliveID = new Random().Next();

					NetworkService.EnqueuePacket(ka);
				}
			}

			if (NetworkService.IsAvalible(new LoginStart())) {
				LoginStart p = NetworkService.GetPacket<LoginStart>() as LoginStart;
				Player pl = new Player() { OwnerID = p.Owner, Name = p.Name };


				LoginSuccess Logins = new LoginSuccess() { Owner = p.Owner };
				Logins.Username = p.Name;
				Logins.UUID = GetUuid(p.Name);

				NetworkService.EnqueuePacket(Logins);

				JoinGame jg = new JoinGame() { Owner = p.Owner };
				jg.EntityID = 0;
				jg.Gamemode = (byte) pl.GameMode;
				jg.Dimension = 0;
				jg.Difficulty = 0;
				jg.MaxPlayers = 255;
				jg.LevelType = "default";
				jg.ReducedDebugInfo = 0;

				NetworkService.EnqueuePacket(jg);


				PlayerPositionAndLook ppal = new PlayerPositionAndLook() { Owner = p.Owner };
				ppal.X = pl.Posistion.X;
				ppal.Y = pl.Posistion.Y;
				ppal.Z = pl.Posistion.Z;
				ppal.Yaw = pl.Look.Yaw;
				ppal.Pitch = pl.Look.Pitch;
				ppal.Flags = 255;

				NetworkService.EnqueuePacket(ppal);

				pl.Spawned = true;

				MinecraftServer.Worlds[0].Players.Add(pl);
			}
		}


		private string GetUuid(string username) {
			try {
				WebClient wc = new WebClient();
				string result = wc.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username);
				string[] _result = result.Split('"');
				if (_result.Length > 1) {
					string uuid = _result[3];
					return new Guid(uuid).ToString();
				}

				return Guid.NewGuid().ToString();
			} catch { return Guid.NewGuid().ToString(); }
		}
	}
}