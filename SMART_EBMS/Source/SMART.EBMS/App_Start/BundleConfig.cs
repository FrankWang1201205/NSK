using System.Web;
using System.Web.Optimization;

namespace SMART.EBMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-2.1.3.min.js", "~/Scripts/jquery-ui.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap/js/bootstrap.min.js", "~/Scripts/respond.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include("~/Scripts/bootstrapDatepicker/bootstrap-datetimepicker.min.js", "~/Scripts/bootstrapDatepicker/bootstrap-datetimepicker.zh-CN.js"));
            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include("~/Scripts/dataTables/js/jquery.dataTables.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/Common").Include("~/Scripts/Common.js"));
            bundles.Add(new ScriptBundle("~/bundles/Component").Include("~/Scripts/Component.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Scripts/bootstrap/css/bootstrap.min.css",
                "~/Scripts/Bootstrap/css/font-awesome.min.css",
                "~/Scripts/bootstrapDatepicker/bootstrap-datetimepicker.min.css",
                "~/Scripts/dataTables/css/jquery.dataTables.min.css",
                "~/Content/SiteStyle.css",
                "~/Content/dataTableEx.css"));
        }
    }
}
