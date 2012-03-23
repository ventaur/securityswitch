// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Collections;
using System.IO;
using System.Web;


namespace SecuritySwitch.Abstractions {
	public class HttpStaticObjectsCollectionWrapper : HttpStaticObjectsCollectionBase {
		private readonly HttpStaticObjectsCollection _collection;

		public override int Count {
			get { return _collection.Count; }
		}

		public override bool IsReadOnly {
			get { return _collection.IsReadOnly; }
		}

		public override bool IsSynchronized {
			get { return _collection.IsSynchronized; }
		}

		public override object this[string name] {
			get { return _collection[name]; }
		}

		public override bool NeverAccessed {
			get { return _collection.NeverAccessed; }
		}

		public override object SyncRoot {
			get { return _collection.SyncRoot; }
		}

		public HttpStaticObjectsCollectionWrapper(HttpStaticObjectsCollection httpStaticObjectsCollection) {
			if (httpStaticObjectsCollection == null) {
				throw new ArgumentNullException("httpStaticObjectsCollection");
			}
			_collection = httpStaticObjectsCollection;
		}

		public override void CopyTo(Array array, int index) {
			_collection.CopyTo(array, index);
		}

		public override IEnumerator GetEnumerator() {
			return _collection.GetEnumerator();
		}

		public override object GetObject(string name) {
			return _collection.GetObject(name);
		}

		public override void Serialize(BinaryWriter writer) {
			_collection.Serialize(writer);
		}
	}
}