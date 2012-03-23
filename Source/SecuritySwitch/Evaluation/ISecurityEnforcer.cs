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
	/// A contract for an enforcer of request security.
	/// </summary>
	public interface ISecurityEnforcer {
		/// <summary>
		/// Gets any URI for the specified request that ensures it is being accessed by the proper protocol, if a match is found in the settings.
		/// </summary>
		/// <param name="request">The request to ensure proper access for.</param>
		/// <param name="response">The response to use if a redirection or other output is necessary.</param>
		/// <param name="security">The security setting to match.</param>
		/// <param name="settings">The settings used for any redirection.</param>
		/// <returns>A URI that ensures the requested resources matches the specified security; or null if the current request already does.</returns>
		string GetUriForMatchedSecurityRequest(HttpRequestBase request, HttpResponseBase response, RequestSecurity security, Settings settings);
	}
}