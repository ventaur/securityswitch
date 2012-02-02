// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using Common.Logging;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A factory for ISecurityEvaluator.
	/// </summary>
	public static class SecurityEvaluatorFactory {
		private static readonly ILog _log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Gets a security evaluator.
		/// </summary>
		/// <returns></returns>
		public static ISecurityEvaluator Create(HttpRequestBase request, Settings settings) {
			// If a security port is configured, create a PortSecurityEvaluator.
			if (settings.SecurityPort.HasValue) {
				_log.Debug(m => m("Creating PortSecurityEvaluator."));
				return new PortSecurityEvaluator();
			}

			// If security server variables are expected, and server variables exist, create a ServerVariablesSecurityEvaluator.
			if (!string.IsNullOrEmpty(settings.OffloadedSecurityServerVariables) && request.ServerVariables != null) {
				_log.Debug(m => m("Creating ServerVariablesSecurityEvaluator."));
				return new ServerVariablesSecurityEvaluator();
			}
			
			// If security headers are expected, and headers exist, create a HeadersSecurityEvaluator.
			if (!string.IsNullOrEmpty(settings.OffloadedSecurityHeaders) && request.Headers != null) {
				_log.Debug(m => m("Creating HeadersSecurityEvaluator."));
				return new HeadersSecurityEvaluator();
			}

			// Create a StandardSecurityEvaluator.
			_log.Debug(m => m("Creating StandardSecurityEvaluator."));
			return new StandardSecurityEvaluator();
		}
	}
}