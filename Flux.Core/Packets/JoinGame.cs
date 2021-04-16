using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Packets {
	public class JoinGame : Packet {
		public int EntityID { get; set; }
		public byte Gamemode { get; set; }
		public int Dimension { get; set; }
		public byte Difficulty { get; set; }
		public byte MaxPlayers { get; set; }
		public string LevelType { get; set; }
		public sbyte ReducedDebugInfo { get; set; }

		public JoinGame() {
			ID = 0x23;
			Send = true;
		}

		public override void Write(MinecraftStream ms) {

			ms.WriteInt(EntityID);
			ms.WriteByte(Gamemode);
			ms.WriteInt(Dimension);
			ms.WriteByte(Difficulty);
			ms.WriteByte(MaxPlayers);
			ms.WriteString(LevelType);
			ms.WriteSByte(ReducedDebugInfo);

			ms.Flush(ID);
		}
	}
}