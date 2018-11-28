using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;
using System.Threading;

namespace SMART.Api
{
    public interface ISalesPlanService
    {
        SalesPlan Get_SalesPlan_NextMonth(SalesPlan_Filter MF);
        SalesPlan Get_SalesPlan_Item(Guid SPID, SalesPlan_Filter MF);

        PageList<SalesPlan> Get_SalesPlan_PageList(SalesPlan_Filter MF);
        void Set_NextMonth_Line(Guid SPID, Guid MatID, SalesPlan_Line New_Line);
        void Set_NextMonth_Line_IsDeclare(List<Guid> MatIDList, Guid SPID);
        void Set_NextMonth_Line_IsDeclare_Cancel(List<Guid> MatIDList, Guid SPID);
        void Set_SalesPlan_To_Pass(Guid SPID, string Aud_Person);
        void Set_SalesPlan_To_Back(Guid SPID);

        void Create_MatSales_Lib(List<Guid> MatIDList, Guid Sales_UID);
        void Remove_MatSales_Lib(List<Guid> MatIDList, Guid Sales_UID);
    }

    public partial class SalesPlanService : ISalesPlanService
    {
        SmartdbContext db = new SmartdbContext();
    }

    //我的产品
    public partial class SalesPlanService : ISalesPlanService
    {
        /// <summary>
        /// 添加我的产品
        /// </summary>
        public void Create_MatSales_Lib(List<Guid> MatIDList, Guid Sales_UID)
        {
            User U = db.User.Find(Sales_UID);
            if (U == null) { throw new Exception("User Is Null"); }

            List<Guid> OLD_MatID_List = db.MatSales_Lib.Where(x => x.Sales_UID == U.UID).Select(x=>x.MatID).ToList();

            List<MatSales_Lib> LibList = new List<MatSales_Lib>();
            MatSales_Lib Lib = new MatSales_Lib();
            DateTime NowDT = DateTime.Now;
            int i = 0;
            foreach(var MatID in MatIDList.Where(x=> OLD_MatID_List.Contains(x) == false).ToList())
            {
                i++;
                Lib = new MatSales_Lib();
                Lib.MSLID = MyGUID.NewGUID();
                Lib.MatID = MatID;
                Lib.Sales_UID = U.UID;
                Lib.LinkMainCID = U.LinkMainCID;
                Lib.Create_DT = NowDT.AddMilliseconds(i);
                LibList.Add(Lib);
            }
            db.MatSales_Lib.AddRange(LibList);
            MyDbSave.SaveChange(db);

        }

        /// <summary>
        /// 撤回我的产品
        /// </summary>
        public void Remove_MatSales_Lib(List<Guid> MatIDList, Guid Sales_UID)
        {
            User U = db.User.Find(Sales_UID);
            if (U == null) { throw new Exception("User Is Null"); }
            List<MatSales_Lib> LibList = db.MatSales_Lib.Where(x => x.LinkMainCID == U.LinkMainCID && x.Sales_UID == Sales_UID && MatIDList.Contains(x.MatID)).ToList();
            if (LibList.Any())
            {
                db.MatSales_Lib.RemoveRange(LibList);
                MyDbSave.SaveChange(db);
            }
        }
    }

    //申报、评审、预测
    public partial class SalesPlanService : ISalesPlanService
    {
        public PageList<SalesPlan> Get_SalesPlan_PageList(SalesPlan_Filter MF)
        {
            var query = db.SalesPlan.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            if(!string.IsNullOrEmpty(MF.Month))
            {
                query = query.Where(x => x.Month == MF.Month).AsQueryable();
            }

            List<Guid> UIDList = query.Select(x => x.Sales_UID).Distinct().ToList();
            List<User> UList = db.User.Where(x => UIDList.Contains(x.UID)).ToList();
            MF.SalesPersonList = UList;

            if (MF.Sales_UID != Guid.Empty)
            {
                query = query.Where(x => x.Sales_UID == MF.Sales_UID).AsQueryable();
            }

            PageList<SalesPlan> PList = new PageList<SalesPlan>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Creata_DT).ThenBy(s => s.SPID).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> SPID_List = PList.Rows.Select(x => x.SPID).ToList();
            List<Guid> Link_SPID_List = db.SalesPlan_Line.Where(x => SPID_List.Contains(x.Link_SPID) && x.IsDeclare == 1).Select(x => x.Link_SPID).ToList();

