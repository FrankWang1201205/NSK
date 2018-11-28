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
    public partial class POController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        IBrandService IBrand = new BrandService();
        IPurchaseService IPur = new PurchaseService();
        ISupplierService ISup = new SupplierService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class POController : Controller
    {
        public ActionResult PlanToPo()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            PurchasePlan_Filter MF = new PurchasePlan_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();

            Supplier_Filter Sup_MF = new Supplier_Filter();
            Sup_MF.LinkMainCID = U.LinkMainCID;
            Sup_MF.Keyword = string.Empty;
            List<Supplier> SupList = ISup.Get_Supplier_List(Sup_MF);
            ViewData["SupList"] = SupList;

            PurchasePlan Po_Plan = IPur.Get_Po_Head_Plan_For_Po_Create(MF);
            ViewData["MF"] = MF;
            return View(Po_Plan);
        }

        [HttpPost]
        public ActionResult PlanToPo_Preview(string ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            string Month = ID;

            List<Guid> MatIDList = CommonFunctionLib.GuidListStrToGuidArray(Request["MatID"].ToString());
            List<PurchasePlan_Line> PlanLineList = new List<PurchasePlan_Line>();
            PurchasePlan_Line Line = new PurchasePlan_Line();
            foreach (var MatID in MatIDList)
            {
                Line = new PurchasePlan_Line();
                Line.MatID = MatID;
                Line.PoPlan_Next_Mon_Qty_A = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_A_" + MatID.ToString()]);
                Line.PoPlan_Next_Mon_Qty_B = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_B_" + MatID.ToString()]);
                Line.PoPlan_Next_Mon_Qty_C = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_C_" + MatID.ToString()]);

                Line.PoPlan_Next_Mon_Delivery_A = Convert.ToDateTime(Request["PoPlan_Next_Mon_Delivery_A_" + MatID.ToString()]).ToString("yyyy-MM-dd");
                Line.PoPlan_Next_Mon_Delivery_B = Convert.ToDateTime(Request["PoPlan_Next_Mon_Delivery_B_" + MatID.ToString()]).ToString("yyyy-MM-dd");
                Line.PoPlan_Next_Mon_Delivery_C = Convert.ToDateTime(Request["PoPlan_Next_Mon_Delivery_C_" + MatID.ToString()]).ToString("yyyy-MM-dd");

                PlanLineList.Add(Line);
            }

            Guid SupID = new Guid(Request["SupID"].ToString());
            Po_Head Head = IPur.Get_Po_Head_By_PoPlan(PlanLineList, SupID, this.MyUser().UID, Month);
            return View(Head);
        }

        [HttpPost]
        public RedirectToRouteResult PlanToPo_Preview_Post(string ID)
        {
            string Month = ID;
            User U = this.MyUser();
            ViewData["User"] = U;
            try
            {
                List<Guid> POLIDList = CommonFunctionLib.GuidListStrToGuidArray(Request["POLID"].ToString());
                List<Po_Line> LineList = new List<Po_Line>();
                Po_Line Line = new Po_Line();
                foreach (var POLID in POLIDList)
                {
                    Line = new Po_Line();
                    Line.MatID = new Guid(Request["MatID_" + POLID.ToString()]);
                    Line.Qty = Convert.ToInt32(Request["Qty_" + POLID.ToString()]);
                    Line.Delivery_DT = Convert.ToDateTime(Request["Delivery_DT_" + POLID.ToString()]);
                    LineList.Add(Line);
                }

                Guid SupID = new Guid(Request["SupID"].ToString());
                Guid POID = IPur.Create_Po_Head(LineList, SupID, this.MyUser().UID, Month);
                return RedirectToAction("PlanToPo");
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("PlanToPo");
            }
        }
    }

    public partial class POController : Controller
    {
        public ActionResult SentToSup()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Po_Head_Filter MF = new Po_Head_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Status = Po_Status_Enum.待发送供应商.ToString();
            PageList<Po_Head> PList = IPur.Get_Po_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult SentToSup_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid POID = ID;
            Po_Head Head = IPur.Get_Po_Head_Item(POID);
            return View(Head);
        }

        [HttpPost]
        public RedirectToRouteResult SentToSup_Sub_Post(Guid ID)
        {
            Guid POID = ID;
            try
            {
                DateTime Create_DT = Convert.ToDateTime(Request["Create_DT"].ToString());
                IPur.Po_To_Supplier_Confirm(POID, Create_DT);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("SentToSup");
        }

        public ActionResult ConfirmPo()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Po_Head_Filter MF = new Po_Head_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.Status = Po_Status_Enum.待供应商确认.ToString();
            PageList<Po_Head> PList = IPur.Get_Po_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult ConfirmPo_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid POID = ID;
            Po_Head Head = IPur.Get_Po_Head_Item(POID);
            return View(Head);
        }

        [HttpPost]
        public RedirectToRouteResult ConfirmPo_Sub_Post(Guid ID)
        {
            Guid POID = ID;
            try
            {
                IPur.Po_To_Supplier_Confirm_Check(POID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("ConfirmPo");
        }

    }

    public partial class POController : Controller
    {
        public ActionResult PoHead_List()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Po_Head_Filter MF = new Po_Head_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Status = Request["Status"] == null ? string.Empty : Request["Status"].Trim();
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            PageList<Po_Head> PList = IPur.Get_Po_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult PoHead_Edit(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid POID = ID;
            Po_Head Head = IPur.Get_Po_Head_Item(POID);
            return View(Head);
        }

        [HttpPost]
        public string PoHead_Edit_Line_Set(Guid ID, FormCollection FC)
        {
            string result = string.Empty;

            Guid POLID = ID;
            try
            {
                Po_Line Line = new Po_Line();
                TryUpdateModel<Po_Line>(Line, FC);
                IPur.Set_Po_Line_Item(POLID, Line);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public RedirectToRouteResult PoHead_Edit_Update_Post(Guid ID, FormCollection FC)
        {
            Guid POID = ID;
            try
            {
                Po_Head Head = new Po_Head();
                TryUpdateModel<Po_Head>(Head, FC);
                if (Head.Status == Po_Status_Enum.待发送供应商.ToString())
                {
                    return RedirectToAction("SentToSup_Sub", new { ID = POID });
                }
                else if (Head.Status == Po_Status_Enum.待供应商确认.ToString())
                {
                    return RedirectToAction("ConfirmPo_Sub", new { ID = POID });
                }
                else
                {
                    TempData["Success_Po_Update"] = "更新成功";
                    return RedirectToAction("PoHead_Edit", new { ID = POID });
                }
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
                return RedirectToAction("PoHead_Edit", new { ID = POID });
            }
        }

        [HttpPost]
        public RedirectToRouteResult PoHead_Edit_Delete_Post(Guid ID)
        {
            Guid POID = ID;
            string Po_Head_Status = Request["Status"] == null ? string.Empty : Request["Status"].Trim();

            try
            {
                IPur.Delete_Po_Head(POID);
                if (Po_Head_Status == Po_Status_Enum.待发送供应商.ToString())
                {
                    return RedirectToAction("SentToSup");

                }
                else if (Po_Head_Status == Po_Status_Enum.待供应商确认.ToString())
                {
                    return RedirectToAction("ConfirmPo");
                }
                else
                {
                    return RedirectToAction("PoHead_List");
                }
            }
            catch (Exception Ex)
            {
                TempData["Error_Delete"] = Ex.Message.ToString();

                if (Po_Head_Status == Po_Status_Enum.待发送供应商.ToString())
                {
                    return RedirectToAction("SentToSup_Sub", new { ID = ID });
                }
                else if (Po_Head_Status == Po_Status_Enum.待供应商确认.ToString())
                {
                    return RedirectToAction("ConfirmPo_Sub", new { ID = ID });
                }
                else
                {
                    return RedirectToAction("PoHead_Edit", new { ID = ID });
                }
            }
        }

        public ActionResult PoHead_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Po_Head_Filter MF = new Po_Head_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Status = Request["Status"] == null ? string.Empty : Request["Status"].Trim();
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            PageList<Po_Head> PList = IPur.Get_Po_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult PoHead_Search_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid POID = ID;
            Po_Head Head = IPur.Get_Po_Head_Item(POID);
            return View(Head);
        }

    }

}