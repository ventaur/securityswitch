// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;


namespace SecuritySwitch.ResponseEnrichers {
	/// <summary>
	/// A response enricher that adds the necessary HSTS header when necessary.
	/// </summary>
	/// <remarks>
	/// https://www.owasp.org/index.php/HTTP_Strict_Transport_Security
	/// </remarks>
	public class HstsResponseEnricher : IResponseEnricher {
		public void Enrich(HttpResponseBase response, HttpRequestBase request, ISecurityEvaluator securityEvaluator, Settings settings) {
			if (!securityEvaluator.IsSecureConnection(request, settings) || !settings.EnableHsts) {
				return;
			}
			
			// Add the needed STS header.
			response.AddHeader("Strict-Transport-Security", string.Format("max-age={0:f0}", settings.HstsMaxAge));
		}
	}
}