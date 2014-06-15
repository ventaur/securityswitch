// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web;


namespace SecuritySwitch.Abstractions {
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
	 AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class HttpApplicationStateBase : NameObjectCollectionBase, ICollection {
		public virtual string[] AllKeys {
			get { throw new NotImplementedException(); }
		}

		public virtual HttpApplicationStateBase Contents {
			get { throw new NotImplementedException(); }
		}

		public virtual object this[string name] {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public virtual object this[int index] {
			get { throw new NotImplementedException(); }
		}

		public virtual HttpStaticObjectsCollectionBase StaticObjects {
			get { throw new NotImplementedException(); }
		}

		#region ICollection Members

		public virtual void CopyTo(Array array, int index) {
			throw new NotImplementedException();
		}

		public override IEnumerator GetEnumerator() {
			throw new NotImplementedException();
		}

		public override int Count {
			get { throw new NotImplementedException(); }
		}

		public virtual bool IsSynchronized {
			get { throw new NotImplementedException(); }
		}

		public virtual object SyncRoot {
			get { throw new NotImplementedException(); }
		}

		#endregion

		public virtual void Add(string name, object value) {
			throw new NotImplementedException();
		}

		public virtual void Clear() {
			throw new NotImplementedException();
		}

		public virtual object Get(int index) {
			throw new NotImplementedException();
		}

		public virtual object Get(string name) {
			throw new NotImplementedException();
		}

		public virtual string GetKey(int index) {
			throw new NotImplementedException();
		}

		public virtual void Lock() {
			throw new NotImplementedException();
		}

		public virtual void Remove(string name) {
			throw new NotImplementedException();
		}

		public virtual void RemoveAll() {
			throw new NotImplementedException();
		}

		public virtual void RemoveAt(int index) {
			throw new NotImplementedException();
		}

		public virtual void Set(string name, object value) {
			throw new NotImplementedException();
		}

		public virtual void UnLock() {
			throw new NotImplementedException();
		}
	}
}