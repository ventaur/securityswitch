// =================================================================================
// Copyright © 2009 Axiom Forge, Inc.
// All rights reserved.
// 
// This code and information is confidential! It is not to re-produced or shared
// with anyone outside of Axiom Forge unless express, written authorization has
// been issued by an officer of the company.
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
			var mockResponse = new Mock<HttpResponseBase>();
			var redirector = new LocationRedirector();

			// Act.
			redirector.Redirect(mockResponse.Object, RedirectUrl, true);

			// Assert.
			mockResponse.Verify(resp => resp.AddHeader("Refresh", "0;URL=" + RedirectUrl));
		}

		[Fact]
		public void RedirectWithBypassSecurityWarningWritesJavascriptLocationUrl() {
			// Arrange.
			const string RedirectUrl = "https://www.somewebsite.com/admin/protected-content/?testVar=Value1&another=value-2";
			var mockResponse = new Mock<HttpResponseBase>();
			var redirector = new LocationRedirector();

			// Act.
			redirector.Redirect(mockResponse.Object, RedirectUrl, true);

			// Assert.
			mockResponse.Verify(resp => resp.Write(It.Is<string>(s => s.Contains("window.location"))));
			mockResponse.Verify(resp => resp.Write(It.Is<string>(s => s.Contains(RedirectUrl))));
		}
	}
}