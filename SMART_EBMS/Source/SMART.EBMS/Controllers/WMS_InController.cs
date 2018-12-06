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
    public partial class WMS_InController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        ISupplierService IS = new SupplierService();
        IPDFService IP = new PDFService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //收货通知
    public partial class WMS_InController : Controller
    {
        public ActionResult WMS_In_Task()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_In_Filter MF = new WMS_In_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.Global_State = Request["Global_State"] == null ? string.Empty : Request["Global_State"].Trim();
            MF.Logistics_Company = Request["Logistics_Company"] == null ? string.Empty : Request["Logistics_Company"].Trim();
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Supplier = Request["Supplier"] == null ? string.Empty : Request["Supplier"].Trim();
            MF.MatType = Request["MatType"] == null ? string.Empty : Request["MatType"].Trim();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Work_Distribution_Status = Request["Work_Distribution_Status"] == null ? string.Empty : Request["Work_Distribution_Status"].Trim();
            MF.Brand_List = IB.Get_Brand_Name_List(MF.LinkMainCID);
            MF.Head_Type = Request["Head_Type"] == null ? string.Empty : Request["Head_Type"].Trim();
            PageList<WMS_In_Task> PList = IW.Get_WMS_In_Task_PageList_Distribute(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult WMS_In_Task_Set(Guid ID)
        {
            Guid Head_ID = ID;
            WMS_In_Head Head = IW.Get_WMS_In_Head_DB_With_Work_Person(Head_ID);
            List<WMS_Work_Person> List = IW.Get_WMS_Work_Person_List(MyUser().LinkMainCID);
            ViewData["List"] = List;

            string List_Json = Newtonsoft.Json.JsonConvert.SerializeObject(List);
            List<WMS_Work_Person> Driver_List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WMS_Work_Person>>(List_Json);
            ViewData["Driver_List"] = Driver_List;

            return PartialView(Head);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_In_Task_Set_Post(Guid ID, FormCollection FC)
        {
            try
            {
                List<string> Person_List = CommonLib.StringListStrToStringArray(FC["Person_Name"]);
                List<string> Drive_List = CommonLib.StringListStrToStringArray(FC["Driver_Name"]);
                Guid Head_ID = ID;
                IW.Set_WMS_In_Head_Item(Head_ID, Person_List, Drive_List);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }

            return RedirectToAction("WMS_In_Task");
        }

        public ActionResult WMS_In_Task_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            ViewData["MF"] = MF;

            Guid Head_ID = ID;
            WMS_In_Task T = IW.Get_WMS_In_Task_Item(Head_ID, MF);
            return View(T);
        }
    }

    //收货入库
    public partial class WMS_InController : Controller
    {
        public ActionResult WMS_In_Operate()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_In_Filter MF = new WMS_In_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.Global_State = Request["Global_State"] == null ? string.Empty : Request["Global_State"].Trim();
            MF.Logistics_Company = Request["Logistics_Company"] == null ? string.Empty : Request["Logistics_Company"].Trim();
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Supplier = Request["Supplier"] == null ? string.Empty : Request["Supplier"].Trim();
            MF.MatType = Request["MatType"] == null ? string.Empty : Request["MatType"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Brand_List = IB.Get_Brand_Name_List(MF.LinkMainCID);
            MF.Head_Type = Request["Head_Type"] == null ? string.Empty : Request["Head_Type"].Trim();
            PageList<WMS_In_Task> PList = IW.Get_WMS_In_Task_PageList_Distribute(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult WMS_In_Operate_Set(Guid ID)
        {
            Guid Head_ID = ID;
            WMS_In_Head Head = IW.Get_WMS_In_Head_DB(Head_ID);
            return PartialView(Head);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_In_Operate_Set_Post(Guid ID, FormCollection FC)
        {
            try
            {
                WMS_In_Head Head = new WMS_In_Head();
                TryUpdateModel(Head, FC);
                Head.Head_ID = ID;
                IW.Set_WMS_In_Head_Item_For_Scan(Head);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }

            return RedirectToAction("WMS_In_Operate");
        }

        public ActionResult WMS_In_Operate_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Line_Status = Request["Line_Status"] == null ? string.Empty : Request["Line_Status"].Trim();
            ViewData["MF"] = MF;

            Guid Head_ID = ID;
            WMS_In_Task T = IW.Get_WMS_In_Task_Item(Head_ID, MF);

            List<WMS_In_Line_Other> Line_Other_List = IW.Get_WMS_In_Line_Other_List(Head_ID);
            ViewData["Line_Other_List"] = Line_Other_List;

            return View(T);
        }

        public ActionResult WMS_In_Operate_Preview_Diff(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Line_Status = Request["Line_Status"] == null ? string.Empty : Request["Line_Status"].Trim();
            ViewData["MF"] = MF;

            Guid Head_ID = ID;
            WMS_In_Task T = IW.Get_WMS_In_Task_Item(Head_ID, MF);

            List<WMS_In_Line_Other> Line_Other_List = IW.Get_WMS_In_Line_Other_List(Head_ID);
            ViewData["Line_Other_List"] = Line_Other_List;

            return View(T);
        }

        public PartialViewResult WMS_In_Operate_Preview_Diff_Sub(Guid ID)
        {
            string New_MatSn = Request["New_MatSn"] == null ? string.Empty : Request["New_MatSn"].Trim();
            ViewData["MatSn"] = New_MatSn;

            string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            Guid Head_ID = ID;
            WMS_In_Line Line = IW.Get_WMS_In_Line_Item(Head_ID, MatSn);
            return PartialView(Line);
        }

        [HttpPost]
        public string WMS_In_Operate_Preview_Diff_Sub_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
                Guid Line_ID = ID;
                IW.Set_WMS_In_Line_Other_Item(Line_ID, MatSn);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult WMS_In_Operate_Preview_Tray(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Line_Status = Request["Line_Status"] == null ? string.Empty : Request["Line_Status"].Trim();
            ViewData["MF"] = MF;

            Guid Head_ID = ID;
            WMS_In_Task T = IW.Get_WMS_In_Task_Item(Head_ID, MF);
            return View(T);
        }

        public PartialViewResult WMS_In_Operate_Preview_Tray_Sub(Guid ID)
        {
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
            Guid Head_ID = ID;
            List<WMS_In_Scan> List = IW.Get_WMS_In_Scan_List_By_Tray_No(Head_ID, Tray_No);
            WMS_In_Head Head = IW.Get_WMS_In_Head_DB(Head_ID);
            ViewData["Head"] = Head;
            return PartialView(List);
        }

        public PartialViewResult WMS_In_Operate_Preview_Tray_Sub_Box(Guid ID)
        {
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
            Guid Head_ID = ID;
            List<WMS_In_Scan> List = IW.Get_WMS_In_Scan_List_By_Tray_No(Head_ID, Tray_No);
            WMS_In_Head Head = IW.Get_WMS_In_Head_DB(Head_ID);
            ViewData["Head"] = Head;
            return PartialView(List);
        }

        [HttpPost]
        public string WMS_In_Operate_Preview_Tray_Sub_Reset(Guid ID)
        {
            string result = string.Empty;
            try
            {
                string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
                ViewData["Tray_No"] = Tray_No;
                Guid Head_ID = ID;
                IW.Reset_Task_Bat_Tray_No(Head_ID, Tray_No);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_In_Operate_Preview_Tray_Sub_Reset_By_Box(Guid ID)
        {
            string result = string.Empty;
            try
            {
                string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
                string Case_No = Request["Case_No"] == null ? string.Empty : Request["Case_No"].Trim();
                ViewData["Tray_No"] = Tray_No;
                Guid Head_ID = ID;
                IW.Reset_Task_Bat_Tray_No_By_Box(Head_ID, Tray_No, Case_No);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult WMS_In_Operate_Preview_Track(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Line_Status = Request["Line_Status"] == null ? string.Empty : Request["Line_Status"].Trim();
            ViewData["MF"] = MF;

            Guid Head_ID = ID;
            WMS_In_Task T = IW.Get_WMS_In_Task_Item(Head_ID, MF);
            return View(T);
        }

        public ActionResult WMS_In_Operate_Preview_Source(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Line_Status = Request["Line_Status"] == null ? string.Empty : Request["Line_Status"].Trim();
            ViewData["MF"] = MF;

            Guid Head_ID = ID;
            WMS_In_Task T = IW.Get_WMS_In_Task_Item(Head_ID, MF);
            return View(T);
        }

        public ActionResult WMS_In_Operate_Preview_Scan(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Line_Status = Request["Line_Status"] == null ? string.Empty : Request["Line_Status"].Trim();
            ViewData["MF"] = MF;

            Guid Head_ID = ID;
            WMS_In_Task T = IW.Get_WMS_In_Task_Item(Head_ID, MF);
            return View(T);
        }

        public ActionResult WMS_In_Operate_Preview_To_Excel(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Head_ID = ID;
            string Path = IW.Get_Task_List_To_Excel_Temp(Head_ID);
            WMS_In_Head Head = IW.Get_WMS_In_Head_DB(Head_ID);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Head.Task_Bat_No_Str + "_收货明细" + ".xlsx");

        }

        public ActionResult WMS_In_Operate_Preview_To_Excel_By_TrayNo(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Head_ID = ID;
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();

            string Path = IW.Get_Task_List_To_Excel_Temp_By_TrayNo(Head_ID, Tray_No);
            WMS_In_Head Head = IW.Get_WMS_In_Head_DB(Head_ID);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Head.Task_Bat_No_Str + "_" + Tray_No + "_收货明细" + ".xlsx");

        }

        public ActionResult WMS_In_Operate_Preview_To_PDF_By_TrayNo(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Head_ID = ID;
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();

            string text = string.Empty;
            try { text = IP.Create_PDF_For_WMS_In_By_TrayNo(Head_ID, Tray_No); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Tray_No + "[标签打印].pdf");
            return File(text, "application/pdf");
        }

        public ActionResult WMS_In_Operate_Preview_To_PDF_By_TrayNo_With_CaseNo(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Head_ID = ID;
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
            string Case_No = Request["Case_No"] == null ? string.Empty : Request["Case_No"].Trim();

            string text = string.Empty;
            try { text = IP.Create_PDF_For_WMS_In_By_TrayNo_With_CaseNo(Head_ID, Tray_No, Case_No); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Tray_No + "_" + Case_No + "[标签打印].pdf");
            return File(text, "application/pdf");
        }

        public PartialViewResult WMS_In_Operate_Preview_QRCode(string ID)
        {
            Guid Line_ID = new Guid(ID);
            WMS_In_Line Line = IW.Get_WMS_In_Line_Item(Line_ID);
            return PartialView(Line);
        }

        public PartialViewResult WMS_In_Operate_Preview_Label(Guid ID)
        {
            Guid HeadID = ID;
            string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            WMS_In_Line Line = IW.Get_WMS_In_Line_Item(HeadID, MatSn);
            return PartialView(Line);
        }

        public ActionResult WMS_In_Operate_Preview_Label_To_PDF(Guid ID)
        {
            Guid Line_ID = ID;
            int Quantity = 0;
            try { Quantity = Convert.ToInt32(Request["Quantity"].Trim()); } catch { }
            WMS_In_Line Line = IW.Get_WMS_In_Line_Item(Line_ID, Quantity);

            string text = string.Empty;
            try { text = IP.Create_PDF_For_WMS_In_Label(Line); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Line.Line_ID + "[标签打印].pdf");
            return File(text, "application/pdf");
        }

        [HttpPost]
        public string WMS_In_Operate_Preview_To_WMS_Stock(Guid ID)
        {
            string result = string.Empty;
            Guid Head_ID = ID;
            try
            {
                IW.WMS_In_Task_To_WMS_Stock_Check(Head_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public PartialViewResult WMS_In_Operate_Track_Add(Guid ID)
        {
            WMS_Track Track = IW.Get_WMS_Track_Empty();
            Track.Link_Head_ID = ID;
            return PartialView(Track);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_In_Operate_Track_Add_Post(Guid ID, FormCollection FC)
        {
            try
            {
                WMS_Track Track = new WMS_Track();
                TryUpdateModel(Track, FC);
                Track.Link_Head_ID = ID;
                IW.Add_WMS_In_Track(Track);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_In_Operate_Preview_Track", new { ID = ID });
        }

        public PartialViewResult WMS_In_Operate_Track_Sub(Guid ID)
        {
            Guid Tracking_ID = ID;
            WMS_Track Track = IW.Get_WMS_Track_Item(Tracking_ID);
            return PartialView(Track);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_In_Operate_Track_Sub_Post(Guid ID)
        {
            try
            {
                WMS_Track Track = new WMS_Track();
                Track.Tracking_ID = new Guid(Request["Tracking_ID"].Trim());
                Track.Logistics_Cost = Convert.ToDecimal(Request["Logistics_Cost"].Trim());
                Track.Kilometers = Convert.ToDecimal(Request["Kilometers"].Trim());
                IW.Set_WMS_In_Track_Item(Track);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_In_Operate_Preview_Track", new { ID = ID });
        }

        [HttpPost]
        public RedirectToRouteResult WMS_In_Operate_Track_Sub_Delete_Post(Guid ID)
        {
            try
            {
                Guid Tracking_ID = new Guid(Request["Tracking_ID"].Trim());
                IW.Delete_WMS_In_Track(Tracking_ID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_In_Operate_Preview_Track", new { ID = ID });
        }

    }

    //收货记录
    public partial class WMS_InController : Controller
    {
        public ActionResult WMS_In_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_In_Filter MF = new WMS_In_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.Global_State = WMS_In_Global_State_Enum.完成入库.ToString();
            MF.Logistics_Company = Request["Logistics_Company"] == null ? string.Empty : Request["Logistics_Company"].Trim();
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Supplier = Request["Supplier"] == null ? string.Empty : Request["Supplier"].Trim();
            MF.MatType = Request["MatType"] == null ? string.Empty : Request["MatType"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Head_Type = Request["Head_Type"] == null ? string.Empty : Request["Head_Type"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            PageList<WMS_In_Task> PList = IW.Get_WMS_In_Task_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }

    //作业人管理
    public partial class WMS_InController : Controller
    {
        public ActionResult WMS_Person_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Work_Person_Filter MF = new WMS_Work_Person_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            PageList<WMS_Work_Person> PList = IW.Get_WMS_Work_Person_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult WMS_Person_Add()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Work_Person W = IW.Get_WMS_Work_Person_Empty();
            return PartialView(W);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Person_Add_Post(FormCollection FC)
        {
            try
            {
                WMS_Work_Person W = new WMS_Work_Person();
                TryUpdateModel(W, FC);
                W.LinkMainCID = MyUser().LinkMainCID;
                Guid Person_ID = IW.Create_WMS_Work_Person_Item(W);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Person_Search");
        }

        public PartialViewResult WMS_Person_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Person_ID = ID;
            WMS_Work_Person W = IW.Get_WMS_Work_Person_Item(Person_ID);
            return PartialView(W);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Person_Sub_Post(Guid ID, FormCollection FC)
        {
            try
            {
                WMS_Work_Person W = new WMS_Work_Person();
                TryUpdateModel(W, FC);
                W.Person_ID = ID;
                IW.Update_WMS_Work_Person_Item(W);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Person_Search");
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Person_Sub_Delete_Post(Guid ID)
        {
            try
            {
                Guid Person_ID = ID;
                IW.Delete_WMS_Work_Person_Item(Person_ID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Person_Search");
        }
    }
}