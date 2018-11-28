using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;

namespace SMART.EBMS.Controllers
{
    [Authorize]
    public partial class RFQ_InquiryController : Controller
    {
        IUserService IU = new UserService();
        ICustomerService IC = new CustomerService();
        IMaterialService IMat = new MaterialService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class RFQ_InquiryController : Controller
    {
        public ActionResult RFQ_Apply()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public ActionResult RFQ_Apply_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;

            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            PageList<Material> PList = IMat.Get_Material_Stand_PageList(MF);
            ViewData["MF"] = MF;

            return View(PList);
        }

        public ActionResult RFQ_Report()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;

            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            PageList<Material> PList = IMat.Get_Material_Stand_PageList(MF);
            ViewData["MF"] = MF;

            return View(PList);
        }

        public ActionResult RFQ_Report_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;

            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            PageList<Material> PList = IMat.Get_Material_Stand_PageList(MF);
            ViewData["MF"] = MF;

            return View(PList);
        }


    }


}