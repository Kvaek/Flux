using System;
using System.Collections.Generic;
using System.Threading;

namespace Flux {
	public class ServiceManager {
		public ServiceManager() {
			ServiceThread = new Thread(Tick);
		}
		private Dictionary<string, Service> Services { get; set; } = new Dictionary<string, Service>();

		private Thread ServiceThread { get; }
		private object _locker { get; } = new object();

		public void Init() {
			lock (_locker) {
				Services.Clear();
			}
		}

		public Service GetService(string name) {
			return Services[name];
		}

		public void Start() {
			foreach (KeyValuePair<string, Service> i in Services)
				lock (_locker) {
					i.Value.Start();
				}

			ServiceThread.Start();
		}

		public void Stop() {
			ServiceThread.Abort();
			foreach (KeyValuePair<string, Service> i in Services)
				lock (_locker) {
					i.Value.Stop();
				}
		}

		public void AddService(Service s) {
			lock (_locker) {
				Services.Add(s.Name, s);
			}
		}

		private void Tick() {
			while (true) {
				foreach (KeyValuePair<string, Service> i in Services) {
					lock (_locker) {
						i.Value.Tick();
					}
				}
				Thread.Sleep(50);
			}
		}
	}

	public abstract class Service {
		public string Name;
		public int Priority;
		public abstract void Tick();
		public abstract void Start();
		public abstract void Stop();
	}
}