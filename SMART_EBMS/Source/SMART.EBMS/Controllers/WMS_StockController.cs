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
    public partial class WMS_StockController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class WMS_StockController : Controller
    {
        public ActionResult WMS_Stock_List()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatSn_A = Request["MatSn_A"] == null ? string.Empty : Request["MatSn_A"].Trim();
            MF.MatSn_B = Request["MatSn_B"] == null ? string.Empty : Request["MatSn_B"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            PageList<WMS_Stock_Group> List = IW.Get_WMS_Stock_Group_PageList(MF);
            MF.Quantity_Sum = IW.Get_Total_WMS_Stock_Quantity(U.LinkMainCID);
            ViewData["MF"] = MF;
            return View(List);
        }

        public PartialViewResult WMS_Stock_List_Sub_All()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_List(MF);
            ViewData["List"] = List;
            List<WMS_Out_Line> Line_List = IW.Get_WMS_Out_Line_List_For_Stock(MF);
            ViewData["Line_List"] = Line_List;
            List<WMS_Stock_Group> Temp_List = IW.Get_WMS_Stock_Temp_List_For_Stock(MF);
            ViewData["Temp_List"] = Temp_List;
            return PartialView();
        }

        public ActionResult WMS_Stock_To_Excel()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            string Path = IW.Get_WMS_Stock_To_Excel(U.LinkMainCID);

            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "库存差异信息.xlsx");
        }

        public ActionResult WMS_Stock_All_To_Excel()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            string Path = IW.Get_WMS_Stock_All_To_Excel(U.LinkMainCID);

            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "产品库存信息.xlsx");
        }

        public PartialViewResult WMS_Stock_List_Sub_Record_Info()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            try { MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim(); } catch { }
            List<WMS_Stock_Record_Info> List = IW.Get_WMS_Stock_Record_Info_List(MF);
            return PartialView(List);
        }

        public PartialViewResult WMS_Stock_List_Sub()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_List(MF);
            return PartialView(List);
        }

        public PartialViewResult WMS_Stock_List_Preoccupancy_Sub()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            List<WMS_Out_Line> List = IW.Get_WMS_Out_Line_List_For_Stock(MF);
            return PartialView(List);
        }

        public PartialViewResult WMS_Stock_List_Occupied_Sub()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_Temp_List_For_Stock(MF);
            return PartialView(List);
        }

        public ActionResult WMS_Stock_List_By_Location()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 50;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Location_Type = Request["Location_Type"] == null ? string.Empty : Request["Location_Type"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            PageList<WMS_Stock_Group_Location> List = IW.Get_WMS_Stock_By_Location_List(MF);
            MF.Quantity_Sum = IW.Get_Total_WMS_Stock_Quantity(U.LinkMainCID);
            ViewData["MF"] = MF;
            return View(List);
        }

        public PartialViewResult WMS_Stock_List_By_Location_Sub(string ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.Location = ID;
            try { MF.Link_HeadID = new Guid(Request["Link_HeadID"].Trim()); } catch { MF.Link_HeadID = Guid.Empty; }
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_Group_List(MF);
            ViewData["MF"] = MF;
            return PartialView(List);
        }

        public PartialViewResult WMS_Stock_List_By_Location_Sub_By_Case(string ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.Location = ID;
            MF.Case = Request["Case"] == null ? string.Empty : Request["Case"].Trim();
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_Group_List_By_Case(MF);
            ViewData["MF"] = MF;
            return PartialView(List);
        }

        [HttpPost]
        public RedirectToRouteResult WMS_Stock_Batch_Create_Post()
        {
            User U = this.MyUser();
            try
            {
                HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
                IW.Batch_Create_WMS_Stock(ExcelFile, U);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("WMS_Stock_List_By_Location");
        }

    }

    //出入库记录
    public partial class WMS_StockController : Controller
    {
        public ActionResult WMS_Stock_In_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Location_Type = Request["Location_Type"] == null ? string.Empty : Request["Location_Type"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.Supplier = Request["Supplier"] == null ? string.Empty : Request["Supplier"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Remark = WMS_Stock_Record_Remark_Enum.订单入库.ToString();
            PageList<WMS_Stock_Record> List = IW.Get_WMS_Stock_Record_PageList(MF);
            ViewData["MF"] = MF;
            return View(List);
        }

        public ActionResult WMS_Stock_Out_Record()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Location_Type = Request["Location_Type"] == null ? string.Empty : Request["Location_Type"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            MF.Customer = Request["Customer"] == null ? string.Empty : Request["Customer"].Trim();
            MF.Work_Person = Request["Work_Person"] == null ? string.Empty : Request["Work_Person"].Trim();
            MF.Time_End = Request["Time_End"] == null ? string.Empty : Request["Time_End"].Trim();
            MF.Time_Start = Request["Time_Start"] == null ? string.Empty : Request["Time_Start"].Trim();
            MF.Remark = WMS_Stock_Record_Remark_Enum.订单出库.ToString();
            PageList<WMS_Stock_Record> List = IW.Get_WMS_Stock_Record_PageList(MF);
            ViewData["MF"] = MF;
            return View(List);
        }
    }

    //端数移库推荐
    public partial class WMS_StockController : Controller
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
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.Location = Request["Location"] == null ? string.Empty : Request["Location"].Trim();
            PageList<WMS_Stock_Group_Location> List = IW.Get_WMS_Stock_By_Location_List_For_Move(MF);
            ViewData["MF"] = MF;
            return View(List);
        }

        public PartialViewResult WMS_Move_Recommend_Sub(string ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.Location = ID;
            List<WMS_Stock_Group> List = IW.Get_WMS_Stock_Group_List_For_Move(MF);
            ViewData["MF"] = MF;
            return PartialView(List);
        }
    }
}