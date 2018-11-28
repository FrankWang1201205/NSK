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
    public partial class MaterialController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        IBrandService IBrand = new BrandService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //单项产品创建
    public partial class MaterialController : Controller
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
                TryUpdateModel<Material>(Mat, FC);
                Mat.Mat_CODE_List = new List<Material_CODE>();

                List<Guid> CODE_ID_List = CommonFunctionLib.GuidListStrToGuidArray(Request["Mat_CODE_ID"].ToString());
                Material_CODE CODE = new Material_CODE(); 
                foreach (var CODE_ID in CODE_ID_List)
                {
                    CODE = new Material_CODE();
                    CODE.Mat_CODE_ID = CODE_ID;
                    CODE.CODE = Request["CODE_" + CODE.Mat_CODE_ID.ToString()] == null ? string.Empty : Request["CODE_" + CODE.Mat_CODE_ID.ToString()].Trim();
                    CODE.Order_Window = Request["Order_Window_" + CODE.Mat_CODE_ID.ToString()] == null ? string.Empty : Request["Order_Window_" + CODE.Mat_CODE_ID.ToString()].Trim();
                    try
                    {
                        CODE.Order_Price = Convert.ToDecimal(Request["Order_Price_" + CODE.Mat_CODE_ID.ToString()]);
                    }
                    catch { CODE.Order_Price = 0; }

                    if(!string.IsNullOrEmpty(CODE.CODE))
                    {
                        Mat.Mat_CODE_List.Add(CODE);
                    }
                }

                IMat.Create_Material(Mat, U);
                Thread.Sleep(500);

            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //产品更新模块
    public partial class MaterialController : Controller
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
                TryUpdateModel<Material>(Mat, FC);
                IMat.Set_Material_Base(MatID, Mat, U);
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
                IMat.Delete_Material(MatID);
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
    public partial class MaterialController : Controller
    {
        public ActionResult Mat_Batch()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Brand_Filter MF = new Brand_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            PageList<Brand> PList = IBrand.Get_Brand_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
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

            MF.Link_BID = B.BID;
            MF.MatBrand = B.Brand_Name;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Excel_File_Type = Mat_Excel_File_Type.批量导入.ToString();

            PageList<Mat_Excel> PList = IMat.Get_Mat_Excel_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Mat_Batch_Sub_Record(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid MEID = ID;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Excel_Mat_Status = Request["Excel_Mat_Status"] == null ? Mat_Excel_Mat_Status_Enum.完成导入产品.ToString() : Request["Excel_Mat_Status"].Trim();
            MF.Price_Is_AM = Request["Price_Is_AM"] == null ? string.Empty : Request["Price_Is_AM"].Trim();
            MF.CODE = Request["CODE"] == null ? string.Empty : Request["CODE"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();

            Mat_Excel ME = IMat.Get_Mat_Excel(MEID, MF);
            ViewData["MF"] = MF;
            return View(ME);
        }

        [HttpPost]
        public RedirectToRouteResult Mat_Batch_Sub_Post(Guid ID)
        {
            User U = this.MyUser();
            Guid BID = ID;
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                Guid MEID = IMat.Mat_Excel_UpLoad(ExcelFile, U.UID, BID);
                return RedirectToAction("Mat_Batch_Sub_Check", new { ID = MEID });
            }
            catch (Exception Ex)
            {
                TempData["Error_Batch"] = Ex.Message.ToString();
                return RedirectToAction("Mat_Batch_Sub", new { ID = ID });
            }
        }

        //[HttpPost]
        //public RedirectToRouteResult Mat_Batch_Sub_Post(Guid ID)
        //{
        //    User U = this.MyUser();
        //    Guid BID = ID;
        //    try
        //    {
        //        HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
        //        IMat.Mat_Excel_UpLoad_Temp(ExcelFile, U.UID, BID);
        //    }
        //    catch (Exception Ex)
        //    {
        //        TempData["Error_Batch"] = Ex.Message.ToString();
        //    }
        //    return RedirectToAction("Mat_Batch_Sub", new { ID = ID });
        //}

        public ActionResult Mat_Batch_Sub_Check(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid MEID = ID;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Excel_Mat_Status = Request["Excel_Mat_Status"] == null ? string.Empty : Request["Excel_Mat_Status"].Trim();
            MF.Price_Is_AM = Request["Price_Is_AM"] == null ? string.Empty : Request["Price_Is_AM"].Trim();
            MF.CODE = Request["CODE"] == null ? string.Empty : Request["CODE"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();

            Mat_Excel ME = IMat.Get_Mat_Excel(MEID, MF);
            ViewData["MF"] = MF;
            return View(ME);
        }

        [HttpPost]
        public RedirectToRouteResult Mat_Batch_Sub_Check_Post(Guid ID)
        {
            Guid MEID = ID;
            try
            {
                IMat.Mat_Excel_UpLoad_To_DB(MEID);
                TempData["Success_Mat_To_DB"] = "产品资料导入成功";
            }
            catch (Exception Ex)
            {
                TempData["Error_Mat_To_DB"] = Ex.Message.ToString();
            }
            return RedirectToAction("Mat_Batch_Sub_Check", new { ID = MEID });
        }
    }

    //产品价格批量维护
    public partial class MaterialController : Controller
    {
        public ActionResult Mat_Price()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Brand_Filter MF = new Brand_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            PageList<Brand> PList = IBrand.Get_Brand_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Mat_Price_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Link_BID = ID;

            MF.Price_Is_AM = Request["Price_Is_AM"] == null ? string.Empty : Request["Price_Is_AM"].Trim();
            MF.Is_Stock = Request["Is_Stock"] == null ? string.Empty : Request["Is_Stock"].Trim();
            MF.PC_Place = Request["PC_Place"] == null ? string.Empty : Request["PC_Place"].Trim();
            MF.PC = Request["PC"] == null ? string.Empty : Request["PC"].Trim();
            MF.CODE = Request["CODE"] == null ? string.Empty : Request["CODE"].Trim();
            MF.PC_Mon_Type = Request["PC_Mon_Type"] == null ? string.Empty : Request["PC_Mon_Type"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            MF.MatBrand = IBrand.Get_Brand_DB(MF.Link_BID).Brand_Name;
            PageList<Material> PList = IMat.Get_Material_Stand_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Mat_Price_Sub_Single(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid MatID = ID;
            Material Mat = IMat.Get_Material_Item(MatID);
            Brand B = IBrand.Get_Brand_DB(Mat.Link_BID);
            if (B.Is_Customized_Page == 1)
            {
                return View("Mat_Price_Sub_Single_" + B.Brand_Name, Mat);
            }
            else
            {
                return View(Mat);
            }
        }

        public string Mat_Price_Sub_Single_Post(Guid ID, FormCollection FC)
        {
            string result = string.Empty;

            User U = this.MyUser();
            try
            {
                Guid MatID = ID;
                Material Mat = new Material();
                TryUpdateModel<Material>(Mat, FC);

                List<Material_CODE> CODE_List = new List<Material_CODE>();
                Material_CODE CODE = new Material_CODE();
                List<Guid> CODE_ID_List = CommonLib.GuidListStrToGuidArray(FC["Mat_CODE_ID"]);
                foreach(var CODE_ID in CODE_ID_List)
                {
                    CODE = new Material_CODE();
                    CODE.Mat_CODE_ID = CODE_ID;
                    CODE.CODE = Request["CODE_" + CODE_ID.ToString()] == null ? string.Empty : Request["CODE_" + CODE_ID.ToString()].Trim();
                    CODE.Order_Window = Request["Order_Window_" + CODE_ID.ToString()] == null ? string.Empty : Request["Order_Window_" + CODE_ID.ToString()].Trim();
                    try { CODE.Order_Price = Convert.ToDecimal(Request["Order_Price_" + CODE_ID.ToString()].ToString()); } catch { CODE.Order_Price = 0; }
                    CODE_List.Add(CODE);
                }

                IMat.Set_Material_Price(MatID, Mat, CODE_List, U);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }

            return result;
        }

        public ActionResult Mat_Price_Sub_ToExcel(Guid ID)
        {
            User U = this.MyUser();
           
            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = 1;
            MF.PageSize = 10000;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Link_BID = ID;

            MF.Price_Is_AM = Request["Price_Is_AM"] == null ? string.Empty : Request["Price_Is_AM"].Trim();
            MF.Is_Stock = Request["Is_Stock"] == null ? string.Empty : Request["Is_Stock"].Trim();
            MF.PC_Place = Request["PC_Place"] == null ? string.Empty : Request["PC_Place"].Trim();
            MF.PC = Request["PC"] == null ? string.Empty : Request["PC"].Trim();
            MF.CODE = Request["CODE"] == null ? string.Empty : Request["CODE"].Trim();
            MF.PC_Mon_Type = Request["PC_Mon_Type"] == null ? string.Empty : Request["PC_Mon_Type"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();

            Brand B = IBrand.Get_Brand_DB(MF.Link_BID);
            string Path = IMat.Get_Material_Price_List_ToExcel(MF);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "价格批量维护[" + B.Brand_Name + "][" + DateTime.Now.ToString("yyyy-MM-dd") + "][" + U.UserFullName + "]" + ".xls");
        }

        [HttpPost]
        public RedirectToRouteResult Mat_Price_Sub_ToUpload(Guid ID)
        {
            User U = this.MyUser();
            Guid BID = ID;
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                Guid MEID = IMat.Mat_Price_Excel_UpLoad(ExcelFile, U.UID, BID);
                return RedirectToAction("Mat_Price_Sub_Check", new { ID = MEID });
            }
            catch (Exception Ex)
            {
                TempData["Error_Excel"] = Ex.Message.ToString();
                return RedirectToAction("Mat_Price_Sub", new { ID = ID });
            }
        }

        public ActionResult Mat_Price_Sub_Check(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid MEID = ID;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Excel_Mat_Status = Request["Excel_Mat_Status"] == null ? string.Empty : Request["Excel_Mat_Status"].Trim();
            MF.Price_Is_AM = Request["Price_Is_AM"] == null ? string.Empty : Request["Price_Is_AM"].Trim();
            MF.Is_Stock = Request["Is_Stock"] == null ? string.Empty : Request["Is_Stock"].Trim();
            MF.PC = Request["PC"] == null ? string.Empty : Request["PC"].Trim();
            MF.PC_Place = Request["PC_Place"] == null ? string.Empty : Request["PC_Place"].Trim();
            MF.PC_Mon_Type = Request["PC_Mon_Type"] == null ? string.Empty : Request["PC_Mon_Type"].Trim();
            MF.CODE = Request["CODE"] == null ? string.Empty : Request["CODE"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();

            Mat_Excel ME = IMat.Get_Mat_Excel(MEID, MF);
            ViewData["MF"] = MF;
            return View(ME);
        }

        [HttpPost]
        public RedirectToRouteResult Mat_Price_Sub_Check_Post(Guid ID)
        {
            Guid MEID = ID;
            try
            {
                IMat.Mat_Price_Excel_UpLoad_To_DB(MEID);
                TempData["Success_Price_To_DB"] = "产品价格导入成功";
            }
            catch (Exception Ex)
            {
                TempData["Error_Price_To_DB"] = Ex.Message.ToString();
            }
            return RedirectToAction("Mat_Price_Sub_Check", new { ID = MEID});
        }

        public ActionResult Mat_Price_Record(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Brand B = IBrand.Get_Brand_DB(ID);

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;

            MF.Link_BID = B.BID;
            MF.MatBrand = B.Brand_Name;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Excel_File_Type = Mat_Excel_File_Type.价格维护.ToString();

            PageList<Mat_Excel> PList = IMat.Get_Mat_Excel_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Mat_Price_Record_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid MEID = ID;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Excel_Mat_Status = Request["Excel_Mat_Status"] == null ? Mat_Excel_Mat_Status_Enum.完成导入产品.ToString() : Request["Excel_Mat_Status"].Trim();
            MF.Price_Is_AM = Request["Price_Is_AM"] == null ? string.Empty : Request["Price_Is_AM"].Trim();
            MF.Is_Stock = Request["Is_Stock"] == null ? string.Empty : Request["Is_Stock"].Trim();
            MF.PC = Request["PC"] == null ? string.Empty : Request["PC"].Trim();
            MF.PC_Place = Request["PC_Place"] == null ? string.Empty : Request["PC_Place"].Trim();
            MF.PC_Mon_Type = Request["PC_Mon_Type"] == null ? string.Empty : Request["PC_Mon_Type"].Trim();
            MF.CODE = Request["CODE"] == null ? string.Empty : Request["CODE"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();

            Mat_Excel ME = IMat.Get_Mat_Excel(MEID, MF);
            ViewData["MF"] = MF;
            return View(ME);
        }

        public ActionResult Mat_Price_Record_ToExcel(Guid ID)
        {
            User U = this.MyUser();
            Guid MEID = ID;

            Material_Filter MF = new Material_Filter();
            MF.PageIndex = 1;
            MF.PageSize = 100000;
            MF.LinkMainCID = U.LinkMainCID;

            Mat_Excel ME = IMat.Get_Mat_Excel(MEID, MF);
            Brand B = IBrand.Get_Brand_DB(MF.Link_BID);
            string Path = IMat.Get_Mat_Excel_ToExcel(ME.Mat_Excel_Line_PageList.Rows);
            string File_Name_Sim = ME.File_Name.Replace(".xlsx","");
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", File_Name_Sim + ".xlsx");
        }
    }

    //产品名称创建
    public partial class MaterialController
    {
        public ActionResult Mat_Name()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            PageList<Material_Name> PList = IMat.Get_Material_Name_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Mat_Name_Add()
        {
            return PartialView();
        }

        public RedirectToRouteResult Mat_Name_Add_Post(FormCollection FC)
        {
            User U = this.MyUser();
            try
            {
                Material_Name Mat = new Material_Name();
                TryUpdateModel(Mat, FC);
                IMat.Create_Mat_Name(Mat, U.UID);
                TempData["Success"] = "产品名称创建成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Mat_Name");

        }

        public PartialViewResult Mat_Name_Sub(Guid ID)
        {
            Guid Name_ID = ID;
            Material_Name Mat = IMat.Get_Material_Name_Item_DB(Name_ID);
            return PartialView(Mat);
        }

        public RedirectToRouteResult Mat_Name_Sub_Post(Guid ID, FormCollection FC)
        {
            Guid Name_ID = ID;
            User U = this.MyUser();
            try
            {
                Material_Name Mat = new Material_Name();
                TryUpdateModel(Mat, FC);
                IMat.Set_Material_Name_Item(Name_ID, Mat);
                TempData["Success"] = "产品名称更新成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Mat_Name");
        }

        public RedirectToRouteResult Mat_Name_Sub_Delete(Guid ID)
        {
            Guid Name_ID = ID;
            try
            {
                IMat.Delete_Material_Name(Name_ID);
                TempData["Success"] = "产品名称删除成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Mat_Name");
        }

    }

}