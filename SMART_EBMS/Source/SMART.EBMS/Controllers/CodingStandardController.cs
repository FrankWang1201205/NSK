using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;

namespace SMART.EBMS.Controllers
{
    public class CodingStandardController : Controller
    {
        IUserService IU = new UserService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }

        public ActionResult WebUI()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        public ActionResult Interface()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            List<MyType> List = MyAssembly.GetInterfaceList("SMART.Api", "SMART.Api");
            return View(List);
        }

        public ActionResult Model()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            List<MyType> List = MyAssembly.GetModelList("SMART.Api", "SMART.Api.Models");
            return View(List);
        }

        public ActionResult INIT()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            IMainCompanyService IMainCom = new MainCompanyService();
            List<MainCompany> List = IMainCom.Get_MainCompany_List();
            return View(List);
        }

        [HttpPost]
        public RedirectToRouteResult INIT_Post(FormCollection FC)
        {
            try
            {
                MainCompany MC = new MainCompany();
                TryUpdateModel<MainCompany>(MC, FC);
                IMainCompanyService IMainCom = new MainCompanyService();
                IMainCom.Create_MainCom(MC);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("INIT");
        }
    }
}