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
    public partial class SystemController : Controller
    {
        private IUserService IU = new UserService();
        IMainCompanyService IM = new MainCompanyService();
        ISentEmailService ISent = new SentEmailService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //企业资料
    public partial class SystemController : Controller
    {
        public ActionResult MainCom_Update()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            MainCompany MC = IM.Get_MainCompany(U.LinkMainCID);
            return View(MC);
        }

        public ActionResult MainCom_Update_Sub()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            MainCompany MC = IM.Get_MainCompany(U.LinkMainCID);
            return View(MC);
        }

        [HttpPost]
        public RedirectToRouteResult MainCom_Update_Sub_Post(FormCollection FC)
        {
            try
            {
                MainCompany MC = new MainCompany();
                TryUpdateModel<MainCompany>(MC, FC);
                IM.Set_MainCom_Base(this.MyUser().LinkMainCID, MC);
                return RedirectToAction("MainCom_Update");
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("MainCom_Update_Sub");
            }
        }

        [HttpPost]
        public RedirectToRouteResult MainCom_Update_ElectronicStamp_Post(Guid ID)
        {
            Guid LinkMainCID = ID;
            try
            {
                HttpPostedFileBase LogoImg = Request.Files["ImagePath"];
                MyImgUploadFile MyImg = new MyImgUploadFile();
                string ImgPath = MyImg.ImgUpLoadFileProcess(LogoImg, "MainCompany/" + LinkMainCID.ToString());
                IM.Set_MainCom_ElectronicStamp(LinkMainCID, ImgPath);
            }
            catch (Exception Ex)
            {
                TempData["Error_ElectronicStamp"] = Ex.Message.ToString();
            }
            return RedirectToAction("MainCom_Update_Sub");
        }

        [HttpPost]
        public RedirectToRouteResult MainCom_Update_ComLogo_Post(Guid ID)
        {
            Guid LinkMainCID = ID;
            try
            {
                HttpPostedFileBase LogoImg = Request.Files["ImagePath"];
                MyImgUploadFile MyImg = new MyImgUploadFile();
                string ImgPath = MyImg.ImgUpLoadFileProcess(LogoImg, "MainCompany/" + LinkMainCID.ToString());
                IM.Set_MainCom_ComLogo(LinkMainCID, ImgPath);
            }
            catch (Exception Ex)
            {
                TempData["Error_Logo"] = Ex.Message.ToString();
            }
            return RedirectToAction("MainCom_Update_Sub");
        }
    }

    //部门设置
    public partial class SystemController : Controller
    {
        public ActionResult Department()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            List<Department> List = IU.Get_Department_List(U.LinkMainCID);
            return View(List);
        }

        public PartialViewResult Department_Sub(Guid ID)
        {
            Department Dep = IU.Get_Department_Item(ID);
            return PartialView(Dep);
        }

        public string Department_Sub_Post(Guid ID, FormCollection FC)
        {
            string result = string.Empty;
            User U = this.MyUser();
            try
            {
                Guid DepID = ID;
                Department Dep = new Department();
                TryUpdateModel<Department>(Dep, FC);
                if (DepID == Guid.Empty)
                {
                    IU.Create_Department(U.LinkMainCID, Dep);
                }
                else
                {
                    IU.Set_Department(U.LinkMainCID, Dep);
                }
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }


        public string Department_Sub_Delete(Guid ID)
        {
            string result = string.Empty;
            User U = this.MyUser();
            try
            {
                Guid DepID = ID;
                IU.Delete_Department(DepID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

    }

    //SMTP邮箱
    public partial class SystemController : Controller
    {
        public ActionResult SMTP_Email()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            SentEmail SE = ISent.Get_SentEmail(U.LinkMainCID);
            return View(SE);
        }

        public RedirectToRouteResult SMTP_Email_Post(FormCollection FC)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            try
            {
                SentEmail SE = new SentEmail();
                TryUpdateModel<SentEmail>(SE, FC);
                ISent.Set_SentEmail(U.LinkMainCID,SE);
                TempData["OK_Email"] = "OK_Email";
            }
            catch (Exception Ex)
            {
                TempData["ErrorEmail"] = Ex.Message.ToString();
            }
            return RedirectToAction("SMTP_Email");
        }

        public RedirectToRouteResult SMTP_Email_TestMail_Post(FormCollection FC)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            string MailToAddress = Request["MailToAddress"] == null ? string.Empty : Request["MailToAddress"].Trim();
            TempData["MailToAddress"] = MailToAddress;
            try
            {
                ISent.Sent_Email_For_Test(U, MailToAddress);
                TempData["OK_EmailTest"] = "系统测试邮件已发送，请检查收件箱";
            }
            catch (Exception Ex)
            {
                TempData["ErrorEmailTest"] = Ex.Message.ToString();
            }
            return RedirectToAction("SMTP_Email");
        }
    }

}