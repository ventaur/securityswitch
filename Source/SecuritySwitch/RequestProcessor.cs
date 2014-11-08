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
	internal class RequestProcessor {
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

			RequestSecurity expectedSecurity = EvaluateRequestViaCallbackOrEvaluator(context, evaluatorCallback);
			if (expectedSecurity == RequestSecurity.Ignore) {
				// No redirect is needed for a result of Ignore.
				EnrichResponse(context, _settings);
				Logger.Log("Expected security is Ignore; done.", Logger.LogLevel.Info);
				return;
			}

			string targetUrl = DetermineTargetUrl(context, expectedSecurity);
			if (string.IsNullOrEmpty(targetUrl)) {
				// No redirect is needed for a null/empty target URL.
				EnrichResponse(context, _settings);
				Logger.Log("No target URI determined; done.", Logger.LogLevel.Info);
				return;
			}

			Redirect(context, targetUrl);
		}


		/// <summary>
		/// Evaluates this request via any request evaluator callback or an IRequestEvaluator.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="evaluatorCallback">A callback to a custom request evaluator.</param>
		/// <returns></returns>
		private RequestSecurity EvaluateRequestViaCallbackOrEvaluator(HttpContextBase context, RequestEvaluatorCallback evaluatorCallback) {
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
				IRequestEvaluator requestEvaluator = RequestEvaluatorFactory.Instance.Create(context);
				expectedSecurity = requestEvaluator.Evaluate(context.Request, _settings);
			}
			return expectedSecurity;
		}

		/// <summary>
		/// Enriches the response as needed, based on the expected security and settings.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="settings"></param>
		private void EnrichResponse(HttpContextBase context, Settings settings) {
			IList<IResponseEnricher> enrichers = ResponseEnricherFactory.Instance.GetAll(context);
			if (enrichers == null) {
				return;
			}

			ISecurityEvaluator securityEvaluator = SecurityEvaluatorFactory.Instance.Create(context, _settings);
			foreach (var enricher in enrichers) {
				enricher.Enrich(context.Response, context.Request, securityEvaluator, settings);
			}
		}

		/// <summary>
		/// Determines a target URL (if any) for this request, based on the expected security.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="expectedSecurity"></param>
		/// <returns></returns>
		private string DetermineTargetUrl(HttpContextBase context, RequestSecurity expectedSecurity) {
			// Ensure the request matches the expected security.
			Logger.Log("Determining the URI for the expected security.", Logger.LogLevel.Info);
			ISecurityEvaluator securityEvaluator = SecurityEvaluatorFactory.Instance.Create(context, _settings);
			ISecurityEnforcer securityEnforcer = SecurityEnforcerFactory.Instance.Create(context, securityEvaluator);
			string targetUrl = securityEnforcer.GetUriForMatchedSecurityRequest(context.Request, context.Response, expectedSecurity, _settings);
			return targetUrl;
		}

		/// <summary>
		/// Responds with a redirect to the target URL.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="targetUrl"></param>
		private void Redirect(HttpContextBase context, string targetUrl) {
			// Redirect.
			Logger.Log("Redirecting the request.", Logger.LogLevel.Info);
			ILocationRedirector redirector = LocationRedirectorFactory.Instance.Create(context);
			redirector.Redirect(context.Response, targetUrl, _settings.BypassSecurityWarning);
		}
	}
}