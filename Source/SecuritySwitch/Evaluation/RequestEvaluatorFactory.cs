// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
namespace SecuritySwitch.Evaluation {
	public static class RequestEvaluatorFactory {
		/// <summary>
		/// Gets a request evaluator.
		/// </summary>
		/// <returns></returns>
		public static IRequestEvaluator Create() {
			Logger.Log("Creating RequestEvaluator.");
			return new RequestEvaluator();
		}
	}
}