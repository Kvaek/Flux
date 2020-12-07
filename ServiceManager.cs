using System.Collections.Generic;
using System.Threading;

namespace Flux {
	public class ServiceManager {
		public ServiceManager() {
			ServiceThread = new Thread(Tick);
		}

		private Dictionary<string, Service> Services { get; } = new Dictionary<string, Service>();

		private Thread ServiceThread { get; }

		public void Init() {
			Services.Clear();
		}

		public Service GetService(string name) => Services[name];

		public void Start() {
			foreach (KeyValuePair<string, Service> i in Services) {
				i.Value.Start();
			}

			ServiceThread.Start();
		}

		public void Stop() {
			ServiceThread.Abort();
			foreach (KeyValuePair<string, Service> i in Services) {
				i.Value.Stop();
			}
		}

		public void AddService(Service s) {
			Services.Add(s.Name, s);
		}

		private void Tick() {
			while (true) {
				foreach (KeyValuePair<string, Service> i in Services) {
					i.Value.Tick();
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