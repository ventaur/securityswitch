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