using System;

namespace Flux.Network.Packets {
	internal class KeepAlive : Packet<KeepAlive> {
		public KeepAlive(byte[] data) : base(data) {
			ID = 0x0F;
		}

		public override void Write() {
			MinecraftStream ms = new MinecraftStream();
			ms.WriteVarInt(new Random().Next(0, 100));
			var buf = ms.Flush(ID);
			ms.Write(buf, 0, buf.Length);
		}
	}
}