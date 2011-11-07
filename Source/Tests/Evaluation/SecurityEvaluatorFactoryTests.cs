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
	public class SecurityEvaluatorFactoryTests {
		[Fact]
		public void CreateReturnsPortSecurityEvaluatorIfSecurityPortHasValue() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			var settings = new Settings {
				SecurityPort = 81
			};

			// Act.
			var evaluator = SecurityEvaluatorFactory.Create(mockRequest.Object, settings);

			// Assert.
			Assert.IsType<PortSecurityEvaluator>(evaluator);
		}

		[Fact]
		public void CreateReturnsHeadersSecurityEvaluatorIfSecurityPortNotSpecifiedAndHeadersExpectedAndPresent() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.Headers).Returns(new NameValueCollection());

			var settings = new Settings {
				OffloadedSecurityHeaders = "SSL="
			};

			// Act.
			var evaluator = SecurityEvaluatorFactory.Create(mockRequest.Object, settings);

			// Assert.
			Assert.IsType<HeadersSecurityEvaluator>(evaluator);
		}

		[Fact]
		public void CreateReturnsStandardSecurityEvaluatorIfSecurityPortNotSpecifiedAndHeadersNotExpected() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			var settings = new Settings();

			// Act.
			var evaluator = SecurityEvaluatorFactory.Create(mockRequest.Object, settings);

			// Assert.
			Assert.IsType<StandardSecurityEvaluator>(evaluator);
		}
	}
}