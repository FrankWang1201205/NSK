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
    public partial class LogisticsController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }
    
    public partial class LogisticsController : Controller
    {
        public ActionResult Logistics_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Logistics_Filter MF = new WMS_Logistics_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Company_Code = Request["Company_Code"] == null ? string.Empty : Request["Company_Code"].Trim();
            MF.MatType = Request["MatType"] == null ? string.Empty : Request["MatType"].Trim();
            PageList<WMS_Logistics> PList = IW.Get_WMS_Logistics_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Logistics_Add()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Logistics W = IW.Get_WMS_Logistics_Empty();
            return PartialView(W);
        }

        [HttpPost]
        public RedirectToRouteResult Logistics_Add_Post(FormCollection FC)
        {
            try
            {
                WMS_Logistics W = new WMS_Logistics();
                TryUpdateModel(W, FC);
                W.LinkMainCID = MyUser().LinkMainCID;
                Guid Log_ID = IW.Create_WMS_Logistics_Item(W);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Logistics_Search");
        }

        public PartialViewResult Logistics_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Log_ID = ID;
            WMS_Logistics W = IW.Get_WMS_Logistics_Item(Log_ID);
            return PartialView(W);
        }

        [HttpPost]
        public RedirectToRouteResult Logistics_Sub_Post(Guid ID, FormCollection FC)
        {
            try
            {
                WMS_Logistics W = new WMS_Logistics();
                TryUpdateModel(W, FC);
                W.Log_ID = ID;
                IW.Update_WMS_Logistics_Item(W);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Logistics_Search");
        }

        [HttpPost]
        public RedirectToRouteResult Logistics_Sub_Delete_Post(Guid ID)
        {
            try
            {
                Guid Log_ID = ID;
                IW.Delete_WMS_Logistics_Item(Log_ID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Logistics_Search");
        }
    }

}