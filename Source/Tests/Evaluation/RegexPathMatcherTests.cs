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
	public class RegexPathMatcherTests {
		[Fact]
		public void IsMatchWithCaseSensitiveReturnsTrueForPartialQueryMatch() {
			// Arrange.
			const string Path1 = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";
			const string Path2 = "/WebApp/Administration/Test.aspx?Param2=No&Param1=42";
			const string Path3 = "/WebApp/Administration/Test.aspx?Param2=No";
			const string Pattern = @"/WebApp/Administration/Test\.aspx(\?.+&|\?)Param2=No";

			var matcher = new RegexPathMatcher();

			// Act.
			var result1 = matcher.IsMatch(Path1, Pattern, false);
			var result2 = matcher.IsMatch(Path2, Pattern, false);
			var result3 = matcher.IsMatch(Path3, Pattern, false);

			// Assert.
			Assert.True(result1);
			Assert.True(result2);
			Assert.True(result3);
		}

		[Fact]
		public void IsMatchWithCaseInsensitiveReturnsTrueForEquivalentPartialQueryMatch() {
			// Arrange.
			const string Path1 = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";
			const string Path2 = "/WebApp/Administration/Test.aspx?Param2=No&Param1=42";
			const string Path3 = "/WebApp/Administration/Test.aspx?Param2=No";
			const string Pattern = @"/webapp/administration/test\.aspx(\?.+&|\?)param2=no";

			var matcher = new RegexPathMatcher();

			// Act.
			var result1 = matcher.IsMatch(Path1, Pattern, true);
			var result2 = matcher.IsMatch(Path2, Pattern, true);
			var result3 = matcher.IsMatch(Path3, Pattern, true);

			// Assert.
			Assert.True(result1);
			Assert.True(result2);
			Assert.True(result3);
		}

		[Fact]
		public void IsMatchWithCaseSensitiveReturnsFalseForEquivalentMatch() {
			// Arrange.
			const string Path1 = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";
			const string Path2 = "/WebApp/Administration/Test.aspx?Param2=No&Param1=42";
			const string Path3 = "/WebApp/Administration/Test.aspx?Param2=No";
			const string Pattern = @"/webapp/administration/test\.aspx(\?.+&|\?)param2=no";

			var matcher = new RegexPathMatcher();

			// Act.
			var result1 = matcher.IsMatch(Path1, Pattern, false);
			var result2 = matcher.IsMatch(Path2, Pattern, false);
			var result3 = matcher.IsMatch(Path3, Pattern, false);

			// Assert.
			Assert.False(result1);
			Assert.False(result2);
			Assert.False(result3);
		}

		[Fact]
		public void IsMatchWithCaseInsensitiveReturnsFalseForNearMatch() {
			// Arrange.
			const string Path = "/WebApp/Administration/Test.aspx?Param1=42&Param2=No";
			const string Pattern = @"/webapp/administration/test\.aspx(\?.+&|\?)param2=yes";

			var matcher = new RegexPathMatcher();

			// Act.
			var result = matcher.IsMatch(Path, Pattern, true);

			// Assert.
			Assert.False(result);
		}

		[Fact]
		public void IsMatchReturnsFalseForMissingQuery() {
			// Arrange.
			const string Path = "/WebApp/Administration/Test.aspx";
			const string Pattern = @"/WebApp/Administration/Test\.aspx(\?.+&|\?)Param2=No";

			var matcher = new RegexPathMatcher();

			// Act.
			var result = matcher.IsMatch(Path, Pattern, true);

			// Assert.
			Assert.False(result);
		}

		[Fact]
		public void IsMatchReturnsTrueForNonQueryContainmentMatch() {
			// Arrange.
			const string Path1 = "/WebApp/Administration/Test.aspx";
			const string Path2 = "/webapp/admin/manage.aspx";
			const string Path3 = "/WebApp/Admin.aspx";
			const string Path4 = "/WebApp/Reports/TestAdminReport.aspx";
			const string Path5 = "/WebApp/Reports/TestReport.aspx?Admin=true";
			const string Path6 = "/WebApp/Reports/TestReport.aspx?Param1=42&Admin=true";
			const string Pattern = @"(?<!\?|&)Admin";

			var matcher = new RegexPathMatcher();

			// Act.
			var result1 = matcher.IsMatch(Path1, Pattern, true);
			var result2 = matcher.IsMatch(Path2, Pattern, true);
			var result3 = matcher.IsMatch(Path3, Pattern, true);
			var result4 = matcher.IsMatch(Path4, Pattern, true);
			var result5 = matcher.IsMatch(Path5, Pattern, true);
			var result6 = matcher.IsMatch(Path6, Pattern, true);

			// Assert.
			Assert.True(result1);
			Assert.True(result2);
			Assert.True(result3);
			Assert.True(result4);
			Assert.False(result5);
			Assert.False(result6);
		}
	}
}