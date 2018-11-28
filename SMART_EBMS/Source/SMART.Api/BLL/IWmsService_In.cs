using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;
using System.Data.Entity;
using System.Collections;
using System.Web;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Threading;

namespace SMART.Api
{
    public partial interface IWmsService
    {
        //收货作业
        PageList<WMS_In_Task> Get_WMS_In_Task_PageList(WMS_In_Filter MF);
        PageList<WMS_In_Task> Get_WMS_In_Task_PageList_Distribute(WMS_In_Filter MF);
        WMS_In_Task Get_WMS_In_Task_Item(Guid Head_ID, WMS_In_Filter MF);
        List<WMS_In_Line> Get_WMS_In_Line_List_By_MatSn(Guid Head_ID, string MatSn);
        List<WMS_In_Scan> Get_WMS_In_Scan_List_By_MatSn(Guid Head_ID, string MatSn);
        List<WMS_In_Scan> Get_WMS_In_Scan_List_By_Tray_No(Guid Head_ID, string Tray_No);
        WMS_In_Head Get_WMS_In_Head_DB(Guid Head_ID);
        WMS_In_Head Get_WMS_In_Head_DB_With_Work_Person(Guid Head_ID);
        WMS_In_Line Get_WMS_In_Line_Item(Guid LineID);
        WMS_In_Line Get_WMS_In_Line_Item(Guid Head_ID, string MatSn);
        WMS_In_Line Get_WMS_In_Line_Item(Guid Head_ID, int Quantity);
        void Set_WMS_In_Line_Other_Item(Guid LineID, string MatSn);

        string Get_Task_List_To_Excel_Temp(Guid Head_ID);
        string Get_Task_List_To_Excel_Temp_By_TrayNo(Guid Head_ID, string Tray_No);

        string Get_WMS_In_Line_List_To_Excel(Guid Head_ID);
        void Delete_Task_Bat(Guid HeadID);
        void Reset_Task_Bat_Tray_No(Guid Head_ID, string Tray_No);
        void Reset_Task_Bat_Tray_No_By_Box(Guid Head_ID, string Tray_No, string Case_No);
        void WMS_In_Task_To_WMS_Stock_Check(Guid Head_ID);
        List<WMS_In_Head> Get_WMS_In_Task_List(Guid MainCID);
        List<WMS_In_Line_Other> Get_WMS_In_Line_Other_List(Guid Head_ID);

        void Set_WMS_In_Head_Item(Guid Head_ID, List<string> Person_List, List<string> Driver_List);
        void Set_WMS_In_Head_Item_For_Scan(WMS_In_Head Head);

        string Get_WMS_In_Line_List_To_Excel_With_Abnormal(WMS_In_Head Head);
        void Batch_Create_Manufacturer_WMS_In(HttpPostedFileBase ExcelFile, WMS_In_Head Head, User U);
        void Batch_Create_Distributor_WMS_In(HttpPostedFileBase ExcelFile, WMS_In_Head Head, User U);
        void Batch_Create_WMS_In(HttpPostedFileBase ExcelFile, Guid HeadID, User U);

        string MatSn_Check_And_Replace(string MatSn);

        //物流公司
        PageList<WMS_Logistics> Get_WMS_Logistics_PageList(WMS_Logistics_Filter MF);
        List<WMS_Logistics> Get_WMS_Logistics_List(Guid LinkMainCID);
        WMS_Logistics Get_WMS_Logistics_Empty();
        WMS_Logistics Get_WMS_Logistics_Item(Guid Log_ID);
        Guid Create_WMS_Logistics_Item(WMS_Logistics L);
        void Update_WMS_Logistics_Item(WMS_Logistics L);
        void Delete_WMS_Logistics_Item(Guid Log_ID);

        //作业人
        PageList<WMS_Work_Person> Get_WMS_Work_Person_PageList(WMS_Work_Person_Filter MF);
        List<WMS_Work_Person> Get_WMS_Work_Person_List(Guid LinkMainCID);
        WMS_Work_Person Get_WMS_Work_Person_Empty();
        WMS_Work_Person Get_WMS_Work_Person_Item(Guid Person_ID);
        Guid Create_WMS_Work_Person_Item(WMS_Work_Person L);
        void Update_WMS_Work_Person_Item(WMS_Work_Person L);
        void Delete_WMS_Work_Person_Item(Guid Person_ID);

        //上架作业
        PageList<WMS_In_Task> Get_WMS_Up_PageList(WMS_In_Filter MF);
        PageList<WMS_Move> Get_WMS_Up_PageList(WMS_Move_Filter MF);
        PageList<WMS_Move> Get_WMS_Move_PageList(WMS_Move_Filter MF);
        List<WMS_Move> Get_WMS_Move_List(Guid MainCID);
        List<WMS_Move> Get_WMS_Move_Up_List(Guid MainCID);
        WMS_Move Get_WMS_Move_DB(Guid Move_ID);
        void Set_WMS_Move_With_Work_Person(WMS_Move Move);
        void Batch_Create_WMS_Move_With_Work_Person(List<Guid> MoveIDList, List<string> Work_Person_List);
        WMS_Move_Task Get_WMS_Move_Task_Item(Guid Move_ID, WMS_Move_Filter MF);
        void Reset_WMS_Move_Task_Scan(Guid Move_ID);
        void Delete_WMS_Move_Item(Guid Move_ID);
        void Finish_WMS_Move_Stocktaking_Task(Guid Move_ID);
        string Get_WMS_Move_Up_Line_QRCodePath(Guid StockID, int Quantity);
        string Get_WMS_Up_To_Stock_List_With_Excel(Guid Head_ID);
        PageList<WMS_Move_Record> Get_WMS_Move_Record_PageList(WMS_Move_Filter MF);
        void Batch_Finish_WMS_Up_Process(List<string> MatSn_List, string Location, Guid MoveID);
        void Finish_WMS_Move_Task(Guid MoveID);
    }

