using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Internals
{
    public class Packet
    {
        public byte[] Raw { get; set; }

        public List<int> IDs { get; set; } = new List<int>();

        public int ID { get; set; }

        public virtual Packet Parse(byte[] data)
        {
            var re = new Packet();
            MinecraftStream ms = new MinecraftStream();
            ms.ReadVarInt(data);
            re.ID = ms.ReadVarInt(data);
            return re;
        }

        public virtual void Write(NetworkStream ns)
        {
            ns.Write(new byte[] { 0 }, 0, 1);
        }
    }
}
