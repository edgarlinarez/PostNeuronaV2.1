using System.Web;
using System.Web.Optimization;

namespace POSTNeurona
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/Jquery/jquery-3.6.0.min.js"));

            bundles.Add(new Bundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Validate/jquery.validate.min.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/formatter").Include(
                      "~/Scripts/Format/jquery.format.js"));

            bundles.Add(new Bundle("~/bundles/mybundle").Include(
                      "~/Scripts/Bootstrap/bootstrap.bundle.min.js"));

            bundles.Add(new Bundle("~/bundles/sweetalert2").Include(
                      "~/Scripts/Sweetalert2/sweetalert2.all.min.js"));

            //bundles.Add(new Bundle("~/bundles/fontAweson").Include(
            //          "~/Scripts/fontawesome/fontawesome.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Bootstrap/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/POSTNeurona.css"));

            //bundles.Add(new StyleBundle("~/Content/fontAweson").Include(
            //          "~/Content/Fontaweson/font-awesome.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/app-main").Include(
                      "~/Scripts/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileSaver").Include(
                    "~/Scripts/FileSaver.js"
                ));
            
        }
    }
}
