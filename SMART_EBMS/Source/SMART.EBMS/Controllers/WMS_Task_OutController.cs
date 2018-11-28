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
    public partial class WMS_Task_OutController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        ICustomerService IC = new CustomerService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //任务导入
    public partial class WMS_Task_OutController : Controller
    {
        public ActionResult WMS_Out_Start()
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
            MF.Global_State = Request["Global_State"] == null ? string.Empty : Request["Global_State"].Trim();
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();

            PageList<WMS_Out_Head> PList = IW.Get_WMS_Out_Head_PageList_Distribute(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Out_Start_Post()
        {
            User U = this.MyUser();
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                WMS_Out_Head Head = new WMS_Out_Head();
                Head.Out_DT = Convert.ToDateTime(Request.Form["Out_DT"].Trim());
                Head.Logistics_Mode = Request.Form["Logistics_Mode"].Trim();
                Head.Customer_Name = Request.Form["Cust_Name"].Trim();
                Head.Link_Cus_ID = new Guid(Request.Form["Link_Cus_ID"].Trim());
                IW.Batch_Create_WMS_Out(ExcelFile, Head, U);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Out_Start");
        }

        public PartialViewResult Cust_List()
        {
            User U = this.MyUser();
            Customer_Filter MF = new Customer_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.Cust_Name = Request["Cust_Name"] == null ? string.Empty : Request["Cust_Name"].Trim();
            List<Customer> List = IC.Get_Customer_List(MF);
            ViewData["MF"] = MF;
            return PartialView(List);
        }

        public ActionResult WMS_Out_Start_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.LinkHeadID = ID;
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(MF.LinkHeadID);
            ViewData["Head"] = Head;
            MF.Task_Bat_No = Head.Task_Bat_No_Str;
            List<WMS_Out_Line> List = IW.Get_WMS_Out_Line_List(MF);
            ViewData["MF"] = MF;
            return View(List);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Out_Start_Delete(Guid ID)
        {
            Guid Head_ID = ID;
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            try
            {
                IW.Delete_Task_Bat_Out(Head.Head_ID);
                return RedirectToAction("WMS_Out_Start");
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("WMS_Out_Start_Sub", new { ID = Head_ID });
            }
        }

        public ActionResult WMS_Out_Start_To_Excel(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Guid Head_ID = ID;
            string Path = IW.Get_WMS_Out_Line_List_To_Excel(Head_ID);
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(Head_ID);
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Head.Task_Bat_No_Str + "_出库送货单" + ".xlsx");

        }

        [HttpPost]
        public RedirectToRouteResult WMS_Out_Start_Sub_Post(Guid ID)
        {
            User U = this.MyUser();
            Guid HeadID = ID;
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                IW.Batch_Create_WMS_Out(ExcelFile, HeadID, U);
                TempData["Success"] = "上传成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }

            return RedirectToAction("WMS_Out_Start_Sub", new { ID = ID });
        }
    }

    //进程
    public partial class WMS_Task_OutController : Controller
    {
        public ActionResult WMS_Out_Process()
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
            MF.Global_State = Request["Global_State"] == null ? string.Empty : Request["Global_State"].Trim();
            MF.Logistics_Mode = Request["Logistics_Mode"] == null ? string.Empty : Request["Logistics_Mode"].Trim();
            MF.Work_Down_Person = Request["Work_Down_Person"] == null ? string.Empty : Request["Work_Down_Person"].Trim();

            PageList<WMS_Out_Head> PList = IW.Get_WMS_Out_Head_PageList_Distribute(MF);
            ViewData["MF"] = MF;
            return View(PList);
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
    }
    
    //出库记录
    public partial class WMS_Task_OutController : Controller
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
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            PageList<WMS_Out_Head> PList = IW.Get_WMS_Out_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Out_Record_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.LinkHeadID = ID;
            WMS_Out_Head Head = IW.Get_WMS_Out_Head_DB(MF.LinkHeadID);
            MF.Task_Bat_No = Head.Task_Bat_No_Str;
            List<WMS_Out_Line> List = IW.Get_WMS_Out_Line_List(MF);
            ViewData["MF"] = MF;
            return View(List);
        }
    }

}