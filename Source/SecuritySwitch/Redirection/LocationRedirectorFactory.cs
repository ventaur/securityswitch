// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using SecuritySwitch.Abstractions;


namespace SecuritySwitch.Redirection {
	internal class LocationRedirectorFactory : ContextCachedFactoryBase<LocationRedirectorFactory, ILocationRedirector> {
		protected override string CacheKey {
			get { return "SecuritySwitch.LocationRedirector"; }
		}


		/// <summary>
		/// Gets the default location redirector.
		/// </summary>
		/// <returns></returns>
		internal ILocationRedirector Create(HttpContextBase context) {
			var redirector = GetCacheValue(context);
			if (redirector != null) {
				return redirector;
			}

			Logger.Log("Creating LocationRedirector.");
			return new LocationRedirector();
		}
	}
}