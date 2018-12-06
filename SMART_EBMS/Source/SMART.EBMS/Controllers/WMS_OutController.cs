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
    public partial class WMS_OutController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        ICustomerService IC = new CustomerService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //任务分配 
    public partial class WMS_OutController : Controller
    {
        public ActionResult WMS_Out_Task()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.Global_State = Request["Global_State"] == null ? string.Empty : Request["Global_State"].Trim();
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Work_Down_Person = Request["Work_Down_Person"] == null ? string.Empty : Request["Work_Down_Person"].Trim();
            MF.Work_Out_Person = Request["Work_Out_Person"] == null ? string.Empty : Request["Work_Out_Person"].Trim();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Head_Type = Request["Head_Type"] == null ? string.Empty : Request["Head_Type"].Trim();
            PageList<WMS_Out_Head> PList = IW.Get_WMS_Out_Head_PageList_Distribute(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult WMS_Out_Task_Set(Guid ID)
        {
            Guid Head_ID = ID;
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB_With_Work_Person(Head_ID);

            List<WMS_Work_Person> Down_List = IW.Get_WMS_Work_Person_List(MyUser().LinkMainCID);
            ViewData["Down_List"] = Down_List;

            string List_Json = Newtonsoft.Json.JsonConvert.SerializeObject(Down_List);
            List<WMS_Work_Person> Out_List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WMS_Work_Person>>(List_Json);
            ViewData["Out_List"] = Out_List;

            List<WMS_Work_Person> Driver_List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WMS_Work_Person>>(List_Json);
            ViewData["Driver_List"] = Driver_List;

            return PartialView(Head);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Out_Task_Set_Post(Guid ID, FormCollection FC)
        {
            try
            {
                List<string> Down_Person_List = CommonLib.StringListStrToStringArray(FC["Down_Person_Name"]);
                List<string> Out_Person_List = CommonLib.StringListStrToStringArray(FC["Out_Person_Name"]);
                List<string> Drive_List = CommonLib.StringListStrToStringArray(FC["Driver_Name"]);
                Guid Head_ID = ID;
                IW.Set_WMS_Out_Head_With_Work_Person(Head_ID, Down_Person_List, Out_Person_List, Drive_List);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Out_Task");
        }

        public ActionResult WMS_Out_Task_Preview_Pick(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = ID;
            WMS_Out_Task Task = IW.Get_WMS_Out_Task_Item_Pick(MF);
            return View(Task);
        }

        public PartialViewResult WMS_Out_Task_Preview_QRCode()
        {
            string MatSn = Request["MatSn"].Trim();
            string QRCode_Path = IW.Get_QRCode(MatSn);
            ViewData["MatSn"] = MatSn;
            ViewData["QRCode_Path"] = QRCode_Path;
            return PartialView();
        }

        public PartialViewResult WMS_Out_Task_Preview_Pick_Sub(Guid ID)
        {
            string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            Guid Head_ID = ID;
            List<WMS_Out_Pick_Scan> List = IW.Get_WMS_Out_Pick_Scan_List(Head_ID, MatSn);

            return PartialView(List);
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Pick_Finish(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Head_ID = ID;
                IW.WMS_Out_Pick_List_Sub_Finish(Head_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Pick_Location_Finish(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Head_ID = ID;
                IW.WMS_Out_Pick_List_Sub_Finish_Pick_Location(Head_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Pick_Location_Cancel(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Head_ID = ID;
                IW.WMS_Out_Pick_List_Sub_Cancel_Pick_Location(Head_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public PartialViewResult WMS_Out_Task_Preview_Pick_Location_Sub(Guid ID)
        {
            Guid Scan_ID = ID;
            WMS_Out_Pick_Scan Scan = IW.Get_WMS_Out_Pick_Scan_Item(Scan_ID);

            return PartialView(Scan);
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Pick_Location_Sub_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Scan_ID = ID;
                int Quantity = Convert.ToInt32(Request["Quantity"].Trim());
                IW.WMS_Out_Task_Preview_Pick_Location_Set(Scan_ID, Quantity);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Pick_Location_Sub_Delete_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Scan_ID = ID;
                IW.WMS_Out_Task_Preview_Pick_Location_Choose(Scan_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

    }

    //配货进程
    public partial class WMS_OutController : Controller
    {
        public ActionResult WMS_Out_Pick_Process()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.Global_State = WMS_Out_Global_State_Enum.待配货.ToString();
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Work_Down_Person = Request["Work_Down_Person"] == null ? string.Empty : Request["Work_Down_Person"].Trim();
            MF.Head_Type = Request["Head_Type"] == null ? string.Empty : Request["Head_Type"].Trim();
            PageList<WMS_Out_Head> PList = IW.Get_WMS_Out_Head_PageList_Distribute(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Out_Task_Preview_Pick_Scan(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = ID;
            WMS_Out_Task Task = IW.Get_WMS_Out_Task_Item_Pick_Scan(MF);
            return View(Task);
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Pick_Location_Choose_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Head_ID = ID;
                string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
                string Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();

                IW.WMS_Out_Task_Preview_Pick_Location_Choose(Head_ID, MatSn, Location);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Pick_Scan_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Task_ID = ID;
                IW.WMS_Out_Task_Preview_Pick_Scan_Choose(Task_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Pick_Scan_Cancel_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Task_ID = ID;
                IW.WMS_Out_Task_Preview_Pick_Scan_Cancel_Choose(Task_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult WMS_Out_Task_Preview_Pick_Scan_Other(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = ID;
            WMS_Out_Task Task = IW.Get_WMS_Out_Task_Item_Pick_Scan(MF);
            return View(Task);
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Pick_Scan_Check_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Task_ID = ID;
                IW.WMS_Out_Task_Preview_Pick_Scan_Check(Task_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //配货记录
    public partial class WMS_OutController : Controller
    {
        public ActionResult WMS_Out_Pick_Process_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Out_Filter MF = new WMS_Out_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Work_Down_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            PageList<WMS_Out_Pick_Scan> PList = IW.Get_WMS_Out_Pick_Scan_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }

    //验货进程
    public partial class WMS_OutController : Controller
    {
        public ActionResult WMS_Out_Inspection_Process()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.Work_Out_Person = Request["Work_Out_Person"] == null ? string.Empty : Request["Work_Out_Person"].Trim();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Global_State = Request["Global_State"] == null ? string.Empty : Request["Global_State"].Trim();
            MF.Head_Type = Request["Head_Type"] == null ? string.Empty : Request["Head_Type"].Trim();
            PageList<WMS_Out_Head> PList = IW.Get_WMS_Out_Head_PageList_Temp(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult WMS_Out_Inspection_Process_Sub(Guid ID)
        {
            Guid Head_ID = ID;
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            return PartialView(Head);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Out_Inspection_Process_Sub_Post(Guid ID, FormCollection FC)
        {
            try
            {
                WMS_Out_Head Head = new WMS_Out_Head();
                TryUpdateModel(Head, FC);
                Head.Head_ID = ID;
                IW.Set_WMS_Out_Head_With_Scan_Type(Head);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Out_Inspection_Process");
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Out_Inspection_Process_Cancel_Post(FormCollection FC)
        {
            try
            {
                Guid HeadID = new Guid(Request["Head_ID"].Trim());
                IW.Cancel_WMS_Out_Head_Inspection(HeadID);
                TempData["Success"] = "撤销成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Out_Inspection_Process");

        }

        public ActionResult WMS_Out_Task_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = ID;
            WMS_Out_Task Task = IW.Get_WMS_Out_Task_Item(MF);
            ViewData["MF"] = MF;
            return View(Task);
        }

        public PartialViewResult WMS_Out_Task_Preview_Sub(Guid ID)
        {
            string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            Guid HeadID = ID;
            List<WMS_Out_Scan> List = IW.Get_WMS_Out_Scan_List(HeadID, MatSn);
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(HeadID);
            ViewData["Head"] = Head;
            return PartialView(List);
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Sub_Reset(Guid ID)
        {
            string result = string.Empty;
            string MatSn = string.Empty;
            try
            {
                MatSn = Request["MatSn"].Trim();
                Guid Head_ID = ID;
                IW.Reset_WMS_Out_Scan_By_MatSn(Head_ID, MatSn);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult WMS_Out_Task_Preview_Source(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = ID;
            WMS_Out_Task Task = IW.Get_WMS_Out_Task_Item(MF);
            ViewData["MF"] = MF;
            return View(Task);
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Source_Check_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Line_ID = ID;
                IW.WMS_Out_Task_Preview_Source_Check(Line_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult WMS_Out_Task_Preview_Scan(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = ID;
            WMS_Out_Task Task = IW.Get_WMS_Out_Task_Item(MF);
            ViewData["MF"] = MF;
            return View(Task);
        }

        public ActionResult WMS_Out_Task_Preview_Tray(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = ID;
            WMS_Out_Task Task = IW.Get_WMS_Out_Task_Item(MF);
            ViewData["MF"] = MF;
            return View(Task);
        }

        public PartialViewResult WMS_Out_Task_Preview_Tray_Sub(Guid ID)
        {
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
            Guid Head_ID = ID;
            List<WMS_Out_Scan> List = IW.Get_WMS_Out_Scan_List_By_Tray_No(Head_ID, Tray_No);
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            ViewData["Head"] = Head;
            return PartialView(List);
        }

        public PartialViewResult WMS_Out_Task_Preview_Tray_Sub_Box(Guid ID)
        {
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
            Guid Head_ID = ID;
            List<WMS_Out_Scan> List = IW.Get_WMS_Out_Scan_List_By_Tray_No(Head_ID, Tray_No);
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            ViewData["Head"] = Head;
            return PartialView(List);
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_To_WMS_Stock_Check(Guid ID)
        {
            string result = string.Empty;
            Guid Head_ID = ID;
            try
            {
                IW.WMS_Out_Task_To_WMS_Stock_Check(Head_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_To_WMS_Stock(Guid ID)
        {
            string result = string.Empty;
            Guid Head_ID = ID;
            try
            {
                IW.WMS_Out_Task_To_WMS_Stock(Head_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }


        [HttpPost]
        public string WMS_Out_Task_Preview_To_WMS_Stock_Aagin(Guid ID)
        {
            string result = string.Empty;
            Guid Head_ID = ID;
            try
            {
                IW.WMS_Out_Task_To_WMS_Stock_Check_Again(Head_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult WMS_Out_Task_Preview_To_Excel(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Head_ID = ID;
            string Path = IW.Get_Out_Task_List_To_Excel(Head_ID);
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Head.Task_Bat_No_Str + "_出库明细" + ".xlsx");
        }

        public ActionResult WMS_Out_Task_Preview_To_PDF(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Head_ID = ID;
            string text = string.Empty;
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            try { text = IP.Create_PDF_For_WMS_Out_Task(Head); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Head.Customer_Name + "_出库任务明细.pdf");
            return File(text, "application/pdf");
        }

        public ActionResult WMS_Out_Task_Preview_To_Excel_By_Tray(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Head_ID = ID;
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
            string Path = IW.Get_Out_Task_List_To_Excel_By_Tray(Head_ID, Tray_No);
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Head.Task_Bat_No_Str + "_" + Tray_No + "_出库明细" + ".xlsx");
        }

        //装箱明细
        public ActionResult WMS_Out_Task_Preview_To_PDF_By_Tray(Guid ID)
        {
            Guid Head_ID = ID;
            string text = string.Empty;
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();

            try { text = IP.Create_PDF_For_WMS_Out_Task_By_Tray(Head, Tray_No); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Head.Customer_Name + "_" + Tray_No + "_出库任务明细.pdf");
            return File(text, "application/pdf");
        }

        //装箱标签
        public ActionResult WMS_Out_Task_Preview_To_PDF_By_Tray_Label(Guid ID)
        {
            Guid Head_ID = ID;
            string text = string.Empty;
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();

            try { text = IP.Create_PDF_For_WMS_Out_Task_By_Tray_Label(Head, Tray_No); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Head.Customer_Name + "_" + Tray_No + "_出库任务明细.pdf");
            return File(text, "application/pdf");
        }

        //装箱标签
        public ActionResult WMS_Out_Task_Preview_To_PDF_By_Tray_Label_With_Case(Guid ID)
        {
            Guid Head_ID = ID;
            string text = string.Empty;
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
            string Case_No = Request["Case_No"] == null ? string.Empty : Request["Case_No"].Trim();

            try { text = IP.Create_PDF_For_WMS_Out_Task_By_Tray_Label_With_Case(Head, Tray_No, Case_No); }
            catch (Exception e) { throw new Exception(e.Message); }
            Response.AddHeader("content-disposition", "filename=" + Head.Customer_Name + "_" + Tray_No + "_" + Case_No + "_出库任务明细.pdf");
            return File(text, "application/pdf");
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Tray_Sub_Reset(Guid ID)
        {
            string result = string.Empty;
            try
            {
                string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
                ViewData["Tray_No"] = Tray_No;
                Guid Head_ID = ID;
                IW.Reset_Out_Task_Bat_Tray_No(Head_ID, Tray_No);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Out_Task_Preview_Tray_Sub_Reset_By_Box(Guid ID)
        {
            string result = string.Empty;
            try
            {
                string Tray_No = Request["Tray_No"] == null ? string.Empty : Request["Tray_No"].Trim();
                string Case_No = Request["Case_No"] == null ? string.Empty : Request["Case_No"].Trim();

                ViewData["Tray_No"] = Tray_No;
                Guid Head_ID = ID;
                IW.Reset_Out_Task_Bat_Tray_No_By_Box(Head_ID, Tray_No, Case_No);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult WMS_Out_Task_Preview_Track(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = ID;
            WMS_Out_Task Task = IW.Get_WMS_Out_Task_Item(MF);
            ViewData["MF"] = MF;
            return View(Task);
        }

        public PartialViewResult WMS_Out_Track_Add(Guid ID)
        {
            WMS_Track Track = IW.Get_WMS_Track_Empty();
            Track.Link_Head_ID = ID;
            return PartialView(Track);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Out_Track_Add_Post(Guid ID, FormCollection FC)
        {
            try
            {
                WMS_Track Track = new WMS_Track();
                TryUpdateModel(Track, FC);
                Track.Link_Head_ID = ID;
                IW.Add_WMS_Out_Track(Track);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Out_Task_Preview_Track", new { ID = ID });
        }

        public PartialViewResult WMS_Out_Track_Sub(Guid ID)
        {
            Guid Tracking_ID = ID;
            WMS_Track Track = IW.Get_WMS_Track_Item(Tracking_ID);
            return PartialView(Track);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Out_Track_Sub_Post(Guid ID)
        {
            try
            {
                WMS_Track Track = new WMS_Track();
                Track.Tracking_ID = new Guid(Request["Tracking_ID"].Trim());
                Track.Logistics_Cost = Convert.ToDecimal(Request["Logistics_Cost"].Trim());
                Track.Kilometers = Convert.ToDecimal(Request["Kilometers"].Trim());
                IW.Set_WMS_Out_Track_Item(Track);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Out_Task_Preview_Track", new { ID = ID });
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Out_Track_Sub_Delete_Post(Guid ID)
        {
            try
            {
                Guid Tracking_ID = new Guid(Request["Tracking_ID"].Trim());
                IW.Delete_WMS_Out_Track(Tracking_ID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Out_Task_Preview_Track", new { ID = ID });
        }

        public PartialViewResult WMS_Out_Task_Preview_Label(Guid ID)
        {
            Guid HeadID = ID;
            string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            WMS_Out_Line Line = IW.Get_WMS_Out_Line_Item(HeadID, MatSn);
            return PartialView(Line);
        }

        public ActionResult WMS_Out_Task_Preview_Label_To_Modal(Guid ID)
        {
            Guid Line_ID = ID;
            int Quantity = 0;
            try { Quantity = Convert.ToInt32(Request["Quantity"].Trim()); } catch { }
            WMS_Out_Line Line = IW.Get_WMS_Out_Line_Item(Line_ID, Quantity);
            Line.Quantity = Quantity;
            return PartialView(Line);
        }
    }

    //出库记录
    public partial class WMS_OutController : Controller
    {
        public ActionResult WMS_Out_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Customer_Name = Request["Customer_Name"] == null ? string.Empty : Request["Customer_Name"].Trim();
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.Global_State = WMS_Out_Global_State_Enum.已出库.ToString();
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Work_Out_Person = Request["Work_Out_Person"] == null ? string.Empty : Request["Work_Out_Person"].Trim();
            MF.Work_Down_Person = Request["Work_Down_Person"] == null ? string.Empty : Request["Work_Down_Person"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Head_Type = Request["Head_Type"] == null ? string.Empty : Request["Head_Type"].Trim();
            PageList<WMS_Out_Head> PList = IW.Get_WMS_Out_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }

    //出库发运
    public partial class WMS_OutController : Controller
    {
        public ActionResult WMS_Out_Despatch_Process()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Out_Filter MF = new WMS_Out_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.Work_Out_Person = Request["Work_Out_Person"] == null ? string.Empty : Request["Work_Out_Person"].Trim();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Global_State = WMS_Out_Global_State_Enum.待出库.ToString();
            MF.Head_Type = Request["Head_Type"] == null ? string.Empty : Request["Head_Type"].Trim();
            PageList<WMS_Out_Head> PList = IW.Get_WMS_Out_Head_PageList_Distribute(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }
}