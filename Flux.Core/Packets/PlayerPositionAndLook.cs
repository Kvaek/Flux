using Flux.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Packets {
	public class PlayerPositionAndLook : Packet {
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public byte Flags { get; set; } = 255;
		public int TeleportID { get; set; } = new Random().Next();

		public PlayerPositionAndLook() {
			IDs.Add(-1);
			IDs.Add(-1);
			IDs.Add(0x0C);

			ID = 0x2F;
			Send = true;
		}

		public override Packet Read(byte[] data) {
			PlayerPositionAndLook re = new PlayerPositionAndLook();
			MinecraftStream ms = new MinecraftStream(data);
			re.ID = ms.ReadVarInt();

			re.X = ms.ReadDouble();
			re.Y = ms.ReadDouble();
			re.Z = ms.ReadDouble();

			//re.Yaw = ms.ReadFloat(data);
			//  re.Pitch = ms.ReadFloat(data);

			// re.OnGround = ms.ReadByte(data);
			return re;
		}

		public override void Write(MinecraftStream ms) {
			ms.WriteDouble(X);
			ms.WriteDouble(Y);
			ms.WriteDouble(Z);
			ms.WriteFloat(Yaw);
			ms.WriteFloat(Pitch);
			ms.WriteByte(Flags);
			ms.WriteVarInt(TeleportID);

			ms.Flush(ID);
		}
	}
}