using MyvarCraft.Internals.Packets;
using MyvarCraft.Internals.PingList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarCraft.Internals
{
    public class NetPlayer
    {
        private NetworkStream _stream { get; set; }
        private TcpClient _client { get; set; }

        //portical 
        public int State = 0;
        public bool Terminate = false;
        public int LoginStage = 0;

        //Player
        public string Name { get; set; }
        public bool LoggedIn = false;

        public NetPlayer(TcpClient cl)
        {
            _client = cl;
            _stream = cl.GetStream();
        }


        private IEnumerable<Byte> ReadRange(int index, int lenth, byte[] b)
        {
            for (int i = index; i < lenth; i++)
            {
                yield return b[i];
            }
        }

        public Packet GetPacketFromRaw(byte[] ind)
        {
            Packet p = new Packet();
            p.Parse(ind);

            foreach (var i in NetManager.PacketIndex)
            {
                if (i.ID == p.ID && State != i.IlegalState)
                {
                    return i;
                }
            }

            return new Packet();
        }

        private void WritePacket(Packet p)
        {
            var b = p.Build();
            _stream.Write(b, 0, b.Length);
        }

        private byte[] HandleRawNetStream(byte[] ind)
        {
            try
            {
                if (ind.Length == 2)//request packet
                {
                    if (State == 1)// server ping list
                    {
                        StreamHelper sh = new StreamHelper();
                        var ob = new PingPayload()
                        {
                            Version = new VersionPayload() { Name = "1.8.7", Protocol = 47 },
                            Motd = "A MyvarCraft Server",
                            Players = new PlayersPayload() { Max = 1000, Online = 0 }
                        };

                        string s = JsonConvert.SerializeObject(ob, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                        sh.WriteString(s);
                        return sh.Flush(0);
                    }
                }

                var x = GetPacketFromRaw(ind);

                if (State == 3)
                {
                    JoinGamePacket jgp = new JoinGamePacket();
                    WritePacket(jgp);
                }
                else if (State == 2)
                {
                    if (x is LoginStartPacket)
                    {
                        var z = x as LoginStartPacket;
                        z.Parse(ind);
                        Name = z.UserName;

                        //no autentication for now
                        var rp = new LoginSuccessPacket();
                        rp.Username = Name;
                        //8-4-4-4-12
                        var ud = JavaHexDigest(Name);

                        using (MD5 md5 = MD5.Create())
                        {
                            byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(ud));
                            Guid result = new Guid(hash);
                            rp.UUID = result.ToString();
                        }
                        State = 3;
                        WritePacket(rp);


                        return null;
                    }
                }
                else
                {
                    if (x is HandShakePacket)
                    {
                        var z = x as HandShakePacket;
                        z.Parse(ind);
                        State = z.NextState;
                        return null;
                    }
                    if (x is PingPacket)
                    {
                        var z = x as PingPacket;
                        z.Parse(ind);
                        var b = z.Build();
                        if (State == 1)
                        {
                            Terminate = true;//we serverd all the ping list needs no kill connection to save ram and cpu
                        }
                        return b;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }
            return null;
        }

        private string JavaHexDigest(string data)
        {
            var sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
            bool negative = (hash[0] & 0x80) == 0x80;
            if (negative) // check for negative hashes
                hash = TwosCompliment(hash);
            // Create the string and trim away the zeroes
            string digest = GetHexString(hash).TrimStart('0');
            if (negative)
                digest = "-" + digest;
            return digest;
        }

        private string GetHexString(byte[] p)
        {
            string result = string.Empty;
            for (int i = 0; i < p.Length; i++)
                result += p[i].ToString("x2"); // Converts to hex string
            return result;
        }
        private byte[] TwosCompliment(byte[] p) // little endian
        {
            int i;
            bool carry = true;
            for (i = p.Length - 1; i >= 0; i--)
            {
                p[i] = (byte)~p[i];
                if (carry)
                {
                    carry = p[i] == 0xFF;
                    p[i]++;
                }
            }
            return p;
        }

        public void Update()
        {
            if (Terminate)
            {
                Dispose();
            }
            if (State == 3)
            {
                //Keep alive packet
                StreamHelper sh = new StreamHelper();
                sh.WriteVarInt(new Random().Next());
                var b = sh.Flush(0);
                _stream.Write(b, 0, b.Length);

                if (LoginStage == 0)
                {
                    JoinGamePacket jgp = new JoinGamePacket();
                    WritePacket(jgp);
                    LoginStage = 1;
                }
                if (LoginStage == 1)
                {
                    SpawnPositionPacket spp = new SpawnPositionPacket();
                    WritePacket(spp);
                    LoginStage = 2;
                }
                if (LoginStage == 2)
                {
                    PlayerAbilitiesPacket pap = new PlayerAbilitiesPacket();
                    WritePacket(pap);
                    LoginStage = 3;
                }
                if (LoginStage == 3)
                {
                    PlayerPositionAndLookPacket ppalp = new PlayerPositionAndLookPacket();
                    WritePacket(ppalp);
                    LoginStage = 4;
                }
                if (LoginStage == 4)
                {
                    ClientStatusPacket csp = new ClientStatusPacket();
                    
                   // WritePacket(csp);
                    LoginStage = 5;
                }
                if (LoginStage == 5)
                {
                    ChunkDataPacket cdp = new ChunkDataPacket();
                    WritePacket(cdp);
                    LoginStage = 6;
                    Thread.Sleep(1000000);
                }

            }
            if (_stream.DataAvailable)
            {
                StreamHelper sh = new StreamHelper();
                var buffer = new byte[4096];
                _stream.Read(buffer, 0, buffer.Length);
                var c = sh.ReadVarInt(buffer);
                byte[] retbytes = null;
                retbytes = HandleRawNetStream(ReadRange(0, c + 1, buffer).ToArray());
                if (retbytes != null)
                {
                    _stream.Write(retbytes, 0, retbytes.Length);
                }
            }
        }

        public void Dispose()
        {
            _stream.Close();
            _stream.Dispose();
            _client.Close();
        }
    }
}
