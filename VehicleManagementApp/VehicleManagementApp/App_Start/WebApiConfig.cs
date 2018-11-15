using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace VehicleManagementApp
{
    public static class WebApiConfig
    {
        //public static void Configure(HttpConfiguration config)
        //{
        //    Register(config);
        //    ConfigureFormatters(config);
        //}

        //private static void ConfigureFormatters(HttpConfiguration config)
        //{
        //    config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new NiceEnumConverter());
        //}


        public static void Register(HttpConfiguration config)
        {
            // TODO: Add any additional configuration code.


            config.EnableCors();
            // Web API routes
            //config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            // WebAPI when dealing with JSON & JavaScript!
            // Setup json serialization to serialize classes to camel (std. Json format)
            //var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //formatter.SerializerSettings.ContractResolver =
            //    new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
   .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters
                .Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }
    }
}
