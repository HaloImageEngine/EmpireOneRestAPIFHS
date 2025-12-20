using System.IO;
using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(EmpireOneRestAPIITJ.SwaggerConfig), "Register")]

namespace EmpireOneRestAPIITJ
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var httpConfig = GlobalConfiguration.Configuration;

            // Enable Swagger JSON generator (returns SwaggerEnabledConfiguration)
            var swagger = httpConfig.EnableSwagger(c =>
            {
                c.SingleApiVersion("v251028.09", "EmpireOneRestAPIITJ (Web API 4.8)");

                // Optional: XML comments (enable in Project Properties → Build)
                var xmlPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin", "EmpireOneRestAPIITJ.XML");
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);

                // Optional niceties
                c.PrettyPrint();
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.UseFullTypeNameInSchemaIds();

                // Optional: API key header
                c.ApiKey("apiKey")
                 .Description("JWT or API key in the Authorization header. Example: Bearer {token}")
                 .Name("Authorization")
                 .In("header");
            });

            // Enable the classic Swagger UI on the returned configuration
            swagger.EnableSwaggerUi(c =>
            {
                // UI at /swagger/ui/index
                c.DocumentTitle("EmpireOne Swagger UI");
                c.EnableApiKeySupport("Authorization", "header");
                // c.DisableValidator(); // optional
            });
        }
    }
}
