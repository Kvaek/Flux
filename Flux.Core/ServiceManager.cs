using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.Core {
	public class ServiceManager {
		public ServiceManager() => ServiceThread = new Thread(Tick);

		private Dictionary<string, Service> Services { get; } = new Dictionary<string, Service>();

		private Thread ServiceThread { get; }

		public void Init() {
			Services.Clear();
		}

		public Service GetService(string name) => Services[name];

		public async void Start() {
			foreach ((_, Service service)in Services.Where(s => s.Value.Running == false)) {
				Flux.INSTANCE.Logger.Debug($"Starting service: {service.Name}");
				await service.Start().ContinueWith(s => { service.Running = true;});
			}
			ServiceThread.Start();
		}

		public async Task Stop() {
			ServiceThread.Abort();
			foreach ((_, Service service) in Services) {
				Flux.INSTANCE.Logger.Debug($"Stopping service: {service.Name}");
				await service.Stop().ContinueWith(s => { service.Running = false;});
			}
		}

		public void AddService(Service s) {
			Services.Add(s.Name, s);
		}

		private void Tick() {
			const double ns = 1000000000.0 / 20.0;
			double delta = 0;

			long lastTime = DateTime.Now.Ticks * 100;

			while (true) {
				long now = DateTime.Now.Ticks * 100;
				delta += (now - lastTime) / ns;
				lastTime = now;

				while (delta >= 1) {
					foreach ((_, Service service) in Services) {
						service.Tick();
					}
					delta--;
				}
			}
		}
	}

	public abstract class Service {
		public string Name;
		public int Priority;
		public bool Running;
		public abstract Task Tick();
		public abstract Task Start();
		public abstract Task Stop();
	}
}