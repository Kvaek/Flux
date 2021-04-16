using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.Core {
	public static class ServiceManager {
		public static Dictionary<string, IService> ServiceIndex { get; set; } = new Dictionary<string, IService>();

		private static Thread TickThread { get; set; } = new Thread(Tick);
		private static object _Locker { get; set; } = new object();

		public static void Reset() {
			lock (_Locker) { ServiceIndex.Clear(); }
		}

		public static T GetService<T>(string Name) => (T) ServiceIndex[Name];

		public static void Start() {
			foreach (KeyValuePair<string, IService> i in ServiceIndex) {
				lock (_Locker) { i.Value.Start(); }
			}

			TickThread.Start();
		}

		public static void Stop() {
			TickThread.Abort();
			foreach (KeyValuePair<string, IService> i in ServiceIndex)
				lock (_Locker) { i.Value.Stop(); }
		}

		public static void AddService(IService s) {
			lock (_Locker) { ServiceIndex.Add(s.Name, s); }
		}

		public static void Invoke(Action a) {
			lock (_Locker) { a(); }
		}

		private static void Tick() {
			const double ns = 1000000000.0 / 20.0;
			double delta = 0;

			long lastTime = DateTime.Now.Ticks * 100;

			while (true) {
				long now = DateTime.Now.Ticks * 100;
				delta += (now - lastTime) / ns;
				lastTime = now;

				while (delta >= 1) {
					foreach (KeyValuePair<string, IService> i in ServiceIndex) {
						lock (_Locker) { i.Value.Tick(); }
						//i.Value.Tick();
					}
					delta--;
				}
			}
			/*
			while (true) {
				foreach (KeyValuePair<string, IService> i in ServiceIndex) {
					lock (_Locker) { i.Value.Tick(); }
				}
				Thread.Sleep(50);
			}
			*/
		}
	}

	public interface IService {
		string Name { get; set; }
		void Tick();
		void Start();
		void Stop();
	}
}