            foreach(var x in PList.Rows)
            {
                x.Line_Count = Link_SPID_List.Where(c => c == x.SPID).Count();
            }
            return PList;
        }

        public SalesPlan Get_SalesPlan_NextMonth(SalesPlan_Filter MF)
        {
            User U = db.User.Find(MF.Sales_UID);
            U = U == null ? new User() : U;
            string Next_Month = DTList.NextMonth().SD.ToString("yyyy-MM");

            SalesPlan Next_Mon_SP = db.SalesPlan.Where(x => x.LinkMainCID == U.LinkMainCID && x.Sales_UID == U.UID && x.Month == Next_Month).FirstOrDefault();
            Next_Mon_SP = Next_Mon_SP == null ? new SalesPlan() : Next_Mon_SP;
            if(Next_Mon_SP.SPID == Guid.Empty)
            {
                DateTime ThisMon = DTList.ThisMonth().SD;

                Next_Mon_SP.SPID = MyGUID.NewGUID();
                Next_Mon_SP.Creata_DT = DTList.ThisMonth().ED;
                Next_Mon_SP.End_Line_DT = DTList.ThisMonth().ED;
                Next_Mon_SP.Month = Next_Month;
                Next_Mon_SP.Sys_No = this.Auto_Get_Sys_No(DTList.NextMonth().SD.ToString("yyyyMM"));
                Next_Mon_SP.Status = SalesPlan_Status.待评审.ToString();
                Next_Mon_SP.Sales_UID = U.UID;
                Next_Mon_SP.Sales_Person = U.UserFullName;
                Next_Mon_SP.LinkMainCID = U.LinkMainCID;
                db.SalesPlan.Add(Next_Mon_SP);
                MyDbSave.SaveChange(db);
                Thread.Sleep(500);
            }

            return this.Get_SalesPlan_Item(Next_Mon_SP.SPID, MF);
        }

