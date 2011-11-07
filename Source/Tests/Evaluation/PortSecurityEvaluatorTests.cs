// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using System;

using Moq;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;

using Xunit;


namespace SecuritySwitch.Tests.Evaluation {
	public class PortSecurityEvaluatorTests {
		[Fact]
		public void IsSecureConnectionReturnsTrueOnlyIfPortMatchesSecurityPort() {
			// Arrange.
			var uri = new Uri("http://www.mysite.com:81/admin/");
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.Url).Returns(uri);

			var settings = new Settings();
			var evaluator = new PortSecurityEvaluator();

			// Act.
			settings.SecurityPort = 80;
			var resultWithPortMisMatch = evaluator.IsSecureConnection(mockRequest.Object, settings);

			settings.SecurityPort = 81;
			var resultWithPortMatch = evaluator.IsSecureConnection(mockRequest.Object, settings);

			// Assert.
			Assert.False(resultWithPortMisMatch);
			Assert.True(resultWithPortMatch);
		}
	}
}