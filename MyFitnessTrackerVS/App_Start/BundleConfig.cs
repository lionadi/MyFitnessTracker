using System.Web;
using System.Web.Optimization;

namespace MyFitnessTrackerVS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js")
                        .Include(
                        "~/Scripts/jquery-ui-1.11.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/HighCharts").Include(
                      "~/Scripts/highcharts.js",
                      "~/Content/themes/HighCharts/grid-light.js"));

            bundles.Add(new ScriptBundle("~/bundles/MyFittnessAngularMVCApp")
                //.Include("~/Scripts/angular.js")
                //.Include("~/Scripts/angular-route.min.js")
                //.Include("~/Scripts/angular-local-storage.min.js")
                //.Include("~/Scripts/loading-bar.min.js")
                .Include("~/Scripts/jquery.cookie.js")
                .IncludeDirectory("~/Scripts/MyFitScripts/Helpers", "*.js")
                .IncludeDirectory("~/Scripts/MyFitScripts/controllers", "*.js")
                //.Include("~/Scripts/MyFitScripts/app.js")
                .IncludeDirectory("~/Scripts/MyFitScripts/services", "*.js")
                
                .Include("~/Scripts/MyFitScripts/UserFitnessDataHelper.js")
                
                );

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/themes/flick/jquery-ui.css"));
        }
    }
}