        public SalesPlan Get_SalesPlan_Item(Guid SPID, SalesPlan_Filter MF)
        {
            SalesPlan SP = db.SalesPlan.Find(SPID);
            SP = SP == null ? new SalesPlan() : SP;
            SP.This_Mon_Float = 0;
            SP.Next_Mon_Float_A = 10;
            SP.Next_Mon_Float_B = 25;
            SP.Next_Mon_Float_C = 1000;

            List<DTSD> MonNextList = DTList.MonthsByNextList(SP.Month);

            //下月
            DateTime Next_Mon_A_SD = MonNextList.Skip(0).Take(1).FirstOrDefault().SD;
            DateTime Next_Mon_A_ED = MonNextList.Skip(0).Take(1).FirstOrDefault().ED;

            //下下月
            DateTime Next_Mon_B_SD = MonNextList.Skip(1).Take(1).FirstOrDefault().SD;
            DateTime Next_Mon_B_ED = MonNextList.Skip(1).Take(1).FirstOrDefault().ED;


            //下下下月
            DateTime Next_Mon_C_SD = MonNextList.Skip(2).Take(1).FirstOrDefault().SD;
            DateTime Next_Mon_C_ED = MonNextList.Skip(2).Take(1).FirstOrDefault().ED;

            //本月
            DateTime This_Mon_SD = Convert.ToDateTime(Next_Mon_A_SD.AddMonths(-1).ToString("yyyy-MM-01 00:00:00")); //本月一日
            DateTime This_Mon_ED = Convert.ToDateTime(This_Mon_SD.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59")); //本月最后一日

            //抛出时间序列
            string DT_INFO = string.Empty;
            DT_INFO += This_Mon_SD.ToString("yyyy-MM-dd HH:mm:ss");
            DT_INFO += "|";
            DT_INFO += This_Mon_ED.ToString("yyyy-MM-dd HH:mm:ss");
            DT_INFO += "||";

            DT_INFO += Next_Mon_A_SD.ToString("yyyy-MM-dd HH:mm:ss");
            DT_INFO += "|";
            DT_INFO += Next_Mon_A_ED.ToString("yyyy-MM-dd HH:mm:ss");
            DT_INFO += "||";

            DT_INFO += Next_Mon_B_SD.ToString("yyyy-MM-dd HH:mm:ss");
            DT_INFO += "|";
            DT_INFO += Next_Mon_B_ED.ToString("yyyy-MM-dd HH:mm:ss");
            DT_INFO += "||";

            DT_INFO += Next_Mon_C_SD.ToString("yyyy-MM-dd HH:mm:ss");
            DT_INFO += "|";
            DT_INFO += Next_Mon_C_ED.ToString("yyyy-MM-dd HH:mm:ss");

            //throw new Exception(DT_INFO);
            //抛出时间序列

            SP.This_Mon = This_Mon_SD.ToString("MM") + "月";
            SP.Next_Mon_A = Next_Mon_A_SD.ToString("MM") + "月(±" + SP.Next_Mon_Float_A.ToString() + "%)";
            SP.Next_Mon_B = Next_Mon_B_SD.ToString("MM") + "月(±" + SP.Next_Mon_Float_B.ToString() + "%)";
            SP.Next_Mon_C = Next_Mon_C_SD.ToString("MM") + "月(不限)";

            DateTime NowDT = DateTime.Now;
            TimeSpan TS = NowDT.Subtract(SP.End_Line_DT).Duration();
            int HourCount = Convert.ToInt32(TS.TotalHours);
            int DayCount = Convert.ToInt32(TS.TotalDays);
            SP.End_DT_Int = 0;
            SP.End_DT_Unit = string.Empty;

            string Time_Unit = string.Empty;
            if (HourCount <= 48)
            {
                SP.End_DT_Int = HourCount;
                SP.End_DT_Unit = "小时";
            }
            else
            {
                SP.End_DT_Int = DayCount;
                SP.End_DT_Unit = "天";
            }

            string ThisMon = This_Mon_SD.ToString("yyyy-MM");
            SalesPlan This_Mon_SP = db.SalesPlan.Where(x => x.LinkMainCID == SP.LinkMainCID && x.Sales_UID == SP.Sales_UID && x.Month == ThisMon).FirstOrDefault();
            This_Mon_SP = This_Mon_SP == null ? new SalesPlan() : This_Mon_SP;
            List<SalesPlan_Line> ThisMonLineList = db.SalesPlan_Line.Where(x => x.Link_SPID == This_Mon_SP.SPID).ToList();

            List<SalesPlan_Line> LineList = db.SalesPlan_Line.Where(x => x.Link_SPID == SP.SPID).ToList();
            List<MatSales_Lib> MyMatLib = db.MatSales_Lib.Where(x => x.LinkMainCID == SP.LinkMainCID && x.Sales_UID == SP.Sales_UID).ToList();

            List<Guid> MatIDList = MyMatLib.Select(x => x.MatID).ToList();
            MatIDList.AddRange(LineList.Select(x => x.MatID).ToList());
            MatIDList = MatIDList.Distinct().ToList();

            var queryMat = db.Material.Where(x => x.LinkMainCID == MF.LinkMainCID && MatIDList.Contains(x.MatID)).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                queryMat = queryMat.Where(m => m.MatBrand == MF.MatBrand).AsQueryable();
            }

            List<Material> MatchItemList = queryMat.ToList();

            List<SalesPlan_Line> ALL_Line_List = new List<SalesPlan_Line>();
            SalesPlan_Line Line = new SalesPlan_Line();
            Material Mat = new Material();

            foreach (var MatItem in MatchItemList)
            {
                Line = LineList.Where(c => c.MatID == MatItem.MatID).FirstOrDefault();
                Line = Line == null ? new SalesPlan_Line() : Line;
                Line.This_Mon_Qty = ThisMonLineList.Where(c => c.MatID == MatItem.MatID).Sum(c => c.Next_Mon_Qty_A);
                Line.This_Mon_Qty = 100;

                Line.MatID = MatItem.MatID;
                Line.MatInfo = MatItem;

                if (Line.This_Mon_Qty > 0)
                {
                    Line.Next_Mon_Float_A = Convert.ToInt32((Math.Abs(Line.This_Mon_Qty - Line.Next_Mon_Qty_A) / (decimal)Line.This_Mon_Qty * (decimal)100).ToString("0"));
                    Line.Next_Mon_Float_B = Convert.ToInt32((Math.Abs(Line.This_Mon_Qty - Line.Next_Mon_Qty_B) / (decimal)Line.This_Mon_Qty * (decimal)100).ToString("0"));
                }
                else
                {
                    Line.Next_Mon_Float_A = 0;
                    Line.Next_Mon_Float_B = 0;
                }


                if (Line.Next_Mon_Qty_A > 0)
                {
                    Line.Next_Mon_CSS_A = Line.Next_Mon_Float_A > SP.Next_Mon_Float_A ? "Float_Out_Red" : "Float_Out";
                }

                if (Line.Next_Mon_Qty_B > 0)
                {
                    Line.Next_Mon_CSS_B = Line.Next_Mon_Float_B > SP.Next_Mon_Float_B ? "Float_Out_Red" : "Float_Out";
                }

                ALL_Line_List.Add(Line);
            }

            if (MF.IsDeclare == IsDeclare_Enum.已申报.ToString())
            {
                ALL_Line_List = ALL_Line_List.Where(x => x.IsDeclare == 1).ToList();
            }
            else if (MF.IsDeclare == IsDeclare_Enum.未申报.ToString())
            {
                ALL_Line_List = ALL_Line_List.Where(x => x.IsDeclare == 0).ToList();
            }

            PageList<SalesPlan_Line> PList = new PageList<SalesPlan_Line>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = MatchItemList.Count();
            PList.Rows = ALL_Line_List.OrderBy(x => x.MatInfo.MatSn).ThenBy(s => s.MatID).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            SP.SalesPlan_Line_PageList = PList;
            return SP;
        }

