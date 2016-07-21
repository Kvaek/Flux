using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects
{
    public class FlexibleStorage
    {
        private long[] Data { get; set; }
        private int BitsPerEntry { get; set; }
        private int Size { get; set; }
        private long MaxEntryValue { get; set; }

        public FlexibleStorage(int bitsPerEntry, int size)
            : this(bitsPerEntry, new long[RoundToNearest(size * bitsPerEntry, 64) / 64])
        {

        }

        public FlexibleStorage(int bitsPerEntry, long[] data)
        {
            if (bitsPerEntry < 1 || bitsPerEntry > 32)
            {
                throw new ArgumentException("BitsPerEntry cannot be outside of accepted range.");
            }

            BitsPerEntry = bitsPerEntry;
            Data = data;

            Size = this.Data.Length * 64 / this.BitsPerEntry;
            MaxEntryValue = (1L << this.BitsPerEntry) - 1;
        }

        public long[] GetData()
        {
            return Data.ToArray();
        }

        public int GetBitsPerEntry()
        {
            return BitsPerEntry;
        }

        public int GetSize()
        {
            return Size;
        }

        public int Get(int index)
        {
            if (index < 0 || index > Size - 1)
            {
                throw new IndexOutOfRangeException();
            }

            int bitIndex = index * BitsPerEntry;
            int startIndex = bitIndex / 64;
            int endIndex = ((index + 1) * BitsPerEntry - 1) / 64;
            int startBitSubIndex = bitIndex % 64;
            if (startIndex == endIndex)
            {
                return (int)(Data[startIndex] >> startBitSubIndex & MaxEntryValue);
            }
            else
            {
                int endBitSubIndex = 64 - startBitSubIndex;
                return
                    (int)
                        ((Data[startIndex] >> startBitSubIndex | Data[endIndex] << endBitSubIndex) &
                         MaxEntryValue);
            }
        }

        public void Set(int index, int value)
        {
            if (index < 0 || index > Size - 1)
            {
                throw new IndexOutOfRangeException();
            }

            if (value < 0 || value > MaxEntryValue)
            {
                throw new IndexOutOfRangeException();
            }

            int bitIndex = index * BitsPerEntry;
            int startIndex = bitIndex / 64;
            int endIndex = ((index + 1) * BitsPerEntry - 1) / 64;
            int startBitSubIndex = bitIndex % 64;
            this.Data[startIndex] = this.Data[startIndex] & ~(this.MaxEntryValue << startBitSubIndex) |
                                    ((long)value & this.MaxEntryValue) << startBitSubIndex;
            if (startIndex != endIndex)
            {
                int endBitSubIndex = 64 - startBitSubIndex;
                this.Data[endIndex] = this.Data[endIndex] >> endBitSubIndex << endBitSubIndex |
                                      ((long)value & this.MaxEntryValue) >> endBitSubIndex;
            }
        }

        private static int RoundToNearest(int value, int roundTo)
        {
            if (roundTo == 0)
            {
                return 0;
            }
            else if (value == 0)
            {
                return roundTo;
            }
            else
            {
                if (value < 0)
                {
                    roundTo *= -1;
                }

                int remainder = value % roundTo;
                return remainder != 0 ? value + roundTo - remainder : value;
            }
        }
    }
}
