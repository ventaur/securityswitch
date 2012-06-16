using System;
using System.Web;

using SecuritySwitch;
using SecuritySwitch.Configuration;

using log4net;
//using NLog;
//using LogManager = NLog.LogManager;


namespace ExampleWebSite {
	public class Global : HttpApplication {
		#region Logging with NLog
		
		//// Get an NLog logger for SecuritySwitch actions.
		//private static readonly NLog.Logger _logger = LogManager.GetLogger("SecuritySwitch");

		//protected void Application_Start(object sender, EventArgs e) {
		//    // Setup a log action in order to capture logs from SecuritySwitch.
		//    // Here, we just pass-through the log message and level to NLog.
		//    SecuritySwitch.Logger.SetLogAction(
		//        (message, logLevel) => {
		//            // Translate the SecuritySwitch LogLevel to NLog's LogLevel.
		//            var translatedLogLevel = LogLevel.FromString(logLevel.ToString());
		//            _logger.Log(translatedLogLevel, message);
		//        });
		//}

		#endregion

		#region Logging with log4net

		// Get a log4net logger for SecuritySwitch actions.
		private static readonly ILog _logger = LogManager.GetLogger("SecuritySwitch");

		private static void LogSecuritySwitchAction(string message, Logger.LogLevel logLevel) {
			switch (logLevel) {
				case Logger.LogLevel.Debug:
					if (_logger.IsDebugEnabled) {
						_logger.Debug(message);
					}
					break;

				case Logger.LogLevel.Info:
					if (_logger.IsInfoEnabled) {
						_logger.Info(message);
					}
					break;

				case Logger.LogLevel.Warn:
					if (_logger.IsWarnEnabled) {
						_logger.Warn(message);
					}
					break;
			}
		}

		protected void Application_Start(object sender, EventArgs e) {
			log4net.Config.XmlConfigurator.Configure();

			// Setup a log action in order to capture logs from SecuritySwitch.
			// Here, we just pass-through the log message and level to log4net.
			SecuritySwitch.Logger.SetLogAction(LogSecuritySwitchAction);
		}

		#endregion

		protected void Session_Start(object sender, EventArgs e) {}

		protected void Application_BeginRequest(object sender, EventArgs e) {}

		protected void Application_AuthenticateRequest(object sender, EventArgs e) {}

		protected void Application_Error(object sender, EventArgs e) {}

		protected void Session_End(object sender, EventArgs e) {}

		protected void Application_End(object sender, EventArgs e) {}

		protected void SecuritySwitch_EvaluateRequest(object sender, EvaluateRequestEventArgs e) {
			// Decide whether or not to let the SecuritySwitch module evaluate this request.

			// In this case, we are overriding the module's evaluation of this request if there is a query string value of "yes" for the "ignoreSecurity" parameter.
			// * The ignoreSecurity query string parameter is randomly set for pages under the Cms site map area.
			var ignoreSecurityParamValue = Request.QueryString["ignoreSecurity"];
			if (!string.IsNullOrEmpty(ignoreSecurityParamValue) &&
			    ignoreSecurityParamValue.Equals("yes", StringComparison.OrdinalIgnoreCase)) {
				e.ExpectedSecurity = RequestSecurity.Ignore;
			}
		}
	}
}