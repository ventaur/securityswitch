// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using Common.Logging;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A security evaluator that simply relies on the request to determine if the connection is secure.
	/// </summary>
	public class StandardSecurityEvaluator : ISecurityEvaluator {
		private static readonly ILog _log = LogManager.GetLogger<StandardSecurityEvaluator>();

		/// <summary>
		/// Determines whether the specified request is over a secure connection.
		/// </summary>
		/// <param name="request">The request to test.</param>
		/// <param name="settings">The settings used for determining a secure connection.</param>
		/// <returns>
		///   <c>true</c> if the specified request is over a secure connection; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSecureConnection(HttpRequestBase request, Settings settings) {
			_log.Debug(m => m("Connection {0} secure.", request.IsSecureConnection ? "is" : "is not"));
			return request.IsSecureConnection;
		}
	}
}