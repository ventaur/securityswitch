// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using SecuritySwitch.Abstractions;


namespace SecuritySwitch.Evaluation {
	internal class RequestEvaluatorFactory : ContextCachedFactoryBase<RequestEvaluatorFactory, IRequestEvaluator> {
		protected override string CacheKey {
			get { return "SecuritySwitch.RequestEvaluator"; }
		}


		internal IRequestEvaluator Create(HttpContextBase context) {
			var evaluator = GetCacheValue(context);
			if (evaluator != null) {
				return evaluator;
			}

			Logger.Log("Creating RequestEvaluator.");
			return new RequestEvaluator();
		}
	}
}