using System.Web;
using System.Web.Optimization;

namespace FasceMVC
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/i18n/grid.locale-es.js",
                        "~/Scripts/jquery-{version}.slim.min.js",
                        "~/Scripts/jquery.jqGrid.js",
                        "~/Scripts/tooltip.js",
                        "~/Scripts/sweetalert.min.js",
                        "~/Scripts/jquery.tablesorter.js",
                        "~/Scripts/EmisorDatatables.js",
                        "~/Scripts/EmisorProveedorDatatables.js",
                        "~/Scripts/DistribuidorDatatables.js"
                         ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
            "~/Scripts/DataTables/jquery.dataTables.min.js",
            "~/Scripts/DataTables/dataTables.bootstrap4.min.js"));


            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/Content/datatables").Include(
                  "~/Content/DataTables/css/dataTables.bootstrap.min.css",
                  "~/Content/DataTables/css/responsive.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.css",
                     "~/Content/bootstrap.min.css",
                      "~/Content/css/inicio.css",
                      "~/Content/jquery.jqGrid/ui.jqgrid.css",
                      "~/Content/DataTables/css/jquery.dataTables.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery-ui.unobtrusive-{version}.js"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
               "~/Content/themes/base/jquery.ui.core.css",
               "~/Content/themes/base/jquery.ui.datepicker.css",
               "~/Content/themes/base/jquery-ui.css",
               "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}
