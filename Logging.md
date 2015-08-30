# Logging #

There may be times you wish to see what's going on with the _Security Switch_ module/library during the processing of a page or other resource.

_Security Switch_ now supports logging!

Note: Common.Logging was used for this purpose until very recently. Common.Logging v2.0 did not work in Medium Trust, and Common.Logging v2.1 targets .NET Framework 3.5+. In order to maintain a .NET Framework v2.0 target and support Medium Trust, Common.Logging was dropped.

## How to Log Security Switch Actions ##
_Security Switch_ now uses a very basic internal logger that sends all log messages to a delegate, if provided. The delegate is null by default, so no logging occurs.

In order to enable logging through your favorite logging framework, simply provide an implementation for the delegate via the new `Logger.SetLogAction` method in your website's start-up code (i.e., in your boot-strapper or Global.asax). The easiest way to implement the delegate is via a lambda.

### An Example with log4net ###
There is no generic Log method for log4net's ILog interface, so we have to use a nasty switch statement to translate Security Switch's LogLevel into an appropriate ILog call.

```
public class Global : HttpApplication {
  // Get a log4net logger for SecuritySwitch actions.
  private static readonly ILog _logger = LogManager.GetLogger("SecuritySwitch");
  
  private static void LogSecuritySwitchAction(string message, Logger.LogLevel logLevel) {
    switch (logLevel) {
      case Logger.LogLevel.Debug:
        if (_logger.IsDebugEnabled) {
          _logger.Debug(message);
        }
        break;
        
      case Logger.LogLevel.Info:
        if (_logger.IsInfoEnabled) {
          _logger.Info(message);
        }
        break;
        
      case Logger.LogLevel.Warn:
        if (_logger.IsWarnEnabled) {
          _logger.Warn(message);
        }
        break;
    }
  }

  protected void Application_Start(object sender, EventArgs e) {
    log4net.Config.XmlConfigurator.Configure();

    // Setup a log action in order to capture logs from SecuritySwitch.
    // Here, we just pass-through the log message and level to log4net.
    SecuritySwitch.Logger.SetLogAction(LogSecuritySwitchAction);
  }
}
```

### An Example with NLog ###
NLog uses an immutable class instead of a simple enum to represent its log levels. Luckily, its LogLevel class provides a FromString method that we can use to easily translate Security Switch's LogLevel.

```
public class Global : HttpApplication {
  // Get an NLog logger for SecuritySwitch actions.
  private static readonly NLog.Logger _logger = LogManager.GetLogger("SecuritySwitch");

  protected void Application_Start(object sender, EventArgs e) {
    // Setup a log action in order to capture logs from SecuritySwitch.
    // Here, we just pass-through the log message and level to NLog.
    SecuritySwitch.Logger.SetLogAction(
      (message, logLevel) => {
        // Translate the SecuritySwitch LogLevel to NLog's LogLevel.
        var translatedLogLevel = LogLevel.FromString(logLevel.ToString());
        _logger.Log(translatedLogLevel, message);
      });
  }
}
```