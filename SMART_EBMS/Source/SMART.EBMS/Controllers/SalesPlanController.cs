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
    public partial class SalesPlanController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        IBrandService IBrand = new BrandService();
        ISalesPlanService ISPlan = new SalesPlanService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //申报预测
    public partial class SalesPlanController : Controller
    {
        public ActionResult MonthPlan()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            SalesPlan_Filter MF = new SalesPlan_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Sales_UID = U.UID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.IsDeclare = Request["IsDeclare"] == null ? IsDeclare_Enum.未申报.ToString() : Request["IsDeclare"].Trim();

            MF.BrandList = IBrand.Get_Brand_Name_List(MF.LinkMainCID);
            SalesPlan SP = ISPlan.Get_SalesPlan_NextMonth(MF);
            ViewData["MF"] = MF;
            return View(SP);
        }

        public string MonthPlan_Set_Line(Guid ID, FormCollection FC)
        {
            string result = string.Empty;
            Guid SPID = ID;
            try
            {
                Guid MatID = new Guid(Request["MatID"].ToString());
                SalesPlan_Line NewLine = new SalesPlan_Line();
                TryUpdateModel<SalesPlan_Line>(NewLine, FC);
                ISPlan.Set_NextMonth_Line(SPID, MatID, NewLine);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public string MonthPlan_Set_Line_IsDeclare(Guid ID)
        {
            string result = string.Empty;
            Guid SPID = ID;
            try
            {
                string IsDeclare = Request["IsDeclare"] == null ? string.Empty : Request["IsDeclare"].Trim();
                List<Guid> MatIDList = CommonFunctionLib.GuidListStrToGuidArray(Request["MatID"]);
                if (IsDeclare == IsDeclare_Enum.已申报.ToString())
                {
                    ISPlan.Set_NextMonth_Line_IsDeclare_Cancel(MatIDList, SPID);
                }
                else
                {
                    ISPlan.Set_NextMonth_Line_IsDeclare(MatIDList, SPID);
                }
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;


        }

        public ActionResult MonthPlan_Aud()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            SalesPlan_Filter MF = new SalesPlan_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Sales_UID = Guid.Empty;
            MF.Month = DTList.NextMonth().SD.ToString("yyyy-MM");
            try { MF.Sales_UID = new Guid(Request["Sales_UID"]); } catch { }

            PageList<SalesPlan> PList = ISPlan.Get_SalesPlan_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }


        public ActionResult MonthPlan_Aud_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            SalesPlan_Filter MF = new SalesPlan_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.IsDeclare = IsDeclare_Enum.已申报.ToString();

            MF.BrandList = IBrand.Get_Brand_Name_List(MF.LinkMainCID);
            Guid SPID = ID;
            SalesPlan SP = ISPlan.Get_SalesPlan_Item(SPID, MF);
            ViewData["MF"] = MF;
            return View(SP);
        }


        public RedirectToRouteResult MonthPlan_Aud_Sub_Post(Guid ID)
        {
            Guid SPID = ID;
            try
            {
                string Aud_Person = this.MyUser().UserFullName;
                ISPlan.Set_SalesPlan_To_Pass(SPID, Aud_Person);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("MonthPlan_Aud_Sub", new { ID = ID });
        }


        public RedirectToRouteResult MonthPlan_Aud_Sub_Post_Back(Guid ID)
        {
            Guid SPID = ID;
            try
            {
                ISPlan.Set_SalesPlan_To_Back(SPID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("MonthPlan_Aud_Sub", new { ID = ID });
        }


        public ActionResult MonthPlan_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            SalesPlan_Filter MF = new SalesPlan_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Sales_UID = Guid.Empty;
            try { MF.Sales_UID = new Guid(Request["Sales_UID"]); } catch { }

            PageList<SalesPlan> PList = ISPlan.Get_SalesPlan_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult MonthPlan_Search_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            SalesPlan_Filter MF = new SalesPlan_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.IsDeclare = IsDeclare_Enum.已申报.ToString();

            MF.BrandList = IBrand.Get_Brand_Name_List(MF.LinkMainCID);
            Guid SPID = ID;
            SalesPlan SP = ISPlan.Get_SalesPlan_Item(SPID, MF);
            ViewData["MF"] = MF;
            return View(SP);
        }
    }



    //我的产品
    public partial class SalesPlanController : Controller
    {
        public ActionResult MatForSales()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Sales_UID = U.UID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.IsMySales = Request["IsMySales"] == null ? MySales_Enum.IsNotMySales.ToString() : Request["IsMySales"].Trim();

            MF.BrandList = IBrand.Get_Brand_Name_List(MF.LinkMainCID);
            PageList<Material> PList = IMat.Get_Material_PageList_For_MySale(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public RedirectToRouteResult MatForSales_Create_Post()
        {
            List<Guid> MatIDList = CommonFunctionLib.GuidListStrToGuidArray(Request["MatID"].ToString());
            try
            {
                ISPlan.Create_MatSales_Lib(MatIDList, this.MyUser().UID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("MatForSales");
        }

        public RedirectToRouteResult MatForSales_Delete_Post()
        {
            List<Guid> MatIDList = CommonFunctionLib.GuidListStrToGuidArray(Request["MatID"].ToString());
            try
            {
                ISPlan.Remove_MatSales_Lib(MatIDList, this.MyUser().UID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("MatForSales", new { IsMySales = MySales_Enum.IsMySales.ToString() });
        }
    }


}