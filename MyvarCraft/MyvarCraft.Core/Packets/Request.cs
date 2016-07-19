using MyvarCraft.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Packets
{
    public class Request : Packet
    {
        public Request()
        {
            IDs.Add(-1);
            IDs.Add(0);
            IDs.Add(-1);
        }

        public override Packet Read(byte[] data)
        {
            var re = new Request();
            MinecraftStream ms = new MinecraftStream(data);
            re.ID = ms.ReadVarInt();
            return re;
        }
    }
}
