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
    public partial class WMS_UpController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        IMaterialService IM = new MaterialService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //上架通知
    public partial class WMS_UpController : Controller
    {
        public ActionResult WMS_Up_Start()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.MatType = Request["MatType"] == null ? string.Empty : Request["MatType"].Trim();
            MF.Global_State = Request["Global_State"] == null ? WMS_Move_Status_Enum.待移库.ToString() : Request["Global_State"].Trim();
            PageList<WMS_In_Task> PList = IW.Get_WMS_Up_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Up_Start_To_Excel(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Head_ID = ID;
            string Path = IW.Get_WMS_Up_To_Stock_List_With_Excel(Head_ID);
            WMS_In_Head Head = IW.Get_WMS_In_Head_DB(Head_ID);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Head.Task_Bat_No_Str + "_上架明细" + ".xlsx");

        }

        public ActionResult WMS_Up_Start_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Move_Filter MF = new WMS_Move_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Link_HeadID = ID;
            MF.Move_Status = Request["Move_Status"] == null ? WMS_Move_Status_Enum.待移库.ToString() : Request["Move_Status"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            PageList<WMS_Move> PList = IW.Get_WMS_Up_PageList(MF);
            ViewData["MF"] = MF;
            WMS_In_Head Head = IW.Get_WMS_In_Head_DB(MF.Link_HeadID);
            ViewData["Head"] = Head;
            List<WMS_Work_Person> List = IW.Get_WMS_Work_Person_List(MyUser().LinkMainCID);
            ViewData["List"] = List;
            return View(PList);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Up_Task_Batch_Add_Post(Guid ID, FormCollection FC)
        {
            try
            {
                List<Guid> MoveIDList = CommonLib.GuidListStrToGuidArray(FC["Move_ID"]);
                List<string> Work_Person_List = CommonLib.StringListStrToStringArray(FC["Work_Person"]);
                IW.Batch_Create_WMS_Move_With_Work_Person(MoveIDList, Work_Person_List);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Up_Start_Sub", new { ID = ID });
        }

        public PartialViewResult WMS_Up_Start_Sub_Sub(Guid ID)
        {
            Guid Move_ID = ID;
            WMS_Move Move = IW.Get_WMS_Move_DB(Move_ID);
            List<WMS_Work_Person> List = IW.Get_WMS_Work_Person_List(MyUser().LinkMainCID);
            ViewData["List"] = List;
            return PartialView(Move);
        }

        public ActionResult WMS_Up_Start_Sub_Preview(Guid ID)
        {
            Guid Move_ID = ID;
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_Group_List_For_WMS_Up(Move_ID);
            WMS_Move Move = IW.Get_WMS_Move_DB(ID);
            ViewData["Move"] = Move;
            return View(List);
        }

        public PartialViewResult WMS_Up_Start_Sub_Preview_QRCode()
        {
            string MatSn = Request["MatSn"].Trim();
            string QRCode_Path = IW.Get_QRCode(MatSn);
            ViewData["MatSn"] = MatSn;
            ViewData["QRCode_Path"] = QRCode_Path;
            return PartialView();
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Up_Task_Sub_Post(Guid ID)
        {
            try
            {
                WMS_Move Move = new WMS_Move();
                string Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
                Move.Move_ID = ID;
                Move.Work_Person = Work_Person;
                IW.Set_WMS_Move_With_Work_Person(Move);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Up_Start_Sub");
        }

        public ActionResult WMS_Up_Task_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Move_Filter MF = new WMS_Move_Filter();
            ViewData["MF"] = MF;

            Guid Move_ID = ID;
            WMS_Move_Task T = IW.Get_WMS_Move_Task_Item(Move_ID, MF);

            return View(T);
        }

        [HttpPost]
        public string WMS_Up_Task_Reset_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Move_ID = ID;
                IW.Reset_WMS_Move_Task_Scan(Move_ID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Up_Task_To_Stock(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Move_ID = ID;
                IW.Finish_WMS_Move_Stocktaking_Task(Move_ID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Up_Task_Delete_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Move_ID = ID;
                IW.Delete_WMS_Move_Item(Move_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //上架进程
    public partial class WMS_UpController : Controller
    {
        public ActionResult WMS_Up_Process()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatType = Request["MatType"] == null ? string.Empty : Request["MatType"].Trim();
            MF.Global_State = WMS_Move_Status_Enum.待移库.ToString();
            PageList<WMS_In_Task> PList = IW.Get_WMS_Up_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Up_Process_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Move_Filter MF = new WMS_Move_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Link_HeadID = ID;
            MF.Move_Status = WMS_Move_Status_Enum.待移库.ToString();
            PageList<WMS_Move> PList = IW.Get_WMS_Up_PageList(MF);
            ViewData["MF"] = MF;
            WMS_In_Head Head = IW.Get_WMS_In_Head_DB(MF.Link_HeadID);
            ViewData["Head"] = Head;
            return View(PList);
        }

        public ActionResult WMS_Up_Process_Sub_Operate(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
           
            Guid Move_ID = ID;
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_Group_List_For_WMS_Up(Move_ID);
            WMS_Move Move = IW.Get_WMS_Move_DB(ID);
            ViewData["Move"] = Move;
            return View(List);
        }
        
        [HttpPost]
        public RedirectToRouteResult WMS_Up_Process_Sub_Operate_Post(Guid ID, FormCollection FC)
        {
            Guid MoveID = ID;
            try
            {
                List<string> MatSn_List = CommonLib.StringListStrToStringArray(FC["MatSn"]);
                string Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
                IW.Batch_Finish_WMS_Up_Process(MatSn_List, Location, MoveID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Up_Process_Sub_Operate", new { ID = ID });
        }
    }

    //上架记录
    public partial class WMS_UpController : Controller
    {
        public ActionResult WMS_Up_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Move_Filter MF = new WMS_Move_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.In_Location = Request["In_Location"] == null ? string.Empty : Request["In_Location"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();

            MF.Move_Type = WMS_Move_Type_Enum.上架作业.ToString();
            PageList<WMS_Move_Record> PList = IW.Get_WMS_Move_Record_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }

    //移库进程
    public partial class WMS_UpController : Controller
    {
        public ActionResult WMS_Move_Task()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Move_Filter MF = new WMS_Move_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Move_Status = Request["Move_Status"] == null ? WMS_Move_Status_Enum.待移库.ToString() : Request["Move_Status"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.In_Location = Request["In_Location"] == null ? string.Empty : Request["In_Location"].Trim();

            PageList<WMS_Move> PList = IW.Get_WMS_Move_PageList(MF);
            ViewData["MF"] = MF;
            List<WMS_Work_Person> List = IW.Get_WMS_Work_Person_List(MyUser().LinkMainCID);
            ViewData["List"] = List;
            return View(PList);
        }

        public ActionResult WMS_Move_Task_Preview(Guid ID)
        {
            Guid Move_ID = ID;
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_Group_List_For_WMS_Up(Move_ID);
            WMS_Move Move = IW.Get_WMS_Move_DB(ID);
            ViewData["Move"] = Move;
            return View(List);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Move_Task_Batch_Add_Post(FormCollection FC)
        {
            try
            {
                List<Guid> MoveIDList = CommonLib.GuidListStrToGuidArray(FC["Move_ID"]);
                List<string> Work_Person_List = CommonLib.StringListStrToStringArray(FC["Work_Person"]);
                IW.Batch_Create_WMS_Move_With_Work_Person(MoveIDList, Work_Person_List);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Move_Task");
        }

        public ActionResult WMS_Move_Process()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Move_Filter MF = new WMS_Move_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Move_Status = Request["Move_Status"] == null ? WMS_Move_Status_Enum.待移库.ToString() : Request["Move_Status"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            PageList<WMS_Move> PList = IW.Get_WMS_Move_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Move_Process_Operate(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Move_ID = ID;
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_Group_List_For_WMS_Move(Move_ID);
            WMS_Move Move = IW.Get_WMS_Move_DB(ID);
            ViewData["Move"] = Move;
            return View(List);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Move_Process_Operate_Post(Guid ID, FormCollection FC)
        {
            Guid MoveID = ID;
            try
            {
                List<string> MatSn_List = CommonLib.StringListStrToStringArray(FC["MatSn"]);
                string Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
                IW.Batch_Finish_WMS_Up_Process(MatSn_List, Location, MoveID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Move_Process_Operate",new { ID= ID });
        }

        [HttpPost]
        public string WMS_Move_Task_Finish_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid MoveID = ID;
                IW.Finish_WMS_Move_Task(MoveID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

    }

    //移库记录
    public partial class WMS_UpController : Controller
    {
        public ActionResult WMS_Move_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Move_Filter MF = new WMS_Move_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.In_Location = Request["In_Location"] == null ? string.Empty : Request["In_Location"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Move_Type = WMS_Move_Type_Enum.移库作业.ToString();
            PageList<WMS_Move_Record> PList = IW.Get_WMS_Move_Record_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }
    
}