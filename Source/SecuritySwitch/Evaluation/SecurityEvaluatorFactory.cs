// =================================================================================
// Copyright © 2004-2011 Matt Sollars
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
	internal class SecurityEvaluatorFactory {
		/// <summary>
		/// Gets a security evaluator.
		/// </summary>
		/// <returns></returns>
		internal static ISecurityEvaluator Create(HttpRequestBase request, Settings settings) {
			// If a security port is configured, create a PortSecurityEvaluator.
			if (settings.SecurityPort.HasValue) {
				return new PortSecurityEvaluator();
			}
			
			// If security headers are expected, and headers exist, create a HeadersSecurityEvaluator.
			if (string.IsNullOrEmpty(settings.OffloadedSecurityHeaders) && request.Headers != null) {
				return new HeadersSecurityEvaluator();
			}

			// Create a StandardSecurityEvaluator.
			return new StandardSecurityEvaluator();
		}
	}
}