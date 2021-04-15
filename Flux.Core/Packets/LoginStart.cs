using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Packets {
	public class LoginStart : Packet {
		public string Name { get; set; }

		public LoginStart() {
			IDs.Add(-1);
			IDs.Add(-1);
			IDs.Add(0);
		}

		public override Packet Read(byte[] data) {
			MinecraftStream ms = new MinecraftStream(data);
			LoginStart re = new LoginStart() { ID = ms.ReadVarInt() };
			int l = ms.ReadVarInt();
			re.Name = ms.ReadString(l);
			return re;
		}
	}
}