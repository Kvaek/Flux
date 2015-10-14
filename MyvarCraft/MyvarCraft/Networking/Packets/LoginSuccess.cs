using MyvarCraft.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking.Packets
{
    public class LoginSuccess : Packet
    {
        public string UUID { get; set; }
        public string Username { get; set; }

        public LoginSuccess()
        {

            ID = 0x02;
        }

        public override void Write(NetworkStream ns)
        {
            MinecraftStream read = new MinecraftStream();
            read.WriteString(UUID);
            read.WriteString(Username);
            var buf = read.Flush(ID);
            ns.Write(buf, 0, buf.Length);
        }

    }
}
