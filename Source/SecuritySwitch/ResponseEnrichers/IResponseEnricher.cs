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
	/// A contract for enriching a response as needed.
	/// </summary>
	public interface IResponseEnricher {
		void Enrich(HttpResponseBase response, HttpRequestBase request, ISecurityEvaluator securityEvaluator, Settings settings);
	}
}