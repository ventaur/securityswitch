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
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
	 AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class HttpFileCollectionBase : NameObjectCollectionBase, ICollection {
		public virtual string[] AllKeys {
			get { throw new NotImplementedException(); }
		}

		public virtual HttpPostedFileBase this[int index] {
			get { throw new NotImplementedException(); }
		}

		public virtual HttpPostedFileBase this[string name] {
			get { throw new NotImplementedException(); }
		}

		#region ICollection Members

		public virtual void CopyTo(Array dest, int index) {
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

		public virtual HttpPostedFileBase Get(int index) {
			throw new NotImplementedException();
		}

		public virtual HttpPostedFileBase Get(string name) {
			throw new NotImplementedException();
		}

		public virtual string GetKey(int index) {
			throw new NotImplementedException();
		}
	}
}