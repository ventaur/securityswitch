using System.Collections.Generic;
using System.Linq;

using Moq;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;
using SecuritySwitch.Redirection;
using SecuritySwitch.ResponseEnrichers;

using Xunit;


namespace SecuritySwitch.Tests {
	public class RequestProcessorTests : IUseFixture<RequestProcessorFixture> {
		private Settings _settings;
		private Mock<HttpContextBase> _mockContext;

		public void SetFixture(RequestProcessorFixture data) {
			_settings = data.Settings;
			_mockContext = data.MockContext;
		}


		[Fact]
		public void CallbackIsInvoked() {
			var wasCallbackInvoked = false;
			var sut = new RequestProcessor(_settings);
			sut.Process(_mockContext.Object,
				context => {
					wasCallbackInvoked = true;
					return RequestSecurity.Ignore;
				});

			Assert.True(wasCallbackInvoked);
		}

		[Fact]
		public void CallbackResultIsUsed() {
			var mockSecurityEnforcer = new Mock<ISecurityEnforcer>();
			var mockRequestEvaluator = new Mock<IRequestEvaluator>();
			mockRequestEvaluator.Setup(e => e.Evaluate(It.IsAny<HttpRequestBase>(), _settings)).Returns(RequestSecurity.Secure);
			
			var sut = new RequestProcessor(_settings);
			SecurityEnforcerFactory.Instance.ForcedCreation = mockSecurityEnforcer.Object;
			RequestEvaluatorFactory.Instance.ForcedCreation = mockRequestEvaluator.Object;
			
			sut.Process(_mockContext.Object, null);
			mockSecurityEnforcer.Verify(
				e =>
					e.GetUriForMatchedSecurityRequest(
						It.IsAny<HttpRequestBase>(),
						It.IsAny<HttpResponseBase>(),
						RequestSecurity.Secure,
						_settings), Times.Once());
			mockSecurityEnforcer.Verify(
				e =>
					e.GetUriForMatchedSecurityRequest(
						It.IsAny<HttpRequestBase>(),
						It.IsAny<HttpResponseBase>(),
						RequestSecurity.Insecure,
						_settings), Times.Never());

			sut.Process(_mockContext.Object, context => RequestSecurity.Insecure);
			mockSecurityEnforcer.Verify(
				e =>
					e.GetUriForMatchedSecurityRequest(
						It.IsAny<HttpRequestBase>(),
						It.IsAny<HttpResponseBase>(),
						RequestSecurity.Insecure,
						_settings), Times.Once());
		}

		[Fact]
		public void RedirectsToEnforcerUri() {
			const string TargetUri = "https://secure.somwehere.com/nice/";

			var mockSecurityEnforcer = new Mock<ISecurityEnforcer>();
			var mockLocationRedirector = new Mock<ILocationRedirector>();
			mockSecurityEnforcer.Setup(
				e =>
					e.GetUriForMatchedSecurityRequest(
						It.IsAny<HttpRequestBase>(),
						It.IsAny<HttpResponseBase>(),
						It.IsAny<RequestSecurity>(),
						_settings)).Returns(TargetUri);

			var sut = new RequestProcessor(_settings);
			SecurityEnforcerFactory.Instance.ForcedCreation = mockSecurityEnforcer.Object;
			LocationRedirectorFactory.Instance.ForcedCreation = mockLocationRedirector.Object;

			sut.Process(_mockContext.Object, null);

			mockLocationRedirector.Verify(r => r.Redirect(It.IsAny<HttpResponseBase>(), TargetUri, It.IsAny<bool>()));
		}

		[Fact]
		public void EnrichesResponseOnIgnore() {
			var mockEnrichers = new[] { new Mock<IResponseEnricher>(), new Mock<IResponseEnricher>() };

			var sut = new RequestProcessor(_settings);
			ResponseEnricherFactory.Instance.ForcedCreation = mockEnrichers.Select(e => e.Object);

			sut.Process(_mockContext.Object, context => RequestSecurity.Ignore);

			foreach (var mockEnricher in mockEnrichers) {
				mockEnricher.Verify(e => e.Enrich(It.IsAny<HttpResponseBase>(), It.IsAny<HttpRequestBase>(), It.IsAny<ISecurityEvaluator>(), _settings));
			}
		}

		[Fact]
		public void EnrichesResponseWhenNoRedirectNeeded() {
			var mockSecurityEnforcer = new Mock<ISecurityEnforcer>();
			var mockEnrichers = new[] { new Mock<IResponseEnricher>(), new Mock<IResponseEnricher>() };
			mockSecurityEnforcer.Setup(
				e =>
					e.GetUriForMatchedSecurityRequest(
						It.IsAny<HttpRequestBase>(),
						It.IsAny<HttpResponseBase>(),
						RequestSecurity.Secure,
						_settings)).Returns(string.Empty);

			var sut = new RequestProcessor(_settings);
			ResponseEnricherFactory.Instance.ForcedCreation = mockEnrichers.Select(e => e.Object);
			SecurityEnforcerFactory.Instance.ForcedCreation = mockSecurityEnforcer.Object;

			sut.Process(_mockContext.Object, context => RequestSecurity.Secure);

			foreach (var mockEnricher in mockEnrichers) {
				mockEnricher.Verify(e => e.Enrich(It.IsAny<HttpResponseBase>(), It.IsAny<HttpRequestBase>(), It.IsAny<ISecurityEvaluator>(), _settings));
			}
		}
	}


	public class RequestProcessorFixture {
		public Settings Settings { get; set; }
		public Mock<HttpContextBase> MockContext { get; set; }

		public RequestProcessorFixture() {
			Settings = new Settings();
			MockContext = new Mock<HttpContextBase>();
			var mockRequest = new Mock<HttpRequestBase>();
			var mockResponse = new Mock<HttpResponseBase>();
			var contextItems = new Dictionary<object, object>();

			MockContext.SetupGet(c => c.Request).Returns(mockRequest.Object);
			MockContext.SetupGet(c => c.Response).Returns(mockResponse.Object);
			MockContext.SetupGet(c => c.Items).Returns(contextItems);
		}
	}
}