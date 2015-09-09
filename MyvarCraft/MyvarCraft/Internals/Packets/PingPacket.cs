using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class PingPacket : Packet
    {
        public long PayLoad { get; set; }
      
        public PingPacket()
        {
            ID = 0x01;
        }

        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            read.WriteLong(PayLoad);

            return read.Flush(ID);
        }

        public override void Parse(byte[] b)
        {
            StreamHelper read = new StreamHelper();
            read.ReadVarInt(b);//Packet Length
            read.ReadVarInt(b);//Packet ID
            PayLoad = read.ReadLong(b);
        }
    }
}
