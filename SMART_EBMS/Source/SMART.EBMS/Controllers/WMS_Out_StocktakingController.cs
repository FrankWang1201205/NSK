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
    public partial class WMS_Out_StocktakingController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        ICustomerService IC = new CustomerService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }
    
    //配货动盘
    public partial class WMS_Out_StocktakingController : Controller
    {
        public ActionResult WMS_Out_Stocktaking_Pick()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Type = Request["Type"] == null ? string.Empty : Request["Type"].Trim();
            MF.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            PageList<WMS_Stock_Task> TaskList = IW.Get_WMS_Stock_Task_PageList_Pick(MF);
            ViewData["MF"] = MF;
            return View(TaskList);
        }

        public ActionResult WMS_Out_Stocktaking_Pick_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Task_ID = ID;
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_Other(Task_ID);
            return View(Task);
        }

        public ActionResult WMS_Out_Stocktaking_Pick_Preview_For_MatSn(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Task_ID = ID;
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_Other(Task_ID);
            return View(Task);
        }

        [HttpPost]
        public string WMS_Out_Stocktaking_Pick_Preview_Reset_Post(Guid ID)
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
        public string WMS_Out_Stocktaking_Pick_Preview_To_Stock(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Task_ID = ID;
                IW.Finish_WMS_Stocktaking_Task_Other(Task_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public PartialViewResult WMS_Out_Stocktaking_Pick_Preview_For_MatSn_Sub(Guid ID)
        {
            Guid TaskID = ID;
            string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            WMS_Stocktaking Stocktaking = IW.Get_WMS_Stocktaking_Item(TaskID, MatSn);
            return PartialView(Stocktaking);
        }

        [HttpPost]
        public string WMS_Out_Stocktaking_Pick_Preview_For_MatSn_Sub_Post(Guid ID)
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

        public PartialViewResult WMS_Out_Stocktaking_Pick_Preview_Sub(Guid ID)
        {
            string MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            Guid Task_ID = ID;
            List<WMS_Stocktaking_Scan> List = IW.Get_WMS_Stocktaking_Scan_List(Task_ID, MatSn);
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item(Task_ID);
            ViewData["Task"] = Task;
            return PartialView(List);
        }

        [HttpPost]
        public string WMS_Out_Stocktaking_Pick_Preview_Sub_Reset(Guid ID)
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


        public PartialViewResult WMS_Out_Stocktaking_QRCode()
        {
            string MatSn = Request["MatSn"].Trim();
            string QRCode_Path = IW.Get_QRCode(MatSn);
            ViewData["MatSn"] = MatSn;
            ViewData["QRCode_Path"] = QRCode_Path;
            return PartialView();
        }
    }

    //动盘记录
    public partial class WMS_Out_StocktakingController : Controller
    {
        public ActionResult WMS_Out_Stocktaking_Pick_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Type = Request["Type"] == null ? string.Empty : Request["Type"].Trim();
            MF.Status = WMS_Stock_Task_Enum.已盘库.ToString();
            MF.Property = WMS_Stock_Task_Property_Enum.配货动盘.ToString();
            PageList<WMS_Stock_Task> TaskList = IW.Get_WMS_Stock_Task_PageList(MF);
            ViewData["MF"] = MF;
            return View(TaskList);
        }

    }

    //盈亏核验(动盘)
    public partial class WMS_Out_StocktakingController : Controller
    {
        public ActionResult WMS_Profit_Or_Loss_Other_Check()
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
            PageList<WMS_Profit_Loss_Other> PList = IW.Get_WMS_Profit_Loss_Other_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        [HttpPost]
        public string WMS_Profit_Or_Loss_Other_Confirm_Post(Guid ID)
        {
            User U = this.MyUser();

            string result = string.Empty;
            try
            {
                Guid LineID = ID;
                IW.Confirm_WMS_Profit_Loss_Other_Item(LineID, U);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //盈亏记录（动盘）
    public partial class WMS_Out_StocktakingController : Controller
    {
        public ActionResult WMS_Profit_Or_Loss_Other()
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
            PageList<WMS_Profit_Loss_Other> PList = IW.Get_WMS_Profit_Loss_Other_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }

    //移库推荐
    public partial class WMS_Out_StocktakingController : Controller
    {
        public ActionResult WMS_Move_Recommend()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            PageList<WMS_Stock_Task> List = IW.Get_WMS_Stock_Task_PageList_For_Move_Recommend(MF);
            ViewData["MF"] = MF;
            return View(List);
        }

        public ActionResult WMS_Move_Recommend_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid TaskID = ID;
            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_DB(TaskID);
            ViewData["Task"] = Task;

            List<WMS_Move> Move_List = IW.Get_WMS_Move_List_By_Link_HeadID(TaskID);
            ViewData["Move_List"] = Move_List;

            List<WMS_Stocktaking_Scan> List = IW.Get_WMS_Stocktaking_Scan_List_For_Move(TaskID);
            return View(List);
        }

        [HttpPost]
        public string WMS_Move_Recommend_Sub_Finish_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Task_ID = ID;
                IW.Finish_WMS_Stocktaking_Scan_Recommend(Task_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Move_Recommend_Sub_Create_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                WMS_Move_Scan Move_Scan = new WMS_Move_Scan();
                Move_Scan.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
                Move_Scan.In_Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
                Move_Scan.Scan_Quantity = Convert.ToInt32(Request["Quantity"].ToString());
                Guid Task_ID = ID;
                IW.Create_WMS_Move_From_WMS_Stocktaking_Scan_Recommend(Task_ID, Move_Scan);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Move_Recommend_Sub_Delete_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Move_ID = ID;
                IW.Delete_WMS_Move_From_WMS_Stocktaking_Scan_Recommend(Move_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public PartialViewResult WMS_Move_Recommend_Sub_Operate(Guid ID)
        {
            Guid Move_ID = ID;
            WMS_Move Move = IW.Get_WMS_Move_DB(Move_ID);
            ViewData["Move"] = Move;

            List<WMS_Move_Scan> Scan_List = IW.Get_WMS_Move_Scan_List(Move_ID);
            return PartialView(Scan_List);
        }

        [HttpPost]
        public string WMS_Move_Recommend_Sub_Operate_Finish_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Move_ID = ID;
                IW.Finish_WMS_Move_Task_From_Recommend(Move_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

}