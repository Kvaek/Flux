using System;
using System.Threading;
using Flux.Core.Services;
using Flux.Core.Utils;

namespace Flux.Core {
	public class Flux {
		public static Flux INSTANCE { get; private set; }

		public ServiceManager Sm = new ServiceManager();
		public MinecraftConfig Cfg { get; set; }
		public LogHelper Logger = new LogHelper();

		public void Start() {
			INSTANCE = this;
			#region Config
			Cfg = new MinecraftConfig {
				Version = new object[2] { "754", "1.16.5" },
				MaxPlayers = 20,
				Motd = "A .NET sever running Flux",
				Port = 25565
			};
			#endregion

			#region Services

			// Initialize ServiceManager
			Sm.Init();
			// Add NetWork service
			Sm.AddService(new NetworkService());
			// Start ServiceManager
			Sm.Start();

			#endregion

			Console.CancelKeyPress += async (sender, e) => {
				e.Cancel = true;
				await Sm.Stop();
				Environment.Exit(0);
			};
			new CancellationTokenSource().Token.WaitHandle.WaitOne();
		}
	}
}