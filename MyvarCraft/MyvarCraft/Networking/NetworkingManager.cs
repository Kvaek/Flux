using MyvarCraft.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Networking
{
    public class NetworkingManager
    {
        private TcpListener _tcpList;

        public NetworkingManager()
        {
            _tcpList = new TcpListener(IPAddress.Any, Globals.Config.Port);
            _tcpList.Start();
        }

        public Player Listen()
        {
            return new Player(_tcpList.AcceptTcpClient());
        }
    }
}
