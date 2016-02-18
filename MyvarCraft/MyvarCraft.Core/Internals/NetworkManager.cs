using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Internals
{
    public class NetworkManager
    {
        private TcpListener _tcp { get; set; } = new TcpListener(IPAddress.Any, MyvarCraft.Config.Port);

        public void Listen()
        {
            _tcp.Start();
            ThreadPool.QueueUserWorkItem((x) => {
                while (true)
                {
                    MyvarCraft.Levels[MyvarCraft.Config.DefaultLevel].AddPlayer(_tcp.AcceptTcpClient());
                }
            });
        }
    }
}
