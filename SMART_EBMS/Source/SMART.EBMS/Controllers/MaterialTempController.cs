using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;
using System.Threading;
using System.Data;

namespace SMART.EBMS.Controllers
{
    [Authorize]
    public partial class MaterialTempController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        IBrandService IBrand = new BrandService();
        IWmsService IW = new WmsService();
        IPDFService IP = new PDFService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //单项产品创建
    public partial class MaterialTempController : Controller
    {
        public ActionResult Mat_Add_Single()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            List<Brand> List = IBrand.Get_Brand_List(U.LinkMainCID);
            return View(List);
        }

        public ActionResult Mat_Add_Single_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Brand B = IBrand.Get_Brand_DB(ID);
            return View(B);
        }

        public PartialViewResult Mat_Add_Single_Sub_Part(Guid ID)
        {
            Guid BID = ID;
            Material M = IMat.Get_Material_Empty(BID);
            Brand B = IBrand.Get_Brand_DB(M.Link_BID);

            if (B.Is_Customized_Page == 1)
            {
                return PartialView("Mat_Add_Single_Sub_Part_" + B.Brand_Name, M);
            }
            else
            {
                return PartialView(M);
            }
        }

        public string Mat_Add_Single_Sub_Post(FormCollection FC)
        {
            string result = string.Empty;
            User U = this.MyUser();
            try
            {
                Material Mat = new Material();
                TryUpdateModel(Mat, FC);
                IMat.Create_Material_Temp(Mat, U);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //产品更新模块
    public partial class MaterialTempController : Controller
    {
        public ActionResult Mat_Update()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;

            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.Price_Is_AM = Request["Price_Is_AM"] == null ? string.Empty : Request["Price_Is_AM"].Trim();
            MF.Is_Stock = Request["Is_Stock"] == null ? string.Empty : Request["Is_Stock"].Trim();
            MF.PC_Place = Request["PC_Place"] == null ? string.Empty : Request["PC_Place"].Trim();
            MF.PC = Request["PC"] == null ? string.Empty : Request["PC"].Trim();
            MF.CODE = Request["CODE"] == null ? string.Empty : Request["CODE"].Trim();
            MF.PC_Mon_Type = Request["PC_Mon_Type"] == null ? string.Empty : Request["PC_Mon_Type"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            MF.Keyword = string.Empty;
            MF.BrandList = IBrand.Get_Brand_Name_List(MF.LinkMainCID);
            MF.Other_MatSn = Request["Other_MatSn"] == null ? string.Empty : Request["Other_MatSn"].Trim();
            PageList<Material> PList = IMat.Get_Material_Stand_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Mat_Update_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid MatID = ID;
            Material Mat = IMat.Get_Material_Item(MatID);
            return View(Mat);
        }

        public string Mat_Update_Sub_Post(Guid ID, FormCollection FC)
        {
            string result = string.Empty;
            User U = this.MyUser();
            try
            {
                Guid MatID = ID;
                Material Mat = new Material();
                TryUpdateModel(Mat, FC);
                IMat.Set_Material_Base_Temp(MatID, Mat, U);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }

            return result;
        }

        public RedirectToRouteResult Mat_Update_Sub_Delete(Guid ID, FormCollection FC)
        {
            User U = this.MyUser();
            try
            {
                Guid MatID = ID;
                IMat.Delete_Material_Temp(MatID);
                TempData["Success_Delete"] = "产品删除成功";
                return RedirectToAction("Mat_Update");
            }
            catch (Exception Ex)
            {
                TempData["Error_Delete"] = Ex.Message.ToString();
                return RedirectToAction("Mat_Update_Sub", new { ID = ID });
            }
        }
    }

    //批量产品Excel上传
    public partial class MaterialTempController : Controller
    {
        public ActionResult Mat_Batch()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Brand_Filter MF = new Brand_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            PageList<Brand> PList = IBrand.Get_Brand_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        [HttpPost]
        public RedirectToRouteResult Mat_Batch_Post()
        {
            User U = this.MyUser();
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                IMat.Mat_Excel_UpLoad_Temp_Various_Brands(ExcelFile, U.UID);
                TempData["Success"] = "上传成功！";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Mat_Batch");
        }

        public ActionResult Mat_Batch_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Brand B = IBrand.Get_Brand_DB(ID);

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Link_BID = B.BID;
            MF.MatBrand = B.Brand_Name;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            
            PageList<Material> PList = IMat.Get_Material_PageList_Temp(MF);
            ViewData["MF"] = MF;

            Material M = IMat.Get_Material_Empty(B.BID);
            ViewData["M"] = M;
            return View(PList);
        }

        [HttpPost]
        public RedirectToRouteResult Mat_Batch_Sub_Post(Guid ID)
        {
            User U = this.MyUser();
            Guid BID = ID;
            try
            {
                string MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                IMat.Mat_Excel_UpLoad_Temp(ExcelFile, U.UID, BID, MatName);
                TempData["Success"] ="上传成功！";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Mat_Batch_Sub", new { ID = ID });
        }
      
    }

    //产品查询
    public partial class MaterialTempController : Controller
    {
        public ActionResult WMS_Mat_Search()
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
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();

            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            MF.Other_MatSn = Request["Other_MatSn"] == null ? string.Empty : Request["Other_MatSn"].Trim();

            MF.BrandList = IBrand.Get_Brand_Name_List(MF.LinkMainCID);
            PageList<Material> PList = IMat.Get_Material_PageList_Temp(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult WMS_Mat_Search_QRCode(Guid ID)
        {
            Guid MatID = ID;
            Material Mat = IMat.Get_Material_Item_DB(MatID);
            Mat.QRCode_Path = IW.Get_QRCode(Mat.MatSn);

            return PartialView(Mat);
        }

        public ActionResult WMS_Mat_Search_QRCode_To_PDF(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            string text = string.Empty;
            Guid MatID = ID;
            Material Mat = IMat.Get_Material_Item_DB(MatID);

            try { text = IP.Create_Material_Label(Mat.MatSn); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Mat.MatID + "产品标签.pdf");
            return File(text, "application/pdf");
        }

        public RedirectToRouteResult WMS_Mat_Search_Post_Temp()
        {
            User U = this.MyUser();
            try
            {
                IMat.Set_Mat_Other_MatSn_ALL(U.LinkMainCID);
                TempData["Success"] = "产品刷新成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }

            return RedirectToAction("WMS_Mat_Search");
        }
        
        public RedirectToRouteResult WMS_Mat_Search_To_Excel_Post(FormCollection FC)
        {
            User U = this.MyUser();
            try
            {
                string Brand = FC["Brand"];
                return RedirectToAction("WMS_Mat_Search_To_Excel", new { ID = Brand });
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("WMS_Mat_Search");
            }

        }

        public ActionResult WMS_Mat_Search_To_Excel(string ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            string Brand = ID;
            string Path = IMat.Get_Mat_List_ToExcel(U.LinkMainCID, Brand);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Brand + "_产品资料" + ".xlsx");
        }
    }
}