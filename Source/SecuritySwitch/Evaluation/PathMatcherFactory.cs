// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Collections.Generic;
using System.Web;

using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A factory for PathMatchers.
	/// </summary>
	public static class PathMatcherFactory {
		const string CachedMatchersKey = "SecuritySwitch.Matchers";

		/// <summary>
		/// Gets any cached path matchers.
		/// </summary>
		private static IDictionary<PathMatchType, IPathMatcher> CachedMatchers {
			get {
				// If a current HttpContext exists, use it for caching PathMatchers for the current request; otherwise, do not cache them.
				HttpContext currentContext = HttpContext.Current;
				if (currentContext == null) {
					return null;
				}

				// Retrieve any cached matchers for this request; otherwise create a container for them.
				var cachedMatchers = currentContext.Items[CachedMatchersKey] as IDictionary<PathMatchType, IPathMatcher>;
				if (cachedMatchers == null) {
					cachedMatchers = new Dictionary<PathMatchType, IPathMatcher>();
					currentContext.Items.Add(CachedMatchersKey, cachedMatchers);
				}

				return cachedMatchers;
			}
		}


		/// <summary>
		/// Creates an appropriate path matcher for the specified match type.
		/// </summary>
		/// <param name="matchType">The PathMatchType used to determine the appropriate path matcher.</param>
		/// <returns></returns>
		public static IPathMatcher Create(PathMatchType matchType) {
			// Check for cached matchers first.
			IDictionary<PathMatchType, IPathMatcher> cachedMatchers = CachedMatchers;
			if (cachedMatchers != null && cachedMatchers.ContainsKey(matchType)) {
				IPathMatcher cachedPathMatcher = cachedMatchers[matchType];
				Logger.LogFormat("Cached {0} retrieved.", cachedPathMatcher.GetType().Name);
				return cachedPathMatcher;
			}

			// Create the appropriate path matcher.
			IPathMatcher pathMatcher;
			switch (matchType) {
				case PathMatchType.Regex:
					pathMatcher = new RegexPathMatcher();
					break;
				
				case PathMatchType.StartsWith:
					pathMatcher = new StartsWithPathMatcher();
					break;

				case PathMatchType.Exact:
					pathMatcher = new ExactPathMatcher();
					break;

				default:
					throw new ArgumentOutOfRangeException("matchType");
			}

			// Cache the path matcher, if possible.
			if (cachedMatchers != null) {
				cachedMatchers.Add(matchType, pathMatcher);
			}

			Logger.LogFormat("Creating {0}.", pathMatcher.GetType().Name);
			return pathMatcher;
		}
	}
}