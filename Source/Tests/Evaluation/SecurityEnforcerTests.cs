// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Collections.Specialized;

using Moq;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;

using Xunit;


namespace SecuritySwitch.Tests.Evaluation {
	public class SecurityEnforcerTests {
		[Fact]
		public void GetUriRequestReturnsNullIfRequestSecurityAlreadyMatchesSpecifiedSecurity() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			var mockResponse = new Mock<HttpResponseBase>();
			var settings = new Settings();
			var evaluator = new SecurityEvaluator();
			var enforcer = new SecurityEnforcer(evaluator);

			// Act.
			mockRequest.SetupGet(req => req.IsSecureConnection).Returns(true);
			var targetUrlForAlreadySecuredRequest = enforcer.GetUriForMatchedSecurityRequest(mockRequest.Object,
			                                                                                 mockResponse.Object,
			                                                                                 RequestSecurity.Secure,
			                                                                                 settings);

			mockRequest.SetupGet(req => req.IsSecureConnection).Returns(false);
			var targetUrlForAlreadyInsecureRequest = enforcer.GetUriForMatchedSecurityRequest(mockRequest.Object,
			                                                                                  mockResponse.Object,
			                                                                                  RequestSecurity.Insecure,
			                                                                                  settings);


			// Assert.
			Assert.Null(targetUrlForAlreadySecuredRequest);
			Assert.Null(targetUrlForAlreadyInsecureRequest);
		}

