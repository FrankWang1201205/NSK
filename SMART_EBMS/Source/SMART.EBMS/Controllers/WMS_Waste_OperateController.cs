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
    public partial class WMS_Waste_OperateController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        IMaterialService IM = new MaterialService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //经理报废核验
    public partial class WMS_Waste_OperateController : Controller
    {
        public ActionResult WMS_Waste_Check_For_Manager()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Waste_Filter MF = new WMS_Waste_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Status = WMS_Waste_Head_Status_Enum.待审核.ToString();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();

            PageList<WMS_Waste_Head> PList = IW.Get_WMS_Waste_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Waste_Check_For_Manager_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Head_ID = ID;
            WMS_Waste_Head Head = IW.Get_WMS_Waste_Head_Item(Head_ID);

            WMS_In_Head In_Head = IW.Get_WMS_In_Head_DB(Head.Link_HeadID);
            ViewData["In_Head"] = In_Head;

            WMS_Out_Head Out_Head = IW.Get_WMS_Out_Head_DB(In_Head.Link_WMS_In_ID);
            ViewData["Out_Head"] = Out_Head;

            return View(Head);
        }

        [HttpPost]
        public string WMS_Waste_Check_For_Manager_Preview_Confirm_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid HeadID = ID;
                IW.Confirm_WMS_Task_Waste(HeadID, MyUser());
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Waste_Check_For_Manager_Preview_Refuse_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid HeadID = ID;
                string Refuse_Remark = Request["Refuse_Remark"] == null ? string.Empty : Request["Refuse_Remark"].Trim();
                IW.Refuse_WMS_Task_Waste(HeadID, Refuse_Remark, MyUser());
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //财务报废核验
    public partial class WMS_Waste_OperateController : Controller
    {
        public ActionResult WMS_Waste_Check_For_Accounting()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Waste_Filter MF = new WMS_Waste_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Status = WMS_Waste_Head_Status_Enum.已审核.ToString();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();

            PageList<WMS_Waste_Head> PList = IW.Get_WMS_Waste_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Waste_Check_For_Accounting_Preview(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Head_ID = ID;
            WMS_Waste_Head Head = IW.Get_WMS_Waste_Head_Item(Head_ID);

            WMS_In_Head In_Head = IW.Get_WMS_In_Head_DB(Head.Link_HeadID);
            ViewData["In_Head"] = In_Head;

            WMS_Out_Head Out_Head = IW.Get_WMS_Out_Head_DB(In_Head.Link_WMS_In_ID);
            ViewData["Out_Head"] = Out_Head;
            return View(Head);
        }

        [HttpPost]
        public string WMS_Waste_Check_For_Accounting_Preview_Confirm_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid HeadID = ID;
                IW.Confirm_WMS_Task_Waste_By_Accounting(HeadID, MyUser());
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    public partial class WMS_Waste_OperateController : Controller
    {
        public ActionResult WMS_Waste_Record_Search()
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
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Remark = WMS_Stock_Record_Remark_Enum.报废出库.ToString();
            PageList<WMS_Stock_Record> List = IW.Get_WMS_Stock_Record_PageList(MF);
            ViewData["MF"] = MF;
            return View(List);
        }
    }
    
}