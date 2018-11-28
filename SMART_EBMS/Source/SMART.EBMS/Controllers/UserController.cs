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
    public partial class UserController : Controller
    {
        IUserService IU = new UserService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class UserController : Controller
    {
        public ActionResult User_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            User_Filter MF = new User_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;

            MF.LinkMainCID = U.LinkMainCID;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.RoleTitle = Request["RoleTitle"] == null ? string.Empty : Request["RoleTitle"].Trim();
            MF.DepName = Request["DepName"] == null ? string.Empty : Request["DepName"].Trim();
            MF.IsFrozen = Request["IsFrozen"] == null ? string.Empty : Request["IsFrozen"].Trim();
            MF.DepNameList = IU.Get_Department_Name_List(MF.LinkMainCID);

            PageList<User> PList = IU.Get_User_PageList(MF);
            ViewData["MF"] = MF;

            User EmptyUser = IU.Get_Empty_User(U.LinkMainCID);
            ViewData["EmptyUser"] = EmptyUser;
            return View(PList);
        }

        public PartialViewResult User_Search_Sub(Guid ID)
        {
            Guid UID = ID;
            User U = IU.Get_User_Item_More(UID);
            return PartialView(U);
        }


        public ActionResult User_Add()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        [HttpPost]
        public string User_Add_Post(FormCollection FC)
        {
            string result = string.Empty;
            try
            {
                User ThisUser = this.MyUser();

                Guid LinkMainCID = ThisUser.LinkMainCID;
                User U = new User();
                TryUpdateModel<User>(U, FC);

                User_Profile U_P = new User_Profile();
                TryUpdateModel<User_Profile>(U_P, FC);
                U.U_Pro = U_P;

                IU.Create_User(ThisUser.LinkMainCID, ThisUser.UserFullName, U);

            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult User_Update()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            User_Filter MF = new User_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;

            MF.LinkMainCID = U.LinkMainCID;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.RoleTitle = Request["RoleTitle"] == null ? string.Empty : Request["RoleTitle"].Trim();
            MF.DepName = Request["DepName"] == null ? string.Empty : Request["DepName"].Trim();
            MF.DepNameList = IU.Get_Department_Name_List(MF.LinkMainCID);

            PageList<User> PList = IU.Get_User_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult User_Update_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            ViewData["UID"] = ID;
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult User_Update_Sub_Post(Guid ID, FormCollection FC)
        {
            Guid UID = ID;
            try
            {
                User U = new User();
                TryUpdateModel<User>(U, FC);

                User_Profile U_P = new User_Profile();
                TryUpdateModel<User_Profile>(U_P, FC);
                U.U_Pro = U_P;
                IU.Set_User(UID, U);
                TempData["Success_Update"] = "更新成功";
                return RedirectToAction("User_Update_Sub", new { ID = UID });
            }
            catch (Exception Ex)
            {
                TempData["Error_Update"] = Ex.Message.ToString();
                return RedirectToAction("User_Update_Sub", new { ID = UID });
            }

        }

        [HttpPost]
        public RedirectToRouteResult User_Update_Delete_Post(Guid ID)
        {
            Guid UID = ID;
            try
            {
                IU.Delete_User(UID);
                TempData["Success_Delete"] = "删除成功";
                return RedirectToAction("User_Update");
            }
            catch (Exception Ex)
            {
                TempData["Error_Delete"] = Ex.Message.ToString();
                return RedirectToAction("User_Update_Sub", new { ID = UID});
            }
        }
    
        public PartialViewResult User_Edit_Part(Guid ID)
        {
            Guid UID = ID;
            User U = new User();
            if(UID == Guid.Empty)
            {
                U = IU.Get_Empty_User(this.MyUser().LinkMainCID);
            }
            else
            {
                U = IU.Get_User_Item_More(UID);
            }
            return PartialView(U);
        }
    }

    public partial class UserController : Controller
    {
        public ActionResult User_LoginRecord()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            User_Filter MF = new User_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;

            MF.LinkMainCID = U.LinkMainCID;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            ViewData["MF"] = MF;
            return View();
        }

    }
}