		[Fact]
		public void GetUriRequestReturnsNullIfOffloadedHeaderSecurityAlreadyMatchesSpecifiedSecurity() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.IsSecureConnection).Returns(false);

			var mockResponse = new Mock<HttpResponseBase>();
			var settings = new Settings();
			var evaluator = new SecurityEvaluator();
			var enforcer = new SecurityEnforcer(evaluator);

			// Act.
			mockRequest.SetupGet(req => req.Headers).Returns(new NameValueCollection {
				{ "SSL_REQUEST", "on" },
				{ "OTHER_HEADER", "some-value" }
			});
			settings.OffloadedSecurityHeaders = "SSL_REQUEST=";
			var targetUrlForAlreadySecuredRequest = enforcer.GetUriForMatchedSecurityRequest(mockRequest.Object,
																							 mockResponse.Object,
																							 RequestSecurity.Secure,
																							 settings);

			mockRequest.SetupGet(req => req.Headers).Returns(new NameValueCollection {
				{ "OTHER_HEADER", "some-value" }
			});
			var targetUrlForAlreadyInsecureRequest = enforcer.GetUriForMatchedSecurityRequest(mockRequest.Object,
																							  mockResponse.Object,
																							  RequestSecurity.Insecure,
																							  settings);


			// Assert.
			Assert.Null(targetUrlForAlreadySecuredRequest);
			Assert.Null(targetUrlForAlreadyInsecureRequest);
		}

		[Fact]
		public void GetUriReturnsTheRequestUrlWithProtocolReplacedWhenNoBaseUriIsSupplied() {
			// Arrange.
			const string BaseRequestUri = "http://www.testsite.com";
			const string PathRequestUri = "/Manage/Default.aspx?Param=SomeValue";

			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.Url).Returns(new Uri(BaseRequestUri + PathRequestUri));
			mockRequest.SetupGet(req => req.RawUrl).Returns(PathRequestUri);

			var mockResponse = new Mock<HttpResponseBase>();
			mockResponse.Setup(resp => resp.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

			var settings = new Settings {
				Mode = Mode.On,
				Paths = {
					new TestPathSetting("/Manage")
				}
			};
			var evaluator = new SecurityEvaluator();
			var enforcer = new SecurityEnforcer(evaluator);

			// Act.
			var targetUrl = enforcer.GetUriForMatchedSecurityRequest(mockRequest.Object,
			                                                         mockResponse.Object,
			                                                         RequestSecurity.Secure,
			                                                         settings);

			// Assert.
			Assert.Equal(BaseRequestUri.Replace("http://", "https://") + PathRequestUri, targetUrl);
		}

		[Fact]
		public void GetUriReturnsSwitchedUriBasedOnSuppliedBaseSecureUri() {
			const string BaseRequestUri = "http://www.testsite.com";
			const string PathRequestUri = "/Manage/Default.aspx";
			const string QueryRequestUri = "?Param=SomeValue";

			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.ApplicationPath).Returns("/");
			mockRequest.SetupGet(req => req.Url).Returns(new Uri(BaseRequestUri + PathRequestUri + QueryRequestUri));
			mockRequest.SetupGet(req => req.RawUrl).Returns(PathRequestUri + QueryRequestUri);

			var mockResponse = new Mock<HttpResponseBase>();
			mockResponse.Setup(resp => resp.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

			var settings = new Settings {
				Mode = Mode.On,
				BaseSecureUri = "https://secure.someotherwebsite.com/testsite/"
			};
			var evaluator = new SecurityEvaluator();
			var enforcer = new SecurityEnforcer(evaluator);

			// Act.
			var targetUrl = enforcer.GetUriForMatchedSecurityRequest(mockRequest.Object,
																	 mockResponse.Object,
																	 RequestSecurity.Secure,
																	 settings);

			// Assert.
			Assert.Equal(settings.BaseSecureUri + PathRequestUri.Remove(0, 1) + QueryRequestUri, targetUrl);
		}

		[Fact]
		public void GetUriReturnsSwitchedUriBasedOnSuppliedBaseInsecureUri() {
			const string BaseRequestUri = "https://www.testsite.com";
			const string PathRequestUri = "/Info/Default.aspx";
			const string QueryRequestUri = "?Param=SomeValue";

			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.ApplicationPath).Returns("/");
			mockRequest.SetupGet(req => req.Url).Returns(new Uri(BaseRequestUri + PathRequestUri + QueryRequestUri));
			mockRequest.SetupGet(req => req.RawUrl).Returns(PathRequestUri + QueryRequestUri);
			mockRequest.SetupGet(req => req.IsSecureConnection).Returns(true);

			var mockResponse = new Mock<HttpResponseBase>();
			mockResponse.Setup(resp => resp.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

			var settings = new Settings {
				Mode = Mode.On,
				BaseInsecureUri = "http://www.someotherwebsite.com/"
			};
			var evaluator = new SecurityEvaluator();
			var enforcer = new SecurityEnforcer(evaluator);

			// Act.
			var targetUrl = enforcer.GetUriForMatchedSecurityRequest(mockRequest.Object,
																	 mockResponse.Object,
																	 RequestSecurity.Insecure,
																	 settings);

			// Assert.
			Assert.Equal(settings.BaseInsecureUri + PathRequestUri.Remove(0, 1) + QueryRequestUri, targetUrl);
		}

		[Fact]
		public void GetUriDoesNotIncludeApplicationPathWithSuppliedBaseUri() {
			const string BaseRequestUri = "http://www.testsite.com";
			const string ApplicationPathRequestUri = "/MySuperDuperApplication";
			const string PathRequestUri = ApplicationPathRequestUri + "/Manage/Default.aspx";
			const string QueryRequestUri = "?Param=SomeValue";

			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.ApplicationPath).Returns(ApplicationPathRequestUri);
			mockRequest.SetupGet(req => req.Url).Returns(new Uri(BaseRequestUri + PathRequestUri + QueryRequestUri));
			mockRequest.SetupGet(req => req.RawUrl).Returns(PathRequestUri + QueryRequestUri);

			var mockResponse = new Mock<HttpResponseBase>();
			mockResponse.Setup(resp => resp.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

			var settings = new Settings {
				Mode = Mode.On,
				BaseSecureUri = "https://secure.someotherwebsite.com/testsite/"
			};
			var evaluator = new SecurityEvaluator();
			var enforcer = new SecurityEnforcer(evaluator);

			// Act.
			var targetUrl = enforcer.GetUriForMatchedSecurityRequest(mockRequest.Object,
																	 mockResponse.Object,
																	 RequestSecurity.Secure,
																	 settings);

			// Assert.
			Assert.Equal(settings.BaseSecureUri + PathRequestUri.Remove(0, ApplicationPathRequestUri.Length + 1) + QueryRequestUri, targetUrl);
		}
	}
}