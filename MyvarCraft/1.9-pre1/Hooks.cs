using MyvarCraft.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _1._9_pre1
{    
    public static class Hooks
    {
        [Hook("Handshake", 0)]
        public static void HandShake(dynamic Context)
        {
            Context.Player.State = Context.Packet.NextState;
        }

        [Hook("LoginStart", 2)]
        public static void LoginStart(dynamic Context)
        {
            Context.Player.Name = (Context.Packet.Name as string).TrimStart('\0').TrimStart('\a');

            dynamic LoginSuccess = new ExpandoObject();
            LoginSuccess.Name = "LoginSuccess";
            LoginSuccess.ID = 2;
            LoginSuccess.UUID = GetUuid(Context.Player.Name);
            LoginSuccess.Username = Context.Player.Name;

            Context.Player.SendPacket(LoginSuccess);
        }

        private static string GetUuid(string username)
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

        [Hook("Request", 1)]
        public static void StatusRequest(dynamic Context)
        {
            dynamic Response = new ExpandoObject();
            Response.Name = "Request";
            Response.ID = 0;
            Response.JSONResponse = Context.Server.ServerList;

            Context.Player.SendPacket(Response);
        }

        [Hook("Ping", 1)]
        public static void StatusPing(dynamic Context)
        {
            dynamic Pong = new ExpandoObject();
            Pong.Name = "Pong";
            Pong.ID = 1;
            Pong.Payload = Context.Packet.Payload;

            Context.Player.SendPacket(Pong);
        }
    }
}
