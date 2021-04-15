using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Packets {
	public class HandShake : Packet {
		public int ProtocolVersion { get; set; }
		public string ServerAddress { get; set; }
		public int ServerPort { get; set; }
		public int NextState { get; set; }

		public HandShake() {
			IDs.Add(0);
			IDs.Add(-1);
			IDs.Add(-1);
		}

		public override Packet Read(byte[] data) {
			HandShake re = new HandShake();

			MinecraftStream ms = new MinecraftStream(data);
			re.ID = ms.ReadVarInt();

			re.ProtocolVersion = ms.ReadVarInt();

			int l = ms.ReadVarInt();
			re.ServerAddress = ms.ReadString(l);

			re.ServerPort = ms.ReadUShort();

			re.NextState = ms.ReadVarInt();

			return re;
		}
	}
}