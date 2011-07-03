// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using System.Collections.Specialized;

using Moq;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;

using Xunit;


namespace SecuritySwitch.Tests.Evaluation {
	public class SecurityEvaluatorTests {
		[Fact]
		public void IsSecureConnectionReturnsTrueIfRequestIndicatesSecurity() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.IsSecureConnection).Returns(true);

			var settings = new Settings();
			var evaluator = new SecurityEvaluator();

			// Act.
			var result = evaluator.IsSecureConnection(mockRequest.Object, settings);

			// Assert.
			Assert.True(result);
		}

		[Fact]
		public void IsSecureConnectionReturnsTrueIfRequestIndicatesNoSecurityYetHeaderMatchesAnOffloadHeader() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.IsSecureConnection).Returns(false);
			mockRequest.SetupGet(req => req.Headers).Returns(new NameValueCollection {
				{ "SOME_HEADER", "some-value" },
				{ "SSL_REQUEST", "on" }
			});

			var settings = new Settings();
			var evaluator = new SecurityEvaluator();

			// Act.
			settings.OffloadedSecurityHeaders = "SSL_REQUEST=on";
			var resultWithHeaderValueMatch = evaluator.IsSecureConnection(mockRequest.Object, settings);

			settings.OffloadedSecurityHeaders = "SSL_REQUEST=";
			var resultWithJustHeaderPresent = evaluator.IsSecureConnection(mockRequest.Object, settings);

			// Assert.
			Assert.True(resultWithHeaderValueMatch);
			Assert.True(resultWithJustHeaderPresent);
		}

		[Fact]
		public void IsSecureConnectionReturnsFalseIfRequestIndicatesNoSecurityAndNoHeaderMatchesAnOffloadHeader() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.IsSecureConnection).Returns(false);
			mockRequest.SetupGet(req => req.Headers).Returns(new NameValueCollection {
				{ "SOME_HEADER", "some-value" }
			});

			var settings = new Settings {
				OffloadedSecurityHeaders = "SSL_REQUEST=on"
			};

			var evaluator = new SecurityEvaluator();

			// Act.
			var result = evaluator.IsSecureConnection(mockRequest.Object, settings);

			// Assert.
			Assert.False(result);
		}

		[Fact]
		public void IsSecureConnectionReturnsFalseIfRequestIndicatesNoSecurityAndNoOffloadHeadersSpecified() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.IsSecureConnection).Returns(false);

			var settings = new Settings();
			var evaluator = new SecurityEvaluator();

			// Act.
			var result = evaluator.IsSecureConnection(mockRequest.Object, settings);

			// Assert.
			Assert.False(result);
		}
	}
}