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
    public partial class IndexController : Controller
    {
        IUserService IU = new UserService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class IndexController : Controller
    {
        public ActionResult Framework()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            return View();
        }

        public ActionResult MyUserInfo()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            return View(U);
        }

        public ActionResult MyUserInfo_Post(FormCollection FC)
        {
            try
            {
                Guid UID = this.MyUser().UID;
                User U = new User();
                TryUpdateModel<User>(U, FC);
                IU.Set_User_Base(UID, U);
                TempData["Success"] = "更新成功!";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("MyUserInfo");
        }

        public ActionResult MyUserInfo_Password_Post()
        {
            try
            {
                Guid UID = this.MyUser().UID;
                string Password = Request["NewPassword"] == null ? string.Empty : Request["NewPassword"].Trim();
                IU.Set_User_Password(UID, Password);
                TempData["Success"] = "密码重置成功!";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("MyUserInfo");
        }
    }
}