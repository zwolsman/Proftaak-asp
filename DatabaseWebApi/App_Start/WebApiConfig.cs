using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DatabaseLibrary;
using DatabaseWebApi.App_Start;

namespace DatabaseWebApi
{
    public static class WebApiConfig
    {
        private const string DATABASE_USER = "applicatie";
        private const string DATABASE_PASSWORD = "wachtwoord";
        private const string DATABASE_SERVER = "192.168.20.23";
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            DatabaseManager.Initialize(DATABASE_USER, DATABASE_PASSWORD, DATABASE_SERVER);
            config.Formatters.Add(new BrowserJsonFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
