using System;
using System.Web;
using System.Web.Http;

namespace EmpireOneRestAPIFHS
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        // Optional: keep your redirect to Swagger UI
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Url.AbsolutePath == "/")
                Response.Redirect("~/index.html", endResponse: true);
            //if (Request.Url.AbsolutePath == "/")
            //    Response.Redirect("~/swagger/ui/index", endResponse: true);
        }
    }
}
