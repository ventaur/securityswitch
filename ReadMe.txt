Security Switch 4.0.0 beta
==========================
Security Switch enables various ASP.NET applications to automatically switch requests for pages/resources between the HTTP and HTTPS protocols without the need to write absolute URLs in HTML markup.

With deprecated support for ASP.NET 1.1 (via version 2.x) and full support for ASP.NET 2 and higher, you can easily configure what pages/resources should be secured via your website's SSL certificate. This is accomplished through the configuration of an ASP.NET module (IHttpModule).


Special Note:
Security Switch is the new name for the old SecureWebPageModule library written for an article on The Code Project.


Configuration
-------------
Configuring Security Switch is a simple process. Open the web.config file for your web application, or website, and the following lines where indicated.

<configuration>
	...
	<configSections>
		...
		<section name="securitySwitch" type="SecuritySwitch.Configuration.Settings, SecuritySwitch" />
	</configSections>

	<securitySwitch mode="RemoteOnly">
		<paths>
			<add path="~/Login.aspx" />
		</paths>
	</securitySwitch>

	<system.web>
		...
		<httpModules>
			...
			<!-- for IIS <= 6.x, IIS 7.x + Classic Mode, and Web Development Server (Cassini) -->
			<add name="SecuritySwitch" type="SecuritySwitch.SecuritySwitchModule, SecuritySwitch" />
		</httpModules>
		...
	</system.web>
	...
	<system.webServer>
		...
		<validation validateIntegratedModeConfiguration="false" />
		<modules>
			...
			<!-- for IIS 7.x + Integrated Mode -->
			<add name="SecuritySwitch" type="SecuritySwitch.SecuritySwitchModule, SecuritySwitch" />
		</modules>
		...
	</system.webServer>
	...
</configuration>


First, add a new section definition to the configSections element collection. This tells ASP.NET that it can expect to see a section further down named, "securitySwitch". Next, add the aforementioned section. The securitySwitch section is where you will actually configure the module. For now, we set mode to "RemoteOnly" and add an entry to paths for the Login.aspx page (more on these settings later). Finally, add the module entry to either system.Web/httpModules (for IIS <= 6.x, IIS 7.x with Classic Mode enabled, and the Web Development Server/Cassini), system.webServer/modules (for IIS 7.x with Integrated Mode enabled), or both. The excerpt above adds the module to both sections and adds the system.webServer/validation element to prevent IIS from complaining about the entry added to system.web/httpModules.

Another important step that many people forget is to include the SecuritySwitch assembly. Just copy the SecuritySwitch.dll assembly into your site's bin folder, or add a reference to the assembly in your project.


The securitySwitch Section
··························
Configuration of the module done via the securitySwitch section of a web.config file. The main element has several attributes itself, but none are required. The following section declaration is perfectly valid and will enable the module with all defaults. Note, the paths element and at least one add element entry within it are required.

<securitySwitch>
	<paths>
		...
	</paths>
</securitySwitch>


The securitySwitch element may have the following attributes set to an allowed value, as also defined below.

Attribute Name          Data Type   Default Value   Allowed Values
----------------------------------------------------------------------------------
baseInsecureUri         string      [null]          any valid URI
baseSecureUri           string      [null]          any valid URI
bypassSecurityWarning   bool        false           true, false
ignoreSystemHandlers    bool        true            true, false
mode                    Mode        On              On, RemoteOnly, LocalOnly, Off

Set baseSecureUri to a valid URI when you do not have an SSL certificate installed on the same domain as your standard site (accessed via HTTP) or if your server is setup to serve HTTPS on a non-standard port (a port other than 443). Setting baseSecureUri will instruct the module to redirect any requests that need to switch from HTTP to HTTPS to a URI that starts with the baseSecureUri. For example, if baseSecureUri is "https://secure.mysite.com" and a request for http://www.mysite.com/Login.aspx is made (and Login.aspx is configured to be secure), the module will redirect visitors to https://secure.mysite.com/Login.aspx. Similarly, if baseSecureUri is "https://secure.somehostingsite.com/mysite", visitors would be redirected to https://secure.somehostingsite.com/mysite/Login.aspx.

Likewise, set baseInsecureUri to a valid URI when you have supplied a value for baseSecureUri. This ensures the module will send visitors back to your standard site when switching from HTTPS to HTTP. To build on the previous example above, if baseInsecureUri is "http://www.mysite.com", a visitor requesting https://secure.somehostingsite.com/mysite/Info/ContactUs.aspx would be redirected to http://www.mysite.com/Info/ContactUs.aspx.

If either baseSecureUri or baseInsecureUri are set, you must provide both values. The module needs to know how to switch back when necessary and will use the other base URI to accomplish that.

Set bypassSecurityWarning to true when you wish to attempt to avoid browser warnings about switching from HTTPS to HTTP. Many browsers alert visitors when a server issues a redirect request that would remove the user from HTTPS. This is not necessarily a bad feature in browsers. However, some website owners/developers wish to avoid such security warnings when possible. When bypassSecurityWarning is true, the module will forgo the usual practice of issuing a formal redirect and, instead, will output a "Refresh" header followed by some JavaScript to change the visitor's location. A refresh header is not a standard HTTP header. However, many browsers do honor it and "refresh" the current location with the specified URL after a timeout. The module sets the URL to the appropriate redirect location with a timeout of 0 (immediately). In addition, a small JavaScript block is output to the browser as backup. If the browser does not honor the refresh header, the script will set the window's location to the appropriate URL.

When ignoreSystemHandlers is true (the default), the module will automatically add a special path that will effectively ensure that requests for .axd handlers will be ignored during processing. This is most likely desireable, because ASP.NET makes ample use of the WebResource.axd handler. Likewise, Trace.axd and any other handler with the .axd extension will be ignored when this module evaluates the need to redirect the request. This will avoid browser warnings about mixed security, which occurs when a page is requested via one protocol (i.e. HTTPS) and resources referenced by the page are requested via a different protocol (i.e. HTTP). Without this setting, when a request for WebResource.axd is made via HTTPS on a secure page, the module would see that no path entry matching the request is found. Therefore, the module would redirect the request to use HTTP, causing the mixed security alert. Note, you can disable this setting and manually add path entries for WebResource.axd and any others you specifically want the module to ignore.

The mode attribute determines under what circumstances the module evaluates requests. A value of "On" enables the module for all requests, regardless of their origin. "RemoteOnly" will instruct the module to only consider requests that are made from a remote computer. If a request is made on the actual Web server (i.e. localhost, 127.0.0.1, etc.), the module will not act. Likewise, setting the mode to "LocalOnly" will enable module only when a request is made from the Web server. Finally, "Off" disables the module entirely. Disabling the module is great for troubleshooting issues with SSL and/or protocols, because it takes the Security Switch module out of the equation.





Resources
---------
= The original article on The Code Project:
  http://www.codeproject.com/KB/web=security/WebPageSecurity_v2.aspx