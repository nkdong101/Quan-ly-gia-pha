using API.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;
using API.Models;
using MongoDB.Driver;

namespace API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new NotImplExceptionFilterAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new ValidateViewModelAttribute());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //MongoDBAccess.Global.Init("mongodb://giapha:giapha_viettech_2024@localhost:20182/", "viettech_vanhoaviet");
            MongoDBAccess.Global.Init("mongodb://giapha:giapha_viettech_2024@27.71.225.195:20182/", "viettech_vanhoaviet");
        }
    }
}
