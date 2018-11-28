using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;
using System.Web.Security;

namespace SMART.EBMS.Controllers
{
    public class LoginController : Controller
    {
        private IUserService IU = new UserService();

        [OutputCache(Duration = 0)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult Index_Post()
        {
            string UserName = Request["UserName"] == null ? string.Empty : Request["UserName"].Trim();
            string Password = Request["Password"] == null ? string.Empty : Request["Password"].Trim();
            string PinCode = Request["PinCode"] == null ? string.Empty : Request["PinCode"].Trim();

            try
            {
                Guid UID = IU.User_Login(UserName, Password, PinCode);
                User U = IU.Get_User_Item(UID);

                //进行票据验证
                bool isPersistent = true;

                //身份验证
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                  1,                                                    //版本号
                  U.UID.ToString(),                                    //与身份验证票关联的用户名
                  DateTime.Now,                                 //issueDate Cookie 签发日期
                  DateTime.Now.AddDays(3),         //expiration Cookie 的到期日期，如果Cookie是持久的，此属性一定要设置
                  isPersistent,                                     //isPersistent  如果 Cookie 是持久的，为 true；否则为 false。
                  string.Empty,                             //将存储在 Cookie 中的用户定义数据
                  FormsAuthentication.FormsCookiePath
                  );

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                //持久化Cookie写入
                if (isPersistent)
                {
                    HttpCookie AuthCookie;
                    AuthCookie = FormsAuthentication.GetAuthCookie(U.UID.ToString(), isPersistent);
                    AuthCookie.Expires = DateTime.Now.AddDays(3);
                    Response.Cookies.Add(AuthCookie);
                }
                return RedirectToAction("Framework", "Index");
            }
            catch (Exception Ex)
            {
                TempData["LoginError"] = Ex.Message.ToString();
                TempData["UserName"] = UserName;
                TempData["Password"] = Password;
                TempData["PinCode"] = PinCode;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public RedirectToRouteResult LoginOut()
        {
            //清除Session
            Session.Abandon();
            //注销票证
            FormsAuthentication.SignOut();
            //取消缓存

            //清楚Cookie
            HttpCookie aCookie = new HttpCookie(".SMART_EBMS");
            aCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(aCookie);
            return RedirectToAction("Index");
        }

    }
}