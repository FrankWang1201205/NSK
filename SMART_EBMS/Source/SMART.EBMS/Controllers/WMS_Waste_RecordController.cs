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
    public partial class WMS_Waste_RecordController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        IMaterialService IM = new MaterialService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //报废核验
    public partial class WMS_Waste_RecordController : Controller
    {
        public ActionResult WMS_Waste_Check_For_Accounting()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Waste_Record_Filter MF = new WMS_Waste_Record_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Status = WMS_Waste_Record_Status_Enum.未审核.ToString();
            PageList<WMS_Waste_Record> PList = IW.Get_WMS_Waste_Record_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Waste_Check_For_Manager()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Waste_Record_Filter MF = new WMS_Waste_Record_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Status = WMS_Waste_Record_Status_Enum.审核中.ToString();
            PageList<WMS_Waste_Record> PList = IW.Get_WMS_Waste_Record_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        [HttpPost]
        public string WMS_Waste_Record_Confirm_Post(Guid ID)
        {
            User U = this.MyUser();

            string result = string.Empty;
            try
            {
                Guid LineID = ID;
                IW.Confirm_WMS_Waste_Record_Item(LineID, U);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //报废记录
    public partial class WMS_Waste_RecordController : Controller
    {
        public ActionResult WMS_Waste_Record_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Waste_Record_Filter MF = new WMS_Waste_Record_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Status = WMS_Waste_Record_Status_Enum.已审核.ToString();

            PageList<WMS_Waste_Record> PList = IW.Get_WMS_Waste_Record_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }
    
}