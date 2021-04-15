using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Core.Utils {
	public class MinecraftStream {
		public List<byte> _buffer = new List<byte>();
		public int _offset = 0;

		private byte[] buffer { get; set; }

		public MinecraftStream() { }

		public MinecraftStream(byte[] abuffer) => buffer = abuffer;

		public byte ReadByte() {
			byte b = buffer[_offset];
			_offset += 1;
			return b;
		}

		public byte[] Read(int length) {
			byte[] data = new byte[length];
			Array.Copy(buffer, _offset, data, 0, length);
			_offset += length;
			return data;
		}

		public int ReadVarInt() {
			int value = 0;
			int size = 0;
			int b;
			while (((b = ReadByte()) & 0x80) == 0x80) {
				value |= (b & 0x7F) << (size++ * 7);
				if (size > 5) throw new IOException("VarInt too big");
			}

			return value | ((b & 0x7F) << (size * 7));
		}


		public long ReadLong() {
			byte[] b = new byte[8];
			b[0] = ReadByte();
			b[1] = ReadByte();
			b[2] = ReadByte();
			b[3] = ReadByte();

			b[4] = ReadByte();
			b[5] = ReadByte();
			b[6] = ReadByte();
			b[7] = ReadByte();

			return BitConverter.ToInt64(b, 0);
		}

		public short ReadShort() {
			byte[] b = new byte[2];
			b[0] = ReadByte();
			b[1] = ReadByte();
			return BitConverter.ToInt16(b, 0);
		}

		public float ReadFloat() {
			byte[] b = new byte[4];
			b[0] = ReadByte();
			b[1] = ReadByte();
			b[2] = ReadByte();
			b[3] = ReadByte();
			return BitConverter.ToSingle(b, 0);
		}

		public ushort ReadUShort() {
			byte[] b = new byte[2];
			b[0] = ReadByte();
			b[1] = ReadByte();
			return BitConverter.ToUInt16(b, 0);
		}

		public string ReadString(int length) {
			byte[] data = Read(length);
			return Encoding.UTF8.GetString(data);
		}

		/*public void WriteVarInt(int _value) {
			uint value = (uint) _value;
			while (true) {
				if ((value & 0xFFFFFF80u) == 0) {
					WriteByte((byte) value);
					break;
				}

				WriteByte((byte) ((value & 0x7F) | 0x80));
				value >>= 7;
			}
		}*/
		public void WriteVarInt(int paramInt) {
			while (true) {
				if ((paramInt & 0xFFFFFF80) == 0) {
					WriteByte((byte) paramInt);
					return;
				}

				WriteByte((byte) (paramInt & 0x7F | 0x80));
				paramInt >>= 7;
			}
		}

		public byte[] ToByteArray(BitArray bits) {
			int numBytes = bits.Count / 8;
			if (bits.Count % 8 != 0) numBytes++;

			byte[] bytes = new byte[numBytes];
			int byteIndex = 0, bitIndex = 0;

			for (int i = 0; i < bits.Count; i++) {
				if (bits[i]) bytes[byteIndex] |= (byte) (1 << (7 - bitIndex));

				bitIndex++;
				if (bitIndex == 8) {
					bitIndex = 0;
					byteIndex++;
				}
			}

			return bytes;
		}

		public BitArray BitsReverse(BitArray bits) {
			int len = bits.Count;
			BitArray a = new BitArray(bits);
			BitArray b = new BitArray(bits);

			for (int i = 0, j = len - 1; i < len; ++i, --j) {
				a[i] = a[i] ^ b[j];
				b[j] = a[i] ^ b[j];
				a[i] = a[i] ^ b[j];
			}

			return a;
		}

		public void WriteShort(short value) {
			_buffer.AddRange(BitConverter.GetBytes(value));
		}

		public void WriteUShort(ushort value) {
			_buffer.AddRange(BitConverter.GetBytes(value));
		}

		public void WriteInt(int value) {
			_buffer.AddRange(BitConverter.GetBytes(value));
		}

		public void WriteFloat(float value) {
			_buffer.AddRange(BitConverter.GetBytes(value));
		}

		public ulong ReadUInt64() =>
			unchecked(((ulong) ReadByte() << 56) |
			          ((ulong) ReadByte() << 48) |
			          ((ulong) ReadByte() << 40) |
			          ((ulong) ReadByte() << 32) |
			          ((ulong) ReadByte() << 24) |
			          ((ulong) ReadByte() << 16) |
			          ((ulong) ReadByte() << 8) |
			          ReadByte());

		public uint ReadUInt32() =>
			(uint) (
				(ReadByte() << 24) |
				(ReadByte() << 16) |
				(ReadByte() << 8) |
				ReadByte());

		public unsafe float ReadSingle() {
			uint value = ReadUInt32();
			return *(float*) &value;
		}

		public unsafe double ReadDouble() {
			ulong value = ReadUInt64();
			return *(double*) &value;
		}

		public void WriteSingle(float value) {
			_buffer.AddRange(BitConverter.GetBytes(value));
		}


		public unsafe void WriteDouble(double value) {
			WriteUInt64(*(ulong*) &value);
		}

		public void WriteUInt64(ulong value) {
			_buffer.Add((byte) ((value & 0xFF00000000000000) >> 56));
			_buffer.Add((byte) ((value & 0xFF000000000000) >> 48));
			_buffer.Add((byte) ((value & 0xFF0000000000) >> 40));
			_buffer.Add((byte) ((value & 0xFF00000000) >> 32));
			_buffer.Add((byte) ((value & 0xFF000000) >> 24));
			_buffer.Add((byte) ((value & 0xFF0000) >> 16));
			_buffer.Add((byte) ((value & 0xFF00) >> 8));
			_buffer.Add((byte) (value & 0xFF));
		}

		public void WriteInt64(long value) {
			_buffer.AddRange(BitConverter.GetBytes(value));
		}

		public void WriteByte(byte value) {
			_buffer.Add(BitConverter.GetBytes(value)[0]);
		}

		public void WriteSByte(sbyte value) {
			_buffer.Add(BitConverter.GetBytes(value)[0]);
		}

		public void WriteLong(long value) {
			_buffer.AddRange(BitConverter.GetBytes(value));
		}

		public void WriteString(string data, bool length = true) {
			byte[] abuffer = Encoding.UTF8.GetBytes(data);
			if (length) WriteVarInt(abuffer.Length);
			_buffer.AddRange(abuffer);
		}


		public byte[] Flush(int id = -1) {
			byte[] abuffer = _buffer.ToArray();
			_buffer.Clear();

			int add = 0;
			byte[] packetData = new[] { (byte) 0x00 };
			if (id >= 0) {
				WriteVarInt(id);
				packetData = _buffer.ToArray();
				add = packetData.Length;
				_buffer.Clear();
			}

			WriteVarInt(abuffer.Length + add);
			byte[] bufferLength = _buffer.ToArray();
			_buffer.Clear();
			byte[] rv = new byte[bufferLength.Length + packetData.Length + abuffer.Length];
			Buffer.BlockCopy(bufferLength, 0, rv, 0, bufferLength.Length);
			Buffer.BlockCopy(packetData, 0, rv, bufferLength.Length, packetData.Length);
			Buffer.BlockCopy(abuffer, 0, rv, bufferLength.Length + packetData.Length, abuffer.Length);
			return rv;
		}
	}
}