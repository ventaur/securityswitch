# Introduction #

Security Switch enables various ASP.NET applications to automatically switch requests for pages/resources between the HTTP and HTTPS protocols without the need to write absolute URLs in HTML markup.

With deprecated support for ASP.NET 1.1 (via version 2.x) and full support for ASP.NET 2 and higher, you can easily configure what pages/resources should be secured via your website's SSL certificate. This is accomplished through the configuration of an ASP.NET module (IHttpModule).

_Special Note:_
Security Switch is the new name for the old SecureWebPageModule library written for an article on The Code Project.


# Configuration #

_If you install Security Switch via NuGet ("`Install-Package SecuritySwitch`"), the configuration will be taken care of for you after the package installs. Feel free to skip this section._

Configuring Security Switch is a simple process. Open the web.config file for your web application, or website, and the following lines where indicated.

```
<configuration>
    ...
    <configSections>
        ...
        <section name="securitySwitch" 
                 type="SecuritySwitch.Configuration.Settings, SecuritySwitch" 
                 requirePermission="false" />
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
```

First, add a new section definition to the _configSections_ element collection. This tells ASP.NET that it can expect to see a section further down named, "securitySwitch". Next, add the aforementioned section. The [securitySwitch section](SecuritySwitchConfigSection.md) is where you will actually configure the module. For now, we set mode to "RemoteOnly" and add an entry to paths for the Login.aspx page (more on these settings later). Finally, add the module entry to either _system.Web/httpModules_ (for IIS <= 6.x, IIS 7.x with Classic Mode enabled, and the Web Development Server/Cassini), _system.webServer/modules_ (for IIS 7.x with Integrated Mode enabled), or both. The excerpt above adds the module to both sections and adds the _system.webServer/validation_ element to prevent IIS from complaining about the entry added to _system.web/httpModules_.

Another important step that many people forget is to include the Security Switch assembly. Just copy the _SecuritySwitch.dll_ assembly into your site's bin folder, or add a reference to the assembly in your project.

## IIS ##

One very crucial part of configuration is actually something you should **avoid** doing in IIS. Do **not** check the box in IIS that requires SSL.

Again, be sure to **uncheck/clear** the "Require SSL" check box under "SSL Settings" in IIS 7, or the "Require secure channel (SSL)" check box in the "Secure Communications" dialog in IIS 6 or below.

If this check box _is_ checked in IIS, the module will never run as IIS will validate that each connection is secure before processing further.