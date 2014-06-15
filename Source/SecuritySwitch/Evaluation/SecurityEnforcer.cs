// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Text;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// The default implementation of ISecurityEnforcer.
	/// </summary>
	public class SecurityEnforcer : ISecurityEnforcer {
		private readonly ISecurityEvaluator _securityEvaluator;

		public SecurityEnforcer(ISecurityEvaluator securityEvaluator) {
			if (securityEvaluator == null) {
				throw new ArgumentNullException("securityEvaluator");
			}

			_securityEvaluator = securityEvaluator;
		}

		/// <summary>
		/// Gets any URI for the specified request that ensures it is being accessed by the proper protocol, if a match is found in the settings.
		/// </summary>
		/// <param name="request">The request to ensure proper access for.</param>
		/// <param name="response">The response to use if a redirection or other output is necessary.</param>
		/// <param name="security">The security setting to match.</param>
		/// <param name="settings">The settings used for any redirection.</param>
		/// <returns>A URL that ensures the requested resources matches the specified security; or null if the current request already does.</returns>
		public string GetUriForMatchedSecurityRequest(HttpRequestBase request, HttpResponseBase response, RequestSecurity security, Settings settings) {
			string targetUrl = null;
			
			// Evaluate the request's security.
			Logger.Log("Determining if the connection is secure.");
			bool isSecureConnection = _securityEvaluator.IsSecureConnection(request, settings);

			if (security == RequestSecurity.Secure && !isSecureConnection || 
				security == RequestSecurity.Insecure && isSecureConnection) {
				Logger.Log("Calculating the target URI to switch to.");

				// Determine the target protocol and get any base target URL from the settings.
				string targetProtocolScheme;
				string baseTargetUrl;
				if (security == RequestSecurity.Secure) {
					targetProtocolScheme = Uri.UriSchemeHttps;
					baseTargetUrl = settings.BaseSecureUri;
				} else {
					targetProtocolScheme = Uri.UriSchemeHttp;
					baseTargetUrl = settings.BaseInsecureUri;
				}

				if (string.IsNullOrEmpty(baseTargetUrl)) {
					// If there is no base target URI, just switch the protocol scheme of the current request's URI.
					// * Account for cookie-less sessions by applying the application modifier.
					targetUrl = targetProtocolScheme + Uri.SchemeDelimiter + request.Url.Authority + response.ApplyAppPathModifier(request.RawUrl);
				} else {
					// Build the appropriate URI based on the specified target URL.
					var uri = new StringBuilder(baseTargetUrl);

					// - Use the full request path, but remove any sub-application path.
					uri.Append(request.RawUrl);
					if (request.ApplicationPath.Length > 1) {
						uri.Remove(baseTargetUrl.Length, request.ApplicationPath.Length);
					}

					// Normalize the URI.
					uri.Replace("//", "/", baseTargetUrl.Length - 1, uri.Length - baseTargetUrl.Length);

					targetUrl = uri.ToString();
				}
			}

			return targetUrl;
		}
	}
}