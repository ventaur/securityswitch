using System;
using System.Collections.Specialized;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// A base ISecurityEvaluator for checking a name/value collection for a match (or simply the existence of an entry)
	/// against another name/value collection that may contain an indication that the request is secure.
	/// </summary>
	public abstract class NameValueCollectionSecurityEvaluator : ISecurityEvaluator {
		/// <summary>
		/// Determines whether the specified request is over a secure connection.
		/// </summary>
		/// <param name="request">The request to test.</param>
		/// <param name="settings">The settings used for determining a secure connection.</param>
		/// <returns>
		///   <c>true</c> if the specified request is over a secure connection; otherwise, <c>false</c>.
		/// </returns>
		public abstract bool IsSecureConnection(HttpRequestBase request, Settings settings);


		/// <summary>
		/// Finds any name/value match from the expected collection against the actual collection.
		/// </summary>
		/// <param name="expectedCollection">The expected collection.</param>
		/// <param name="actualCollection">The actual collection.</param>
		/// <returns></returns>
		protected bool FindAnyNameValueMatch(NameValueCollection expectedCollection, NameValueCollection actualCollection) {
			foreach (string name in expectedCollection.AllKeys) {
				// Match not found, move along.
				if (actualCollection[name] == null) {
					continue;
				}

				// If a matching name exists, but no value is expected OR if the expected value matches the actual's value, 
				// indicated a secure connection.
				if (string.IsNullOrEmpty(expectedCollection[name]) ||
				    string.Equals(expectedCollection[name], actualCollection[name], StringComparison.OrdinalIgnoreCase)) {
					return true;
				}
			}

			return false;
		}
	}
}