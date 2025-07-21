using System.Web;
using System.Web.Optimization;

namespace TetraTech.TTProjetPlus
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/kendo")
                .Include("~/Scripts/kendo/2018.3.1017/kendo.web.min.js")
                .Include("~/Scripts/kendo/2018.3.1017/kendo.aspnetmvc.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/librairiesdatejsFR") 
                .Include("~/Scripts/jquery.inputmask.js")
                .Include("~/Scripts/jquery.ui.datepicker-fr.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/librairiesdatejs")
               .Include("~/Scripts/jquery.inputmask.js")
               .Include("~/Scripts/jquery.ui.datepicker.js")
               );

            bundles.Add(new ScriptBundle("~/bundles/site")
                .Include("~/Scripts/Site.js"));

            bundles.Add(new StyleBundle("~/Content/kendo/2018.3.1017/css")
                .Include("~/Content/kendo/2018.3.1017/kendo.common-bootstrap.min.css")
                .Include("~/Content/kendo/2018.3.1017/kendo.bootstrap.min.css"));


            bundles.Add(new StyleBundle("~/Content/font-awesomecss")
                .Include("~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/site/css")
                .Include("~/Content/site.css"));

            bundles.IgnoreList.Clear();
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}
