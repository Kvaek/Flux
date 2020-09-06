using System;
using System.IO;
using System.Net.Sockets;

namespace Flux.Network {
	public class MinecraftStream {
		public static int ReadVarInt(NetworkStream ns) {
			int numRead = 0;
			int result = 0;
			int read;

			while (((read = ns.ReadByte()) & 0x80) == 0x80) {
				result |= (read & 0x7F) << (numRead++ * 7);
				
				if (numRead > 5) {
					throw new IOException("VarInt is too big");
				}
			}
			return result | ((read & 0x7F) << (numRead * 7));
		}
		
		public static void WriteVarInt(int value) {
			while (value != 0) {
				byte temp = (byte)(value & 0x7F);
				// Note: >> means that the sign bit is shifted with the rest of the number rather than being left alone
				value >>= 7;
				if (value != 0) {
					temp |= 0x7F;
				}
				writeByte(temp);
			}
		}
	}
}