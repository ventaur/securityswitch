// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System.Collections.Generic;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;
using SecuritySwitch.Redirection;
using SecuritySwitch.ResponseEnrichers;


namespace SecuritySwitch {
	public class RequestProcessor {
		private readonly Settings _settings;


		public delegate RequestSecurity? RequestEvaluatorCallback(HttpContextBase context);

		public RequestProcessor(Settings settings) {
			_settings = settings;
		}


		/// <summary>
		/// Processes a request.
		/// </summary>
		/// <param name="context">The context in which the request to process is running.</param>
		/// <param name="evaluatorCallback">A callback to a custom request evaluator.</param>
		public void Process(HttpContextBase context, RequestEvaluatorCallback evaluatorCallback) {
			Logger.Log("Begin request processing.");

			HttpRequestBase request = context.Request;
			HttpResponseBase response = context.Response;
			ISecurityEvaluator securityEvaluator = SecurityEvaluatorFactory.Create(request, _settings);

			RequestSecurity expectedSecurity = EvaluateRequestViaCallbackOrEvaluator(context, request, evaluatorCallback);
			if (expectedSecurity == RequestSecurity.Ignore) {
				// No redirect is needed for a result of Ignore.
				EnrichResponse(response, request, securityEvaluator, _settings);
				Logger.Log("Expected security is Ignore; done.", Logger.LogLevel.Info);
				return;
			}

			string targetUrl = DetermineTargetUrl(securityEvaluator, request, response, expectedSecurity);
			if (string.IsNullOrEmpty(targetUrl)) {
				// No redirect is needed for a null/empty target URL.
				EnrichResponse(response, request, securityEvaluator, _settings);
				Logger.Log("No target URI determined; done.", Logger.LogLevel.Info);
				return;
			}

			Redirect(response, targetUrl);
		}


		/// <summary>
		/// Evaluates this request via any request evaluator callback or an IRequestEvaluator.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="request"></param>
		/// <param name="evaluatorCallback">A callback to a custom request evaluator.</param>
		/// <returns></returns>
		private RequestSecurity EvaluateRequestViaCallbackOrEvaluator(HttpContextBase context, HttpRequestBase request, RequestEvaluatorCallback evaluatorCallback) {
			RequestSecurity? evaluatorSecurity = null;
			if (evaluatorCallback != null) {
				evaluatorSecurity = evaluatorCallback(context);
			}

			RequestSecurity expectedSecurity;
			if (evaluatorSecurity.HasValue) {
				// Use the value returned by the EvaluateRequest event.
				Logger.Log("Using the expected security value provided by the RequestEvaluatorCallback.", Logger.LogLevel.Info);
				expectedSecurity = evaluatorSecurity.Value;
			} else {
				// Evaluate this request with the configured settings, if necessary.
				IRequestEvaluator requestEvaluator = RequestEvaluatorFactory.Create();
				expectedSecurity = requestEvaluator.Evaluate(request, _settings);
			}
			return expectedSecurity;
		}

		/// <summary>
		/// Enriches the response as needed, based on the expected security and settings.
		/// </summary>
		/// <param name="response"></param>
		/// <param name="securityEvaluator"></param>
		/// <param name="settings"></param>
		/// <param name="request"></param>
		private void EnrichResponse(HttpResponseBase response, HttpRequestBase request, ISecurityEvaluator securityEvaluator, Settings settings) {
			IList<IResponseEnricher> enrichers = ResponseEnricherFactory.GetAll();
			if (enrichers == null) {
				return;
			}

			foreach (var enricher in enrichers) {
				enricher.Enrich(response, request, securityEvaluator, settings);
			}
		}

		/// <summary>
		/// Determines a target URL (if any) for this request, based on the expected security.
		/// </summary>
		/// <param name="securityEvaluator"></param>
		/// <param name="request"></param>
		/// <param name="response"></param>
		/// <param name="expectedSecurity"></param>
		/// <returns></returns>
		private string DetermineTargetUrl(ISecurityEvaluator securityEvaluator, HttpRequestBase request, HttpResponseBase response, RequestSecurity expectedSecurity) {
			// Ensure the request matches the expected security.
			Logger.Log("Determining the URI for the expected security.", Logger.LogLevel.Info);
			ISecurityEnforcer securityEnforcer = SecurityEnforcerFactory.Create(securityEvaluator);
			string targetUrl = securityEnforcer.GetUriForMatchedSecurityRequest(request, response, expectedSecurity, _settings);
			return targetUrl;
		}

		/// <summary>
		/// Responds with a redirect to the target URL.
		/// </summary>
		/// <param name="response"></param>
		/// <param name="targetUrl"></param>
		private void Redirect(HttpResponseBase response, string targetUrl) {
			// Redirect.
			Logger.Log("Redirecting the request.", Logger.LogLevel.Info);
			ILocationRedirector redirector = LocationRedirectorFactory.Create();
			redirector.Redirect(response, targetUrl, _settings.BypassSecurityWarning);
		}
	}
}