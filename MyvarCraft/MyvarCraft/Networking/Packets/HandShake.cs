using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class HandShake : Packet
    {
        public int ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public int NextState { get; set; }

        public HandShake()
        {
            IDs.Add(0);
            IDs.Add(-1);
            IDs.Add(-1);
        }

        public override Packet Parse(byte[] data)
        {
            var re = new HandShake();

            MinecraftStream ms = new MinecraftStream();
            ms.ReadVarInt(data);
            re.ID = ms.ReadVarInt(data);

            re.ProtocolVersion = ms.ReadVarInt(data);

            var l = ms.ReadVarInt(data);
            re.ServerAddress = ms.ReadString(data, l);

            re.ServerPort = ms.ReadUShort(data);

            re.NextState = ms.ReadVarInt(data);

            return re;
        }
    }
}
