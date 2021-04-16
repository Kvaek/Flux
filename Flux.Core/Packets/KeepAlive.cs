using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Packets {
	public class KeepAlive : Packet {
		public int KeepAliveID { get; set; }

		public KeepAlive() {
			ID = 0x1F;
			Send = true;
		}


		public override void Write(MinecraftStream ms) {
			ms.WriteLong(KeepAliveID);
			ms.Flush(ID);
		}
	}
}