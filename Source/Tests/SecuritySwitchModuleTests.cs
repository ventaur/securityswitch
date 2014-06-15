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
using SecuritySwitch.Configuration;

using Xunit;


namespace SecuritySwitch.Tests {
	public class SecuritySwitchModuleTests {
		[Fact]
		public void EvaluateRequestInvoked() {
			// Arrange.
			var mockContext = new Mock<HttpContextBase>();
			var mockRequest = new Mock<HttpRequestBase>();
			var mockResponse = new Mock<HttpResponseBase>();
			mockContext.SetupGet(ctx => ctx.Request).Returns(mockRequest.Object);
			mockContext.SetupGet(ctx => ctx.Response).Returns(mockResponse.Object);

			var module = new TestSecuritySwitchModule();
			var invoked = false;
			module.EvaluateRequest += (sender, args) => {
			                          	invoked = true;
			                          	args.ExpectedSecurity = RequestSecurity.Ignore;
			                          };

			// Act.
			module.ProcessRequest(mockContext.Object);

			// Assert.
			Assert.True(invoked);
		}
	}


	public class TestSecuritySwitchModule : SecuritySwitchModule {
		public new void ProcessRequest(HttpContextBase context) {
			base.ProcessRequest(context);
		}
	}
}