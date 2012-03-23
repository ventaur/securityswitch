// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using Common.Logging;


namespace SecuritySwitch.Evaluation {
	public static class RequestEvaluatorFactory {
		private static readonly ILog _log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Gets a request evaluator.
		/// </summary>
		/// <returns></returns>
		public static IRequestEvaluator Create() {
			_log.Debug(m => m("Creating RequestEvaluator."));
			return new RequestEvaluator();
		}
	}
}