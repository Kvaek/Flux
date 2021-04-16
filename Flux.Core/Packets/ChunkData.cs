using Flux.Core.Objects.Meta;
using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Packets {
	public class ChunkData : Packet {
		public int X { get; set; }
		public int Y { get; set; }
		public byte GroundUp { get; set; } = 1;
		public int BitMask { get; set; } = 1;
		public Chunk Data { get; set; }

		public ChunkData() {
			ID = 0x20;
			Send = true;
		}

		public override void Write(MinecraftStream ms) {

			ms.WriteInt(X);
			ms.WriteInt(Y);
			ms.WriteByte(GroundUp);
			ms.WriteVarInt(BitMask);


			byte[] bufc = Data.RawData.Write();

			ms.WriteVarInt(bufc.Length + 256);

			foreach (byte i in bufc) ms.WriteByte(i);

			for (int i = 0; i < 256; i++) ms.WriteByte(1);
			ms.WriteVarInt(0);
			
			ms.Flush(ID);
		}
	}
}