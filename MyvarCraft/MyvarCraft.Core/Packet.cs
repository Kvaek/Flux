using MyvarCraft.Core.Packets;
using MyvarCraft.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core
{
    public class Packet
    {
        public List<int> IDs { get; set; } = new List<int>();
        public int ID { get; set; }
        public Guid Owner { get; set; }
        public bool Send { get; set; }

        public Packet()
        {

        }

        public Packet(byte[] data)
        {
            Read(data);
        }

        public virtual void Flush(NetworkStream ns)
        {
            ns.Write(new byte[] { 0 }, 0, 1);
        }

        public virtual Packet Read(byte[] data)
        {
            var re = new Packet();
            MinecraftStream ms = new MinecraftStream(data);
            re.ID = ms.ReadVarInt();
            return re;
        }


        private static List<Packet> _packets { get; set; } = new List<Packet>()
        {
            new Ping(),
            new HandShake(),
            new Request()
        };

        public static Packet GetPacket(byte[] raw, int state)
        {
            var ms = new MinecraftStream(raw);
            var id = ms.ReadVarInt();
            foreach (var i in _packets)
            {
                if (i.IDs[state] == id)
                {
                    return i.Read(raw);
                }
            }
            return null;
        }
    }
}
