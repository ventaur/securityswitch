// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

namespace SecuritySwitch {
	/// <summary>
	/// A simple static logger class. 
	/// Set the LogAction property to an action that handles the log message appropriately.
	/// </summary>
	public static class Logger {
		/// <summary>
		/// Represents an action to log a message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="logLevel">The log level.</param>
		public delegate void LogMessage(string message, LogLevel logLevel);

		/// <summary>
		/// Represents the levels of logging.
		/// </summary>
		public enum LogLevel {
			Debug,
			Info,
			Warn
		}


		private static LogMessage _logMessage;

		/// <summary>
		/// Logs the specified message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="logLevel">The log level.</param>
		public static void Log(string message, LogLevel logLevel = LogLevel.Debug) {
			if (_logMessage == null) {
				return;
			}

			_logMessage(message, logLevel);
		}

		/// <summary>
		/// Logs the specified message after formatting it with any arguments.
		/// </summary>
		/// <param name="messageFormat">The message format.</param>
		/// <param name="logLevel">The log level.</param>
		/// <param name="arguments">The arguments.</param>
		public static void LogFormat(string messageFormat, LogLevel logLevel, params object[] arguments) {
			Log(string.Format(messageFormat, arguments), logLevel);
		}

		/// <summary>
		/// Logs the specified message after formatting it with any arguments.
		/// </summary>
		/// <param name="messageFormat">The message format.</param>
		/// <param name="arguments">The arguments.</param>
		public static void LogFormat(string messageFormat, params object[] arguments) {
			LogFormat(messageFormat, LogLevel.Debug, arguments);
		}

		/// <summary>
		/// Sets the log action for the logger. Set to a custom action that handles log messages as you see fit.
		/// </summary>
		/// <param name="logMessage">The delegate to set as the action for logging messages.</param>
		public static void SetLogAction(LogMessage logMessage) {
			_logMessage = logMessage;
		}
	}
}