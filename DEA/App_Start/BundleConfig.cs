﻿using System.Web;
using System.Web.Optimization;

namespace DEA
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //                "~/Scripts/jquery-{version}.js"));

            //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //                "~/Scripts/jquery.validate*"));

            //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
            //    // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //                "~/Scripts/modernizr-*"));

            //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //              "~/Scripts/bootstrap.js",
            //              "~/Scripts/respond.js"));

            //    bundles.Add(new StyleBundle("~/Content/css").Include(
            //              "~/Content/bootstrap.css",
            //              "~/Content/site.css"));
            //}
            bundles.Add(new ScriptBundle("~/Js").Include(
                          "~/Scripts/jquery-2.1.4.min.js",
                           "~/Scripts/bootstrap.min.js",
                             "~/Content/dist/js/adminlte.js",
                             "~/Content/dist/js/demo.js"
                          ));

            bundles.Add(new StyleBundle("~/css").Include(
                      "~/Content/bootstrap/bootstrap.min.css",
                      "~/Content/dist/css/AdminLTE.min.css",
                      "~/Content/dist/css/skins/_all-skins.min.css"
                      ));

            //BundleTable.EnableOptimizations = true;
        }
    }
}
