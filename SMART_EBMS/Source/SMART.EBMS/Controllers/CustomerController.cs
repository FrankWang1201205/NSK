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
    public partial class CustomerController : Controller
    {
        IUserService IU = new UserService();
        ICustomerService ICust = new CustomerService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class CustomerController : Controller
    {
        public PartialViewResult Cu_Edit_Part(Guid ID)
        {
            User U = this.MyUser();
            Guid CID = ID;
            Customer C = new Customer();
            if (CID == Guid.Empty)
            {
                C = ICust.Get_Customer_Empty(U.LinkMainCID);
            }
            else
            {
                C = ICust.Get_Customer_Item(CID);
            }
            return PartialView(C);
        }

        public ActionResult Cu_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Customer_Filter MF = new Customer_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Cust_Code = Request["Cust_Code"] == null ? string.Empty : Request["Cust_Code"].Trim();
            MF.Cust_Name = Request["Cust_Name"] == null ? string.Empty : Request["Cust_Name"].Trim();
            MF.Cust_Type = Request["Cust_Type"] == null ? string.Empty : Request["Cust_Type"].Trim();
            MF.District = Request["District"] == null ? string.Empty : Request["District"].Trim();
            MF.Industry = Request["Industry"] == null ? string.Empty : Request["Industry"].Trim();
            MF.FormBy = Request["FormBy"] == null ? string.Empty : Request["FormBy"].Trim();
            MF.Sales_Type = Request["Sales_Type"] == null ? string.Empty : Request["Sales_Type"].Trim();
            MF.Settle_Accounts = Request["Settle_Accounts"] == null ? string.Empty : Request["Settle_Accounts"].Trim();
            MF.Payment_Type = Request["Payment_Type"] == null ? string.Empty : Request["Payment_Type"].Trim();
            MF.Statement_Day = Request["Statement_Day"] == null ? string.Empty : Request["Statement_Day"].Trim();
            MF.Contract_Type = Request["Contract_Type"] == null ? string.Empty : Request["Contract_Type"].Trim();
            MF.Ship_Type = Request["Ship_Type"] == null ? string.Empty : Request["Ship_Type"].Trim();


            MF.Sales_UID = Guid.Empty;
            try { MF.Sales_UID = new Guid(Request["Sales_UID"]); } catch { MF.Sales_UID = Guid.Empty; }

            PageList<Customer> PList = ICust.Get_Customer_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Cu_Add()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        [HttpPost]
        public string Cu_Add_Post(FormCollection FC)
        {
            string result = string.Empty;
            try
            {
                Customer C = new Customer();
                TryUpdateModel<Customer>(C, FC);
                ICust.Create_Customer(C, this.MyUser().LinkMainCID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult Cu_Update()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Customer_Filter MF = new Customer_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Cust_Code = Request["Cust_Code"] == null ? string.Empty : Request["Cust_Code"].Trim();
            MF.Cust_Name = Request["Cust_Name"] == null ? string.Empty : Request["Cust_Name"].Trim();
            MF.Cust_Type = Request["Cust_Type"] == null ? string.Empty : Request["Cust_Type"].Trim();
            MF.District = Request["District"] == null ? string.Empty : Request["District"].Trim();
            MF.Industry = Request["Industry"] == null ? string.Empty : Request["Industry"].Trim();
            MF.FormBy = Request["FormBy"] == null ? string.Empty : Request["FormBy"].Trim();
            MF.Sales_Type = Request["Sales_Type"] == null ? string.Empty : Request["Sales_Type"].Trim();
            MF.Settle_Accounts = Request["Settle_Accounts"] == null ? string.Empty : Request["Settle_Accounts"].Trim();
            MF.Payment_Type = Request["Payment_Type"] == null ? string.Empty : Request["Payment_Type"].Trim();
            MF.Statement_Day = Request["Statement_Day"] == null ? string.Empty : Request["Statement_Day"].Trim();
            MF.Contract_Type = Request["Contract_Type"] == null ? string.Empty : Request["Contract_Type"].Trim();
            MF.Ship_Type = Request["Ship_Type"] == null ? string.Empty : Request["Ship_Type"].Trim();

            MF.Sales_UID = Guid.Empty;
            try { MF.Sales_UID = new Guid(Request["Sales_UID"]); } catch { MF.Sales_UID = Guid.Empty; }

            PageList<Customer> PList = ICust.Get_Customer_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Cu_Update_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            ViewData["CID"] = ID;
            return View();
        }


        [HttpPost]
        public RedirectToRouteResult Cu_Update_Sub_Post(Guid ID, FormCollection FC)
        {
            Guid CID = ID;
            try
            {
                Customer C = new Customer();
                TryUpdateModel<Customer>(C, FC);
                ICust.Set_Customer_Base(CID, C);
                TempData["Success"] = "更新成功！";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Cu_Update_Sub", new { ID = CID });
        }

      

        [HttpPost]
        public RedirectToRouteResult Cu_Delete_post(Guid ID)
        {
            try
            {
                ICust.Delete_Customer(ID);
                TempData["Success"] = "删除成功！";
                return RedirectToAction("Cu_Update", "Customer", new { ID = ID });
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("Cu_Update_Sub", "Customer", new { ID = ID });
            }
        }
    }


    public partial class CustomerController : Controller
    {
        public ActionResult Cu_Preview(Guid ID)
        {
            User U = new User();
            U = this.MyUser();

            Customer C = ICust.Get_Customer_Item(ID);
            return View(C);
        }
    }

}