    //收货作业
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_In_Task> Get_WMS_In_Task_PageList(WMS_In_Filter MF)
        {
            var query = db.WMS_In_Head.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_Bat_No))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_Bat_No)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Company))
            {
                query = query.Where(x => x.Logistics_Company.Contains(MF.Logistics_Company)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Mode))
            {
                query = query.Where(x => x.Logistics_Mode.Contains(MF.Logistics_Mode)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Supplier))
            {
                query = query.Where(x => x.Supplier_Name.Contains(MF.Supplier)).AsQueryable();
            }

            if (Enum.GetNames(typeof(WMS_In_Global_State_Enum)).ToList().Where(x => x == MF.Global_State).Any())
            {
                query = query.Where(x => x.Status == MF.Global_State).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatType))
            {
                query = query.Where(x => x.MatType == MF.MatType).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person == MF.Create_Person).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Time_Start) && !string.IsNullOrEmpty(MF.Time_End))
            {
                DateTime Start_DT = Convert.ToDateTime((MF.Time_Start));
                DateTime End_DT = Convert.ToDateTime((MF.Time_End));
                End_DT = End_DT.AddDays(1);
                if (DateTime.Compare(Start_DT, End_DT) > 0)
                {
                    throw new Exception("起始时间不可大于结束时间！");
                }

                query = query.Where(x => x.Create_DT >= Start_DT && x.Create_DT <= End_DT).AsQueryable();
            }

            List<WMS_In_Head> List = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> Head_ID_List = List.Select(x => x.Head_ID).ToList();
            var WMS_In_Line_Select = db.WMS_In_Line.Where(x => x.LinkMainCID == MF.LinkMainCID && Head_ID_List.Contains(x.Link_Head_ID)).Select(x => new { x.Line_ID, x.MatSn, x.Link_Head_ID, x.Quantity, x.Delivery_DT }).ToList();
            List<WMS_In_Line> WMS_In_Line_List = WMS_In_Line_Select.Select(x => new WMS_In_Line { Line_ID = x.Line_ID, MatSn = x.MatSn, Link_Head_ID = x.Link_Head_ID, Quantity = x.Quantity, Delivery_DT = x.Delivery_DT }).ToList();

            PageList<WMS_In_Task> PList = new PageList<WMS_In_Task>();
            List<WMS_In_Task> Row_List = new List<WMS_In_Task>();
            WMS_In_Task T = new WMS_In_Task();
            List<WMS_In_Line> WMS_In_Line_List_Sub = new List<WMS_In_Line>();
            foreach (var x in List)
            {
                T = new WMS_In_Task();
                T.Head_ID = x.Head_ID;
                T.Task_Bat_No_Str = x.Task_Bat_No_Str;
                T.Create_DT = x.Create_DT;
                T.Create_Person = x.Create_Person;
                T.Logistics_Company = x.Logistics_Company;
                T.Logistics_Mode = x.Logistics_Mode;
                T.MatType = x.MatType;
                T.Brand = x.Brand;
                T.Logistics_Cost_Type = x.Logistics_Cost_Type;
                T.Supplier_Name = x.Supplier_Name;
                T.Global_State = x.Status;
                T.Work_Person = x.Work_Person;
                T.Driver_Name = x.Driver_Name;
                T.Scan_Mat_Type = x.Scan_Mat_Type;
                WMS_In_Line_List_Sub = WMS_In_Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                T.MatSn_Count = WMS_In_Line_List_Sub.GroupBy(c => c.MatSn).Count();
                T.Line_Count = WMS_In_Line_List_Sub.Count();
                T.Line_Quantity_Sum = WMS_In_Line_List_Sub.Sum(c => c.Quantity);
                Row_List.Add(T);
            }

            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = List.Count();
            PList.Rows = Row_List;
            return PList;
        }

        public PageList<WMS_In_Task> Get_WMS_In_Task_PageList_Distribute(WMS_In_Filter MF)
        {
            var query = db.WMS_In_Head.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status != WMS_In_Global_State_Enum.完成入库.ToString()).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_Bat_No))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_Bat_No)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Company))
            {
                query = query.Where(x => x.Logistics_Company.Contains(MF.Logistics_Company)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Mode))
            {
                query = query.Where(x => x.Logistics_Mode.Contains(MF.Logistics_Mode)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Supplier))
            {
                query = query.Where(x => x.Supplier_Name.Contains(MF.Supplier)).AsQueryable();
            }

            if (Enum.GetNames(typeof(WMS_In_Global_State_Enum)).ToList().Where(x => x == MF.Global_State).Any())
            {
                query = query.Where(x => x.Status == MF.Global_State).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatType))
            {
                query = query.Where(x => x.MatType == MF.MatType).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person.Contains(MF.Create_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Distribution_Status))
            {
                if (MF.Work_Distribution_Status == WMS_Work_Distribution_State_Enum.已派工.ToString())
                {
                    query = query.Where(x => x.Work_Person != "").AsQueryable();
                }
                else if (MF.Work_Distribution_Status == WMS_Work_Distribution_State_Enum.未派工.ToString())
                {
                    query = query.Where(x => x.Work_Person == "").AsQueryable();
                }
            }

            List<WMS_In_Head> List = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> Head_ID_List = List.Select(x => x.Head_ID).ToList();
            var WMS_In_Line_Select = db.WMS_In_Line.Where(x => x.LinkMainCID == MF.LinkMainCID && Head_ID_List.Contains(x.Link_Head_ID)).Select(x => new { x.Line_ID, x.MatSn, x.Link_Head_ID, x.Quantity, x.Delivery_DT }).ToList();
            List<WMS_In_Line> WMS_In_Line_List = WMS_In_Line_Select.Select(x => new WMS_In_Line { Line_ID = x.Line_ID, MatSn = x.MatSn, Link_Head_ID = x.Link_Head_ID, Quantity = x.Quantity, Delivery_DT = x.Delivery_DT }).ToList();

            PageList<WMS_In_Task> PList = new PageList<WMS_In_Task>();
            List<WMS_In_Task> Row_List = new List<WMS_In_Task>();
            WMS_In_Task T = new WMS_In_Task();
            List<WMS_In_Line> WMS_In_Line_List_Sub = new List<WMS_In_Line>();
            foreach (var x in List)
            {
                T = new WMS_In_Task();
                T.Head_ID = x.Head_ID;
                T.Task_Bat_No_Str = x.Task_Bat_No_Str;
                T.Create_DT = x.Create_DT;
                T.Create_Person = x.Create_Person;
                T.Logistics_Company = x.Logistics_Company;
                T.Logistics_Mode = x.Logistics_Mode;
                T.MatType = x.MatType;
                T.Brand = x.Brand;
                T.Logistics_Cost_Type = x.Logistics_Cost_Type;
                T.Supplier_Name = x.Supplier_Name;
                T.Global_State = x.Status;
                T.Work_Person = x.Work_Person;
                T.Driver_Name = x.Driver_Name;
                T.Scan_Mat_Type = x.Scan_Mat_Type;
                WMS_In_Line_List_Sub = WMS_In_Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                T.MatSn_Count = WMS_In_Line_List_Sub.GroupBy(c => c.MatSn).Count();
                T.Line_Count = WMS_In_Line_List_Sub.Count();
                T.Line_Quantity_Sum = WMS_In_Line_List_Sub.Sum(c => c.Quantity);
                Row_List.Add(T);
            }

            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = List.Count();
            PList.Rows = Row_List;
            return PList;
        }

        public WMS_In_Task Get_WMS_In_Task_Item(Guid Head_ID, WMS_In_Filter MF)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            Head = Head == null ? new WMS_In_Head() : Head;
            List<WMS_In_Line> List = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            WMS_In_Task T = new WMS_In_Task();
            T.Head_ID = Head.Head_ID;
            T.Task_Bat_No_Str = Head.Task_Bat_No_Str;
            T.Create_DT = Head.Create_DT;
            T.Create_Person = Head.Create_Person;
            T.Logistics_Company = Head.Logistics_Company;
            T.Logistics_Mode = Head.Logistics_Mode;
            T.MatType = Head.MatType;
            T.Brand = Head.Brand;
            T.Logistics_Cost_Type = Head.Logistics_Cost_Type;
            T.Supplier_Name = Head.Supplier_Name;
            T.Global_State = Head.Status;
            T.Work_Person = Head.Work_Person;
            T.Driver_Name = Head.Driver_Name;
            T.Scan_Mat_Type = Head.Scan_Mat_Type;
            T.Line_List = new List<WMS_In_Task_Line>();
            var Line_Group = from x in List
                             group x by x.MatSn into G
                             select new
                             {
                                 MatSn = G.Key,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                                 Line_No = G.FirstOrDefault().Line_No,
                                 Line_Count = G.Count()
                             };

            List<WMS_In_Scan> List_Scan = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            int i = 0;
            WMS_In_Task_Line T_Line = new WMS_In_Task_Line();
            List<WMS_In_Scan> List_Scan_Sub = new List<WMS_In_Scan>();
            foreach (var x in Line_Group.OrderBy(x => x.Line_No).ToList())
            {
                i++;
                List_Scan_Sub = List_Scan.Where(c => c.MatSn == x.MatSn).ToList();
                T_Line = new WMS_In_Task_Line();
                T_Line.Line_No = i;
                T_Line.MatSn = x.MatSn;
                T_Line.Line_Count = x.Line_Count;
                T_Line.Quantity_Sum = x.Quantity_Sum;
                T_Line.Quantity_Sum_Scan = List_Scan_Sub.Sum(c => c.Scan_Quantity);

                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    if (List_Scan_Sub.Where(c => c.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).Any())
                    {
                        T_Line.Cases_Scan_Count += List_Scan_Sub.Where(c => c.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).Count();
                    }

                    if (List_Scan_Sub.Where(c => c.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).Any())
                    {
                        T_Line.Cases_Scan_Count += List_Scan_Sub.Where(c => c.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).Select(c => c.Case_No).Distinct().Count();
                    }

                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    T_Line.Cases_Scan_Count = List_Scan_Sub.Select(c => c.Tray_No).Distinct().Count();
                }

                if (T_Line.Quantity_Sum == T_Line.Quantity_Sum_Scan)
                {
                    T_Line.Line_State = WMS_In_Line_State_Enum.数量一致.ToString();
                }
                else if (T_Line.Quantity_Sum > T_Line.Quantity_Sum_Scan && T_Line.Quantity_Sum_Scan > 0)
                {
                    T_Line.Line_State = WMS_In_Line_State_Enum.低于到货.ToString();
                }
                else if (T_Line.Quantity_Sum < T_Line.Quantity_Sum_Scan && T_Line.Quantity_Sum_Scan > 0)
                {
                    T_Line.Line_State = WMS_In_Line_State_Enum.超出到货.ToString();
                }
                else
                {
                    T_Line.Line_State = WMS_In_Line_State_Enum.还未扫码.ToString();
                }

                T_Line.Tray_No_List = List_Scan_Sub.Select(c => c.Tray_No).Distinct().OrderBy(c => c).ToList();
                foreach (var Tray in T_Line.Tray_No_List)
                {
                    T_Line.Tray_No_List_Str += Tray + ",";
                }
                T_Line.Tray_No_List_Str = CommonLib.Trim_End_Char(T_Line.Tray_No_List_Str);


                T.Line_List.Add(T_Line);
            }

            //获取未匹配扫码信息
            List<string> MatSn_Line_ALL = Line_Group.Select(x => x.MatSn).ToList();
            List<WMS_In_Scan> List_Scan_Other = List_Scan.Where(x => MatSn_Line_ALL.Contains(x.MatSn) == false).ToList();
            foreach (var MatSn in List_Scan_Other.Select(x => x.MatSn).Distinct().ToList())
            {
                i++;
                List_Scan_Sub = List_Scan.Where(c => c.MatSn == MatSn).ToList();
                T_Line = new WMS_In_Task_Line();
                T_Line.Line_No = 0;
                T_Line.MatSn = MatSn;
                T_Line.Line_Count = 0;
                T_Line.Quantity_Sum = 0;
                T_Line.Quantity_Sum_Scan = List_Scan_Sub.Sum(c => c.Scan_Quantity);
                T_Line.Cases_Scan_Count = List_Scan_Sub.Count();
                T_Line.Tray_No_List = List_Scan_Sub.Select(c => c.Tray_No).Distinct().OrderBy(c => c).ToList();
                T_Line.Line_State = WMS_In_Line_State_Enum.多出型号.ToString();
                foreach (var Tray in T_Line.Tray_No_List)
                {
                    T_Line.Tray_No_List_Str += Tray + ",";
                }
                T_Line.Tray_No_List_Str = CommonLib.Trim_End_Char(T_Line.Tray_No_List_Str);
                T.Line_List.Add(T_Line);
            }

            MF.Return_Info = string.Empty;
            int Error_A_Count = T.Line_List.Where(x => x.Line_State == WMS_In_Line_State_Enum.还未扫码.ToString()).Count();
            int Error_B_Count = T.Line_List.Where(x => x.Line_State == WMS_In_Line_State_Enum.低于到货.ToString()).Count();
            int Error_C_Count = T.Line_List.Where(x => x.Line_State == WMS_In_Line_State_Enum.超出到货.ToString()).Count();
            int Error_D_Count = T.Line_List.Where(x => x.Line_State == WMS_In_Line_State_Enum.多出型号.ToString()).Count();

            if (Error_A_Count > 0) { MF.Return_Info += "剩余<strong>" + Error_A_Count + "</strong>项 还未扫码，"; }
            if (Error_B_Count > 0) { MF.Return_Info += "发现<strong>" + Error_B_Count + "</strong>项 低于到货，"; }
            if (Error_C_Count > 0) { MF.Return_Info += "发现<strong>" + Error_C_Count + "</strong>项 超出到货，"; }
            if (Error_D_Count > 0) { MF.Return_Info += "发现<strong>" + Error_D_Count + "</strong>项 多余型号，"; }

            if (Enum.GetNames(typeof(WMS_In_Line_State_Enum)).Where(x => x == MF.Line_Status).Any())
            {
                T.Line_List = T.Line_List.Where(x => x.Line_State == MF.Line_Status).ToList();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                T.Line_List = T.Line_List.Where(x => x.MatSn.ToLower().Contains(MF.MatSn.ToLower())).ToList();
            }

            T.Line_List = T.Line_List.OrderBy(x => x.Line_No).ToList();

            //快递信息
            T.Track_List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).OrderBy(x => x.Tracking_No).ToList();

            //原表单信息
            T.WMS_In_Line_List = List.OrderBy(x => x.Line_No).ToList();

            //托盘信息
            T.Group_Tray_List = this.Get_WMS_In_Task_Group_Tray_List(List_Scan);

            //扫码列表
            T.Scan_List = List_Scan.OrderByDescending(x => x.Create_DT).ToList();

            //扫码错误
            T.Scan_Error_List = db.WMS_In_Scan_Error.Where(x => x.Link_Head_ID == Head.Head_ID).OrderByDescending(x => x.Create_DT).ToList();

            return T;
        }

        private List<WMS_In_Task_Group_Tray> Get_WMS_In_Task_Group_Tray_List(List<WMS_In_Scan> Scan_List)
        {
            var Group = from x in Scan_List
                        group x by x.Tray_No into g
                        select new
                        {
                            Tray_No = g.Key,
                        };

            List<WMS_In_Task_Group_Tray> List = new List<WMS_In_Task_Group_Tray>();
            WMS_In_Task_Group_Tray T = new WMS_In_Task_Group_Tray();
            foreach (var x in Group)
            {
                T = new WMS_In_Task_Group_Tray();
                T.Tray_No = x.Tray_No;
                T.Scan_List = Scan_List.Where(c => c.Tray_No == x.Tray_No).ToList();
                List.Add(T);
            }

            List = List.OrderBy(x => x.Tray_No).ToList();
            return List;
        }

        public List<WMS_In_Line> Get_WMS_In_Line_List_By_MatSn(Guid Head_ID, string MatSn)
        {
            List<WMS_In_Line> List = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head_ID && x.MatSn == MatSn).OrderBy(x => x.Line_No).ToList();
            List<WMS_In_Scan> List_Scan = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head_ID && x.MatSn == MatSn).OrderBy(x => x.Create_DT).ToList();
            if (List.Any() == false && List_Scan.Any())
            {
                WMS_In_Line T = new WMS_In_Line();
                T.Line_No = 0;
                T.MatSn = MatSn;
                T.Quantity = 0;
                List.Add(T);
            }
            return List;
        }

        public List<WMS_In_Scan> Get_WMS_In_Scan_List_By_MatSn(Guid Head_ID, string MatSn)
        {
            List<WMS_In_Scan> List_Scan = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head_ID && x.MatSn == MatSn).OrderBy(x => x.Create_DT).ToList();
            return List_Scan;
        }

        public List<WMS_In_Scan> Get_WMS_In_Scan_List_By_Tray_No(Guid Head_ID, string Tray_No)
        {
            List<WMS_In_Scan> List_Scan = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head_ID && x.Tray_No == Tray_No).OrderBy(x => x.Create_DT).ToList();
            return List_Scan;
        }

        public WMS_In_Head Get_WMS_In_Head_DB(Guid Head_ID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            Head = Head == null ? new WMS_In_Head() : Head;
            return Head;
        }

        public WMS_In_Head Get_WMS_In_Head_DB_With_Work_Person(Guid Head_ID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            Head = Head == null ? new WMS_In_Head() : Head;
            
            Head.Work_Person_List= CommonLib.StringListStrToStringArray(Head.Work_Person);
            Head.Driver_Person_List = CommonLib.StringListStrToStringArray(Head.Driver_Name);
            return Head;
        }

        public WMS_In_Line Get_WMS_In_Line_Item(Guid LineID)
        {
            WMS_In_Line Line = db.WMS_In_Line.Find(LineID);
            Line = Line == null ? new WMS_In_Line() : Line;
            Line.QRCode_Path = QRCode.CreateQRCode(Line.MatSn, Line.Line_ID);
            return Line;
        }

        public WMS_In_Line Get_WMS_In_Line_Item(Guid Head_ID, string MatSn)
        {
            if (string.IsNullOrEmpty(MatSn)) { throw new Exception("产品型号为空"); }

            WMS_In_Line Line = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head_ID && x.MatSn == MatSn).FirstOrDefault();
            Line = Line == null ? new WMS_In_Line() : Line;
            return Line;
        }

        public WMS_In_Line Get_WMS_In_Line_Item(Guid LineID, int Quantity)
        {
            if (Quantity <= 0) { throw new Exception("产品数量不小于0"); }

            WMS_In_Line Line = db.WMS_In_Line.Find(LineID);
            Line = Line == null ? new WMS_In_Line() : Line;
            Line.Quantity = Quantity;
            string Str = "HONGEN/" + Line.MatSn + "/" + Line.Quantity + "/" + Line.MatBrand;
            Line.QRCode_Path = QRCode.CreateQRCode_With_No(Str, Line.Line_ID);
            return Line;
        }

        public void Set_WMS_In_Line_Other_Item(Guid LineID, string MatSn)
        {
            if (string.IsNullOrEmpty(MatSn)) { throw new Exception("产品型号为空"); }

            WMS_In_Line Line = db.WMS_In_Line.Find(LineID);
            Line = Line == null ? new WMS_In_Line() : Line;

            WMS_In_Head Head = db.WMS_In_Head.Find(Line.Link_Head_ID);
            Head = Head == null ? new WMS_In_Head() : Head;
            if (Head.Status != WMS_In_Global_State_Enum.等待收货.ToString())
            {
                throw new Exception("该产品当前状态，已不支持更新到货型号");
            }

            WMS_In_Line_Other Line_Other = new WMS_In_Line_Other();

            if (db.WMS_In_Line_Other.Where(x => x.Link_Head_ID == Line.Link_Head_ID && x.MatSn == Line.MatSn).Any())
            {
                Line_Other = db.WMS_In_Line_Other.Where(x => x.Link_Head_ID == Line.Link_Head_ID && x.MatSn == Line.MatSn).FirstOrDefault();
                Line_Other.New_MatSn = MatSn;
                db.Entry(Line_Other).State = EntityState.Modified;
            }
            else
            {
                Line_Other = new WMS_In_Line_Other();
                Line_Other.LineID = MyGUID.NewGUID();
                Line_Other.MatSn = Line.MatSn;
                Line_Other.New_MatSn = MatSn;
                Line_Other.Link_Head_ID = Line.Link_Head_ID;
                Line_Other.LinkMainCID = Line.LinkMainCID;
                db.WMS_In_Line_Other.Add(Line_Other);
            }
            MyDbSave.SaveChange(db);
        }

        public void Batch_Create_Manufacturer_WMS_In(HttpPostedFileBase ExcelFile, WMS_In_Head Head, User U)
        {
            if (string.IsNullOrEmpty(Head.Logistics_Company)) { throw new Exception("未选择物流公司"); }

            if (string.IsNullOrEmpty(Head.Logistics_Mode)) { throw new Exception("未选择运输方式"); }

            if (string.IsNullOrEmpty(Head.Brand)) { throw new Exception("未选择品牌"); }

            Supplier S = db.Supplier.Find(Head.Sup_ID);
            if (S == null) { throw new Exception("未选择供应商"); }

            Head.In_DT_Str = Head.In_DT.ToString("yyyy-MM-dd");
            Head.Logistics_Company = Head.Logistics_Company.Trim();

            //执行数据持久化
            Head.Head_ID = MyGUID.NewGUID();
            Head.Task_Bat_No = this.Auto_Create_Task_Bat_No();
            Head.Task_Bat_No_Str = this.Auto_Create_Task_Bat_No_Str_In(Head, S);
            Head.Create_DT = DateTime.Now;
            Head.Create_Person = U.UserFullName;
            Head.Status = WMS_In_Global_State_Enum.等待收货.ToString();
            Head.LinkMainCID = U.LinkMainCID;
            Head.MatType = WMS_In_Type_Enum.常规期货.ToString();
            Head.Logistics_Mode = Head.Logistics_Mode.Trim();
            Head.Brand = Head.Brand.Trim();
            Head.Supplier_Name = S.Sup_Short_Name;
            db.WMS_In_Head.Add(Head);

            if (Head.Brand == "NSK")
            {
                this.Batch_Create_Manufacturer_WMS_In_Line(ExcelFile, Head, U);
            }
            else if (Head.Brand == "NMB")
            {
                this.Batch_Create_Manufacturer_NMB_WMS_In_Line(ExcelFile, Head, U);
            }
            else
            {
                throw new Exception("常规期货目前仅支持NSK、NMB品牌的导入！");
            }

            MyDbSave.SaveChange(db);
        }

        public void Batch_Create_Manufacturer_WMS_In_Line(HttpPostedFileBase ExcelFile, WMS_In_Head Head, User U)
        {
            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "WMS_In_Line/" + U.UID);

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            string Currency = Currency_Enum.CNY.ToString();
            string MatUnit = "PCS";

            //读取Excel列，装箱数据
            List<WMS_In_Line> Line_List = new List<WMS_In_Line>();
            WMS_In_Line Line = new WMS_In_Line();
            ISheet sheet = workbook.GetSheetAt(0);

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                Line = new WMS_In_Line();
                Line.Line_ID = MyGUID.NewGUID();
                Line.Task_Bat_No = Head.Task_Bat_No;
                Line.Task_Bat_No_Str = Head.Task_Bat_No_Str;
                Line.Create_DT = Head.Create_DT;
                Line.Create_Person = Head.Create_Person;
                Line.Line_No = i;
                Line.MatType = Head.MatType;
                Line.MatUnit = MatUnit;
                Line.Currency = Currency;
                Line.LinkMainCID = Head.LinkMainCID;
                Line.Link_Head_ID = Head.Head_ID;
                Line.Logistics_Company = Head.Logistics_Company;
                Line.Logistics_Mode = Head.Logistics_Mode;
                Line.MatBrand = Head.Brand;
                Line.Supplier_Name = Head.Supplier_Name;

                try { Line.MatName = row.GetCell(0).ToString().Trim(); } catch { Line.MatName = string.Empty; }
                try { Line.MatSn = row.GetCell(1).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }
                try { Line.Order_Win_Code = row.GetCell(2).ToString().Trim(); } catch { Line.Order_Win_Code = string.Empty; }
                try { Line.Quantity = Convert.ToInt32(row.GetCell(3).ToString().Trim()); } catch { Line.Quantity = 0; }
                try { Line.Price_Cost = Convert.ToDecimal(row.GetCell(4).ToString().Trim()); } catch { Line.Price_Cost = 0; }
                try { Line.Contract_Number = row.GetCell(5).ToString().Trim(); } catch { Line.Contract_Number = string.Empty; }
                try { Line.Lading_Bill_No = row.GetCell(6).ToString().Trim(); } catch { Line.Lading_Bill_No = string.Empty; }
                try { Line.PC_Month = row.GetCell(7).ToString().Trim(); } catch { Line.PC_Month = string.Empty; }
                try { Line.Order_Win = row.GetCell(8).ToString().Trim(); } catch { Line.Order_Win = string.Empty; }

                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(9).ToString())) { throw new Exception("预计到货时间,不得为空"); }
                try { Line.Delivery_DT = Convert.ToDateTime(row.GetCell(9).DateCellValue.ToString()); } catch { throw new Exception("预计到货时间，格式不正确！"); }
                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(10).ToString())) { throw new Exception("发票日,不得为空"); }
                try { Line.Receipt_DT = Convert.ToDateTime(row.GetCell(10).DateCellValue.ToString()); } catch { throw new Exception("发票日，格式不正确！"); }
                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(11).ToString())) { throw new Exception("付款日,不得为空"); }
                try { Line.Payment_DT = Convert.ToDateTime(row.GetCell(11).DateCellValue.ToString()); } catch { throw new Exception("付款日，格式不正确！"); }

                //过滤换行符
                Line.MatName = Line.MatName.Replace(Environment.NewLine, "");
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);
                Line.Order_Win_Code = Line.Order_Win_Code.Replace(Environment.NewLine, "");
                Line.Contract_Number = Line.Contract_Number.Replace(Environment.NewLine, "");
                Line.Lading_Bill_No = Line.Lading_Bill_No.Replace(Environment.NewLine, "");
                Line.PC_Month = Line.PC_Month.Replace(Environment.NewLine, "");
                Line.Order_Win = Line.Order_Win.Replace(Environment.NewLine, "");

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

                //判断单元格是否有公式
                for (int j = 0; j < 12; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Line.MatSn) && Line.Quantity > 0 && Line.Price_Cost >= 0
                    && !string.IsNullOrEmpty(Line.MatBrand)
                    && !string.IsNullOrEmpty(Line.Contract_Number) && !string.IsNullOrEmpty(Line.Lading_Bill_No)
                    && !string.IsNullOrEmpty(Line.PC_Month) && !string.IsNullOrEmpty(Line.MatName))
                {
                    Line.Quantity = Line.Quantity;
                    Line_List.Add(Line);
                }
            }

            if (Line_List.Any())
            {

                //与系统中的型号进行比对
                List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();
                List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == Head.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
               
                Mat_List = Mat_List.Where(x => x.MatBrand == Decode_Scan_Brand.NSK.ToString()).ToList();

                Material Mat = new Material();
                foreach (var x in Line_List)
                {
                    Mat = Mat_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                    if (Mat == null)
                    {
                        throw new Exception(x.MatSn + "与系统型号不一致");
                    }
                }

                db.WMS_In_Line.AddRange(Line_List);
            }
        }

        public void Batch_Create_Manufacturer_NMB_WMS_In_Line(HttpPostedFileBase ExcelFile, WMS_In_Head Head, User U)
        {
            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "WMS_In_Line/" + U.UID);

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            string Currency = Currency_Enum.CNY.ToString();
            string MatUnit = "PCS";

            //读取Excel列，装箱数据
            List<WMS_In_Line> Line_List = new List<WMS_In_Line>();
            WMS_In_Line Line = new WMS_In_Line();
            ISheet sheet = workbook.GetSheetAt(0);

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                Line = new WMS_In_Line();
                Line.Line_ID = MyGUID.NewGUID();
                Line.Task_Bat_No = Head.Task_Bat_No;
                Line.Task_Bat_No_Str = Head.Task_Bat_No_Str;
                Line.Create_DT = Head.Create_DT;
                Line.Create_Person = Head.Create_Person;
                Line.Line_No = i;
                Line.MatType = Head.MatType;
                Line.MatUnit = MatUnit;
                Line.Currency = Currency;
                Line.LinkMainCID = Head.LinkMainCID;
                Line.Link_Head_ID = Head.Head_ID;
                Line.Logistics_Company = Head.Logistics_Company;
                Line.Logistics_Mode = Head.Logistics_Mode;
                Line.Logistics_Cost_Type = Head.Logistics_Cost_Type;
                Line.MatBrand = Head.Brand;
                Line.Supplier_Name = Head.Supplier_Name;

                try { Line.MatName = row.GetCell(0).ToString().Trim(); } catch { Line.MatName = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatName)) { break; }
                try { Line.MatSn = row.GetCell(1).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                try { Line.Quantity = Convert.ToInt32(row.GetCell(2).ToString().Trim()); } catch { Line.Quantity = 0; }
                try { Line.Price_Cost = Convert.ToDecimal(row.GetCell(3).ToString().Trim()); } catch { Line.Price_Cost = 0; }

                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(4).ToString())) { throw new Exception("下单日期,不得为空"); }
                try { Line.Order_DT = Convert.ToDateTime(row.GetCell(4).DateCellValue.ToString()); } catch { throw new Exception("下单日期，格式不正确！"); }
                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(5).ToString())) { throw new Exception("预计到货时间,不得为空"); }
                try { Line.Delivery_DT = Convert.ToDateTime(row.GetCell(5).DateCellValue.ToString()); } catch { throw new Exception("预计到货时间，格式不正确！"); }
                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(6).ToString())) { throw new Exception("发票日,不得为空"); }
                try { Line.Receipt_DT = Convert.ToDateTime(row.GetCell(6).DateCellValue.ToString()); } catch { throw new Exception("发票日，格式不正确！"); }
                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(7).ToString())) { throw new Exception("付款日,不得为空"); }
                try { Line.Payment_DT = Convert.ToDateTime(row.GetCell(7).DateCellValue.ToString()); } catch { throw new Exception("付款日，格式不正确！"); }

                //过滤换行符
                Line.MatName = Line.MatName.Replace(Environment.NewLine, "");
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

                //判断单元格是否有公式
                for (int j = 0; j < 8; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Line.MatSn) && Line.Quantity > 0 && Line.Price_Cost >= 0
                    && !string.IsNullOrEmpty(Line.MatBrand) && !string.IsNullOrEmpty(Line.MatName))
                {
                    Line_List.Add(Line);
                }
            }

            if (Line_List.Any())
            {
                //与系统中的型号进行比对
                List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();
                List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == Head.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
                Mat_List = Mat_List.Where(x => x.MatBrand == Decode_Scan_Brand.NMB.ToString()).ToList();
                Material Mat = new Material();
                foreach (var x in Line_List)
                {
                    Mat = Mat_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                    if (Mat == null)
                    {
                        throw new Exception(x.MatSn + "与系统型号不一致");
                    }
                }

                db.WMS_In_Line.AddRange(Line_List);
            }
        }

        public void Batch_Create_Distributor_WMS_In(HttpPostedFileBase ExcelFile, WMS_In_Head Head, User U)
        {
            if (string.IsNullOrEmpty(Head.Logistics_Mode)) { throw new Exception("未选择运输方式"); }

            Supplier S = db.Supplier.Find(Head.Sup_ID);
            if (S == null) { throw new Exception("未选择供应商"); }

            if (Head.Logistics_Mode != Logistics_Mode_Enum.自提.ToString() && string.IsNullOrEmpty(Head.Logistics_Company)) { throw new Exception("未选择物流公司"); }

            if (Head.Logistics_Mode != Logistics_Mode_Enum.自提.ToString() && string.IsNullOrEmpty(Head.Logistics_Cost_Type)) { throw new Exception("未选择快递费用"); }

            Head.In_DT_Str = Head.In_DT.ToString("yyyy-MM-dd");

            //执行数据持久化
            Head.Head_ID = MyGUID.NewGUID();
            Head.Task_Bat_No = this.Auto_Create_Task_Bat_No();
            Head.Task_Bat_No_Str = Auto_Create_Task_Bat_No_Str_In(Head, S);
            Head.Create_DT = DateTime.Now;
            Head.Create_Person = U.UserFullName;
            Head.Status = WMS_In_Global_State_Enum.等待收货.ToString();
            Head.LinkMainCID = U.LinkMainCID;
            Head.MatType = WMS_In_Type_Enum.零星调货.ToString();
            Head.Logistics_Company = Head.Logistics_Company.Trim();
            Head.Logistics_Mode = Head.Logistics_Mode.Trim();
            Head.Logistics_Cost_Type = Head.Logistics_Cost_Type.Trim();
            Head.Supplier_Name = S.Sup_Short_Name.Trim();
            this.Batch_Create_Distributor_WMS_In_Line(ExcelFile, Head, U);
            db.WMS_In_Head.Add(Head);
            MyDbSave.SaveChange(db);
        }

        public void Batch_Create_Distributor_WMS_In_Line(HttpPostedFileBase ExcelFile, WMS_In_Head Head, User U)
        {
            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "WMS_In_Line/" + U.UID);

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            string Currency = Currency_Enum.CNY.ToString();
            string MatUnit = "PCS";

            //读取Excel列，装箱数据
            List<WMS_In_Line> Line_List = new List<WMS_In_Line>();
            WMS_In_Line Line = new WMS_In_Line();
            ISheet sheet = workbook.GetSheetAt(0);

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                Line = new WMS_In_Line();
                Line.Task_Bat_No = Head.Task_Bat_No;
                Line.Task_Bat_No_Str = Head.Task_Bat_No_Str;
                Line.Line_ID = MyGUID.NewGUID();
                Line.Create_DT = Head.Create_DT;
                Line.Create_Person = Head.Create_Person;
                Line.Line_No = i;
                Line.MatType = Head.MatType;
                Line.MatUnit = MatUnit;
                Line.Currency = Currency;
                Line.LinkMainCID = Head.LinkMainCID;
                Line.Link_Head_ID = Head.Head_ID;
                Line.Logistics_Company = Head.Logistics_Company;
                Line.Logistics_Mode = Head.Logistics_Mode;
                Line.Logistics_Cost_Type = Head.Logistics_Cost_Type;
                Line.Supplier_Name = Head.Supplier_Name;

                try { Line.MatName = row.GetCell(0).ToString().Trim(); } catch { Line.MatName = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatName)) { break; }
                try { Line.Order_DT = Convert.ToDateTime(row.GetCell(1).DateCellValue.ToString()); } catch { throw new Exception("下单日期的格式错误！"); }
                try { Line.MatSn = row.GetCell(2).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                try { Line.MatBrand = row.GetCell(3).ToString().Trim(); } catch { Line.MatBrand = string.Empty; }
                try { Line.Quantity_Request = Convert.ToInt32(row.GetCell(4).ToString().Trim()); } catch { Line.Quantity_Request = 0; }
                try { Line.Quantity_Request_More = Convert.ToInt32(row.GetCell(5).ToString().Trim()); } catch { Line.Quantity_Request_More = 0; }
                try { Line.Customer_Name = row.GetCell(6).ToString().Trim(); } catch { Line.Customer_Name = string.Empty; }
                try { Line.Sales_Person = row.GetCell(7).ToString().Trim(); } catch { Line.Sales_Person = string.Empty; }
                try { Line.Quantity = Convert.ToInt32(row.GetCell(8).ToString().Trim()); } catch { Line.Quantity = 0; }
                try { Line.Supplier_Name = row.GetCell(9).ToString().Trim(); } catch { Line.Supplier_Name = string.Empty; }

                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(10).ToString())) { throw new Exception("预计到货时间,不得为空"); }
                try { Line.Delivery_DT = Convert.ToDateTime(row.GetCell(10).DateCellValue.ToString()); } catch { throw new Exception("预计到货时间，格式不正确！"); }
                try { Line.Price_Cost = Convert.ToDecimal(row.GetCell(11).ToString().Trim()); } catch { Line.Price_Cost = 0; }
                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(12).ToString())) { throw new Exception("发票日,不得为空"); }
                try { Line.Receipt_DT = Convert.ToDateTime(row.GetCell(12).DateCellValue.ToString()); } catch { throw new Exception("发票日，格式不正确！"); }
                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(row.GetCell(13).ToString())) { throw new Exception("付款日,不得为空"); }
                try { Line.Payment_DT = Convert.ToDateTime(row.GetCell(13).DateCellValue.ToString()); } catch { throw new Exception("付款日，格式不正确！"); }

                //过滤换行符
                Line.MatName = Line.MatName.Replace(Environment.NewLine, "");
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);
                Line.MatBrand = Line.MatBrand.Replace(Environment.NewLine, "");
                Line.Customer_Name = Line.Customer_Name.Replace(Environment.NewLine, "");
                Line.Sales_Person = Line.Sales_Person.Replace(Environment.NewLine, "");
                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(Line.MatBrand)) { throw new Exception("品牌未填写"); }

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

                //判断单元格是否有公式
                for (int j = 0; j < 14; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Line.MatSn) && Line.Quantity > 0 && !string.IsNullOrEmpty(Line.Sales_Person) && !string.IsNullOrEmpty(Line.MatBrand))
                {
                    Line_List.Add(Line);
                }
            }

            List<string> Supplier_Name_List = Line_List.Select(x => x.Supplier_Name).Distinct().ToList();
            if (Supplier_Name_List.Count() > 1) { throw new Exception("Excel内含多家『供应商』"); }
            string Supplier_Name = string.Empty;
            try { Supplier_Name = Supplier_Name_List.FirstOrDefault().ToString(); } catch { }
            if (Supplier_Name != Head.Supplier_Name) { throw new Exception("Excel内『供应商』与任务单不匹配"); }

            if (Line_List.Any())
            {
                List<string> Brand_List = Line_List.Select(c => c.MatBrand).Distinct().ToList();
                if (!string.IsNullOrEmpty(Head.Brand))
                {
                    Head.Brand = string.Empty;
                }

                foreach (var x in Brand_List)
                {
                    Head.Brand += x + ",";
                }
                Head.Brand = CommonLib.Trim_End_Char(Head.Brand);

                //与系统中的型号进行比对
                List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();
                List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == Head.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
                Mat_List = Mat_List.Where(x => Brand_List.Contains(x.MatBrand)).ToList();

                Material Mat = new Material();
                foreach (var x in Line_List)
                {
                    Mat = Mat_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                    if (Mat == null)
                    {
                        throw new Exception(x.MatSn + "与系统型号不一致");
                    }
                }

                db.WMS_In_Line.AddRange(Line_List);
            }
        }

        public void Batch_Create_WMS_In(HttpPostedFileBase ExcelFile, Guid HeadID, User U)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_In_Head is null"); }
            Head.Status = WMS_In_Global_State_Enum.等待收货.ToString();
            db.Entry(Head).State = EntityState.Modified;

            List<WMS_In_Line> List = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            db.WMS_In_Line.RemoveRange(List);

            if (Head.MatType == WMS_In_Type_Enum.常规期货.ToString())
            {
                if (Head.Brand == "NSK")
                {
                    this.Batch_Create_Manufacturer_WMS_In_Line(ExcelFile, Head, U);
                }
                else if (Head.Brand == "NMB")
                {
                    this.Batch_Create_Manufacturer_NMB_WMS_In_Line(ExcelFile, Head, U);
                }
            }
            else
            {
                this.Batch_Create_Distributor_WMS_In_Line(ExcelFile, Head, U);
            }

            MyDbSave.SaveChange(db);
        }

        public string MatSn_Check_And_Replace(string MatSn)
        {
            //System.Web.HttpUtility.HtmlEncode(MatSn);
            MatSn = MatSn.Replace(" ", " "); //&#160;
            return MatSn;
        }

        private long Auto_Create_Task_Bat_No()
        {
            long Task_Bat_No_Min = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + "0001");
            long Task_Bat_No_Max = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + "9999");

            long Task_Bat_No = 0;
            if (db.WMS_In_Head.Where(x => x.Task_Bat_No >= Task_Bat_No_Min).Any())
            {
                Task_Bat_No = db.WMS_In_Head.Max(x => x.Task_Bat_No) + 1;
            }
            else
            {
                Task_Bat_No = Task_Bat_No_Min;
            }

            if (Task_Bat_No > Task_Bat_No_Max)
            {
                throw new Exception("已经超过最大值，恭喜公司滴业务好得飞起( $ _ $ )");
            }
            return Task_Bat_No;
        }

        private string Auto_Create_Task_Bat_No_Str_In(WMS_In_Head Head, Supplier S)
        {
            WMS_Logistics Logis = db.WMS_Logistics.Where(x => x.Company_Name == Head.Logistics_Company).FirstOrDefault();
            Logis = Logis == null ? new WMS_Logistics() : Logis;

            List<string> Task_Bat_No_Str_List = db.WMS_In_Head.Where(x => x.LinkMainCID == S.LinkMainCID && x.In_DT_Str == Head.In_DT_Str).Select(x => x.Task_Bat_No_Str).Distinct().ToList();
            if (string.IsNullOrEmpty(Logis.Company_Code))
            {
                Head.Task_Bat_No_Str = S.SupplierCode + "_" + Head.In_DT.ToString("yyyyMMdd") + "_";
            }
            else
            {
                Head.Task_Bat_No_Str = S.SupplierCode + "_" + Head.In_DT.ToString("yyyyMMdd") + "_" + Logis.Company_Code + "_";
            }

            string Task_Bat_No_Str_Temp = string.Empty;

            string[] Char = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            for (int i = 0; i < Char.Length; i++)
            {
                Task_Bat_No_Str_Temp = Head.Task_Bat_No_Str + Char[i];
                if (Task_Bat_No_Str_List.Where(c => c.Contains(Task_Bat_No_Str_Temp)).Any() == false)
                {
                    Head.Task_Bat_No_Str = Head.Task_Bat_No_Str + Char[i];
                    break;
                }
            }
            return Head.Task_Bat_No_Str;
        }

        public void Delete_Task_Bat(Guid HeadID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_In_Head is null"); }
            List<WMS_In_Line> List = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            List<WMS_In_Scan> List_Scan = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            //加入不允许删除的验证
            if (List_Scan.Any()) { throw new Exception("存在扫描内容未清除,不支持删除任务单！"); }

            List<WMS_Track> Track_List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            if (Track_List.Any()) { throw new Exception("存在扫描快递信息未清除,不支持删除任务单！"); }

            db.WMS_In_Line.RemoveRange(List);
            db.WMS_In_Head.Remove(Head);
            MyDbSave.SaveChange(db);
        }

        public void Reset_Task_Bat_Tray_No(Guid Head_ID, string Tray_No)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            Head = Head == null ? new WMS_In_Head() : Head;
            if (Head.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("已完成收货，不支持重置！");
            }

            Tray_No = Tray_No.Trim();
            List<WMS_In_Scan> List_Scan = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No).ToList();
            if (List_Scan.Any())
            {
                db.WMS_In_Scan.RemoveRange(List_Scan);
                MyDbSave.SaveChange(db);
            }
        }

        public void Reset_Task_Bat_Tray_No_By_Box(Guid Head_ID, string Tray_No, string Case_No)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            Head = Head == null ? new WMS_In_Head() : Head;
            if (Head.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("已完成收货，不支持重置！");
            }

            Tray_No = Tray_No.Trim();
            Case_No = Case_No.Trim();

            List<WMS_In_Scan> List_Scan = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No && x.Case_No == Case_No).ToList();
            if (List_Scan.Any())
            {
                db.WMS_In_Scan.RemoveRange(List_Scan);
                MyDbSave.SaveChange(db);
            }
        }

        public string Get_Task_List_To_Excel_Temp(Guid Head_ID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            List<WMS_In_Scan> Scan_List = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            var Group = from x in Scan_List
                        group x by new { x.MatSn, x.Tray_No, x.Package_Type } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Tray_No = G.Key.Tray_No,
                            Package_Type = G.Key.Package_Type,
                            Line_Count = G.Count(),
                            Scan_Quantity_Sum = G.Sum(c => c.Scan_Quantity)
                        };

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("序号");
            TableHeads.Add("产品型号");
            TableHeads.Add("临时储位");
            TableHeads.Add("箱数");
            TableHeads.Add("数量");
            TableHeads.Add("供应商");

            foreach (string TableHead in TableHeads)
            {
                //TableHead
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = TableHead;
                myDataColumn.ReadOnly = true;
                myDataColumn.Unique = false;  //获取或设置一个值，该值指示列的每一行中的值是否必须是唯一的。
                DT.Columns.Add(myDataColumn);
            }

            int i = 0;
            DataRow newRow;
            foreach (var x in Group)
            {
                i++;
                newRow = DT.NewRow();
                newRow["序号"] = i.ToString();
                newRow["产品型号"] = x.MatSn;
                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    newRow["临时储位"] = "第" + x.Tray_No + "托";
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    newRow["临时储位"] = "第" + x.Tray_No + "箱";
                }

                if (x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString())
                {
                    newRow["箱数"] = x.Line_Count;
                }
                else if (x.Package_Type == WMS_Stock_Package_Enum.零头.ToString())
                {
                    newRow["箱数"] = "零头箱";
                }

                newRow["数量"] = x.Scan_Quantity_Sum;
                newRow["供应商"] = Head.Supplier_Name;
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public string Get_Task_List_To_Excel_Temp_By_TrayNo(Guid Head_ID, string Tray_No)
        {
            Tray_No = Tray_No.Trim();
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            List<WMS_In_Scan> Scan_List = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No).ToList();
            var Group = from x in Scan_List
                        group x by new { x.MatSn, x.Package_Type } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Package_Type = G.Key.Package_Type,
                            Line_Count = G.Count(),
                            Scan_Quantity_Sum = G.Sum(c => c.Scan_Quantity)
                        };

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("序号");
            TableHeads.Add("产品型号");
            TableHeads.Add("临时储位");
            TableHeads.Add("箱数");
            TableHeads.Add("数量");
            TableHeads.Add("供应商");

            foreach (string TableHead in TableHeads)
            {
                //TableHead
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = TableHead;
                myDataColumn.ReadOnly = true;
                myDataColumn.Unique = false;  //获取或设置一个值，该值指示列的每一行中的值是否必须是唯一的。
                DT.Columns.Add(myDataColumn);
            }

            int i = 0;
            DataRow newRow;
            foreach (var x in Group)
            {
                i++;
                newRow = DT.NewRow();
                newRow["序号"] = i.ToString();
                newRow["产品型号"] = x.MatSn;
                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    newRow["临时储位"] = "第" + Tray_No + "托";
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    newRow["临时储位"] = "第" + Tray_No + "箱";
                }

                if (x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString())
                {
                    newRow["箱数"] = x.Line_Count;
                }
                else if (x.Package_Type == WMS_Stock_Package_Enum.零头.ToString())
                {
                    newRow["箱数"] = "零头箱";
                }
                newRow["数量"] = x.Scan_Quantity_Sum;
                newRow["供应商"] = Head.Supplier_Name;
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public string Get_WMS_In_Line_List_To_Excel(Guid Head_ID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_In_Head is null"); }
            List<WMS_In_Line> List = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            string Path = string.Empty;
            if (Head.MatType == WMS_In_Type_Enum.常规期货.ToString())
            {
                if (Head.Brand == "NSK")
                {
                    Path = Get_WMS_In_Line_List_To_Excel_Manufacturer(List);
                }
                else if (Head.Brand == "NMB")
                {
                    Path = Get_WMS_In_Line_List_To_Excel_Manufacturer_NMB(List);
                }
            }
            else
            {
                Path = Get_WMS_In_Line_List_To_Excel_Distributor(List);
            }

            return Path;
        }

        private string Get_WMS_In_Line_List_To_Excel_Manufacturer(List<WMS_In_Line> List)
        {
            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("产品名称");
            TableHeads.Add("产品型号");
            TableHeads.Add("订货窗口编号");
            TableHeads.Add("到货数量");
            TableHeads.Add("未税单价");
            TableHeads.Add("合同号");
            TableHeads.Add("提单编号");
            TableHeads.Add("生产指令月");
            TableHeads.Add("订货窗口");
            TableHeads.Add("预计到货时间");
            TableHeads.Add("发票日");
            TableHeads.Add("付款日");

            foreach (string TableHead in TableHeads)
            {
                //TableHead
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = TableHead;
                myDataColumn.ReadOnly = true;
                myDataColumn.Unique = false;  //获取或设置一个值，该值指示列的每一行中的值是否必须是唯一的。
                DT.Columns.Add(myDataColumn);
            }

            int i = 0;
            DataRow newRow;
            foreach (var x in List)
            {
                i++;
                newRow = DT.NewRow();
                newRow["产品名称"] = x.MatName;
                newRow["产品型号"] = x.MatSn;
                newRow["订货窗口编号"] = x.Order_Win_Code;
                newRow["到货数量"] = x.Quantity;
                newRow["未税单价"] = x.Price_Cost;
                newRow["合同号"] = x.Contract_Number;
                newRow["提单编号"] = x.Lading_Bill_No;
                newRow["生产指令月"] = x.PC_Month;
                newRow["订货窗口"] = x.Order_Win;
                newRow["预计到货时间"] = x.Delivery_DT.ToString("yyyy/MM/dd");
                newRow["发票日"] = x.Receipt_DT.ToString("yyyy/MM/dd");
                newRow["付款日"] = x.Payment_DT.ToString("yyyy/MM/dd");
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        private string Get_WMS_In_Line_List_To_Excel_Manufacturer_NMB(List<WMS_In_Line> List)
        {
            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("产品名称");
            TableHeads.Add("产品型号");
            TableHeads.Add("到货数量");
            TableHeads.Add("未税单价");
            TableHeads.Add("下单日期");
            TableHeads.Add("预计到货日期");
            TableHeads.Add("发票日");
            TableHeads.Add("付款日");

            foreach (string TableHead in TableHeads)
            {
                //TableHead
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = TableHead;
                myDataColumn.ReadOnly = true;
                myDataColumn.Unique = false;  //获取或设置一个值，该值指示列的每一行中的值是否必须是唯一的。
                DT.Columns.Add(myDataColumn);
            }

            int i = 0;
            DataRow newRow;
            foreach (var x in List)
            {
                i++;
                newRow = DT.NewRow();
                newRow["产品名称"] = x.MatName;
                newRow["产品型号"] = x.MatSn;
                newRow["到货数量"] = x.Quantity;
                newRow["未税单价"] = x.Price_Cost;
                newRow["下单日期"] = x.Order_DT.ToString("yyyy/MM/dd");
                newRow["预计到货日期"] = x.Delivery_DT.ToString("yyyy/MM/dd");
                newRow["发票日"] = x.Receipt_DT.ToString("yyyy/MM/dd");
                newRow["付款日"] = x.Payment_DT.ToString("yyyy/MM/dd");
                DT.Rows.Add(newRow);
            }

            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        private string Get_WMS_In_Line_List_To_Excel_Distributor(List<WMS_In_Line> List)
        {
            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("产品名称");
            TableHeads.Add("下单日期");
            TableHeads.Add("产品型号");
            TableHeads.Add("品牌");
            TableHeads.Add("需求数");
            TableHeads.Add("备货数");
            TableHeads.Add("客户名");
            TableHeads.Add("调货人");
            TableHeads.Add("调货数");
            TableHeads.Add("调货处");
            TableHeads.Add("预计到货日期");
            TableHeads.Add("未税单价");
            TableHeads.Add("发票日");
            TableHeads.Add("付款日");

            foreach (string TableHead in TableHeads)
            {
                //TableHead
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = TableHead;
                myDataColumn.ReadOnly = true;
                myDataColumn.Unique = false;  //获取或设置一个值，该值指示列的每一行中的值是否必须是唯一的。
                DT.Columns.Add(myDataColumn);
            }

            int i = 0;
            DataRow newRow;
            foreach (var x in List)
            {
                i++;
                newRow = DT.NewRow();
                newRow["产品名称"] = x.MatName;
                newRow["下单日期"] = x.Order_DT.ToString("yyyy/MM/dd");
                newRow["产品型号"] = x.MatSn;
                newRow["品牌"] = x.MatBrand;
                newRow["需求数"] = x.Quantity_Request;
                newRow["备货数"] = x.Quantity_Request_More;
                newRow["客户名"] = x.Customer_Name;
                newRow["调货人"] = x.Sales_Person;
                newRow["调货数"] = x.Quantity;
                newRow["调货处"] = x.Supplier_Name;
                newRow["预计到货日期"] = x.Delivery_DT.ToString("yyyy/MM/dd");
                newRow["未税单价"] = x.Price_Cost;
                newRow["发票日"] = x.Receipt_DT.ToString("yyyy/MM/dd");
                newRow["付款日"] = x.Payment_DT.ToString("yyyy/MM/dd");
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        //完成收货库指令
        public void WMS_In_Task_To_WMS_Stock_Check(Guid Head_ID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);

            if (db.WMS_Stock.Where(x => x.Wms_In_Head_ID == Head.Head_ID).Any())
            {
                throw new Exception("此任务单已完成入库，请勿重复操作");
            }

            WMS_In_Filter MF = new WMS_In_Filter();
            WMS_In_Task T_Head = this.Get_WMS_In_Task_Item(Head.Head_ID, MF);

            if (T_Head.Line_List.Any() == false)
            {
                throw new Exception("Error - 未能匹配入库记录，无法发送");
            }

            if (Head.Logistics_Mode == Logistics_Mode_Enum.自提.ToString()
               && db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).Any() == false)
            {
                throw new Exception("Error - 未填写车辆信息，无法发送");
            }

            if (Head.Logistics_Cost_Type == Logistics_Cost_Type_Enum.有快递.ToString()
               && db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).Any() == false)
            {
                throw new Exception("Error - 未扫描快递单，无法发送");
            }

            List<string> Error_Status_List = new List<string>();
            Error_Status_List.Add(WMS_In_Line_State_Enum.还未扫码.ToString());
            Error_Status_List.Add(WMS_In_Line_State_Enum.低于到货.ToString());
            Error_Status_List.Add(WMS_In_Line_State_Enum.超出到货.ToString());
            Error_Status_List.Add(WMS_In_Line_State_Enum.多出型号.ToString());

            ISentEmailService IS = new SentEmailService();

            if (T_Head.Line_List.Where(x => Error_Status_List.Contains(x.Line_State)).Any() && Head.Status == WMS_In_Global_State_Enum.通知采购.ToString())
            {
                throw new Exception("之前已通知采购，仍存在任务单信息不符！");
            }

            if (T_Head.Line_List.Where(x => Error_Status_List.Contains(x.Line_State)).Any())
            {
                IS.Sent_To_Buyer_With_WMS_In_Abnormal(Head.Head_ID);
                Head.Status = WMS_In_Global_State_Enum.通知采购.ToString();
                db.Entry(Head).State = EntityState.Modified;
            }
            else
            {
                //收货进入库存
                WMS_In_Task_To_WMS_Stock(Head);
                IS.Sent_To_Buyer_With_WMS_In(Head.Head_ID);
            }
            MyDbSave.SaveChange(db);

        }

        private void WMS_In_Task_To_WMS_Stock(WMS_In_Head Head)
        {
            //未识别二维码删除
            List<WMS_In_Scan_Error> Error_List = db.WMS_In_Scan_Error.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            if (Error_List.Any()) { db.WMS_In_Scan_Error.RemoveRange(Error_List); }

            //执行入库并写入库存表
            List<WMS_Stock> Stock_List = new List<WMS_Stock>();
            WMS_Stock S = new WMS_Stock();

            List<WMS_Stock_Record> Stock_Record_List = new List<WMS_Stock_Record>();
            WMS_Stock_Record S_Record = new WMS_Stock_Record();

            List<WMS_In_Scan> Scan_List = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            List<WMS_In_Line> Line_List = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            WMS_In_Line Line = new WMS_In_Line();
            string WMS_In_DT = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            DateTime DT = DateTime.Now;
            Supplier Sup = db.Supplier.Find(Head.Sup_ID);
            if (Sup == null) { throw new Exception("供应商为空"); }

            string Location = Sup.SupplierCode + "_";
            string Location_Temp = string.Empty;
            List<string> Location_List = db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Location_Type == WMS_Stock_Location_Type_Enum.临时库位.ToString()).Select(x => x.Location).Distinct().ToList();

            string[] Char = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            for (int i = 0; i < Char.Length; i++)
            {
                Location_Temp = Location + Char[i];
                if (Location_List.Where(c => c.Contains(Location_Temp)).Any() == false)
                {
                    Location = Location + Char[i];
                    break;
                }
            }

            foreach (var x in Scan_List)
            {
                Line = Line_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Line = Line == null ? new WMS_In_Line() : Line;

                S = new WMS_Stock();
                S.Stock_ID = MyGUID.NewGUID();
                S.Quantity = x.Scan_Quantity;
                S.Package = x.Package_Type;
                S.Location_Type = WMS_Stock_Location_Type_Enum.临时库位.ToString();
                S.Location = Location + "_" + x.Tray_No;
                S.WMS_In_DT = WMS_In_DT;
                S.MatSn = x.MatSn;
                S.MatName = Line.MatName;
                S.MatUnit = Line.MatUnit;
                S.MatBrand = Line.MatBrand;
                S.Price = Line.Price_Cost;
                S.Cases = x.Case_No;
                S.Wms_In_Head_ID = Head.Head_ID;
                S.LinkMainCID = Head.LinkMainCID;
                Stock_List.Add(S);

                S_Record = new WMS_Stock_Record();
                S_Record.Stock_ID = MyGUID.NewGUID();
                S_Record.Quantity = x.Scan_Quantity;
                S_Record.Package = x.Package_Type;
                S_Record.Location_Type = WMS_Stock_Location_Type_Enum.临时库位.ToString();
                S_Record.Location = Location + "_" + x.Tray_No;
                S_Record.Create_DT = DT;
                S_Record.MatSn = x.MatSn;
                S_Record.MatName = Line.MatName;
                S_Record.MatUnit = Line.MatUnit;
                S_Record.MatBrand = Line.MatBrand;
                S_Record.Price = Line.Price_Cost;
                S_Record.Cases = x.Case_No;
                S_Record.Wms_In_Head_ID = Head.Head_ID;
                S_Record.Supplier = Head.Supplier_Name;
                S_Record.LinkMainCID = Head.LinkMainCID;
                S_Record.Remark = WMS_Stock_Record_Remark_Enum.订单入库.ToString();
                S_Record.Work_Person = Head.Work_Person;

                Stock_Record_List.Add(S_Record);
            }

            if (Stock_List.Any())
            {
                db.WMS_Stock.AddRange(Stock_List);
                db.WMS_Stock_Record.AddRange(Stock_Record_List);
            }

            Head.Status = WMS_In_Global_State_Enum.完成入库.ToString();
            db.Entry(Head).State = EntityState.Modified;

            //填补库存中无价格的产品
            List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();
            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && MatSn_List.Contains(x.MatSn) && x.Price == 0).ToList();

            if (Stock_List_DB.Any())
            {
                foreach (var x in Stock_List_DB)
                {
                    Line = Line_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                    Line = Line == null ? new WMS_In_Line() : Line;
                    x.Price = Line.Price_Cost;
                }
            }

            //生成移库任务
            List<string> Move_Location_List = Stock_List.Select(x => x.Location).Distinct().ToList();
            WMS_Move_Batch_Create(Head, Move_Location_List);
            MyDbSave.SaveChange(db);
        }

        private void WMS_Move_Batch_Create(WMS_In_Head Head, List<string> Move_Location_List)
        {
            WMS_Move Move = new WMS_Move();
            List<WMS_Move> List = new List<WMS_Move>();

            DateTime DT = DateTime.Now;
            foreach (var Location in Move_Location_List)
            {
                Move = new WMS_Move();
                Move.Move_ID = MyGUID.NewGUID();
                Move.Out_Location = Location;
                Move.Move_Status = WMS_Move_Status_Enum.待移库.ToString();
                Move.Create_DT = DT;
                Move.Task_Bat_No = Head.Task_Bat_No_Str;
                Move.Supplier_Name = Head.Supplier_Name;
                Move.LinkMainCID = Head.LinkMainCID;
                Move.Link_HeadID = Head.Head_ID;
                List.Add(Move);
            }

            db.WMS_Move.AddRange(List);
        }

        public List<WMS_In_Line_Other> Get_WMS_In_Line_Other_List(Guid Head_ID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            Head = Head == null ? new WMS_In_Head() : Head;
            List<WMS_In_Line_Other> List = db.WMS_In_Line_Other.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            return List;
        }

        //待收货列表
        public List<WMS_In_Head> Get_WMS_In_Task_List(Guid MainCID)
        {
            List<WMS_In_Head> List = db.WMS_In_Head.Where(x => x.LinkMainCID == MainCID && x.Status != WMS_In_Global_State_Enum.完成入库.ToString()).ToList();
            return List;
        }

        public void Set_WMS_In_Head_Item(Guid Head_ID, List<string> Person_List, List<string> Driver_List)
        {
            if (Person_List.Count <= 0) { throw new Exception("未勾选收货作业人！"); }

            WMS_In_Head Head_DB = db.WMS_In_Head.Find(Head_ID);
            Head_DB = Head_DB == null ? new WMS_In_Head() : Head_DB;

            if (Head_DB.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("该收货任务已完成入库");
            }

            if (Head_DB.Logistics_Mode == Logistics_Mode_Enum.自提.ToString() && Driver_List.Count() <= 0) { throw new Exception("未填写驾驶员！"); }

            Head_DB.Work_Person = string.Empty;

            foreach (var Person in Person_List)
            {
                Head_DB.Work_Person += Person.Trim() + ",";
            }

            Head_DB.Work_Person = CommonLib.Trim_End_Char(Head_DB.Work_Person);

            if (Head_DB.Logistics_Mode == Logistics_Mode_Enum.自提.ToString())
            {
                Head_DB.Driver_Name = string.Empty;

                foreach (var Driver in Driver_List)
                {
                    Head_DB.Driver_Name += Driver.Trim() + ",";
                }

                Head_DB.Driver_Name = CommonLib.Trim_End_Char(Head_DB.Driver_Name);
            }

            db.Entry(Head_DB).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Set_WMS_In_Head_Item_For_Scan(WMS_In_Head Head)
        {
            if (string.IsNullOrEmpty(Head.Scan_Mat_Type)) { throw new Exception("未选择收货方式！"); }

            WMS_In_Head Head_DB = db.WMS_In_Head.Find(Head.Head_ID);
            Head_DB = Head_DB == null ? new WMS_In_Head() : Head_DB;

            if (db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID).Any())
            {
                throw new Exception("该任务单存在收货扫码内容，不支持更改收货方式");
            }

            Head_DB.Scan_Mat_Type = Head.Scan_Mat_Type.Trim();
            db.Entry(Head_DB).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public string Get_WMS_In_Line_List_To_Excel_With_Abnormal(WMS_In_Head Head)
        {
            List<WMS_In_Line> List = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            List<WMS_In_Task_Line> Line_List = new List<WMS_In_Task_Line>();
            var Line_Group = from x in List
                             group x by x.MatSn into G
                             select new
                             {
                                 MatSn = G.Key,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                                 Line_No = G.FirstOrDefault().Line_No,
                                 Line_Count = G.Count()
                             };

            List<WMS_In_Scan> List_Scan = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            List<WMS_In_Line_Other> Line_Other = db.WMS_In_Line_Other.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            int i = 0;
            WMS_In_Task_Line T_Line = new WMS_In_Task_Line();
            List<WMS_In_Scan> List_Scan_Sub = new List<WMS_In_Scan>();
            foreach (var x in Line_Group.OrderBy(x => x.Line_No).ToList())
            {
                i++;
                List_Scan_Sub = List_Scan.Where(c => c.MatSn == x.MatSn).ToList();
                T_Line = new WMS_In_Task_Line();
                T_Line.Line_No = i;
                T_Line.MatSn = x.MatSn;
                if (Line_Other.Where(c => c.MatSn == x.MatSn).Any())
                {
                    T_Line.MatSn_Real = Line_Other.Where(c => c.MatSn == x.MatSn).FirstOrDefault().New_MatSn;
                }
                else
                {
                    T_Line.MatSn_Real = x.MatSn;

                }

                T_Line.Line_Count = x.Line_Count;
                T_Line.Quantity_Sum = x.Quantity_Sum;
                T_Line.Quantity_Sum_Scan = List_Scan_Sub.Sum(c => c.Scan_Quantity);
                T_Line.Cases_Scan_Count = List_Scan_Sub.Count();

                if (T_Line.Quantity_Sum == T_Line.Quantity_Sum_Scan)
                {
                    T_Line.Line_State = WMS_In_Line_State_Enum.数量一致.ToString();
                }
                else if (T_Line.Quantity_Sum > T_Line.Quantity_Sum_Scan && T_Line.Quantity_Sum_Scan > 0)
                {
                    T_Line.Line_State = WMS_In_Line_State_Enum.低于到货.ToString();
                }
                else if (T_Line.Quantity_Sum < T_Line.Quantity_Sum_Scan && T_Line.Quantity_Sum_Scan > 0)
                {
                    T_Line.Line_State = WMS_In_Line_State_Enum.超出到货.ToString();
                }
                else
                {
                    T_Line.Line_State = WMS_In_Line_State_Enum.还未扫码.ToString();
                }

                T_Line.Tray_No_List = List_Scan_Sub.Select(c => c.Tray_No).Distinct().ToList();
                foreach (var Tray in T_Line.Tray_No_List)
                {
                    T_Line.Tray_No_List_Str += Tray + ",";
                }

                T_Line.Tray_No_List_Str = CommonLib.Trim_End_Char(T_Line.Tray_No_List_Str);

                //快递单号绑定
                T_Line.Track_No_List = List_Scan_Sub.Select(c => c.Track_No).Distinct().ToList();
                foreach (var Track in T_Line.Track_No_List)
                {
                    T_Line.Track_No_List_Str += Track + ",";
                }

                T_Line.Track_No_List_Str = CommonLib.Trim_End_Char(T_Line.Track_No_List_Str);

                Line_List.Add(T_Line);
            }

            //获取未匹配扫码信息
            List<string> MatSn_Line_ALL = Line_Group.Select(x => x.MatSn).ToList();
            List<WMS_In_Scan> List_Scan_Other = List_Scan.Where(x => MatSn_Line_ALL.Contains(x.MatSn) == false).ToList();
            foreach (var MatSn in List_Scan_Other.Select(x => x.MatSn).Distinct().ToList())
            {
                i++;
                List_Scan_Sub = List_Scan.Where(c => c.MatSn == MatSn).ToList();
                T_Line = new WMS_In_Task_Line();
                T_Line.Line_No = 0;
                T_Line.MatSn = MatSn;
                if (Line_Other.Where(c => c.MatSn == MatSn).Any())
                {
                    T_Line.MatSn_Real = Line_Other.Where(c => c.MatSn == MatSn).FirstOrDefault().New_MatSn;
                }
                else
                {
                    T_Line.MatSn_Real = MatSn;
                }

                T_Line.Line_Count = 0;
                T_Line.Quantity_Sum = 0;
                T_Line.Quantity_Sum_Scan = List_Scan_Sub.Sum(c => c.Scan_Quantity);
                T_Line.Cases_Scan_Count = List_Scan_Sub.Count();
                T_Line.Tray_No_List = List_Scan_Sub.Select(c => c.Tray_No).Distinct().ToList();
                T_Line.Line_State = WMS_In_Line_State_Enum.多出型号.ToString();
                foreach (var Tray in T_Line.Tray_No_List)
                {
                    T_Line.Tray_No_List_Str += Tray + ",";
                }
                T_Line.Tray_No_List_Str = CommonLib.Trim_End_Char(T_Line.Tray_No_List_Str);

                //快递单号绑定
                T_Line.Track_No_List = List_Scan_Sub.Select(c => c.Track_No).Distinct().ToList();
                foreach (var Track in T_Line.Track_No_List)
                {
                    T_Line.Track_No_List_Str += Track + ",";
                }

                T_Line.Track_No_List_Str = CommonLib.Trim_End_Char(T_Line.Track_No_List_Str);

                Line_List.Add(T_Line);
            }

            Line_List = Line_List.OrderBy(x => x.Line_No).ToList();

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("产品名称");
            TableHeads.Add("产品型号");
            TableHeads.Add("到货型号");
            TableHeads.Add("通知到货");
            TableHeads.Add("实际到货");

            if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
            {
                TableHeads.Add("箱数");
                TableHeads.Add("托号");
            }
            else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
            {
                TableHeads.Add("箱号");
            }

            TableHeads.Add("快递单号");
            TableHeads.Add("当前状态");
            foreach (string TableHead in TableHeads)
            {
                //TableHead
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = TableHead;
                myDataColumn.ReadOnly = true;
                myDataColumn.Unique = false;  //获取或设置一个值，该值指示列的每一行中的值是否必须是唯一的。
                DT.Columns.Add(myDataColumn);
            }

            DataRow newRow;
            foreach (var x in Line_List)
            {
                newRow = DT.NewRow();
                newRow["产品型号"] = x.MatSn;
                newRow["到货型号"] = x.MatSn_Real;
                newRow["通知到货"] = x.Quantity_Sum;
                newRow["实际到货"] = x.Quantity_Sum_Scan;

                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    newRow["箱数"] = x.Cases_Scan_Count;
                    newRow["托号"] = x.Tray_No_List_Str;
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    newRow["箱号"] = x.Tray_No_List_Str;
                }

                newRow["快递单号"] = x.Track_No_List_Str;
                newRow["当前状态"] = x.Line_State;
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }
    }

    //物流公司
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Logistics> Get_WMS_Logistics_PageList(WMS_Logistics_Filter MF)
        {
            var query = db.WMS_Logistics.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.Company_Name.Contains(MF.Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatType))
            {
                query = query.Where(x => x.MatType == MF.MatType).AsQueryable();
            }

            PageList<WMS_Logistics> PList = new PageList<WMS_Logistics>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList(); ;
            return PList;
        }

        public List<WMS_Logistics> Get_WMS_Logistics_List(Guid LinkMainCID)
        {
            List<WMS_Logistics> List = db.WMS_Logistics.Where(x => x.LinkMainCID == LinkMainCID).OrderBy(x => x.Create_DT).ToList();
            return List;
        }

        public WMS_Logistics Get_WMS_Logistics_Empty()
        {
            WMS_Logistics L = new WMS_Logistics();
            return L;
        }

        public WMS_Logistics Get_WMS_Logistics_Item(Guid Log_ID)
        {
            WMS_Logistics L = db.WMS_Logistics.Find(Log_ID);
            L = L == null ? new WMS_Logistics() : L;
            return L;
        }

        public Guid Create_WMS_Logistics_Item(WMS_Logistics L)
        {
            L.Log_ID = MyGUID.NewGUID();
            L.Company_Name = L.Company_Name.Trim();
            L.Company_Code = L.Company_Code.Trim();
            if (db.WMS_Logistics.Where(x => x.LinkMainCID == L.LinkMainCID && x.Log_ID != L.Log_ID && x.Company_Name == L.Company_Name).Any())
            {
                throw new Exception("物流公司名称重复！");
            }

            if (db.WMS_Logistics.Where(x => x.LinkMainCID == L.LinkMainCID && x.Log_ID != L.Log_ID && x.Company_Code == L.Company_Code).Any())
            {
                throw new Exception("物流公司代码重复！");
            }

            db.WMS_Logistics.Add(L);
            MyDbSave.SaveChange(db);
            return L.Log_ID;
        }

        public void Update_WMS_Logistics_Item(WMS_Logistics L)
        {
            WMS_Logistics OldL = db.WMS_Logistics.Find(L.Log_ID);
            if (OldL == null) { throw new Exception("WMS_Logistics is null!"); }
            OldL.Company_Name = L.Company_Name.Trim();
            OldL.MatType = L.MatType.Trim();
            OldL.Company_Code = L.Company_Code.Trim();
            if (db.WMS_Logistics.Where(x => x.LinkMainCID == OldL.LinkMainCID && x.Log_ID != OldL.Log_ID && x.Company_Name == OldL.Company_Name).Any())
            {
                throw new Exception("物流公司名称重复！");
            }

            if (db.WMS_Logistics.Where(x => x.LinkMainCID == OldL.LinkMainCID && x.Log_ID != OldL.Log_ID && x.Company_Code == OldL.Company_Code).Any())
            {
                throw new Exception("物流公司代码重复！");
            }
            db.Entry(OldL).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_WMS_Logistics_Item(Guid Log_ID)
        {
            WMS_Logistics L = db.WMS_Logistics.Find(Log_ID);
            if (L == null) { throw new Exception("WMS_Logistics is null!"); }
            db.WMS_Logistics.Remove(L);
            MyDbSave.SaveChange(db);
        }
    }

    //作业人
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Work_Person> Get_WMS_Work_Person_PageList(WMS_Work_Person_Filter MF)
        {
            var query = db.WMS_Work_Person.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.Person_Name.Contains(MF.Keyword)).AsQueryable();
            }

            PageList<WMS_Work_Person> PList = new PageList<WMS_Work_Person>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList(); ;
            return PList;
        }

        public List<WMS_Work_Person> Get_WMS_Work_Person_List(Guid LinkMainCID)
        {
            List<WMS_Work_Person> List = db.WMS_Work_Person.Where(x => x.LinkMainCID == LinkMainCID).OrderBy(x => x.Create_DT).ToList();
            return List;
        }

        public WMS_Work_Person Get_WMS_Work_Person_Empty()
        {
            WMS_Work_Person L = new WMS_Work_Person();
            return L;
        }

        public WMS_Work_Person Get_WMS_Work_Person_Item(Guid Person_ID)
        {
            WMS_Work_Person L = db.WMS_Work_Person.Find(Person_ID);
            L = L == null ? new WMS_Work_Person() : L;
            return L;
        }

        public Guid Create_WMS_Work_Person_Item(WMS_Work_Person L)
        {
            L.Person_ID = MyGUID.NewGUID();
            L.Person_Name = L.Person_Name.Trim();
            if (db.WMS_Work_Person.Where(x => x.LinkMainCID == L.LinkMainCID && x.Person_ID != L.Person_ID && x.Person_Name == L.Person_Name).Any())
            {
                throw new Exception("作业人名称重复！");
            }

            db.WMS_Work_Person.Add(L);
            MyDbSave.SaveChange(db);
            return L.Person_ID;
        }

        public void Update_WMS_Work_Person_Item(WMS_Work_Person L)
        {
            WMS_Work_Person OldL = db.WMS_Work_Person.Find(L.Person_ID);
            if (OldL == null) { throw new Exception("WMS_Work_Person is null!"); }
            OldL.Person_Name = L.Person_Name.Trim();

            if (db.WMS_Work_Person.Where(x => x.LinkMainCID == L.LinkMainCID && x.Person_ID != OldL.Person_ID && x.Person_Name == OldL.Person_Name).Any())
            {
                throw new Exception("作业人名称重复！");
            }

            db.Entry(OldL).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_WMS_Work_Person_Item(Guid Person_ID)
        {
            WMS_Work_Person L = db.WMS_Work_Person.Find(Person_ID);
            if (L == null) { throw new Exception("WMS_Work_Person is null!"); }
            db.WMS_Work_Person.Remove(L);
            MyDbSave.SaveChange(db);
        }
    }

    //上架作业
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_In_Task> Get_WMS_Up_PageList(WMS_In_Filter MF)
        {
            var query = db.WMS_In_Head.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status == WMS_In_Global_State_Enum.完成入库.ToString()).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_Bat_No))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_Bat_No)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Company))
            {
                query = query.Where(x => x.Logistics_Company.Contains(MF.Logistics_Company)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Mode))
            {
                query = query.Where(x => x.Logistics_Mode.Contains(MF.Logistics_Mode)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Supplier))
            {
                query = query.Where(x => x.Supplier_Name.Contains(MF.Supplier)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatType))
            {
                query = query.Where(x => x.MatType == MF.MatType).AsQueryable();
            }

            List<WMS_In_Head> List = query.ToList();

            List<Guid> Head_ID_List = List.Select(x => x.Head_ID).ToList();
            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => Head_ID_List.Contains(x.Wms_In_Head_ID)).ToList();

            PageList<WMS_In_Task> PList = new PageList<WMS_In_Task>();
            List<WMS_In_Task> Row_List = new List<WMS_In_Task>();
            WMS_In_Task T = new WMS_In_Task();
            List<WMS_Stock> Stock_List_Sub = new List<WMS_Stock>();

            List<WMS_Move> Move_List_DB = db.WMS_Move.Where(x => Head_ID_List.Contains(x.Link_HeadID)).ToList();
            List<WMS_Move> Move_List = new List<WMS_Move>();
            foreach (var x in Move_List_DB)
            {
                if (Stock_List.Where(c => c.Wms_In_Head_ID == x.Link_HeadID && c.Location == x.Out_Location).Any())
                {
                    Move_List.Add(x);
                }
            }

            List<WMS_Move> Move_List_Sub = new List<WMS_Move>();
            foreach (var x in List)
            {
                Stock_List_Sub = Stock_List.Where(c => c.Wms_In_Head_ID == x.Head_ID).ToList();
                T = new WMS_In_Task();
                T.Head_ID = x.Head_ID;
                T.Task_Bat_No_Str = x.Task_Bat_No_Str;
                T.Create_DT = x.Create_DT;
                T.Create_Person = x.Create_Person;
                T.Logistics_Company = x.Logistics_Company;
                T.Logistics_Mode = x.Logistics_Mode;
                T.MatType = x.MatType;
                T.Brand = x.Brand;
                T.Logistics_Cost_Type = x.Logistics_Cost_Type;
                T.Supplier_Name = x.Supplier_Name;
                T.MatSn_Count = Stock_List_Sub.GroupBy(c => c.MatSn).Count();
                T.Line_Count = Stock_List_Sub.Count();
                T.Line_Quantity_Sum = Stock_List_Sub.Sum(c => c.Quantity);

                if (Move_List.Where(c => c.Link_HeadID == x.Head_ID).Any())
                {
                    T.Global_State = WMS_Move_Status_Enum.待移库.ToString();
                }
                else
                {
                    T.Global_State = WMS_Move_Status_Enum.已移库.ToString();
                }

                Row_List.Add(T);
            }

            if (!string.IsNullOrEmpty(MF.Global_State))
            {

                if (MF.Global_State == WMS_Move_Status_Enum.待移库.ToString())
                {
                    Row_List = Row_List.Where(x => x.Global_State.Contains(MF.Global_State)).OrderBy(x => x.Create_DT).ToList();
                }
                else
                {
                    Row_List = Row_List.Where(x => x.Global_State.Contains(MF.Global_State)).OrderByDescending(x => x.Create_DT).ToList();
                }

            }

            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Row_List.Count();
            PList.Rows = Row_List.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public PageList<WMS_Move> Get_WMS_Up_PageList(WMS_Move_Filter MF)
        {
            var query = db.WMS_Move.Where(x => x.Link_HeadID == MF.Link_HeadID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Out_Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Move_Status))
            {
                query = query.Where(x => x.Move_Status == MF.Move_Status).AsQueryable();
            }

            List<WMS_Move> Move_List_DB = query.ToList();

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.Wms_In_Head_ID == MF.Link_HeadID).ToList();
            List<WMS_Move> Move_List = new List<WMS_Move>();
            foreach (var x in Move_List_DB)
            {
                if (Stock_List.Where(c => c.Location == x.Out_Location).Any())
                {
                    Move_List.Add(x);
                }
            }

            List<Guid> Guid_List = Move_List.Select(x => x.Move_ID).Distinct().ToList();
            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => Guid_List.Contains(x.Link_TaskID) && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();

            List<WMS_Stock> Stock_List_Sub = new List<WMS_Stock>();

            foreach (var x in Move_List)
            {
                if (Stocktaking_List.Where(c => c.Link_TaskID == x.Move_ID).Any())
                {
                    x.Is_Stocktaking = true;
                }

                Stock_List_Sub = Stock_List.Where(c => c.Location == x.Out_Location).ToList();
                x.MatSn_Count = Stock_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Stock_List_Sub.Sum(c => c.Quantity);
            }

            PageList<WMS_Move> PList = new PageList<WMS_Move>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Move_List.Count();
            PList.Rows = Move_List.OrderBy(x => x.Out_Location).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public PageList<WMS_Move> Get_WMS_Move_PageList(WMS_Move_Filter MF)
        {
            var query = db.WMS_Move.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Task_Bat_No == "").AsQueryable();

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Out_Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.In_Location))
            {
                query = query.Where(x => x.In_Location.Contains(MF.In_Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Move_Status))
            {
                query = query.Where(x => x.Move_Status == MF.Move_Status).AsQueryable();
            }

            List<WMS_Move> Move_List = query.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> Guid_List = Move_List.Select(x => x.Move_ID).Distinct().ToList();
            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => Guid_List.Contains(x.Link_TaskID) && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();

            List<string> Loc_List = Move_List.Select(x => x.Out_Location).Distinct().ToList();
            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => Loc_List.Contains(x.Location)).ToList();
            List<WMS_Stock> Stock_List_Sub = new List<WMS_Stock>();

            foreach (var x in Move_List)
            {
                if (Stocktaking_List.Where(c => c.Link_TaskID == x.Move_ID).Any())
                {
                    x.Is_Stocktaking = true;
                }

                Stock_List_Sub = Stock_List.Where(c => c.Location == x.Out_Location).ToList();
                x.MatSn_Count = Stock_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Stock_List_Sub.Sum(c => c.Quantity);
            }

            PageList<WMS_Move> PList = new PageList<WMS_Move>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Move_List.Count();
            PList.Rows = Move_List.OrderBy(x => x.Create_DT).ToList();
            return PList;
        }

        public List<WMS_Move> Get_WMS_Move_List(Guid MainCID)
        {
            List<WMS_Move> Move_List = db.WMS_Move.Where(x => x.LinkMainCID == MainCID && x.Task_Bat_No == "" && x.Move_Status == WMS_Move_Status_Enum.待移库.ToString()).OrderBy(x => x.Create_DT).ToList();
            return Move_List;
        }

        public List<WMS_Move> Get_WMS_Move_Up_List(Guid MainCID)
        {
            List<WMS_Move> Move_List = db.WMS_Move.Where(x => x.LinkMainCID == MainCID && x.Move_Status == WMS_Move_Status_Enum.待移库.ToString() && x.Task_Bat_No != "").ToList();
            List<WMS_Move> Remove_List = new List<WMS_Move>();
            List<string> Out_Location_List = Move_List.Select(x => x.Out_Location).Distinct().ToList();
            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == MainCID && Out_Location_List.Contains(x.Location)).ToList();
            foreach (var x in Move_List.Where(x => x.Move_Status == WMS_Move_Status_Enum.待移库.ToString()).ToList())
            {
                if (Stock_List.Where(c => c.Wms_In_Head_ID == x.Link_HeadID).Any() == false)
                {
                    Remove_List.Add(x);
                }
            }

            foreach (var x in Remove_List)
            {
                Move_List.Remove(x);
            }

            if (Remove_List.Any())
            {
                db.WMS_Move.RemoveRange(Remove_List);
                MyDbSave.SaveChange(db);
            }

            return Move_List.OrderBy(x => x.Out_Location).ToList();
        }

        public WMS_Move Get_WMS_Move_DB(Guid Move_ID)
        {
            WMS_Move Move = db.WMS_Move.Find(Move_ID);
            Move = Move == null ? new WMS_Move() : Move;
            return Move;
        }

        public void Set_WMS_Move_With_Work_Person(WMS_Move Move)
        {
            WMS_Move Move_DB = db.WMS_Move.Find(Move.Move_ID);
            Move_DB = Move_DB == null ? new WMS_Move() : Move_DB;

            if (Move_DB.Move_Status == WMS_Move_Status_Enum.已移库.ToString())
            {
                throw new Exception("该移库任务已完成，不支持派工!");
            }

            Move_DB.Work_Person = Move.Work_Person.Trim();
            if (string.IsNullOrEmpty(Move_DB.Work_Person)) { throw new Exception("未填写移库作业人！"); }

            db.Entry(Move_DB).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Batch_Create_WMS_Move_With_Work_Person(List<Guid> MoveIDList, List<string> Work_Person_List)
        {
            if (Work_Person_List.Count() <= 0) { throw new Exception("请填写作业人！"); }

            List<WMS_Move> List = db.WMS_Move.Where(x => MoveIDList.Contains(x.Move_ID)).ToList();

            if (List.Where(x => x.Move_Status == WMS_Move_Status_Enum.已移库.ToString()).Any())
            {
                throw new Exception("存在已移库任务，不支持派工!");
            }

            string Work_Person = string.Empty;
            foreach (var Person in Work_Person_List)
            {
                Work_Person += Person.Trim() + ",";
            }

            Work_Person = CommonLib.Trim_End_Char(Work_Person);

            foreach (var x in List)
            {
                x.Work_Person = Work_Person;
                db.Entry(x).State = EntityState.Modified;
            }

            MyDbSave.SaveChange(db);
        }

        public void Batch_Finish_WMS_Up_Process(List<string> MatSn_List, string Location, Guid MoveID)
        {
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("任务不存在"); }

            Location = Location == null ? string.Empty : Location;
            if (string.IsNullOrEmpty(Location)) { throw new Exception("请填写库位！"); }

            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).ToList();
            Stock_List_DB = Stock_List_DB.Where(x => MatSn_List.Contains(x.MatSn)).ToList();

            if (string.IsNullOrEmpty(Move.Work_Person)) { throw new Exception("未派工"); }

            if (db.WMS_Location.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Location).Any() == false)
            {
                throw new Exception("系统库位中不存在填写的库位");
            }

            List<WMS_Move_Record> Record_List = new List<WMS_Move_Record>();
            WMS_Move_Record Record = new WMS_Move_Record();

            DateTime DT = DateTime.Now;

            //库存产品变换库位
            foreach (var x in Stock_List_DB)
            {
                //上架移库记录
                Record = new WMS_Move_Record();
                Record.Record_ID = MyGUID.NewGUID();
                Record.Out_Location = x.Location;
                Record.In_Location = Location;
                Record.Create_DT = DT;
                Record.LinkMainCID = x.LinkMainCID;
                Record.Work_Person = Move.Work_Person;
                Record.Link_TaskID = Move.Move_ID;
                Record.MatSn = x.MatSn;
                Record.Quantity = x.Quantity;
                Record.Package_Type = x.Package;
                Record.MatName = x.MatName;
                Record.MatBrand = x.MatBrand;
                Record.MatUnit = x.MatUnit;
                if (Move.Link_HeadID == Guid.Empty)
                {
                    Record.Move_Type = WMS_Move_Type_Enum.移库作业.ToString();
                }
                else
                {
                    Record.Move_Type = WMS_Move_Type_Enum.上架作业.ToString();
                }
                Record_List.Add(Record);

                x.Location = Location;
                x.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                db.Entry(x).State = EntityState.Modified;
            }

            db.WMS_Move_Record.AddRange(Record_List);
            MyDbSave.SaveChange(db);
            Thread.Sleep(500);

            //移库任务变换状态
            if (db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).Any() == false)
            {
                Move.Move_Status = WMS_Move_Status_Enum.已移库.ToString();
            }

            if (string.IsNullOrEmpty(Move.In_Location))
            {
                Move.In_Location = Location;
            }
            else
            {
                Move.In_Location = Move.In_Location + "," + Location;
            }

            db.Entry(Move).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public WMS_Move_Task Get_WMS_Move_Task_Item(Guid Move_ID, WMS_Move_Filter MF)
        {
            WMS_Move Move = db.WMS_Move.Find(Move_ID);
            Move = Move == null ? new WMS_Move() : Move;
            List<WMS_Stocktaking> List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();
            WMS_Stocktaking Stocktaking = List.FirstOrDefault();
            Stocktaking = Stocktaking == null ? new WMS_Stocktaking() : Stocktaking;
            WMS_Move_Task T = new WMS_Move_Task();
            T.Move_ID = Move.Move_ID;
            T.Create_DT = Move.Create_DT;
            T.Status = Stocktaking.Status;
            T.Work_Person = Move.Work_Person;
            T.Out_Location = Move.Out_Location;
            T.In_Location = Stocktaking.Location;
            T.Task_Bat_No = Move.Task_Bat_No;
            T.Supplier_Name = Move.Supplier_Name;
            T.Line_List = new List<WMS_Move_Task_Line>();
            var Line_Group = from x in List
                             group x by new { x.MatSn, x.MatBrand } into G
                             select new
                             {
                                 MatSn = G.Key.MatSn,
                                 MatBrand = G.Key.MatBrand,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                                 Line_Count = G.Count()
                             };

            List<WMS_Stocktaking_Scan> List_Scan = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();

            int i = 0;
            WMS_Move_Task_Line T_Line = new WMS_Move_Task_Line();
            List<WMS_Stocktaking_Scan> List_Scan_Sub = new List<WMS_Stocktaking_Scan>();
            foreach (var x in Line_Group)
            {
                i++;
                List_Scan_Sub = List_Scan.Where(c => c.MatSn == x.MatSn).ToList();
                T_Line = new WMS_Move_Task_Line();
                T_Line.Line_No = i;
                T_Line.MatSn = x.MatSn;
                T_Line.MatBrand = x.MatBrand;
                T_Line.Line_Count = x.Line_Count;
                T_Line.Quantity_Sum = x.Quantity_Sum;
                T_Line.Quantity_Sum_Scan = List_Scan_Sub.Sum(c => c.Scan_Quantity);

                if (T_Line.Quantity_Sum == T_Line.Quantity_Sum_Scan)
                {
                    T_Line.Line_State = WMS_Move_Line_State_Enum.数量一致.ToString();
                }
                else if (T_Line.Quantity_Sum > T_Line.Quantity_Sum_Scan && T_Line.Quantity_Sum_Scan > 0)
                {
                    T_Line.Line_State = WMS_Move_Line_State_Enum.低于系统.ToString();
                }
                else if (T_Line.Quantity_Sum < T_Line.Quantity_Sum_Scan && T_Line.Quantity_Sum_Scan > 0)
                {
                    T_Line.Line_State = WMS_Move_Line_State_Enum.超出系统.ToString();
                }
                else
                {
                    T_Line.Line_State = WMS_Move_Line_State_Enum.还未扫码.ToString();
                }

                T.Line_List.Add(T_Line);
            }

            //获取未匹配扫码信息
            List<string> MatSn_Line_ALL = Line_Group.Select(x => x.MatSn).ToList();
            List<WMS_Stocktaking_Scan> List_Scan_Other = List_Scan.Where(x => MatSn_Line_ALL.Contains(x.MatSn) == false).ToList();
            foreach (var x in List_Scan_Other)
            {
                i++;
                List_Scan_Sub = List_Scan.Where(c => c.MatSn == x.MatSn).ToList();
                T_Line = new WMS_Move_Task_Line();
                T_Line.Line_No = 0;
                T_Line.MatSn = x.MatSn;
                T_Line.MatBrand = x.MatBrand;
                T_Line.Line_Count = 0;
                T_Line.Quantity_Sum = 0;
                T_Line.Quantity_Sum_Scan = List_Scan_Sub.Sum(c => c.Scan_Quantity);
                T_Line.Line_State = WMS_In_Line_State_Enum.多出型号.ToString();
                T.Line_List.Add(T_Line);
            }

            MF.Return_Info = string.Empty;
            int Error_A_Count = T.Line_List.Where(x => x.Line_State == WMS_Move_Line_State_Enum.还未扫码.ToString()).Count();
            int Error_B_Count = T.Line_List.Where(x => x.Line_State == WMS_Move_Line_State_Enum.低于系统.ToString()).Count();
            int Error_C_Count = T.Line_List.Where(x => x.Line_State == WMS_Move_Line_State_Enum.超出系统.ToString()).Count();
            int Error_D_Count = T.Line_List.Where(x => x.Line_State == WMS_Move_Line_State_Enum.多出型号.ToString()).Count();

            if (Error_A_Count > 0) { MF.Return_Info += "剩余<strong>" + Error_A_Count + "</strong>项 还未扫码，"; }
            if (Error_B_Count > 0) { MF.Return_Info += "发现<strong>" + Error_B_Count + "</strong>项 低于系统，"; }
            if (Error_C_Count > 0) { MF.Return_Info += "发现<strong>" + Error_C_Count + "</strong>项 超出系统，"; }
            if (Error_D_Count > 0) { MF.Return_Info += "发现<strong>" + Error_D_Count + "</strong>项 多出型号，"; }

            T.Line_List = T.Line_List.OrderBy(x => x.Line_No).ToList();

            //扫描列表
            T.Scan_List = List_Scan.OrderByDescending(x => x.Create_DT).ToList();

            return T;
        }

        public void Reset_WMS_Move_Task_Scan(Guid Move_ID)
        {
            WMS_Move Move = db.WMS_Move.Find(Move_ID);
            Move = Move == null ? new WMS_Move() : Move;
            if (Move.Move_Status == WMS_Move_Status_Enum.已移库.ToString())
            {
                throw new Exception("该移库任务已完成，不支持重置扫描！");
            }

            if (db.WMS_Profit_Loss.Where(x => x.Link_TaskID == Move.Move_ID).Any())
            {
                throw new Exception("未删除盈亏记录，不支持重置扫描！");
            }

            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Move.Move_ID).ToList();
            db.WMS_Stocktaking_Scan.RemoveRange(Scan_List);
            MyDbSave.SaveChange(db);
        }

        public void Delete_WMS_Move_Item(Guid Move_ID)
        {
            WMS_Move Move = db.WMS_Move.Find(Move_ID);
            Move = Move == null ? new WMS_Move() : Move;
            if (Move == null) { throw new Exception("该移库任务不存在！"); }

            if (db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location_Type == WMS_Stock_Location_Type_Enum.临时库位.ToString() && x.Location == Move.Out_Location).Any())
            {
                throw new Exception("该库位中存在产品，不支持删除！");
            }

            if (db.WMS_Stocktaking_Scan.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Link_TaskID == Move.Move_ID).Any())
            {
                throw new Exception("该移库任务已进行底盘，不支持删除！");
            }

            if (Move.Move_Status == WMS_Move_Status_Enum.已移库.ToString())
            {
                throw new Exception("该移库任务已完成，不支持删除!");
            }

            List<WMS_Stocktaking> List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Move.Move_ID).ToList();
            db.WMS_Stocktaking.RemoveRange(List);
            db.WMS_Move.Remove(Move);

            MyDbSave.SaveChange(db);
        }

        public void Finish_WMS_Move_Stocktaking_Task(Guid Move_ID)
        {
            WMS_Move Move = db.WMS_Move.Find(Move_ID);
            Move = Move == null ? new WMS_Move() : Move;

            if (Move.Move_Status == WMS_Move_Status_Enum.已移库.ToString())
            {
                throw new Exception("此移库任务已完成，请勿重复操作");
            }

            List<WMS_Stocktaking> List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();
            List<WMS_Stocktaking_Scan> List_Scan = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();
            WMS_Stocktaking Stocktaking = List.FirstOrDefault();
            Stocktaking = Stocktaking == null ? new WMS_Stocktaking() : Stocktaking;

            List<WMS_Stock> Stock_List_With_Price = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Stocktaking.Location && x.MatSn == Stocktaking.MatSn).ToList();

            int Quantity_Sum = List.Sum(x => x.Quantity);
            int Quantity_Sum_Scan = List_Scan.Sum(x => x.Scan_Quantity);

            if (Quantity_Sum != Quantity_Sum_Scan)
            {
                WMS_Profit_Loss PL = new WMS_Profit_Loss();
                PL.Line_ID = MyGUID.NewGUID();
                PL.MatSn = Stocktaking.MatSn;
                PL.MatBrand = Stocktaking.MatBrand;
                PL.Old_Quantity = Quantity_Sum;
                PL.New_Quantity = Quantity_Sum_Scan;
                PL.Location = Stocktaking.Location;
                PL.Create_DT = DateTime.Now;
                PL.Link_TaskID = Move.Move_ID;
                PL.LinkMainCID = Move.LinkMainCID;
                PL.Price = Stock_List_With_Price.FirstOrDefault().Price;
                PL.Status = WMS_Profit_Loss_Status_Enum.未确定.ToString();
                db.WMS_Profit_Loss.Add(PL);

                if (db.WMS_Profit_Loss.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Profit_Loss_Status_Enum.未确定.ToString()).Any())
                {
                    throw new Exception("已存在盈亏记录未确定，不支持生成新的盈亏记录");
                }
            }
            else
            {
                //修改底盘状态
                foreach (var x in List)
                {
                    x.Status = WMS_Stocktaking_Status_Enum.已底盘.ToString();
                    db.Entry(x).State = EntityState.Modified;
                }

                //修改底盘扫描状态
                foreach (var x in List_Scan)
                {
                    x.Status = WMS_Stocktaking_Status_Enum.已底盘.ToString();
                    db.Entry(x).State = EntityState.Modified;
                }

                List<WMS_Move_Scan> Move_Scan_List = db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.In_Location == "").ToList();
                List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).ToList();

                List<WMS_Stock> Stock_List = new List<WMS_Stock>();
                WMS_Stock Stock = new WMS_Stock();

                //扫描产品确定库位
                foreach (var x in Move_Scan_List)
                {
                    //获取已拆托移库的列表
                    Stock = Stock_List_DB.Where(c => c.MatSn == x.MatSn && c.Quantity == x.Scan_Quantity).FirstOrDefault();
                    Stock.Location = Move.In_Location;
                    Stock.Package = x.Package_Type;
                    Stock.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                    db.Entry(Stock).State = EntityState.Modified;

                    x.In_Location = Move.In_Location;
                    db.Entry(x).State = EntityState.Modified;
                }

                MyDbSave.SaveChange(db);
                Thread.Sleep(500);

                //判断是否全部移库
                if (db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).Any() == false)
                {
                    Move.Move_Status = WMS_Move_Status_Enum.已移库.ToString();
                    db.Entry(Move).State = EntityState.Modified;
                }
            }
            MyDbSave.SaveChange(db);
        }

        public string Get_WMS_Move_Up_Line_QRCodePath(Guid StockID, int Quantity)
        {
            if (Quantity <= 0) { throw new Exception("产品数量不小于0"); }

            WMS_Stock Stock = db.WMS_Stock.Find(StockID);
            Stock = Stock == null ? new WMS_Stock() : Stock;
            string Str = "HONGEN/" + Stock.MatSn + "/" + Quantity + "/" + Stock.MatBrand;
            string QRCode_Path = QRCode.CreateQRCode_With_No(Str, Stock.Stock_ID);
            return QRCode_Path;
        }

        public string Get_WMS_Up_To_Stock_List_With_Excel(Guid Head_ID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.Wms_In_Head_ID == Head.Head_ID).ToList();

            var Group = from x in Stock_List
                        group x by new { x.MatSn, x.Location, x.Package } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Package = G.Key.Package,
                            Location = G.Key.Location,
                            Box_Count = G.Count(),
                            Quantity_Sum = G.Sum(c => c.Quantity)
                        };

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("序号");
            TableHeads.Add("产品型号");
            TableHeads.Add("库位");
            TableHeads.Add("箱数");
            TableHeads.Add("数量");
            TableHeads.Add("供应商");

            foreach (string TableHead in TableHeads)
            {
                //TableHead
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = TableHead;
                myDataColumn.ReadOnly = true;
                myDataColumn.Unique = false;  //获取或设置一个值，该值指示列的每一行中的值是否必须是唯一的。
                DT.Columns.Add(myDataColumn);
            }

            int i = 0;
            DataRow newRow;

            foreach (var x in Group)
            {
                i++;
                newRow = DT.NewRow();
                newRow["序号"] = i.ToString();
                newRow["产品型号"] = x.MatSn;
                newRow["库位"] = x.Location;

                if (x.Package == WMS_Stock_Package_Enum.整箱.ToString())
                {
                    newRow["箱数"] = x.Box_Count;
                }
                else
                {
                    newRow["箱数"] = "零头箱";
                }

                newRow["数量"] = x.Quantity_Sum;
                newRow["供应商"] = Head.Supplier_Name;
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public PageList<WMS_Move_Record> Get_WMS_Move_Record_PageList(WMS_Move_Filter MF)
        {
            var query = db.WMS_Move_Record.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Move_Type))
            {
                query = query.Where(x => x.Move_Type == MF.Move_Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Out_Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.In_Location))
            {
                query = query.Where(x => x.In_Location.Contains(MF.In_Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Person))
            {
                query = query.Where(x => x.Work_Person.Contains(MF.Work_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Time_Start) && !string.IsNullOrEmpty(MF.Time_End))
            {
                DateTime Start_DT = Convert.ToDateTime((MF.Time_Start));
                DateTime End_DT = Convert.ToDateTime((MF.Time_End));
                End_DT = End_DT.AddDays(1);
                if (DateTime.Compare(Start_DT, End_DT) > 0)
                {
                    throw new Exception("起始时间不可大于结束时间！");
                }

                query = query.Where(x => x.Create_DT >= Start_DT && x.Create_DT <= End_DT).AsQueryable();
            }

            List<WMS_Move_Record> Record_List = query.ToList();
            PageList<WMS_Move_Record> PList = new PageList<WMS_Move_Record>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Record_List.Count();
            PList.Rows = Record_List.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public void Finish_WMS_Move_Task(Guid MoveID)
        {
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            if (string.IsNullOrEmpty(Move.Work_Person)) { throw new Exception("未派工"); }

            Move.Move_Status = WMS_Move_Status_Enum.已移库.ToString();
            db.Entry(Move).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }
    }
}

