# Dynamic Evaluation of Requests #

There may be times when you cannot configure the paths that need to be secured, because your application generates URLs/paths dynamically. This is especially true for Content Management Systems (CMS). In those cases, you can leave out the _paths_ element from the configuration section and provide an event handler for the module's _EvaluateRequest_ event. To do this, add an event handler to your site's Global.asax file named, "SecuritySwitch\_EvaluateRequest" with the following signature:

```
protected void SecuritySwitch_EvaluateRequest(object sender, EvaluateRequestEventArgs e) {
  // TODO: Update e.ExpectedSecurity based on the current Request.
}
```

Set the event argument's _ExpectedSecurity_ property to one of the _RequestSecurity_ values and the module will honor it instead of attempting to figure out how the request should be handled through the configuration of paths.