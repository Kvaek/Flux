using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class JoinGamePacket : Packet
    {

        public int EntityID { get; set; } = 0;
        public byte Gamemode { get; set; } = 16;
        public sbyte Dimension { get; set; } = 0;
        public byte Difficulty { get; set; } = 0;
        public byte MaxPlayers { get; set; } = 100;
        public string LevelType { get; set; } = "flat";
        public byte ReducedDebugInfo { get; set; } = 0x00;

        public JoinGamePacket()
        {
            ID = 1;
            IlegalState = 2;
        }

        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            read.WriteInt(EntityID);
            read.WriteByte(Gamemode);
            read.WriteSByte(Dimension);
            read.WriteByte(Difficulty);
            read.WriteByte(MaxPlayers);
            read.WriteString(LevelType);
            read.WriteByte(ReducedDebugInfo);
            return read.Flush(1);
        }

    }
}
