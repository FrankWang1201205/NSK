using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;

namespace SMART.EBMS.Controllers
{
    public partial class DevelopmentToolController : Controller
    {
        IUserService IU = new UserService();
        IDevToolService IDev = new DevToolService();

        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class DevelopmentToolController : Controller
    {
        public ActionResult Clean_Data()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public RedirectToRouteResult Add_Out_In_Record_From_First_Stocktaking()
        {
            try
            {
                IDev.Add_Out_In_Record_From_First_Stocktaking(MyUser().LinkMainCID);
                TempData["Success"] = "写入成功";
            }
            catch(Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Clean_Data");
        }

        public RedirectToRouteResult Reset_Out_In_Record_From_WMS_In()
        {
            try
            {
                IDev.Reset_Out_In_Record_From_WMS_In(MyUser().LinkMainCID);
                TempData["Success"] = "写入成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Clean_Data");
        }

        public RedirectToRouteResult Reset_WMS_Stock_Task_Type()
        {
            try
            {
                IDev.Reset_WMS_Stock_Task_Type(MyUser().LinkMainCID);
                TempData["Success"] = "写入成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Clean_Data");
        }

        public RedirectToRouteResult Clean_Material()
        {
            IDev.Clean_Material();
            return RedirectToAction("Clean_Data");
        }

        public RedirectToRouteResult Clean_WMS_In()
        {
            IDev.Clean_WMS_In();
            return RedirectToAction("Clean_Data");
        }

        public RedirectToRouteResult Set_Data()
        {
            IDev.Set_Tracking_Info();
            return RedirectToAction("Clean_Data");
        }

        public RedirectToRouteResult Set_Supplier_Name()
        {
            IDev.Set_Supplier_Name();
            return RedirectToAction("Clean_Data");
        }

        public RedirectToRouteResult Create_WMS_Move()
        {
            IDev.Create_WMS_Move(MyUser().LinkMainCID);
            return RedirectToAction("Clean_Data");
        }

        public RedirectToRouteResult Refresh_WMS_Stock()
        {
            try
            {
                IDev.Refresh_WMS_Stock(MyUser().LinkMainCID);
                TempData["Success"] = "写入成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Clean_Data");
        }

        public RedirectToRouteResult Refresh_WMS_Stock_Record()
        {
            try
            {
                IDev.Refresh_WMS_Stock_Record(MyUser().LinkMainCID);
                TempData["Success"] = "写入成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Clean_Data");
        }
    }


}