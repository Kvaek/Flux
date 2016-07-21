using MyvarCraft.Core.Objects.Meta;
using MyvarCraft.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Packets
{
    public class ChunckData : Packet
    {

        public int X { get; set; }
        public int Y { get; set; }
        public byte GroundUp { get; set; } = 1;
        public int BitMask { get; set; } = 1;
        public Chunk Data { get; set; }

        public ChunckData()
        {
            ID = 0x20;
            Send = true;
        }

        public override void Flush(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();

            read.WriteInt(X);
            read.WriteInt(Y);
            read.WriteByte(GroundUp);
            read.WriteVarInt(BitMask);

           

            var bufc = Data.RawData.Write();

            read.WriteVarInt(bufc.Length + 256);

            foreach (var i in bufc)
            {
                read.WriteByte(i);
            }

            for (int i = 0; i < 256; i++)
            {
                read.WriteByte(1);
            }
            read.WriteVarInt(0);
            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }
    }
}
