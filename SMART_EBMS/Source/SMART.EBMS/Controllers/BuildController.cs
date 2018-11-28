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
    public partial class BuildController : Controller
    {
        IUserService IU = new UserService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class BuildController : Controller
    {
        public ActionResult Index()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }
    }
}