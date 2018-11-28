using System;
using System.Collections.Generic;
using System.Linq;
using SMART.Api.Models;
using System.Threading;

namespace SMART.Api
{
    public partial interface IPurchaseService
    {
        PurchasePlan_Group Get_PurchasePlan_Group(Guid MainCID);

        PurchasePlan Get_PurchasePlan_By_Month(PurchasePlan_Filter MF);
        PurchasePlan Get_PurchasePlan_NextMonth_Detail(PurchasePlan_Filter MF);

        PurchasePlan_Line Get_PurchasePlan_Line_Item(Guid MatID, Guid LinkMainCID, string Month);

        void Set_PurchasePlan_Line(PurchasePlan_Line PlanLine, Guid MatID, Guid MainCID, string Month);
        void Po_Plan_Is_Sent_Buyer(List<PurchasePlan_Line> PlanLineList, string Month, Guid LinkMainCID);

        PurchasePlan Get_Po_Head_Plan_For_Po_Create(PurchasePlan_Filter MF);
        Po_Head Get_Po_Head_By_PoPlan(List<PurchasePlan_Line> PlanLineList, Guid SupID, Guid UID, string Month);
        Guid Create_Po_Head(List<Po_Line> PoLineList, Guid SupID, Guid UID, string Month);

        PageList<Po_Head> Get_Po_Head_PageList(Po_Head_Filter MF);
        Po_Head Get_Po_Head_Item(Guid POID);

        void Po_To_Supplier_Confirm(Guid POID, DateTime Create_DT);
        void Po_To_Supplier_Confirm_Check(Guid POID);
        void Delete_Po_Head(Guid POID);
        void Set_Po_Line_Item(Guid POLID, Po_Line Line);
    }

    public partial interface IPurchaseService
    {
        PageList<Po_Wait_Lading_Group> Get_Po_Wait_Lading_Group(Po_Head_Filter MF);
    }

    public partial class PurchaseService : IPurchaseService
    {
        SmartdbContext db = new SmartdbContext();
    }

    //计划编制、计划下达、计划撤回
    public partial class PurchaseService : IPurchaseService
    {
        public PurchasePlan_Group Get_PurchasePlan_Group(Guid MainCID)
        {
            string Next_Month = DTList.NextMonth().SD.ToString("yyyy-MM");
            PurchasePlan_Group PG = new PurchasePlan_Group();
            PG.MainCID = MainCID;
            PG.Month = Next_Month;

            List<SalesPlan> SPlanList = db.SalesPlan.Where(x => x.LinkMainCID == PG.MainCID && x.Month == PG.Month && x.Status == SalesPlan_Status.已核准.ToString()).ToList();
            List<Guid> SPIDList = SPlanList.Select(x => x.SPID).ToList();
            List<SalesPlan_Line> SPlanLineList = db.SalesPlan_Line.Where(x => SPIDList.Contains(x.Link_SPID) && x.IsDeclare == 1).ToList();
            foreach(var x in SPlanList)
            {
                x.Line_Count = SPlanLineList.Where(c => c.Link_SPID == x.SPID).Count();
            }

            PG.Mat_Count = SPlanLineList.Select(x => x.MatID).Distinct().Count();
            PG.Mat_Count_Is_Sent = db.PurchasePlan_Line.Where(x => x.LinkMainCID == MainCID && x.Month == PG.Month && x.Is_Sent_Buyer == 1).Count();
            PG.Mat_Count_Is_Not_Sent = PG.Mat_Count - PG.Mat_Count_Is_Sent;
            PG.SalesPlanList = SPlanList;
            return PG;
        }

        public PurchasePlan Get_PurchasePlan_NextMonth_Detail(PurchasePlan_Filter MF)
        {
            string Next_Month = DTList.NextMonth().SD.ToString("yyyy-MM");
            MF.Month = Next_Month;

            PurchasePlan Plan = this.Get_PurchasePlan_By_Month(MF);
            return Plan;
        }

        public PurchasePlan Get_PurchasePlan_By_Month(PurchasePlan_Filter MF)
        {
            string Next_Month = MF.Month;
            List<DTSD> MonNextList = DTList.MonthsByNextList(Next_Month);
            PurchasePlan PO_Plan = new PurchasePlan();
            PO_Plan.MainCID = MF.LinkMainCID;
            PO_Plan.Month = Next_Month;


            DTSD Next_DTSD_A = MonNextList.Skip(0).Take(1).FirstOrDefault();
            DTSD Next_DTSD_B = MonNextList.Skip(1).Take(1).FirstOrDefault();
            DTSD Next_DTSD_C = MonNextList.Skip(2).Take(1).FirstOrDefault();

            PO_Plan.Next_Mon_A = Next_DTSD_A.SD.ToString("yyyy-MM");
            PO_Plan.Next_Mon_B = Next_DTSD_B.SD.ToString("yyyy-MM");
            PO_Plan.Next_Mon_C = Next_DTSD_C.SD.ToString("yyyy-MM");

            MF.Month = PO_Plan.Month;
            List<PurchasePlan_Line> ALL_Line_List = this.Get_PurchasePlan_Line_List_By_Month(MF);
            MF.BrandList = ALL_Line_List.Select(x => x.MatInfo.MatBrand).Distinct().ToList();
            MF.BrandList = MF.BrandList.Where(x => x != string.Empty).ToList();


            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                ALL_Line_List = ALL_Line_List.Where(x => x.MatInfo.MatBrand == MF.MatBrand).ToList();
            }

