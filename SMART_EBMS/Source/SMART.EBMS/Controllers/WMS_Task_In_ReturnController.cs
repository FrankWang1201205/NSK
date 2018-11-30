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
    public partial class WMS_Task_In_ReturnController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        ISupplierService IS = new SupplierService();
        IPDFService IP = new PDFService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //任务创建
    public partial class WMS_Task_In_ReturnController : Controller
    {
        public ActionResult WMS_In_Return_Search()
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
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Head_Type = WMS_In_Head_Type_Enum.订单退货.ToString();
            PageList<WMS_In_Task> PList = IW.Get_WMS_In_Task_PageList_Distribute(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_In_Return_Search_Create()
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
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Head_Type = WMS_Out_Head_Type_Enum.订单出货.ToString();
            PageList<WMS_Out_Head> PList = IW.Get_WMS_Out_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_In_Return_Search_Create_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_In_Filter MF = new WMS_In_Filter();
            List<WMS_Logistics> Logistics_List = IW.Get_WMS_Logistics_List(U.LinkMainCID);
            ViewData["Logistics_List"] = Logistics_List;
            ViewData["MF"] = MF;
            Guid Head_ID = ID;
            WMS_Out_Task T = IW.Get_WMS_Out_Task_Item_DB(Head_ID);
            return View(T);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_In_Return_Search_Create_Sub_Add_Post(Guid ID, FormCollection FC)
        {
            User U = this.MyUser();
            try
            {
                Guid HeadID = ID;
                WMS_In_Head Head = new WMS_In_Head();
                Head.Head_ID = HeadID;
                TryUpdateModel(Head, FC);
                List<string> MatSnList = CommonLib.StringListStrToStringArray(Request.Form["MatSn"].ToString());
                List<WMS_In_Line> Line_List = new List<WMS_In_Line>();
                WMS_In_Line Line = new WMS_In_Line();
                foreach (var MatSn in MatSnList)
                {
                    Line = new WMS_In_Line();
                    Line.MatSn = MatSn;
                    Line.Quantity = Convert.ToInt32(Request.Form["Quantity_" + MatSn].ToString());
                    Line_List.Add(Line);
                }

                IW.Create_WMS_Task_In_Return(Head, Line_List, U);
                TempData["Success"] = "退货单创建成功！";
                return RedirectToAction("WMS_In_Return_Search");
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("WMS_In_Return_Search_Create_Sub", new { ID = ID });
            }
        }

        public ActionResult WMS_In_Return_Search_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_In_Filter MF = new WMS_In_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.LinkHeadID = ID;
            WMS_In_Task T = IW.Get_WMS_In_Task_Item(MF);
            ViewData["MF"] = MF;
            return View(T);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_In_Return_Search_Sub_Delete_Post(Guid ID)
        {
            try
            {
                Guid Head_ID = ID;
                IW.Delete_Task_Bat(Head_ID);
                TempData["Success"] = "退货单删除成功";
                return RedirectToAction("WMS_In_Return_Search");
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("WMS_In_Return_Search_Sub", new { ID = ID });
            }
        }

    }

    //入库进程
    public partial class WMS_Task_In_ReturnController : Controller
    {
        public ActionResult WMS_In_Return_Process()
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
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Head_Type = WMS_In_Head_Type_Enum.订单退货.ToString();
            PageList<WMS_In_Task> PList = IW.Get_WMS_In_Task_PageList_Distribute(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }

    //入库记录
    public partial class WMS_Task_In_ReturnController : Controller
    {
        public ActionResult WMS_In_Return_Record()
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
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Head_Type = WMS_In_Head_Type_Enum.订单退货.ToString();
            PageList<WMS_In_Task> PList = IW.Get_WMS_In_Task_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }
}