        public void Set_NextMonth_Line(Guid SPID, Guid MatID, SalesPlan_Line New_Line)
        {
            //不允许负数
            New_Line.Next_Mon_Qty_A = New_Line.Next_Mon_Qty_A < 0 ? 0 : New_Line.Next_Mon_Qty_A;
            New_Line.Next_Mon_Qty_B = New_Line.Next_Mon_Qty_B < 0 ? 0 : New_Line.Next_Mon_Qty_B;
            New_Line.Next_Mon_Qty_C = New_Line.Next_Mon_Qty_C < 0 ? 0 : New_Line.Next_Mon_Qty_C;

            SalesPlan SP = db.SalesPlan.Find(SPID);
            SalesPlan_Line Line = db.SalesPlan_Line.Where(x => x.MatID == MatID && x.Link_SPID == SP.SPID).FirstOrDefault();

            if (Line == null)
            {
                Line = new SalesPlan_Line();
                Line.SPLineID = MyGUID.NewGUID();
                Line.Link_SPID = SPID;
                Line.MatID = MatID;
                Line.Next_Mon_Qty_A = New_Line.Next_Mon_Qty_A;
                Line.Next_Mon_Qty_B = New_Line.Next_Mon_Qty_B;
                Line.Next_Mon_Qty_C = New_Line.Next_Mon_Qty_C;
                Line.POLID = Guid.Empty;
                Line.LinkMainCID = SP.LinkMainCID;
                db.SalesPlan_Line.Add(Line);
                MyDbSave.SaveChange(db);
            }
            else
            {
                Line.Next_Mon_Qty_A = New_Line.Next_Mon_Qty_A;
                Line.Next_Mon_Qty_B = New_Line.Next_Mon_Qty_B;
                Line.Next_Mon_Qty_C = New_Line.Next_Mon_Qty_C;
                MyDbSave.SaveChange(db);
            }

        }

