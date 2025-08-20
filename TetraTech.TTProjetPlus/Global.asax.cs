using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TetraTech.TTProjetPlus
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue; // ~2 Go
        }

        protected void Application_AcquireRequestState(Object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            var languageSession = "en";

            if (context != null && context.Session != null)
            {

                    if (context.Session["lang"] != null)
                {
                    languageSession = context.Session["lang"].ToString();
                }
                    else
                {
                  //  var culture = System.Globalization.CultureInfo.InstalledUICulture;
                    var culture2 = Request.UserLanguages[0];
                    if (culture2 == "en" || culture2 == "en-US")
                    {
                    languageSession = "en";
                    Session["lang"] = "en";
                    }
                    else if (culture2 == "fr" || culture2 =="fr-FR" || culture2 == "fr-CA")
                    {
                        languageSession = "fr";
                        Session["lang"] = "fr";

                    }
                    else
                    {
                        languageSession = "en";
                        Session["lang"] = "en";

                    }
                } 
                }
            

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageSession);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(languageSession);
        }
    }
}
