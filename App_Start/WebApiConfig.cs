using System.Web.Http;
using System.Web.Http.Cors;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // 1) CORS – global policy
        var cors = new EnableCorsAttribute(
            "http://localhost:4200,https://localhost:4200," +
            "https://CMSDemo.com,https://www.CMSDemo.com," +
            "https://techinterviewjump.com,https://www.techinterviewjump.com",
            headers: "*",
            methods: "*"
        );
        config.EnableCors(cors);

        // 2) Attribute routes (needed for [RoutePrefix])
        config.MapHttpAttributeRoutes();

        // 3) Conventional fallback route (fine to keep)
        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}
