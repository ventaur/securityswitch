# The securitySwitch Section #

Configuration of the module is done via the _securitySwitch_ section of a web.config file. The main element has several attributes itself, but none are required. The following section declaration is perfectly valid and will enable the module with all defaults. Note, the [paths element](ConfiguringPaths.md) and at least one _add_ element entry within it are required.

```
<securitySwitch>
    <paths>
        ...
    </paths>
</securitySwitch>
```

The _securitySwitch_ element may have the following attributes set to an allowed value, as also defined below.

| **Attribute Name** | **Data Type** | **Default Value** | **Allowed Values** |
|:-------------------|:--------------|:------------------|:-------------------|
| baseInsecureUri    | string        | null              | any valid URI      |
| baseSecureUri      | string        | null              | any valid URI      |
| bypassSecurityWarning | bool          | false             | true, false        |
| enableHsts         | bool          | false             | true, false        |
| hstsMaxAge         | int           | 31536000          | maximum age to maintain STS |
| ignoreAjaxRequests | bool          | false             | true, false        |
| ignoreImages       | bool          | true              | true, false        |
| ignoreStyleSheets  | bool          | true              | true, false        |
| ignoreSystemHandlers | bool          | true              | true, false        |
| mode               | Mode          | On                | On, RemoteOnly, LocalOnly, Off |
| offloadedSecurityHeaders | string        | null              | query string like name/value pairs |
| offloadedSecurityServerVariables | string        | null              | query string like name/value pairs |
| securityPort       | int?          | null              | port indicating secure connection |

Set _baseSecureUri_ to a valid URI when you do not have an SSL certificate installed on the same domain as your standard site (accessed via HTTP) or if your server is setup to serve HTTPS on a non-standard port (a port other than 443). Setting _baseSecureUri_ will instruct the module to redirect any requests that need to switch from HTTP to HTTPS to a URI that starts with the _baseSecureUri_. For example, if _baseSecureUri_ is "https://secure.mysite.com" and a request for http://www.mysite.com/Login.aspx is made (and Login.aspx is configured to be secure), the module will redirect visitors to https://secure.mysite.com/Login.aspx. Similarly, if _baseSecureUri_ is "https://secure.somehostingsite.com/mysite", visitors would be redirected to https://secure.somehostingsite.com/mysite/Login.aspx.

Likewise, set _baseInsecureUri_ to a valid URI when you have supplied a value for _baseSecureUri_. This ensures the module will send visitors back to your standard site when switching from HTTPS to HTTP. To build on the previous example above, if _baseInsecureUri_ is "http://www.mysite.com", a visitor requesting https://secure.somehostingsite.com/mysite/Info/ContactUs.aspx would be redirected to http://www.mysite.com/Info/ContactUs.aspx.

If either _baseSecureUri_ or _baseInsecureUri_ are set, you **must** provide both values. The module needs to know how to switch back when necessary and will use the other base URI to accomplish that.

Set _bypassSecurityWarning_ to true when you wish to attempt to avoid browser warnings about switching from HTTPS to HTTP. Many browsers alert visitors when a server issues a redirect request that would remove the user from HTTPS. This is not necessarily a bad feature in browsers. However, some website owners/developers wish to avoid such security warnings when possible. When _bypassSecurityWarning_ is true, the module will forgo the usual practice of issuing a formal redirect and, instead, will output a "Refresh" header followed by some JavaScript to change the visitor's location. A refresh header is **not** a standard HTTP header. However, many browsers do honor it and "refresh" the current location with the specified URL after a timeout. The module sets the URL to the appropriate redirect location with a timeout of 0 (immediately). In addition, a small JavaScript block is output to the browser as backup. If the browser does not honor the refresh header, the script will set the window's location to the appropriate URL.

