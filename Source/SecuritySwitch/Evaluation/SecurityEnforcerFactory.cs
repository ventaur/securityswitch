// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A factory for ISecurityEnforcer.
	/// </summary>
	public static class SecurityEnforcerFactory {
		/// <summary>
		/// Gets a security enforcer.
		/// </summary>
		/// <returns></returns>
		public static ISecurityEnforcer Create(ISecurityEvaluator securityEvaluator) {
			Logger.Log("Creating SecurityEnforcer.");
			return new SecurityEnforcer(securityEvaluator);
		}
	}
}