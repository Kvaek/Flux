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

		public override void Write(MinecraftStream ms) {
			ms.WriteString(UUID);
			ms.WriteString(Username);
			
			ms.Flush(ID);
		}
	}
}