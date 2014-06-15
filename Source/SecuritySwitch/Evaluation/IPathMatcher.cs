// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// The contract for a path matcher.
	/// </summary>
	public interface IPathMatcher {
		/// <summary>
		/// Determines whether the specified path is a match to the provided pattern.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="pattern">The pattern to match against.</param>
		/// <param name="ignoreCase">A flag that indicates whether or not to ignore the case of the path and pattern when matching.</param>
		/// <returns>
		/// 	<c>true</c> if the specified path is a match with the pattern; otherwise, <c>false</c>.
		/// </returns>
		bool IsMatch(string path, string pattern, bool ignoreCase);
	}
}