using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Utils
{
    public class MinecraftStream
    {

        public List<byte> _buffer = new List<byte>();
        public int _offset = 0;

        private byte[] buffer { get; set; }

        public MinecraftStream()
        {

        }

        public MinecraftStream(byte[] abuffer)
        {
            buffer = abuffer;
        }

        public byte ReadByte()
        {
            var b = buffer[_offset];
            _offset += 1;
            return b;
        }

        public byte[] Read(int length)
        {
            var data = new byte[length];
            Array.Copy(buffer, _offset, data, 0, length);
            _offset += length;
            return data;
        }

        public int ReadVarInt()
        {
            var value = 0;
            var size = 0;
            int b;
            while (((b = ReadByte()) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("This VarInt is an imposter!");
                }
            }
            return value | ((b & 0x7F) << (size * 7));
        }


        public long ReadLong()
        {
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

        public short ReadShort()
        {
            byte[] b = new byte[2];
            b[0] = ReadByte();
            b[1] = ReadByte();
            return BitConverter.ToInt16(b, 0);
        }

        public float ReadFloat()
        {
            byte[] b = new byte[4];
            b[0] = ReadByte();
            b[1] = ReadByte();
            b[2] = ReadByte();
            b[3] = ReadByte();
            return BitConverter.ToSingle(b, 0);
        }

        public ushort ReadUShort()
        {
            byte[] b = new byte[2];
            b[0] = ReadByte();
            b[1] = ReadByte();
            return BitConverter.ToUInt16(b, 0);
        }

        public string ReadString(int length)
        {
            var data = Read(length);
            return Encoding.UTF8.GetString(data);
        }

        public void WriteVarInt(int _value)
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
        public void WriteShort(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void WriteUShort(ushort value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void WriteInt(int value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void WriteFloat(float value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public ulong ReadUInt64()
        {
            return unchecked(
                   ((ulong)ReadByte() << 56) |
                   ((ulong)ReadByte() << 48) |
                   ((ulong)ReadByte() << 40) |
                   ((ulong)ReadByte() << 32) |
                   ((ulong)ReadByte() << 24) |
                   ((ulong)ReadByte() << 16) |
                   ((ulong)ReadByte() << 8) |
                    (ulong)ReadByte());
        }

        public uint ReadUInt32()
        {
            return (uint)(
                (ReadByte() << 24) |
                (ReadByte() << 16) |
                (ReadByte() << 8) |
                 ReadByte());
        }

        public unsafe float ReadSingle()
        {
            uint value = ReadUInt32();
            return *(float*)&value;
        }

        public unsafe double ReadDouble()
        {
            ulong value = ReadUInt64();
            return *(double*)&value;
        }

        public void WriteSingle(Single value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }



        public unsafe void WriteDouble(double value)
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

        public void WriteInt64(Int64 value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void WriteByte(byte value)
        {
            _buffer.Add(BitConverter.GetBytes(value)[0]);
        }
        public void WriteSByte(sbyte value)
        {
            _buffer.Add(BitConverter.GetBytes(value)[0]);
        }

        public void WriteLong(long value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void WriteString(string data, bool length = true)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            if (length)
            {
                WriteVarInt(buffer.Length);
            }
            _buffer.AddRange(buffer);
        }


        public byte[] Flush(int id = -1)
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
