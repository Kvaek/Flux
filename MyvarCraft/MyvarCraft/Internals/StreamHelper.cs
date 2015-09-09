using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals
{
    internal class StreamHelper
    {
        #region Read/Write methods

        internal List<byte> _buffer = new List<byte>();
        internal int _offset = 0;

        internal  byte ReadByte(byte[] buffer)
        {
            var b = buffer[_offset];
            _offset += 1;
            return b;
        }

        internal  byte[] Read(byte[] buffer, int length)
        {
            var data = new byte[length];
            Array.Copy(buffer, _offset, data, 0, length);
            _offset += length;
            return data;
        }

        internal  int ReadVarInt(byte[] buffer)
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

       

        internal ushort ReadUShort(byte[] buffer)
        {
            byte[] b = new byte[2];
            b[0] = ReadByte(buffer);
            b[1] = ReadByte(buffer);
            return BitConverter.ToUInt16(b, 0);
        }

        internal  string ReadString(byte[] buffer, int length)
        {
            var data = Read(buffer, length);
            return Encoding.UTF8.GetString(data);
        }

        internal  void WriteVarInt(int value)
        {
            while ((value & 128) != 0)
            {
                _buffer.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            _buffer.Add((byte)value);
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

        internal void WriteDouble(double value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
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

        internal  void WriteString(string data)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            WriteVarInt(buffer.Length);
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
        #endregion
    }
}
