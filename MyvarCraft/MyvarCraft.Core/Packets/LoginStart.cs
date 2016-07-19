using MyvarCraft.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Packets
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

        public override Packet Read(byte[] data)
        {
            var re = new LoginStart();
            MinecraftStream ms = new MinecraftStream(data);
            re.ID = ms.ReadVarInt();
            var l = ms.ReadVarInt();
            re.Name = ms.ReadString(l);
            return re;
        }
    }
}