            if (MF.Line_Status == PurchasePlan_Line_Status.已下达计划.ToString())
            {
                ALL_Line_List = ALL_Line_List.Where(x => x.Is_Sent_Buyer == 1).ToList();
            }
            else if (MF.Line_Status == PurchasePlan_Line_Status.待下达计划.ToString())
            {
                ALL_Line_List = ALL_Line_List.Where(x => x.Is_Sent_Buyer == 0).ToList();
            }

            PageList<PurchasePlan_Line> PList = new PageList<PurchasePlan_Line>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = ALL_Line_List.Count();
            PList.Rows = ALL_Line_List.OrderBy(x => x.MatInfo.MatSn).ThenBy(s => s.MatID).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PO_Plan.PurchasePlan_Line_PageList = PList;
            return PO_Plan;
        }

        public List<PurchasePlan_Line> Get_PurchasePlan_Line_List_By_Month(PurchasePlan_Filter MF)
        {
            List<DTSD> MonNextList = DTList.MonthsByNextList(MF.Month);
            PurchasePlan PO_Plan = new PurchasePlan();
            PO_Plan.MainCID = MF.LinkMainCID;
            PO_Plan.Month = MF.Month;

            DTSD Next_DTSD_A = MonNextList.Skip(0).Take(1).FirstOrDefault();
            DTSD Next_DTSD_B = MonNextList.Skip(1).Take(1).FirstOrDefault();
            DTSD Next_DTSD_C = MonNextList.Skip(2).Take(1).FirstOrDefault();

            PO_Plan.Next_Mon_A = Next_DTSD_A.SD.ToString("yyyy-MM");
            PO_Plan.Next_Mon_B = Next_DTSD_B.SD.ToString("yyyy-MM");
            PO_Plan.Next_Mon_C = Next_DTSD_C.SD.ToString("yyyy-MM");

            //获取所有已核准销售预测
            List<SalesPlan> SPlan = db.SalesPlan.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Month == PO_Plan.Month && x.Status == SalesPlan_Status.已核准.ToString()).ToList();
            List<Guid> SPIDList = SPlan.Select(x => x.SPID).ToList();
            List<SalesPlan_Line> SalesPlanLineList = new List<SalesPlan_Line>();
            if (MF.MatID != Guid.Empty)
            {
                SalesPlanLineList = db.SalesPlan_Line.Where(x => SPIDList.Contains(x.Link_SPID) && x.IsDeclare == 1 && x.MatID == MF.MatID).ToList();
            }
            else
            {
                SalesPlanLineList = db.SalesPlan_Line.Where(x => SPIDList.Contains(x.Link_SPID) && x.IsDeclare == 1).ToList();
            }

