using MyvarCraft.Core.Objects.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Utils
{
    public class ChunkStream
    {
        public List<byte> RawBuffer { get; set; } = new List<byte>();
        public int Offset { get; set; }

        public void WriteByte(byte b)
        {
            RawBuffer.Add(b);
            Offset++;
        }

        public void WriteHeader(Chunck c)
        {
            WriteByte(13);//bpb
            WriteVarInt(0);//we are using global pallet
            
        }

        public void WriteBlockData(Chunck c)
        {
            BitArray bits = new BitArray((16 * 16 * 16 * 13));
            int off = 0;
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        var b = c.GetBlock(x, y, z);
                        var id = new BitArray(BitConverter.GetBytes(b.ID));
                        for (int i = 0; i < 9; i++)
                        {
                            bits.Set(off, id[i]);
                            off++;
                        }

                        var damage = new BitArray(BitConverter.GetBytes(b.Damage));
                        for (int i = 0; i < 4; i++)
                        {
                            bits.Set(off, damage[i]);
                            off++;
                        }
                    }
                }
            }

            byte[] tmp = new byte[(bits.Length + 7) / 8];
            bits.CopyTo(tmp, 0);

            WriteVarInt((tmp.Count() / 8));//data size

            int cout = 0;
            List<byte> tmpbuf = new List<byte>();
            foreach(var i in tmp)
            {
                tmpbuf.Add(i);
                cout++;
                if(cout == 8)
                {
                    WriteLong((long)BitConverter.ToInt64(tmpbuf.ToArray(), 0));
                    tmpbuf.Clear();
                    cout = 0;
                }
            }


            List<byte> tmpbuflight = new List<byte>();
            for (int i = 0; i < (16 * 16 * 16) / 2; i++)
            {
                tmpbuflight.Add(0XFF);
            }

            RawBuffer.AddRange(tmpbuflight);//blocklight
            RawBuffer.AddRange(tmpbuflight);//skylight
        }

        public void WriteLong(long value)
        {
            RawBuffer.AddRange(BitConverter.GetBytes(value));
        }

        public void WriteVarInt(int _value)
        {
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
    }
}
