// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Tests.Evaluation {
	/// <summary>
	/// A test fixture for RequestEvaluator tests.
	/// </summary>
	public class RequestEvaluatorTestFixture {
		/// <summary>
		/// Settings for testing ReequestEvaluator.
		/// </summary>
		public Settings Settings { get; set; }

		public RequestEvaluatorTestFixture() {
			var settings = new TestSettings {
				Mode = Mode.On,
				IgnoreSystemHandlers = true,
				Paths = {
					new TestPathSetting("/Info/ContactUs.aspx", PathMatchType.StartsWith, true, RequestSecurity.Insecure),
					new TestPathSetting("/Login.aspx"),
					new TestPathSetting("/Admin/ViewOrders.aspx"),

					new TestPathSetting("/info/contactus", PathMatchType.StartsWith, true, RequestSecurity.Insecure),
					new TestPathSetting("/login/"),
					new TestPathSetting("/admin/vieworders/"),

					new TestPathSetting("/Manage")
				}
			};
			settings.CallPostDeserialize();

			Settings = settings;
		}
	}
}