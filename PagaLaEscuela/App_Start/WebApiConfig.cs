using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PagaLaEscuela.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "WebApi",
                routeTemplate: "pagaLaEscuela/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "IntegracionesClubPago",
                routeTemplate: "Pagalaescuela/Service/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            config.Routes.MapHttpRoute(
                name: "ClubPago",
                routeTemplate: "Service/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "TwiApi",
                routeTemplate: "HookPayCard/ReceiveSms",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}