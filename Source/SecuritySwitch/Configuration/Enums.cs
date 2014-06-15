// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
namespace SecuritySwitch.Configuration {
	/// <summary>
	/// The different modes the module works in.
	/// </summary>
	public enum Mode {
		/// <summary>
		/// Indicates that the module is "on" and all requests should be evaluated.
		/// </summary>
		On,

		/// <summary>
		/// Only remote requests will be evaluated.
		/// </summary>
		/// <remarks>This is the default for the securitySwitch settings if its mode is not specified.</remarks>
		RemoteOnly,

		/// <summary>
		/// Only local requests will be evaluated.
		/// </summary>
		LocalOnly,

		/// <summary>
		/// The module is "off" and no evaluation should occur.
		/// </summary>
		Off
	}

	/// <summary>
	/// The possible types to match request paths by.
	/// </summary>
	public enum PathMatchType {
		/// <summary>
		/// An exact match with a request path is needed for action to be considered.
		/// </summary>
		Exact,

		/// <summary>
		/// The start of request paths are matched against (e.g. ~/Administration/).
		/// All requests with a path that start with the specified path to match will have action taken.
		/// </summary>
		/// <remarks>This is the default for a PathSetting that does not specify its matchType.</remarks>
		StartsWith,

		/// <summary>
		/// A regular expression is used to match any request paths for action.
		/// </summary>
		Regex

		// * Perhaps the Smart type will match a full path and any part of the query string provided without requiring 
		// * all params and the exact order of each.
		//Smart
	}

	/// <summary>
	/// Indicates the type of security to apply to a matched request.
	/// </summary>
	public enum RequestSecurity {
		/// <summary>
		/// The request should be made secure, if necessary.
		/// </summary>
		/// <remarks>This is the default for a PathSetting that does not specify its security.</remarks>
		Secure,

		/// <summary>
		/// The request should be made insecure, if necessary.
		/// </summary>
		Insecure,

		/// <summary>
		/// The request should be ignored.
		/// </summary>
		Ignore
	}
}