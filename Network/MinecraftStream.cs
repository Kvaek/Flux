using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Flux.Network {
	public class MinecraftStream {
		private readonly List<byte> _buffer = new List<byte>();
		private int _offset;

		public byte[] Buffer {
			get => _buffer.ToArray();
			set => Buffer = value;
		}

		public MinecraftStream() {
		}
		public MinecraftStream(byte[] data) {
			Buffer = data;
		}

		public void Flush(int id) {
			byte[] _data = Buffer;
			_buffer.Clear();
			try {
				WriteVarInt(id);
				
			} catch (IOException ex) {
				Console.WriteLine(ex);
			}
		}

		public byte[] Read(int length) {
			byte[] data = new byte[length];
			Array.Copy(Buffer, _offset, data, 0, length);
			_offset += length;
			return data;
		}

		public byte ReadByte() {
			byte b = Buffer[_offset];
			_offset += 1;
			return b;
		}
		public int ReadVarInt() {
			int numRead = 0;
			int result = 0;
			int read;

			while (((read = ReadByte()) & 0x80) == 0x80) {
				result |= (read & 0x7F) << (numRead++ * 7);
				if (numRead > 5) {
					throw new IOException("VarInt is too big");
				}
			}
			return result | ((read & 0x7F) << (numRead * 7));
		}

		public void WriteVarInt(int value) {
			while ((value & -0x80) != 0) {
				_buffer.Add((byte) (value & 0x7F | 0x80));
				value >>= 7;
			}
		}

		public void Write(byte[] data) {
			foreach (byte b in data) {
				_buffer.Add(b);
			}
		}

		public void Write(byte[] data, int offset, int length) {
			for (int i = 0; i < length; i++) {
				_buffer.Add(data[i + offset]);
			}
		}
	}
}