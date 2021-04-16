using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Packets {
	public class Pong : Packet {
		public long Payload { get; set; }

		public Pong() {
			ID = 0x01;
			Send = true;
		}

		public override void Write(MinecraftStream ms) {
			ms.WriteLong(Payload);
			
			ms.Flush(ID);

		}
	}
}