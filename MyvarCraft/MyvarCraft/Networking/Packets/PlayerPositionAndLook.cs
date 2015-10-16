using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class PlayerPositionAndLook : Packet
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public byte Flags { get; set; }
        public byte OnGround { get; set; }

        public PlayerPositionAndLook()
        {
            IDs.Add(-1);
            IDs.Add(-1);
            IDs.Add(0x0C);

            ID = 0x2E;
        }

        public override Packet Parse(byte[] data)
        {
            var re = new PlayerPositionAndLook();
            MinecraftStream ms = new MinecraftStream();
            ms.ReadVarInt(data);
            re.ID = ms.ReadVarInt(data);

            re.X = ms.ReadDouble(data);
            re.Y = ms.ReadDouble(data);
            re.Z = ms.ReadDouble(data);

            //re.Yaw = ms.ReadFloat(data);
          //  re.Pitch = ms.ReadFloat(data);

           // re.OnGround = ms.ReadByte(data);
            return re;
        }

        public override void Write(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();
            read.WriteDouble(X);
            read.WriteDouble(Y);
            read.WriteDouble(X);
            read.WriteFloat(Yaw);
            read.WriteFloat(Pitch);
            read.WriteByte(Flags);
            read.WriteByte(0);

            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }

    }
}
