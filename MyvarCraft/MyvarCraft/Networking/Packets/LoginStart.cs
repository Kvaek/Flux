using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class LoginStart : Packet
    {
        public string Name { get; set; }

        public LoginStart()
        {
            IDs.Add(-1);
            IDs.Add(-1);
            IDs.Add(0);
        }

        public override Packet Parse(byte[] data)
        {
            var re = new LoginStart();
            MinecraftStream ms = new MinecraftStream();
            ms.ReadVarInt(data);
            re.ID = ms.ReadVarInt(data);
            var l = ms.ReadVarInt(data);
            re.Name = ms.ReadString(data, l);
            return re;
        }
    }
}
