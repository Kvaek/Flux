using MyvarCraft.Internals;
using MyvarCraft.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking
{
    public static class PacketManager
    {
        private static List<Packet> _packets { get; set; } = new List<Packet>()
        {
            new HandShake(),
            new Request(),
            new Ping(),
            new LoginStart(),
            new PlayerPositionAndLook(),
            new ChatMessage()
           
        };

        public static Packet GetPacket(byte[] raw, int state)
        {
            var ms = new MinecraftStream();
            ms.ReadVarInt(raw);
            var id = ms.ReadVarInt(raw);
            foreach(var i in _packets)
            {
                if(i.IDs[state] == id)
                {
                    return i.Parse(raw);
                }
            }
            return null;
        }
    }
}
