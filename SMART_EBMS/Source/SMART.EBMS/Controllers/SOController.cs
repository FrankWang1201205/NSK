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
    public partial class SOController : Controller
    {
        IUserService IU = new UserService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class SOController : Controller
    {
        public ActionResult _A()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public ActionResult _B()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public ActionResult _C()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }
    }
}