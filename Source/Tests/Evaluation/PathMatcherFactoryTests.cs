// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;

using Xunit;


namespace SecuritySwitch.Tests.Evaluation {
	public class PathMatcherFactoryTests {
		[Fact]
		public void CreateReturnsAppropriatePathMatcherBasedOnPathMatchTypeSpecified() {
			// Arrange.
			// Act.
			var matcherForExact = PathMatcherFactory.Create(PathMatchType.Exact);
			var matcherForStartsWith = PathMatcherFactory.Create(PathMatchType.StartsWith);
			var matcherForRegex = PathMatcherFactory.Create(PathMatchType.Regex);

			// Assert.
			Assert.IsType<ExactPathMatcher>(matcherForExact);
			Assert.IsType<StartsWithPathMatcher>(matcherForStartsWith);
			Assert.IsType<RegexPathMatcher>(matcherForRegex);
		}
	}
}