// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using Moq;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Redirection;

using Xunit;


namespace SecuritySwitch.Tests.Redirection {
	public class LocationRedirectorTests {
		[Fact]
		public void RedirectWithoutBypassingSecurityWarningIssuesPermanentRedirect() {
			// Arrange.
			const string RedirectUrl = "https://www.somewebsite.com/admin/protected-content/";
			var mockResponse = new Mock<HttpResponseBase>();
			var redirector = new LocationRedirector();

			// Act.
			redirector.Redirect(mockResponse.Object, RedirectUrl, false);

			// Assert.
			mockResponse.VerifySet(resp => resp.StatusCode = 301);
			mockResponse.VerifySet(resp => resp.RedirectLocation = RedirectUrl);
			mockResponse.Verify(resp => resp.End());
		}

		[Fact]
		public void RedirectWithBypassSecurityWarningAddsCorrectRefreshHeader() {
			// Arrange.
			const string RedirectUrl = "https://www.somewebsite.com/admin/protected-content/?testVar=Value1&another=value-2";
			const string ExpectedUrl = @"https://www.somewebsite.com/admin/protected-content/?testVar\x3dValue1\x26another\x3dvalue-2";
			var mockResponse = new Mock<HttpResponseBase>();
			var redirector = new LocationRedirector();

			// Act.
			redirector.Redirect(mockResponse.Object, RedirectUrl, true);

			// Assert.
			mockResponse.Verify(resp => resp.AddHeader("Refresh", "0;URL=" + ExpectedUrl));
		}

		[Fact]
		public void RedirectWithBypassSecurityWarningWritesJavascriptLocationUrl() {
			// Arrange.
			const string RedirectUrl = "https://www.somewebsite.com/admin/protected-content/?testVar=Value1&another=value-2";
			const string ExpectedUrl = @"https://www.somewebsite.com/admin/protected-content/?testVar\x3dValue1\x26another\x3dvalue-2";
			var mockResponse = new Mock<HttpResponseBase>();
			var redirector = new LocationRedirector();

			// Act.
			redirector.Redirect(mockResponse.Object, RedirectUrl, true);

			// Assert.
			mockResponse.Verify(resp => resp.Write(It.Is<string>(s => s.Contains("window.location"))));
			mockResponse.Verify(resp => resp.Write(It.Is<string>(s => s.Contains(ExpectedUrl))));
		}
	}
}