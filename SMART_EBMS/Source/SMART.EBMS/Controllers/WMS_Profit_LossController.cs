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
    public partial class WMS_Profit_LossController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        IMaterialService IM = new MaterialService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //审核查看
    public partial class WMS_Profit_LossController : Controller
    {
        public ActionResult WMS_Profit_Loss_Task_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Profit_Loss_Filter MF = new WMS_Profit_Loss_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Status = Request["Status"] == null ? string.Empty : Request["Status"].Trim();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();

            PageList<WMS_Profit_Loss_Head> PList = IW.Get_WMS_Profit_Loss_Head_PageList_Sub(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Profit_Loss_Task_Search_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Head_ID = ID;
            WMS_Profit_Loss_Head Head = IW.Get_WMS_Profit_Loss_Head_Item(Head_ID);

            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_DB(Head.Link_HeadID);
            ViewData["Task"] = Task;

            return View(Head);
        }

        [HttpPost]
        public string WMS_Profit_Loss_Task_Search_Preview_Finish_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid HeadID = ID;
                IW.Finish_WMS_Profit_Loss_Head(HeadID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //审核记录
    public partial class WMS_Profit_LossController : Controller
    {
        public ActionResult WMS_Profit_Loss_Task_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Profit_Loss_Filter MF = new WMS_Profit_Loss_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Status = Request["Status"] == null ? string.Empty : Request["Status"].Trim();
            PageList<WMS_Profit_Loss_Head> PList = IW.Get_WMS_Profit_Loss_Head_PageList_Record(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Profit_Loss_Task_Record_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Head_ID = ID;
            WMS_Profit_Loss_Head Head = IW.Get_WMS_Profit_Loss_Head_Item(Head_ID);

            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_DB(Head.Link_HeadID);
            ViewData["Task"] = Task;

            return View(Head);
        }

    }
    
    //经理核验
    public partial class WMS_Profit_LossController : Controller
    {
        public ActionResult WMS_Profit_Loss_Task_For_Manager()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Profit_Loss_Filter MF = new WMS_Profit_Loss_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Status = WMS_Profit_Loss_Head_Status_Enum.待审核.ToString();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();

            PageList<WMS_Profit_Loss_Head> PList = IW.Get_WMS_Profit_Loss_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Profit_Loss_Task_For_Manager_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Head_ID = ID;
            WMS_Profit_Loss_Head Head = IW.Get_WMS_Profit_Loss_Head_Item(Head_ID);

            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_DB(Head.Link_HeadID);
            ViewData["Task"] = Task;

            return View(Head);
        }

        [HttpPost]
        public string WMS_Profit_Loss_Task_For_Manager_Preview_Confirm_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid HeadID = ID;
                IW.Confirm_WMS_Profit_Loss_Head(HeadID, MyUser());
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Profit_Loss_Task_For_Manager_Preview_Refuse_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid HeadID = ID;
                string Refuse_Remark = Request["Refuse_Remark"] == null ? string.Empty : Request["Refuse_Remark"].Trim();
                IW.Refuse_WMS_Profit_Loss_Head(HeadID, Refuse_Remark, MyUser());
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //财务核验
    public partial class WMS_Profit_LossController : Controller
    {
        public ActionResult WMS_Profit_Loss_Task_For_Accounting()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Profit_Loss_Filter MF = new WMS_Profit_Loss_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Status = WMS_Profit_Loss_Head_Status_Enum.已审核.ToString();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();

            PageList<WMS_Profit_Loss_Head> PList = IW.Get_WMS_Profit_Loss_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Profit_Loss_Task_For_Accounting_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Head_ID = ID;
            WMS_Profit_Loss_Head Head = IW.Get_WMS_Profit_Loss_Head_Item(Head_ID);

            WMS_Stock_Task Task = IW.Get_WMS_Stock_Task_Item_DB(Head.Link_HeadID);
            ViewData["Task"] = Task;

            return View(Head);
        }

        [HttpPost]
        public string WMS_Profit_Loss_Task_For_Accounting_Preview_Confirm_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid HeadID = ID;
                IW.Confirm_WMS_Profit_Loss_Head_By_Accounting(HeadID, MyUser());
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //盈亏记录
    public partial class WMS_Profit_LossController : Controller
    {
        public ActionResult WMS_Profit_Loss_Record_Search()
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
            PageList<WMS_Profit_Loss> PList = IW.Get_WMS_Profit_Loss_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }

}