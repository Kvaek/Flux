using MyvarCraft.Core.Internals.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Internals
{

    public static class PacketManager
    {
        private static List<Packet> _packets { get; set; } = new List<Packet>()
        {
          new HandShake(),
          new Ping(),
          new Pong(),
          new Request(),
          new Response()
        };

        public static Packet GetPacket(byte[] raw, int state)
        {
            var ms = new MinecraftStream();
            ms.ReadVarInt(raw);
            var id = ms.ReadVarInt(raw);
            foreach (var i in _packets)
            {
                if (i.IDs[state] == id)
                {
                    return i.Parse(raw);
                }
            }
            return null;
        }
    }

}
