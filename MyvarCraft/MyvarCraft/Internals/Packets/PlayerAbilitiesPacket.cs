using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class PlayerAbilitiesPacket : Packet
    {

        public byte Flags { get; set; } = 0;
        public float FlyingSpeed { get; set; } = 2f;
        public float WalkingSpeed { get; set; } = 2f;


        public PlayerAbilitiesPacket()
        {
            ID = 0x39;
            IlegalState = 2;
        }

        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            read.WriteByte(Flags);
            read.WriteFloat(FlyingSpeed);
            read.WriteFloat(WalkingSpeed);
            return read.Flush(ID);
        }

    }
}
