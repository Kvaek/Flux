using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Internals
{
    public class MinecraftStream
    {
        internal List<byte> _buffer = new List<byte>();
        internal int _offset = 0;

        internal byte ReadByte(byte[] buffer)
        {
            var b = buffer[_offset];
            _offset += 1;
            return b;
        }

        internal byte[] Read(byte[] buffer, int length)
        {
            var data = new byte[length];
            Array.Copy(buffer, _offset, data, 0, length);
            _offset += length;
            return data;
        }

        internal int ReadVarInt(byte[] buffer)
        {
            var value = 0;
            var size = 0;
            int b;
            while (((b = ReadByte(buffer)) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("This VarInt is an imposter!");
                }
            }
            return value | ((b & 0x7F) << (size * 7));
        }


        internal long ReadLong(byte[] buffer)
        {
            byte[] b = new byte[8];
            b[0] = ReadByte(buffer);
            b[1] = ReadByte(buffer);
            b[2] = ReadByte(buffer);
            b[3] = ReadByte(buffer);

            b[4] = ReadByte(buffer);
            b[5] = ReadByte(buffer);
            b[6] = ReadByte(buffer);
            b[7] = ReadByte(buffer);

            return BitConverter.ToInt64(b, 0);
        }

        internal short ReadShort(byte[] buffer)
        {
            byte[] b = new byte[2];
            b[0] = ReadByte(buffer);
            b[1] = ReadByte(buffer);
            return BitConverter.ToInt16(b, 0);
        }

        internal float ReadFloat(byte[] buffer)
        {
            byte[] b = new byte[4];
            b[0] = ReadByte(buffer);
            b[1] = ReadByte(buffer);
            b[2] = ReadByte(buffer);
            b[3] = ReadByte(buffer);
            return BitConverter.ToSingle(b, 0);
        }

        internal ushort ReadUShort(byte[] buffer)
        {
            byte[] b = new byte[2];
            b[0] = ReadByte(buffer);
            b[1] = ReadByte(buffer);
            return BitConverter.ToUInt16(b, 0);
        }

        internal string ReadString(byte[] buffer, int length)
        {
            var data = Read(buffer, length);
            return Encoding.UTF8.GetString(data);
        }

        internal void WriteVarInt(int _value)
        {
            /*   while ((value & 128) != 0)
               {
                   _buffer.Add((byte)(value & 127 | 128));
                   value = (int)((uint)value) >> 7;
               }
               _buffer.Add((byte)value);*/
            uint value = (uint)_value;
            while (true)
            {
                if ((value & 0xFFFFFF80u) == 0)
                {
                    WriteByte((byte)value);
                    break;
                }
                WriteByte((byte)(value & 0x7F | 0x80));
                value >>= 7;
            }

        }

        public static byte[] ToByteArray(BitArray bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0) numBytes++;

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }

        public static BitArray BitsReverse(BitArray bits)
        {
            int len = bits.Count;
            BitArray a = new BitArray(bits);
            BitArray b = new BitArray(bits);

            for (int i = 0, j = len - 1; i < len; ++i, --j)
            {
                a[i] = a[i] ^ b[j];
                b[j] = a[i] ^ b[j];
                a[i] = a[i] ^ b[j];
            }

            return a;
        }
        internal void WriteShort(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        internal void WriteUShort(ushort value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        internal void WriteInt(int value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        internal void WriteFloat(float value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public ulong ReadUInt64(byte[] b)
        {
            return unchecked(
                   ((ulong)ReadByte(b) << 56) |
                   ((ulong)ReadByte(b) << 48) |
                   ((ulong)ReadByte(b) << 40) |
                   ((ulong)ReadByte(b) << 32) |
                   ((ulong)ReadByte(b) << 24) |
                   ((ulong)ReadByte(b) << 16) |
                   ((ulong)ReadByte(b) << 8) |
                    (ulong)ReadByte(b));
        }

        public uint ReadUInt32(byte[] b)
        {
            return (uint)(
                (ReadByte(b) << 24) |
                (ReadByte(b) << 16) |
                (ReadByte(b) << 8) |
                 ReadByte(b));
        }

        public unsafe float ReadSingle(byte[] b)
        {
            uint value = ReadUInt32(b);
            return *(float*)&value;
        }

        public unsafe double ReadDouble(byte[] b)
        {
            ulong value = ReadUInt64(b);
            return *(double*)&value;
        }

        internal void WriteSingle(Single value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }



        internal unsafe void WriteDouble(double value)
        {
            WriteUInt64(*(ulong*)&value);
        }

        public void WriteUInt64(ulong value)
        {
            _buffer.Add((byte)((value & 0xFF00000000000000) >> 56));
            _buffer.Add((byte)((value & 0xFF000000000000) >> 48));
            _buffer.Add((byte)((value & 0xFF0000000000) >> 40));
            _buffer.Add((byte)((value & 0xFF00000000) >> 32));
            _buffer.Add((byte)((value & 0xFF000000) >> 24));
            _buffer.Add((byte)((value & 0xFF0000) >> 16));
            _buffer.Add((byte)((value & 0xFF00) >> 8));
            _buffer.Add((byte)(value & 0xFF));

        }

        internal void WriteInt64(Int64 value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        internal void WriteByte(byte value)
        {
            _buffer.Add(BitConverter.GetBytes(value)[0]);
        }
        internal void WriteSByte(sbyte value)
        {
            _buffer.Add(BitConverter.GetBytes(value)[0]);
        }

        internal void WriteLong(long value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        internal void WriteString(string data, bool length = true)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            if (length)
            {
                WriteVarInt(buffer.Length);
            }
            _buffer.AddRange(buffer);
        }


        internal byte[] Flush(int id = -1)
        {
            var buffer = _buffer.ToArray();
            _buffer.Clear();

            var add = 0;
            var packetData = new[] { (byte)0x00 };
            if (id >= 0)
            {
                WriteVarInt(id);
                packetData = _buffer.ToArray();
                add = packetData.Length;
                _buffer.Clear();
            }

            WriteVarInt(buffer.Length + add);
            var bufferLength = _buffer.ToArray();
            _buffer.Clear();
            byte[] rv = new byte[bufferLength.Length + packetData.Length + buffer.Length];
            System.Buffer.BlockCopy(bufferLength, 0, rv, 0, bufferLength.Length);
            System.Buffer.BlockCopy(packetData, 0, rv, bufferLength.Length, packetData.Length);
            System.Buffer.BlockCopy(buffer, 0, rv, bufferLength.Length + packetData.Length, buffer.Length);
            return rv;
        }
    }
}
