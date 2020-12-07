using System;
using System.Net.Sockets;

namespace Flux.Network {
	public class Packet {
		public Client Client { get; set; }
		public int ID { get; set; }
		public byte[] Data { get; set; }
		
		public Packet() { }

		public Packet(byte[] data) {
			Packet p = new Packet();
			MinecraftStream ms = new MinecraftStream(data);
			p.ID = ms.ReadVarInt();
		}

		public virtual void Read() { }

		public virtual void Write() { }
		
	}
	public abstract class Packet<T> : Packet where T : Packet<T> {
		protected Packet(byte[] data) : base(data) {
		}
	}
}