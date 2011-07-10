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
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web;


namespace SecuritySwitch.Abstractions {
	public class HttpFileCollectionWrapper : HttpFileCollectionBase {
		private readonly HttpFileCollection _collection;

		public override string[] AllKeys {
			get { return _collection.AllKeys; }
		}

		public override int Count {
			get { return _collection.Count; }
		}

		public override bool IsSynchronized {
			get { return ((ICollection)_collection).IsSynchronized; }
		}

		public override HttpPostedFileBase this[string name] {
			get {
				var httpPostedFile = _collection[name];
				return (httpPostedFile == null ? null : new HttpPostedFileWrapper(httpPostedFile));
			}
		}

		public override HttpPostedFileBase this[int index] {
			get { return new HttpPostedFileWrapper(_collection[index]); }
		}

		public override KeysCollection Keys {
			get { return _collection.Keys; }
		}

		public override object SyncRoot {
			get { return ((ICollection)_collection).SyncRoot; }
		}

		public HttpFileCollectionWrapper(HttpFileCollection httpFileCollection) {
			if (httpFileCollection == null) {
				throw new ArgumentNullException("httpFileCollection");
			}
			_collection = httpFileCollection;
		}

		public override void CopyTo(Array dest, int index) {
			_collection.CopyTo(dest, index);
		}

		public override HttpPostedFileBase Get(int index) {
			return new HttpPostedFileWrapper(_collection.Get(index));
		}

		public override HttpPostedFileBase Get(string name) {
			var httpPostedFile = _collection.Get(name);
			return (httpPostedFile == null ? null : new HttpPostedFileWrapper(httpPostedFile));
		}

		public override IEnumerator GetEnumerator() {
			return _collection.GetEnumerator();
		}

		public override string GetKey(int index) {
			return _collection.GetKey(index);
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			_collection.GetObjectData(info, context);
		}

		public override void OnDeserialization(object sender) {
			_collection.OnDeserialization(sender);
		}
	}
}