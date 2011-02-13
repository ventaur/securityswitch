using System;
using System.Web;

using SecuritySwitch;


namespace ExampleWebSite {
	public class Global : HttpApplication {
		protected void Application_Start(object sender, EventArgs e) {}

		protected void Session_Start(object sender, EventArgs e) {}

		protected void Application_BeginRequest(object sender, EventArgs e) {}

		protected void Application_AuthenticateRequest(object sender, EventArgs e) {}

		protected void Application_Error(object sender, EventArgs e) {}

		protected void Session_End(object sender, EventArgs e) {}

		protected void Application_End(object sender, EventArgs e) {}

		protected void SecuritySwitch_BeforeEvaluateRequest(object sender, BeforeEvaluateRequestEventArgs e) {
			// Decide whether or not to cancel the SecuritySwitch module's evaluation of this request.

			// In this case, we are canceling the module's evaluation of this request if there is a query string value of "yes" for the "ignoreSecurity" parameter.
			// * The ignoreSecurity query string parameter is randomly set for pages under the Cms site map area.
			var ignoreSecurityParamValue = Request.QueryString["ignoreSecurity"];
			e.CancelEvaluation = (!string.IsNullOrEmpty(ignoreSecurityParamValue) &&
			                      ignoreSecurityParamValue.Equals("yes", StringComparison.OrdinalIgnoreCase));
		}
	}
}