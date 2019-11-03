using System.Web;
using System.Web.Optimization;

namespace bob
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                    ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/umd/popper.min.js",
                        "~/Scripts/bootstrap.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/d3").Include(
                        "~/Scripts/d3/d3.min.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        // "~/Scripts/angular-animate.js",
                        "~//Scripts/angular-local-storage.js",
                        "~/Scripts/angular-ui-router.js"
                    ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"
                    ));
            bundles.Add(new ScriptBundle("~/bundles/components").Include(
                        "~/App/bob/app-module.js",
                        "~/App/bob/bob-app.component.js",
                        "~/App/bob/admin/admin.component.js",
                        "~/App/bob/servicios/caece.service.js",
                        "~/App/bob/nav/nav.component.js",
                        "~/App/bob/userstatus/userstatus.component.js",
                        "~/App/bob/login/login.component.js",
                        "~/App/bob/register/register.component.js",
                        "~/App/bob/changepassword/changepassword.component.js",
                        "~/App/bob/planestudio/planestudio.component.js",
                        "~/App/bob/cursos/cursos.component.js",
                        "~/App/bob/cursos/cursos-auto.component.js",
                        "~/App/bob/cursos/cursos-manual.component.js",
                        "~/App/bob/finales/finales.component.js",
                        "~/App/bob/estadisticas/estadisticas.component.js",
                        "~/App/bob/pendientes/pendientes.component.js",
                        "~/App/bob/servicios/auth-interceptor.service.js",
                        "~/App/bob/servicios/auth.service.js"
                    ));
        }
    }
}
