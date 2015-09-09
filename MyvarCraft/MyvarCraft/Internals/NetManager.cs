using MyvarCraft.Internals.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Internals
{
    public class NetManager
    {
        public static List<Packet> PacketIndex = new List<Packet>() {
            new HandShakePacket(),
            new PingPacket(),
            new LoginStartPacket()
        };


        public List<NetPlayer> Player = new List<NetPlayer>();

        public void AddNetPlayer(NetPlayer p)
        {
            Player.Add(p);
        }

        public void Start()
        {
            while (true)
            {
                try
                {

                    foreach (var i in Player.ToArray())
                    {
                        if (!i.Terminate)
                        {
                            i.Update();
                        }
                    }
                }
                catch(Exception ee)
                {

                }
            }

        }
    }
}