//扫描枪交互
namespace SMART.Api
{
    public partial interface IWmsService
    {
        //收货作业
        DataTable WMS_In_Task_List_A(Guid MainCID);
        DataTable WMS_In_Task_List_B(Guid MainCID);
        DataTable WMS_In_Task_List_C(Guid MainCID);
        DataTable WMS_In_Task_List_D(Guid MainCID);

        DataTable WMS_In_Task_By_Tray_No(string HeadID, string Tray_No);
        DataTable WMS_In_Task_By_Tray_No_Other(string HeadID, string Tray_No, string Case_No);
        void WMS_In_Task_Scan_Item(string HeadID, string Tray_No, string Scan_Source, string Track_No);
        void WMS_In_Task_Scan_Item_Other(string HeadID, string Tray_No, string Scan_Source, string Track_No, string Quantity, string Case_No);

        void WMS_In_Track_Scan_Item(string HeadID, string Scan_Source, string Cost);
        DataTable WMS_In_Track_List(string HeadID);
        void WMS_In_Track_Scan_Delete(string HeadID);

        //上架作业   
        DataTable WMS_Up_List(Guid MainCID);
        DataTable WMS_Up_List_Sub(string Head_ID);
        void WMS_Up_Process(string Move_ID, string Scan_Source);
        DataTable WMS_Up_List_With_Location(string Move_ID);
        DataTable WMS_Up_Scan_List_With_Location(string Move_ID);
        void WMS_Up_Scan_Item_With_Location(string Move_ID, string Scan_Source);
        DataTable WMS_Up_Scan_List_With_Location_Other(string Move_ID);
        void WMS_Up_Scan_Item_With_Location_Other(string Move_ID, string Scan_Source, string Quantity);
        void WMS_Up_Scan_List_With_Location_Delete(string Move_ID);

