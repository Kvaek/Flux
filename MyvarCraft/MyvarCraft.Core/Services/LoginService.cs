using MyvarCraft.Core.Objects;
using MyvarCraft.Core.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyvarCraft.Core.Services
{
    public class LoginService : IService
    {
        public string Name { get; set; } = "LoginService";
       

        public static void Disconnected(Guid pl)
        {
            lock (MinecraftServer.Worlds)
            {
                foreach (var i in MinecraftServer.Worlds)
                {
                    foreach (var p in i.Players.ToArray())
                    {
                        if (p.OwnerID == pl)
                        {
                            i.Players.Remove(p);
                            return;
                        }

                    }
                }
            }
        }

        public void Start()
        {

        }

        public void Stop()
        {
            
        }


        public void Tick()
        {
            if (NetworkService.IsAvalible(new LoginStart()))
            {
                var p = NetworkService.GetPacket<LoginStart>() as LoginStart;
                var pl = new Player() { OwnerID = p.Owner, Name = p.Name };
               

                var Logins = new LoginSuccess() { Owner = p.Owner };
                Logins.Username = p.Name;
                Logins.UUID = GetUuid(p.Name);

                NetworkService.EnqueuePacket(Logins);

                var jg = new JoinGame() { Owner = p.Owner };
                jg.EntityID = 0;
                jg.Gamemode = (byte)pl.GameMode;
                jg.Dimension = 0;
                jg.Difficulty = 0;
                jg.MaxPlayers = 255;
                jg.LevelType = "default";
                jg.ReducedDebugInfo = 0;

                NetworkService.EnqueuePacket(jg);


                var ppal = new PlayerPositionAndLook() { Owner = p.Owner };
                ppal.X = pl.X;
                ppal.Y = pl.Y;
                ppal.Z = pl.Z;
                ppal.Yaw = pl.Yaw;
                ppal.Pitch = pl.Pitch;
                ppal.Flags = 255;

                NetworkService.EnqueuePacket(ppal);

                pl.Spawned = true;

                MinecraftServer.Worlds[0].Players.Add(pl);
            }
        }


        private string GetUuid(string username)
        {
            try
            {
                var wc = new WebClient();
                var result = wc.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username);
                var _result = result.Split('"');
                if (_result.Length > 1)
                {
                    var uuid = _result[3];
                    return new Guid(uuid).ToString();
                }
                return Guid.NewGuid().ToString();
            }
            catch
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}
