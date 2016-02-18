using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Objects
{
    public class Level
    {
        public List<Player> Players { get; set; } = new List<Player>();

        public void AddPlayer(TcpClient t)
        {
            Players.Add(new Player() { _tcp = t, _ns = t.GetStream() , Level = MyvarCraft.Levels.IndexOf(this) });
        }

        public void RemovePlayer(Player p)
        {
            Players.Remove(p);
        }
    }
}
