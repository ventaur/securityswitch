using Moq;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;
using SecuritySwitch.ResponseEnrichers;

using Xunit;


namespace SecuritySwitch.Tests.ResponseEnrichers {
	public class HstsResponseEnricherTests {
		[Fact]
		public void EnrichAddsHstsHeaderWithMaxAge() {
			const int HstsMaxAge = 42;

			var mockResponse = new Mock<HttpResponseBase>();
			var mockRequest = new Mock<HttpRequestBase>();
			var mockSecurityEvaluator = new Mock<ISecurityEvaluator>();
			var enricher = new HstsResponseEnricher();

			var settings = new Settings {
				EnableHsts = true,
				HstsMaxAge = HstsMaxAge
			};
			mockSecurityEvaluator.Setup(e => e.IsSecureConnection(mockRequest.Object, settings)).Returns(true);

			enricher.Enrich(mockResponse.Object, mockRequest.Object, mockSecurityEvaluator.Object, settings);

			mockResponse.Verify(resp => resp.AddHeader("Strict-Transport-Security", string.Format("max-age={0:f0}", HstsMaxAge)));
		}

		[Fact]
		public void EnrichDoesntAddHeaderIfRequestNotSecure() {
			var mockResponse = new Mock<HttpResponseBase>();
			var mockRequest = new Mock<HttpRequestBase>();
			var mockSecurityEvaluator = new Mock<ISecurityEvaluator>();
			var enricher = new HstsResponseEnricher();

			var settings = new Settings {
				EnableHsts = true,
				HstsMaxAge = 42
			};
			mockSecurityEvaluator.Setup(e => e.IsSecureConnection(mockRequest.Object, settings)).Returns(false);

			enricher.Enrich(mockResponse.Object, mockRequest.Object, mockSecurityEvaluator.Object, settings);

			mockResponse.Verify(resp => resp.AddHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
		}

		[Fact]
		public void EnrichDoesntAddHeaderIfHstsNotEnabled() {
			var mockResponse = new Mock<HttpResponseBase>();
			var mockRequest = new Mock<HttpRequestBase>();
			var mockSecurityEvaluator = new Mock<ISecurityEvaluator>();
			var enricher = new HstsResponseEnricher();

			var settings = new Settings {
				EnableHsts = false,
				HstsMaxAge = 42
			};
			mockSecurityEvaluator.Setup(e => e.IsSecureConnection(mockRequest.Object, settings)).Returns(true);

			enricher.Enrich(mockResponse.Object, mockRequest.Object, mockSecurityEvaluator.Object, settings);

			mockResponse.Verify(resp => resp.AddHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
		}
	}
}