using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class PlayerPositionPacet : Packet
    {

        public double X { get; set; } = 2.0;
        public double Y { get; set; } = 10.0;
        public double Z { get; set; } = 2.0;
        public byte  OnGround { get; set; } = 0;



        public PlayerPositionPacet()
        {
            ID = 0x04;
          //  IlegalState = 2;
        }

        public override byte[] Build()
        {
            StreamHelper read = new StreamHelper();
            read.WriteDouble(X);
            read.WriteByte(OnGround);

            read.WriteDouble(Y);
            read.WriteDouble(Z);
            return read.Flush(ID);
        }

    }
}
