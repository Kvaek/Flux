using Flux.Core.Packets;
using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.Core.Services {
	public class NetworkService : IService {
		public string Name { get; set; } = "NetworkService";
		public static List<Packet> Packets { get; set; } = new List<Packet>();
		public static List<ConnectionWrapper> _cw { get; set; } = new List<ConnectionWrapper>();

		private static object _Locker { get; set; } = new object();
		private bool Run { get; set; } = false;

		public static void EnqueuePacket(Packet p) {
			lock (_Locker) { Packets.Add(p); }
		}

		public static Packet GetPacket<T>() where T : new() {
			lock (_Locker) {
				Packet re = Packets.FirstOrDefault(i => i.GetType() == new T().GetType());

				if (re != null) {
					Packets.Remove(re);
					return re;
				}

				return null;
			}
		}

		public static Packet GetPacket(Guid g, bool send = false) {
			lock (_Locker) {
				Packet re = Packets.FirstOrDefault(i => i.Owner == g && i.Send == send);

				if (re != null) {
					Packets.Remove(re);
					return re;
				}

				return null;
			}
		}

		public static bool IsAvailable(Packet p) {
			lock (_Locker) { return Packets.Any(i => i.GetType() == p.GetType()); }
		}

		public void Tick() {/*
			new Thread(() => {
				Thread.CurrentThread.IsBackground = true; 
				lock (_Locker) {
					foreach (ConnectionWrapper i in _cw.ToArray())
						try {
							TickNetwork(i);
							SendPackets(i);
						} catch (Exception) { _cw.Remove(i); }
				}
			}).Start();*/
			lock (_Locker) {
				foreach (ConnectionWrapper i in _cw.ToArray())
					try {
						TickNetwork(i);
						SendPackets(i);
					} catch (Exception) { _cw.Remove(i); }
			}
		}

		public void SendPackets(ConnectionWrapper i) {
			
			new Thread(() => {
				Thread.CurrentThread.IsBackground = true; 
				while (true) {
					Packet send = GetPacket(i.OwnerID, true);
					if (send != null) {
						i.Send(send);
						if (send.KillSwitch) {
							_cw.Remove(i);
							LoginService.Disconnected(i.OwnerID);
						}
					} else { break; }
				}
			}).Start();
			/*
			while (true) {
				Packet send = GetPacket(i.OwnerID, true);
				if (send != null) {
					i.Send(send);
					if (send.KillSwitch) {
						_cw.Remove(i);
						LoginService.Disconnected(i.OwnerID);
					}
				} else { break; }
			}
			*/
		}

		public int ReadVarInt(NetworkStream ns) {
			int value = 0;
			int size = 0;
			int b;
			while (((b = ns.ReadByte()) & 0x80) == 0x80) {
				value |= (b & 0x7F) << (size++ * 7);
				if (size > 5) throw new IOException("raise the shields intruder alert!");
			}

			return value | ((b & 0x7F) << (size * 7));
		}

		public void TickNetwork(ConnectionWrapper i) {
			try {
				if (!i._Client.Connected) {
					_cw.Remove(i);
				} else {
					if (!i._ns.DataAvailable) return;
					byte[] buffer = new byte[ReadVarInt(i._ns)];

					int bytesread = i._ns.Read(buffer, 0, buffer.Length);
					Array.Resize(ref buffer, bytesread); //resize just to be on the safe side
					Packet pp = null;

					if (i.LoggedIn) {
						MinecraftStream ms = new MinecraftStream(buffer);
						int id = ms.ReadVarInt();
						if (id == 0) { } else { pp = Packet.GetPacket(buffer, i.State); }
					} else { pp = Packet.GetPacket(buffer, i.State); }


					if (i.LoggedIn) { }

					if (pp is LoginStart) i.LoggedIn = true;

					if (pp != null) {
						pp.Owner = i.OwnerID;
						EnqueuePacket(pp);
					}
				}
			} catch (Exception ee) {
				Console.WriteLine(ee);
				_cw.Remove(i);
				LoginService.Disconnected(i.OwnerID);
			}
		}

		public void Start() {
			Run = true;

			ThreadPool.QueueUserWorkItem((c) => {
				TcpListener tl = new TcpListener(IPAddress.Any, 25565);
				tl.Start();

				while (Run) {
					ConnectionWrapper cq = new ConnectionWrapper(tl.AcceptTcpClient());
					lock (_Locker) { _cw.Add(cq); }
				}
			});
		}

		public void Stop() {
			Run = false;
		}
	}

	public class ConnectionWrapper {
		public TcpClient _Client { get; set; }
		public NetworkStream _ns { get; set; }
		public Guid OwnerID { get; set; } = Guid.NewGuid();
		public int State { get; set; } = 0;
		public bool LoggedIn { get; set; } = false;

		public ConnectionWrapper(TcpClient c) {
			_Client = c;
			_ns = _Client.GetStream();
		}

		public void Send(Packet p) {
			p.Flush(_ns);
		}
	}
}