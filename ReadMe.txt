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

Paths
~~~~~
Within the securitySwitch section element, there should be a paths element. The paths element is a collection of entries that tell the module how to handle certain requests. Adding path entries should be familiar to most ASP.NET developers. Each element in the paths collection is an "add" element, with attributes itself. Below is an example of a few path entries.

<securitySwitch>
	<paths>
		<add path="~/Info/Contact.aspx" matchType="Exact" />
		<add path="~/Login.aspx" />
		<add path="~/Manage" />
		
		<add path="~/Admin(/|/[Dd]efault\.aspx)?$" matchType="Regex" ignoreCase="false" security="Insecure" />
		<add path="~/Admin/" />
		
		<add path="~/Media/" security="Ignore" />
		
		<add path="~/Cms/Default\.aspx\?([a-zA-Z0-9\-%_= ]+&amp;)*pageId=2(&amp;[a-zA-Z0-9\-%_= ]+)*$" matchType="Regex" />
	</paths>
</securitySwitch>

The first entry will ensure that any request for the Contact.aspx page in the Info sub-directory of the site will be secured via HTTPS. The matchType is "Exact" and that means that only an exact request for that path will be matched. In other words if there is any tail, query string, or bookmark included in a request, it will not be redirected (e.g. /Info/Contact.aspx?ref=email, /Info/Contact.aspx#form).

The next two entries will secure requests for the Login.aspx page and any path starting with /Manage. Since no matchType is specified, the default, "StartsWith", is used. This works better for these two, because often requests for the login page will have a query string attached to it with the return URL (e.g. /Login.aspx?ReturnUrl=%2fManage). Likewise, anything in the /Manage sub-directory will be secured. Note, however, that a request for /ManagementInfo.aspx will also be secured because that request starts with /Manage.

The fourth and fifth entries are all about the /Admin sub-directory. The fifth entry ensures that any request to the /Admin sub-directory are secured. However, the fourth entry preempts the fifth, because it is listed beforehand. It instructs the module to access the default page in the /Admin sub-directory insecurely (via HTTP). It uses a matchType of Regex to catch the various possible ways a request may be made for the default page (e.g. /Admin, /Admin/, /Admin/Default.aspx). Also, the ignoreCase attribute is set to false to prove a point; /Admin/Default.aspx and /Admin/default.aspx are separate requests. The regex accounts for both. If we omit ignoreCase, or set it to true (the default), the regex path could be rewritten to just "~/Admin(/|/Default\.aspx)?$" and either request will be matched.

The sixth entry will force the module to ignore any requests for resources in the /Media sub-directory. This is especially important if you are running a website on IIS 7.x in Integrated Mode or if you have a wildcard handler setup in IIS to process all requests through the ASP.NET pipeline. In these cases, a request for /Media/Images/Title.jpg will use the same protocol that the page it's reference in uses. If left out and a page secured via HTTPS references that image, the image request would be redirected to HTTP by the module; causing mixed security warnings in the browser.

The final entry uses regex to secure a particular query string value when requested with the /Cms/Default.aspx page. If an insecure request for /Cms/Default.aspx?pageId=2 is made, it will be redirected by the module in order to secure it via HTTPS. This entry even accounts for the pageId=2 parameter being anywhere within the query string. It can be the first parameter, the only parameter, or the third parameter; it doesn't matter (e.g. /Cms/Default.aspx?cache=On&pageId=2&author=Matt).

Finally, if no path entry matches a request, the module will ensure it is accessed insecurely (via HTTP). This prevents "getting stuck in HTTPS". That is, accessing the site via HTTPS and continuing to request resources via HTTPS. Such behavior would result in more CPU usage on the server (SSL requires a bit more processing for the encryption) and more bandwidth consumption (encrypted data is inherently larger than raw data). Either could end up costing you or your client quite a bit more in hosting bills!

Take care when ordering your path entries. Order definitely matters. In the example above, entries four and five are ordered specifically to achieve the desired results. If the fourth entry (the one that sets security to "Insecure") were below the fifth entry, the module would never get to it. The module processes entries in the order you specify them, and once it finds a matching entry, it acts on it. In fact, the only reason there is an option to set the security attribute to "Insecure" is to override more general entries below. As in this example, anything in the /Admin sub-directory would be secured if it were not for the fourth entry overriding such behavior for the default page.

IntelliSense and the securitySwitch Section Schema
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
To enable IntelliSense while editing the securitySwitch section in a web.config file, add an xmlns attribute to the section and include the provided schema file in your solution. Below is an example of the section with the necessary attribute.

<securitySwitch xmlns="http://SecuritySwitch-v4.xsd" ...>
	<paths>
		...
	</paths>
</securitySwitch>

Be sure to either include the SecuritySwitch-v4.xsd file in your solution, or (better still) install the schema file for Visual Studio. If Visual Studio does not automatically detect the schema file in your solution, you can add it to the Schemas property in the Properties window while the web.config file is open. To install the schema file for Visual Studio to always find in all your projects, copy the .xsd file to the appropriate directory, as shown below ([version] indicates the version of Visual Studio you are installing to).

	* for 32-bit systems: %ProgramFiles%\Microsoft Visual Studio [version]\Xml\Schemas
	* for 64-bit systems: %ProgramFiles(x86)%\Microsoft Visual Studio [version]\Xml\Schemas


Additional Resources
--------------------
* The original article on The Code Project
  http://www.codeproject.com/KB/web=security/WebPageSecurity_v2.aspx

* Transport Layer Security (TLS) and Secure Sockets Layer (SSL) on Wikipedia
  http://en.wikipedia.org/wiki/Transport_Layer_Security

* Tip/Trick: Enabling SSL on IIS 7.0 Using Self-Signed Certificates (by the Gu)
  http://weblogs.asp.net/scottgu/archive/2007/04/06/tip-trick-enabling-ssl-on-iis7-using-self-signed-certificates.aspx

* How to Set Up SSL on IIS 7
  http://learn.iis.net/page.aspx/144/how-to-set-up-ssl-on-iis-7/