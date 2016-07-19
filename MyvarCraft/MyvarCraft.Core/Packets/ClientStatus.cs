using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Packets
{
    public class ClientStatus : Packet
    {
        public ClientStatus()
        {
            ID = 0x03;
            Send = true;
        }
    }
}
