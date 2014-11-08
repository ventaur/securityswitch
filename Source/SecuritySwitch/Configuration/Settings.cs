// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
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
		/// Gets or sets a flag indicating whether or not to enable HSTS.
		/// Enabling HTTP Strict Transport Security (HSTS) will have this module send 
		/// a specific header on all HTTPS requests. The header sent is named 
		/// "Strict-Transport-Security". It's value is set to "max-age=[HstsMaxAge]", 
		/// where the max age value is that of the HstsMaxAge property/attribute.
		/// </summary>
		[ConfigurationProperty(ElementNames.EnableHsts, DefaultValue = false)]
		public bool EnableHsts {
			get { return (bool)this[ElementNames.EnableHsts]; }
			set { this[ElementNames.EnableHsts] = value; }
		}

		/// <summary>
		/// Gets or sets the maximum age value sent as part of the value for the 
		/// "Strict-Transport-Security" header, if EnableHsts is true. The header 
		/// value is set to "max-age=[HstsMaxAge]".
		/// </summary>
		[ConfigurationProperty(ElementNames.HstsMaxAge, DefaultValue = 31536000U)]
		public uint HstsMaxAge {
			get { return (uint)this[ElementNames.HstsMaxAge]; }
			set { this[ElementNames.HstsMaxAge] = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether or not to ignore AJAX requests.
		/// </summary>
		/// <value>
		///   <c>true</c> if AJAX requests should be ignored; otherwise, <c>false</c>.
		/// </value>
		[ConfigurationProperty(ElementNames.IgnoreAjaxRequests, DefaultValue = false)]
		public bool IgnoreAjaxRequests {
			get { return (bool)this[ElementNames.IgnoreAjaxRequests]; }
			set { this[ElementNames.IgnoreAjaxRequests] = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether or not to ignore request for images.
		/// </summary>
		/// <value>
		///   <c>true</c> if images should be ignored; otherwise, <c>false</c>.
		/// </value>
		[ConfigurationProperty(ElementNames.IgnoreImages, DefaultValue = true)]
		public bool IgnoreImages {
			get { return (bool)this[ElementNames.IgnoreImages]; }
			set { this[ElementNames.IgnoreImages] = value; }
		}
		
		/// <summary>
		/// Gets or sets a flag indicating whether or not to ignore request for style sheets.
		/// </summary>
		/// <value>
		///   <c>true</c> if style sheets should be ignored; otherwise, <c>false</c>.
		/// </value>
		[ConfigurationProperty(ElementNames.IgnoreStyleSheets, DefaultValue = true)]
		public bool IgnoreStyleSheets {
			get { return (bool)this[ElementNames.IgnoreStyleSheets]; }
			set { this[ElementNames.IgnoreStyleSheets] = value; }
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
		/// Gets or sets any request headers (and optional values) that indicate if the request is secure. This is often used when 
		/// security (SSL) is offloaded to another server (e.g., ISA Server). This property's value should mimic a query string 
		/// (without the leading '?').
		/// </summary>
		[ConfigurationProperty(ElementNames.OffloadedSecurityHeaders), RegexStringValidator(@"^([^?=&]+)(=([^&]*))(&[^?=&]+)(=([^&]*))*|$")]
		public string OffloadedSecurityHeaders {
			get { return (string)this[ElementNames.OffloadedSecurityHeaders]; }
			set { this[ElementNames.OffloadedSecurityHeaders] = (!string.IsNullOrEmpty(value) ? value : null); }
		}

		/// <summary>
		/// Gets or sets any server variables (and optional values) that indicate if the request is secure. This is often used when 
		/// security (SSL) is offloaded to another server (e.g., ISA Server). This property's value should mimic a query string 
		/// (without the leading '?').
		/// </summary>
		[ConfigurationProperty(ElementNames.OffloadedSecurityServerVariables), RegexStringValidator(@"^([^?=&]+)(=([^&]*))(&[^?=&]+)(=([^&]*))*|$")]
		public string OffloadedSecurityServerVariables {
			get { return (string)this[ElementNames.OffloadedSecurityServerVariables]; }
			set { this[ElementNames.OffloadedSecurityServerVariables] = (!string.IsNullOrEmpty(value) ? value : null); }
		}

		/// <summary>
		/// Gets the collection of path settings read from the configuration section.
		/// </summary>
		[ConfigurationProperty(ElementNames.Paths, IsDefaultCollection = true, IsRequired = false)]
		public PathSettingCollection Paths {
			get { return (PathSettingCollection)this[ElementNames.Paths]; }
		}

		/// <summary>
		/// Gets or sets any security port that will indicate if a request is secure. This is sometimes used by load balancers or
		/// security/certificate servers.
		/// </summary>
		[ConfigurationProperty(ElementNames.SecurityPort, DefaultValue = null)]
		public int? SecurityPort {
			get { return (int?)this[ElementNames.SecurityPort]; }
			set { this[ElementNames.SecurityPort] = value; }
		}

		#endregion

		/// <summary>
		/// Overridden to ignore namespace-related attributes.
		/// </summary>
		/// <param name="name">The name of the unrecognized attribute.</param>
		/// <param name="value">The value of the unrecognized attribute.</param>
		/// <returns>
		/// true when an unknown attribute is encountered while de-serializing; otherwise, false.
		/// </returns>
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value) {
			if (name.IndexOf(':') > 0 || name.StartsWith("xmlns")) {
				return true;
			}

			return base.OnDeserializeUnrecognizedAttribute(name, value);
		}

		/// <summary>
		/// Called after de-serialization for special validation, app-path resolution, and to setup any system handler ignore settings.
		/// </summary>
		protected override void PostDeserialize() {
			base.PostDeserialize();

			Validate();

			// Resolve any special tokens found in each PathSetting's path.
			Logger.Log("Resolving any application relative tokens for all paths.");
			foreach (PathSetting pathSetting in Paths) {
				ResolveAppRelativeToken(pathSetting);
			}

			ConfigureForIgnoringImages();
			ConfigureForIgnoringStyleSheets();
			ConfigureForIgnoringSystemHandlers();
		}

		/// <summary>
		/// Validates these settings. This method is called after the settings are deserialized from configuration.
		/// </summary>
		/// <exception cref="System.ApplicationException">Thrown if any settings property is not validate.</exception>
		protected virtual void Validate() {
			// Validate the base secure/insecure URI settings.
			bool isBaseInsecureUriEmpty = string.IsNullOrEmpty(BaseInsecureUri);
			bool isBaseSecureUriEmpty = string.IsNullOrEmpty(BaseSecureUri);
			if (!isBaseInsecureUriEmpty && isBaseSecureUriEmpty || isBaseInsecureUriEmpty && !isBaseSecureUriEmpty) {
				throw new ApplicationException("If either baseInsecureUri or baseSecureUri are specified, then both must be provided.");
			}

			// Validate the SecurityPort, if specified.
			if (SecurityPort.HasValue && (SecurityPort.Value < 1 || SecurityPort.Value > 65535)) {
				throw new ApplicationException("If provided, securityPort must be a valid port number between 1 and 65535.");
			}
		}


		private void ConfigureForIgnoringImages() {
			// Insert a special PathSetting to ignore images, if indicated.
			if (!IgnoreImages) {
				return;
			}

			Logger.Log("Inserting a new path setting to ignore images.");
			Paths.Insert(0,
			             new PathSetting {
				             Path = @"\.(?:bmp|gif|ico|jpe?g|png|svg|tiff?|webp|xbm)(?:[/\?#].*)?$",
				             MatchType = PathMatchType.Regex,
				             IgnoreCase = true,
				             Security = RequestSecurity.Ignore
			             });
			Paths.Insert(0,
			             new PathSetting {
				             Path = @".*/images/.*$",
				             MatchType = PathMatchType.Regex,
				             IgnoreCase = true,
				             Security = RequestSecurity.Ignore
			             });
		}

		private void ConfigureForIgnoringStyleSheets() {
			// Insert a special PathSetting to ignore style sheets, if indicated.
			if (!IgnoreStyleSheets) {
				return;
			}

			Logger.Log("Inserting a new path setting to ignore style sheets.");
			Paths.Insert(0,
			             new PathSetting {
				             Path = @"\.css(?:[/\?#].*)?$",
				             MatchType = PathMatchType.Regex,
				             IgnoreCase = true,
				             Security = RequestSecurity.Ignore
			             });
			Paths.Insert(0,
			             new PathSetting {
				             Path = @".*/(?:styles|stylesheets)/.*$",
				             MatchType = PathMatchType.Regex,
				             IgnoreCase = true,
				             Security = RequestSecurity.Ignore
			             });
		}

		private void ConfigureForIgnoringSystemHandlers() {
			// Insert a special PathSetting to ignore system handlers, if indicated.
			if (!IgnoreSystemHandlers) {
				return;
			}

			Logger.Log("Inserting a new path setting to ignore system handlers.");
			Paths.Insert(0,
			             new PathSetting {
				             Path = @"\.axd(?:[/\?#].*)?$",
				             MatchType = PathMatchType.Regex,
				             IgnoreCase = true,
				             Security = RequestSecurity.Ignore
			             });
		}

		/// <summary>
		/// Resolves any application relative token (~/) to the application's virtual path.
		/// </summary>
		/// <param name="pathSetting">The PathSetting to evaluate.</param>
		private static void ResolveAppRelativeToken(PathSetting pathSetting) {
			if (!pathSetting.Path.StartsWith("~/")) {
				return;
			}

			// Get the application virtual path.
			string appVirtualPath = VirtualPathUtility.AppendTrailingSlash(HttpRuntime.AppDomainAppVirtualPath);

			// If the match type is Regex, be sure to escape the app virtual path.
			if (pathSetting.MatchType == PathMatchType.Regex) {
				appVirtualPath = Regex.Escape(appVirtualPath);
			}

			// Replace the app-relative token with the app virtual path.
			pathSetting.Path = pathSetting.Path.Replace("~/", appVirtualPath);
		}
	}
}