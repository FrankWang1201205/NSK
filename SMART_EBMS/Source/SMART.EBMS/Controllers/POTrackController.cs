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
    public partial class POTrackController : Controller
    {
        IUserService IU = new UserService();
        IPurchaseService IPur = new PurchaseService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class POTrackController : Controller
    {
        public ActionResult Wait_Bill_Lading()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Po_Head_Filter MF = new Po_Head_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Sup_Name = Request["Sup_Name"] == null ? string.Empty : Request["Sup_Name"].Trim();
            PageList<Po_Wait_Lading_Group> PList = IPur.Get_Po_Wait_Lading_Group(MF);

            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult _B()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public ActionResult _C()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }
    }
}