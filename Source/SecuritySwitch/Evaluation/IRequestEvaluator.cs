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


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// The contract for a request evaluator.
	/// </summary>
	public interface IRequestEvaluator {
		/// <summary>
		/// Evaluates the specified request for the need to switch its security.
		/// </summary>
		/// <param name="request">The request to evaluate.</param>
		/// <param name="settings">The settings to use for evaluation.</param>
		/// <return>
		/// A RequestSecurity value indicating the security the evaluated request should be under.
		/// </return>
		RequestSecurity Evaluate(HttpRequestBase request, Settings settings);
	}
}