// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System.Configuration;


namespace SecuritySwitch.Configuration {
	/// <summary>
	/// Represents an entry in the securitySwitch/paths; configuration section.
	/// </summary>
	public class PathSetting : ConfigurationElement {
		#region Properties

		/// <summary>
		/// Gets or sets a flag indicating whether or not to ignore the paths' case when attempting to match them.
		/// </summary>
		[ConfigurationProperty(ElementNames.IgnoreCase, DefaultValue = true)]
		public bool IgnoreCase {
			get { return (bool)this[ElementNames.IgnoreCase]; }
			set { this[ElementNames.IgnoreCase] = value; }
		}

		/// <summary>
		/// Gets or sets the match type to use when matching a request's path to this path setting's Path.
		/// </summary>
		[ConfigurationProperty(ElementNames.MatchType, DefaultValue = PathMatchType.StartsWith)]
		public PathMatchType MatchType {
			get { return (PathMatchType)this[ElementNames.MatchType]; }
			set { this[ElementNames.MatchType] = value; }
		}

		/// <summary>
		/// Gets or sets the path of this path setting.
		/// </summary>
		[ConfigurationProperty(ElementNames.Path, IsRequired = true, IsKey = true)]
		public string Path {
			get { return (string)this[ElementNames.Path]; }
			set { this[ElementNames.Path] = value; }
		}

		/// <summary>
		/// Gets or sets the type of request security for this path setting.
		/// </summary>
		[ConfigurationProperty(ElementNames.Security, DefaultValue = RequestSecurity.Secure)]
		public RequestSecurity Security {
			get { return (RequestSecurity)this[ElementNames.Security]; }
			set { this[ElementNames.Security] = value; }
		}

		#endregion

		/// <summary>
		/// Creates an instance of PathSetting.
		/// </summary>
		protected internal PathSetting() {}
	}
}