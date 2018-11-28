using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;
using System.Threading;

namespace SMART.EBMS.Controllers
{
    [Authorize]
    public partial class MatSearchController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        IBrandService IBrand = new BrandService();
        ICustomerService ICust = new CustomerService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class MatSearchController : Controller
    {
        public ActionResult SearchList()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;

            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.Price_Is_AM = Request["Price_Is_AM"] == null ? string.Empty : Request["Price_Is_AM"].Trim();
            MF.Is_Stock = Request["Is_Stock"] == null ? string.Empty : Request["Is_Stock"].Trim();
            MF.PC_Place = Request["PC_Place"] == null ? string.Empty : Request["PC_Place"].Trim();
            MF.PC = Request["PC"] == null ? string.Empty : Request["PC"].Trim();
            MF.CODE = Request["CODE"] == null ? string.Empty : Request["CODE"].Trim();
            MF.PC_Mon_Type = Request["PC_Mon_Type"] == null ? string.Empty : Request["PC_Mon_Type"].Trim();
            MF.MatSn = Request["MatSn"] == null ? string.Empty : Request["MatSn"].Trim();
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            MF.Keyword = string.Empty;
            MF.BrandList = IBrand.Get_Brand_Name_List(MF.LinkMainCID);
            PageList<Material> PList = IMat.Get_Material_Stand_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }

    public partial class MatSearchController : Controller
    {
        public ActionResult CustMatList()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Customer_Filter MF = new Customer_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Cust_Name = Request["Cust_Name"] == null ? string.Empty : Request["Cust_Name"].Trim();
            MF.Cust_Type = Request["Cust_Type"] == null ? string.Empty : Request["Cust_Type"].Trim();
            MF.Sales_UID = Guid.Empty;
            try { MF.Sales_UID = new Guid(Request["Sales_UID"]); } catch { MF.Sales_UID = Guid.Empty; }

            PageList<Customer> PList = ICust.Get_Customer_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

    }
}