        DataTable WMS_Up_Stocktaking_Scan_List(string Move_ID);
        void WMS_Up_Stocktaking_Scan_Item(string Move_ID, string Scan_Source);
        DataTable WMS_Up_Stocktaking_Scan_List_Other(string Move_ID);
        void WMS_Up_Stocktaking_Scan_Item_Other(string Move_ID, string Scan_Source, string Quantity);

        //移库作业   
        DataTable WMS_Move_List(Guid MainCID);
        void WMS_Move_Create(Guid MainCID, string Scan_Source);
    }

    //收货作业
    public partial class WmsService : IWmsService
    {
        //获取待扫码任务
        public DataTable WMS_In_Task_List_A(Guid MainCID)
        {
            WMS_In_Filter MF = new WMS_In_Filter();
            MF.LinkMainCID = MainCID;
            MF.Global_State = WMS_In_Global_State_Enum.等待收货.ToString();
            MF.PageIndex = 1;
            MF.PageSize = 100;
            MF.Logistics_Mode = Logistics_Mode_Enum.快递.ToString();
            List<WMS_In_Task> List_DB = this.Get_WMS_In_Task_PageList(MF).Rows;
            List<WMS_In_Task> List = new List<WMS_In_Task>();
            foreach (var x in List_DB)
            {
                if (!string.IsNullOrEmpty(x.Work_Person))
                {
                    List.Add(x);
                }
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("任务编号");
            dt.Columns.Add("型号数");
            dt.Columns.Add("到货数");
            dt.Columns.Add("行数");
            dt.Columns.Add("供应商");
            dt.Columns.Add("作业人");
            dt.Columns.Add("页码");
            dt.Columns.Add("运输方式");
            foreach (var x in List)
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Head_ID;
                dr["任务编号"] = x.Task_Bat_No_Str;
                dr["型号数"] = x.MatSn_Count;
                dr["到货数"] = x.Line_Quantity_Sum;
                dr["行数"] = x.Line_Count;
                if (x.Logistics_Cost_Type == Logistics_Cost_Type_Enum.有快递.ToString())
                {
                    dr["页码"] = 0;
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    dr["页码"] = 1;
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    dr["页码"] = 2;
                }

                dr["供应商"] = x.Supplier_Name;
                dr["作业人"] = x.Work_Person;
                dr["运输方式"] = x.Logistics_Mode;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable WMS_In_Task_List_B(Guid MainCID)
        {
            WMS_In_Filter MF = new WMS_In_Filter();
            MF.LinkMainCID = MainCID;
            MF.Global_State = WMS_In_Global_State_Enum.等待收货.ToString();
            MF.PageIndex = 1;
            MF.PageSize = 100;
            MF.Logistics_Mode = Logistics_Mode_Enum.物流.ToString();
            List<WMS_In_Task> List_DB = this.Get_WMS_In_Task_PageList(MF).Rows;
            List<WMS_In_Task> List = new List<WMS_In_Task>();
            foreach (var x in List_DB)
            {
                if (!string.IsNullOrEmpty(x.Work_Person))
                {
                    List.Add(x);
                }
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("任务编号");
            dt.Columns.Add("型号数");
            dt.Columns.Add("到货数");
            dt.Columns.Add("行数");
            dt.Columns.Add("供应商");
            dt.Columns.Add("作业人");
            dt.Columns.Add("页码");
            dt.Columns.Add("运输方式");
            foreach (var x in List)
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Head_ID;
                dr["任务编号"] = x.Task_Bat_No_Str;
                dr["型号数"] = x.MatSn_Count;
                dr["到货数"] = x.Line_Quantity_Sum;
                dr["行数"] = x.Line_Count;
                if (x.Logistics_Cost_Type == Logistics_Cost_Type_Enum.有快递.ToString())
                {
                    dr["页码"] = 0;
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    dr["页码"] = 1;
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    dr["页码"] = 2;
                }

                dr["供应商"] = x.Supplier_Name;
                dr["作业人"] = x.Work_Person;
                dr["运输方式"] = x.Logistics_Mode;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable WMS_In_Task_List_C(Guid MainCID)
        {
            WMS_In_Filter MF = new WMS_In_Filter();
            MF.LinkMainCID = MainCID;
            MF.Global_State = WMS_In_Global_State_Enum.等待收货.ToString();
            MF.PageIndex = 1;
            MF.PageSize = 100;
            MF.Logistics_Mode = Logistics_Mode_Enum.自提.ToString();
            List<WMS_In_Task> List_DB = this.Get_WMS_In_Task_PageList(MF).Rows;
            List<WMS_In_Task> List = new List<WMS_In_Task>();
            foreach (var x in List_DB)
            {
                if (!string.IsNullOrEmpty(x.Work_Person))
                {
                    List.Add(x);
                }
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("任务编号");
            dt.Columns.Add("型号数");
            dt.Columns.Add("到货数");
            dt.Columns.Add("行数");
            dt.Columns.Add("供应商");
            dt.Columns.Add("作业人");
            dt.Columns.Add("页码");
            dt.Columns.Add("运输方式");
            foreach (var x in List)
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Head_ID;
                dr["任务编号"] = x.Task_Bat_No_Str;
                dr["型号数"] = x.MatSn_Count;
                dr["到货数"] = x.Line_Quantity_Sum;
                dr["行数"] = x.Line_Count;
                if (x.Logistics_Cost_Type == Logistics_Cost_Type_Enum.有快递.ToString())
                {
                    dr["页码"] = 0;
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    dr["页码"] = 1;
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    dr["页码"] = 2;
                }

                dr["供应商"] = x.Supplier_Name;
                dr["作业人"] = x.Work_Person;
                dr["运输方式"] = x.Logistics_Mode;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable WMS_In_Task_List_D(Guid MainCID)
        {
            WMS_In_Filter MF = new WMS_In_Filter();
            MF.LinkMainCID = MainCID;
            MF.Global_State = WMS_In_Global_State_Enum.等待收货.ToString();
            MF.PageIndex = 1;
            MF.PageSize = 100;
            MF.Logistics_Mode = Logistics_Mode_Enum.自送.ToString();
            List<WMS_In_Task> List_DB = this.Get_WMS_In_Task_PageList(MF).Rows;
            List<WMS_In_Task> List = new List<WMS_In_Task>();
            foreach (var x in List_DB)
            {
                if (!string.IsNullOrEmpty(x.Work_Person))
                {
                    List.Add(x);
                }
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("任务编号");
            dt.Columns.Add("型号数");
            dt.Columns.Add("到货数");
            dt.Columns.Add("行数");
            dt.Columns.Add("供应商");
            dt.Columns.Add("作业人");
            dt.Columns.Add("页码");
            dt.Columns.Add("运输方式");
            foreach (var x in List)
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Head_ID;
                dr["任务编号"] = x.Task_Bat_No_Str;
                dr["型号数"] = x.MatSn_Count;
                dr["到货数"] = x.Line_Quantity_Sum;
                dr["行数"] = x.Line_Count;
                if (x.Logistics_Cost_Type == Logistics_Cost_Type_Enum.有快递.ToString())
                {
                    dr["页码"] = 0;
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    dr["页码"] = 1;
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    dr["页码"] = 2;
                }

                dr["供应商"] = x.Supplier_Name;
                dr["作业人"] = x.Work_Person;
                dr["运输方式"] = x.Logistics_Mode;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //整箱
        public DataTable WMS_In_Task_By_Tray_No(string HeadID, string Tray_No)
        {
            Guid Head_ID = Guid.NewGuid();
            try { Head_ID = new Guid(HeadID); } catch { }
            List<WMS_In_Scan> List = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head_ID && x.Tray_No == Tray_No && x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).OrderByDescending(x => x.Create_DT).ToList();

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序号");
            dt.Columns.Add("产品型号");
            dt.Columns.Add("数量");
            int i = 0;
            foreach (var x in List)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序号"] = i;
                dr["产品型号"] = x.MatSn;
                dr["数量"] = x.Scan_Quantity;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //端数
        public DataTable WMS_In_Task_By_Tray_No_Other(string HeadID, string Tray_No, string Case_No)
        {
            Guid Head_ID = Guid.NewGuid();
            try { Head_ID = new Guid(HeadID); } catch { }
            List<WMS_In_Scan> List = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head_ID && x.Tray_No == Tray_No && x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).OrderByDescending(x => x.Create_DT).ToList();
            if (!string.IsNullOrEmpty(Case_No))
            {
                List = List.Where(x => x.Case_No == Case_No).ToList();
            }
            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序号");
            dt.Columns.Add("产品型号");
            dt.Columns.Add("数量");
            int i = 0;
            foreach (var x in List)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序号"] = i;
                dr["产品型号"] = x.MatSn;
                dr["数量"] = x.Scan_Quantity;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //整箱扫描
        public void WMS_In_Task_Scan_Item(string HeadID, string Tray_No, string Scan_Source, string Track_No)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid Link_Head_ID = new Guid(HeadID);
            WMS_In_Head Head = db.WMS_In_Head.Find(Link_Head_ID);
            if (Head == null) { throw new Exception("任务编号未匹配"); }

            Tray_No = Tray_No == null ? string.Empty : Tray_No.Trim();
            if (string.IsNullOrEmpty(Tray_No))
            {
                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    throw new Exception("未填写托盘号");
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    throw new Exception("未填写箱号");
                }
            }

            int TrayNo = 0;
            try
            {
                TrayNo = Convert.ToInt32(Tray_No.Trim());
            }
            catch
            {
                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    throw new Exception("托盘号必须为数字");
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    throw new Exception("箱号必须为数字");
                }
            }

            if (string.IsNullOrEmpty(Head.Work_Person)) { throw new Exception("未派工"); }

            if (Head.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("已执行完成入库，请勿重复操作");
            }

            List<string> Line_MatSn_List = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head.Head_ID).Select(x => x.MatSn).ToList();
            Line_MatSn_List = Line_MatSn_List.Distinct().ToList();

            WMS_In_Scan Scan = new WMS_In_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.Create_DT = DateTime.Now;
            Scan.Link_Head_ID = Head.Head_ID;
            Scan.LinkMainCID = Head.LinkMainCID;
            Scan.Scan_Source = Scan_Source;
            Scan.Tray_No = Tray_No;
            Scan.Track_No = Track_No;
            Scan.Package_Type = WMS_Stock_Package_Enum.整箱.ToString();

            //执行解码
            Decode_Scan De_Scan = new Decode_Scan();
            string MatBrand = string.Empty;
            if (Head.Brand != Decode_Scan_Brand.NSK.ToString() && Head.Brand != Decode_Scan_Brand.NMB.ToString())
            {
                List<string> Brand_List = new List<string>();
                Brand_List.Add(Decode_Scan_Brand.NSK.ToString());
                Brand_List.Add(Decode_Scan_Brand.NMB.ToString());

                foreach (var Brand in Brand_List)
                {
                    De_Scan = this.Decode_Scan_Source(Brand, Scan.Scan_Source, Line_MatSn_List);
                    if (De_Scan.Is_Scan_Error == false)
                    {
                        MatBrand = Brand;
                        break;
                    }
                }
            }
            else
            {
                MatBrand = Head.Brand;
            }

            //执行解码
            if (string.IsNullOrEmpty(MatBrand))
            {
                throw new Exception("Error，未识别二维码『" + Scan_Source + "』");
            }
            else
            {
                De_Scan = this.Decode_Scan_Source(MatBrand, Scan.Scan_Source, Line_MatSn_List);
                if (De_Scan.Is_Scan_Error)
                {
                    this.WMS_In_Track_Scan_Item_Scan_Error_Add(Scan);
                    MyDbSave.SaveChange(db);
                }
                else
                {
                    Scan.MatSn = De_Scan.Decode_MatSn;
                    Scan.Scan_Quantity = De_Scan.Decode_Scan_Quantity;
                    db.WMS_In_Scan.Add(Scan);
                    MyDbSave.SaveChange(db);
                }
            }

            if (De_Scan.Is_Scan_Error)
            {
                throw new Exception("Error，未识别二维码『" + Scan_Source + "』");
            }
        }

        //端数扫描
        public void WMS_In_Task_Scan_Item_Other(string HeadID, string Tray_No, string Scan_Source, string Track_No, string Quantity, string Case_No)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid Link_Head_ID = new Guid(HeadID);
            WMS_In_Head Head = db.WMS_In_Head.Find(Link_Head_ID);
            if (Head == null) { throw new Exception("任务编号未匹配"); }

            Track_No = Track_No == null ? string.Empty : Track_No.Trim();
            if (Head.Logistics_Cost_Type == Logistics_Cost_Type_Enum.有快递.ToString() && string.IsNullOrEmpty(Track_No))
            {
                throw new Exception("未获取快递单号");
            }

            Case_No = Case_No == null ? string.Empty : Case_No.Trim();
            if (string.IsNullOrEmpty(Case_No) && Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
            {
                throw new Exception("未填写箱号");
            }

            if (!string.IsNullOrEmpty(Case_No) && Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
            {
                int CaseNo = 0;
                try
                {
                    CaseNo = Convert.ToInt32(Case_No.Trim());
                }
                catch
                {
                    throw new Exception("箱号必须为数字");
                }
            }

            Tray_No = Tray_No == null ? string.Empty : Tray_No.Trim();
            if (string.IsNullOrEmpty(Tray_No))
            {
                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    throw new Exception("未填写托盘号");
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    throw new Exception("未填写箱号");
                }
            }

            int TrayNo = 0;
            try
            {
                TrayNo = Convert.ToInt32(Tray_No.Trim());
            }
            catch
            {
                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    throw new Exception("托盘号必须为数字");
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    throw new Exception("箱号必须为数字");
                }
            }

            if (string.IsNullOrEmpty(Head.Work_Person)) { throw new Exception("未派工"); }

            if (Head.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("已执行完成入库，请勿重复操作");
            }

            List<string> Line_MatSn_List = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head.Head_ID).Select(x => x.MatSn).Distinct().ToList();

            WMS_In_Scan Scan = new WMS_In_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.Create_DT = DateTime.Now;
            Scan.Link_Head_ID = Head.Head_ID;
            Scan.LinkMainCID = Head.LinkMainCID;
            Scan.Scan_Source = Scan_Source;
            Scan.Tray_No = Tray_No;
            Scan.Track_No = Track_No;
            Scan.Case_No = Case_No;
            Scan.Package_Type = WMS_Stock_Package_Enum.零头.ToString();

            bool Flag_Success = false;
            foreach (var MatSn in Line_MatSn_List)
            {
                if (Scan_Source == MatSn)
                {
                    Scan.MatSn = MatSn;
                    Flag_Success = true;
                    break;
                }
            }

            //无法识别，则直接进行报错
            if (Flag_Success == false) { throw new Exception("Error，未识别二维码『" + Scan.Scan_Source + "』"); }

            Quantity = Quantity == null ? string.Empty : Quantity.Trim();
            if (string.IsNullOrEmpty(Quantity)) { throw new Exception("未填写数量"); }

            try
            {
                Scan.Scan_Quantity = Convert.ToInt32(Quantity);
                if (Scan.Scan_Quantity <= 0) { throw new Exception("未填入正确的数字"); }
            }
            catch { throw new Exception("填入数量的格式错误"); }

            if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString() && db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No && x.MatSn == Scan.MatSn).Any())
            {
                throw new Exception("该箱中已存在相同型号的产品");
            }

            db.WMS_In_Scan.Add(Scan);
            MyDbSave.SaveChange(db);
        }

