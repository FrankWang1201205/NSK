using SMART.Api;
using SMART.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMART.EBMS.Controllers
{
    [Authorize]
    public partial class WMS_StocktakingController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IMaterialService IMat = new MaterialService();
        IBrandService IBrand = new BrandService();
        IPDFService IP = new PDFService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //盘库任务提醒
    public partial class WMS_StocktakingController : Controller
    {
        public ActionResult WMS_Stocktaking_Task_Temp()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.Type = Request["Type"] == null ? string.Empty : Request["Type"].Trim();

            PageList<WMS_Location> List = IW.Get_WMS_Stocktaking_Task_PageList_Notice(MF);
            ViewData["MF"] = MF;
            return View(List);
        }

        public ActionResult WMS_Stocktaking_Task_Temp_To_Excel()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            string Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();

            string Path = IW.Get_WMS_Stocktaking_To_Excel(U.LinkMainCID, Location);

            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "库位" + Location + "底盘信息.xlsx");
        }

    }
  
    //盘库任务
    public partial class WMS_StocktakingController : Controller
    {
        public ActionResult WMS_Stocktaking_Task()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            MF.Property = WMS_Stock_Task_Property_Enum.日常盘库.ToString();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            PageList<WMS_Stock_Task> TaskList = IW.Get_WMS_Stock_Task_PageList(MF);
            ViewData["MF"] = MF;
            List<WMS_Work_Person> List = IW.Get_WMS_Work_Person_List(MyUser().LinkMainCID);
            ViewData["List"] = List;
            return View(TaskList);
        }

        public ActionResult WMS_Stocktaking_Task_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Task_ID = ID;
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_DB(Task_ID);
            List<WMS_Work_Person> List = IW.Get_WMS_Work_Person_List(MyUser().LinkMainCID);
            ViewData["List"] = List;
            return View(Task);
        }

        public ActionResult WMS_Stocktaking_Task_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid TaskID = ID;
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item(TaskID);
            return View(Task);
        }

        public ActionResult WMS_Stocktaking_Task_Preview_For_MatSn(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid TaskID = ID;
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item(TaskID);
            return View(Task);
        }

        public PartialViewResult WMS_Stocktaking_Task_Preview_For_MatSn_Sub(Guid ID)
        {
            Guid TaskID = ID;
            string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            WMS_Stocktaking Stocktaking = IW.Get_WMS_Stocktaking_Item(TaskID, MatSn);
            return PartialView(Stocktaking);
        }

        [HttpPost]
        public string WMS_Stocktaking_Task_Preview_For_MatSn_Sub_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid TaskID = ID;
                string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
                int Quantity = Convert.ToInt32(Request["Quantity"].Trim());
                IW.Set_WMS_Stocktaking_Task_For_MatSn(TaskID, MatSn, Quantity);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Stocktaking_Sub_Post(Guid ID)
        {
            try
            {
                WMS_Stock_Task Task = new WMS_Stock_Task();
                Task.Task_ID = ID;
                Task.Work_Person = Request["Work_Person"].Trim();
                IW.Set_WMS_Stock_Task_Work_Person(Task);
                TempData["Success"] = "作业人设置成功！";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Stocktaking_Task");
        }

        [HttpPost]
        public string WMS_Stocktaking_Task_To_Stock(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Move_ID = ID;
                IW.Finish_WMS_Stocktaking_Task(Move_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Stocktaking_Task_Delete_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid TaskID = ID;
                IW.Delete_WMS_Stocktaking_Task(TaskID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public PartialViewResult WMS_Stocktaking_Task_Preview_Sub(Guid ID)
        {
            string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            Guid Task_ID = ID;
            List<WMS_Stocktaking_Scan> List = IW.Get_WMS_Stocktaking_Scan_List(Task_ID, MatSn);
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item(Task_ID);
            ViewData["Task"] = Task;
            return PartialView(List);
        }

        [HttpPost]
        public string WMS_Stocktaking_Task_Preview_Sub_Reset(Guid ID)
        {
            string result = string.Empty;
            string MatSn = string.Empty;
            try
            {
                MatSn = Request["MatSn"].Trim();
                Guid Task_ID = ID;
                IW.Reset_WMS_Stocktaking_Task_Scan_By_MatSn(Task_ID, MatSn);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Stocktaking_Scan_Reset_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Task_ID = ID;
                IW.Reset_WMS_Stocktaking_Task_Scan(Task_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Stocktaking_Task_Preview_Reset(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Task_ID = ID;
                IW.Reset_WMS_Stocktaking_Task_Scan(Task_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Stocktaking_Batch_Add_Post(FormCollection FC)
        {
            try
            {
                List<Guid> HeadIDList = CommonLib.GuidListStrToGuidArray(FC["Task_ID"]);
                List<string> Work_Person_List = CommonLib.StringListStrToStringArray(FC["Work_Person"]);
                IW.Batch_Create_WMS_Stocktaking_With_Work_Person(HeadIDList, Work_Person_List);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }

            return RedirectToAction("WMS_Stocktaking_Task");
        }
    }

    //盘库记录
    public partial class WMS_StocktakingController : Controller
    {
        public ActionResult WMS_Stocktaking_Task_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Status = WMS_Stock_Task_Enum.已盘库.ToString();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Property = WMS_Stock_Task_Property_Enum.日常盘库.ToString();
            PageList<WMS_Stock_Task> List = IW.Get_WMS_Stock_Task_PageList(MF);
            ViewData["MF"] = MF;
            return View(List);
        }
    }

    //首次盘库
    public partial class WMS_StocktakingController : Controller
    {
        public ActionResult WMS_Stocktaking_Task_First()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Status = Request["Status"] == null ? WMS_Stock_Task_Enum.未盘库.ToString() : Request["Status"].Trim();
            MF.Property = WMS_Stock_Task_Property_Enum.首次盘库.ToString();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            PageList<WMS_Stock_Task> List = IW.Get_WMS_Stock_Task_PageList(MF);
            ViewData["MF"] = MF;
            return View(List);
        }

        public ActionResult WMS_Stocktaking_Task_First_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Task_ID = ID;
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_DB(Task_ID);
            return View(Task);
        }

        //public ActionResult WMS_Stocktaking_Task_First_To_Excel()
        //{
        //    User U = this.MyUser();
        //    ViewData["User"] = U;
        //    string Path = IW.Get_WMS_Stocktaking_List_To_Excel(U.LinkMainCID);
        //    return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "端数型号表.xlsx");
        //}

        [HttpPost]
        public RedirectToRouteResult WMS_Stocktaking_Task_First_Sub_Post(Guid ID)
        {
            try
            {
                WMS_Stock_Task Task = new WMS_Stock_Task();
                Task.Task_ID = ID;
                Task.Work_Person = Request["Work_Person"].Trim();
                IW.Set_WMS_Stock_Task_Work_Person(Task);
                TempData["Success"] = "作业人设置成功！";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Stocktaking_Task_First");
        }

        public ActionResult WMS_Stocktaking_Task_Preview_First(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid TaskID = ID;
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_First(TaskID);
            return View(Task);
        }

        [HttpPost]
        public string WMS_Stocktaking_Task_To_Stock_First(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid TaskID = ID;
                IW.Finish_WMS_Stocktaking_Task_First(TaskID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult WMS_Stocktaking_Task_First_MatSn_To_PDF(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            string text = string.Empty;
            Guid Scan_ID = ID;
            WMS_Stocktaking_Scan Stock = IW.Get_WMS_Stocktaking_Scan_Item_DB(Scan_ID);

            try { text = IP.Create_Material_Label(Stock.MatSn); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Stock.Scan_ID + "产品标签.pdf");
            return File(text, "application/pdf");
        }

        public ActionResult WMS_Stocktaking_Task_Preview_First_To_Excel(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Task_ID = ID;
            string Path = IW.Get_WMS_Stocktaking_List_To_Excel(Task_ID);

            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "盘库产品清单.xlsx");
        }

        public ActionResult WMS_Stocktaking_Task_Preview_First_All_To_Excel()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            string Path = IW.Get_WMS_Stocktaking_List_All_To_Excel(U.LinkMainCID);

            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "盘库产品清单(全).xlsx");
        }

        public ActionResult WMS_Stocktaking_Task_First_MatSn_List_To_PDF(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            string text = string.Empty;
            Guid TaskID = ID;
            try { text = IP.Create_Material_Label_List(TaskID); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + TaskID + "端数产品标签.pdf");
            return File(text, "application/pdf");
        }
    }

    //临时导入
    public partial class WMS_StocktakingController : Controller
    {
        public ActionResult WMS_Stock_Record_Temp()
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
            PageList<WMS_Stock_Record> List = IW.Get_WMS_Stock_Record_PageList_Temp(MF);
            ViewData["MF"] = MF;
            return View(List);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Stock_Record_Batch_Create_Post()
        {
            User U = this.MyUser();
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                IW.Batch_Create_WMS_Stock_Record(ExcelFile, U);
                TempData["Success"] = "导入成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Stock_Record_Temp");
        }

    }
}