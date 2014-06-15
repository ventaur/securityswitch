// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Security.Permissions;
using System.Web;


namespace SecuritySwitch.Abstractions {
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
	 AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class HttpCachePolicyBase {
		public virtual HttpCacheVaryByContentEncodings VaryByContentEncodings {
			get { throw new NotImplementedException(); }
		}

		public virtual HttpCacheVaryByHeaders VaryByHeaders {
			get { throw new NotImplementedException(); }
		}

		public virtual HttpCacheVaryByParams VaryByParams {
			get { throw new NotImplementedException(); }
		}

		public virtual void AddValidationCallback(HttpCacheValidateHandler handler, object data) {
			throw new NotImplementedException();
		}

		public virtual void AppendCacheExtension(string extension) {
			throw new NotImplementedException();
		}

		public virtual void SetAllowResponseInBrowserHistory(bool allow) {
			throw new NotImplementedException();
		}

		public virtual void SetCacheability(HttpCacheability cacheability) {
			throw new NotImplementedException();
		}

		public virtual void SetCacheability(HttpCacheability cacheability, string field) {
			throw new NotImplementedException();
		}

		public virtual void SetETag(string etag) {
			throw new NotImplementedException();
		}

		public virtual void SetETagFromFileDependencies() {
			throw new NotImplementedException();
		}

		public virtual void SetExpires(DateTime date) {
			throw new NotImplementedException();
		}

		public virtual void SetLastModified(DateTime date) {
			throw new NotImplementedException();
		}

		public virtual void SetLastModifiedFromFileDependencies() {
			throw new NotImplementedException();
		}

		public virtual void SetMaxAge(TimeSpan delta) {
			throw new NotImplementedException();
		}

		public virtual void SetNoServerCaching() {
			throw new NotImplementedException();
		}

		public virtual void SetNoStore() {
			throw new NotImplementedException();
		}

		public virtual void SetNoTransforms() {
			throw new NotImplementedException();
		}

		public virtual void SetOmitVaryStar(bool omit) {
			throw new NotImplementedException();
		}

		public virtual void SetProxyMaxAge(TimeSpan delta) {
			throw new NotImplementedException();
		}

		public virtual void SetRevalidation(HttpCacheRevalidation revalidation) {
			throw new NotImplementedException();
		}

		public virtual void SetSlidingExpiration(bool slide) {
			throw new NotImplementedException();
		}

		public virtual void SetValidUntilExpires(bool validUntilExpires) {
			throw new NotImplementedException();
		}

		public virtual void SetVaryByCustom(string custom) {
			throw new NotImplementedException();
		}
	}
}