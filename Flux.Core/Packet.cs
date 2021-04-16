using Flux.Core.Packets;
using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core {
	public class Packet {/*
		public static List<Packet> Packets = typeof(Packet)
			.Assembly.GetTypes()
			.Where(c => c.IsSubclassOf(typeof(Packet)) && !c.IsAbstract)
			.Select(c => (Packet) Activator.CreateInstance(c))
			.ToList();
			*/
		public List<int> IDs { get; set; } = new List<int>();
		public int ID { get; set; }
		public Guid Owner { get; set; }
		public bool Send { get; set; }
		public bool KillSwitch { get; set; }

		public Packet() { }

		public Packet(byte[] data) {
			Read(data);
		}

		public virtual void Write(MinecraftStream ms) {
			//ms.Flush();
			ms.Ns.Write(new byte[] { 0 }, 0, 1);
		}

		public virtual Packet Read(byte[] data) {
			Packet re = new Packet();
			MinecraftStream ms = new MinecraftStream(data);
			re.ID = ms.ReadVarInt();
			return re;
		}


		private static List<Packet> _packets { get; set; } =
			new List<Packet> { new Ping(), new HandShake(), new Request(), new LoginStart() };

		public static Packet GetPacket(byte[] raw, int state) {
			MinecraftStream ms = new MinecraftStream(raw);
			int id = ms.ReadVarInt();

			foreach (Packet i in _packets)
				if (i.IDs[state] == id)
					return i.Read(raw);
			return null;
		}
	}
}