// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using Common.Logging;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A factory for ISecurityEnforcer.
	/// </summary>
	public static class SecurityEnforcerFactory {
		private static readonly ILog _log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Gets a security enforcer.
		/// </summary>
		/// <returns></returns>
		public static ISecurityEnforcer Create(ISecurityEvaluator securityEvaluator) {
			_log.Debug(m => m("Creating SecurityEnforcer."));
			return new SecurityEnforcer(securityEvaluator);
		}
	}
}