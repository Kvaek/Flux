using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals.Packets
{
    public class HandShakePacket : Packet
    {
        public int ProticalVertion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public int NextState { get; set; }


        public HandShakePacket()
        {
            ID = 0;
            IlegalState = 2;
        }

        public override void Parse(byte[] b)
        {
            StreamHelper read = new StreamHelper();
            read.ReadVarInt(b);//Packet Length
            read.ReadVarInt(b);//Packet ID
            
            ProticalVertion = read.ReadVarInt(b);
            var ServerAddressLength = read.ReadVarInt(b);
            ServerAddress = read.ReadString(b, ServerAddressLength);
            ServerPort = read.ReadUShort(b);
            NextState = read.ReadVarInt(b);
        }

    }
}