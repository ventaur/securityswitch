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


namespace SecuritySwitch {
	public abstract class ContextCachedFactoryBase<TFactory, TCreation> 
		where TFactory : class, new()
		where TCreation : class {
		
		[ThreadStatic]
		private static TFactory _instance;

		
		protected abstract string CacheKey { get; }

		protected TCreation GetCacheValue(HttpContextBase context) {
			return context.Items[CacheKey] as TCreation;
		}

		protected TCreation SetCacheValue(TCreation value, HttpContextBase context) {
			context.Items[CacheKey] = value;
			return value;
		}


		public static TFactory Instance {
			get { return _instance ?? (_instance = new TFactory()); }
		}
	}
}