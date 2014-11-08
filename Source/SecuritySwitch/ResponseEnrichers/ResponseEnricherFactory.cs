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


namespace SecuritySwitch.ResponseEnrichers {
	internal class ResponseEnricherFactory : ContextCachedFactoryBase<ResponseEnricherFactory, IList<IResponseEnricher>> {
		protected override string CacheKey {
			get { return "SecuritySwitch.ResponseEnrichers"; }
		}

		internal IList<IResponseEnricher> GetAll(HttpContextBase context) {
			var cachedEnrichers = GetCacheValue(context);
			if (cachedEnrichers != null) {
				return cachedEnrichers;
			}

			Logger.Log("Creating all response enrichers.");
			Logger.Log("    - creating HstsResponseEnricher");
			var enrichers = new List<IResponseEnricher> {
				new HstsResponseEnricher()
			};
			return SetCacheValue(enrichers, context);
		}
	}
}