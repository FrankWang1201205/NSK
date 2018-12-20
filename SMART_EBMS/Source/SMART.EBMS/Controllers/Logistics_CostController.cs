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
    public partial class Logistics_CostController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //快递费用
    public partial class Logistics_CostController : Controller
    {
        public ActionResult Logistics_Cost()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Logistics_Cost_Filter MF = new Logistics_Cost_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.Year = Request["Year"] == null ? DateTime.Now.ToString("yyyy") : Request["Year"].Trim();
            MF.TrackingType = Request["TrackingType"] == null ? string.Empty : Request["TrackingType"].Trim();
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            Logistics_Cost_Year Year = IW.GetLogistics_Cost_YearList(MF);
            ViewData["MF"] = MF;
            return View(Year);
        }

        public ActionResult Logistics_Cost_Sub_ALL()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            DateTime SD = DateTime.Now;
            try { SD = Convert.ToDateTime(Request["SD"].Trim()); } catch { }

            DateTime ED = DateTime.Now;
            try { ED = Convert.ToDateTime(Request["ED"].Trim()); } catch { }
            
            ViewData["SD"] = SD;
            ViewData["ED"] = ED;
            List<WMS_Track> List = IW.Get_WMS_In_Track_By_Month(SD, ED, Guid.Empty, U.LinkMainCID);
            return View(List);
        }

        public ActionResult Logistics_Cost_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            DateTime SD = DateTime.Now;
            try { SD = Convert.ToDateTime(Request["SD"].Trim()); } catch { }

            DateTime ED = DateTime.Now;
            try { ED = Convert.ToDateTime(Request["ED"].Trim()); } catch { }

            Guid LinkMainCID = this.MyUser().LinkMainCID;
            Guid Logistics_ID = ID;
            ViewData["Logistics_ID"] = Logistics_ID;
            ViewData["SD"] = SD;
            ViewData["ED"] = ED;

            List<WMS_Track> List = IW.Get_WMS_In_Track_By_Month(SD, ED, Logistics_ID, LinkMainCID);
            return View(List);
        }

        public ActionResult Logistics_Cost_Sub_ALL_To_Excel()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            DateTime SD = DateTime.Now;
            try { SD = Convert.ToDateTime(Request["SD"].Trim()); } catch { }

            DateTime ED = DateTime.Now;
            try { ED = Convert.ToDateTime(Request["ED"].Trim()); } catch { }

            string Path = IW.Get_WMS_In_Track_By_Month_To_Excel(SD, ED, Guid.Empty, U.LinkMainCID);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "快递费用" + ".xlsx");
        }

        public ActionResult Logistics_Cost_Sub_To_Excel(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            DateTime SD = DateTime.Now;
            try { SD = Convert.ToDateTime(Request["SD"].Trim()); } catch { }

            DateTime ED = DateTime.Now;
            try { ED = Convert.ToDateTime(Request["ED"].Trim()); } catch { }

            Guid Logistics_ID = ID;
            ViewData["Logistics_ID"] = Logistics_ID;
            WMS_Logistics Log = IW.Get_WMS_Logistics_Item(Logistics_ID);

            string Path = IW.Get_WMS_In_Track_By_Month_To_Excel(SD, ED, Log.Log_ID, U.LinkMainCID);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Log.Company_Name + "快递费用" + ".xlsx");
        }
       
    }

    //驾驶员费用
    public partial class Logistics_CostController : Controller
    {
        public ActionResult Driver_Cost()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Logistics_Cost_Filter MF = new Logistics_Cost_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.SD = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.ED = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.TrackingType = Request["TrackingType"] == null ? string.Empty : Request["TrackingType"].Trim();
            MF.Tracking_No= Request["Tracking_No"] == null ? string.Empty : Request["Tracking_No"].Trim();
            MF.Com_Name = Request["Com_Name"] == null ? string.Empty : Request["Com_Name"].Trim();

            PageList<WMS_Track> PList = IW.Get_WMS_In_Track_With_Driver(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Driver_Cost_To_Excel()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Logistics_Cost_Filter MF = new Logistics_Cost_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 10000;
            MF.LinkMainCID = U.LinkMainCID;
            MF.SD = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.ED = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.TrackingType = Request["TrackingType"] == null ? string.Empty : Request["TrackingType"].Trim();

            string Path = IW.Get_WMS_In_Track_With_Driver_To_Excel(MF);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "快递费用.xlsx");
        }
    }

    //出库快递费用
    public partial class Logistics_CostController : Controller
    {
        public ActionResult Logistics_Cost_WMS_Out()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Logistics_Cost_Filter MF = new Logistics_Cost_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.SD = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.ED = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.TrackingType = Tracking_Type_Enum.送货.ToString();
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Com_Name = Request["Com_Name"] == null ? string.Empty : Request["Com_Name"].Trim();
            PageList<WMS_Track> PList = IW.Get_WMS_Out_Track_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Logistics_Cost_WMS_Out_To_Excel()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Logistics_Cost_Filter MF = new Logistics_Cost_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 10000;
            MF.LinkMainCID = U.LinkMainCID;
            MF.SD = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.ED = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.TrackingType = Tracking_Type_Enum.送货.ToString();

            string Path = IW.Get_WMS_Out_Track_To_Excel(MF);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "出库快递重量.xlsx");
        }
    }
}