using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class LoginSuccessPacket : Packet
    {
        public string UUID { get; set; }
        public string Username { get; set; }

        public LoginSuccessPacket()
        {
            ID = 0x02;
            IlegalState = 1;
        }

        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            read.WriteString(UUID);
            read.WriteString(Username);
            return read.Flush(0x02);
        }

    }
}