        //获取快递单列表
        public DataTable WMS_In_Track_List(string HeadID)
        {
            Guid Head_ID = new Guid(HeadID);
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("任务编号未匹配"); }

            List<WMS_Track> List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            DataTable dt = new DataTable("List");
            dt.Columns.Add("序号");
            dt.Columns.Add("快递单号");
            dt.Columns.Add("金额");
            dt.Columns.Add("页码");
            int i = 0;
            foreach (var x in List.OrderByDescending(x => x.Create_DT).ToList())
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序号"] = i;
                dr["快递单号"] = x.Tracking_No;
                dr["金额"] = x.Logistics_Cost.ToString("N2");
                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    dr["页码"] = 1;
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    dr["页码"] = 2;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //快递单扫描
        public void WMS_In_Track_Scan_Item(string HeadID, string Scan_Source, string Cost)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            decimal Logistics_Cost = 0;
            try { Logistics_Cost = Convert.ToDecimal(Cost.Trim()); } catch { Logistics_Cost = 0; }
            Logistics_Cost = Logistics_Cost <= 0 ? 0 : Logistics_Cost;

            Guid Head_ID = new Guid(HeadID);
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("任务编号未匹配"); }

            if (string.IsNullOrEmpty(Head.Work_Person)) { throw new Exception("未派工"); }

