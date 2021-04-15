using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Packets {
	public class Response : Packet {
		public string Json { get; set; }

		public Response() {
			ID = 0;
			Send = true;
		}

		public override void Flush(NetworkStream ns) {
			MinecraftStream read = new MinecraftStream();
			read.WriteString(Json);
			byte[] buf = read.Flush(ID);
			ns.Write(buf, 0, buf.Length);
		}
	}
}