// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Web;


namespace SecuritySwitch.Abstractions {
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
	 AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class HttpRequestBase {
		public virtual string[] AcceptTypes {
			get { throw new NotImplementedException(); }
		}

		public virtual string AnonymousID {
			get { throw new NotImplementedException(); }
		}

		public virtual string ApplicationPath {
			get { throw new NotImplementedException(); }
		}

		public virtual string AppRelativeCurrentExecutionFilePath {
			get { throw new NotImplementedException(); }
		}

		public virtual HttpBrowserCapabilitiesBase Browser {
			get { throw new NotImplementedException(); }
		}

		public virtual HttpClientCertificate ClientCertificate {
			get { throw new NotImplementedException(); }
		}

		public virtual Encoding ContentEncoding {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public virtual int ContentLength {
			get { throw new NotImplementedException(); }
		}

		public virtual string ContentType {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public virtual HttpCookieCollection Cookies {
			get { throw new NotImplementedException(); }
		}

		public virtual string CurrentExecutionFilePath {
			get { throw new NotImplementedException(); }
		}

		public virtual string FilePath {
			get { throw new NotImplementedException(); }
		}

		public virtual HttpFileCollectionBase Files {
			get { throw new NotImplementedException(); }
		}

		public virtual Stream Filter {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public virtual NameValueCollection Form {
			get { throw new NotImplementedException(); }
		}

		public virtual NameValueCollection Headers {
			get { throw new NotImplementedException(); }
		}

		public virtual string HttpMethod {
			get { throw new NotImplementedException(); }
		}

		public virtual Stream InputStream {
			get { throw new NotImplementedException(); }
		}

		public virtual bool IsAuthenticated {
			get { throw new NotImplementedException(); }
		}

		public virtual bool IsLocal {
			get { throw new NotImplementedException(); }
		}

		public virtual bool IsSecureConnection {
			get { throw new NotImplementedException(); }
		}

		public virtual string this[string key] {
			get { throw new NotImplementedException(); }
		}

		public virtual WindowsIdentity LogonUserIdentity {
			get { throw new NotImplementedException(); }
		}

		public virtual NameValueCollection Params {
			get { throw new NotImplementedException(); }
		}

		public virtual string Path {
			get { throw new NotImplementedException(); }
		}

		public virtual string PathInfo {
			get { throw new NotImplementedException(); }
		}

		public virtual string PhysicalApplicationPath {
			get { throw new NotImplementedException(); }
		}

		public virtual string PhysicalPath {
			get { throw new NotImplementedException(); }
		}

		public virtual NameValueCollection QueryString {
			get { throw new NotImplementedException(); }
		}

		public virtual string RawUrl {
			get { throw new NotImplementedException(); }
		}

		public virtual string RequestType {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public virtual NameValueCollection ServerVariables {
			get { throw new NotImplementedException(); }
		}

		public virtual int TotalBytes {
			get { throw new NotImplementedException(); }
		}

		public virtual Uri Url {
			get { throw new NotImplementedException(); }
		}

		public virtual Uri UrlReferrer {
			get { throw new NotImplementedException(); }
		}

		public virtual string UserAgent {
			get { throw new NotImplementedException(); }
		}

		public virtual string UserHostAddress {
			get { throw new NotImplementedException(); }
		}

		public virtual string UserHostName {
			get { throw new NotImplementedException(); }
		}

		public virtual string[] UserLanguages {
			get { throw new NotImplementedException(); }
		}

		public virtual byte[] BinaryRead(int count) {
			throw new NotImplementedException();
		}

		public virtual int[] MapImageCoordinates(string imageFieldName) {
			throw new NotImplementedException();
		}

		public virtual string MapPath(string virtualPath) {
			throw new NotImplementedException();
		}

		public virtual string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping) {
			throw new NotImplementedException();
		}

		public virtual void SaveAs(string filename, bool includeHeaders) {
			throw new NotImplementedException();
		}

		public virtual void ValidateInput() {
			throw new NotImplementedException();
		}
	}
}