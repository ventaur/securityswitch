// =================================================================================
// Copyright © 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Collections;
using System.Globalization;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.SessionState;


namespace SecuritySwitch.Abstractions {
	public class HttpContextWrapper : HttpContextBase {
		private readonly HttpContext _context;

		public override Exception[] AllErrors {
			get { return _context.AllErrors; }
		}

		public override HttpApplicationStateBase Application {
			get { return new HttpApplicationStateWrapper(_context.Application); }
		}

		public override HttpApplication ApplicationInstance {
			get { return _context.ApplicationInstance; }
			set { _context.ApplicationInstance = value; }
		}

		public override Cache Cache {
			get { return _context.Cache; }
		}

		public override IHttpHandler CurrentHandler {
			get { return _context.CurrentHandler; }
		}

		public override RequestNotification CurrentNotification {
			get { return _context.CurrentNotification; }
		}

		public override Exception Error {
			get { return _context.Error; }
		}

		public override IHttpHandler Handler {
			get { return _context.Handler; }
			set { _context.Handler = value; }
		}

		public override bool IsCustomErrorEnabled {
			get { return _context.IsCustomErrorEnabled; }
		}

		public override bool IsDebuggingEnabled {
			get { return _context.IsDebuggingEnabled; }
		}

		public override bool IsPostNotification {
			get { return _context.IsDebuggingEnabled; }
		}

		public override IDictionary Items {
			get { return _context.Items; }
		}

		public override IHttpHandler PreviousHandler {
			get { return _context.PreviousHandler; }
		}

		public override ProfileBase Profile {
			get { return _context.Profile; }
		}

		public override HttpRequestBase Request {
			get { return new HttpRequestWrapper(_context.Request); }
		}

		public override HttpResponseBase Response {
			get { return new HttpResponseWrapper(_context.Response); }
		}

		public override HttpServerUtilityBase Server {
			get { return new HttpServerUtilityWrapper(_context.Server); }
		}

		public override HttpSessionStateBase Session {
			get {
				HttpSessionState session = _context.Session;
				return (session == null ? null : new HttpSessionStateWrapper(session));
			}
		}

		public override bool SkipAuthorization {
			get { return _context.SkipAuthorization; }
			set { _context.SkipAuthorization = value; }
		}

		public override DateTime Timestamp {
			get { return _context.Timestamp; }
		}

		public override TraceContext Trace {
			get { return _context.Trace; }
		}

		public override IPrincipal User {
			get { return _context.User; }
			set { _context.User = value; }
		}


		public HttpContextWrapper(HttpContext httpContext) {
			if (httpContext == null) {
				throw new ArgumentNullException("httpContext");
			}
			_context = httpContext;
		}

		public override void AddError(Exception errorInfo) {
			_context.AddError(errorInfo);
		}

		public override void ClearError() {
			_context.ClearError();
		}

		public override object GetGlobalResourceObject(string classKey, string resourceKey) {
			return HttpContext.GetGlobalResourceObject(classKey, resourceKey);
		}

		public override object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture) {
			return HttpContext.GetGlobalResourceObject(classKey, resourceKey, culture);
		}

		public override object GetLocalResourceObject(string virtualPath, string resourceKey) {
			return HttpContext.GetLocalResourceObject(virtualPath, resourceKey);
		}

		public override object GetLocalResourceObject(string virtualPath, string resourceKey, CultureInfo culture) {
			return HttpContext.GetLocalResourceObject(virtualPath, resourceKey, culture);
		}

		public override object GetSection(string sectionName) {
			return _context.GetSection(sectionName);
		}

		public override object GetService(Type serviceType) {
			return ((IServiceProvider)_context).GetService(serviceType);
		}

		public override void RemapHandler(IHttpHandler handler) {
			_context.RemapHandler(handler);
		}

		public override void RewritePath(string path) {
			_context.RewritePath(path);
		}

		public override void RewritePath(string path, bool rebaseClientPath) {
			_context.RewritePath(path, rebaseClientPath);
		}

		public override void RewritePath(string filePath, string pathInfo, string queryString) {
			_context.RewritePath(filePath, pathInfo, queryString);
		}

		public override void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath) {
			_context.RewritePath(filePath, pathInfo, queryString, setClientFilePath);
		}
	}
}