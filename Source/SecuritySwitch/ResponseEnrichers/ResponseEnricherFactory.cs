// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System.Collections.Generic;


namespace SecuritySwitch.ResponseEnrichers {
	public static class ResponseEnricherFactory {
		private static List<IResponseEnricher> _enrichers;

		public static IList<IResponseEnricher> GetAll() {
			if (_enrichers == null) {
				Logger.Log("Creating all response enrichers.");
				Logger.Log("    - creating HstsResponseEnricher");
				_enrichers = new List<IResponseEnricher> {
					new HstsResponseEnricher()
				};
			}

			return _enrichers;
		}
	}
}