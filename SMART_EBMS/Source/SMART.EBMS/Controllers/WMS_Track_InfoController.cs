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
    public partial class WMS_Track_InfoController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        ICustomerService IC = new CustomerService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class WMS_Track_InfoController : Controller
    {
        public ActionResult WMS_Track_Info_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Track_Info_Filter MF = new Track_Info_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Sender_Name = Request["Sender_Name"] == null ? string.Empty : Request["Sender_Name"].Trim();
            MF.Logistics_Company = Request["Logistics_Company"] == null ? string.Empty : Request["Logistics_Company"].Trim();
            MF.Receiver_Name = Request["Receiver_Name"] == null ? string.Empty : Request["Receiver_Name"].Trim();
            MF.Tracking_No = Request["Tracking_No"] == null ? string.Empty : Request["Tracking_No"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            PageList<WMS_Track_Info> PList = IW.Get_WMS_Track_Info_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }


    }
}