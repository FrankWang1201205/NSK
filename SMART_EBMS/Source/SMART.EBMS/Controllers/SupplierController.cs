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
    public partial class SupplierController : Controller
    {
        IUserService IU = new UserService();
        ISupplierService ISup = new SupplierService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }

        public ActionResult Sup_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Supplier_Filter MF = new Supplier_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Type = Request["Type"] == null ? string.Empty : Request["Type"].Trim();
            MF.Qualification = Request["Qualification"] == null ? string.Empty : Request["Qualification"].Trim();
            MF.Sup_Level = Request["Sup_Level"] == null ? string.Empty : Request["Sup_Level"].Trim();
            MF.District = Request["District"] == null ? string.Empty : Request["District"].Trim();
            MF.Sup_Brand = Request["Sup_Brand"] == null ? string.Empty : Request["Sup_Brand"].Trim();
            MF.Act_Status = Request["Act_Status"] == null ? string.Empty : Request["Act_Status"].Trim();
            PageList<Supplier> PList = ISup.Get_Supplier_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Sup_Add()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            return View();
        }

        [HttpPost]
        public string Sup_Add_Post(FormCollection FC)
        {
            string result = string.Empty;
            try
            {
                Supplier S = new Supplier();
                TryUpdateModel<Supplier>(S, FC);

                S.Main_Business_Json = Request.Form["Main_Business_Json"] == null ? string.Empty : Request.Form["Main_Business_Json"].Trim();
                S.Main_Brand_Json = Request.Form["Main_Brand_Json"] == null ? string.Empty : Request.Form["Main_Brand_Json"].Trim();
                S.Payment_Json = Request.Form["Payment_Json"] == null ? string.Empty : Request.Form["Payment_Json"].Trim();
                S.Settle_Accounts_Json = Request.Form["Settle_Accounts_Json"] == null ? string.Empty : Request.Form["Settle_Accounts_Json"].Trim();

                ISup.Create_Supplier(S, this.MyUser().UID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult Sup_Update()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Supplier_Filter MF = new Supplier_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Type = Request["Type"] == null ? string.Empty : Request["Type"].Trim();
            MF.Qualification = Request["Qualification"] == null ? string.Empty : Request["Qualification"].Trim();
            MF.Sup_Level = Request["Sup_Level"] == null ? string.Empty : Request["Sup_Level"].Trim();
            MF.District = Request["District"] == null ? string.Empty : Request["District"].Trim();
            MF.Sup_Brand = Request["Sup_Brand"] == null ? string.Empty : Request["Sup_Brand"].Trim();
            MF.Act_Status = Request["Act_Status"] == null ? string.Empty : Request["Act_Status"].Trim();

            PageList<Supplier> PList = ISup.Get_Supplier_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Sup_Update_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            ViewData["SupID"] = ID;
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult Sup_Update_Sub_Update_Post(Guid ID, FormCollection FC)
        {
            Guid SupID = ID;
            try
            {
                Supplier S = new Supplier();
                TryUpdateModel<Supplier>(S, FC);
                S.Main_Business_Json = Request.Form["Main_Business_Json"] == null ? string.Empty : Request.Form["Main_Business_Json"].Trim();
                S.Main_Brand_Json = Request.Form["Main_Brand_Json"] == null ? string.Empty : Request.Form["Main_Brand_Json"].Trim();
                S.Payment_Json = Request.Form["Payment_Json"] == null ? string.Empty : Request.Form["Payment_Json"].Trim();
                S.Settle_Accounts_Json = Request.Form["Settle_Accounts_Json"] == null ? string.Empty : Request.Form["Settle_Accounts_Json"].Trim();
                ISup.Set_Supplier_Base(SupID, S);
                TempData["Success"] = "更新成功！";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Sup_Update_Sub", new { ID = SupID });
        }

        [HttpPost]
        public RedirectToRouteResult Sup_Delete_post(Guid ID)
        {
            try
            {
                ISup.Delete_Supplier(ID);
                TempData["Success"] = "删除成功！";
                return RedirectToAction("Sup_Update", new { ID = ID });
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("Sup_Update_Sub", new { ID = ID });
            }
        }
           
        public PartialViewResult Sup_Edit_Part(Guid ID)
        {
            Guid SupID = ID;
            Supplier Sup = new Supplier();
            if(SupID == Guid.Empty)
            {
                Sup = ISup.Get_Supplier_Empty();
            }
            else
            {
                Sup = ISup.Get_Supplier_Item(SupID);
            }
            return PartialView(Sup);
        }

    }

    public partial class SupplierController : Controller
    {
        public ActionResult Sup_Preview(Guid ID)
        {
            User U = new User();
            U = this.MyUser();

            Supplier C = ISup.Get_Supplier_Item(ID);
            return View(C);
        }
    }

}