// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;


namespace SecuritySwitch.Configuration {
	/// <summary>
	/// Contains the settings of a &lt;securitySwitch&gt; configuration section.
	/// </summary>
	public class Settings : ConfigurationSection {
		private const string UriValidationPattern = @"[\w\-][\w\.\-,]*(?:\:\d+)?(?:/[\w\.\-]+)*/?)$";

		#region Properties

		/// <summary>
		/// Gets or sets the path to a URI for unencrypted redirections, if any.
		/// </summary>
		[ConfigurationProperty(ElementNames.BaseInsecureUri), RegexStringValidator(@"^(?:|(?:http://)?" + UriValidationPattern)]
		public string BaseInsecureUri {
			get { return (string)this[ElementNames.BaseInsecureUri]; }
			set { this[ElementNames.BaseInsecureUri] = (!string.IsNullOrEmpty(value) ? value : null); }
		}

		/// <summary>
		/// Gets or sets the base path to a URI for encrypted redirections, if any.
		/// </summary>
		[ConfigurationProperty(ElementNames.BaseSecureUri), RegexStringValidator(@"^(?:|(?:https://)?" + UriValidationPattern)]
		public string BaseSecureUri {
			get { return (string)this[ElementNames.BaseSecureUri]; }
			set { this[ElementNames.BaseSecureUri] = (!string.IsNullOrEmpty(value) ? value : null); }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether or not to bypass security warnings
		/// when switching to a unencrypted page.
		/// </summary>
		[ConfigurationProperty(ElementNames.BypassSecurityWarning, DefaultValue = false)]
		public bool BypassSecurityWarning {
			get { return (bool)this[ElementNames.BypassSecurityWarning]; }
			set { this[ElementNames.BypassSecurityWarning] = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether or not to ignore requests for system handlers (*.axd paths).
		/// </summary>
		/// <value>
		///   <c>true</c> if system handlers should be ignored (*.axd); otherwise, <c>false</c>.
		/// </value>
		[ConfigurationProperty(ElementNames.IgnoreSystemHandlers, DefaultValue = true)]
		public bool IgnoreSystemHandlers {
			get { return (bool)this[ElementNames.IgnoreSystemHandlers]; }
			set { this[ElementNames.IgnoreSystemHandlers] = value; }
		}

		/// <summary>
		/// Gets or sets the mode indicating how the secure switch settings are handled.
		/// </summary>
		[ConfigurationProperty(ElementNames.Mode, DefaultValue = Mode.On)]
		public Mode Mode {
			get { return (Mode)this[ElementNames.Mode]; }
			set { this[ElementNames.Mode] = value; }
		}

		/// <summary>
		/// Gets the collection of path settings read from the configuration section.
		/// </summary>
		[ConfigurationProperty(ElementNames.Paths, IsDefaultCollection = true, IsRequired = true)]
		public PathSettingCollection Paths {
			get { return (PathSettingCollection)this[ElementNames.Paths]; }
		}

		
		/// <summary>
		/// This property is for internal use and is not meant to be set.
		/// </summary>
		[ConfigurationProperty(ElementNames.XmlNamespace)]
		protected string XmlNamespace {
			get { return (string)this[ElementNames.XmlNamespace]; }
			set { this[ElementNames.XmlNamespace] = value; }
		}

		#endregion

		protected override void PostDeserialize() {
			base.PostDeserialize();

			// Validate the base secure/insecure URI settings.
			var isBaseInsecureUriEmpty = string.IsNullOrEmpty(BaseInsecureUri);
			var isBaseSecureUriEmpty = string.IsNullOrEmpty(BaseSecureUri);
			if (!isBaseInsecureUriEmpty && isBaseSecureUriEmpty || isBaseInsecureUriEmpty && !isBaseSecureUriEmpty) {
				throw new ConfigurationErrorsException(
					"If either baseInsecureUri or baseSecureUri are specified, then both must be provided.");
			}

			// Resolve any special tokens found in each PathSetting's path.
			foreach (PathSetting pathSetting in Paths) {
				ResolveAppRelativeToken(pathSetting);
			}

			// Insert a special PathSetting to ignore system handlers, if indicated.
			if (IgnoreSystemHandlers) {
				Paths.Insert(0, new PathSetting {
					Path = @"\.axd(?:[/\?#].*)?$",
					MatchType = PathMatchType.Regex,
					IgnoreCase = true,
					Security = RequestSecurity.Ignore
				});
			}
		}


		/// <summary>
		/// Resolves any application relative token (~/) to the application's virtual path.
		/// </summary>
		/// <param name="pathSetting">The PathSetting to evaluate.</param>
		private static void ResolveAppRelativeToken(PathSetting pathSetting) {
			if (pathSetting.Path.StartsWith("~/")) {
				// Get the application virtual path.
				var appVirtualPath = VirtualPathUtility.AppendTrailingSlash(HttpRuntime.AppDomainAppVirtualPath);

				// If the match type is Regex, be sure to escape the app virtual path.
				if (pathSetting.MatchType == PathMatchType.Regex) {
					appVirtualPath = Regex.Escape(appVirtualPath);
				}

				// Replace the app-relative token with the app virtual path.
				pathSetting.Path = pathSetting.Path.Replace("~/", appVirtualPath);
			}
		}
	}
}