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

        public PlayerPositionAndLook()
        {
            ID = 0x2E;
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

            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }

    }
}
