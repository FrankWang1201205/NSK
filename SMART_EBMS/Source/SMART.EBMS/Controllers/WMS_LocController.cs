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
    public partial class WMS_LocController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IPDFService IP = new PDFService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class WMS_LocController : Controller
    {
        public ActionResult Loc_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Location_Filter MF = new WMS_Location_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Type = Request["Type"] == null ? string.Empty : Request["Type"].Trim();
            MF.Link_MatSn_Count = Request["Link_MatSn_Count"] == null ? string.Empty : Request["Link_MatSn_Count"].Trim();
            PageList<WMS_Location> List = IW.Get_WMS_Location_PageList(MF);
            ViewData["MF"] = MF;
            return View(List);
        }

        public ActionResult Loc_Search_Print()
        {
            string text = string.Empty;

            List<Guid> IDList = CommonLib.GuidListStrToGuidArray(Request.Form["Loc_ID"]);

            try { text = IP.Create_PDF_For_WMS_Location_List(MyUser().LinkMainCID, IDList); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + IDList.FirstOrDefault() + "库位标签打印.pdf");
            return File(text, "application/pdf");
        }

        public ActionResult Loc_Update()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Location_Filter MF = new WMS_Location_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Type = Request["Type"] == null ? string.Empty : Request["Type"].Trim();
            MF.Link_MatSn_Count = Request["Link_MatSn_Count"] == null ? string.Empty : Request["Link_MatSn_Count"].Trim();
            PageList<WMS_Location> List = IW.Get_WMS_Location_PageList(MF);
            ViewData["MF"] = MF;
            return View(List);
        }

        public PartialViewResult Loc_Update_Add()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Location W = IW.Get_WMS_Location_Empty();
            return PartialView(W);
        }

        [HttpPost]
        public RedirectToRouteResult Loc_Update_Add_Post(FormCollection FC)
        {
            try
            {
                WMS_Location W = new WMS_Location();
                TryUpdateModel(W, FC);
                W.LinkMainCID = MyUser().LinkMainCID;
                Guid Loc_ID = IW.Create_WMS_Location(W);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Loc_Update");
        }

        [HttpPost]
        public RedirectToRouteResult Loc_Update_Batch_Add_Post()
        {
            try
            {
                IW.Batch_Create_WMS_Location(MyUser().LinkMainCID);
                TempData["Success"] = "创建成功！";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Loc_Update");
        }

        public PartialViewResult Loc_Update_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Loc_ID = ID;
            WMS_Location W = IW.Get_WMS_Location_Item(Loc_ID);
            return PartialView(W);
        }

        [HttpPost]
        public RedirectToRouteResult Loc_Update_Sub_Post(Guid ID, FormCollection FC)
        {
            try
            {
                WMS_Location W = new WMS_Location();
                TryUpdateModel(W, FC);
                Guid Loc_ID = ID;
                IW.Set_WMS_Location_Item(Loc_ID, W);
                TempData["Success"] = "更新成功！";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Loc_Update");
        }

        [HttpPost]
        public RedirectToRouteResult Loc_Update_Sub_Delete_Post(Guid ID)
        {
            try
            {
                Guid Loc_ID = ID;
                IW.Delete_WMS_Location(Loc_ID);
                TempData["Success"] = "删除成功！";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Loc_Update");
        }

        public PartialViewResult QRCode_Preview(Guid ID)
        {
            Guid Loc_ID = ID;
            WMS_Location W = IW.Get_WMS_Location_Item(Loc_ID);
            return PartialView(W);
        }

        public ActionResult Loc_Preview_Label_To_PDF(Guid ID)
        {
            Guid Loc_ID = ID;
            WMS_Location Loc = IW.Get_WMS_Location_Item(Loc_ID);

            string text = string.Empty;
            try { text = IP.Create_PDF_For_WMS_Location(Loc); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Loc.Location + "[标签打印].pdf");
            return File(text, "application/pdf");
        }
    }
}