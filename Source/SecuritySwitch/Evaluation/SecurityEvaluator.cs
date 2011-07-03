// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using System;
using System.Web;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// The default implementation of ISecurityEvaluator.
	/// </summary>
	public class SecurityEvaluator : ISecurityEvaluator {
		/// <summary>
		/// Determines whether the specified request is over a secure connection.
		/// </summary>
		/// <param name="request">The request to test.</param>
		/// <param name="settings">The settings used for determining a secure connection.</param>
		/// <returns>
		///   <c>true</c> if the specified request is over a secure connection; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSecureConnection(HttpRequestBase request, Settings settings) {
			// If there are no headers, or no headers are expected, look to the request.
			if (request.Headers == null || string.IsNullOrEmpty(settings.OffloadedSecurityHeaders)) {
				return request.IsSecureConnection;
			}

			// Parse the expected security headers and check for each.
			var expectedSecurityHeaders = HttpUtility.ParseQueryString(settings.OffloadedSecurityHeaders);
			foreach (var name in expectedSecurityHeaders.AllKeys) {
				// Header not found, move along.
				if (request.Headers[name] == null) {
					continue;
				}

				// If the header exists, but no value is expected OR if the expected value matches the header's value, indicated a secure connection.
				if (string.IsNullOrEmpty(expectedSecurityHeaders[name]) || string.Equals(expectedSecurityHeaders[name], request.Headers[name], StringComparison.OrdinalIgnoreCase)) {
					return true;
				}
			}

			return false;
		}
	}
}