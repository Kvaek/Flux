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

		public override void Flush(NetworkStream ns) {
			MinecraftStream Write = new MinecraftStream();
			Write.WriteLong(Payload);
			byte[] buf = Write.Flush(ID);
			ns.Write(buf, 0, buf.Length);
		}
	}
}