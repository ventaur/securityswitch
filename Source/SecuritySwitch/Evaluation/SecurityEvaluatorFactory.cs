// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A factory for ISecurityEvaluator.
	/// </summary>
	public static class SecurityEvaluatorFactory {
		/// <summary>
		/// Gets a security evaluator.
		/// </summary>
		/// <returns></returns>
		public static ISecurityEvaluator Create(HttpRequestBase request, Settings settings) {
			// If a security port is configured, create a PortSecurityEvaluator.
			if (settings.SecurityPort.HasValue) {
				Logger.Log("Creating PortSecurityEvaluator.");
				return new PortSecurityEvaluator();
			}

			// If security server variables are expected, and server variables exist, create a ServerVariablesSecurityEvaluator.
			if (!string.IsNullOrEmpty(settings.OffloadedSecurityServerVariables) && request.ServerVariables != null) {
				Logger.Log("Creating ServerVariablesSecurityEvaluator.");
				return new ServerVariablesSecurityEvaluator();
			}
			
			// If security headers are expected, and headers exist, create a HeadersSecurityEvaluator.
			if (!string.IsNullOrEmpty(settings.OffloadedSecurityHeaders) && request.Headers != null) {
				Logger.Log("Creating HeadersSecurityEvaluator.");
				return new HeadersSecurityEvaluator();
			}

			// Create a StandardSecurityEvaluator.
			Logger.Log("Creating StandardSecurityEvaluator.");
			return new StandardSecurityEvaluator();
		}
	}
}