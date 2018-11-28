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
    public partial class PurchasePlanController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        IBrandService IBrand = new BrandService();
        IPurchaseService IPur = new PurchaseService();
        ISalesPlanService ISPlan = new SalesPlanService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //新增计划
    public partial class PurchasePlanController : Controller
    {
        public ActionResult Plan_Group()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            PurchasePlan_Group PG = IPur.Get_PurchasePlan_Group(U.LinkMainCID);
            return View(PG);
        }

        public ActionResult Plan_Group_Sub()
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
            MF.Line_Status = PurchasePlan_Line_Status.待下达计划.ToString();
            PurchasePlan Po_Plan = IPur.Get_PurchasePlan_NextMonth_Detail(MF);
            ViewData["MF"] = MF;
            return View(Po_Plan);
        }

        public PartialViewResult Plan_Group_Sub_Item(Guid ID)
        {
            Guid MatID = ID;
            string Month = Request["Month"] == null ? string.Empty : Request["Month"].Trim();
            PurchasePlan_Line Line = IPur.Get_PurchasePlan_Line_Item(MatID, this.MyUser().LinkMainCID, Month);
            return PartialView(Line);
        }


        public string Plan_Group_Sub_Set_Line(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid MatID = ID;
                Guid MainCID = this.MyUser().LinkMainCID;
                string Month = Request["Month"] == null ? string.Empty : Request["Month"].Trim();
                PurchasePlan_Line PlanLine = new PurchasePlan_Line();
                PlanLine.PoPlan_Next_Mon_Qty_A = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_A"].ToString());
                PlanLine.PoPlan_Next_Mon_Qty_B = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_B"].ToString());
                PlanLine.PoPlan_Next_Mon_Qty_C = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_C"].ToString());
                IPur.Set_PurchasePlan_Line(PlanLine, MatID, MainCID, Month);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string Plan_Group_Sub_Post(string ID)
        {
            string result = string.Empty;
            User U = this.MyUser();

            try
            {
                string Month = ID;
                List<Guid> MatIDList = CommonFunctionLib.GuidListStrToGuidArray(Request["MatID"].ToString());
                List<PurchasePlan_Line> PlanLineList = new List<PurchasePlan_Line>();
                PurchasePlan_Line Line = new PurchasePlan_Line();
                foreach(var MatID in MatIDList)
                {
                    Line = new PurchasePlan_Line();
                    Line.MatID = MatID;
                    Line.PoPlan_Next_Mon_Qty_A = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_A_" + MatID.ToString()]);
                    Line.PoPlan_Next_Mon_Qty_B = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_B_" + MatID.ToString()]);
                    Line.PoPlan_Next_Mon_Qty_C = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_C_" + MatID.ToString()]);
                    PlanLineList.Add(Line);
                }
                IPur.Po_Plan_Is_Sent_Buyer(PlanLineList, Month, U.LinkMainCID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

    }

    //更新计划
    public partial class PurchasePlanController : Controller
    {
        public ActionResult Plan_Update()
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
            MF.Po_Head_Status = Request["Po_Head_Status"] == null ? Po_Status_Enum.待采购员下单.ToString() + Po_Status_Enum.待发送供应商.ToString() + Po_Status_Enum.待供应商确认.ToString() : Request["Po_Head_Status"].Trim();
            MF.Line_Status = PurchasePlan_Line_Status.已下达计划.ToString();
            PurchasePlan Po_Plan = IPur.Get_PurchasePlan_NextMonth_Detail(MF);
            ViewData["MF"] = MF;
            return View(Po_Plan);
        }

        public PartialViewResult Plan_Update_Sub(Guid ID)
        {
            Guid MatID = ID;
            string Month = Request["Month"] == null ? string.Empty : Request["Month"].Trim();
            PurchasePlan_Line Line = IPur.Get_PurchasePlan_Line_Item(MatID, this.MyUser().LinkMainCID, Month);
            return PartialView(Line);
        }

        public string Plan_Update_Sub_Post(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid MatID = ID;
                Guid MainCID = this.MyUser().LinkMainCID;
                string Month = Request["Month"] == null ? string.Empty : Request["Month"].Trim();
                PurchasePlan_Line PlanLine = new PurchasePlan_Line();
                PlanLine.PoPlan_Next_Mon_Qty_A = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_A"].ToString());
                PlanLine.PoPlan_Next_Mon_Qty_B = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_B"].ToString());
                PlanLine.PoPlan_Next_Mon_Qty_C = Convert.ToInt32(Request["PoPlan_Next_Mon_Qty_C"].ToString());
                IPur.Set_PurchasePlan_Line(PlanLine, MatID, MainCID, Month);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    // 查看计划
    public partial class PurchasePlanController : Controller
    {
        public ActionResult Plan_Search()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            PurchasePlan_Filter MF = new PurchasePlan_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Month = Request["Month"] == null ? DTList.NextMonth().SD.ToString("yyyy-MM") : Request["Month"].Trim();
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();
            MF.Line_Status = PurchasePlan_Line_Status.已下达计划.ToString();
            PurchasePlan Po_Plan = IPur.Get_PurchasePlan_By_Month(MF);
            ViewData["MF"] = MF;
            return View(Po_Plan);
        }
    }
}