When _enableHsts_ is true, the "Strict-Transport-Security" header will be sent with all secure (HTTPS) respones. The max-age set for this header will be the _hstsMaxAge_ value. For more information about HSTS, see https://www.owasp.org/index.php/HTTP_Strict_Transport_Security.

If the "Strict-Transport-Security" header is sent with a response, the _hstsMaxAge_ value is used to build the header value. For example, if _hstsMaxAge_is set to 30, the header value sent will be "max-age=30".

Setting _ignoreAjaxRequests_ to true will have the module ignore all AJAX requests, regardless of the request's path. When true, this setting overrides any matching path's settings if the request is made via AJAX. If false, the module will process the request like all others by checking for any matching path.

_ignoreImages_ is true by default, and that instructs Security Switch to add special paths that will ignore requests in an "images" folder and the most common web image file requests (requests for files with a common web image extension; e.g., .gif, .jpg, .png, etc.).

The default setting for _ignoreStyleSheets_ (true), has the module add special paths to ignore requests in a "styles" or "stylesheets" folder and any requests for files with the .css extension.

When _ignoreSystemHandlers_ is true (the default), the module will automatically add a special path that will effectively ensure that requests for .axd handlers will be ignored during processing. This is most likely desireable, because ASP.NET makes ample use of the WebResource.axd handler. Likewise, Trace.axd and any other handler with the .axd extension will be ignored when this module evaluates the need to redirect the request. This will avoid browser warnings about mixed security, which occurs when a page is requested via one protocol (i.e. HTTPS) and resources referenced by the page are requested via a different protocol (i.e. HTTP). Without this setting, when a request for WebResource.axd is made via HTTPS on a secure page, the module would see that no path entry matching the request is found. Therefore, the module would redirect the request to use HTTP, causing the mixed security alert. Note, you can disable this setting and manually add path entries for WebResource.axd and any others you specifically want the module to ignore.

The _mode_ attribute determines under what circumstances the module evaluates requests. A value of "On" enables the module for all requests, regardless of their origin. "RemoteOnly" will instruct the module to only consider requests that are made from a remote computer. If a request is made on the actual Web server (i.e. localhost, 127.0.0.1, etc.), the module will not act. Likewise, setting the _mode_ to "LocalOnly" will enable module only when a request is made from the Web server. Finally, "Off" disables the module entirely. **Disabling the module is great for troubleshooting issues with SSL and/or protocols**, because it takes the Security Switch module out of the equation.

Use _offloadedSecurityHeaders_ to designate request headers that may be present from an offloaded security device (such as a dedicated SSL server/accelerator; e.g., ISA Server, etc.). The value of this attribute should look like a query string without the leading "?", with a name/value pair (e.g., SSL=Yes). If there are more than one headers the module should consider, delimit each pair with an ampersand (e.g., SSL=Yes&HTTPS=on).

Use offloadedSecurityServerVariables to designate server variables that may be present from an offloaded security device (such as a dedicated SSL server/accelerator; e.g., ISA Server, etc.) that indicate a secure connection. The value of this attribute should look like a query string without the leading "?", with a name/value pair (e.g., HTTP\_X\_FORWARD\_PROTOCOL=HTTPS). If there is more than one server variable the module should consider, delimit each pair with an ampersand (e.g., HTTP\_X\_FORWARD\_PROTOCOL=HTTPS&SSL=on).

Use _securityPort_ to indicate a port that must match a request's port in order for the module to consider the request is over a secure connection.

## External Configuration Source ##
As with all sections of .NET configuration files, you may supply an external configuration source for the securitySwitch section. Create a new configuration file somewhere accessible to your website and put just the securitySwitch configuration section in it (as the root). Then, include just the section in your main configuration file with a configSource attribute similar to the following.

```
<securitySwitch configSource="MySecuritySwitch.config" />
```

## Paths ##
Paths are required in order for the module to actually allow for secure pages. Read more about [configuring paths](ConfiguringPaths.md) to learn about the various options available for controlling how Security Switch evaluates requests.