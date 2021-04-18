using System;

namespace Flux.Core.Utils {
	// TODO: Make useful and pretty
	public class LogHelper {
		private readonly object _lock = new object();
		private const string DatetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

		/// <summary>
		/// Initiate an instance of LogHelper class constructor.
		/// </summary>
		public LogHelper() {
		}

		/// <summary>
		/// Log an INFO message
		/// </summary>
		/// <param name="text">Message</param>
		public void Info(string text) {
			lock (_lock) {
				Console.WriteLine($"{DateTime.Now.ToString(DatetimeFormat)} [INFO]    {text}");
			}
		}
		
		/// <summary>
		/// Log a DEBUG message
		/// </summary>
		/// <param name="text">Message</param>
		public void Debug(string text) {
			lock (_lock) {
				Console.WriteLine($"{DateTime.Now.ToString(DatetimeFormat)} [DEBUG]   {text}");
			}
		}
		
		/// <summary>
		/// Log an ERROR message
		/// </summary>
		/// <param name="text">Message</param>
		public void Error(string text) {
			lock (_lock) {
				Console.WriteLine($"{DateTime.Now.ToString(DatetimeFormat)} [ERROR]   {text}");
			}
		}
		
		/// <summary>
		/// Log a FATAL message
		/// </summary>
		/// <param name="text">Message</param>
		public void Fatal(string text) {
			lock (_lock) {
				Console.WriteLine($"{DateTime.Now.ToString(DatetimeFormat)} [FATAL]   {text}");
			}
		}
		
		/// <summary>
		/// Log a WARN message
		/// </summary>
		/// <param name="text">Message</param>
		public void Warn(string text) {
			lock (_lock) {
				Console.WriteLine($"{DateTime.Now.ToString(DatetimeFormat)} [WARN]   {text}");
			}
		}
		
		/// <summary>
		/// Log a TRACE message
		/// </summary>
		/// <param name="text">Message</param>
		public void Trace(string text) {
			lock (_lock) {
				Console.WriteLine($"{DateTime.Now.ToString(DatetimeFormat)} [TRACE]   {text}");
			}
		}
		[Flags]
		private enum LogLevel {
			TRACE,
			INFO,
			DEBUG,
			WARNING,
			ERROR,
			FATAL
		}
	}
}