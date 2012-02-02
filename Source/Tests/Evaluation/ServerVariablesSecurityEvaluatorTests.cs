using System.Collections.Specialized;

using Moq;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;

using Xunit;


namespace SecuritySwitch.Tests.Evaluation {
	public class ServerVariablesSecurityEvaluatorTests {
		[Fact]
		public void IsSecureConnectionReturnsTrueIfHeaderMatchesAnOffloadHeader() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.ServerVariables).Returns(new NameValueCollection {
				{ "SOME_VARIABLE", "some-value" },
				{ "HTTP_X_FORWARD_PROTOCOL", "HTTPS" }
			});

			var settings = new Settings();
			var evaluator = new ServerVariablesSecurityEvaluator();

			// Act.
			settings.OffloadedSecurityServerVariables = "HTTP_X_FORWARD_PROTOCOL=HTTPS";
			var resultWithHeaderValueMatch = evaluator.IsSecureConnection(mockRequest.Object, settings);

			settings.OffloadedSecurityServerVariables = "HTTP_X_FORWARD_PROTOCOL=";
			var resultWithJustHeaderPresent = evaluator.IsSecureConnection(mockRequest.Object, settings);

			// Assert.
			Assert.True(resultWithHeaderValueMatch);
			Assert.True(resultWithJustHeaderPresent);
		}

		[Fact]
		public void IsSecureConnectionReturnsFalseIfNoHeaderMatchesAnOffloadHeader() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.ServerVariables).Returns(new NameValueCollection {
				{ "SOME_VARIABLE", "some-value" }
			});

			var settings = new Settings {
				OffloadedSecurityServerVariables = "HTTP_X_FORWARD_PROTOCOL=HTTPS"
			};

			var evaluator = new ServerVariablesSecurityEvaluator();

			// Act.
			var result = evaluator.IsSecureConnection(mockRequest.Object, settings);

			// Assert.
			Assert.False(result);
		}
	}
}