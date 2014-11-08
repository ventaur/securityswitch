// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using SecuritySwitch.Abstractions;


namespace SecuritySwitch.Evaluation {
	internal class SecurityEnforcerFactory : ContextCachedFactoryBase<SecurityEnforcerFactory, ISecurityEnforcer> {
		protected override string CacheKey {
			get { return "SecuritySwitch.SecurityEnforcer"; }
		}


		/// <summary>
		/// Gets a security enforcer.
		/// </summary>
		/// <returns></returns>
		internal ISecurityEnforcer Create(HttpContextBase context, ISecurityEvaluator securityEvaluator) {
			var enforcer = GetCacheValue(context);
			if (enforcer != null) {
				return enforcer;
			}

			Logger.Log("Creating SecurityEnforcer.");
			return new SecurityEnforcer(securityEvaluator);
		}
	}
}