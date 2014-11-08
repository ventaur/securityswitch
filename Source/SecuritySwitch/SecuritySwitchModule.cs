// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Web;
using System.Web.Configuration;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch {
	/// <summary>
	/// Evaluates each request for the need to switch to HTTP/HTTPS.
	/// </summary>
	public class SecuritySwitchModule : IHttpModule {
		private Settings _settings;
		private RequestProcessor _requestProcessor;


		/// <summary>
		/// Raised before the SecureSwitchModule evaluates the current request to allow subscribers a chance to evaluate the request.
		/// </summary>
		public event EvaluateRequestEventHandler EvaluateRequest;


		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">
		/// An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to 
		/// all application objects within an ASP.NET application.
		/// </param>
		public void Init(HttpApplication context) {
			if (context == null) {
				Logger.Log("No HttpApplication supplied.", Logger.LogLevel.Warn);
				return;
			}

			Logger.Log("Begin module initialization.");

			// Get the settings for the securitySwitch section.
			Logger.Log("Getting securitySwitch configuration section.", Logger.LogLevel.Info);
			_settings = WebConfigurationManager.GetSection("securitySwitch") as Settings;
			if (_settings == null || _settings.Mode == Mode.Off) {
				Logger.LogFormat("{0}; module not activated.", Logger.LogLevel.Info, _settings == null ? "No settings provided" : "Mode is Off");
				return;
			}

			Logger.Log("Creating RequestProcessor.");
			_requestProcessor = new RequestProcessor(_settings);

			// Hook the application's AcquireRequestState event.
			// * This ensures that the session ID is available for cookie-less session processing.
			// * I would rather hook sooner into the pipeline, but...
			// * It just is not possible (that I know of) to get the original URL requested when cookie-less sessions are used.
			//   The Framework uses RewritePath when the HttpContext is created to strip the Session ID from the request's 
			//   Path/Url. The rewritten URL is actually stored in an internal field of HttpRequest; short of reflection, 
			//   it's not obtainable.
			// WARNING: Do not access the Form collection of the HttpRequest object to avoid weird issues with post-backs from the application root.
			Logger.Log("Adding handler for the application's 'AcquireRequestState' event.");
			context.AcquireRequestState += ProcessRequest;

			Logger.Log("End module initialization.");
		}

		/// <summary>
		/// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
		/// </summary>
		public void Dispose() {
			Logger.Log("Dispose: Module disposing.");
		}


		/// <summary>
		/// Raises the EvaluateRequest event and returns any result for the expected security.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected RequestSecurity? EvaluatorCallback(HttpContextBase context) {
			Logger.Log("Raising the EvaluateRequest event.", Logger.LogLevel.Info);
			var eventArgs = new EvaluateRequestEventArgs(context, _settings);
			InvokeEvaluateRequest(eventArgs);

			return eventArgs.ExpectedSecurity;
		}


		/// <summary>
		/// Processes the request, evaluating it for the need to redirect based on configured settings.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void ProcessRequest(object sender, EventArgs e) {
			// Cast the source as an HttpApplication instance.
			var application = sender as HttpApplication;
			if (application == null) {
				Logger.Log("No HttpApplication supplied.", Logger.LogLevel.Warn);
				return;
			}

			// Wrap the application's context (for testability) and process the request.
			var context = new HttpContextWrapper(application.Context);
			_requestProcessor.Process(context, EvaluatorCallback);
		}

		/// <summary>
		/// Raises the EvaluateRequest event.
		/// </summary>
		/// <param name="args">The EvaluateRequestEventArgs used by any event handler(s).</param>
		private void InvokeEvaluateRequest(EvaluateRequestEventArgs args) {
			var handler = EvaluateRequest;
			if (handler != null) {
				handler(this, args);
			}
		}
	}


	/// <summary>
	/// The delegate for handlers of the EvaluateRequest event.
	/// </summary>
	public delegate void EvaluateRequestEventHandler(object sender, EvaluateRequestEventArgs args);
}