            if (Head.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("已执行完成入库，请勿重复操作");
            }

            List<WMS_Track> OLD_Track_List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            if (OLD_Track_List.Where(x => x.Tracking_No == Scan_Source).Any()) { throw new Exception("此快递单已扫描"); }

            WMS_Track T = new WMS_Track();
            T.Tracking_ID = MyGUID.NewGUID();
            T.Logistics_Company = Head.Logistics_Company;
            T.Logistics_Mode = Head.Logistics_Mode;
            T.Logistics_Cost = Logistics_Cost;
            T.Tracking_No = Scan_Source;
            T.Scan_PDA_Date = DateTime.Now;
            T.LinkMainCID = Head.LinkMainCID;
            T.Link_Head_ID = Head.Head_ID;
            T.Create_DT = DateTime.Now;
            T.Link_Head_Com_Name = Head.Supplier_Name;
            T.Tracking_Type = Tracking_Type_Enum.收货.ToString();
            db.WMS_Track.Add(T);
            MyDbSave.SaveChange(db);
        }

        //快递单删除
        public void WMS_In_Track_Scan_Delete(string HeadID)
        {
            Guid Head_ID = new Guid(HeadID);
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("任务编号未匹配"); }

            if (Head.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("已执行完成入库，请勿重复操作");
            }

            List<WMS_Track> List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            List<string> Track_No_List = List.Select(x => x.Tracking_No).Distinct().ToList();

            //若绑定扫描产品则需要进行判断
            if (db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && Track_No_List.Contains(x.Track_No)).Any())
            {
                throw new Exception("存在扫描产品绑定快递单号");
            }

            if (List.Any())
            {
                db.WMS_Track.RemoveRange(List);
                MyDbSave.SaveChange(db);
            }
        }

        //未识别的二维码
        private void WMS_In_Track_Scan_Item_Scan_Error_Add(WMS_In_Scan Scan)
        {
            WMS_In_Scan_Error E = new WMS_In_Scan_Error();
            E.Scan_EID = MyGUID.NewGUID();
            E.Create_DT = DateTime.Now;
            E.Scan_Source = Scan.Scan_Source;
            E.Link_Head_ID = Scan.Link_Head_ID;
            E.LinkMainCID = Scan.LinkMainCID;
            if (db.WMS_In_Scan_Error.Where(x => x.Link_Head_ID == Scan.Link_Head_ID && x.Scan_Source == Scan.Scan_Source).Any() == false)
            {
                db.WMS_In_Scan_Error.Add(E);
            }
        }
    }

    //上架作业
    public partial class WmsService : IWmsService
    {
        public DataTable WMS_Up_List(Guid MainCID)
        {
            WMS_In_Filter MF = new WMS_In_Filter();
            MF.PageIndex = 1;
            MF.PageSize = 10000;
            MF.LinkMainCID = MainCID;
            MF.Global_State = WMS_Move_Status_Enum.待移库.ToString();
            List<WMS_In_Task> Up_List = this.Get_WMS_Up_PageList(MF).Rows;

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("任务批次");
            dt.Columns.Add("产品型号");
            dt.Columns.Add("产品数量");
            dt.Columns.Add("作业人");
            foreach (var x in Up_List)
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Head_ID;
                dr["任务批次"] = x.Task_Bat_No_Str;
                dr["产品型号"] = x.MatSn_Count;
                dr["产品数量"] = x.Line_Quantity_Sum;
                dr["作业人"] = x.Work_Person;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public DataTable WMS_Up_List_Sub(string Head_ID)
        {
            Guid HeadID = Guid.NewGuid();
            try { HeadID = new Guid(Head_ID); } catch { }
            WMS_In_Head Head = db.WMS_In_Head.Find(HeadID);
            if (Head == null) { throw new Exception("该批次任务不存在上架"); }

            WMS_Move_Filter MF = new WMS_Move_Filter();
            MF.PageSize = 10000;
            MF.Link_HeadID = Head.Head_ID;
            MF.Move_Status = WMS_Move_Status_Enum.待移库.ToString();
            List<WMS_Move> Move_List_DB = this.Get_WMS_Up_PageList(MF).Rows;
            List<Guid> Guid_List = Move_List_DB.Select(x => x.Move_ID).Distinct().ToList();
            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => Guid_List.Contains(x.Link_TaskID) && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();

            List<WMS_Move> Move_List = new List<WMS_Move>();
            foreach (var x in Move_List_DB)
            {
                if (!string.IsNullOrEmpty(x.Work_Person))
                {
                    Move_List.Add(x);
                }
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("移出库位");
            dt.Columns.Add("是否底盘");
            dt.Columns.Add("底盘库位");
            dt.Columns.Add("页码");

            foreach (var x in Move_List.OrderBy(x => x.Out_Location))
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Move_ID;
                dr["移出库位"] = x.Out_Location;

                if (Stocktaking_List.Where(c => c.Link_TaskID == x.Move_ID).Any())
                {
                    dr["底盘库位"] = Stocktaking_List.Where(c => c.Link_TaskID == x.Move_ID).FirstOrDefault().Location;
                    dr["是否底盘"] = "是";
                    dr["页码"] = 1;
                }
                else
                {
                    dr["底盘库位"] = "";
                    dr["是否底盘"] = "";
                    dr["页码"] = 0;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public void WMS_Up_Process(string Move_ID, string Scan_Source)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未扫描移入库位"); }

            Guid MoveID = Guid.NewGuid();
            try { MoveID = new Guid(Move_ID); } catch { }
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("任务不存在"); }

            if (string.IsNullOrEmpty(Move.Work_Person) && !string.IsNullOrEmpty(Move.Task_Bat_No)) { throw new Exception("未派工"); }

            if (db.WMS_Location.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Scan_Source).Any() == false)
            {
                throw new Exception("系统库位中不存在该移入库位");
            }

            if (db.WMS_Stocktaking.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).Any())
            {
                throw new Exception("存在底盘任务未扫描");
            }

            List<WMS_Stock> Stock_List_DB = new List<WMS_Stock>();
            if (string.IsNullOrEmpty(Move.Task_Bat_No))
            {
                Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).ToList();
            }
            else
            {
                Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Wms_In_Head_ID == Move.Link_HeadID && x.Location == Move.Out_Location).ToList();
            }

            List<WMS_Move_Scan> Move_Scan_List = new List<WMS_Move_Scan>();
            if (db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Move_Status_Enum.待移库.ToString()).Any())
            {
                Move_Scan_List = db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Move_Status_Enum.待移库.ToString()).ToList();
                List<WMS_Stock> Stock_List = new List<WMS_Stock>();
                WMS_Stock Stock = new WMS_Stock();

                foreach (var x in Move_Scan_List)
                {
                    //获取已拆托移库的列表
                    Stock = Stock_List_DB.Where(c => c.MatSn == x.MatSn && c.Quantity == x.Scan_Quantity).FirstOrDefault();
                    if (Stock == null)
                    {
                        throw new Exception("未找到相同数量的产品");
                    }
                    Stock_List.Add(Stock);
                    Stock_List_DB.Remove(Stock);
                }

                Finish_WMS_Up_Process(Move, Stock_List, Move_Scan_List, Scan_Source);
            }
            else
            {
                Finish_WMS_Up_Process(Move, Stock_List_DB, Move_Scan_List, Scan_Source);
            }

            if (string.IsNullOrEmpty(Move.In_Location))
            {
                Move.In_Location = Scan_Source;
            }
            else
            {
                Move.In_Location = Move.In_Location + "," + Scan_Source;
            }

            db.Entry(Move).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        private void Finish_WMS_Up_Process(WMS_Move Move, List<WMS_Stock> Stock_List, List<WMS_Move_Scan> Move_Scan_List, string Scan_Source)
        {
            //List<string> MatSn_List = new List<string>();
            //if (Move_Scan_List.Any())
            //{
            //    MatSn_List = Move_Scan_List.Select(x => x.MatSn).Distinct().ToList();
            //}
            //else
            //{
            //    MatSn_List = Stock_List.Select(x => x.MatSn).Distinct().ToList();
            //}

            ////判断移入库位是否存在相同型号产品
            //if (db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Scan_Source && MatSn_List.Contains(x.MatSn)).Any())
            //{
            //    //创建底盘信息
            //    List<WMS_Stocktaking> Stocktaking_List = new List<WMS_Stocktaking>();
            //    WMS_Stocktaking Stocktaking = new WMS_Stocktaking();

            //    List<WMS_Stock> Stock_List_In = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Scan_Source).ToList();
            //    Stock_List_In = Stock_List_In.Where(x => MatSn_List.Contains(x.MatSn)).ToList();

            //    DateTime Create_DT = DateTime.Now;
            //    foreach (var x in Stock_List_In)
            //    {
            //        Stocktaking = new WMS_Stocktaking();
            //        Stocktaking.Stocktaking_ID = MyGUID.NewGUID();
            //        Stocktaking.MatSn = x.MatSn;
            //        Stocktaking.MatBrand = x.MatBrand;
            //        Stocktaking.Quantity = x.Quantity;
            //        Stocktaking.Location = x.Location;
            //        Stocktaking.Create_DT = Create_DT;
            //        Stocktaking.Link_TaskID = Move.Move_ID;
            //        Stocktaking.LinkMainCID = Move.LinkMainCID;
            //        Stocktaking.Task_Bat_No = Move.Task_Bat_No;
            //        Stocktaking.Work_Person = Move.Work_Person;
            //        Stocktaking.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
            //        Stocktaking_List.Add(Stocktaking);
            //    }

            //    if (Stocktaking_List.Any())
            //    {
            //        db.WMS_Stocktaking.AddRange(Stocktaking_List);
            //    }
            //}

            if (Move_Scan_List.Any())
            {
                //扫描产品更新
                foreach (var x in Move_Scan_List)
                {
                    x.In_Location = Scan_Source;
                    x.Status = WMS_Move_Status_Enum.已移库.ToString();
                    db.Entry(x).State = EntityState.Modified;
                }
            }

            List<WMS_Move_Record> Record_List = new List<WMS_Move_Record>();
            WMS_Move_Record Record = new WMS_Move_Record();

            DateTime DT = DateTime.Now;

            //库存产品变换库位
            foreach (var x in Stock_List)
            {
                //上架移库记录
                Record = new WMS_Move_Record();
                Record.Record_ID = MyGUID.NewGUID();
                Record.Out_Location = x.Location;
                Record.In_Location = Scan_Source;
                Record.Create_DT = DT;
                Record.LinkMainCID = x.LinkMainCID;
                Record.Work_Person = Move.Work_Person;
                Record.Link_TaskID = Move.Move_ID;
                Record.MatSn = x.MatSn;
                Record.Quantity = x.Quantity;
                Record.Package_Type = x.Package;
                Record.MatName = x.MatName;
                Record.MatBrand = x.MatBrand;
                Record.MatUnit = x.MatUnit;
                if (Move.Link_HeadID == Guid.Empty)
                {
                    Record.Move_Type = WMS_Move_Type_Enum.移库作业.ToString();
                }
                else
                {
                    Record.Move_Type = WMS_Move_Type_Enum.上架作业.ToString();
                }
                Record_List.Add(Record);

                x.Location = Scan_Source;
                x.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                db.Entry(x).State = EntityState.Modified;
            }

            db.WMS_Move_Record.AddRange(Record_List);
            MyDbSave.SaveChange(db);
            Thread.Sleep(500);

            //移库任务变换状态
            if (db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).Any() == false)
            {
                Move.Move_Status = WMS_Move_Status_Enum.已移库.ToString();
            }

        }

        public DataTable WMS_Up_List_With_Location(string Move_ID)
        {
            Guid MoveID = Guid.NewGuid();
            try { MoveID = new Guid(Move_ID); } catch { }
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).ToList();

            List<string> MatSn_List = Stock_List_DB.Select(x => x.MatSn).Distinct().ToList();

            List<WMS_Stock> Stock_List = new List<WMS_Stock>();
            WMS_Stock Stock = new WMS_Stock();

            foreach (var MatSn in MatSn_List)
            {
                Stock = new WMS_Stock();
                Stock.MatSn = MatSn;
                Stock.Quantity = Stock_List_DB.Where(c => c.MatSn == MatSn).Sum(c => c.Quantity);
                Stock_List.Add(Stock);
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序");
            dt.Columns.Add("产品型号");
            dt.Columns.Add("产品数");

            int i = 0;
            foreach (var x in Stock_List)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序"] = i;
                dr["产品型号"] = x.MatSn;
                dr["产品数"] = x.Quantity;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //拆托扫描列表
        public DataTable WMS_Up_Scan_List_With_Location(string Move_ID)
        {
            Guid MoveID = Guid.NewGuid();
            try { MoveID = new Guid(Move_ID); } catch { }
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            List<WMS_Move_Scan> Move_Scan_List_DB = db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.In_Location == "" && x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).ToList();
            List<string> MatSn_List = Move_Scan_List_DB.OrderByDescending(x => x.Create_DT).Select(x => x.MatSn).Distinct().ToList();

            List<WMS_Move_Scan> Move_Scan_List = new List<WMS_Move_Scan>();
            WMS_Move_Scan Move_Scan = new WMS_Move_Scan();

            foreach (var MatSn in MatSn_List)
            {
                Move_Scan = new WMS_Move_Scan();
                Move_Scan.MatSn = MatSn;
                Move_Scan.Scan_Quantity = Move_Scan_List_DB.Where(c => c.MatSn == MatSn).Sum(c => c.Scan_Quantity);
                Move_Scan_List.Add(Move_Scan);
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序");
            dt.Columns.Add("产品型号");
            dt.Columns.Add("扫描数");

            int i = 0;
            foreach (var x in Move_Scan_List)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序"] = i;
                dr["产品型号"] = x.MatSn;
                dr["扫描数"] = x.Scan_Quantity;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //拆托扫描
        public void WMS_Up_Scan_Item_With_Location(string Move_ID, string Scan_Source)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未扫描产品"); }

            Guid MoveID = Guid.NewGuid();
            try { MoveID = new Guid(Move_ID); } catch { }
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            if (string.IsNullOrEmpty(Move.Work_Person) && !string.IsNullOrEmpty(Move.Task_Bat_No)) { throw new Exception("未派工"); }

            List<WMS_Stock> Stock_List_DB = Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).ToList();

            List<string> Line_MatSn_List = Stock_List_DB.Select(x => x.MatSn).Distinct().ToList();

            WMS_Move_Scan Scan = new WMS_Move_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.Create_DT = DateTime.Now;
            Scan.Link_TaskID = Move.Move_ID;
            Scan.LinkMainCID = Move.LinkMainCID;
            Scan.MatBrand = Stock_List_DB.FirstOrDefault().MatBrand;
            Scan.Out_Location = Move.Out_Location;
            Scan.Scan_Source = Scan_Source;
            Scan.Package_Type = WMS_Stock_Package_Enum.整箱.ToString();
            Scan.Status = WMS_Move_Status_Enum.待移库.ToString();

            //执行解码
            Decode_Scan De_Scan = this.Decode_Scan_Source(Stock_List_DB.FirstOrDefault().MatBrand, Scan_Source, Line_MatSn_List);
            if (De_Scan.Is_Scan_Error)
            {
                throw new Exception("Error，未识别二维码『" + Scan_Source + "』");
            }
            else
            {
                Scan.MatSn = De_Scan.Decode_MatSn;
                Scan.Scan_Quantity = De_Scan.Decode_Scan_Quantity;

                if (Stock_List_DB.Where(x => x.MatSn == Scan.MatSn).Any() == false)
                {
                    throw new Exception("移出库位中不存在此产品");
                }

                int Scan_Qty = 0;
                int Qty_DB = 0;

                List<WMS_Move_Scan> Move_Scan_List = db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.MatSn == Scan.MatSn && x.In_Location == "").ToList();
                Scan_Qty = Move_Scan_List.Sum(x => x.Scan_Quantity) + Scan.Scan_Quantity;
                Qty_DB = Stock_List_DB.Where(x => x.MatSn == Scan.MatSn).Sum(x => x.Quantity);

                if (Scan_Qty > Qty_DB)
                {
                    throw new Exception("移出库位中此产品数量已达到系统数量");
                }

                db.WMS_Move_Scan.Add(Scan);
                MyDbSave.SaveChange(db);
            }

        }

        //拆托扫描列表（端数）
        public DataTable WMS_Up_Scan_List_With_Location_Other(string Move_ID)
        {
            Guid MoveID = Guid.NewGuid();
            try { MoveID = new Guid(Move_ID); } catch { }
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            List<WMS_Move_Scan> Move_Scan_List_DB = db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.In_Location == "" && x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).ToList();
            List<string> MatSn_List = Move_Scan_List_DB.OrderByDescending(x => x.Create_DT).Select(x => x.MatSn).Distinct().ToList();

            List<WMS_Move_Scan> Move_Scan_List = new List<WMS_Move_Scan>();
            WMS_Move_Scan Move_Scan = new WMS_Move_Scan();

            foreach (var MatSn in MatSn_List)
            {
                Move_Scan = new WMS_Move_Scan();
                Move_Scan.MatSn = MatSn;
                Move_Scan.Scan_Quantity = Move_Scan_List_DB.Where(c => c.MatSn == MatSn).Sum(c => c.Scan_Quantity);
                Move_Scan_List.Add(Move_Scan);
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序");
            dt.Columns.Add("产品型号");
            dt.Columns.Add("扫描数");

            int i = 0;
            foreach (var x in Move_Scan_List)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序"] = i;
                dr["产品型号"] = x.MatSn;
                dr["扫描数"] = x.Scan_Quantity;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //拆托扫描（端数）
        public void WMS_Up_Scan_Item_With_Location_Other(string Move_ID, string Scan_Source, string Quantity)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未扫描产品"); }

            Guid MoveID = Guid.NewGuid();
            try { MoveID = new Guid(Move_ID); } catch { }
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            if (string.IsNullOrEmpty(Move.Work_Person) && !string.IsNullOrEmpty(Move.Task_Bat_No)) { throw new Exception("未派工"); }

            List<WMS_Stock> Stock_List_DB = Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).ToList();

            List<string> Line_MatSn_List = Stock_List_DB.Select(x => x.MatSn).Distinct().ToList();

            WMS_Move_Scan Scan = new WMS_Move_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.Create_DT = DateTime.Now;
            Scan.Link_TaskID = Move.Move_ID;
            Scan.LinkMainCID = Move.LinkMainCID;
            Scan.MatBrand = Stock_List_DB.FirstOrDefault().MatBrand;
            Scan.Out_Location = Move.Out_Location;
            Scan.Scan_Source = Scan_Source;
            Scan.Package_Type = WMS_Stock_Package_Enum.零头.ToString();
            Scan.Status = WMS_Move_Status_Enum.待移库.ToString();

            bool Flag_Success = false;
            foreach (var MatSn in Line_MatSn_List)
            {
                if (Scan_Source == MatSn)
                {
                    Scan.MatSn = MatSn;
                    Flag_Success = true;
                    break;
                }
            }

            //无法识别，则直接进行报错
            if (Flag_Success == false) { throw new Exception("Error，未识别二维码『" + Scan.Scan_Source + "』"); }

            try
            {
                Scan.Scan_Quantity = Convert.ToInt32(Quantity);
                if (Scan.Scan_Quantity <= 0) { throw new Exception("未填入正确的数字"); }
            }
            catch { throw new Exception("填入数量的格式错误"); }

            if (Stock_List_DB.Where(x => x.MatSn == Scan.MatSn).Any() == false)
            {
                throw new Exception("移出库位中不存在此产品");
            }

            int Scan_Qty = 0;
            int Qty_DB = 0;

            List<WMS_Move_Scan> Move_Scan_List = db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.MatSn == Scan.MatSn && x.In_Location == "").ToList();
            Scan_Qty = Move_Scan_List.Sum(x => x.Scan_Quantity) + Scan.Scan_Quantity;
            Qty_DB = Stock_List_DB.Where(x => x.MatSn == Scan.MatSn).Sum(x => x.Quantity);

            if (Scan_Qty > Qty_DB)
            {
                throw new Exception("移出库位中此产品数量已达到系统数量");
            }

            db.WMS_Move_Scan.Add(Scan);
            MyDbSave.SaveChange(db);
        }

        //拆托扫描清空
        public void WMS_Up_Scan_List_With_Location_Delete(string Move_ID)
        {
            Guid MoveID = Guid.NewGuid();
            try { MoveID = new Guid(Move_ID); } catch { }
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            if (string.IsNullOrEmpty(Move.Work_Person) && !string.IsNullOrEmpty(Move.Task_Bat_No)) { throw new Exception("未派工"); }

            List<WMS_Move_Scan> Move_Scan_List = db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID && x.In_Location == "").ToList();

            if (Move_Scan_List.Any())
            {
                db.WMS_Move_Scan.RemoveRange(Move_Scan_List);
                MyDbSave.SaveChange(db);
            }
        }

        public DataTable WMS_Up_Stocktaking_Scan_List(string Move_ID)
        {
            Guid MoveID = Guid.NewGuid();
            try { MoveID = new Guid(Move_ID); } catch { }

            List<WMS_Stocktaking_Scan> List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == MoveID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString() && x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).OrderByDescending(x => x.Create_DT).ToList();

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序号");
            dt.Columns.Add("产品型号");
            dt.Columns.Add("数量");
            int i = 0;
            foreach (var x in List.OrderByDescending(x => x.Create_DT))
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序号"] = i;
                dr["产品型号"] = x.MatSn;
                dr["数量"] = x.Scan_Quantity;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //底盘扫描
        public void WMS_Up_Stocktaking_Scan_Item(string Move_ID, string Scan_Source)
        {
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid MoveID = new Guid(Move_ID);
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            if (string.IsNullOrEmpty(Move.Work_Person)) { throw new Exception("未派工"); }

            if (string.IsNullOrEmpty(Move.In_Location))
            {
                throw new Exception("移入库位不存在");
            }

            if (Move.Move_Status == WMS_Move_Status_Enum.已移库.ToString())
            {
                throw new Exception("已执行移库，请勿重复操作");
            }

            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();
            List<string> Line_MatSn_List = Stocktaking_List.Select(x => x.MatSn).Distinct().ToList();

            WMS_Stocktaking_Scan Scan = new WMS_Stocktaking_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.Create_DT = DateTime.Now;
            Scan.Link_TaskID = Move.Move_ID;
            Scan.LinkMainCID = Move.LinkMainCID;
            Scan.MatBrand = Stocktaking_List.FirstOrDefault().MatBrand;
            Scan.Location = Stocktaking_List.FirstOrDefault().Location;
            Scan.Scan_Source = Scan_Source;
            Scan.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
            Scan.Package_Type = WMS_Stock_Package_Enum.整箱.ToString();

            //执行解码
            Decode_Scan De_Scan = this.Decode_Scan_Source(Stocktaking_List.FirstOrDefault().MatBrand, Scan_Source, Line_MatSn_List);
            if (De_Scan.Is_Scan_Error)
            {
                throw new Exception("Error，未识别二维码『" + Scan_Source + "』");
            }
            else
            {
                Scan.MatSn = De_Scan.Decode_MatSn;
                Scan.Scan_Quantity = De_Scan.Decode_Scan_Quantity;

                if (Stocktaking_List.Where(x => x.MatSn == Scan.MatSn).Any() == false)
                {
                    throw new Exception("该底盘库位中不存在此产品");
                }

                db.WMS_Stocktaking_Scan.Add(Scan);
                MyDbSave.SaveChange(db);
            }
        }

        public DataTable WMS_Up_Stocktaking_Scan_List_Other(string Move_ID)
        {
            Guid MoveID = Guid.NewGuid();
            try { MoveID = new Guid(Move_ID); } catch { }

            List<WMS_Stocktaking_Scan> List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == MoveID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString() && x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).OrderByDescending(x => x.Create_DT).ToList();

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序号");
            dt.Columns.Add("产品型号");
            dt.Columns.Add("数量");
            int i = 0;
            foreach (var x in List.OrderByDescending(x => x.Create_DT))
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序号"] = i;
                dr["产品型号"] = x.MatSn;
                dr["数量"] = x.Scan_Quantity;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //底盘扫描(端数)
        public void WMS_Up_Stocktaking_Scan_Item_Other(string Move_ID, string Scan_Source, string Quantity)
        {
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid MoveID = new Guid(Move_ID);
            WMS_Move Move = db.WMS_Move.Find(MoveID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            if (string.IsNullOrEmpty(Move.Work_Person)) { throw new Exception("未派工"); }

            if (string.IsNullOrEmpty(Move.In_Location))
            {
                throw new Exception("移入库位不存在");
            }

            if (Move.Move_Status == WMS_Move_Status_Enum.已移库.ToString())
            {
                throw new Exception("已执行移库，请勿重复操作");
            }

            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Move.Move_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();
            List<string> Line_MatSn_List = Stocktaking_List.Select(x => x.MatSn).Distinct().ToList();

            WMS_Stocktaking_Scan Scan = new WMS_Stocktaking_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.Create_DT = DateTime.Now;
            Scan.Link_TaskID = Move.Move_ID;
            Scan.LinkMainCID = Move.LinkMainCID;
            Scan.MatBrand = Stocktaking_List.FirstOrDefault().MatBrand;
            Scan.Location = Stocktaking_List.FirstOrDefault().Location;
            Scan.Scan_Source = Scan_Source;
            Scan.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
            Scan.Package_Type = WMS_Stock_Package_Enum.零头.ToString();

            bool Flag_Success = false;
            foreach (var MatSn in Line_MatSn_List)
            {
                if (Scan_Source == MatSn)
                {
                    Scan.MatSn = MatSn;
                    Flag_Success = true;
                    break;
                }
            }

            //无法识别，则直接进行报错
            if (Flag_Success == false) { throw new Exception("Error，未识别二维码『" + Scan.Scan_Source + "』"); }

            try
            {
                Scan.Scan_Quantity = Convert.ToInt32(Quantity);
                if (Scan.Scan_Quantity <= 0) { throw new Exception("未填入正确的数字"); }
            }
            catch { throw new Exception("填入数量的格式错误"); }

            db.WMS_Stocktaking_Scan.Add(Scan);
            MyDbSave.SaveChange(db);
        }
    }

    //移库作业
    public partial class WmsService : IWmsService
    {
        public DataTable WMS_Move_List(Guid MainCID)
        {
            List<WMS_Move> Move_List_DB = db.WMS_Move.Where(x => x.LinkMainCID == MainCID && x.Move_Status == WMS_Move_Status_Enum.待移库.ToString() && x.Task_Bat_No == "").ToList();
            List<Guid> Guid_List = Move_List_DB.Select(x => x.Move_ID).Distinct().ToList();
            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => Guid_List.Contains(x.Link_TaskID) && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();
            List<WMS_Move> Move_List = new List<WMS_Move>();
            foreach (var x in Move_List_DB)
            {
                if (!string.IsNullOrEmpty(x.Work_Person))
                {
                    Move_List.Add(x);
                }
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("移出库位");
            dt.Columns.Add("是否底盘");
            dt.Columns.Add("底盘库位");
            dt.Columns.Add("作业人");
            dt.Columns.Add("页码");

            foreach (var x in Move_List.OrderBy(x => x.Task_Bat_No))
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Move_ID;
                dr["移出库位"] = x.Out_Location;

                if (Stocktaking_List.Where(c => c.Link_TaskID == x.Move_ID).Any())
                {
                    dr["底盘库位"] = Stocktaking_List.Where(c => c.Link_TaskID == x.Move_ID).FirstOrDefault().Location;
                    dr["是否底盘"] = "是";
                    dr["页码"] = 1;
                }
                else
                {
                    dr["底盘库位"] = "";
                    dr["是否底盘"] = "";
                    dr["页码"] = 0;
                }
                dr["作业人"] = x.Work_Person;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public void WMS_Move_Create(Guid MainCID, string Scan_Source)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未扫描库位"); }

            WMS_Move Move = new WMS_Move();
            Move.Move_ID = MyGUID.NewGUID();
            Move.LinkMainCID = MainCID;
            Move.Create_DT = DateTime.Now;
            Move.Out_Location = Scan_Source;
            if (db.WMS_Location.Where(x => x.LinkMainCID == MainCID && x.Location == Move.Out_Location).Any() == false) { throw new Exception("系统中不存在该库位"); }

            if (db.WMS_Stock.Where(x => x.LinkMainCID == MainCID && x.Location == Move.Out_Location).Any() == false) { throw new Exception("该库位无产品"); }

            if (db.WMS_Move.Where(x => x.Move_ID != Move.Move_ID && x.Out_Location == Move.Out_Location && x.Move_Status == WMS_Move_Status_Enum.待移库.ToString()).Any())
            {
                throw new Exception("该库位已创建待移库任务");
            }

            Move.Move_Status = WMS_Move_Status_Enum.待移库.ToString();
            db.WMS_Move.Add(Move);
            MyDbSave.SaveChange(db);
        }
    }

    //二维码解码标准库
    public partial class WmsService : IWmsService
    {
        private Decode_Scan Decode_Scan_Source(string Brand, string Scan_Source, List<string> Line_MatSn_List)
        {
            Decode_Scan De_Scan = new Decode_Scan();

            if (Brand == Decode_Scan_Brand.NSK.ToString())
            {
                De_Scan = this.Decode_Scan_Source_NSK(Scan_Source, Line_MatSn_List);
            }
            else if (Brand == Decode_Scan_Brand.NMB.ToString())
            {
                De_Scan = this.Decode_Scan_Source_NMB(Scan_Source);
            }
            else
            {
                throw new Exception("解码失败");
            }
            return De_Scan;
        }

        private Decode_Scan Decode_Scan_Source_NSK(string Scan_Source, List<string> Line_MatSn_List)
        {
            bool Check_Flag = false;
            Decode_Scan De_Scan = new Decode_Scan();

            //解码_产品型号
            De_Scan.Decode_MatSn = string.Empty;

            //解码_数量
            De_Scan.Decode_Scan_Quantity = 0;

            //是否为无法识别二维码
            De_Scan.Is_Scan_Error = false;

            int Index = 0;
            if (Line_MatSn_List.Any())
            {
                //正常处理任务单内已匹配的产品型号,一旦匹配直接跳出循环
                foreach (var MatSn in Line_MatSn_List)
                {
                    if (Scan_Source.Contains(MatSn))
                    {
                        try
                        {
                            De_Scan.Decode_MatSn = MatSn;
                            Index = Scan_Source.IndexOf(De_Scan.Decode_MatSn);
                            De_Scan.Decode_Scan_Quantity = Convert.ToInt32(Scan_Source.Substring(Index + De_Scan.Decode_MatSn.Length, 7));
                        }
                        catch
                        {
                            De_Scan.Is_Scan_Error = true;
                        }
                        Check_Flag = true;
                        break;
                    }
                }
            }

            //非任务单匹配型号，进行标准解码程序
            if (Check_Flag == false)
            {
                //标准期货箱号二维码
                if (Scan_Source.StartsWith("NSK2D"))
                {
                    try
                    {
                        //标准解码前缀，NSK2D，前缀
                        De_Scan.Decode_MatSn = Scan_Source.Substring(25, 27);
                        Index = Scan_Source.IndexOf(De_Scan.Decode_MatSn);
                        De_Scan.Decode_Scan_Quantity = Convert.ToInt32(Scan_Source.Substring(Index + De_Scan.Decode_MatSn.Length, 7));

                        //判断去除空格后时候具备27位
                        if (De_Scan.Decode_MatSn.Length != 27) { De_Scan.Is_Scan_Error = true; }
                    }
                    catch
                    {
                        De_Scan.Is_Scan_Error = true;
                    }
                }
                else
                {
                    try
                    {
                        De_Scan.Decode_MatSn = Scan_Source.Substring(0, 27);
                        Index = Scan_Source.IndexOf(De_Scan.Decode_MatSn);
                        if (Scan_Source.Length == 34)
                        {
                            De_Scan.Decode_Scan_Quantity = Convert.ToInt32(Scan_Source.Substring(Index + De_Scan.Decode_MatSn.Length, 7));
                        }
                        else
                        {
                            De_Scan.Decode_Scan_Quantity = 0;
                        }

                        //判断去除空格后时候具备27位
                        if (De_Scan.Decode_MatSn.Trim().Length != 27) { De_Scan.Is_Scan_Error = true; }

                        if (De_Scan.Decode_Scan_Quantity == 0) { De_Scan.Is_Scan_Error = true; }
                    }
                    catch
                    {
                        De_Scan.Is_Scan_Error = true;
                    }
                }
            }
            return De_Scan;
        }

        private Decode_Scan Decode_Scan_Source_NMB(string Scan_Source)
        {
            Decode_Scan De_Scan = new Decode_Scan();

            //解码_产品型号
            De_Scan.Decode_MatSn = string.Empty;

            //解码_数量
            De_Scan.Decode_Scan_Quantity = 0;

            //是否为无法识别二维码
            De_Scan.Is_Scan_Error = false;

            try
            {
                string[] Source = Scan_Source.Trim().Split('/');
                De_Scan.Decode_MatSn = Source[0].Replace(" ", "") + Source[1].Replace(" ", "");
                De_Scan.Decode_Scan_Quantity = Convert.ToInt32(Source[2].Trim());
            }
            catch
            {
                De_Scan.Is_Scan_Error = true;
            }

            return De_Scan;
        }

        //private Decode_Scan Decode_Scan_Source_HONGEN(string Scan_Source)
        //{
        //    Decode_Scan De_Scan = new Decode_Scan();

        //    //解码_产品型号
        //    De_Scan.Decode_MatSn = string.Empty;

        //    //解码_数量
        //    De_Scan.Decode_Scan_Quantity = 0;

        //    //是否为无法识别二维码
        //    De_Scan.Is_Scan_Error = false;

        //    try
        //    {
        //        string[] Source = Scan_Source.Trim().Split('/');
        //        De_Scan.Decode_MatSn = Source[1];
        //        De_Scan.Decode_Scan_Quantity = Convert.ToInt32(Source[2].Trim());
        //    }
        //    catch
        //    {
        //        De_Scan.Is_Scan_Error = true;
        //    }

        //    return De_Scan;
        //}
    }
}
