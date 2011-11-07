// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using Common.Logging;


namespace SecuritySwitch.Redirection {
	/// <summary>
	/// A factory for ILocationRedirector.
	/// </summary>
	public static class LocationRedirectorFactory {
		private static readonly ILog _log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Gets the default location redirector.
		/// </summary>
		/// <returns></returns>
		internal static ILocationRedirector Create() {
			_log.Debug(m => m("Creating LocationRedirector."));
			return new LocationRedirector();
		}
	}
}