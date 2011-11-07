// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using Common.Logging;

using SecuritySwitch.Abstractions;


namespace SecuritySwitch.Redirection {
	/// <summary>
	/// The default implementation of ILocationRedirector.
	/// </summary>
	public class LocationRedirector : ILocationRedirector {
		private readonly ILog _log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Redirects to the specified URL.
		/// </summary>
		/// <param name="response">The response to use for redirection.</param>
		/// <param name="url">The URL to redirect to.</param>
		/// <param name="bypassSecurityWarning">If set to <c>true</c> security warnings will be bypassed.</param>
		public void Redirect(HttpResponseBase response, string url, bool bypassSecurityWarning) {
			if (bypassSecurityWarning) {
				_log.Debug(m => m("Bypassing security warning via a response header and JavaScript."));

				// Clear the current response buffer.
				response.Clear();

				// Add a refresh header to the response for the new path.
				response.AddHeader("Refresh", "0;URL=" + url);

				// Also, add JavaScript to replace the current location as backup.
				response.Write("<html><head><title></title>");
				response.Write("<script language=\"javascript\">window.location = '");
				response.Write(url);
				response.Write("';</script>");
				response.Write("</head><body></body></html>");
			} else {
				_log.Debug(m => m("Issuing permanent redirect."));

				// Permanent redirect.
				// TODO: Make the status code configurable (i.e. permanent vs. temporary).
				response.StatusCode = 301;
				response.RedirectLocation = url;
			}

			// End the current response.
			response.End();
		}
	}
}