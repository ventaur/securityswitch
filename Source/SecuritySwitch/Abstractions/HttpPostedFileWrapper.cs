// =================================================================================
// Copyright © 2004-2012 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.IO;
using System.Web;


namespace SecuritySwitch.Abstractions {
	public class HttpPostedFileWrapper : HttpPostedFileBase {
		private readonly HttpPostedFile _file;

		public override int ContentLength {
			get { return _file.ContentLength; }
		}

		public override string ContentType {
			get { return _file.ContentType; }
		}

		public override string FileName {
			get { return _file.FileName; }
		}

		public override Stream InputStream {
			get { return _file.InputStream; }
		}

		public HttpPostedFileWrapper(HttpPostedFile httpPostedFile) {
			if (httpPostedFile == null) {
				throw new ArgumentNullException("httpPostedFile");
			}
			_file = httpPostedFile;
		}

		public override void SaveAs(string filename) {
			_file.SaveAs(filename);
		}
	}
}