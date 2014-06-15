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
	/// Settings for tests.
	/// </summary>
	public class TestSettings : Settings {
		public void CallPostDeserialize() {
			base.PostDeserialize();
		}
	}
}