using System.Web.Http;
using Owin;

namespace ExternalLogin
{
    public static class WebApiConfig
    {
        public static void Configure(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(configuration);
        }
    }
}
