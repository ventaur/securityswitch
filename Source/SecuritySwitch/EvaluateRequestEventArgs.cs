// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch {
	/// <summary>
	/// Arguments used for the SecuritySwitchModule.EvaluateRequest event.
	/// </summary>
	public class EvaluateRequestEventArgs : EventArgs {
		/// <summary>
		/// The context for the request to evaluate.
		/// </summary>
		public HttpContextBase Context { get; private set; }

		/// <summary>
		/// The expected security for an evaluated request.
		/// </summary>
		/// <remarks>
		/// Subscribers should set this property to a RequestSecurity value expected for the 
		/// request evaluated. If not null, the module will use this value to determine what to do.
		/// </remarks>
		public RequestSecurity? ExpectedSecurity { get; set; }

		/// <summary>
		/// The Settings used to evaluate the request.
		/// </summary>
		public Settings Settings { get; private set; }


		/// <summary>
		/// Creates an instance of EvaluateRequestEventArgs with the specified application and settings.
		/// </summary>
		/// <param name="context">The current context.</param>
		/// <param name="settings">An instance of Settings used for the evaluation of the request.</param>
		public EvaluateRequestEventArgs(HttpContextBase context, Settings settings) {
			ExpectedSecurity = null;
			Context = context;
			Settings = settings;
		}
	}
}