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
	public class HttpApplicationStateWrapper : HttpApplicationStateBase {
		private readonly HttpApplicationState _application;

		public override string[] AllKeys {
			get { return _application.AllKeys; }
		}

		public override HttpApplicationStateBase Contents {
			get { return this; }
		}

		public override int Count {
			get { return _application.Count; }
		}

		public override bool IsSynchronized {
			get { return ((ICollection)_application).IsSynchronized; }
		}

		public override object this[int index] {
			get { return _application[index]; }
		}

		public override object this[string name] {
			get { return _application[name]; }
			set { _application[name] = value; }
		}

		public override KeysCollection Keys {
			get { return _application.Keys; }
		}

		public override HttpStaticObjectsCollectionBase StaticObjects {
			get { return new HttpStaticObjectsCollectionWrapper(_application.StaticObjects); }
		}

		public override object SyncRoot {
			get { return ((ICollection)_application).SyncRoot; }
		}


		public HttpApplicationStateWrapper(HttpApplicationState httpApplicationState) {
			if (httpApplicationState == null) {
				throw new ArgumentNullException("httpApplicationState");
			}
			_application = httpApplicationState;
		}

		public override void Add(string name, object value) {
			_application.Add(name, value);
		}

		public override void Clear() {
			_application.Clear();
		}

		public override void CopyTo(Array array, int index) {
			((ICollection)_application).CopyTo(array, index);
		}

		public override object Get(int index) {
			return _application.Get(index);
		}

		public override object Get(string name) {
			return _application.Get(name);
		}

		public override IEnumerator GetEnumerator() {
			return _application.GetEnumerator();
		}

		public override string GetKey(int index) {
			return _application.GetKey(index);
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			_application.GetObjectData(info, context);
		}

		public override void Lock() {
			_application.Lock();
		}

		public override void OnDeserialization(object sender) {
			_application.OnDeserialization(sender);
		}

		public override void Remove(string name) {
			_application.Remove(name);
		}

		public override void RemoveAll() {
			_application.RemoveAll();
		}

		public override void RemoveAt(int index) {
			_application.RemoveAt(index);
		}

		public override void Set(string name, object value) {
			_application.Set(name, value);
		}

		public override void UnLock() {
			_application.UnLock();
		}
	}
}