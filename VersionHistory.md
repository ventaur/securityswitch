### Version 4.4.0 ###
  * Added HSTS support. Set enableHsts to "true" to enable. Also, set hstsMaxAge to specify how long (in seconds) browsers should honor STS.

### Version 4.3.0 ###
  * JS encoding to prevent XSS vulnerabilities for JS-related redirects.

### Version 4.2.0 ###
  * Added ignoreImages and ignoreStyleSheets settings for automatically ignoring these types of requests if set.
  * Both of the new settings default to true, and could be a breaking change to some sites using the module.

### Version 4.1.4414 ###
  * Added support for a offloadedSecurityServerVariables setting that determines if the request is secure or not via one or more server variables.

### Version 4.1.4327 ###
  * Added support for logging.

### Version 4.1.4326 ###
  * Added support for a securityPort setting that determines if the request is secure or not via the request's port.

### Version 4.1.0 ###
  * Added support for Request headers (offloadedSecurityHeaders) as the determinator of whether the request is secure or not.

### Version 4.0.0 ###
  * Complete rewrite of the module for performance and maintainability.
  * Support for all variants of ASP.NET (Web Forms, MVC, etc.).
  * Support for any URI matching (i.e., query strings, fragments, etc.).
  * Support for matchTypes: StartsWith, Exact, and Regex.