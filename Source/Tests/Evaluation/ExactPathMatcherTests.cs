// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using SecuritySwitch.Evaluation;

using Xunit;


namespace SecuritySwitch.Tests.Evaluation {
	public class ExactPathMatcherTests {
		[Fact]
		public void IsMatchWithCaseSensitiveReturnsTrueForExactMatch() {
			// Arrange.
			const string Path = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";
			const string Pattern = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";

			var matcher = new ExactPathMatcher();

			// Act.
			var result = matcher.IsMatch(Path, Pattern, false);

			// Assert.
			Assert.True(result);
		}

		[Fact]
		public void IsMatchWithCaseInsensitiveReturnsTrueForEquivalentMatch() {
			// Arrange.
			const string Path = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";
			const string Pattern = "/webapp/administration/test.aspx?param1=42&param2=no";

			var matcher = new ExactPathMatcher();

			// Act.
			var result = matcher.IsMatch(Path, Pattern, true);

			// Assert.
			Assert.True(result);
		}

		[Fact]
		public void IsMatchWithCaseSensitiveReturnsFalseForEquivalentMatch() {
			// Arrange.
			const string Path = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";
			const string Pattern = "/webapp/administration/test.aspx?param1=42&param2=no";

			var matcher = new ExactPathMatcher();

			// Act.
			var result = matcher.IsMatch(Path, Pattern, false);

			// Assert.
			Assert.False(result);
		}

		[Fact]
		public void IsMatchWithCaseInsensitiveReturnsFalseForNearMatch() {
			// Arrange.
			const string Path = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";
			const string Pattern = "/webapp/administration/test.aspx?param1=42&param2=yes";

			var matcher = new ExactPathMatcher();

			// Act.
			var result = matcher.IsMatch(Path, Pattern, true);

			// Assert.
			Assert.False(result);
		}

		[Fact]
		public void IsMatchReturnsFalseForMissingQuery() {
			// Arrange.
			const string Path = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";
			const string Pattern = "/WebApp/Administration/Test.aspx";

			var matcher = new ExactPathMatcher();

			// Act.
			var result = matcher.IsMatch(Path, Pattern, true);

			// Assert.
			Assert.False(result);
		}
	}
}