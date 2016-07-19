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

        public BitArray BitsReverse(BitArray bits)
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


      

        public void WriteBlockData(Chunck c)
        {
            BitArray bits = new BitArray((16 * 16 * 16 * 13));
            int offm = 0;
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        int off = 0;
                        var tmpbts = new BitArray(13);
                        var b = c.GetBlock(new Location() { X = x, Y = y, Z = z });
                                                   

                        var id = BitsReverse(new BitArray(BitConverter.GetBytes(b.ID).Reverse().ToArray()));
                        for (int i = 0; i < 9; i++)
                        {
                            tmpbts.Set(off, id[i]);
                            off++;
                        }

                        var damage = BitsReverse(new BitArray(BitConverter.GetBytes(b.Damage).Reverse().ToArray()));
                        for (int i = 0; i < 4; i++)
                        {
                            tmpbts.Set(off, damage[i]);
                            off++;
                        }

                        foreach (bool i in tmpbts)
                        {
                            bits[offm] = i;
                            offm++;
                        }
                    }
                }
            }

            byte[] tmp = new byte[DivideRoundingUp(bits.Count, 8)];
            bits.CopyTo(tmp, 0);

            WriteVarInt(DivideRoundingUp(tmp.Count(), 8));//data size

             int cout = 0;
             List<byte> tmpbuf = new List<byte>();
             foreach (var i in tmp)
             {
                 tmpbuf.Add(i);
                 cout++;
                 if (cout == 8)
                 {
                    tmpbuf.Reverse();
                    RawBuffer.AddRange(tmpbuf);
                    // WriteLong(BitConverter.ToInt64(tmpbuf.ToArray(), 0));
                     tmpbuf.Clear();
                     cout = 0;
                 }
             }

            
        /*    unsafe
            {
                fixed (byte* pBuffer = tmp)
                {
                    Int64* pSample = (long*)pBuffer;
                    for (int i = 0; i < DivideRoundingUp(bits.Count, 8); i++)
                    {
                        var along = pSample[i];
                        WriteLong(along);
                    }
                    
                }
            }*/

           //   RawBuffer.AddRange(tmp);

            List<byte> tmpbuflight = new List<byte>();
            for (int i = 0; i < (16 * 16 * 16) / 2; i++)
            {
                tmpbuflight.Add(0XFF);
            }

            RawBuffer.AddRange(tmpbuflight);//blocklight
            RawBuffer.AddRange(tmpbuflight);//skylight
        }

        public static int DivideRoundingUp(int x, int y)
        {
            // TODO: Define behaviour for negative numbers
            int remainder;
            int quotient = Math.DivRem(x, y, out remainder);
            return remainder == 0 ? quotient : quotient + 1;
        }

        public void WriteLong(long value)
        {
            var bytes = BitConverter.GetBytes(value);
   
            Array.Reverse(bytes);
            
            RawBuffer.AddRange(bytes);
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