        public void Set_NextMonth_Line_IsDeclare(List<Guid> MatIDList, Guid SPID)
        {
            SalesPlan SP = db.SalesPlan.Find(SPID);
            List<SalesPlan_Line> LineList = db.SalesPlan_Line.Where(x => x.Link_SPID == SP.SPID).ToList();
            SalesPlan_Line Line = new SalesPlan_Line();
            List<SalesPlan_Line> New_LineList = new List<SalesPlan_Line>();

            foreach (var MatID in MatIDList)
            {
                Line = LineList.Where(x => x.MatID == MatID).FirstOrDefault();
                if (Line != null)
                {
                    Line.IsDeclare = 1;
                }
                else
                {
                    Line = new SalesPlan_Line();
                    Line.SPLineID = MyGUID.NewGUID();
                    Line.Link_SPID = SP.SPID;
                    Line.MatID = MatID;
                    Line.Next_Mon_Qty_A = 0;
                    Line.Next_Mon_Qty_B = 0;
                    Line.Next_Mon_Qty_C = 0;
                    Line.IsDeclare = 1;
                    Line.POLID = Guid.Empty;
                    Line.LinkMainCID = SP.LinkMainCID;
                    New_LineList.Add(Line);
                }
            }

            //添加剩余项
            if (New_LineList.Any())
            {
                db.SalesPlan_Line.AddRange(New_LineList);
            }
            db.SaveChanges();
        }

        public void Set_NextMonth_Line_IsDeclare_Cancel(List<Guid> MatIDList, Guid SPID)
        {
            List<SalesPlan_Line> LineList = db.SalesPlan_Line.Where(x => x.Link_SPID == SPID).ToList();
            foreach (var x in LineList)
            {
                if (MatIDList.Where(c => c == x.MatID).Any())
                {
                    x.IsDeclare = 0;
                }
            }
            db.SaveChanges();
        }

        public void Set_SalesPlan_To_Pass(Guid SPID, string Aud_Person)
        {
            SalesPlan SP = db.SalesPlan.Find(SPID);

            if (SP.Status == SalesPlan_Status.已核准.ToString())
            {
                throw new Exception("预测计划已核准，请勿重复操作！");
            }

            SP.Status = SalesPlan_Status.已核准.ToString();
            SP.Aud_Person = Aud_Person;
            MyDbSave.SaveChange(db);
        }

        public void Set_SalesPlan_To_Back(Guid SPID)
        {
            SalesPlan SP = db.SalesPlan.Find(SPID);
            List<Guid> MatIDList = db.PurchasePlan_Line.Where(x => x.LinkMainCID == SP.LinkMainCID && x.Month == SP.Month).Select(x => x.MatID).ToList();

            if(MatIDList.Any())
            {
                throw new Exception("已开始计划，无法撤回申报！");
            }


            if (SP.Status == SalesPlan_Status.待评审.ToString())
            {
                throw new Exception("预测计划撤回，请勿重复操作！");
            }
            SP.Status = SalesPlan_Status.待评审.ToString();
            SP.Aud_Person = string.Empty;
            MyDbSave.SaveChange(db);
        }

        private string Auto_Get_Sys_No(string Next_Month)
        {
            //初始化数字编码
            string SysIntNo = string.Empty;

            //最大系统编码号
            IMaxIntService IMax = new MaxIntService();
            MaxInt Max = IMax.GetMaxIntByApp(MaxIntAppName.SalesPlan.ToString());

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
            string NewSysNo = Max.PreCode + Next_Month + SysIntNo;
            return NewSysNo;
        }
    }

  


}
