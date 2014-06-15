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
using SecuritySwitch.Evaluation;

using Xunit;


namespace SecuritySwitch.Tests.Evaluation {
	public class StandardSecurityEvaluatorTests {
		[Fact]
		public void IsSecureConnectionReturnsTrueIfRequestIndicatesSecurity() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.IsSecureConnection).Returns(true);

			var settings = new Settings();
			var evaluator = new StandardSecurityEvaluator();

			// Act.
			var result = evaluator.IsSecureConnection(mockRequest.Object, settings);

			// Assert.
			Assert.True(result);
		}
	}
}