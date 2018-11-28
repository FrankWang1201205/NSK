using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;

namespace SMART.EBMS.Controllers
{
    [Authorize]
    public partial class RFQController : Controller
    {
        IUserService IU = new UserService();
        ICustomerService IC = new CustomerService();
        IRFQService IRFQ = new RFQService();

        IMaterialService IMat = new MaterialService();
        IBrandService IBrand = new BrandService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //录入询价单
    public partial class RFQController : Controller
    {
        public ActionResult RFQ_New()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            RFQ_Head_Filter MF = new RFQ_Head_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;

            PageList<RFQ_Head> PList = IRFQ.Get_RFQ_Head_By_Wait_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult RFQ_New_Cust_List()
        {
            User U = this.MyUser();
            Customer_Filter MF = new Customer_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.Sales_UID = Guid.Empty;
            MF.Cust_Name_Or_Code = Request["Cust_Name_Or_Code"] == null ? string.Empty : Request["Cust_Name_Or_Code"].Trim();
            List<Customer> List = IC.Get_Customer_List(MF);
            ViewData["MF"] = MF;
            return PartialView(List);
        }

        public PartialViewResult RFQ_New_Add(Guid ID)
        {
            Guid CID = ID;
            Customer C = IC.Get_Customer_Item(CID);

            RFQ_Head Head = new RFQ_Head();
            Head.CID = C.CID;
            Head.Cust_Name = C.Cust_Name;
            Head.Buyer = C.Buyer;
            Head.Buyer_Tel = C.Buyer_Tel;
            Head.Buyer_Fax = C.Buyer_Fax;
            Head.Buyer_Mail = C.Buyer_Mail;
            Head.Create_DT = DateTime.Now;
            return PartialView(Head);
        }

        public PartialViewResult RFQ_New_Add_Customer()
        {
            User U = this.MyUser();
            Customer C = IC.Get_Customer_Empty(U.LinkMainCID);
            RFQ_Head Head = new RFQ_Head();
            Head.CID = C.CID;
            Head.Cust_Name = C.Cust_Name;
            Head.Buyer = C.Buyer;
            Head.Buyer_Tel = C.Buyer_Tel;
            Head.Buyer_Fax = C.Buyer_Fax;
            Head.Buyer_Mail = C.Buyer_Mail;
            Head.Create_DT = DateTime.Now;
            return PartialView(Head);
        }

        public RedirectToRouteResult RFQ_New_Add_Post(Guid ID, FormCollection FC)
        {
            Guid CID = ID;
            try
            {
                RFQ_Head Head = new RFQ_Head();
                TryUpdateModel<RFQ_Head>(Head, FC);
                Head.CID = ID;
                Head.Ass_UID = this.MyUser().UID;
                Guid RID = IRFQ.Create_RFQ(Head);
                return RedirectToAction("RFQ_New_Update", new { ID = RID });
            }
            catch (Exception Ex)
            {

                TempData["Error_Add_RFQ"] = Ex.Message.ToString();
                return RedirectToAction("RFQ_New");
            }
        }

        public ActionResult RFQ_New_Update(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid RID = ID;
            RFQ_Head Head = IRFQ.Get_RFQ_Head_DB(RID);
            return View(Head);
        }

        public RedirectToRouteResult RFQ_New_Delete_Post(Guid ID)
        {
            Guid RID = ID;
            try
            {
                IRFQ.Delete_RFQ_Item(RID);
                return RedirectToAction("RFQ_New");
            }
            catch (Exception Ex)
            {
                TempData["Error_Delete_RFQ"] = Ex.Message.ToString();
                return RedirectToAction("RFQ_New_Update", new { ID = RID });
            }
        }

        public PartialViewResult RFQ_New_Update_Add_Sub()
        {
            RFQ_Head_Line Line = new RFQ_Head_Line();
            List<Brand> Brand_List = IBrand.Get_Brand_List(this.MyUser().LinkMainCID);
            ViewData["Brand_List"] = Brand_List;
            return PartialView(Line);
        }

