// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Collections.Specialized;
using System.Web;

using Common.Logging;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A security evaluator that looks for the presence of, or an expected value match with, one or more headers.
	/// </summary>
	public class HeadersSecurityEvaluator : ISecurityEvaluator {
		private static readonly ILog _log = LogManager.GetLogger<HeadersSecurityEvaluator>();

		/// <summary>
		/// Determines whether the specified request is over a secure connection.
		/// </summary>
		/// <param name="request">The request to test.</param>
		/// <param name="settings">The settings used for determining a secure connection.</param>
		/// <returns>
		///   <c>true</c> if the specified request is over a secure connection; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSecureConnection(HttpRequestBase request, Settings settings) {
			_log.Debug(m => m("Checking for any header that matches one from OffloadedSecurityHeaders..."));

			// Parse the expected security headers and check for each.
			NameValueCollection expectedSecurityHeaders = HttpUtility.ParseQueryString(settings.OffloadedSecurityHeaders);
			foreach (string name in expectedSecurityHeaders.AllKeys) {
				// Header not found, move along.
				if (request.Headers[name] == null) {
					continue;
				}

				// If the header exists, but no value is expected OR if the expected value matches the header's value, 
				// indicated a secure connection.
				if (string.IsNullOrEmpty(expectedSecurityHeaders[name]) ||
				    string.Equals(expectedSecurityHeaders[name], request.Headers[name], StringComparison.OrdinalIgnoreCase)) {
						_log.Debug(m => m("Header match found; connection is secure."));
					return true;
				}
			}

			_log.Debug(m => m("No match found; connection is presumed not secure."));
			return false;
		}
	}
}