            //获取所有实体采购计划
            List<PurchasePlan_Line> PlanLineALL_DB = new List<PurchasePlan_Line>();
            if (MF.MatID != Guid.Empty)
            {
                PlanLineALL_DB = db.PurchasePlan_Line.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Month == PO_Plan.Month && x.MatID == MF.MatID).ToList();
            }
            else
            {
                PlanLineALL_DB = db.PurchasePlan_Line.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Month == PO_Plan.Month).ToList();
            }


            //拼接所有计划
            List<Guid> MatID_ALL_List = new List<Guid>();
            MatID_ALL_List.AddRange(PlanLineALL_DB.Select(x => x.MatID).ToList());
            MatID_ALL_List.AddRange(SalesPlanLineList.Select(x => x.MatID).ToList());
            MatID_ALL_List = MatID_ALL_List.Distinct().ToList();

            List<Material> MatList = db.Material.Where(x => x.LinkMainCID == MF.LinkMainCID && MatID_ALL_List.Contains(x.MatID)).ToList();
            Material Mat = new Material();

            //List<WMS_Stock> WmsList = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID && MatID_ALL_List.Contains(x.MatID)).ToList();
            //WMS_Stock WS = new WMS_Stock();

            //三个月跨度的采购订单
            List<Po_Line> Po_Line_List = db.Po_Line.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Delivery_DT >= Next_DTSD_A.SD && x.Delivery_DT <= Next_DTSD_C.ED && MatID_ALL_List.Contains(x.MatID)).ToList();

            List<PurchasePlan_Line> PlanLineALL = new List<PurchasePlan_Line>();
            PurchasePlan_Line PlanLine = new PurchasePlan_Line();
            foreach (var MatID in MatID_ALL_List)
            {
                PlanLine = PlanLineALL_DB.Where(c => c.MatID == MatID).FirstOrDefault();
                PlanLine = PlanLine == null ? new PurchasePlan_Line() : PlanLine;
                PlanLine.Month = PO_Plan.Month;
                PlanLine.Next_Mon_A = PO_Plan.Next_Mon_A;
                PlanLine.Next_Mon_B = PO_Plan.Next_Mon_B;
                PlanLine.Next_Mon_C = PO_Plan.Next_Mon_C;
                PlanLine.LinkMainCID = PO_Plan.MainCID;

                PlanLine.MatID = MatID;
                Mat = MatList.Where(c => c.MatID == PlanLine.MatID).FirstOrDefault();
                PlanLine.MatInfo = Mat == null ? new Material() : Mat;

                //WS = WmsList.Where(c => c.MatID == PlanLine.MatID).FirstOrDefault();
                //WS = WS == null ? new WMS_Stock() : WS;
                //PlanLine.Wms_Qty = WS.Quantity;

                //预测需求 - 已核准 - 汇总
                PlanLine.Next_Mon_Qty_A = SalesPlanLineList.Where(c => c.MatID == PlanLine.MatID).Sum(c => c.Next_Mon_Qty_A);
                PlanLine.Next_Mon_Qty_B = SalesPlanLineList.Where(c => c.MatID == PlanLine.MatID).Sum(c => c.Next_Mon_Qty_B);
                PlanLine.Next_Mon_Qty_C = SalesPlanLineList.Where(c => c.MatID == PlanLine.MatID).Sum(c => c.Next_Mon_Qty_C);

                //在库可用
                PlanLine.Wms_Qty_A = PlanLine.Wms_Qty;
                PlanLine.Wms_Qty_B = PlanLine.Wms_Qty - PlanLine.Next_Mon_Qty_A;
                PlanLine.Wms_Qty_B = PlanLine.Wms_Qty_B <= 0 ? 0 : PlanLine.Wms_Qty_B;

                //PlanLine.Wms_Qty_C = WS.Quantity - PlanLine.Next_Mon_Qty_A - PlanLine.Next_Mon_Qty_B;
                PlanLine.Wms_Qty_C = PlanLine.Wms_Qty_C <= 0 ? 0 : PlanLine.Wms_Qty_C;

                //在途可用
                PlanLine.PoLine_Qty_A = Po_Line_List.Where(c => c.Delivery_DT >= Next_DTSD_A.SD && c.Delivery_DT <= Next_DTSD_A.ED).Sum(x => x.Qty);
                PlanLine.PoLine_Qty_B = Po_Line_List.Where(c => c.Delivery_DT >= Next_DTSD_B.SD && c.Delivery_DT <= Next_DTSD_B.ED).Sum(x => x.Qty);
                PlanLine.PoLine_Qty_C = Po_Line_List.Where(c => c.Delivery_DT >= Next_DTSD_C.SD && c.Delivery_DT <= Next_DTSD_C.ED).Sum(x => x.Qty);

                //计划采购
                PlanLine.Auto_PoPlan_Next_Mon_Qty_A = PlanLine.Next_Mon_Qty_A - (PlanLine.Wms_Qty_A + PlanLine.PoLine_Qty_A);
                PlanLine.Auto_PoPlan_Next_Mon_Qty_B = PlanLine.Next_Mon_Qty_B - (PlanLine.Wms_Qty_B + PlanLine.PoLine_Qty_B);
                PlanLine.Auto_PoPlan_Next_Mon_Qty_C = PlanLine.Next_Mon_Qty_C - (PlanLine.Wms_Qty_C + PlanLine.PoLine_Qty_C);

                PlanLine.Auto_PoPlan_Next_Mon_Qty_A = PlanLine.Auto_PoPlan_Next_Mon_Qty_A <= 0 ? 0 : PlanLine.Auto_PoPlan_Next_Mon_Qty_A;
                PlanLine.Auto_PoPlan_Next_Mon_Qty_B = PlanLine.Auto_PoPlan_Next_Mon_Qty_B <= 0 ? 0 : PlanLine.Auto_PoPlan_Next_Mon_Qty_B;
                PlanLine.Auto_PoPlan_Next_Mon_Qty_C = PlanLine.Auto_PoPlan_Next_Mon_Qty_C <= 0 ? 0 : PlanLine.Auto_PoPlan_Next_Mon_Qty_C;

                if(PlanLine.PPID == Guid.Empty)
                {
                    PlanLine.PoPlan_Next_Mon_Qty_A = PlanLine.Auto_PoPlan_Next_Mon_Qty_A;
                    PlanLine.PoPlan_Next_Mon_Qty_B = PlanLine.Auto_PoPlan_Next_Mon_Qty_B;
                    PlanLine.PoPlan_Next_Mon_Qty_C = PlanLine.Auto_PoPlan_Next_Mon_Qty_C;
                }

                //建议采购日期
                PlanLine.PoPlan_Next_Mon_Delivery_A = PlanLine.Next_Mon_A + "-01";
                PlanLine.PoPlan_Next_Mon_Delivery_B = PlanLine.Next_Mon_B + "-01";
                PlanLine.PoPlan_Next_Mon_Delivery_C = PlanLine.Next_Mon_C + "-01";

                PlanLineALL.Add(PlanLine);
            }

            return PlanLineALL;
        }

        public PurchasePlan_Line Get_PurchasePlan_Line_Item(Guid MatID, Guid LinkMainCID, string Month)
        {
            PurchasePlan_Filter MF = new PurchasePlan_Filter();
            MF.MatID = MatID;
            MF.Month = Month;
            MF.LinkMainCID = LinkMainCID;
            PurchasePlan_Line Line = this.Get_PurchasePlan_Line_List_By_Month(MF).FirstOrDefault();
            Line = Line == null ? new PurchasePlan_Line() : Line;
            return Line;
        }

        public void Set_PurchasePlan_Line(PurchasePlan_Line PlanLine,  Guid MatID, Guid MainCID, string Month)
        {
            if(MatID ==  Guid.Empty)
            {
                throw new Exception("MatID Is Empty");
            }

            if (MainCID == Guid.Empty)
            {
                throw new Exception("MainCID Is Empty");
            }

            if (string.IsNullOrEmpty(Month))
            {
                throw new Exception("Month Is Empty");
            }

            PlanLine.PoPlan_Next_Mon_Qty_A = PlanLine.PoPlan_Next_Mon_Qty_A <= 0 ? 0 : PlanLine.PoPlan_Next_Mon_Qty_A;
            PlanLine.PoPlan_Next_Mon_Qty_B = PlanLine.PoPlan_Next_Mon_Qty_B <= 0 ? 0 : PlanLine.PoPlan_Next_Mon_Qty_B;
            PlanLine.PoPlan_Next_Mon_Qty_C = PlanLine.PoPlan_Next_Mon_Qty_C <= 0 ? 0 : PlanLine.PoPlan_Next_Mon_Qty_C;
            PurchasePlan_Line OLD_Line = db.PurchasePlan_Line.Where(x => x.MatID == MatID && x.LinkMainCID == MainCID && x.Month == Month).FirstOrDefault();

            if (OLD_Line == null)
            {
                OLD_Line = new PurchasePlan_Line();
                OLD_Line.PPID = MyGUID.NewGUID();
                OLD_Line.Month = Month;
                OLD_Line.MatID = MatID;
                OLD_Line.PoPlan_Next_Mon_Qty_A = PlanLine.PoPlan_Next_Mon_Qty_A;
                OLD_Line.PoPlan_Next_Mon_Qty_B = PlanLine.PoPlan_Next_Mon_Qty_B;
                OLD_Line.PoPlan_Next_Mon_Qty_C = PlanLine.PoPlan_Next_Mon_Qty_C;
                OLD_Line.LinkMainCID = MainCID;
                db.PurchasePlan_Line.Add(OLD_Line);
                MyDbSave.SaveChange(db);
            }
            else
            {
                OLD_Line.PoPlan_Next_Mon_Qty_A = PlanLine.PoPlan_Next_Mon_Qty_A;
                OLD_Line.PoPlan_Next_Mon_Qty_B = PlanLine.PoPlan_Next_Mon_Qty_B;
                OLD_Line.PoPlan_Next_Mon_Qty_C = PlanLine.PoPlan_Next_Mon_Qty_C;
                MyDbSave.SaveChange(db);
            }
        }

        /// <summary>
        /// 采购计划下达至采购员，待采购清单
        /// </summary>
        public void Po_Plan_Is_Sent_Buyer(List<PurchasePlan_Line> PlanLineList, string Month, Guid LinkMainCID)
        {
            //过滤已创建采购，MatID
            List<Guid> MatIDList_PoLine = db.Po_Line.Where(x => x.Month == Month && x.LinkMainCID == LinkMainCID).Select(x => x.MatID).ToList();

            List<PurchasePlan_Line> OLD_PoPlan_Line = db.PurchasePlan_Line.Where(x => x.LinkMainCID == LinkMainCID && x.Month == Month && MatIDList_PoLine.Contains(x.MatID) == false).ToList();
            PurchasePlan_Line New_Line = new PurchasePlan_Line();
            foreach (var x in OLD_PoPlan_Line)
            {
                New_Line = PlanLineList.Where(c => c.MatID == x.MatID).FirstOrDefault();
                if(New_Line != null)
                {
                    x.PoPlan_Next_Mon_Qty_A = New_Line.PoPlan_Next_Mon_Qty_A;
                    x.PoPlan_Next_Mon_Qty_B = New_Line.PoPlan_Next_Mon_Qty_B;
                    x.PoPlan_Next_Mon_Qty_C = New_Line.PoPlan_Next_Mon_Qty_C;
                    x.Is_Sent_Buyer = 1;
                }
            }

            //新增剩余项
            List<PurchasePlan_Line> New_Line_Add_List = new List<PurchasePlan_Line>();
            PurchasePlan_Line New_Line_Add = new PurchasePlan_Line();
            List<Guid> OLD_MatID_List = OLD_PoPlan_Line.Select(x => x.MatID).ToList();
            foreach(var x in PlanLineList.Where(c=> MatIDList_PoLine.Contains(c.MatID) == false).ToList())
            {
                if(OLD_MatID_List.Where(c=>c == x.MatID).Any() == false)
                {
                    New_Line_Add = new PurchasePlan_Line();
                    New_Line_Add.PPID = MyGUID.NewGUID();
                    New_Line_Add.Month = Month;
                    New_Line_Add.MatID = x.MatID;
                    New_Line_Add.PoPlan_Next_Mon_Qty_A = x.PoPlan_Next_Mon_Qty_A;
                    New_Line_Add.PoPlan_Next_Mon_Qty_B = x.PoPlan_Next_Mon_Qty_B;
                    New_Line_Add.PoPlan_Next_Mon_Qty_C = x.PoPlan_Next_Mon_Qty_C;
                    New_Line_Add.LinkMainCID = LinkMainCID;
                    New_Line_Add.Is_Sent_Buyer = 1;
                    New_Line_Add_List.Add(New_Line_Add);
                }
            }


            //新增计划信息
            if (New_Line_Add_List.Any())
            {
                db.PurchasePlan_Line.AddRange(New_Line_Add_List);
            }
            MyDbSave.SaveChange(db);
        }

       
    }

    //采购下单
    public partial class PurchaseService : IPurchaseService
    {
        public PurchasePlan Get_Po_Head_Plan_For_Po_Create(PurchasePlan_Filter MF)
        {
            string Next_Month = DTList.NextMonth().SD.ToString("yyyy-MM");
            List<DTSD> MonNextList = DTList.MonthsByNextList(Next_Month);
            PurchasePlan PO_Plan = new PurchasePlan();
            PO_Plan.MainCID = MF.LinkMainCID;
            PO_Plan.Month = Next_Month;


            DTSD Next_DTSD_A = MonNextList.Skip(0).Take(1).FirstOrDefault();
            DTSD Next_DTSD_B = MonNextList.Skip(1).Take(1).FirstOrDefault();
            DTSD Next_DTSD_C = MonNextList.Skip(2).Take(1).FirstOrDefault();

            PO_Plan.Next_Mon_A = Next_DTSD_A.SD.ToString("yyyy-MM");
            PO_Plan.Next_Mon_B = Next_DTSD_B.SD.ToString("yyyy-MM");
            PO_Plan.Next_Mon_C = Next_DTSD_C.SD.ToString("yyyy-MM");

            MF.Month = PO_Plan.Month;
            List<PurchasePlan_Line> ALL_Line_List = this.Get_PurchasePlan_Line_List_By_Month(MF).Where(x=>x.Is_Sent_Buyer == 1).ToList();
            ALL_Line_List = ALL_Line_List.Where(x => x.PoPlan_Next_Mon_Qty_A > 0 || x.PoPlan_Next_Mon_Qty_B > 0 || x.PoPlan_Next_Mon_Qty_C > 0).ToList();

            List<Po_Line> PoLineList = db.Po_Line.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Month == PO_Plan.Month).ToList();
            List<Guid> MatID_By_PoLine = PoLineList.Select(x => x.MatID).ToList();
            ALL_Line_List = ALL_Line_List.Where(x => MatID_By_PoLine.Contains(x.MatID) == false).ToList();

            MF.BrandList = ALL_Line_List.Select(x => x.MatInfo.MatBrand).Distinct().ToList();
            MF.BrandList = MF.BrandList.Where(x => x != string.Empty).ToList();

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                ALL_Line_List = ALL_Line_List.Where(x => x.MatInfo.MatBrand == MF.MatBrand).ToList();
            }


            if (MF.Line_Status == PurchasePlan_Line_Status.已下达计划.ToString())
            {
                ALL_Line_List = ALL_Line_List.Where(x => x.Is_Sent_Buyer == 1).ToList();
            }
            else if (MF.Line_Status == PurchasePlan_Line_Status.待下达计划.ToString())
            {
                ALL_Line_List = ALL_Line_List.Where(x => x.Is_Sent_Buyer == 0).ToList();
            }

            PageList<PurchasePlan_Line> PList = new PageList<PurchasePlan_Line>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = ALL_Line_List.Count();
            PList.Rows = ALL_Line_List.OrderBy(x => x.MatInfo.MatSn).ThenBy(s => s.MatID).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PO_Plan.PurchasePlan_Line_PageList = PList;
            return PO_Plan;

        }

        public Po_Head Get_Po_Head_By_PoPlan(List<PurchasePlan_Line> PlanLineList, Guid SupID, Guid UID, string Month)
        {
            Supplier Sup = db.Supplier.Find(SupID);
            Sup = Sup == null ? new Supplier() : Sup;

            MainCompany MC = db.MainCompany.Find(Sup.LinkMainCID);
            MC = MC == null ? new MainCompany() : MC;

            User U = db.User.Find(UID);
            U = U == null ? new User() : U;

            Po_Head Head = new Po_Head();
            Head.Month = Month;
            Head.Buyer_UID = U.UID;
            Head.Buyer = U.UserFullName;
            Head.Buyer_Tel = U.Tel == null? string.Empty: U.Tel;
            Head.Buyer_Phone = U.MobilePhone == null ? string.Empty : U.MobilePhone;
            Head.Buyer_Email = U.Email == null ? string.Empty : U.Email;

            Head.SupID = Sup.SupID;
            Head.Sup_Name = Sup.Sup_Name;
            Head.Sup_Code = Sup.SupplierCode == null ? string.Empty : Sup.SupplierCode;
            Head.Sup_Address = Sup.Address;

            Head.Main_ComName = MC.MainCompanyName;
            Head.Main_Address = MC.MainAddress == null ? string.Empty : MC.MainAddress;
            Head.Main_MainInvoiceTel = MC.MainTel == null ? string.Empty : MC.MainTel;
            Head.Main_InvoiceAddress = MC.MainInvoiceAddress == null ? string.Empty : MC.MainInvoiceAddress;
            Head.Main_TaxpayerIdentificationNo = MC.TaxpayerIdentificationNo == null ? string.Empty : MC.TaxpayerIdentificationNo;
            Head.Main_MainBankInfo = MC.MainBankInfo == null ? string.Empty : MC.MainBankInfo;
            Head.Main_MainBankAccount = MC.MainBankAccount == null ? string.Empty : MC.MainBankAccount;
            Head.Main_ElectronicStamp = MC.ElectronicStamp == null ? string.Empty : MC.ElectronicStamp;
            Head.Main_ComLogo = MC.ComLogo == null ? string.Empty : MC.ComLogo;

            Head.Ship_To_Address = MC.MainAddress == null ? string.Empty : MC.MainAddress;

            //Head.Sup_Person = Sup_Person.Name == null ? string.Empty : Sup_Person.Name;
            //Head.Sup_Tel = Sup_Person.Tel == null ? string.Empty : Sup_Person.Tel;
            //Head.Sup_Phone = Sup_Person.Mobile_Phone == null ? string.Empty : Sup_Person.Mobile_Phone;
            //Head.Sup_Email = Sup_Person.Email == null ? string.Empty : Sup_Person.Email;


            List<Guid> MatIDList = PlanLineList.Select(x => x.MatID).ToList();
            List<Material> MatList = db.Material.Where(x => MatIDList.Contains(x.MatID)).ToList();
            Material Mat = new Material();

            Head.Po_Line_List = new List<Po_Line>();
            Po_Line Line = new Po_Line();
            int i = 0;
            foreach (var x in PlanLineList)
            {
                Mat = MatList.Where(c => c.MatID == x.MatID).FirstOrDefault();
                Mat = Mat == null ? new Material() : Mat;

                if (x.PoPlan_Next_Mon_Qty_A > 0)
                {
                    i++;
                    Line = new Po_Line();
                    Line.POLID = MyGUID.NewGUID();
                    Line.Line_Number = i;
                    Line.MatID = x.MatID;
                    Line.MatInfo = Mat;
                    Line.Qty = x.PoPlan_Next_Mon_Qty_A;
                    Line.Delivery_DT = Convert.ToDateTime(x.PoPlan_Next_Mon_Delivery_A);
                    Line.CostPrice = 100;
                    Head.Po_Line_List.Add(Line);
                }

                if (x.PoPlan_Next_Mon_Qty_B > 0)
                {
                    i++;
                    Line = new Po_Line();
                    Line.POLID = MyGUID.NewGUID();
                    Line.Line_Number = i;
                    Line.MatID = x.MatID;
                    Line.MatInfo = Mat;
                    Line.Qty = x.PoPlan_Next_Mon_Qty_B;
                    Line.Delivery_DT = Convert.ToDateTime(x.PoPlan_Next_Mon_Delivery_B);
                    Line.CostPrice = 100;
                    Head.Po_Line_List.Add(Line);
                }

                if (x.PoPlan_Next_Mon_Qty_C > 0)
                {
                    i++;
                    Line = new Po_Line();
                    Line.POLID = MyGUID.NewGUID();
                    Line.Line_Number = i;
                    Line.MatID = x.MatID;
                    Line.MatInfo = Mat;
                    Line.Qty = x.PoPlan_Next_Mon_Qty_C;
                    Line.Delivery_DT = Convert.ToDateTime(x.PoPlan_Next_Mon_Delivery_C);
                    Line.CostPrice = 100;
                    Head.Po_Line_List.Add(Line);
                }
            }

            Head.Total_Amount = Head.Po_Line_List.Sum(x => x.CostPrice * x.Qty);
            return Head;
        }

        public Guid Create_Po_Head(List<Po_Line> PoLineList, Guid SupID, Guid UID, string Month)
        {
            Supplier Sup = db.Supplier.Find(SupID);
            Sup = Sup == null ? new Supplier() : Sup;

            MainCompany MC = db.MainCompany.Find(Sup.LinkMainCID);
            MC = MC == null ? new MainCompany() : MC;

            User U = db.User.Find(UID);
            U = U == null ? new User() : U;

            if(Sup.LinkMainCID == Guid.Empty)
            {
                throw new Exception("未选择供应商");
            }

            if (MC.MainCID == Guid.Empty)
            {
                throw new Exception("MainCompany Is Null");
            }

            if (U.LinkMainCID == Guid.Empty)
            {
                throw new Exception("User Is Null");
            }

            if (MC.MainCID != Sup.LinkMainCID || MC.MainCID != U.LinkMainCID)
            {
                throw new Exception("未知错误");
            }

            Po_Head Head = new Po_Head();
            Head.POID = MyGUID.NewGUID();
            Head.Create_DT = DateTime.Now;
            Head.Month = Month;
            Head.Po_No = this.Auto_Get_Po_No();
            Head.Buyer_UID = U.UID;
            Head.Buyer = U.UserFullName;
            Head.Buyer_Tel = U.Tel == null ? string.Empty : U.Tel;
            Head.Buyer_Phone = U.MobilePhone == null ? string.Empty : U.MobilePhone;
            Head.Buyer_Email = U.Email == null ? string.Empty : U.Email;

            Head.SupID = Sup.SupID;
            Head.Sup_Name = Sup.Sup_Name;
            Head.Sup_Code = Sup.SupplierCode == null ? string.Empty : Sup.SupplierCode;
            Head.Sup_Address = Sup.Address == null? string.Empty: Sup.Address;

            Head.Main_ComName = MC.MainCompanyName;
            Head.Main_Address = MC.MainAddress == null ? string.Empty : MC.MainAddress;
            Head.Main_MainInvoiceTel = MC.MainTel == null ? string.Empty : MC.MainTel;
            Head.Main_InvoiceAddress = MC.MainInvoiceAddress == null ? string.Empty : MC.MainInvoiceAddress;
            Head.Main_TaxpayerIdentificationNo = MC.TaxpayerIdentificationNo == null ? string.Empty : MC.TaxpayerIdentificationNo;
            Head.Main_MainBankInfo = MC.MainBankInfo == null ? string.Empty : MC.MainBankInfo;
            Head.Main_MainBankAccount = MC.MainBankAccount == null ? string.Empty : MC.MainBankAccount;
            Head.Main_ElectronicStamp = MC.ElectronicStamp == null ? string.Empty : MC.ElectronicStamp;
            Head.Main_ComLogo = MC.ComLogo == null ? string.Empty : MC.ComLogo;

            Head.Ship_To_Address = MC.MainAddress == null ? string.Empty : MC.MainAddress;

            Head.Status = Po_Status_Enum.待发送供应商.ToString();
            Head.LinkMainCID = MC.MainCID;

            //Head.Sup_Person = Sup_Person.Name == null ? string.Empty : Sup_Person.Name;
            //Head.Sup_Tel = Sup_Person.Tel == null ? string.Empty : Sup_Person.Tel;
            //Head.Sup_Phone = Sup_Person.Mobile_Phone == null ? string.Empty : Sup_Person.Mobile_Phone;
            //Head.Sup_Email = Sup_Person.Email == null ? string.Empty : Sup_Person.Email;


            List<Guid> MatIDList = PoLineList.Select(x => x.MatID).ToList();
            List<Material> MatList = db.Material.Where(x => MatIDList.Contains(x.MatID)).ToList();
            Material Mat = new Material();

            List<Po_Line> Po_Line_List_DB = new List<Po_Line>();
            Po_Line Line = new Po_Line();
            int i = 0;
            foreach (var x in PoLineList.Where(x=>x.Qty > 0).ToList())
            {
                Mat = MatList.Where(c => c.MatID == x.MatID).FirstOrDefault();
                Mat = Mat == null ? new Material() : Mat;

                i++;
                Line = new Po_Line();
                Line.POLID = MyGUID.NewGUID();
                Line.Create_DT = Head.Create_DT;
                Line.Month = Head.Month;
                Line.Line_Number = i;
                Line.MatID = x.MatID;
                Line.MatInfo = Mat;
                Line.Qty = x.Qty;
                Line.Delivery_DT = x.Delivery_DT;
                Line.CostPrice = 100;
                Line.LinkMainCID = Head.LinkMainCID;
                Line.Link_POID = Head.POID;
                Po_Line_List_DB.Add(Line);
            }

            if(Po_Line_List_DB.Any() == false)
            {
                throw new Exception("未添加产品项");
            }

            db.Po_Head.Add(Head);
            db.Po_Line.AddRange(Po_Line_List_DB);
            MyDbSave.SaveChange(db);
            return Head.POID;
        }

        private string Auto_Get_Po_No()
        {
            //初始化数字编码
            string SysIntNo = string.Empty;

            //最大系统编码号
            IMaxIntService IMax = new MaxIntService();
            MaxInt Max = IMax.GetMaxIntByApp(MaxIntAppName.Po_Head.ToString());

            //数字编码位数设定
            int PadLeftNo = 4;

            //超过预设位数，则截取后n位数字
            string MaxIntStr = Max.MaxNo.ToString();
            if (MaxIntStr.Length >= PadLeftNo)
            {
                //Substring(参数1为左起始位数，参数2为截取几位。)
                SysIntNo = MaxIntStr.Substring((MaxIntStr.Length - PadLeftNo), PadLeftNo);
            }
            else
            {
                //用0填充未满部分字符
                SysIntNo = MaxIntStr.ToString().PadLeft(PadLeftNo, '0');
            }
            string NewSysNo = Max.PreCode + DateTime.Now.ToString("yyyyMM") + SysIntNo;
            return NewSysNo;
        }


    }

    //采购单、发送、确认、付款
    public partial class PurchaseService : IPurchaseService
    {
        public PageList<Po_Head> Get_Po_Head_PageList(Po_Head_Filter MF)
        {
            var query = db.Po_Head.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            if(!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.Po_No.Contains(MF.Keyword) || x.Sup_Name.Contains(MF.Keyword)).AsQueryable();
            }

            if(!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
            }
            else
            {
                query = query.Where(x => x.Status == Po_Status_Enum.供应商已确认.ToString() || x.Status == Po_Status_Enum.待付前金.ToString() || x.Status == Po_Status_Enum.待付后金.ToString()).AsQueryable();
            }


            PageList<Po_Head> PList = new PageList<Po_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            List<Guid> POIDList = PList.Rows.Select(x => x.POID).ToList();
            List<Po_Line> LineList = db.Po_Line.Where(x => POIDList.Contains(x.Link_POID)).ToList();

            foreach (var x in PList.Rows)
            {
                x.Po_Line_List = LineList.Where(c => c.Link_POID == x.POID).ToList();
                x.Total_Amount = x.Po_Line_List.Where(c=>c.Link_POID == x.POID).Sum(c => c.Qty * c.CostPrice);
            }
            return PList;
        }

        public Po_Head Get_Po_Head_Item(Guid POID)
        {
            Po_Head Head = db.Po_Head.Find(POID);
            Head = Head == null ? new Po_Head() : Head;

            MainCompany MC = db.MainCompany.Find(Head.LinkMainCID);
            MC = MC == null ? new MainCompany() : MC;
            Head.Main_ComName = MC.MainCompanyName;
            Head.Main_Address = MC.MainAddress == null ? string.Empty : MC.MainAddress;
            Head.Main_MainInvoiceTel = MC.MainTel == null ? string.Empty : MC.MainTel;
            Head.Main_InvoiceAddress = MC.MainInvoiceAddress == null ? string.Empty : MC.MainInvoiceAddress;
            Head.Main_TaxpayerIdentificationNo = MC.TaxpayerIdentificationNo == null ? string.Empty : MC.TaxpayerIdentificationNo;
            Head.Main_MainBankInfo = MC.MainBankInfo == null ? string.Empty : MC.MainBankInfo;
            Head.Main_MainBankAccount = MC.MainBankAccount == null ? string.Empty : MC.MainBankAccount;
            Head.Main_ElectronicStamp = MC.ElectronicStamp == null ? string.Empty : MC.ElectronicStamp;
            Head.Main_ComLogo = MC.ComLogo == null ? string.Empty : MC.ComLogo;
            Head.Po_Line_List = db.Po_Line.Where(x => x.Link_POID == Head.POID).ToList().OrderBy(x=>x.Line_Number).ToList();
            Head.Total_Amount = Head.Po_Line_List.Sum(x => x.Qty * x.CostPrice);

            List<Guid> MatIDList = Head.Po_Line_List.Select(x => x.MatID).ToList();
            List<Material> MatList = db.Material.Where(x => MatIDList.Contains(x.MatID)).ToList();
            Material Mat = new Material();
            foreach (var x in Head.Po_Line_List)
            {
                Mat = MatList.Where(c => c.MatID == x.MatID).FirstOrDefault();
                Mat = Mat == null ? new Material() : Mat;
                x.MatInfo = Mat;
            }

            return Head;
        }

        public void Po_To_Supplier_Confirm(Guid POID, DateTime Create_DT)
        {
            Po_Head Head = db.Po_Head.Find(POID);
            Head.Create_DT = Create_DT;

            Head.Status = Po_Status_Enum.待供应商确认.ToString();
            MyDbSave.SaveChange(db);
        }

        public void Po_To_Supplier_Confirm_Check(Guid POID)
        {
            Po_Head Head = db.Po_Head.Find(POID);
            Head.Status = Po_Status_Enum.供应商已确认.ToString();
            MyDbSave.SaveChange(db);
        }

        public void Delete_Po_Head(Guid POID)
        {
            Po_Head Head = db.Po_Head.Find(POID);
            db.Po_Head.Remove(Head);

            List<Po_Line> PoLine_List = db.Po_Line.Where(x => x.Link_POID == Head.POID).ToList();
            db.Po_Line.RemoveRange(PoLine_List);

            MyDbSave.SaveChange(db);

        }

        public void Set_Po_Line_Item(Guid POLID, Po_Line Line)
        {
            Po_Line OLD_Line = db.Po_Line.Find(POLID);
            OLD_Line.Qty = Line.Qty;
            OLD_Line.Delivery_DT = Line.Delivery_DT;
            OLD_Line.CostPrice = Line.CostPrice;

            if(OLD_Line.Qty <= 0)
            {
                throw new Exception("数量不能为 0");
            }

            if (OLD_Line.CostPrice < 0)
            {
                throw new Exception("采购单价不能为负数");
            }

            MyDbSave.SaveChange(db);
        }
    }


    //采购单提单录入
    public partial class PurchaseService : IPurchaseService
    {
        public PageList<Po_Wait_Lading_Group> Get_Po_Wait_Lading_Group(Po_Head_Filter MF)
        {
            var query = db.Po_Line.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            //忽略待确认的采购单
            List<Guid> Ignore_POID = db.Po_Head.Where(x => x.Status != Po_Status_Enum.待发送供应商.ToString() && x.Status != Po_Status_Enum.待供应商确认.ToString()).Select(x => x.POID).ToList();
            //query = query.Where(x => Ignore_POID.Contains(x.Link_POID)).AsQueryable();

            List<Po_Line> PoLineList = query.ToList();
            List<Guid> POIDList = PoLineList.Select(x => x.Link_POID).Distinct().ToList();
            List<Po_Head> PoHeadList = db.Po_Head.Where(x => POIDList.Contains(x.POID)).ToList();

            Po_Head Head = new Po_Head();
            foreach (var x in PoLineList)
            {
                Head = PoHeadList.Where(c => c.POID == x.Link_POID).FirstOrDefault();
                Head = Head == null ? new Po_Head() : Head;
                x.Link_SupID = Head.SupID;
                x.Link_Sup_Name = Head.Sup_Name;
            }


            var PoLineGroup = from x in PoLineList
                              group x by new { x.Link_SupID, x.Link_Sup_Name } into G
                              select new
                              {
                                  SupID = G.Key.Link_SupID,
                                  Sup_Name = G.Key.Link_Sup_Name,
                                  SubLineList = G.ToList(),
                              };

            List<Po_Wait_Lading_Group> List = new List<Po_Wait_Lading_Group>();
            Po_Wait_Lading_Group WG = new Po_Wait_Lading_Group();
            foreach (var x in PoLineGroup.ToList())
            {
                WG = new Po_Wait_Lading_Group();
                WG.SupID = x.SupID;
                WG.Sup_Name = x.Sup_Name;
                WG.Po_Head_Count = x.SubLineList.Select(c=>c.Link_POID).Distinct().ToList().Count();
                WG.Po_Line_Count = x.SubLineList.Count();
                WG.Min_Delivery = x.SubLineList.Min(c => c.Delivery_DT);
                List.Add(WG);
            }

            PageList<Po_Wait_Lading_Group> PList = new PageList<Po_Wait_Lading_Group>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = List.OrderBy(x => x.Min_Delivery).ThenBy(x=>x.SupID).ThenBy(x=>x.Sup_Name).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }
    }

}
