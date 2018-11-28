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
    public partial class WMS_Task_NoticeController : Controller
    {
        IUserService IU = new UserService();
        IWmsService IW = new WmsService();
        IBrandService IB = new BrandService();
        IPDFService IP = new PDFService();
        ICustomerService IC = new CustomerService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //显示待收货和待上架
    public partial class WMS_Task_NoticeController : Controller
    {
        public ActionResult WMS_Task_Notice()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public ActionResult WMS_In_Task_List()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public PartialViewResult WMS_In_Task_List_Sub()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            List<WMS_In_Head> List = IW.Get_WMS_In_Task_List(U.LinkMainCID);
            return PartialView(List);
        }

        public PartialViewResult WMS_In_Move_List_Sub()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            List<WMS_Move> List = IW.Get_WMS_Move_Up_List(U.LinkMainCID);
            return PartialView(List);
        }
    }

    //显示待配货和待验货
    public partial class WMS_Task_NoticeController : Controller
    {
        public ActionResult WMS_Out_Task_List()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public PartialViewResult WMS_Out_Down_List_Sub()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
          
            List<WMS_Out_Head> List =IW.Get_WMS_Out_Head_Down_List(U.LinkMainCID);
            return PartialView(List);
        }

        public PartialViewResult WMS_Out_Inspection_List_Sub()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            List<WMS_Out_Head> List = IW.Get_WMS_Out_Head_Out_List(U.LinkMainCID);
            return PartialView(List);
        }
    }

    //显示待盘库和待移库
    public partial class WMS_Task_NoticeController : Controller
    {
        public ActionResult WMS_Task_List()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public PartialViewResult WMS_Task_List_Move()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            
            List<WMS_Move> List = IW.Get_WMS_Move_List(U.LinkMainCID);
            return PartialView(List);
        }

        public PartialViewResult WMS_Task_List_Stocktaking()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            List<WMS_Stock_Task> List = IW.Get_WMS_Stock_Task_PageList(MF).Rows;
            return PartialView(List);
        }
    }
}