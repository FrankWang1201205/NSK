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
    public partial class PurchaseController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        IBrandService IBrand = new BrandService();
        IWmsService IW = new WmsService();
        IPurchaseTempService IP = new PurchaseTempService();
        IPDFService IPDF = new PDFService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //价格比对
    public partial class PurchaseController : Controller
    {
        public ActionResult Purchase_Temp_List()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Purchase_Temp_Filter MF = new Purchase_Temp_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            PageList<Purchase_Temp> PList = IP.Get_Purchase_Temp_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        [HttpPost]
        public RedirectToRouteResult Purchase_Temp_Create_Post()
        {
            User U = this.MyUser();
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                IP.Batch_Create_Purchase_Temp(ExcelFile, U);
                TempData["Success"] = "导入成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Purchase_Temp_List");
        }

        public ActionResult Purchase_Temp_List_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            List<Purchase_Temp_Search> List = IP.Get_Purchase_Temp_Search_List(U);
            return View(List);
        }

        [HttpPost]
        public RedirectToRouteResult Purchase_Temp_Batch_Search_Post()
        {
            User U = this.MyUser();
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                IP.Batch_Search_Purchase_Temp(ExcelFile, U);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Purchase_Temp_List_Search");
        }

        [HttpPost]
        public RedirectToRouteResult Purchase_Temp_Batch_Search_Abandon_Post()
        {
            User U = this.MyUser();
            try
            {
                IP.Abandon_Search_Purchase_Temp(U);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Purchase_Temp_List");
        }

        public ActionResult Purchase_Temp_Batch_Search_To_Excel()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            string Path = IP.Get_Batch_Search_Purchase_Temp_To_Excel(U);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("yyyy-MM-dd") + "批量查询结果" + ".xlsx");
        }
    }

    //型号比对
    public partial class PurchaseController : Controller
    {
        public ActionResult Purchase_Temp_MatSn_List()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            MF.Other_MatSn = Request["Other_MatSn"] == null ? string.Empty : Request["Other_MatSn"].Trim();

            MF.BrandList = IBrand.Get_Brand_Name_List(MF.LinkMainCID);
            PageList<Material> PList = IMat.Get_Material_PageList_Temp(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Purchase_Temp_MatSn_List_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            List<Purchase_Temp_Search> List = IP.Get_Purchase_Temp_Search_List(U);
            return View(List);
        }

        [HttpPost]
        public RedirectToRouteResult Purchase_Temp_MatSn_Batch_Search_Post()
        {
            User U = this.MyUser();
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                IP.Batch_Search_Purchase_Temp_MatSn(ExcelFile, U);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Purchase_Temp_MatSn_List_Search");
        }

        public ActionResult Purchase_Temp_MatSn_List_Search_Print()
        {
            string text = string.Empty;

            List<Guid> IDList = CommonLib.GuidListStrToGuidArray(Request.Form["PTS_ID"]);

            try { text = IPDF.Create_Material_Label_List(MyUser().LinkMainCID, IDList); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + IDList.FirstOrDefault() + "产品型号打印.pdf");
            return File(text, "application/pdf");
        }
    }
}