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
    public partial class WMS_Profit_LossController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        IMaterialService IM = new MaterialService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //盈亏核验
    public partial class WMS_Profit_LossController : Controller
    {
        public ActionResult WMS_Profit_Or_Loss_Check()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Status = WMS_Profit_Loss_Status_Enum.未确定.ToString();
            PageList<WMS_Profit_Loss> PList = IW.Get_WMS_Profit_Loss_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        [HttpPost]
        public string WMS_Profit_Loss_Delete_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid LineID = ID;
                IW.Delete_WMS_Profit_Loss_Item(LineID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Profit_Loss_Confirm_Post(Guid ID)
        {
            User U = this.MyUser();

            string result = string.Empty;
            try
            {
                Guid LineID = ID;
                IW.Confirm_WMS_Profit_Loss_Item(LineID, U);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //盈亏记录
    public partial class WMS_Profit_LossController : Controller
    {
        public ActionResult WMS_Profit_Or_Loss()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Status = WMS_Profit_Loss_Status_Enum.已确定.ToString();
            PageList<WMS_Profit_Loss> PList = IW.Get_WMS_Profit_Loss_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }
    
}