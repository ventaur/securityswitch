// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;

using Common.Logging;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// The default implementation of IRequestEvaluator.
	/// </summary>
	public class RequestEvaluator : IRequestEvaluator {
		public const string XRequestedWithHeaderKey = "X-Requested-With";
		public const string AjaxRequestHeaderValue = "XMLHttpRequest";

		private static readonly ILog _log = LogManager.GetLogger<RequestEvaluator>();

		/// <summary>
		/// Evaluates the specified request for the need to switch its security.
		/// </summary>
		/// <param name="request">The request to evaluate.</param>
		/// <param name="settings">The settings to use for evaluation.</param>
		/// <return>
		/// A RequestSecurity value indicating the security the evaluated request should be under.
		/// </return>
		public RequestSecurity Evaluate(HttpRequestBase request, Settings settings) {
			// Test if the request matches the configured mode.
			if (!RequestMatchesMode(request, settings.Mode)) {
				_log.Debug(m => m("Request does not match mode and should be ignored."));
				return RequestSecurity.Ignore;
			}

			if (settings.IgnoreAjaxRequests && IsAjaxRequest(request)) {
				_log.Debug(m => m("Request is an AJAX request that should be ignored."));
				return RequestSecurity.Ignore;
			}

			// Find any matching path setting for the request.
			_log.Debug(m => m("Checking for a matching path for this request..."));
			string requestPath = request.RawUrl;
			foreach (PathSetting pathSetting in settings.Paths) {
				// Get an appropriate path matcher and test the request's path for a match.
				IPathMatcher matcher = PathMatcherFactory.Create(pathSetting.MatchType);
				if (matcher.IsMatch(requestPath, pathSetting.Path, pathSetting.IgnoreCase)) {
					_log.Debug(m => m("Matching path found; security is {0}.", pathSetting.Security));
					return pathSetting.Security;
				}
			}

			// Any non-matching request should default to Insecure.
			_log.Debug(m => m("No matching path found; security defaults to Insecure."));
			return RequestSecurity.Insecure;
		}


		/// <summary>
		/// Determines whether or not a request is an AJAX request.
		/// </summary>
		/// <param name="request">The request to test.</param>
		/// <returns>
		///   <c>true</c> if the specified request is an AJAX request; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsAjaxRequest(HttpRequestBase request) {
			if (request == null) {
				throw new ArgumentNullException("request");
			}

			// * Avoid accessing HttpRequestBase.Form at this point. There is a weird issue that forces the Framework to ignore the target of a
			//   post-back if you access the Form collection during a request to the root of the application (i.e. http://mydomain.com/).
			//   This issue does not appear if an actual page name is used in the URL (i.e. http://mydomain.com/Default.aspx).
			return (request.QueryString != null && request.QueryString[XRequestedWithHeaderKey] == AjaxRequestHeaderValue ||
			        request.Headers != null && request.Headers[XRequestedWithHeaderKey] == AjaxRequestHeaderValue);
		}

		/// <summary>
		/// Tests the given request to see if it matches the specified mode.
		/// </summary>
		/// <param name="request">An HttpRequestBase to test.</param>
		/// <param name="mode">The Mode used for the test.</param>
		/// <returns>
		///		Returns true if the request matches the mode as follows:
		///		<list type="disc">
		///			<item>If mode is On.</item>
		///			<item>If mode is set to RemoteOnly and the request is from a computer other than the server.</item>
		///			<item>If mode is set to LocalOnly and the request is from the server.</item>
		///		</list>
		///	</returns>
		private static bool RequestMatchesMode(HttpRequestBase request, Mode mode) {
			switch (mode) {
				case Mode.On:
					return true;

				case Mode.RemoteOnly:
					return !request.IsLocal;

				case Mode.LocalOnly:
					return request.IsLocal;

				default:
					return false;
			}
		}
	}
}