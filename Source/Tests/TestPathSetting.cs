// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Tests {
	/// <summary>
	/// A PathSetting used for tests.
	/// </summary>
	public class TestPathSetting : PathSetting {
		public TestPathSetting(string path, PathMatchType matchType, bool ignoreCase, RequestSecurity security) {
			Path = path;
			MatchType = matchType;
			IgnoreCase = ignoreCase;
			Security = security;
		}

		public TestPathSetting(string path) : this(path, PathMatchType.StartsWith, true, RequestSecurity.Secure) {}
	}
}