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
    public partial class CatalogController : Controller
    {
        IUserService IU = new UserService();
        ICategoryService IC = new CategoryService();
        IMaterialService IMat = new MaterialService();
        IBrandService IB = new BrandService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class CatalogController : Controller
    {
        public ActionResult Cat_Config()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            CatTree CT = new CatTree();
            CT = IC.Get_CatTree(U.LinkMainCID);
            return View(CT);
        }

        public RedirectToRouteResult Cat_Config_Post(FormCollection FC)
        {
            User U = this.MyUser();
            try
            {
                Category Cat = new Category();
                TryUpdateModel<Category>(Cat, FC);
                IC.Create_Category(Cat, U.LinkMainCID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Cat_Config");
        }

        public PartialViewResult Cat_Config_Sub(Guid ID)
        {
            Guid CatID = ID;
            Category Cat = IC.Get_Category_Item(CatID);
            return PartialView(Cat);
        }

        public RedirectToRouteResult Cat_Config_Sub_Post(Guid ID, FormCollection FC)
        {
            Guid CatID = ID;
            try
            {
                Category Cat = new Category();
                TryUpdateModel<Category>(Cat, FC);
                IC.Set_Category(CatID, Cat);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Cat_Config");
        }

        public RedirectToRouteResult Cat_Config_Sub_Delete(Guid ID)
        {
            Guid CatID = ID;
            try
            {
                IC.Delete_Category(CatID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Cat_Config");
        }
    }

    public partial class CatalogController : Controller
    {
        public ActionResult MatToCatList()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.MatName = Request["MatName"] == null ? string.Empty : Request["MatName"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();

            MF.CatID_Str = Request["CatID_Str"] == null ? Cat_Enum.NoneCat.ToString() : Request["CatID_Str"].Trim();
            try { MF.CatID = new Guid(Request["CatID_Str"].ToString()); } catch { MF.CatID = Guid.Empty; }

            PageList<Material> PList = IMat.Get_Material_PageList(MF);
            MF.BrandList = IB.Get_Brand_Name_List(U.LinkMainCID);
            List<Category> CatList = IC.Get_Category_List(U.LinkMainCID);
            ViewData["CatList"] = CatList;
            ViewData["MF"] = MF;
            return View(PList);
        }

        public RedirectToRouteResult MatToCatList_Post(FormCollection FC)
        {
            Material_Filter MF = new Material_Filter();
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.CatID_Str = Request["CatID_Str"] == null ? Cat_Enum.NoneCat.ToString() : Request["CatID_Str"].Trim();

            Guid CatID = Guid.Empty;
            try
            {
                try { CatID = new Guid(Request["ToCatID"]); } catch { }
                List<Guid> MatIDList = CommonFunctionLib.GuidListStrToGuidArray(Request["MatID"].ToString());
                IMat.Set_Mat_CatID_Batch(MatIDList, CatID);
                TempData["Textarea_Success"] = "Success";
            }
            catch (Exception Ex) {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("MatToCatList", new { CatID_Str = MF.CatID_Str, ToCatID = CatID, MatBrand = MF.MatBrand, Keyword = MF.Keyword });
        }
    }
}