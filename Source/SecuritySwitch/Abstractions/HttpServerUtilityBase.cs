// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;


namespace SecuritySwitch.Abstractions {
	public abstract class HttpServerUtilityBase {
		public virtual string MachineName {
			get { throw new NotImplementedException(); }
		}

		public virtual int ScriptTimeout {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public virtual void ClearError() {
			throw new NotImplementedException();
		}

		public virtual object CreateObject(string progID) {
			throw new NotImplementedException();
		}

		public virtual object CreateObject(Type type) {
			throw new NotImplementedException();
		}

		public virtual object CreateObjectFromClsid(string clsid) {
			throw new NotImplementedException();
		}

		public virtual void Execute(string path) {
			throw new NotImplementedException();
		}

		public virtual void Execute(string path, bool preserveForm) {
			throw new NotImplementedException();
		}

		public virtual void Execute(string path, TextWriter writer) {
			throw new NotImplementedException();
		}

		public virtual void Execute(string path, TextWriter writer, bool preserveForm) {
			throw new NotImplementedException();
		}

		public virtual void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm) {
			throw new NotImplementedException();
		}

		public virtual Exception GetLastError() {
			throw new NotImplementedException();
		}

		public virtual string HtmlDecode(string s) {
			throw new NotImplementedException();
		}

		public virtual void HtmlDecode(string s, TextWriter output) {
			throw new NotImplementedException();
		}

		public virtual string HtmlEncode(string s) {
			throw new NotImplementedException();
		}

		public virtual void HtmlEncode(string s, TextWriter output) {
			throw new NotImplementedException();
		}

		public virtual string MapPath(string path) {
			throw new NotImplementedException();
		}

		public virtual void Transfer(string path) {
			throw new NotImplementedException();
		}

		public virtual void Transfer(string path, bool preserveForm) {
			throw new NotImplementedException();
		}

		public virtual void Transfer(IHttpHandler handler, bool preserveForm) {
			throw new NotImplementedException();
		}

		public virtual void TransferRequest(string path) {
			throw new NotImplementedException();
		}

		public virtual void TransferRequest(string path, bool preserveForm) {
			throw new NotImplementedException();
		}

		public virtual void TransferRequest(string path, bool preserveForm, string method, NameValueCollection headers) {
			throw new NotImplementedException();
		}

		public virtual string UrlDecode(string s) {
			throw new NotImplementedException();
		}

		public virtual void UrlDecode(string s, TextWriter output) {
			throw new NotImplementedException();
		}

		public virtual string UrlEncode(string s) {
			throw new NotImplementedException();
		}

		public virtual void UrlEncode(string s, TextWriter output) {
			throw new NotImplementedException();
		}

		public virtual string UrlPathEncode(string s) {
			throw new NotImplementedException();
		}

		public virtual byte[] UrlTokenDecode(string input) {
			throw new NotImplementedException();
		}

		public virtual string UrlTokenEncode(byte[] input) {
			throw new NotImplementedException();
		}
	}
}