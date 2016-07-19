using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Services
{
    public class NetworkService : IService
    {
        public string Name { get; set; } = "NetworkService";
        public static List<Packet> Packets { get; set; } = new List<Packet>();
        public static List<ConnectionWrapper> _cw { get; set; } = new List<ConnectionWrapper>();

        private static object _Locker { get; set; } = new object();
        private bool Run { get; set; } = false;

        public static void EnqueuePacket(Packet p)
        {
            lock (_Locker)
            {
                Packets.Add(p);
            }
        }

        public static Packet GetPacket<T>() where T : new()
        {
            lock (_Locker)
            {
                Packet re = null;
                foreach (var i in Packets)
                {
                    if (i.GetType() == new T().GetType())
                    {
                        re = i;
                        break;
                    }
                }

                if (re != null)
                {
                    Packets.Remove(re);
                    return re;
                }

                return null;
            }

        }

        public static Packet GetPacket(Guid g, bool send = false)
        {
            lock (_Locker)
            {
                Packet re = null;
                foreach (var i in Packets)
                {
                    if (i.Owner == g && i.Send == send)
                    {
                        re = i;
                        break;
                    }
                }

                if (re != null)
                {
                    Packets.Remove(re);
                    return re;
                }

                return null;
            }
        }

        public static bool IsAvalible(Packet p)
        {
            lock (_Locker)
            {
                foreach (var i in Packets)
                {
                    if (i.GetType() == p.GetType())
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public  void Tick()
        {
            lock (_Locker)
            {
                foreach (var i in _cw.ToArray())
                {
                    try
                    {
                        if (i._ns.DataAvailable)
                        {

                            byte[] buffer = new byte[4096];


                            var value = 0;
                            var size = 0;
                            var bsize = 0;
                            int b;
                            while (((b = i._ns.ReadByte()) & 0x80) == 0x80)
                            {
                                bsize++;
                                value |= (b & 0x7F) << (size++ * 7);
                                if (size > 5)
                                {
                                    throw new IOException("raise the shields intruder alert!");// imagin Jean-Luc Picard saying that on the bridge of the enterprise
                                }
                            }
                            var psize = value | ((b & 0x7F) << (size * 7));

                            buffer = new byte[psize - bsize];

                            int bytesread = i._ns.Read(buffer, 0, buffer.Length);
                            Array.Resize(ref buffer, bytesread);
                            var pp = Packet.GetPacket(buffer, i.State);
                            if (pp != null)
                            {
                                pp.Owner = i.OwnerID;

                                EnqueuePacket(pp);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee);
                        _cw.Remove(i);
                    }

                    var send = GetPacket(i.OwnerID, true);
                    if (send != null)
                    {
                        i.Send(send);
                    }
                }
            }
        }

        public void Start()
        {
            Run = true;

            ThreadPool.QueueUserWorkItem((c) =>
            {
                var tl = new TcpListener(IPAddress.Any, 25565);
                tl.Start();

                while (Run)
                {
                    var cq = new ConnectionWrapper(tl.AcceptTcpClient());
                    lock (_Locker)
                    {
                        _cw.Add(cq);
                    }
                }

            });

        }

        public void Stop()
        {
            Run = false;
        }
    }

    public class ConnectionWrapper
    {
        public TcpClient _Client { get; set; }
        public NetworkStream _ns { get; set; }
        public Guid OwnerID { get; set; } = Guid.NewGuid();
        public int State { get; set; } = 0;

        public ConnectionWrapper(TcpClient c)
        {
            _Client = c;
            _ns = _Client.GetStream();
        }

        public void Send(Packet p)
        {
            p.Flush(_ns);
        }
    }
}
