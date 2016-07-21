using MyvarCraft.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects
{
    public class BlockStorage
    {
        private int BitsPerEntry { get; set; }
        private List<int> States { get; set; }
        private FlexibleStorage Storage { get; set; }

        public BlockStorage()
        {
            BitsPerEntry = 4;
            States = new List<int> { 0 };

            Storage = new FlexibleStorage(BitsPerEntry, 4096);
        }

        public byte[] Write()
        {
            var stream = new MinecraftStream();
            stream.WriteByte((byte)BitsPerEntry);
            stream.WriteVarInt(States.Count);
            foreach (var state in States)
            {
                stream.WriteVarInt(state);
            }

            long[] data = Storage.GetData();
            stream.WriteVarInt(data.Length);

            foreach (var i in data)
            {
                stream.WriteLong(i);
            }

            for (int i = 0; i < (16 * 16 * 16) / 2; i++)
            {
                stream.WriteByte(255);
            }

            for (int i = 0; i < (16 * 16 * 16) / 2; i++)
            {
                stream.WriteByte(255);
            }

            return stream._buffer.ToArray();
        }

        public int GetBitsPerEntry()
        {
            return BitsPerEntry;
        }

        public int[] GetStates()
        {
            return States.ToArray();
        }

        public FlexibleStorage GetStorage()
        {
            return Storage;
        }

        public int Get(int x, int y, int z)
        {
            int id = Storage.Get(Index(x, y, z));
            return BitsPerEntry <= 8 ? (id >= 0 && id < States.Count ? States[id] : 0) : id;
        }

        public void Set(int x, int y, int z, int state)
        {
            int id = BitsPerEntry <= 8 ? States.IndexOf(state) : state;
            if (id == -1)
            {
                States.Add(state);
                if (States.Count > 1 << BitsPerEntry)
                {
                    BitsPerEntry++;

                    List<int> oldStates = States.ToList();
                    if (BitsPerEntry > 8)
                    {
                        oldStates = new List<int>(States);
                        States.Clear();
                        BitsPerEntry = 13;
                    }

                    FlexibleStorage oldStorage = Storage;
                    Storage = new FlexibleStorage(BitsPerEntry, Storage.GetSize());
                    for (int index = 0; index < Storage.GetSize(); index++)
                    {
                        int value = oldStorage.Get(index);
                        Storage.Set(index, BitsPerEntry <= 8 ? value : oldStates[value]);
                    }
                }

                id = BitsPerEntry <= 8 ? States.IndexOf(state) : state;
            }

            Storage.Set(Index(x, y, z), id);
        }

        public bool IsEmpty()
        {
            for (int index = 0; index < Storage.GetSize(); index++)
            {
                if (Storage.Get(index) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static int Index(int x, int y, int z)
        {
            return y << 8 | z << 4 | x;
        }
    }
}
