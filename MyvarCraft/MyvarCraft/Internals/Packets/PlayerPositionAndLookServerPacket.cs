using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class PlayerPositionAndLookServerPacket : Packet
    {
        public double X { get; set; } = 10;
        public double Y { get; set; } = 30;
        public double Z { get; set; } = 10;
        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;
        public byte OnGround { get; set; } = 0;



        public PlayerPositionAndLookServerPacket()
        {
            ID = 0x06;
            IlegalState = 2;
        }

        public override void Parse(byte[] b)
        {
            StreamHelper read = new StreamHelper();
            X = read.ReadDouble(b);
            Y = read.ReadDouble(b);
            Z = read.ReadDouble(b);
            Yaw = read.ReadSingle(b);
            Pitch = read.ReadSingle(b);
            OnGround = read.ReadByte(b);
        }

        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            read.WriteDouble(X);
            read.WriteDouble(Y);
            read.WriteDouble(Z);
            read.WriteSingle(Yaw);
            read.WriteSingle(Pitch);
            read.WriteByte(OnGround);
            return read.Flush(ID);
        }

    }
}
