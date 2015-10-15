using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class ChunkData : Packet
    {
        public int X { get; set; }
        public int Y { get; set; }
        public byte GroundUpContinuous { get; set; }
        public int PrimaryBitMask { get; set; }
        public int Size { get; set; }
        public byte[] Data { get; set; }



        public ChunkData()
        {
            ID = 0x20;
        }

        public override void Write(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();
            read.WriteInt(X);
            read.WriteInt(Y);
            read.WriteByte(1);
            read.WriteVarInt(0Xfffffff);

            read.WriteVarInt(Size + 256);
            
            foreach(var i in Data)
            {
                read.WriteByte(i);
            }

            for (int i = 0; i < 256; i++)
            {
                ns.WriteByte(1);
            }

            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);            
        }

    }
}
