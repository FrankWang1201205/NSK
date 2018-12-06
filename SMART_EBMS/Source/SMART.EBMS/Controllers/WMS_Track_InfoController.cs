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

        public PartialViewResult WMS_Track_Info_Search_Sub(Guid ID)
        {
            Guid InfoID = ID;
            WMS_Track_Info Info = IW.Get_WMS_Track_Info_DB(InfoID);
            return PartialView(Info);
        }
    }

    public partial class WMS_Track_InfoController : Controller
    {
        public ActionResult WMS_Track_Info_Upload()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            return View();
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Track_Info_Upload_Post()
        {
            User U = this.MyUser();
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                List<WMS_Track_Info> Info_List = IW.Get_WMS_Track_Info_List_From_Upload(ExcelFile, U);
                string Info_List_Str = Newtonsoft.Json.JsonConvert.SerializeObject(Info_List);
                TempData["Success"] = Info_List_Str;
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }

            return RedirectToAction("WMS_Track_Info_Upload");
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Track_Info_Upload_Finish_Post()
        {
            User U = this.MyUser();
            try
            {
                string List_Str = Request["List"].ToString();
                List<WMS_Track_Info> Info_List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WMS_Track_Info>>(List_Str);
                IW.Batch_Create_WMS_Track_Info(Info_List, U);
                TempData["Success"] = "递交成功";
                return RedirectToAction("WMS_Track_Info_Search");
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("WMS_Track_Info_Upload");
            }
        }
    }
}