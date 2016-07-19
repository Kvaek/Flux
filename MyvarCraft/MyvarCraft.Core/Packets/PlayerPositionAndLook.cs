using MyvarCraft.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Packets
{
    public class PlayerPositionAndLook : Packet
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public byte Flags { get; set; } = 255;
        public int TeleportID { get; set; } = new Random().Next();

        public PlayerPositionAndLook()
        {
            IDs.Add(-1);
            IDs.Add(-1);
            IDs.Add(0x0C);

            ID = 0x2E;
            Send = true;
        }

        public override Packet Read(byte[] data)
        {
            var re = new PlayerPositionAndLook();
            MinecraftStream ms = new MinecraftStream(data);
            re.ID = ms.ReadVarInt();

            re.X = ms.ReadDouble();
            re.Y = ms.ReadDouble();
            re.Z = ms.ReadDouble();

            //re.Yaw = ms.ReadFloat(data);
            //  re.Pitch = ms.ReadFloat(data);

            // re.OnGround = ms.ReadByte(data);
            return re;
        }

        public override void Flush(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();
            read.WriteDouble(X);
            read.WriteDouble(Y);
            read.WriteDouble(X);
            read.WriteFloat(Yaw);
            read.WriteFloat(Pitch);
            read.WriteByte(Flags);
            read.WriteVarInt(TeleportID);

            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }
    }
}