        public string RFQ_New_Update_Add_Sub_Post(Guid ID, FormCollection FC)
        {
            string result = string.Empty;
            Guid RID = ID;
            try
            {
                RFQ_Head_Line Line = new RFQ_Head_Line();
                TryUpdateModel<RFQ_Head_Line>(Line, FC);
                Line.Cust_Mat_Sn_INFO_STR = Request["Cust_Mat_Sn_INFO_STR"] == null ? string.Empty : Request["Cust_Mat_Sn_INFO_STR"].Trim();
                IRFQ.Create_RFQ_Line(RID, Line);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public ActionResult RFQ_New_Update_Excel_Template()
        {
            string Path = IRFQ.RFQ_Excel_Template();
            return File(Path, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "报价产品模板.xlsx");

        }

        public RedirectToRouteResult RFQ_New_Update_Excel_Upload(Guid ID)
        {
            Guid RID = ID;
            try
            {
               HttpPostedFileBase ExcelFile = Request.Files["ExcelFile"];
               IRFQ.RFQ_Excel_Template_Upload(ExcelFile,RID);
               Thread.Sleep(1000);
            }
            catch (Exception Ex)
            {
                TempData["Error_Excel"] = Ex.Message.ToString();
            }
            return RedirectToAction("RFQ_New_Update", new { ID = RID });
        }

        public PartialViewResult RFQ_New_Update_List(Guid ID)
        {
            Guid RID = ID;
            List<RFQ_Head_Line> List = IRFQ.Get_RFQ_Line_List(RID);
            return PartialView(List);
        }

        public PartialViewResult RFQ_New_Update_List_Sub(Guid ID)
        {
            Guid Line_ID = ID;
            RFQ_Head_Line Line = IRFQ.Get_RFQ_Line_By_Line_ID(Line_ID);
            return PartialView(Line);
        }

        public string RFQ_New_Update_List_Sub_Post_Delete(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Line_ID = ID;
                IRFQ.Delete_RFQ_Line(Line_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public string RFQ_New_Update_List_Sub_Post(Guid ID, FormCollection FC)
        {
            string result = string.Empty;
            try
            {
                Guid Line_ID = ID;
                RFQ_Head_Line Line = new RFQ_Head_Line();
                TryUpdateModel<RFQ_Head_Line>(Line, FC);
                Line.Cust_Mat_Sn_INFO_STR = Request["Cust_Mat_Sn_INFO_STR"] == null ? string.Empty : Request["Cust_Mat_Sn_INFO_STR"].Trim();
                IRFQ.Set_RFQ_Line_More(Line_ID, Line);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public string RFQ_New_Update_List_Set_Post(Guid ID, FormCollection FC)
        {
            string result = string.Empty;
            try
            {
                Guid Line_ID = ID;
                RFQ_Head_Line Line = new RFQ_Head_Line();
                TryUpdateModel<RFQ_Head_Line>(Line, FC);
                IRFQ.Set_RFQ_Line(Line_ID, Line);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public RedirectToRouteResult RFQ_New_Update_Post(Guid ID, FormCollection FC)
        {
            Guid RID = ID;
            try
            {
                RFQ_Head Head = new RFQ_Head();
                TryUpdateModel<RFQ_Head>(Head, FC);
                IRFQ.Set_RFQ_Head_Base(RID, Head);
                Thread.Sleep(500);
                IRFQ.RFQ_Sent_To_Sales(RID);
                TempData["Success_Update_RFQ"] = "发送成功";
                return RedirectToAction("RFQ_New", new { ID = RID });
            }
            catch (Exception Ex)
            {
                TempData["Error_Update_RFQ"] = Ex.Message.ToString();
                return RedirectToAction("RFQ_New_Update", new { ID = RID });
            }
        }

    }

    //报价处理
    public partial class RFQController : Controller
    {
        public ActionResult RFQ_Set()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            RFQ_Head_Filter MF = new RFQ_Head_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 100;
            MF.LinkMainCID = U.LinkMainCID;

            MF.RFQ_No = Request["RFQ_No"] == null ? string.Empty : Request["RFQ_No"].Trim();
            MF.Cust_Name = Request["Cust_Name"] == null ? string.Empty : Request["Cust_Name"].Trim();
            MF.Status_List.Add(RFQ_Status_Enum.进行中.ToString());

            PageList<RFQ_Head> PList = IRFQ.Get_RFQ_Head_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult RFQ_Set_Update(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid RID = ID;
            RFQ_Head Head = IRFQ.Get_RFQ_Head_Item(RID);
            return View(Head);
        }

        public RedirectToRouteResult RFQ_Set_Update_Post(Guid ID, FormCollection FC)
        {
            Guid RID = ID;
            try
            {
                RFQ_Head Head = new RFQ_Head();
                TryUpdateModel<RFQ_Head>(Head, FC);
                IRFQ.Set_RFQ_Head_Base(RID, Head);
                return RedirectToAction("RFQ_Set_Update", new { ID = RID });
            }
            catch (Exception Ex)
            {
                TempData["Error_Update_RFQ"] = Ex.Message.ToString();
                return RedirectToAction("RFQ_Set_Update", new { ID = RID });
            }
        }

        public RedirectToRouteResult RFQ_Set_Update_Delete_Post(Guid ID)
        {
            Guid RID = ID;
            try
            {
                IRFQ.Delete_RFQ_Item(RID);
                return RedirectToAction("RFQ_Set");
            }
            catch (Exception Ex)
            {
                TempData["Error_Delete_RFQ"] = Ex.Message.ToString();
                return RedirectToAction("RFQ_Set_Update", new { ID = RID });
            }
        }

        public PartialViewResult RFQ_Set_Update_Mat(Guid ID)
        {
            Guid Line_ID = ID;
            RFQ_Head_Line Line = IRFQ.Get_RFQ_Line_By_Line_ID(Line_ID);
            return PartialView(Line);
        }

        public PartialViewResult RFQ_Set_Update_Mat_List(Guid ID)
        {
            User U = this.MyUser();
            Guid BID = ID;

            Material_Filter MF = new Material_Filter();
            MF.LinkMainCID = U.LinkMainCID;
            MF.PageSize = 50;
            MF.MatSn = Request["MatSn_Keyword"] == null ? string.Empty : Request["MatSn_Keyword"].Trim();
            MF.Link_BID = BID;
            List<Material> List = IMat.Get_Material_List_By_MatSn(MF);
            return PartialView(List);
        }

        public string RFQ_Set_Update_Mat_List_Select(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Line_ID = ID;
                Guid MatID = new Guid(Request["MatID"].ToString());
                IRFQ.Set_RFQ_Line_MatID(Line_ID, MatID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public string RFQ_Set_Update_Mat_List_Cancel(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid Line_ID = ID;
                IRFQ.Set_RFQ_Line_MatID_Cancel(Line_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }
}