// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using System.Collections.Generic;

using Moq;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;

using Xunit;


namespace SecuritySwitch.Tests {
	public class SecuritySwitchModuleTests {
		[Fact]
		public void EvaluateRequestIsInvoked() {
			// Arrange.
			var mockContext = new Mock<HttpContextBase>();
			var mockRequest = new Mock<HttpRequestBase>();
			var mockResponse = new Mock<HttpResponseBase>();
			var items = new Dictionary<object, object>();
			mockContext.SetupGet(ctx => ctx.Items).Returns(items);
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
		public void ProcessRequest(HttpContextBase context) {
			var settings = new Settings();
			var requestProcessor = new RequestProcessor(settings);

			requestProcessor.Process(context, EvaluatorCallback);
		}
	}
}