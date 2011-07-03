// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A factory for ISecurityEvaluator.
	/// </summary>
	internal class SecurityEvaluatorFactory {
		/// <summary>
		/// Gets a security evaluator.
		/// </summary>
		/// <returns></returns>
		internal static ISecurityEvaluator Create() {
			return new SecurityEvaluator();
		}
	}
}