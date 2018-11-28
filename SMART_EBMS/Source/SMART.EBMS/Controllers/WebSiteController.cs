using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;

namespace SMART.EBMS.Controllers
{
    public partial class WebSiteController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        IBrandService IB = new BrandService();
        ICategoryService IC = new CategoryService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class WebSiteController : Controller
    {
        public ActionResult PublicList()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.IsPublic = Request["IsPublic"] == null ? Mat_Public.未上架.ToString() : Request["IsPublic"].Trim();

            MF.CatID_Str = Request["CatID_Str"] == null ? Cat_Enum.ALL.ToString() : Request["CatID_Str"].Trim();
            try { MF.CatID = new Guid(Request["CatID_Str"].ToString()); } catch { MF.CatID = Guid.Empty; }

            PageList<Material> PList = IMat.Get_Material_PageList(MF);
            MF.BrandList = IB.Get_Brand_Name_List(U.LinkMainCID);
            List<Category> CatList = IC.Get_Category_List(U.LinkMainCID);
            ViewData["CatList"] = CatList;
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult TopList()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            return View();
        }

        public ActionResult PromotionList()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            return View();
        }
    }
}