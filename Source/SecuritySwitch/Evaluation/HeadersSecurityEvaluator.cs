// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System.Collections.Specialized;
using System.Web;

using Common.Logging;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A security evaluator that looks for the presence of, or an expected value match with, one or more headers.
	/// </summary>
	public class HeadersSecurityEvaluator : NameValueCollectionSecurityEvaluator {
		private static readonly ILog _log = LogManager.GetLogger<HeadersSecurityEvaluator>();

		/// <summary>
		/// Determines whether the specified request is over a secure connection.
		/// </summary>
		/// <param name="request">The request to test.</param>
		/// <param name="settings">The settings used for determining a secure connection.</param>
		/// <returns>
		///   <c>true</c> if the specified request is over a secure connection; otherwise, <c>false</c>.
		/// </returns>
		public override bool IsSecureConnection(HttpRequestBase request, Settings settings) {
			_log.Debug(m => m("Checking for any header that matches one from OffloadedSecurityHeaders..."));

			// Parse the expected security headers and check for each against the request headers.
			NameValueCollection expectedSecurityHeaders = HttpUtility.ParseQueryString(settings.OffloadedSecurityHeaders);
			bool isSecure = FindAnyNameValueMatch(expectedSecurityHeaders, request.Headers);

			_log.Debug(
				m =>
				m(isSecure 
					? "Header match found; connection is secure." 
					: "No match found; connection is presumed not secure."));

			return isSecure;
		}
	}
}