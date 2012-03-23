// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
namespace SecuritySwitch.Configuration {
	/// <summary>
	/// The element names used for the configuration section and its elements.
	/// </summary>
	internal sealed class ElementNames {
		internal const string BaseInsecureUri = "baseInsecureUri";
		internal const string BaseSecureUri = "baseSecureUri";
		internal const string BypassSecurityWarning = "bypassSecurityWarning";
		internal const string IgnoreAjaxRequests = "ignoreAjaxRequests";
		internal const string IgnoreSystemHandlers = "ignoreSystemHandlers";
		internal const string Mode = "mode";
		internal const string OffloadedSecurityHeaders = "offloadedSecurityHeaders";
		internal const string OffloadedSecurityServerVariables = "offloadedSecurityServerVariables";
		internal const string SecurityPort = "securityPort";

		internal const string Paths = "paths";
		
		internal const string IgnoreCase = "ignoreCase";
		internal const string MatchType = "matchType";
		internal const string Path = "path";
		internal const string Security = "security";
	}
}