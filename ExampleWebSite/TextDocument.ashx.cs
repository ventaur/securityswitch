using System.Web;


namespace ExampleWebSite {
	public class FileDownload : IHttpHandler {
		public bool IsReusable {
			get { return false; }
		}

		public void ProcessRequest(HttpContext context) {
			var textId = context.Request.QueryString["textId"];

			context.Response.ContentType = "text/plain";
			context.Response.Write(string.Format("I'm plain text, as requested by ID: '{0}'.", textId));
			context.Response.End();
		}
	}
}