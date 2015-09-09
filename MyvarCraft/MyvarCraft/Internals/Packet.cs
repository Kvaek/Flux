using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals
{
    public class Packet
    {
        public int ID { get; set; }
        public int IlegalState { get; set; }
        internal StreamHelper Reader = new StreamHelper();

        public virtual byte[] Build()
        {
            return new byte[] { 0 };
        }

        public virtual void Parse(byte[] b)
        {
            Reader = new StreamHelper();
            Reader.ReadVarInt(b);//Packet Length
            ID = Reader.ReadVarInt(b);
        }

    }
}
