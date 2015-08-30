# Configuring Paths #

Within the _securitySwitch_ section element, there should be a _paths_ element. The _paths_ element is a collection of entries that tell the module how to handle certain requests. Adding path entries should be familiar to most ASP.NET developers. Each element in the paths collection is an _add_ element, with attributes itself. Below is an example of a few path entries.

```
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
```

The first entry will ensure that any request for the Contact.aspx page in the Info sub-directory of the site will be secured via HTTPS. The _matchType_ is "Exact" and that means that only an exact request for that path will be matched. In other words, if there is any tail, query string, or bookmark included in a request, it will not be redirected (e.g. /Info/Contact.aspx?ref=email, /Info/Contact.aspx#form).

The next two entries will secure requests for the Login.aspx page and any path starting with /Manage. Since no _matchType_ is specified, the default, "StartsWith", is used. This works better for these two, because often requests for the login page will have a query string attached to it with the return URL (e.g. /Login.aspx?ReturnUrl=%2fManage). Likewise, anything in the /Manage sub-directory will be secured. Note, however, that a request for /ManagementInfo.aspx will also be secured because that request starts with /Manage.

The fourth and fifth entries are all about the /Admin sub-directory. The fifth entry ensures that any request to the /Admin sub-directory are secured. However, the fourth entry preempts the fifth, because it is listed beforehand. It instructs the module to access the default page in the /Admin sub-directory insecurely (via HTTP). It uses a _matchType_ of "Regex" to catch the various possible ways a request may be made for the default page (e.g. /Admin, /Admin/, /Admin/Default.aspx). Also, the _ignoreCase_ attribute is set to false to prove a point; /Admin/Default.aspx and /Admin/default.aspx are separate requests. The regex accounts for both. If we omit _ignoreCase_, or set it to true (the default), the regex path could be rewritten to just "~/Admin(/|/Default\.aspx)?$" and either request will be matched.

The sixth entry will force the module to ignore any requests for resources in the /Media sub-directory. This is **especially important** if you are running a website on IIS 7.x in Integrated Mode or if you have a wildcard handler setup in IIS to process all requests through the ASP.NET pipeline. In these cases, a request for /Media/Images/Title.jpg will use the same protocol that the page it's reference in uses. If left out and a page secured via HTTPS references that image, the image request would be redirected to HTTP by the module; causing mixed security warnings in the browser.

The final entry uses regex to secure a particular query string value when requested with the /Cms/Default.aspx page. If an insecure request for /Cms/Default.aspx?pageId=2 is made, it will be redirected by the module in order to secure it via HTTPS. This entry even accounts for the pageId=2 parameter being anywhere within the query string. It can be the first parameter, the only parameter, or the third parameter; it doesn't matter (e.g. /Cms/Default.aspx?cache=On&pageId=2&author=Matt).

Finally, if no path entry matches a request, the module will ensure it is accessed insecurely (via HTTP). This prevents "getting stuck in HTTPS". That is, accessing the site via HTTPS and continuing to request resources via HTTPS. Such behavior would result in more CPU usage on the server (SSL requires a bit more processing for the encryption) and more bandwidth consumption (encrypted data is inherently larger than raw data). **Either could end up costing you or your client quite a bit more in hosting bills!**

Take care when ordering your path entries. Order definitely matters. In the example above, entries four and five are ordered specifically to achieve the desired results. If the fourth entry (the one that sets _security_ to "Insecure") were below the fifth entry, the module would never get to it. The module processes entries in the order you specify them, and once it finds a matching entry, it acts on it. In fact, the only reason there is an option to set the _security_ attribute to "Insecure" is to override more general entries below. As in this example, anything in the /Admin sub-directory would be secured if it were not for the fourth entry overriding such behavior for the default page.

## Dynamic URLs ##
If you are unable to configure the paths of your application for security because they are dynamically created, you will likely need to forgo the _paths_ element and refer to [Dynamic Evaluation of Requests](DynamicEvaluation.md).