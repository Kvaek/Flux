using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class ChatMessage : Packet
    {
        public string Message { get; set; }
        public string JSONData { get; set; }
        public byte Position { get; set; }
       

        public ChatMessage()
        {
            IDs.Add(-1);
            IDs.Add(-1);
            IDs.Add(0x01);

            ID = 0x0F;
        }

        public override Packet Parse(byte[] data)
        {
            var re = new ChatMessage();
            MinecraftStream ms = new MinecraftStream();
            ms.ReadVarInt(data);
            re.ID = ms.ReadVarInt(data);
            var l = ms.ReadVarInt(data);
            re.Message = ms.ReadString(data, l);
        
            return re;
        }

        public override void Write(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();
            read.WriteString(JSONData);
            read.WriteByte(Position);
            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }

    }
}
