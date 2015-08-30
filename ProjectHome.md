Security Switch enables various ASP.NET applications to automatically switch requests for pages/resources between the HTTP and HTTPS protocols without the need to write absolute URLs in HTML markup.

With deprecated support for ASP.NET 1.1 (via version 2.x) and full support for ASP.NET 2 and higher, you can easily configure what pages/resources should be secured via your website's SSL certificate. This is accomplished through the configuration of an ASP.NET module (IHttpModule).

Install via NuGet: `PM> Install-Package SecuritySwitch`

View the [GettingStarted](GettingStarted.md) page for a jump-start.