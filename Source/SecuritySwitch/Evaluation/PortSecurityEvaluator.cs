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
	/// A security evaluator that checks if the request's port matches a configured security port.
	/// </summary>
	public class PortSecurityEvaluator : ISecurityEvaluator {
		/// <summary>
		/// Determines whether the specified request is over a secure connection.
		/// </summary>
		/// <param name="request">The request to test.</param>
		/// <param name="settings">The settings used for determining a secure connection.</param>
		/// <returns>
		///   <c>true</c> if the specified request is over a secure connection; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSecureConnection(HttpRequestBase request, Settings settings) {
			bool isPortMatch = (request.Url.Port == settings.SecurityPort);
			Logger.LogFormat("Checking if the request port matches the SecurityPort; {0}.", isPortMatch ? "it does" : "no match");
			return isPortMatch;
		}
	}
}