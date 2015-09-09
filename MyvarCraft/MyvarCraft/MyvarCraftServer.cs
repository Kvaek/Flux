using MyvarCraft.Internals;
using MyvarCraft.Internals.Packets;
using MyvarCraft.Internals.PingList;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft
{
    public class MyvarCraftServer
    {

        private TcpListener tpc = new TcpListener(IPAddress.Any, 25565);
        private int BufferSize = 4096;
        public NetManager NetManager = new NetManager();



        public void Start()
        {
            tpc.Start();
            ThreadPool.QueueUserWorkItem((x) => { NetManager.Start(); }, null);
            ThreadPool.QueueUserWorkItem((x) =>
            {
                while (true)
                {
                    var _cl = tpc.AcceptTcpClient();

                    NetManager.AddNetPlayer(new NetPlayer(_cl));
                }
            }, null);

        }
    }
}