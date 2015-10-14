using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class JoinGame : Packet
    {
        public int EntityID { get; set; }
        public byte Gamemode { get; set; }
        public SByte Dimension { get; set; }
        public byte Difficulty { get; set; }
        public byte MaxPlayers { get; set; }
        public string LevelType { get; set; }
        public SByte ReducedDebugInfo { get; set; }

        public JoinGame()
        {
            ID = 0x24;
        }

        public override void Write(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();

            read.WriteInt(EntityID);
            read.WriteByte(Gamemode);
            read.WriteSByte(Dimension);
            read.WriteByte(Difficulty);
            read.WriteByte(MaxPlayers);
            read.WriteString(LevelType);
            read.WriteSByte(ReducedDebugInfo);


            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }

    }
}
