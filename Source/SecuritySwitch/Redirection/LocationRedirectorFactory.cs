// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

namespace SecuritySwitch.Redirection {
	/// <summary>
	/// A factory for ILocationRedirector.
	/// </summary>
	public static class LocationRedirectorFactory {
		/// <summary>
		/// Gets the default location redirector.
		/// </summary>
		/// <returns></returns>
		internal static ILocationRedirector Create() {
			Logger.Log("Creating LocationRedirector.");
			return new LocationRedirector();
		}
	}
}