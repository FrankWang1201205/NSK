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
    public partial class WMS_Task_WasteController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        ISupplierService IS = new SupplierService();
        IPDFService IP = new PDFService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //任务创建
    public partial class WMS_Task_WasteController : Controller
    {
        public ActionResult WMS_Task_Waste_Create()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Waste_Filter MF = new WMS_Waste_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Status = Request["Status"] == null ? string.Empty : Request["Status"].Trim();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();
         
            PageList<WMS_Waste_Head> PList = IW.Get_WMS_Waste_Head_PageList_Sub(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Task_Waste_Create_Post()
        {
            User U = this.MyUser();
            try
            {
                Guid Head_ID = IW.Create_WMS_Waste_Head(U);
                TempData["Success"] = "报废单创建成功！";
                return RedirectToAction("WMS_Task_Waste_Create_Sub", new { ID = Head_ID });
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("WMS_Task_Waste_Create");
            }
        }
        
        public ActionResult WMS_Task_Waste_Create_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid Head_ID = ID;
            WMS_Waste_Head Head = IW.Get_WMS_Waste_Head_DB(Head_ID);
            WMS_In_Head In_Head = IW.Get_WMS_In_Head_DB(Head.Link_HeadID);
            ViewData["In_Head"] = In_Head;
           
            return View(Head);
        }

        public PartialViewResult WMS_Task_Waste_Create_Sub_Modal(Guid ID)
        {
            User U = this.MyUser();

            WMS_In_Filter MF = new WMS_In_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 10;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_Bat_No = Request["Task_Bat_No"] == null ? string.Empty : Request["Task_Bat_No"].Trim();
            MF.Head_Type = WMS_In_Head_Type_Enum.订单退货.ToString();
            MF.Global_State = WMS_In_Global_State_Enum.完成入库.ToString();
            List<WMS_In_Task> List = IW.Get_WMS_In_Task_PageList(MF).Rows;
            Guid HeadID = ID;
            ViewData["HeadID"] = HeadID;

            return PartialView(List);
        }

        [HttpPost]
        public string WMS_Task_Waste_Create_Sub_Modal_Post(Guid ID)
        {
            string result = string.Empty;
            User U = this.MyUser();
            try
            {
                Guid Link_HeadID = ID;
                Guid HeadID = new Guid(Request["Head_ID"].ToString());
                IW.Link_WMS_Task_Waste(HeadID, Link_HeadID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Task_Waste_Create_Sub_Cancel_Post(Guid ID)
        {
            string result = string.Empty;
            User U = this.MyUser();
            try
            {
                Guid Link_HeadID = ID;
                IW.Cancel_Link_WMS_Task_Waste(Link_HeadID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Task_Waste_Create_Sub_Delete_Post(Guid ID)
        {
            User U = this.MyUser();
            try
            {
                Guid Head_ID = ID;
                IW.Delete_WMS_Waste_Head(Head_ID);
                TempData["Success"] = "报废单删除成功！";
                return RedirectToAction("WMS_Task_Waste_Create");
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("WMS_Task_Waste_Create_Sub", new { ID = ID });
            }
        }
        
        public PartialViewResult WMS_Task_Waste_Create_Sub_Input(Guid ID)
        {
            User U = this.MyUser();

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();

            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_Group_List_For_Waste(MF);
            Guid HeadID = ID;
            ViewData["HeadID"] = HeadID;
            return PartialView(List);
        }

        [HttpPost]
        public string WMS_Task_Waste_Create_Sub_Input_Post(Guid ID)
        {
            string result = string.Empty;
            User U = this.MyUser();
            try
            {
                WMS_Waste_Line Line = new WMS_Waste_Line();
                Line.LinkMainCID = U.LinkMainCID;
                Line.Link_Head_ID = ID;
                Line.MatSn = Request["MatSn"] == null ? string.Empty: Request["MatSn"].Trim();
                Line.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
                Line.Max_Quantity = Convert.ToInt32(Request["Quantity"].Trim());
                IW.Create_WMS_Waste_Line(Line);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public PartialViewResult WMS_Task_Waste_Create_Sub_Output(Guid ID)
        {
            User U = this.MyUser();

            Guid HeadID = ID;
            List<WMS_Waste_Line> List = IW.Get_WMS_Waste_Line_List(HeadID);
            return PartialView(List);
        }

        [HttpPost]
        public string WMS_Task_Waste_Create_Sub_Output_Clear_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid HeadID = ID;
                IW.Delete_WMS_Waste_Line_List(HeadID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Task_Waste_Create_Sub_Output_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                WMS_Waste_Line Line = new WMS_Waste_Line();
                Line.Line_ID = ID;
                Line.Quantity = Convert.ToInt32(Request["Qty"].ToString());
                IW.Set_WMS_Waste_Line(Line);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string WMS_Task_Waste_Create_Sub_Output_Delete_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid LineID = ID;
                IW.Delete_WMS_Waste_Line(LineID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Task_Waste_Create_Sub_Post(Guid ID)
        {
            try
            {
                Guid HeadID = ID;
                IW.Set_WMS_Task_Waste(HeadID);
                TempData["Success"] = "递交成功";
                return RedirectToAction("WMS_Task_Waste_Create");
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("WMS_Task_Waste_Create_Sub", new { ID = ID });
            }

        }

        public ActionResult WMS_Task_Waste_Create_Sub_Preview(Guid ID)
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
        public string WMS_Task_Waste_Create_Sub_Preview_Finish_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid HeadID = ID;
                IW.Finish_WMS_Task_Waste(HeadID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //任务记录
    public partial class WMS_Task_WasteController : Controller
    {
        public ActionResult WMS_Task_Waste_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Waste_Filter MF = new WMS_Waste_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Task_No_Str = Request["Task_No_Str"] == null ? string.Empty : Request["Task_No_Str"].Trim();
            MF.Status = WMS_Waste_Head_Status_Enum.已消库.ToString();
            MF.Create_Person = Request["Create_Person"] == null ? string.Empty : Request["Create_Person"].Trim();

            PageList<WMS_Waste_Head> PList = IW.Get_WMS_Waste_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult WMS_Task_Waste_Record_Preview(Guid ID)
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

    }
}