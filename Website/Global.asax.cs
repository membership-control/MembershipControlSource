using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;

namespace WebMembership
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DataTables.AspNet.Mvc5.Configuration.RegisterDataTables();

            //log4net.Config.XmlConfigurator.Configure();

            //DisconnectTimeout 
            //Microsoft.AspNet.SignalR.GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(50);
          
        }

    }
}
