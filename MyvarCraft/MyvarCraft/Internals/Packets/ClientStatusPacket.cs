using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class ClientStatusPacket : Packet
    {

        public int ActionID { get; set; } = 0;
      


        public ClientStatusPacket()
        {
            ID = 0x16;
            IlegalState = 2;
        }

        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            read.WriteVarInt(ActionID);           
            return read.Flush(ID);
        }

    }
}
