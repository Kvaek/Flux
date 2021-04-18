using Flux.Core.Services;

namespace Flux.Core {
	public class MinecraftConfig {
		/// <summary>
		/// <b>Version</b> determines which features are enabled, and what clients can connect.
		/// <typeparam name="[0]">Protocol version, e.g. 47</typeparam>
		/// <typeparam name="[1]">Version "nickname", e.g. 1.8.9</typeparam>
		/// <example>new object[2] { 47, "1.8.9" }</example>
		/// <seealso cref="Flux.Start"/>
		/// </summary>
		public object[] Version { get; set; } = new object[2];
		
		/// <summary>
		/// <b>MaxPlayers</b> determines how many <b>concurrent</b> players is allowed.
		/// <example>20</example>
		/// <seealso cref="ClientManagerService"/>
		/// </summary>
		public int MaxPlayers { get; set; } = 20;
		
		/// <summary>
		/// Message shown in the server list before joining.
		/// <example>My Minecraft Server</example>
		/// <seealso cref="ListService"/>
		/// </summary>
		public string Motd { get; set; } = "A Flux server";
		
		/// <summary>
		/// Server port.
		/// <example>25565</example>
		/// <seealso cref="NetworkService"/>
		/// </summary>
		public short Port { get; set; } = 25565;

		/// <summary>
		/// Server icon
		/// </summary>
		public string Favicon { get; set; } = "data:image/png;base64,<data>";
	}
}