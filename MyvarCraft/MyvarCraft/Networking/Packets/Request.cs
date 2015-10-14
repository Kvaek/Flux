using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class Request : Packet
    {


        public Request()
        {
            IDs.Add(-1);
            IDs.Add(0);
            IDs.Add(-1);
        }

        public override Packet Parse(byte[] data)
        {
            var re = new Request();
            MinecraftStream ms = new MinecraftStream();
            ms.ReadVarInt(data);
            re.ID = ms.ReadVarInt(data);
            return re;
        }
    }
}
