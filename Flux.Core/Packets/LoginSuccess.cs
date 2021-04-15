using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Packets {
	public class LoginSuccess : Packet {
		public string UUID { get; set; }
		public string Username { get; set; }

		public LoginSuccess() {
			ID = 0x02;
			Send = true;
		}

		public override void Flush(NetworkStream ns) {
			MinecraftStream read = new MinecraftStream();
			read.WriteString(UUID);
			read.WriteString(Username);
			byte[] buf = read.Flush(ID);
			ns.Write(buf, 0, buf.Length);
		}
	}
}