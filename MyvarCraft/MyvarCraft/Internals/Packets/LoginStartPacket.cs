using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class LoginStartPacket : Packet
    {
        public string UserName { get; set; }

        public LoginStartPacket()
        {
            ID = 0;
            IlegalState = 1;
        }

        public override void Parse(byte[] b)
        {
            StreamHelper read = new StreamHelper();
            read.ReadVarInt(b);//Packet Length
            read.ReadVarInt(b);//Packet ID

            var UserNameLength = read.ReadVarInt(b);
            UserName = read.ReadString(b, UserNameLength);

        }

    }
}
