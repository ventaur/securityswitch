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
	/// A factory for ISecurityEvaluator.
	/// </summary>
	public class SecurityEvaluatorFactory : ContextCachedFactoryBase<SecurityEvaluatorFactory, ISecurityEvaluator> {
		protected override string CacheKey { get { return "SecuritySwitch.SecurityEvaluator"; } }
		
		/// <summary>
		/// Gets a security evaluator.
		/// </summary>
		/// <returns></returns>
		public ISecurityEvaluator Create(HttpContextBase context, Settings settings) {
			var cachedValue = GetCacheValue(context);
			if (cachedValue != null) {
				return cachedValue;
			}

			// If a security port is configured, create a PortSecurityEvaluator.
			if (settings.SecurityPort.HasValue) {
				Logger.Log("Creating PortSecurityEvaluator.");
				return SetCacheValue(new PortSecurityEvaluator(), context);
			}

			// If security server variables are expected, and server variables exist, create a ServerVariablesSecurityEvaluator.
			if (!string.IsNullOrEmpty(settings.OffloadedSecurityServerVariables) && context.Request.ServerVariables != null) {
				Logger.Log("Creating ServerVariablesSecurityEvaluator.");
				return SetCacheValue(new ServerVariablesSecurityEvaluator(), context);
			}
			
			// If security headers are expected, and headers exist, create a HeadersSecurityEvaluator.
			if (!string.IsNullOrEmpty(settings.OffloadedSecurityHeaders) && context.Request.Headers != null) {
				Logger.Log("Creating HeadersSecurityEvaluator.");
				return SetCacheValue(new HeadersSecurityEvaluator(), context);
			}

			// Create a StandardSecurityEvaluator.
			Logger.Log("Creating StandardSecurityEvaluator.");
			return SetCacheValue(new StandardSecurityEvaluator(), context);
		}
	}
}