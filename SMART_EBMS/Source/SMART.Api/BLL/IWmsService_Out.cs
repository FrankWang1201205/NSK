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
using NPOI.SS.Util;

namespace SMART.Api
{
    public partial interface IWmsService
    {
        //送货单
        PageList<WMS_Out_Head> Get_WMS_Out_Head_PageList(WMS_Out_Filter MF);
        PageList<WMS_Out_Head> Get_WMS_Out_Head_PageList_Temp(WMS_Out_Filter MF);
        PageList<WMS_Out_Head> Get_WMS_Out_Head_PageList_Distribute(WMS_Out_Filter MF);
        List<WMS_Out_Line> Get_WMS_Out_Line_List(WMS_Out_Filter MF);
        void Batch_Create_WMS_Out(HttpPostedFileBase ExcelFile, WMS_Out_Head Head, User U);
        void Batch_Create_WMS_Out_Line(HttpPostedFileBase ExcelFile, WMS_Out_Head Head, User U);
        WMS_Out_Task Get_WMS_Out_Task_Item(WMS_Out_Filter MF);
        List<WMS_Out_Scan> Get_WMS_Out_Scan_List_By_Tray_No(Guid Head_ID, string Tray_No);
        WMS_Out_Task Get_WMS_Out_Task_Item_Pick(WMS_Out_Filter MF);
        WMS_Out_Task Get_WMS_Out_Task_Item_Pick_Scan(WMS_Out_Filter MF);
        WMS_Out_Head Get_WMS_Out_Head_DB(Guid Head_ID);
        WMS_Out_Head Get_WMS_Out_Head_DB_With_Work_Person(Guid Head_ID);
        void Delete_Task_Bat_Out(Guid HeadID);
        string Get_WMS_Out_Line_List_To_Excel(Guid Head_ID);
        void Batch_Create_WMS_Out(HttpPostedFileBase ExcelFile, Guid HeadID, User U);
        void Set_WMS_Out_Head_With_Work_Person(Guid Head_ID, List<string> Down_Person_List, List<string> Out_Person_List, List<string> Driver_List);
        void Reset_Out_Task_Bat_Tray_No(Guid Head_ID, string Tray_No);
        void Reset_Out_Task_Bat_Tray_No_By_Box(Guid Head_ID, string Tray_No, string Case_No);
        string Get_QRCode(string Str);

        //任务列表展示
        List<WMS_Out_Head> Get_WMS_Out_Head_Down_List(Guid LinkMainCID);
        List<WMS_Out_Head> Get_WMS_Out_Head_Out_List(Guid LinkMainCID);

        //配货底盘
        List<WMS_Out_Task_Line> Get_WMS_Out_Task_Line_List(WMS_Out_Filter MF);
        List<WMS_Out_Pick_Scan> Get_WMS_Out_Pick_Scan_List(Guid HeadID, string MatSn);
        void WMS_Out_Pick_List_Sub_Finish(Guid Head_ID);
        List<WMS_Stocktaking_Scan> Get_WMS_Stocktaking_Scan_List(Guid ScanID);
        void WMS_Out_Pick_List_Sub_With_Location_Reset(Guid Head_ID, string MatSn);
        WMS_Out_Pick_Scan Get_WMS_Out_Pick_Scan_Item(Guid Scan_ID);
        WMS_Out_Task_Line Get_WMS_Out_Task_Line_With_Stocktaking(Guid ScanID);
        void WMS_Out_Task_Preview_Pick_Scan_Choose(Guid TaskID);
        void WMS_Out_Task_Preview_Pick_Location_Choose(Guid Head_ID, string MatSn, string Location);
        void WMS_Out_Task_Preview_Pick_Location_Choose(Guid Scan_ID);
        void WMS_Out_Task_Preview_Pick_Scan_Cancel_Choose(Guid TaskID);
        void WMS_Out_Task_Preview_Pick_Scan_Check(Guid TaskID);
        void WMS_Out_Task_Preview_Pick_Location_Set(Guid Scan_ID, int Quantity);
        void WMS_Out_Pick_List_Sub_Finish_Pick_Location(Guid Head_ID);
        void WMS_Out_Pick_List_Sub_Cancel_Pick_Location(Guid Head_ID);

        PageList<WMS_Out_Pick_Scan> Get_WMS_Out_Pick_Scan_PageList(WMS_Out_Filter MF);

        //出库验货
        void Cancel_WMS_Out_Head_Inspection(Guid HeadID);
        void Set_WMS_Out_Head_With_Scan_Type(WMS_Out_Head Head);
        WMS_Out_Line Get_WMS_Out_Line_Item(Guid Head_ID, string MatSn);
        WMS_Out_Line Get_WMS_Out_Line_Item(Guid LineID, int Quantity);
        void WMS_Out_Task_To_WMS_Stock_Check(Guid Head_ID);
        void WMS_Out_Task_To_WMS_Stock(Guid Head_ID);
        void WMS_Out_Task_To_WMS_Stock_Check_Again(Guid Head_ID);
        string Get_Out_Task_List_To_Excel(Guid Head_ID);
        string Get_Out_Task_List_To_Excel_By_Tray(Guid Head_ID, string Tray_No);
        List<WMS_Out_Scan> Get_WMS_Out_Scan_List(Guid Head_ID, string MatSn);
        void Reset_WMS_Out_Scan_By_MatSn(Guid Head_ID, string MatSn);
        void WMS_Out_Task_Preview_Source_Check(Guid LineID);
        string Get_Out_Task_List_To_Excel_With_Tracking_No(WMS_Out_Head Head, List<WMS_Out_Scan> Scan_List, List<WMS_Track> Track_List, List<WMS_Track_Info> Track_Info_List);

        //退货作业
        WMS_Out_Task Get_WMS_Out_Task_Item_DB(Guid HeadID);
        void Create_WMS_Task_Out_Return(WMS_Out_Head Head, List<WMS_Out_Line> Line_List, User U);
        WMS_Out_Task Get_WMS_Out_Task_Item_DB(WMS_Out_Filter MF);
        List<WMS_In_Line> Get_WMS_In_Line_List(WMS_Out_Head Head);
        void Update_WMS_Task_Out_Return(WMS_Out_Head Head, List<WMS_Out_Line> Line_List);

        //移库推荐
        PageList<WMS_Stock_Task> Get_WMS_Stock_Task_PageList_For_Move_Recommend(WMS_Stock_Filter MF);
        List<WMS_Stocktaking_Scan> Get_WMS_Stocktaking_Scan_List_For_Move(Guid TaskID);
        void Finish_WMS_Stocktaking_Scan_Recommend(Guid TaskID);
        List<WMS_Move> Get_WMS_Move_List_By_Link_HeadID(Guid Link_HeadID);
        void Create_WMS_Move_From_WMS_Stocktaking_Scan_Recommend(Guid TaskID, WMS_Move_Scan Move_Scan);
        void Delete_WMS_Move_From_WMS_Stocktaking_Scan_Recommend(Guid Move_ID);
        List<WMS_Move_Scan> Get_WMS_Move_Scan_List(Guid Move_ID);
        void Finish_WMS_Move_Task_From_Recommend(Guid Move_ID);
    }

    //送货单创建
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Out_Head> Get_WMS_Out_Head_PageList(WMS_Out_Filter MF)
        {
            var query = db.WMS_Out_Head.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_Bat_No))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_Bat_No)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Customer_Name))
            {
                query = query.Where(x => x.Customer_Name.Contains(MF.Customer_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Mode))
            {
                query = query.Where(x => x.Logistics_Mode.Contains(MF.Logistics_Mode)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Head_Type))
            {
                query = query.Where(x => x.Head_Type == MF.Head_Type).AsQueryable();
            }

            if (Enum.GetNames(typeof(WMS_Out_Global_State_Enum)).ToList().Where(x => x == MF.Global_State).Any())
            {
                query = query.Where(x => x.Status == MF.Global_State).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person == MF.Create_Person).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Down_Person))
            {
                query = query.Where(x => x.Work_Down_Person.Contains(MF.Work_Down_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Out_Person))
            {
                query = query.Where(x => x.Work_Out_Person.Contains(MF.Work_Out_Person)).AsQueryable();
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

            List<WMS_Out_Head> Head_list = query.ToList();
            List<Guid> ID_List = Head_list.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Out_Line> Line_List_Sub = new List<WMS_Out_Line>();
            foreach (var x in Head_list)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
            }

            PageList<WMS_Out_Head> PList = new PageList<WMS_Out_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Head_list.Count();
            PList.Rows = Head_list.OrderBy(x => x.Out_DT_Str).ThenBy(x => x.Task_Bat_No_Str).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public PageList<WMS_Out_Head> Get_WMS_Out_Head_PageList_Distribute(WMS_Out_Filter MF)
        {
            var query = db.WMS_Out_Head.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status != WMS_Out_Global_State_Enum.已出库.ToString()).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_Bat_No))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_Bat_No)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Customer_Name))
            {
                query = query.Where(x => x.Customer_Name.Contains(MF.Customer_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Mode))
            {
                query = query.Where(x => x.Logistics_Mode.Contains(MF.Logistics_Mode)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person == MF.Create_Person).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Down_Person))
            {
                query = query.Where(x => x.Work_Down_Person.Contains(MF.Work_Down_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Out_Person))
            {
                query = query.Where(x => x.Work_Out_Person.Contains(MF.Work_Out_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Head_Type))
            {
                query = query.Where(x => x.Head_Type == MF.Head_Type).AsQueryable();
            }

            if (Enum.GetNames(typeof(WMS_Out_Global_State_Enum)).ToList().Where(x => x == MF.Global_State).Any())
            {
                query = query.Where(x => x.Status == MF.Global_State).AsQueryable();
            }

            List<WMS_Out_Head> Head_list = query.ToList();
            List<Guid> ID_List = Head_list.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Out_Line> Line_List_Sub = new List<WMS_Out_Line>();
            foreach (var x in Head_list)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
            }

            PageList<WMS_Out_Head> PList = new PageList<WMS_Out_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Head_list.Count();
            PList.Rows = Head_list.OrderBy(x => x.Out_DT_Str).ThenBy(x => x.Task_Bat_No_Str).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public PageList<WMS_Out_Head> Get_WMS_Out_Head_PageList_Temp(WMS_Out_Filter MF)
        {
            var query = db.WMS_Out_Head.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status != WMS_Out_Global_State_Enum.已出库.ToString()).AsQueryable();
            query = query.Where(x => x.Status == WMS_Out_Global_State_Enum.待包装.ToString() || x.Status == WMS_Out_Global_State_Enum.待验货.ToString()).AsQueryable();
            if (!string.IsNullOrEmpty(MF.Task_Bat_No))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_Bat_No)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Customer_Name))
            {
                query = query.Where(x => x.Customer_Name.Contains(MF.Customer_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Mode))
            {
                query = query.Where(x => x.Logistics_Mode.Contains(MF.Logistics_Mode)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person == MF.Create_Person).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Down_Person))
            {
                query = query.Where(x => x.Work_Down_Person.Contains(MF.Work_Down_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Out_Person))
            {
                query = query.Where(x => x.Work_Out_Person.Contains(MF.Work_Out_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Head_Type))
            {
                query = query.Where(x => x.Head_Type == MF.Head_Type).AsQueryable();
            }

            query = query.Where(x => x.Status == WMS_Out_Global_State_Enum.待验货.ToString() || x.Status == WMS_Out_Global_State_Enum.待包装.ToString() || x.Status == WMS_Out_Global_State_Enum.待出库.ToString()).AsQueryable();

            List<WMS_Out_Head> Head_list = query.ToList();
            List<Guid> ID_List = Head_list.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Out_Line> Line_List_Sub = new List<WMS_Out_Line>();
            foreach (var x in Head_list)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
            }

            PageList<WMS_Out_Head> PList = new PageList<WMS_Out_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Head_list.Count();
            PList.Rows = Head_list.OrderBy(x => x.Out_DT_Str).ThenBy(x => x.Task_Bat_No_Str).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public List<WMS_Out_Head> Get_WMS_Out_Head_Down_List(Guid LinkMainCID)
        {
            List<WMS_Out_Head> Head_list = db.WMS_Out_Head.Where(x => x.LinkMainCID == LinkMainCID && x.Status == WMS_Out_Global_State_Enum.待配货.ToString()).ToList();
            List<Guid> ID_List = Head_list.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Out_Line> Line_List_Sub = new List<WMS_Out_Line>();
            foreach (var x in Head_list)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
            }
            return Head_list.OrderBy(x => x.Out_DT_Str).ToList();
        }

        public List<WMS_Out_Head> Get_WMS_Out_Head_Out_List(Guid LinkMainCID)
        {
            List<WMS_Out_Head> Head_list = db.WMS_Out_Head.Where(x => x.LinkMainCID == LinkMainCID && x.Status != WMS_Out_Global_State_Enum.待配货.ToString() && x.Status != WMS_Out_Global_State_Enum.已出库.ToString()).ToList();
            List<Guid> ID_List = Head_list.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Out_Line> Line_List_Sub = new List<WMS_Out_Line>();
            foreach (var x in Head_list)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
            }
            return Head_list.OrderBy(x => x.Out_DT_Str).ToList();

        }

        public List<WMS_Out_Line> Get_WMS_Out_Line_List(WMS_Out_Filter MF)
        {
            var query = db.WMS_Out_Line.Where(x => x.Link_Head_ID == MF.LinkHeadID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }
            List<WMS_Out_Line> Line_List_DB = query.ToList();
            List<string> MatSn_List = Line_List_DB.Select(x => x.MatSn).Distinct().ToList();
            List<WMS_Out_Line> Line_List = new List<WMS_Out_Line>();
            WMS_Out_Line Line = new WMS_Out_Line();
            List<WMS_Out_Line> Line_List_Sub = new List<WMS_Out_Line>();
            int i = 0;
            foreach (var MatSn in MatSn_List)
            {
                i++;
                Line = new WMS_Out_Line();
                Line.Line_No = i;
                Line.MatSn = MatSn;
                Line_List_Sub = Line_List_DB.Where(c => c.MatSn == MatSn).ToList();
                Line.Quantity = Line_List_Sub.Sum(c => c.Quantity);
                Line_List.Add(Line);
            }

            return Line_List;
        }

        private List<WMS_Out_Task_Group_Tray> Get_WMS_Out_Task_Group_Tray_List(List<WMS_Out_Scan> Scan_List)
        {
            var Group = from x in Scan_List
                        group x by x.Tray_No into g
                        select new
                        {
                            Tray_No = g.Key,
                        };

            List<WMS_Out_Task_Group_Tray> List = new List<WMS_Out_Task_Group_Tray>();
            WMS_Out_Task_Group_Tray T = new WMS_Out_Task_Group_Tray();
            foreach (var x in Group)
            {
                T = new WMS_Out_Task_Group_Tray();
                T.Tray_No = x.Tray_No;
                T.Scan_List = Scan_List.Where(c => c.Tray_No == x.Tray_No).ToList();
                List.Add(T);
            }

            List = List.OrderBy(x => x.Tray_No).ToList();
            return List;
        }

        public List<WMS_Out_Scan> Get_WMS_Out_Scan_List_By_Tray_No(Guid Head_ID, string Tray_No)
        {
            List<WMS_Out_Scan> List_Scan = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head_ID && x.Tray_No == Tray_No).OrderBy(x => x.Create_DT).ToList();
            return List_Scan;
        }

        public void Batch_Create_WMS_Out(HttpPostedFileBase ExcelFile, WMS_Out_Head Head, User U)
        {
            if (string.IsNullOrEmpty(Head.Logistics_Mode)) { throw new Exception("未选择运输方式"); }
            if (string.IsNullOrEmpty(Head.Customer_Name)) { throw new Exception("未选择客户"); }

            Customer C = db.Customer.Find(Head.Link_Cus_ID);
            if (C == null) { throw new Exception("未选择客户"); }

            Head.Out_DT_Str = Head.Out_DT.ToString("yyyy-MM-dd");
            Head.Logistics_Mode = Head.Logistics_Mode.Trim();

            //执行数据持久化
            Head.Head_ID = MyGUID.NewGUID();
            Head.Task_Bat_No = this.Auto_Create_Task_Bat_No_Out(U);
            Head.Task_Bat_No_Str = Auto_Create_Task_Bat_No_Str_Out(Head, C);
            Head.Create_DT = DateTime.Now;
            Head.Create_Person = U.UserFullName;
            Head.Status = WMS_Out_Global_State_Enum.待配货.ToString();
            Head.LinkMainCID = U.LinkMainCID;

            Head.Head_Type = WMS_Out_Head_Type_Enum.订单出货.ToString();

            db.WMS_Out_Head.Add(Head);

            this.Batch_Create_WMS_Out_Line(ExcelFile, Head, U);
            MyDbSave.SaveChange(db);
        }

        public void Batch_Create_WMS_Out_Line(HttpPostedFileBase ExcelFile, WMS_Out_Head Head, User U)
        {
            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "WMS_Out_Line/" + U.UID);

            //根据路径通过已存在的excel来创建XSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            string Currency = Currency_Enum.CNY.ToString();

            //读取Excel列，装箱数据
            List<WMS_Out_Line> Line_List = new List<WMS_Out_Line>();
            WMS_Out_Line Line = new WMS_Out_Line();
            ISheet sheet = workbook.GetSheetAt(0);

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                Line = new WMS_Out_Line();
                Line.Line_ID = MyGUID.NewGUID();
                Line.Task_Bat_No = Head.Task_Bat_No;
                Line.Task_Bat_No_Str = Head.Task_Bat_No_Str;
                Line.Create_DT = Head.Create_DT;
                Line.Create_Person = Head.Create_Person;
                Line.Line_No = i;
                Line.LinkMainCID = Head.LinkMainCID;
                Line.Link_Head_ID = Head.Head_ID;
                Line.Logistics_Mode = Head.Logistics_Mode;

                try { Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }
                try { Line.MatUnit = row.GetCell(1).ToString().Trim(); } catch { Line.MatUnit = string.Empty; }
                try { Line.Quantity = Convert.ToInt32(row.GetCell(2).ToString().Trim()); } catch { Line.Quantity = 0; }
                try { Line.Price = Convert.ToDecimal(row.GetCell(3).ToString().Trim()); } catch { Line.Price = 0; }

                //过滤换行符
                Line.MatUnit = Line.MatUnit.Replace(Environment.NewLine, "");
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);

                //判断单元格是否有公式
                for (int j = 0; j < 4; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                if (Line.Quantity <= 0)
                {
                    throw new Exception("此型号" + Line.MatSn + "的数量格式不正确");
                }

                if (Line.Price <= 0)
                {
                    throw new Exception("此型号" + Line.MatSn + "的单价格式不正确");
                }

                //判断型号是否存在系统库存中
                if (db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && x.MatSn == Line.MatSn).Any() == false)
                {
                    throw new Exception("此型号" + Line.MatSn + "不存在于系统库存中");
                }

                Line_List.Add(Line);
            }

            if (Line_List.Any())
            {
                //判断系统库存是否满足出货需求
                List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();
                List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();

                //预占用库存
                List<Guid> Out_HeadID_List_DB = db.WMS_Out_Head.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Status == WMS_Out_Global_State_Enum.待配货.ToString()).Select(x => x.Head_ID).ToList();
                List<Guid> Out_HeadID_List = new List<Guid>();
                List<WMS_Stock_Temp> Stock_Temp_List = db.WMS_Stock_Temp.Where(x => Out_HeadID_List_DB.Contains(x.WMS_Out_Head_ID)).ToList();

                foreach (var ID in Out_HeadID_List_DB)
                {
                    if (Stock_Temp_List.Where(c => c.WMS_Out_Head_ID == ID).Any() == false)
                    {
                        Out_HeadID_List.Add(ID);
                    }
                }

                List<WMS_Out_Line> Out_Line_List = db.WMS_Out_Line.Where(x => Out_HeadID_List.Contains(x.Link_Head_ID)).ToList();

                int Quantity_Avaliable = 0;
                int Quantity_Preoccupancy = 0;
                int Quantity = 0;
                foreach (var x in Line_List)
                {
                    Quantity_Avaliable = Stock_List_DB.Where(c => c.MatSn == x.MatSn).Sum(c => c.Quantity);
                    Quantity_Preoccupancy = Out_Line_List.Where(c => c.MatSn == x.MatSn).Sum(c => c.Quantity);

                    Quantity = Quantity_Avaliable - Quantity_Preoccupancy;
                    if (x.Quantity > Quantity)
                    {
                        throw new Exception(x.MatSn + "的需求数量超过库存能够满足的数量，当前库存可用余量为" + Quantity_Avaliable + ",待配货数量为" + Quantity_Preoccupancy);
                    }
                }

                db.WMS_Out_Line.AddRange(Line_List);
            }
        }

        private long Auto_Create_Task_Bat_No_Out(User U)
        {
            long Task_Bat_No_Min = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + "0001");
            long Task_Bat_No_Max = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + "9999");

            long Task_Bat_No = 0;
            if (db.WMS_Out_Head.Where(x => x.LinkMainCID == U.LinkMainCID && x.Task_Bat_No >= Task_Bat_No_Min).Any())
            {
                Task_Bat_No = db.WMS_Out_Head.Where(x => x.LinkMainCID == U.LinkMainCID).Max(x => x.Task_Bat_No) + 1;
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

        private string Auto_Create_Task_Bat_No_Str_Out(WMS_Out_Head Head, Customer C)
        {
            List<string> Task_Bat_No_Str_List = db.WMS_Out_Head.Where(x => x.LinkMainCID == C.LinkMainCID && x.Out_DT_Str == Head.Out_DT_Str).Select(x => x.Task_Bat_No_Str).Distinct().ToList();

            string Logistics_Mode_Code = string.Empty;

            if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.快递.ToString())
            {
                Logistics_Mode_Code = "KD";
            }
            else if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.物流.ToString())
            {
                Logistics_Mode_Code = "WL";
            }
            else if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.自送.ToString())
            {
                Logistics_Mode_Code = "ZS";
            }
            else if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.自提.ToString())
            {
                Logistics_Mode_Code = "ZT";
            }

            Head.Task_Bat_No_Str = C.Cust_Code + "_" + Head.Out_DT.ToString("yyyyMMdd") + "_" + Logistics_Mode_Code + "_";

            //string Task_Bat_No_Str_Temp = string.Empty;

            //string[] Char = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            //for (int i = 0; i < Char.Length; i++)
            //{
            //    Task_Bat_No_Str_Temp = Head.Task_Bat_No_Str + Char[i];
            //    if (Task_Bat_No_Str_List.Where(c => c.Contains(Task_Bat_No_Str_Temp)).Any() == false)
            //    {
            //        Head.Task_Bat_No_Str = Head.Task_Bat_No_Str + Char[i];
            //        break;
            //    }
            //}

            Task_Bat_No_Str_List = Task_Bat_No_Str_List.Where(x => x.Contains(Head.Task_Bat_No_Str)).ToList();
            if (Task_Bat_No_Str_List.Any() == false)
            {
                Head.Task_Bat_No_Str = Head.Task_Bat_No_Str + "1";
            }
            else
            {
                List<long> Task_Bat_No_Num_List = new List<long>();

                foreach (var No in Task_Bat_No_Str_List)
                {
                    string[] Source = No.Trim().Split('_');
                    try
                    {
                        Task_Bat_No_Num_List.Add(Convert.ToInt64((Source[3].Trim())));
                    }
                    catch
                    {
                        Task_Bat_No_Num_List.Add(1);
                    }

                }

                long Task_Bat_No_Num = Task_Bat_No_Num_List.Max() + 1;
                Head.Task_Bat_No_Str = Head.Task_Bat_No_Str + Task_Bat_No_Num.ToString();
            }

            return Head.Task_Bat_No_Str;
        }

        public WMS_Out_Head Get_WMS_Out_Head_DB(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Out_Head() : Head;
            return Head;
        }

        public void Delete_Task_Bat_Out(Guid HeadID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null"); }
            if (Head.Status != WMS_Out_Global_State_Enum.待配货.ToString())
            {
                throw new Exception("该任务已完成配货，不支持删除！");
            }

            List<WMS_Out_Line> List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            List<string> MatSn_List = List.Select(x => x.MatSn).Distinct().ToList();

            //加入不允许删除的验证
            if (db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && MatSn_List.Contains(x.MatSn)).Any()) { throw new Exception("存在配货指令,不支持删除任务单！"); }

            db.WMS_Out_Line.RemoveRange(List);
            db.WMS_Out_Head.Remove(Head);
            MyDbSave.SaveChange(db);
        }

        public string Get_WMS_Out_Line_List_To_Excel(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_In_Head is null"); }
            List<WMS_Out_Line> List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            string Path = string.Empty;

            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("产品名称");
            TableHeads.Add("产品型号");
            TableHeads.Add("数量");
            TableHeads.Add("品牌");
            TableHeads.Add("单价");

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
                newRow["数量"] = x.Quantity;
                newRow["品牌"] = x.MatBrand;
                newRow["单价"] = x.Price.ToString("N4");
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public void Batch_Create_WMS_Out(HttpPostedFileBase ExcelFile, Guid HeadID, User U)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_In_Head is null"); }

            List<WMS_Out_Line> List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            db.WMS_Out_Line.RemoveRange(List);
            this.Batch_Create_WMS_Out_Line(ExcelFile, Head, U);

            MyDbSave.SaveChange(db);
        }

        public string Get_QRCode(string Str)
        {
            Guid TempID = MyGUID.NewGUID();
            string QRCode_Path = string.Empty;
            QRCode_Path = QRCode.CreateQRCode(Str, TempID);
            return QRCode_Path;
        }

        public void Set_WMS_Out_Head_With_Work_Person(Guid Head_ID, List<string> Down_Person_List, List<string> Out_Person_List, List<string> Driver_List)
        {
            if (Down_Person_List.Count <= 0) { throw new Exception("未勾选配货人！"); }

            WMS_Out_Head Head_DB = db.WMS_Out_Head.Find(Head_ID);
            Head_DB = Head_DB == null ? new WMS_Out_Head() : Head_DB;

            if (Head_DB.Status == WMS_Out_Global_State_Enum.已出库.ToString())
            {
                throw new Exception("该出库任务已完成出库");
            }

            Head_DB.Work_Down_Person = string.Empty;

            foreach (var Person in Down_Person_List)
            {
                Head_DB.Work_Down_Person += Person.Trim() + ",";
            }

            Head_DB.Work_Down_Person = CommonLib.Trim_End_Char(Head_DB.Work_Down_Person);

            if (Out_Person_List.Count() > 0)
            {
                Head_DB.Work_Out_Person = string.Empty;

                foreach (var Person in Out_Person_List)
                {
                    Head_DB.Work_Out_Person += Person.Trim() + ",";
                }

                Head_DB.Work_Out_Person = CommonLib.Trim_End_Char(Head_DB.Work_Out_Person);
            }

            if (Head_DB.Logistics_Mode == Logistics_Out_Mode_Enum.自送.ToString() && Driver_List.Count() > 0)
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

        public WMS_Out_Head Get_WMS_Out_Head_DB_With_Work_Person(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Out_Head() : Head;

            Head.Work_Out_Person_List = CommonLib.StringListStrToStringArray(Head.Work_Out_Person);
            Head.Work_Down_Person_List = CommonLib.StringListStrToStringArray(Head.Work_Down_Person);
            Head.Driver_Person_List = CommonLib.StringListStrToStringArray(Head.Driver_Name);
            return Head;
        }
    }

    //配货作业
    public partial class WmsService : IWmsService
    {
        public WMS_Out_Task Get_WMS_Out_Task_Item_Pick(WMS_Out_Filter MF)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(MF.LinkHeadID);
            Head = Head == null ? new WMS_Out_Head() : Head;
            List<WMS_Out_Line> List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            WMS_Out_Task T = new WMS_Out_Task();
            T.Head_ID = Head.Head_ID;
            T.Task_Bat_No_Str = Head.Task_Bat_No_Str;
            T.Global_State = Head.Status;
            T.Work_Down_Person = Head.Work_Down_Person;
            T.Customer_Name = Head.Customer_Name;
            if (db.WMS_Stock_Temp.Where(x => x.WMS_Out_Head_ID == Head.Head_ID).Any())
            {
                T.Is_Finish_Pick_Location = 1;
            }

            T.Line_List = new List<WMS_Out_Task_Line>();
            var Line_Group = from x in List
                             group x by x.MatSn into G
                             select new
                             {
                                 MatSn = G.Key,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                             };

            WMS_Out_Task_Line T_Line = new WMS_Out_Task_Line();

            List<string> MatSn_List = List.Select(x => x.MatSn).Distinct().ToList();
            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
            var Stock_List_Group = from x in Stock_List
                                   group x by new { x.MatSn, x.Location } into G
                                   select new
                                   {
                                       MatSn = G.Key.MatSn,
                                       Location = G.Key.Location,
                                       WMS_In_DT = G.FirstOrDefault().WMS_In_DT,
                                       Location_Type = G.FirstOrDefault().Location_Type,
                                       Quantity_Sum = G.Sum(c => c.Quantity),
                                       Box_Count = G.Count(),
                                   };

            WMS_Out_Line_Commend_Loc Commend_Loc = new WMS_Out_Line_Commend_Loc();
            DateTime Now_DT = DateTime.Now;
            TimeSpan TimeSpan = new TimeSpan();
            int i = 0;
            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID).ToList();

            List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == Head.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();

            foreach (var x in Line_Group.ToList())
            {
                i++;
                T_Line = new WMS_Out_Task_Line();
                T_Line.Line_No = i;
                T_Line.MatSn = x.MatSn;
                T_Line.Quantity_Sum = x.Quantity_Sum;

                if (Mat_List.Where(c => c.MatSn == x.MatSn).Any())
                {
                    T_Line.Pack_Qty = Mat_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault().Pack_Qty;
                }

                foreach (var c in Scan_List.Where(c => c.MatSn == x.MatSn).OrderBy(c => c.Create_DT).ToList())
                {
                    T_Line.Scan_Location += c.Scan_Location + "(" + c.Quantity + ")、";
                }

                T_Line.Scan_Location = CommonLib.Trim_End_Char(T_Line.Scan_Location);

                if (Scan_List.Where(c => c.MatSn == x.MatSn).Any())
                {
                    T_Line.Is_Scan = true;
                }

                DateTime WMS_In_DT = new DateTime();
                foreach (var xx in Stock_List_Group.Where(xx => xx.MatSn == x.MatSn).ToList())
                {
                    if (string.IsNullOrEmpty(xx.WMS_In_DT))
                    {
                        WMS_In_DT = DateTime.Now;
                    }
                    else
                    {
                        WMS_In_DT = Convert.ToDateTime(xx.WMS_In_DT);
                    }

                    TimeSpan = Now_DT - WMS_In_DT;

                    Commend_Loc = new WMS_Out_Line_Commend_Loc();
                    Commend_Loc.Location = xx.Location;
                    Commend_Loc.Box_Count = xx.Box_Count;
                    Commend_Loc.Qty_Sum = xx.Quantity_Sum;

                    //用于匹配接近数，当前未用到
                    Commend_Loc.Qty_Diff = xx.Quantity_Sum - x.Quantity_Sum;

                    if (TimeSpan.TotalDays >= 183 && x.Quantity_Sum <= xx.Quantity_Sum)
                    {
                        Commend_Loc.MatchDegree = 3;
                    }
                    else if (TimeSpan.TotalDays >= 183)
                    {
                        Commend_Loc.MatchDegree = 2;
                    }
                    else if (TimeSpan.TotalDays < 183 && x.Quantity_Sum <= xx.Quantity_Sum)
                    {
                        Commend_Loc.MatchDegree = 1;
                    }

                    if (xx.Location_Type == WMS_Stock_Location_Type_Enum.临时库位.ToString())
                    {
                        Commend_Loc.MatchDegree += 6;
                    }

                    T_Line.Commend_Loc_List.Add(Commend_Loc);

                }

                //T_Line.Commend_Loc_List = T_Line.Commend_Loc_List.OrderByDescending(c => c.MatchDegree).ThenBy(c=>c.Qty_Sum).ToList();
                T_Line.Commend_Loc_List = T_Line.Commend_Loc_List.OrderBy(c => c.Qty_Sum).ThenBy(c => c.Location).ToList();

                T.Line_List.Add(T_Line);
            }

            T.Pick_Scan_List = Scan_List.OrderBy(x => x.Scan_Location).ToList();

            //foreach (var x in T.Pick_Scan_List)
            //{
            //    x.Remain_Quantity = Stock_List.Where(c => c.Location == x.Scan_Location && c.MatSn == x.MatSn).Sum(c => c.Quantity);
            //}

            T.Line_List = T.Line_List.OrderBy(x => x.Line_No).ToList();

            return T;
        }

        public WMS_Out_Task Get_WMS_Out_Task_Item_Pick_Scan(WMS_Out_Filter MF)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(MF.LinkHeadID);
            Head = Head == null ? new WMS_Out_Head() : Head;
            List<WMS_Out_Line> List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            WMS_Out_Task T = new WMS_Out_Task();
            T.Head_ID = Head.Head_ID;
            T.Task_Bat_No_Str = Head.Task_Bat_No_Str;
            T.Global_State = Head.Status;
            T.Work_Down_Person = Head.Work_Down_Person;
            T.Customer_Name = Head.Customer_Name;
            T.Line_List = new List<WMS_Out_Task_Line>();
            var Line_Group = from x in List
                             group x by x.MatSn into G
                             select new
                             {
                                 MatSn = G.Key,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                             };

            WMS_Out_Task_Line T_Line = new WMS_Out_Task_Line();

            List<string> MatSn_List = Line_Group.Select(x => x.MatSn).Distinct().ToList();
            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();

            var Stock_List_Group = from x in Stock_List
                                   group x by new { x.MatSn, x.Location } into G
                                   select new
                                   {
                                       MatSn = G.Key.MatSn,
                                       Location = G.Key.Location,
                                       WMS_In_DT = G.FirstOrDefault().WMS_In_DT,
                                       Location_Type = G.FirstOrDefault().Location_Type,
                                       Quantity_Sum = G.Sum(c => c.Quantity),
                                       Box_Count = G.Count(),
                                   };

            int i = 0;
            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID).ToList();

            foreach (var x in Line_Group.ToList())
            {
                i++;
                T_Line = new WMS_Out_Task_Line();
                T_Line.Line_No = i;
                T_Line.MatSn = x.MatSn;
                T_Line.Quantity_Sum = x.Quantity_Sum;
                T.Line_List.Add(T_Line);
            }

            List<WMS_Stock_Task> Task_List = db.WMS_Stock_Task.Where(x => x.Link_HeadID == Head.Head_ID).ToList();

            List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == Head.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
            Material Mat = new Material();
            if (Task_List.Any())
            {
                WMS_Stock_Task Stocktaking_Task = new WMS_Stock_Task();
                List<Guid> ID_List = Task_List.Select(x => x.Task_ID).Distinct().ToList();
                List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => ID_List.Contains(x.Link_TaskID)).ToList();
                List<string> Loc_Str_List = Task_List.Select(x => x.Location).Distinct().ToList();
                List<WMS_Location> Location_List = db.WMS_Location.Where(x => x.LinkMainCID == Head.LinkMainCID && Loc_Str_List.Contains(x.Location)).ToList();
                WMS_Location Location = new WMS_Location();

                foreach (var x in Scan_List)
                {
                    Location = Location_List.Where(c => c.Location == x.Scan_Location).FirstOrDefault();
                    if (Location != null)
                    {
                        if (Location.Type == Type_Enum.整箱.ToString())
                        {
                            Stocktaking_Task = Task_List.Where(c => c.Location == x.Scan_Location).FirstOrDefault();
                            if (Stocktaking_Task != null)
                            {
                                x.Stocktaking_ID = Stocktaking_Task.Task_ID;
                                x.Stocktaking_Loction_Type = Stocktaking_Task.Type;
                            }
                        }
                        else if (Location.Type == Type_Enum.端数.ToString())
                        {
                            Stocktaking_Task = Task_List.Where(c => c.Location == x.Scan_Location).FirstOrDefault();
                            if (Stocktaking_Task != null && Stocktaking_List.Where(c => c.MatSn == x.MatSn).Any())
                            {
                                x.Stocktaking_ID = Stocktaking_Task.Task_ID;
                                x.Stocktaking_Loction_Type = Stocktaking_Task.Type;
                            }
                        }
                    }
                }
            }

            //装箱数
            foreach (var x in Scan_List)
            {
                Mat = Mat_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                if (Mat != null)
                {
                    x.Pack_Qty = Mat.Pack_Qty;
                }
            }

            T.Pick_Scan_List = Scan_List.OrderBy(x => x.Scan_Location).ToList();
            return T;
        }

        public List<WMS_Out_Task_Line> Get_WMS_Out_Task_Line_List(WMS_Out_Filter MF)
        {
            var query = db.WMS_Out_Line.Where(x => x.Link_Head_ID == MF.LinkHeadID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }
            List<WMS_Out_Line> Line_List_DB = query.ToList();

            List<string> MatSn_List = Line_List_DB.Select(x => x.MatSn).Distinct().ToList();
            List<WMS_Out_Task_Line> Line_List = new List<WMS_Out_Task_Line>();
            WMS_Out_Task_Line Line = new WMS_Out_Task_Line();

            int i = 0;
            foreach (var MatSn in MatSn_List)
            {
                i++;
                Line = new WMS_Out_Task_Line();
                Line.Line_No = i;
                Line.Link_HeadID = MF.LinkHeadID;
                Line.MatSn = MatSn;
                Line.Quantity_Sum = Line_List_DB.Where(c => c.MatSn == MatSn).Sum(c => c.Quantity);
                Line.Line_State = WMS_Out_Scan_Status_Enum.未完成.ToString();
                Line_List.Add(Line);
            }

            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == MF.LinkHeadID && MatSn_List.Contains(x.MatSn)).ToList();
            List<WMS_Out_Pick_Scan> Scan_List_Sub = new List<WMS_Out_Pick_Scan>();
            if (Scan_List.Any())
            {
                foreach (var x in Line_List)
                {
                    Scan_List_Sub = Scan_List.Where(c => c.MatSn == x.MatSn).ToList();
                    if (Scan_List_Sub.Any() && Scan_List_Sub.Where(c => c.Status == WMS_Out_Scan_Status_Enum.未完成.ToString()).Any() == false)
                    {
                        x.Line_State = WMS_Out_Scan_Status_Enum.已完成.ToString();
                    }

                    if (x.Quantity_Sum != Scan_List_Sub.Sum(c => c.Quantity))
                    {
                        x.Line_State = WMS_Out_Scan_Status_Enum.未完成.ToString();
                    }
                }
            }

            return Line_List;
        }

        public PageList<WMS_Out_Pick_Scan> Get_WMS_Out_Pick_Scan_PageList(WMS_Out_Filter MF)
        {
            var query = db.WMS_Out_Pick_Scan.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Scan_Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Down_Person))
            {
                query = query.Where(x => x.Scan_Person.Contains(MF.Work_Down_Person)).AsQueryable();
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

            List<WMS_Out_Pick_Scan> Scan_list = query.ToList();

            PageList<WMS_Out_Pick_Scan> PList = new PageList<WMS_Out_Pick_Scan>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Scan_list.Count();
            PList.Rows = Scan_list.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            List<Guid> ID_List = PList.Rows.Select(x => x.Link_TaskID).Distinct().ToList();
            List<WMS_Out_Head> Head_List = db.WMS_Out_Head.Where(x => ID_List.Contains(x.Head_ID)).ToList();
            WMS_Out_Head Head = new WMS_Out_Head();
            foreach (var x in PList.Rows)
            {
                Head = Head_List.Where(c => c.Head_ID == x.Link_TaskID).FirstOrDefault();
                Head = Head == null ? new WMS_Out_Head() : Head;
                x.Customer = Head.Customer_Name;
            }

            return PList;
        }

        public List<WMS_Out_Pick_Scan> Get_WMS_Out_Pick_Scan_List(Guid HeadID, string MatSn)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(HeadID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            MatSn = MatSn.Trim();
            if (string.IsNullOrEmpty(MatSn)) { throw new Exception("产品型号为空"); }

            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && x.MatSn == MatSn).ToList();

            return Scan_List;
        }

        public List<WMS_Stocktaking_Scan> Get_WMS_Stocktaking_Scan_List(Guid ScanID)
        {
            WMS_Out_Pick_Scan Pick_Scan = db.WMS_Out_Pick_Scan.Find(ScanID);
            if (Pick_Scan == null) { throw new Exception("底盘任务不存在"); }

            List<WMS_Stocktaking_Scan> List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Pick_Scan.Scan_ID).OrderByDescending(x => x.Create_DT).ToList();
            return List;
        }

        public WMS_Out_Pick_Scan Get_WMS_Out_Pick_Scan_Item(Guid Scan_ID)
        {
            WMS_Out_Pick_Scan Scan = db.WMS_Out_Pick_Scan.Find(Scan_ID);
            Scan = Scan == null ? new WMS_Out_Pick_Scan() : Scan;
            return Scan;
        }

        public WMS_Out_Task_Line Get_WMS_Out_Task_Line_With_Stocktaking(Guid ScanID)
        {
            WMS_Out_Pick_Scan Pick_Scan = db.WMS_Out_Pick_Scan.Find(ScanID);
            if (Pick_Scan == null) { throw new Exception("底盘任务不存在"); }

            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Pick_Scan.Scan_ID).OrderByDescending(x => x.Create_DT).ToList();
            List<WMS_Stocktaking> List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Pick_Scan.Scan_ID).OrderByDescending(x => x.Create_DT).ToList();
            int Quantity = List.Sum(x => x.Quantity);
            int Scan_Quantity = Scan_List.Sum(x => x.Scan_Quantity);

            WMS_Out_Task_Line Line = new WMS_Out_Task_Line();
            Line.MatSn = Pick_Scan.MatSn;
            Line.Quantity_Sum = Quantity;
            Line.Quantity_Sum_Scan = Scan_Quantity;

            if (Line.Quantity_Sum == Line.Quantity_Sum_Scan)
            {
                Line.Line_State = WMS_Out_Task_Line_State_Enum.数量一致.ToString();
            }
            else if (Line.Quantity_Sum > Line.Quantity_Sum_Scan && Line.Quantity_Sum_Scan > 0)
            {
                Line.Line_State = WMS_Out_Task_Line_State_Enum.低于出货.ToString();
            }
            else if (Line.Quantity_Sum < Line.Quantity_Sum_Scan && Line.Quantity_Sum_Scan > 0)
            {
                Line.Line_State = WMS_Out_Task_Line_State_Enum.超出出货.ToString();
            }
            else
            {
                Line.Line_State = WMS_Out_Task_Line_State_Enum.还未扫码.ToString();
            }

            //扫描列表
            Line.Scan_List = Scan_List.OrderByDescending(x => x.Create_DT).ToList();

            return Line;
        }

        //配货指令完成
        public void WMS_Out_Pick_List_Sub_Finish_Pick_Location(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            if (db.WMS_Stock_Temp.Where(x => x.WMS_Out_Head_ID == Head.Head_ID).Any())
            {
                throw new Exception("该配货单已完成配货指令");
            }

            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = Head.Head_ID;
            List<WMS_Out_Task_Line> Line_List = Get_WMS_Out_Task_Line_List(MF);
            List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();

            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && MatSn_List.Contains(x.MatSn)).ToList();

            if (Scan_List.Any() == false)
            {
                throw new Exception("未设置取货库位");
            }

            int Scan_Quantity = 0;
            int Line_Quantity = 0;

            foreach (var x in Line_List)
            {
                Scan_Quantity = Scan_List.Where(c => c.MatSn == x.MatSn).Sum(c => c.Quantity);
                Line_Quantity = Line_List.Where(c => c.MatSn == x.MatSn).Sum(c => c.Quantity_Sum);

                if (Scan_Quantity != Line_Quantity)
                {
                    throw new Exception("型号" + x.MatSn + "的配货数量与取货数量不一致");
                }
            }

            //生成临时库存
            List<WMS_Stock_Temp> Stock_Temp_List = new List<WMS_Stock_Temp>();
            WMS_Stock_Temp Stock_Temp = new WMS_Stock_Temp();

            List<string> Location_List = Scan_List.Select(x => x.Scan_Location).Distinct().ToList();
            
            //有盘库任务不允许出库
            List<WMS_Stock_Task> Task_List = db.WMS_Stock_Task.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Status == WMS_Stock_Task_Enum.未盘库.ToString()).ToList();
            foreach (var Loc in Location_List)
            {
                if (Task_List.Where(c => c.Location == Loc).Any())
                {
                    throw new Exception("库位" + Loc + "存在盘库任务，不支持配货");
                }
            }

            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => Location_List.Contains(x.Location)).ToList();
            Stock_List_DB = Stock_List_DB.Where(x => MatSn_List.Contains(x.MatSn)).ToList();
            List<WMS_Stock> Stock_List_Sub = new List<WMS_Stock>();
            List<WMS_Stock> Stock_List = new List<WMS_Stock>();

            string WMS_Out_DT = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            int Quantity = 0;
            foreach (var x in Line_List)
            {
                foreach (var xx in Scan_List.Where(c => c.MatSn == x.MatSn).OrderBy(c => c.Create_DT).ToList())
                {
                    Quantity = xx.Quantity;
                    Stock_List_Sub = Stock_List_DB.Where(c => c.MatSn == xx.MatSn && c.Location == xx.Scan_Location).ToList();
                    foreach (var xxx in Stock_List_Sub.OrderBy(c => c.Quantity))
                    {
                        if (Quantity >= xxx.Quantity && Quantity > 0)
                        {
                            //创建临时库存
                            Stock_Temp = new WMS_Stock_Temp();
                            Stock_Temp.Stock_Temp_ID = MyGUID.NewGUID();
                            Stock_Temp.WMS_In_DT = xxx.WMS_In_DT;
                            Stock_Temp.WMS_Out_DT = xxx.WMS_Out_DT;
                            Stock_Temp.MatSn = xxx.MatSn;
                            Stock_Temp.MatName = xxx.MatName;
                            Stock_Temp.MatBrand = xxx.MatBrand;
                            Stock_Temp.MatUnit = xxx.MatUnit;
                            Stock_Temp.Quantity = xxx.Quantity;
                            Stock_Temp.Price = xxx.Price;
                            Stock_Temp.Package = xxx.Package;
                            Stock_Temp.Location = xxx.Location;
                            Stock_Temp.Case = xxx.Cases;
                            Stock_Temp.Location_Type = xxx.Location_Type;
                            Stock_Temp.WMS_In_Head_ID = xxx.Wms_In_Head_ID;
                            Stock_Temp.WMS_Out_Head_ID = Head.Head_ID;
                            Stock_Temp.LinkMainCID = xxx.LinkMainCID;
                            Stock_Temp.Link_Stock_ID = xxx.Stock_ID;
                            Stock_Temp_List.Add(Stock_Temp);

                            Quantity = Quantity - xxx.Quantity;

                            Stock_List.Add(xxx);
                        }
                        else if (Quantity < xxx.Quantity && Quantity > 0)
                        {
                            //创建临时库存
                            Stock_Temp = new WMS_Stock_Temp();
                            Stock_Temp.Stock_Temp_ID = MyGUID.NewGUID();
                            Stock_Temp.WMS_In_DT = xxx.WMS_In_DT;
                            Stock_Temp.WMS_Out_DT = xxx.WMS_Out_DT;
                            Stock_Temp.MatSn = xxx.MatSn;
                            Stock_Temp.MatName = xxx.MatName;
                            Stock_Temp.MatBrand = xxx.MatBrand;
                            Stock_Temp.MatUnit = xxx.MatUnit;
                            Stock_Temp.Quantity = Quantity;
                            Stock_Temp.Price = xxx.Price;
                            Stock_Temp.Package = WMS_Stock_Package_Enum.零头.ToString();
                            Stock_Temp.Location = xxx.Location;
                            Stock_Temp.Case = xxx.Cases;
                            Stock_Temp.Location_Type = xxx.Location_Type;
                            Stock_Temp.WMS_In_Head_ID = xxx.Wms_In_Head_ID;
                            Stock_Temp.WMS_Out_Head_ID = Head.Head_ID;
                            Stock_Temp.LinkMainCID = xxx.LinkMainCID;
                            Stock_Temp.Link_Stock_ID = xxx.Stock_ID;
                            Stock_Temp_List.Add(Stock_Temp);

                            //更新原有库存
                            xxx.Quantity = xxx.Quantity - Quantity;
                            xxx.WMS_Out_DT = WMS_Out_DT;
                            db.Entry(xxx).State = EntityState.Modified;
                            break;
                        }
                    }
                }
            }

            if (Stock_List.Any())
            {
                db.WMS_Stock.RemoveRange(Stock_List);
            }

            db.WMS_Stock_Temp.AddRange(Stock_Temp_List);
            MyDbSave.SaveChange(db);
        }

        //配货指令取消
        public void WMS_Out_Pick_List_Sub_Cancel_Pick_Location(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            if (Head.Status != WMS_Out_Global_State_Enum.待配货.ToString())
            {
                throw new Exception("当前状态 不支持取消配货指令");
            }

            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID).ToList();

            if (Scan_List.Where(x => x.Is_Chose == 1).Any())
            {
                throw new Exception("当前配货任务存在作业勾选");
            }

            db.WMS_Out_Pick_Scan.RemoveRange(Scan_List);

            List<WMS_Stock_Temp> Stock_Temp_List = db.WMS_Stock_Temp.Where(x => x.WMS_Out_Head_ID == Head.Head_ID).ToList();

            if (Stock_Temp_List.Any() == false) { throw new Exception("当前配货单未设置取货库位"); }

            List<Guid> StockID_Temp_List = Stock_Temp_List.Select(x => x.Link_Stock_ID).Distinct().ToList();
            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => StockID_Temp_List.Contains(x.Stock_ID)).ToList();

            WMS_Stock Stock = new WMS_Stock();
            WMS_Stock Stock_DB = new WMS_Stock();
            List<WMS_Stock> Stock_List = new List<WMS_Stock>();
            foreach (var x in Stock_Temp_List)
            {
                Stock_DB = Stock_List_DB.Where(c => c.Stock_ID == x.Link_Stock_ID).FirstOrDefault();

                if (Stock_DB == null)
                {
                    Stock = new WMS_Stock();
                    Stock.Stock_ID = x.Link_Stock_ID;
                    Stock.WMS_In_DT = x.WMS_In_DT;
                    Stock.WMS_Out_DT = x.WMS_Out_DT;
                    Stock.MatSn = x.MatSn;
                    Stock.MatName = x.MatName;
                    Stock.MatBrand = x.MatBrand;
                    Stock.MatUnit = x.MatUnit;
                    Stock.Quantity = x.Quantity;
                    Stock.Price = x.Price;
                    Stock.Package = x.Package;
                    Stock.Location = x.Location;
                    Stock.Cases = x.Case;
                    Stock.Location_Type = x.Location_Type;
                    Stock.Wms_In_Head_ID = x.WMS_In_Head_ID;
                    Stock.LinkMainCID = x.LinkMainCID;
                    Stock_List.Add(Stock);
                }
                else
                {
                    Stock_DB.Quantity = Stock_DB.Quantity + x.Quantity;
                    db.Entry(Stock_DB).State = EntityState.Modified;
                }
            }

            if (Stock_List.Any())
            {
                db.WMS_Stock.AddRange(Stock_List);
            }

            db.WMS_Stock_Temp.RemoveRange(Stock_Temp_List);

            MyDbSave.SaveChange(db);
        }

        //配货完成
        public void WMS_Out_Pick_List_Sub_Finish(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            if (Head.Status != WMS_Out_Global_State_Enum.待配货.ToString())
            {
                throw new Exception("当前配货状态异常");
            }

            if (db.WMS_Stock_Temp.Where(x => x.WMS_Out_Head_ID == Head.Head_ID).Any() == false)
            {
                throw new Exception("该配货单未完成取货库位设置");
            }

            if (db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && x.Is_Chose == 0).Any())
            {
                throw new Exception("当前配货未勾选完成");
            }

            //WMS_Stocktaking_Pick_Whether_Finish(Head);

            if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.自提.ToString())
            {
                //零售配货（自提）
                WMS_Out_Pick_List_Sub_Finish_Sub_Other(Head);
            }
            else
            {
                Head.Status = WMS_Out_Global_State_Enum.待验货.ToString();
                db.Entry(Head).State = EntityState.Modified;
            }

            MyDbSave.SaveChange(db);
        }

        //零售配货（自提）
        private void WMS_Out_Pick_List_Sub_Finish_Sub_Other(WMS_Out_Head Head)
        {
            List<string> MatSn_List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).Select(x => x.MatSn).Distinct().ToList();

            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && MatSn_List.Contains(x.MatSn)).ToList();

            List<string> Location_List = Scan_List.Select(x => x.Scan_Location).Distinct().ToList();
            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => Location_List.Contains(x.Location)).ToList();
            Stock_List_DB = Stock_List_DB.Where(x => MatSn_List.Contains(x.MatSn)).ToList();

            DateTime DT = DateTime.Now;

            //生成出入库记录
            List<WMS_Stock_Record> Stock_Record_List = new List<WMS_Stock_Record>();
            WMS_Stock_Record S_Record = new WMS_Stock_Record();
            WMS_Stock Stock_Temp = new WMS_Stock();
            foreach (var x in Scan_List)
            {
                Stock_Temp = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_Temp = Stock_Temp == null ? new WMS_Stock() : Stock_Temp;

                S_Record = new WMS_Stock_Record();
                S_Record.Stock_ID = MyGUID.NewGUID();
                S_Record.Quantity = x.Quantity;
                S_Record.Location = x.Scan_Location;
                S_Record.Create_DT = DT;
                S_Record.MatSn = x.MatSn;
                S_Record.MatName = Stock_Temp.MatName;
                S_Record.MatUnit = Stock_Temp.MatUnit;
                S_Record.MatBrand = Stock_Temp.MatBrand;
                S_Record.Price = Stock_Temp.Price;
                S_Record.Wms_Out_Head_ID = Head.Head_ID;
                S_Record.Customer = Head.Customer_Name;
                S_Record.LinkMainCID = Head.LinkMainCID;
                S_Record.Remark = WMS_Stock_Record_Remark_Enum.订单出库.ToString();
                S_Record.Work_Person = Head.Work_Down_Person;
                Stock_Record_List.Add(S_Record);
            }

            db.WMS_Stock_Record.AddRange(Stock_Record_List);

            Head.Status = WMS_Out_Global_State_Enum.已出库.ToString();
            db.Entry(Head).State = EntityState.Modified;

        }

        //判断是否动盘
        private void WMS_Stocktaking_Pick_Whether_Finish(WMS_Out_Head Head)
        {
            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID).ToList();
            List<string> Loc_Str_List = Scan_List.Select(x => x.Scan_Location).Distinct().ToList();

            List<WMS_Stock_Task> Task_List = db.WMS_Stock_Task.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Property == WMS_Stock_Task_Property_Enum.配货动盘.ToString() && x.Status == WMS_Stock_Task_Enum.未盘库.ToString()).ToList();
            if (Task_List.Where(x => Loc_Str_List.Contains(x.Location) && x.Type == Type_Enum.端数.ToString()).Any())
            {
                throw new Exception("存在端数库位未盘库");
            }
        }

        //简单页面勾选
        public void WMS_Out_Task_Preview_Pick_Scan_Check(Guid TaskID)
        {
            WMS_Out_Pick_Scan Scan = db.WMS_Out_Pick_Scan.Find(TaskID);
            if (Scan == null) { throw new Exception("勾选存在错误"); }

            WMS_Out_Head Head = db.WMS_Out_Head.Find(Scan.Link_TaskID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            if (Head.Status != WMS_Out_Global_State_Enum.待配货.ToString())
            {
                throw new Exception("此任务单已完成配货，不支持操作");
            }

            if (Scan.Is_Chose_Sim == 1)
            {
                Scan.Is_Chose_Sim = 0;
            }
            else if (Scan.Is_Chose_Sim == 0)
            {
                Scan.Is_Chose_Sim = 1;
            }

            db.Entry(Scan).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        //页面勾选
        public void WMS_Out_Task_Preview_Pick_Scan_Choose(Guid TaskID)
        {
            WMS_Out_Pick_Scan Scan = db.WMS_Out_Pick_Scan.Find(TaskID);
            if (Scan == null) { throw new Exception("勾选存在错误"); }

            WMS_Out_Head Head = db.WMS_Out_Head.Find(Scan.Link_TaskID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            if (Head.Status != WMS_Out_Global_State_Enum.待配货.ToString())
            {
                throw new Exception("此任务单已完成配货，不支持操作");
            }

            //创建配货动盘任务
            Create_WMS_Stocktaking(Scan, Head);

            Scan.Is_Chose = 1;
            db.Entry(Scan).State = EntityState.Modified;

            MyDbSave.SaveChange(db);
        }

        //页面取消勾选
        public void WMS_Out_Task_Preview_Pick_Scan_Cancel_Choose(Guid TaskID)
        {
            WMS_Out_Pick_Scan Scan = db.WMS_Out_Pick_Scan.Find(TaskID);
            if (Scan == null) { throw new Exception("勾选存在错误"); }

            WMS_Out_Head Head = db.WMS_Out_Head.Find(Scan.Link_TaskID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            if (Head.Status != WMS_Out_Global_State_Enum.待配货.ToString())
            {
                throw new Exception("此任务单已完成配货，不支持操作");
            }

            //删除配货动盘任务
            Delete_WMS_Stocktaking(Scan, Head);

            Scan.Is_Chose = 0;
            db.Entry(Scan).State = EntityState.Modified;
            MyDbSave.SaveChange(db);

        }

        //创建配货动盘任务
        private void Create_WMS_Stocktaking(WMS_Out_Pick_Scan Scan, WMS_Out_Head Head)
        {
            List<Guid> Head_ID_List = db.WMS_Out_Head.Where(x => x.LinkMainCID == Scan.LinkMainCID && x.Status == WMS_Out_Global_State_Enum.待配货.ToString()).Select(x => x.Head_ID).ToList();
            List<WMS_Out_Pick_Scan> Pick_Scan_List = db.WMS_Out_Pick_Scan.Where(x => Head_ID_List.Contains(x.Link_TaskID)).ToList();

            int Total_Pick_Quantity = Pick_Scan_List.Where(x => x.Scan_Location == Scan.Scan_Location && x.MatSn == Scan.MatSn).Sum(x => x.Quantity);
            int Finish_Pick_Quantity = Pick_Scan_List.Where(x => x.Scan_Location == Scan.Scan_Location && x.MatSn == Scan.MatSn && x.Is_Chose == 1).Sum(x => x.Quantity);
            int Remain_Pick_Quantity = Total_Pick_Quantity - Finish_Pick_Quantity;

            if (Remain_Pick_Quantity <= Scan.Quantity)
            {
                List<WMS_Stocktaking> Stocktaking_List = new List<WMS_Stocktaking>();
                WMS_Stocktaking Stocktaking = new WMS_Stocktaking();
                DateTime Create_DT = DateTime.Now;
                WMS_Stock_Task Task = new WMS_Stock_Task();

                WMS_Location Loc = db.WMS_Location.Where(x => x.Location == Scan.Scan_Location).FirstOrDefault();
                Loc = Loc == null ? new WMS_Location() : Loc;

                List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Loc.LinkMainCID && x.Location == Loc.Location).ToList();
                WMS_Stock Stock = new WMS_Stock();
                Task = db.WMS_Stock_Task.Where(x => x.LinkMainCID == Scan.LinkMainCID && x.Location == Loc.Location && x.Status == WMS_Stock_Task_Enum.未盘库.ToString() && x.Property == WMS_Stock_Task_Property_Enum.配货动盘.ToString()).FirstOrDefault();
                if (Loc.Type == Type_Enum.整箱.ToString() && Stock_List_DB.Any())
                {
                    if (Task == null)
                    {
                        Task = new WMS_Stock_Task();
                        Task.Task_ID = MyGUID.NewGUID();
                        Task.LinkMainCID = Loc.LinkMainCID;
                        Task.Create_DT = Create_DT;
                        Task.Location = Loc.Location;
                        Task.Type = Loc.Type;
                        Task.Status = WMS_Stock_Task_Enum.未盘库.ToString();
                        Task.Property = WMS_Stock_Task_Property_Enum.配货动盘.ToString();
                        Task.Link_HeadID = Head.Head_ID;
                        Task.Work_Person = Head.Work_Down_Person;
                        db.WMS_Stock_Task.Add(Task);

                        //创建底盘信息
                        foreach (var x in Stock_List_DB)
                        {
                            Stocktaking = new WMS_Stocktaking();
                            Stocktaking.Stocktaking_ID = MyGUID.NewGUID();
                            Stocktaking.MatSn = x.MatSn;
                            Stocktaking.MatBrand = x.MatBrand;
                            Stocktaking.Quantity = x.Quantity;
                            Stocktaking.Location = x.Location;
                            Stocktaking.Create_DT = Create_DT;
                            Stocktaking.Link_TaskID = Task.Task_ID;
                            Stocktaking.LinkMainCID = Task.LinkMainCID;
                            Stocktaking.Work_Person = WMS_Stock_Task_Property_Enum.配货动盘.ToString();
                            Stocktaking.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
                            Stocktaking_List.Add(Stocktaking);
                        }

                        db.WMS_Stocktaking.AddRange(Stocktaking_List);
                    }
                }
                else if (Loc.Type == Type_Enum.端数.ToString() && Stock_List_DB.Where(x => x.MatSn == Scan.MatSn).Any())
                {
                    if (Task == null)
                    {
                        Task = new WMS_Stock_Task();
                        Task.Task_ID = MyGUID.NewGUID();
                        Task.LinkMainCID = Loc.LinkMainCID;
                        Task.Create_DT = Create_DT;
                        Task.Location = Loc.Location;
                        Task.Type = Loc.Type;
                        Task.Status = WMS_Stock_Task_Enum.未盘库.ToString();
                        Task.Property = WMS_Stock_Task_Property_Enum.配货动盘.ToString();
                        Task.Work_Person = Head.Work_Down_Person;
                        Task.Link_HeadID = Head.Head_ID;
                        db.WMS_Stock_Task.Add(Task);

                        Stocktaking = new WMS_Stocktaking();
                        Stocktaking.Stocktaking_ID = MyGUID.NewGUID();
                        Stocktaking.MatSn = Scan.MatSn;
                        Stocktaking.Quantity = Stock_List_DB.Where(x => x.MatSn == Scan.MatSn).Sum(x => x.Quantity);
                        Stocktaking.Location = Task.Location;
                        Stocktaking.Create_DT = Create_DT;
                        Stocktaking.Link_TaskID = Task.Task_ID;
                        Stocktaking.LinkMainCID = Task.LinkMainCID;
                        Stocktaking.Work_Person = Head.Work_Down_Person;
                        Stocktaking.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
                        db.WMS_Stocktaking.Add(Stocktaking);
                    }
                    else
                    {
                        if (Task.Link_HeadID == Head.Head_ID)
                        {
                            Stocktaking = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Task.Task_ID && x.MatSn == Scan.MatSn).FirstOrDefault();
                            if (Stocktaking == null)
                            {
                                Stocktaking = new WMS_Stocktaking();
                                Stocktaking.Stocktaking_ID = MyGUID.NewGUID();
                                Stocktaking.MatSn = Scan.MatSn;
                                Stocktaking.Quantity = Stock_List_DB.Where(x => x.MatSn == Scan.MatSn).Sum(x => x.Quantity);
                                Stocktaking.Location = Task.Location;
                                Stocktaking.Create_DT = Create_DT;
                                Stocktaking.Link_TaskID = Task.Task_ID;
                                Stocktaking.LinkMainCID = Task.LinkMainCID;
                                Stocktaking.Work_Person = Head.Work_Down_Person;
                                Stocktaking.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
                                db.WMS_Stocktaking.Add(Stocktaking);
                            }
                        }
                    }
                }
            }
        }

        //删除配货动盘任务
        private void Delete_WMS_Stocktaking(WMS_Out_Pick_Scan Scan, WMS_Out_Head Head)
        {
            WMS_Location Loc = db.WMS_Location.Where(x => x.Location == Scan.Scan_Location).FirstOrDefault();
            Loc = Loc == null ? new WMS_Location() : Loc;

            WMS_Stock_Task Task = db.WMS_Stock_Task.Where(x => x.Link_HeadID == Head.Head_ID && x.Location == Scan.Scan_Location).FirstOrDefault();
            if (Task != null)
            {
                if (Task.Status == WMS_Stock_Task_Enum.已盘库.ToString())
                {
                    throw new Exception("生成的动盘任务已完成动盘，不支持删除");
                }

                List<WMS_Stocktaking> Stocktaking_List = new List<WMS_Stocktaking>();
                List<WMS_Stocktaking> Stocktaking_List_Sub = new List<WMS_Stocktaking>();

                List<WMS_Stocktaking_Scan> Stocktaking_Scan_List = new List<WMS_Stocktaking_Scan>();

                Stocktaking_Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID).ToList();
                if (Stocktaking_Scan_List.Any())
                {
                    throw new Exception("生成的动盘任务已进行动盘，不支持删除");
                }

                Stocktaking_List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Task.Task_ID).ToList();

                if (Loc.Type == Type_Enum.整箱.ToString())
                {
                    db.WMS_Stock_Task.Remove(Task);
                    db.WMS_Stocktaking.RemoveRange(Stocktaking_List);
                }
                else if (Loc.Type == Type_Enum.端数.ToString())
                {
                    Stocktaking_List_Sub = Stocktaking_List.Where(x => x.MatSn == Scan.MatSn).ToList();
                    if (Stocktaking_List_Sub.Any())
                    {
                        if (Stocktaking_List_Sub.Count() == Stocktaking_List.Count())
                        {
                            db.WMS_Stock_Task.Remove(Task);
                            db.WMS_Stocktaking.RemoveRange(Stocktaking_List);
                        }
                        else
                        {
                            db.WMS_Stocktaking.RemoveRange(Stocktaking_List_Sub);
                        }
                    }
                }
            }
        }

        //页面勾选取货库位
        public void WMS_Out_Task_Preview_Pick_Location_Choose(Guid Head_ID, string MatSn, string Location)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            if (db.WMS_Stock_Temp.Where(x => x.WMS_Out_Head_ID == Head.Head_ID).Any())
            {
                throw new Exception("该配货单已完成配货指令，不支持勾选");
            }

            MatSn = MatSn.Trim();
            Location = Location.Trim();
            if (string.IsNullOrEmpty(Head.Work_Down_Person)) { throw new Exception("未设置配货人"); }

            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && x.MatSn == MatSn).ToList();
            if (Scan_List.Where(x => x.Scan_Location == Location).Any())
            {
                throw new Exception("库位" + Location + "已设置取货");
            }

            WMS_Out_Pick_Scan Scan = new WMS_Out_Pick_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.LinkMainCID = Head.LinkMainCID;
            Scan.Link_TaskID = Head.Head_ID;
            Scan.Create_DT = DateTime.Now;
            Scan.MatSn = MatSn;
            Scan.Scan_Person = Head.Work_Down_Person;
            Scan.Scan_Location = Location;

            if (db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Location == Scan.Scan_Location).Any() == false) { throw new Exception("该库位不存在产品"); }

            if (db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Location == Scan.Scan_Location && x.MatSn == MatSn).Any() == false) { throw new Exception("该库位无此型号产品"); }

            int Line_Quantity = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID && x.MatSn == MatSn).Sum(x => x.Quantity);

            if (db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && x.MatSn == Scan.MatSn).Any())
            {
                Line_Quantity = Line_Quantity - db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && x.MatSn == Scan.MatSn).Sum(x => x.Quantity);
            }

            if (Line_Quantity <= 0) { throw new Exception("取货数量已满足，请勿添加"); }

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Location == Scan.Scan_Location && x.MatSn == MatSn).ToList();
            int Stock_Quantity = Stock_List.Sum(x => x.Quantity);

            ////判断是否完成底盘任务
            //if (db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && x.MatSn == MatSn && x.Status == WMS_Out_Scan_Status_Enum.未完成.ToString()).Any())
            //{
            //    throw new Exception("该产品型号存在底盘未完成");
            //}

            ////判断已存在配货任务
            //if (db.WMS_Out_Pick_Scan.Where(x => x.Scan_ID != Scan.Scan_ID && x.Scan_Location == Scan.Scan_Location && x.Status == WMS_Out_Scan_Status_Enum.未完成.ToString()).Any())
            //{
            //    throw new Exception("该库位已创建配货任务");
            //}

            //判断是否底盘
            if (Line_Quantity < Stock_Quantity)
            {
                //创建底盘信息
                WMS_Stocktaking Stocktaking = new WMS_Stocktaking();
                Stocktaking.Stocktaking_ID = MyGUID.NewGUID();
                Stocktaking.MatSn = MatSn;
                Stocktaking.MatBrand = Stock_List.FirstOrDefault().MatBrand;
                Stocktaking.Quantity = Stock_Quantity - Line_Quantity;
                Stocktaking.Location = Scan.Scan_Location;
                Stocktaking.Create_DT = DateTime.Now;
                Stocktaking.Link_TaskID = Scan.Scan_ID;
                Stocktaking.LinkMainCID = Head.LinkMainCID;
                Stocktaking.Task_Bat_No = Head.Task_Bat_No_Str;
                Stocktaking.Work_Person = Head.Work_Down_Person;
                Stocktaking.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
                db.WMS_Stocktaking.Add(Stocktaking);

                Scan.Quantity = Line_Quantity;
                Scan.Status = WMS_Out_Scan_Status_Enum.未完成.ToString();

            }
            else
            {
                Scan.Quantity = Stock_Quantity;
                Scan.Status = WMS_Out_Scan_Status_Enum.已完成.ToString();
            }

            db.WMS_Out_Pick_Scan.Add(Scan);
            MyDbSave.SaveChange(db);
        }

        //页面删除取货库位
        public void WMS_Out_Task_Preview_Pick_Location_Choose(Guid Scan_ID)
        {
            WMS_Out_Pick_Scan Pick_Scan = db.WMS_Out_Pick_Scan.Find(Scan_ID);
            if (Pick_Scan == null) { throw new Exception("取货库位不存在"); }

            if (db.WMS_Stock_Temp.Where(x => x.WMS_Out_Head_ID == Pick_Scan.Link_TaskID).Any())
            {
                throw new Exception("该配货单已完成配货指令，不支持删除");
            }

            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Pick_Scan.Scan_ID).ToList();

            db.WMS_Stocktaking.RemoveRange(Stocktaking_List);
            db.WMS_Out_Pick_Scan.Remove(Pick_Scan);
            MyDbSave.SaveChange(db);
        }

        //页面设置取货库位
        public void WMS_Out_Task_Preview_Pick_Location_Set(Guid Scan_ID, int Quantity)
        {
            WMS_Out_Pick_Scan Pick_Scan = db.WMS_Out_Pick_Scan.Find(Scan_ID);
            if (Pick_Scan == null) { throw new Exception("取货库位不存在"); }

            WMS_Out_Head Head = db.WMS_Out_Head.Find(Pick_Scan.Link_TaskID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            if (db.WMS_Stock_Temp.Where(x => x.WMS_Out_Head_ID == Head.Head_ID).Any())
            {
                throw new Exception("该配货单已完成配货指令，不支持设置");
            }

            if (Quantity <= 0) { throw new Exception("请设置正确的取货数量"); }

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == Pick_Scan.LinkMainCID && x.Location == Pick_Scan.Scan_Location && x.MatSn == Pick_Scan.MatSn).ToList();
            int Quantity_DB = Stock_List.Sum(x => x.Quantity);

            //不能大于库存数量
            if (Quantity_DB < Quantity) { throw new Exception("库存数量为" + Quantity_DB + ",请设置正确的取货数量"); }

            int Line_Quantity = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID && x.MatSn == Pick_Scan.MatSn).Sum(x => x.Quantity);
            if (db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && x.MatSn == Pick_Scan.MatSn).Any())
            {
                Line_Quantity = Line_Quantity - db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && x.MatSn == Pick_Scan.MatSn).Sum(x => x.Quantity);
                Line_Quantity = Line_Quantity + Pick_Scan.Quantity;
            }

            if (Line_Quantity < Quantity) { throw new Exception("取货数量多于需求数量，请填写正确的取货数量"); }

            Pick_Scan.Quantity = Quantity;

            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Pick_Scan.Scan_ID).ToList();
            db.WMS_Stocktaking.RemoveRange(Stocktaking_List);

            //判断是否底盘
            if (Quantity < Quantity_DB)
            {
                //创建底盘信息
                WMS_Stocktaking Stocktaking = new WMS_Stocktaking();
                Stocktaking.Stocktaking_ID = MyGUID.NewGUID();
                Stocktaking.MatSn = Pick_Scan.MatSn;
                Stocktaking.MatBrand = Stock_List.FirstOrDefault().MatBrand;
                Stocktaking.Quantity = Quantity_DB - Quantity;
                Stocktaking.Location = Pick_Scan.Scan_Location;
                Stocktaking.Create_DT = DateTime.Now;
                Stocktaking.Link_TaskID = Pick_Scan.Scan_ID;
                Stocktaking.LinkMainCID = Pick_Scan.LinkMainCID;
                Stocktaking.Task_Bat_No = Head.Task_Bat_No_Str;
                Stocktaking.Work_Person = Head.Work_Down_Person;
                Stocktaking.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
                db.WMS_Stocktaking.Add(Stocktaking);

                Pick_Scan.Status = WMS_Out_Scan_Status_Enum.未完成.ToString();
            }
            else
            {
                Pick_Scan.Quantity = Quantity_DB;
                Pick_Scan.Status = WMS_Out_Scan_Status_Enum.已完成.ToString();
            }

            db.Entry(Pick_Scan).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        //清空扫描库位列表
        public void WMS_Out_Pick_List_Sub_With_Location_Reset(Guid Head_ID, string MatSn)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("配货任务不存在"); }

            if (Head.Status != WMS_Out_Global_State_Enum.待配货.ToString())
            {
                throw new Exception("当前状态 不支持清空取货库位");
            }

            MatSn = MatSn.Trim();

            List<WMS_Out_Pick_Scan> Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID && x.MatSn == MatSn).ToList();
            List<Guid> ID_List = Scan_List.Select(x => x.Scan_ID).Distinct().ToList();
            List<WMS_Out_Pick_Scan> Scan_List_Sub = new List<WMS_Out_Pick_Scan>();

            //List<WMS_Stocktaking_Scan> Stocktaking_Scan_List = db.WMS_Stocktaking_Scan.Where(x => ID_List.Contains(x.Link_TaskID)).ToList();

            //foreach (var x in Scan_List)
            //{
            //    if (Stocktaking_Scan_List.Where(c => c.Link_TaskID == x.Scan_ID).Any() == false)
            //    {
            //        Scan_List_Sub.Add(x);
            //    }
            //}

            //List<Guid> ID_List_Sub = Scan_List_Sub.Select(x => x.Scan_ID).Distinct().ToList();
            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => ID_List.Contains(x.Link_TaskID)).ToList();

            db.WMS_Stocktaking.RemoveRange(Stocktaking_List);

            db.WMS_Out_Pick_Scan.RemoveRange(Scan_List_Sub);

            MyDbSave.SaveChange(db);
        }
    }

    //出库验货
    public partial class WmsService : IWmsService
    {
        public void Cancel_WMS_Out_Head_Inspection(Guid HeadID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(HeadID);
            if (Head == null) { throw new Exception("任务不存在"); }

            if (Head.Status == WMS_Out_Global_State_Enum.已出库.ToString())
            {
                throw new Exception("该任务已完成出库，不支持撤销！");
            }

            //配货记录
            List<WMS_Out_Pick_Scan> Pick_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID).ToList();
            foreach (var x in Pick_List)
            {
                x.Is_Chose = 0;
                db.Entry(x).State = EntityState.Modified;
            }

            //验货记录
            List<WMS_Out_Scan> Scan_List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            //验货快递信息
            List<WMS_Track> Track_List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            if (Track_List.Any())
            {
                db.WMS_Track.RemoveRange(Track_List);
            }

            if (Scan_List.Any())
            {
                db.WMS_Out_Scan.RemoveRange(Scan_List);
            }

            Head.Status = WMS_Out_Global_State_Enum.待配货.ToString();
            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public WMS_Out_Task Get_WMS_Out_Task_Item(WMS_Out_Filter MF)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(MF.LinkHeadID);
            Head = Head == null ? new WMS_Out_Head() : Head;
            List<WMS_Out_Line> List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            WMS_Out_Task T = new WMS_Out_Task();
            T.Head_ID = Head.Head_ID;
            T.Task_Bat_No_Str = Head.Task_Bat_No_Str;
            T.Create_DT = Head.Create_DT;
            T.Create_Person = Head.Create_Person;
            T.Logistics_Company = Head.Logistics_Company;
            T.Logistics_Mode = Head.Logistics_Mode;
            T.Brand = Head.Brand;
            T.Logistics_Cost_Type = Head.Logistics_Cost_Type;
            T.Global_State = Head.Status;
            T.Scan_Mat_Type = Head.Scan_Mat_Type;
            T.Work_Out_Person = Head.Work_Out_Person;
            T.Work_Down_Person = Head.Work_Down_Person;
            T.Customer_Name = Head.Customer_Name;
            T.Customer_Tel = Head.Customer_Tel;
            T.Customer_Address = Head.Customer_Address;
            T.Total_Cases = Head.Total_Cases;
            T.Line_List = new List<WMS_Out_Task_Line>();
            var Line_Group = from x in List
                             group x by x.MatSn into G
                             select new
                             {
                                 MatSn = G.Key,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                                 Line_No = G.FirstOrDefault().Line_No,
                                 Line_Count = G.Count()
                             };

            List<WMS_Out_Scan> List_Scan = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            int i = 0;
            WMS_Out_Task_Line T_Line = new WMS_Out_Task_Line();
            List<WMS_Out_Scan> List_Scan_Sub = new List<WMS_Out_Scan>();
            foreach (var x in Line_Group.OrderBy(x => x.Line_No).ToList())
            {
                i++;
                List_Scan_Sub = List_Scan.Where(c => c.MatSn == x.MatSn).ToList();
                T_Line = new WMS_Out_Task_Line();
                T_Line.Line_No = i;
                T_Line.MatSn = x.MatSn;
                T_Line.Quantity_Sum = x.Quantity_Sum;
                T_Line.Quantity_Sum_Scan = List_Scan_Sub.Sum(c => c.Scan_Quantity);

                if (T_Line.Quantity_Sum == T_Line.Quantity_Sum_Scan)
                {
                    T_Line.Line_State = WMS_Out_Task_Line_State_Enum.数量一致.ToString();
                }
                else if (T_Line.Quantity_Sum > T_Line.Quantity_Sum_Scan && T_Line.Quantity_Sum_Scan > 0)
                {
                    T_Line.Line_State = WMS_Out_Task_Line_State_Enum.低于出货.ToString();
                }
                else if (T_Line.Quantity_Sum < T_Line.Quantity_Sum_Scan && T_Line.Quantity_Sum_Scan > 0)
                {
                    T_Line.Line_State = WMS_Out_Task_Line_State_Enum.超出出货.ToString();
                }
                else
                {
                    T_Line.Line_State = WMS_Out_Task_Line_State_Enum.还未扫码.ToString();
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
            List<WMS_Out_Scan> List_Scan_Other = List_Scan.Where(x => MatSn_Line_ALL.Contains(x.MatSn) == false).ToList();
            foreach (var MatSn in List_Scan_Other.Select(x => x.MatSn).Distinct().ToList())
            {
                i++;
                List_Scan_Sub = List_Scan.Where(c => c.MatSn == MatSn).ToList();
                T_Line = new WMS_Out_Task_Line();
                T_Line.Line_No = 0;
                T_Line.MatSn = MatSn;
                T_Line.Quantity_Sum = 0;
                T_Line.Quantity_Sum_Scan = List_Scan_Sub.Sum(c => c.Scan_Quantity);
                T_Line.Tray_No_List = List_Scan_Sub.Select(c => c.Tray_No).Distinct().OrderBy(c => c).ToList();
                T_Line.Line_State = WMS_Out_Task_Line_State_Enum.多出型号.ToString();
                foreach (var Tray in T_Line.Tray_No_List)
                {
                    T_Line.Tray_No_List_Str += Tray + ",";
                }
                T_Line.Tray_No_List_Str = CommonLib.Trim_End_Char(T_Line.Tray_No_List_Str);
                T.Line_List.Add(T_Line);
            }

            MF.Return_Info = string.Empty;
            int Error_A_Count = T.Line_List.Where(x => x.Line_State == WMS_Out_Task_Line_State_Enum.还未扫码.ToString()).Count();
            int Error_B_Count = T.Line_List.Where(x => x.Line_State == WMS_Out_Task_Line_State_Enum.低于出货.ToString()).Count();
            int Error_C_Count = T.Line_List.Where(x => x.Line_State == WMS_Out_Task_Line_State_Enum.超出出货.ToString()).Count();
            int Error_D_Count = T.Line_List.Where(x => x.Line_State == WMS_Out_Task_Line_State_Enum.多出型号.ToString()).Count();

            if (Error_A_Count > 0) { MF.Return_Info += "剩余<strong>" + Error_A_Count + "</strong>项 还未扫码，"; }
            if (Error_B_Count > 0) { MF.Return_Info += "发现<strong>" + Error_B_Count + "</strong>项 低于出货，"; }
            if (Error_C_Count > 0) { MF.Return_Info += "发现<strong>" + Error_C_Count + "</strong>项 超出出货，"; }
            if (Error_D_Count > 0) { MF.Return_Info += "发现<strong>" + Error_D_Count + "</strong>项 多出型号，"; }

            if (Enum.GetNames(typeof(WMS_Out_Task_Line_State_Enum)).Where(x => x == MF.Line_Status).Any())
            {
                T.Line_List = T.Line_List.Where(x => x.Line_State == MF.Line_Status).ToList();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                T.Line_List = T.Line_List.Where(x => x.MatSn.ToLower().Contains(MF.MatSn.ToLower())).ToList();
            }

            T.Line_List = T.Line_List.OrderBy(x => x.Line_No).ToList();

            //快递或车辆信息
            T.Track_List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).OrderBy(x => x.Tracking_No).ToList();

            //原表单信息
            T.WMS_Out_Line_List = List.OrderBy(x => x.Line_No).ToList();

            //装箱信息
            T.Group_Tray_List = this.Get_WMS_Out_Task_Group_Tray_List(List_Scan);

            //扫描列表
            T.Scan_List = List_Scan.OrderByDescending(x => x.Create_DT).ToList();

            return T;
        }

        public WMS_Out_Line Get_WMS_Out_Line_Item(Guid Head_ID, string MatSn)
        {
            if (string.IsNullOrEmpty(MatSn)) { throw new Exception("产品型号为空"); }

            WMS_Out_Line Line = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head_ID && x.MatSn == MatSn).FirstOrDefault();
            Line = Line == null ? new WMS_Out_Line() : Line;
            return Line;
        }

        public WMS_Out_Line Get_WMS_Out_Line_Item(Guid LineID, int Quantity)
        {
            if (Quantity <= 0) { throw new Exception("产品数量不小于0"); }

            WMS_Out_Line Line = db.WMS_Out_Line.Find(LineID);
            Line = Line == null ? new WMS_Out_Line() : Line;
            Line.Quantity = Quantity;
            string Str = "HONGEN/" + Line.MatSn + "/" + Line.Quantity + "/" + Line.MatBrand;
            Line.QRCode_Path = QRCode.CreateQRCode_With_No(Str, Line.Line_ID);
            return Line;
        }

        public void Set_WMS_Out_Head_With_Scan_Type(WMS_Out_Head Head)
        {
            if (string.IsNullOrEmpty(Head.Scan_Mat_Type)) { throw new Exception("未选择验货方式！"); }

            if (Head.Total_Cases <= 0) { throw new Exception("总箱数需大于零！"); }

            WMS_Out_Head Head_DB = db.WMS_Out_Head.Find(Head.Head_ID);
            Head_DB = Head_DB == null ? new WMS_Out_Head() : Head_DB;
            Head_DB.Scan_Mat_Type = Head.Scan_Mat_Type.Trim();
            Head_DB.Total_Cases = Head.Total_Cases;
            db.Entry(Head_DB).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public List<WMS_Out_Scan> Get_WMS_Out_Scan_List(Guid Head_ID, string MatSn)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null"); }
            if (string.IsNullOrEmpty(MatSn)) { throw new Exception("产品型号为空"); }
            List<WMS_Out_Scan> Scan_List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.MatSn == MatSn).ToList();
            return Scan_List;
        }

        public void Reset_WMS_Out_Scan_By_MatSn(Guid Head_ID, string MatSn)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null"); }
            if (string.IsNullOrEmpty(MatSn)) { throw new Exception("产品型号为空"); }
            List<WMS_Out_Scan> Scan_List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.MatSn == MatSn).ToList();

            if (Head.Status != WMS_Out_Global_State_Enum.待验货.ToString())
            {
                throw new Exception("产品已完成验货，不支持重置扫描");
            }

            db.WMS_Out_Scan.RemoveRange(Scan_List);
            MyDbSave.SaveChange(db);
        }

        //简单页面勾选
        public void WMS_Out_Task_Preview_Source_Check(Guid LineID)
        {
            WMS_Out_Line Line = db.WMS_Out_Line.Find(LineID);
            if (Line == null) { throw new Exception("勾选存在错误"); }

            if (Line.Is_Chose == 1)
            {
                Line.Is_Chose = 0;
            }
            else if (Line.Is_Chose == 0)
            {
                Line.Is_Chose = 1;
            }

            db.Entry(Line).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        //完成验货
        public void WMS_Out_Task_To_WMS_Stock_Check(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Out_Head() : Head;

            if (Head.Status != WMS_Out_Global_State_Enum.待验货.ToString())
            {
                throw new Exception("此任务单状态异常");
            }

            WMS_Out_Filter MF = new WMS_Out_Filter();
            MF.LinkHeadID = Head.Head_ID;
            WMS_Out_Task T_Head = this.Get_WMS_Out_Task_Item(MF);

            if (T_Head.Line_List.Where(x => x.Line_State != WMS_Out_Task_Line_State_Enum.数量一致.ToString()).Any())
            {
                throw new Exception("验货未完成，无法发送");
            }

            if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.自送.ToString() && db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).Any() == false)
            {
                throw new Exception("未填写车辆信息，无法发送");
            }

            List<WMS_Out_Scan> Scan_List = T_Head.Scan_List;
            int Box_Count = 0;

            if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
            {
                if (Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).Any())
                {
                    Box_Count += Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).Count();
                }

                if (Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).Any())
                {
                    var Line_Group = from x in Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.零头.ToString())
                                     group x by new { x.Case_No, x.Tray_No } into G
                                     select new
                                     {
                                         Case_No = G.Key.Case_No,
                                         Tray_No = G.Key.Tray_No,
                                     };

                    Box_Count += Line_Group.Count();
                }

            }
            else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
            {
                Box_Count = Scan_List.Select(x => x.Tray_No).Distinct().Count();
            }

            if (Box_Count != Head.Total_Cases)
            {
                throw new Exception("系统验货箱数与实际验货箱数不一致！");
            }

            ISentEmailService IS = new SentEmailService();

            if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.自送.ToString())
            {
                Head.Status = WMS_Out_Global_State_Enum.待出库.ToString();
                db.Entry(Head).State = EntityState.Modified;

                IS.Sent_To_Sales_With_WMS_Out_Inspection(Head.Head_ID);
            }
            else
            {
                Head.Status = WMS_Out_Global_State_Enum.待包装.ToString();
                db.Entry(Head).State = EntityState.Modified;
            }

            MyDbSave.SaveChange(db);

        }

        //完成包装出库
        public void WMS_Out_Task_To_WMS_Stock(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Out_Head() : Head;

            if (Head.Status != WMS_Out_Global_State_Enum.待包装.ToString())
            {
                throw new Exception("此任务单状态异常");
            }
            
            //if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.快递.ToString() || Head.Logistics_Mode == Logistics_Out_Mode_Enum.物流.ToString())
            //{
            //    if (db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).Any() == false)
            //    {
            //        throw new Exception("此任务单未添加快递物流信息");
            //    }
            //}

            //出库消库存
            WMS_Out_Task_To_WMS_Stock(Head);

            Head.Status = WMS_Out_Global_State_Enum.已出库.ToString();
            db.Entry(Head).State = EntityState.Modified;

            ISentEmailService IS = new SentEmailService();
            IS.Sent_To_Sales_With_WMS_Out_Finish(Head.Head_ID);

            MyDbSave.SaveChange(db);
        }
        
        //完成出库
        public void WMS_Out_Task_To_WMS_Stock_Check_Again(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Out_Head() : Head;

            if (Head.Status != WMS_Out_Global_State_Enum.待出库.ToString())
            {
                throw new Exception("此任务单状态异常");
            }

            //出库消库存
            WMS_Out_Task_To_WMS_Stock(Head);

            Head.Status = WMS_Out_Global_State_Enum.已出库.ToString();
            db.Entry(Head).State = EntityState.Modified;

            ISentEmailService IS = new SentEmailService();
            IS.Sent_To_Sales_With_WMS_Out_Finish(Head.Head_ID);
            MyDbSave.SaveChange(db);
        }

        //出库消库存
        private void WMS_Out_Task_To_WMS_Stock(WMS_Out_Head Head)
        {
            //执行出库并写入库存表
            List<WMS_Stock_Temp> Stock_Temp_List = db.WMS_Stock_Temp.Where(x => x.WMS_Out_Head_ID == Head.Head_ID).ToList();
            WMS_Stock_Temp Stock_Temp = new WMS_Stock_Temp();

            List<WMS_Stock_Record> Stock_Record_List = new List<WMS_Stock_Record>();
            WMS_Stock_Record S_Record = new WMS_Stock_Record();

            List<WMS_Out_Pick_Scan> Pick_Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.Link_TaskID == Head.Head_ID).ToList();

            DateTime DT = DateTime.Now;
            foreach (var x in Pick_Scan_List)
            {
                Stock_Temp = Stock_Temp_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_Temp = Stock_Temp == null ? new WMS_Stock_Temp() : Stock_Temp;

                S_Record = new WMS_Stock_Record();
                S_Record.Stock_ID = MyGUID.NewGUID();
                S_Record.Quantity = x.Quantity;
                S_Record.Location = x.Scan_Location;
                S_Record.Create_DT = DT;
                S_Record.MatSn = x.MatSn;
                S_Record.MatName = Stock_Temp.MatName;
                S_Record.MatUnit = Stock_Temp.MatUnit;
                S_Record.MatBrand = Stock_Temp.MatBrand;
                S_Record.Price = Stock_Temp.Price;
                S_Record.Wms_Out_Head_ID = Head.Head_ID;
                S_Record.Customer = Head.Customer_Name;
                S_Record.LinkMainCID = Head.LinkMainCID;
                S_Record.Remark = WMS_Stock_Record_Remark_Enum.订单出库.ToString();
                S_Record.Work_Person = Head.Work_Out_Person;
                Stock_Record_List.Add(S_Record);
            }

            db.WMS_Stock_Temp.RemoveRange(Stock_Temp_List);
            db.WMS_Stock_Record.AddRange(Stock_Record_List);
        }

        //重置扫描
        public void Reset_Out_Task_Bat_Tray_No(Guid Head_ID, string Tray_No)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Out_Head() : Head;
            if (Head.Status == WMS_Out_Global_State_Enum.待出库.ToString() || Head.Status == WMS_Out_Global_State_Enum.已出库.ToString() || Head.Status == WMS_Out_Global_State_Enum.待包装.ToString())
            {
                throw new Exception("已完成验货，不支持重置！");
            }
            Tray_No = Tray_No.Trim();
            List<WMS_Out_Scan> List_Scan = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No).ToList();
            if (List_Scan.Any())
            {
                db.WMS_Out_Scan.RemoveRange(List_Scan);
                MyDbSave.SaveChange(db);
            }
        }

        //重置扫描
        public void Reset_Out_Task_Bat_Tray_No_By_Box(Guid Head_ID, string Tray_No, string Case_No)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Out_Head() : Head;
            if (Head.Status == WMS_Out_Global_State_Enum.待出库.ToString() || Head.Status == WMS_Out_Global_State_Enum.已出库.ToString() || Head.Status == WMS_Out_Global_State_Enum.待包装.ToString())
            {
                throw new Exception("已完成验货，不支持重置！");
            }
            Tray_No = Tray_No.Trim();
            Case_No = Case_No.Trim();

            List<WMS_Out_Scan> List_Scan = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No && x.Case_No == Case_No).ToList();
            if (List_Scan.Any())
            {
                db.WMS_Out_Scan.RemoveRange(List_Scan);
                MyDbSave.SaveChange(db);
            }
        }

        public string Get_Out_Task_List_To_Excel(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            List<WMS_Out_Scan> Scan_List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            var Group = from x in Scan_List
                        group x by new { x.MatSn, x.Tray_No } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Box_Count = G.Count(),
                            Tray_No = G.Key.Tray_No,
                            Scan_Quantity_Sum = G.Sum(c => c.Scan_Quantity)
                        };

            //List<WMS_Track> Track_List = new List<WMS_Track>();
            //if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.快递.ToString() || Head.Logistics_Mode == Logistics_Out_Mode_Enum.物流.ToString())
            //{
            //    Track_List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            //}

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("序号");
            TableHeads.Add("产品型号");
            TableHeads.Add("数量");

            if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
            {
                TableHeads.Add("托号");
                TableHeads.Add("箱数");
            }
            else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
            {
                TableHeads.Add("箱号");
            }

            //TableHeads.Add("快递单号");
            //TableHeads.Add("重量");

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
                newRow["数量"] = x.Scan_Quantity_Sum;
                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    newRow["托号"] = x.Tray_No;
                    newRow["箱数"] = x.Box_Count;
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    newRow["箱号"] = x.Tray_No;
                }

                //if (Track_List.Where(c => c.Tray_No == x.Tray_No).Any())
                //{
                //    newRow["快递单号"] = Track_List.Where(c => c.Tray_No == x.Tray_No).FirstOrDefault().Tracking_No;
                //    newRow["重量"] = Track_List.Where(c => c.Tray_No == x.Tray_No).FirstOrDefault().Weight;
                //}
                //else
                //{
                //    newRow["快递单号"] = "";
                //    newRow["重量"] = "";
                //}

                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public string Get_Out_Task_List_To_Excel_By_Tray(Guid Head_ID, string Tray_No)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            List<WMS_Out_Scan> Scan_List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No).ToList();
            var Group = from x in Scan_List
                        group x by new { x.MatSn } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Box_Count = G.Count(),
                            Scan_Quantity_Sum = G.Sum(c => c.Scan_Quantity)
                        };

            List<WMS_Track> Track_List = new List<WMS_Track>();
            if (Head.Logistics_Mode != Logistics_Out_Mode_Enum.自送.ToString())
            {
                Track_List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            }

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("序号");
            TableHeads.Add("产品型号");

            if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.自送.ToString())
            {
                TableHeads.Add("托号");
                TableHeads.Add("箱数");
            }
            else
            {
                TableHeads.Add("箱号");
                TableHeads.Add("快递单号");
            }

            TableHeads.Add("数量");

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

                if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.自送.ToString())
                {
                    newRow["托号"] = Tray_No;
                    newRow["箱数"] = x.Box_Count;
                }
                else
                {
                    newRow["箱号"] = Tray_No;
                    if (Track_List.Where(c => c.Tray_No == Tray_No).Any())
                    {
                        string Tracking_No = string.Empty;
                        foreach (var xx in Track_List.Where(c => c.Tray_No == Tray_No).ToList())
                        {
                            Tracking_No += xx.Tracking_No + ",";
                        }

                        newRow["快递单号"] = CommonLib.Trim_End_Char(Tracking_No);
                    }
                    else
                    {
                        newRow["快递单号"] = "";
                    }
                }

                newRow["数量"] = x.Scan_Quantity_Sum;
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public string Get_Out_Task_List_To_Excel_With_Tracking_No(WMS_Out_Head Head, List<WMS_Out_Scan> Scan_List, List<WMS_Track> Track_List,List<WMS_Track_Info> Track_Info_List)
        {
            WMS_Track Track = new WMS_Track();
            WMS_Track_Info Info = new WMS_Track_Info();

            var Group = from x in Scan_List
                        group x by new { x.MatSn, x.Tray_No } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Box_Count = G.Count(),
                            Tray_No = G.Key.Tray_No,
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
            TableHeads.Add("数量");

            if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
            {
                TableHeads.Add("托号");
                TableHeads.Add("箱数");
            }
            else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
            {
                TableHeads.Add("箱号");
            }

            TableHeads.Add("快递单号");
            TableHeads.Add("重量");
            TableHeads.Add("快递公司");
            TableHeads.Add("寄件人");
            TableHeads.Add("收件人");
            TableHeads.Add("收件人手机");
            TableHeads.Add("收件人座机");
            TableHeads.Add("收件人地址");

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
                newRow["数量"] = x.Scan_Quantity_Sum;
                if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    newRow["托号"] = x.Tray_No;
                    newRow["箱数"] = x.Box_Count;
                }
                else if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    newRow["箱号"] = x.Tray_No;
                }

                Track = Track_List.Where(c => c.Tray_No == x.Tray_No).FirstOrDefault();
                Track = Track == null ? new WMS_Track() : Track;

                newRow["快递单号"] = Track.Tracking_No;
                newRow["重量"] = Track.Weight;

                Info = Track_Info_List.Where(c => c.Tracking_No == Track.Tracking_No).FirstOrDefault();
                Info = Info == null ? new WMS_Track_Info() : Info;

                newRow["快递公司"] = Info.Logistics_Company;
                newRow["寄件人"] = Info.Sender_Name;
                newRow["收件人"] = Info.Receiver_Name;
                newRow["收件人手机"] = Info.Receiver_Phone;
                newRow["收件人座机"] = Info.Receiver_Tel;
                newRow["收件人地址"] = Info.Receiver_Address;

                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }
    }

    //退货作业
    public partial class WmsService : IWmsService
    {
        public WMS_Out_Task Get_WMS_Out_Task_Item_DB(Guid HeadID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(HeadID);
            Head = Head == null ? new WMS_Out_Head() : Head;
            List<WMS_Out_Line> List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            WMS_Out_Task T = new WMS_Out_Task();
            T.Head_ID = Head.Head_ID;
            T.Task_Bat_No_Str = Head.Task_Bat_No_Str;
            T.Create_DT = Head.Create_DT;
            T.Create_Person = Head.Create_Person;
            T.Logistics_Company = Head.Logistics_Company;
            T.Logistics_Mode = Head.Logistics_Mode;
            T.Logistics_Cost_Type = Head.Logistics_Cost_Type;
            T.Global_State = Head.Status;
            T.Scan_Mat_Type = Head.Scan_Mat_Type;
            T.Work_Out_Person = Head.Work_Out_Person;
            T.Work_Down_Person = Head.Work_Down_Person;
            T.Customer_Name = Head.Customer_Name;
            T.Customer_Tel = Head.Customer_Tel;
            T.Customer_Address = Head.Customer_Address;
            T.Link_Cus_ID = Head.Link_Cus_ID;
            T.Line_List = new List<WMS_Out_Task_Line>();
            var Line_Group = from x in List
                             group x by x.MatSn into G
                             select new
                             {
                                 MatSn = G.Key,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                                 Line_No = G.FirstOrDefault().Line_No,
                                 Price = G.FirstOrDefault().Price,
                             };

            int i = 0;
            WMS_Out_Task_Line T_Line = new WMS_Out_Task_Line();
            foreach (var x in Line_Group.OrderBy(x => x.Line_No).ToList())
            {
                i++;
                T_Line = new WMS_Out_Task_Line();
                T_Line.Line_No = i;
                T_Line.MatSn = x.MatSn;
                T_Line.Quantity_Sum = x.Quantity_Sum;
                T_Line.Unit_Price = x.Price;
                T.Line_List.Add(T_Line);
            }

            T.Line_List = T.Line_List.OrderBy(x => x.Line_No).ToList();
            return T;
        }

        public void Create_WMS_Task_Out_Return(WMS_Out_Head Head, List<WMS_Out_Line> Line_List, User U)
        {
            if (Line_List.Any() == false) { throw new Exception("未选择退货产品"); }

            if (string.IsNullOrEmpty(Head.Logistics_Mode)) { throw new Exception("未选择运输方式"); }

            Supplier Sup = db.Supplier.Find(Head.Link_Cus_ID);
            if (Sup == null) { throw new Exception("系统中不存在此供应商"); }

            WMS_In_Head In_Head = db.WMS_In_Head.Find(Head.Head_ID);
            if (In_Head == null) { throw new Exception("系统中不存在此收货单"); }

            Head.Out_DT_Str = Head.Out_DT.ToString("yyyy-MM-dd");

            WMS_Out_Head Head_New = new WMS_Out_Head();
            Head_New.Head_ID = MyGUID.NewGUID();
            Head_New.Task_Bat_No = this.Auto_Create_Task_Bat_No_Out(U);
            Head_New.Task_Bat_No_Str = this.Auto_Create_Task_Bat_No_Str_Out_Return(Head, Sup);
            Head_New.Create_DT = DateTime.Now;
            Head_New.Create_Person = U.UserFullName;
            Head_New.Status = WMS_Out_Global_State_Enum.待配货.ToString();
            Head_New.LinkMainCID = U.LinkMainCID;
            Head_New.Logistics_Company = Head.Logistics_Company;
            Head_New.Logistics_Mode = Head.Logistics_Mode;
            Head_New.Logistics_Cost_Type = Head.Logistics_Cost_Type;
            Head_New.Customer_Name = Sup.Sup_Name;
            Head_New.Out_DT_Str = Head.Out_DT_Str;
            Head_New.Link_Cus_ID = Sup.SupID;
            Head_New.Brand = In_Head.Brand;
            Head_New.Head_Type = WMS_Out_Head_Type_Enum.订单退货.ToString();
            Head_New.Return_Remark = Head.Return_Remark.Trim();
            Head_New.Link_WMS_Out_ID = In_Head.Head_ID;
            Head_New.Link_WMS_Out_No = In_Head.Task_Bat_No_Str;
            db.WMS_Out_Head.Add(Head_New);

            List<WMS_In_Line> Line_List_DB = db.WMS_In_Line.Where(x => x.Link_Head_ID == In_Head.Head_ID).ToList();
            WMS_In_Line Line_DB = new WMS_In_Line();
            List<WMS_Out_Line> Line_List_New = new List<WMS_Out_Line>();
            WMS_Out_Line Line = new WMS_Out_Line();
            int i = 0;
            foreach (var x in Line_List)
            {
                i++;
                Line_DB = Line_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                if (Line_DB == null) { throw new Exception("送货单中不存在此产品型号"); }

                Line = new WMS_Out_Line();
                Line.Line_ID = MyGUID.NewGUID();
                Line.MatSn = x.MatSn;
                Line.Quantity = x.Quantity;
                Line.LinkMainCID = Head_New.LinkMainCID;
                Line.Link_Head_ID = Head_New.Head_ID;
                Line.Task_Bat_No = Head_New.Task_Bat_No;
                Line.Task_Bat_No_Str = Head_New.Task_Bat_No_Str;
                Line.Create_DT = Head_New.Create_DT;
                Line.Create_Person = Head_New.Create_Person;
                Line.Line_No = i;
                Line.MatUnit = "PCS";
                Line.Price = Line_DB.Price_Cost;
                Line.Customer_Name = Head_New.Customer_Name;
                Line.Logistics_Company = Head_New.Logistics_Company;
                Line.Logistics_Mode = Head_New.Logistics_Mode;
                Line.Logistics_Cost_Type = Head_New.Logistics_Cost_Type;
                Line_List_New.Add(Line);
            }

            db.WMS_Out_Line.AddRange(Line_List_New);
            MyDbSave.SaveChange(db);
        }

        private string Auto_Create_Task_Bat_No_Str_Out_Return(WMS_Out_Head Head, Supplier Sup)
        {
            List<string> Task_Bat_No_Str_List = db.WMS_Out_Head.Where(x => x.LinkMainCID == Sup.LinkMainCID && x.Out_DT_Str == Head.Out_DT_Str).Select(x => x.Task_Bat_No_Str).Distinct().ToList();

            string Logistics_Mode_Code = string.Empty;

            if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.快递.ToString())
            {
                Logistics_Mode_Code = "KD";
            }
            else if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.物流.ToString())
            {
                Logistics_Mode_Code = "WL";
            }
            else if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.自送.ToString())
            {
                Logistics_Mode_Code = "ZS";
            }
            else if (Head.Logistics_Mode == Logistics_Out_Mode_Enum.自提.ToString())
            {
                Logistics_Mode_Code = "ZT";
            }

            Head.Task_Bat_No_Str = Sup.SupplierCode + "_" + Head.Out_DT.ToString("yyyyMMdd") + "_" + Logistics_Mode_Code + "_";

            Task_Bat_No_Str_List = Task_Bat_No_Str_List.Where(x => x.Contains(Head.Task_Bat_No_Str)).ToList();
            if (Task_Bat_No_Str_List.Any() == false)
            {
                Head.Task_Bat_No_Str = Head.Task_Bat_No_Str + "1";
            }
            else
            {
                List<long> Task_Bat_No_Num_List = new List<long>();

                foreach (var No in Task_Bat_No_Str_List)
                {
                    string[] Source = No.Trim().Split('_');
                    try
                    {
                        Task_Bat_No_Num_List.Add(Convert.ToInt64((Source[3].Trim())));
                    }
                    catch
                    {
                        Task_Bat_No_Num_List.Add(1);
                    }

                }

                long Task_Bat_No_Num = Task_Bat_No_Num_List.Max() + 1;
                Head.Task_Bat_No_Str = Head.Task_Bat_No_Str + Task_Bat_No_Num.ToString();
            }

            return Head.Task_Bat_No_Str;
        }

        public WMS_Out_Task Get_WMS_Out_Task_Item_DB(WMS_Out_Filter MF)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(MF.LinkHeadID);
            Head = Head == null ? new WMS_Out_Head() : Head;
            List<WMS_Out_Line> List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            WMS_Out_Task T = new WMS_Out_Task();
            T.Head_ID = Head.Head_ID;
            T.Task_Bat_No_Str = Head.Task_Bat_No_Str;
            T.Create_DT = Head.Create_DT;
            T.Create_Person = Head.Create_Person;
            T.Logistics_Company = Head.Logistics_Company;
            T.Logistics_Mode = Head.Logistics_Mode;
            T.Brand = Head.Brand;
            T.Logistics_Cost_Type = Head.Logistics_Cost_Type;
            T.Global_State = Head.Status;
            T.Scan_Mat_Type = Head.Scan_Mat_Type;
            T.Work_Out_Person = Head.Work_Out_Person;
            T.Work_Down_Person = Head.Work_Down_Person;
            T.Customer_Name = Head.Customer_Name;
            T.Customer_Tel = Head.Customer_Tel;
            T.Customer_Address = Head.Customer_Address;
            T.Total_Cases = Head.Total_Cases;
            T.Line_List = new List<WMS_Out_Task_Line>();
            var Line_Group = from x in List
                             group x by x.MatSn into G
                             select new
                             {
                                 MatSn = G.Key,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                                 Line_No = G.FirstOrDefault().Line_No,
                             };

            int i = 0;
            WMS_Out_Task_Line T_Line = new WMS_Out_Task_Line();
            foreach (var x in Line_Group.OrderBy(x => x.Line_No).ToList())
            {
                i++;
                T_Line = new WMS_Out_Task_Line();
                T_Line.Line_No = i;
                T_Line.MatSn = x.MatSn;
                T_Line.Quantity_Sum = x.Quantity_Sum;


                T.Line_List.Add(T_Line);
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                T.Line_List = T.Line_List.Where(x => x.MatSn.ToLower().Contains(MF.MatSn.ToLower())).ToList();
            }

            T.Line_List = T.Line_List.OrderBy(x => x.Line_No).ToList();

            return T;
        }

        public List<WMS_In_Line> Get_WMS_In_Line_List(WMS_Out_Head Head)
        {
            List<WMS_Out_Line> List_DB = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            List<WMS_In_Line> In_List_DB = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head.Link_WMS_Out_ID).ToList();

            List<string> MatSn_List = In_List_DB.Select(x => x.MatSn).Distinct().ToList();
            List<WMS_In_Line> List = new List<WMS_In_Line>();
            WMS_In_Line Line = new WMS_In_Line();

            foreach (var MatSn in MatSn_List)
            {
                Line = new WMS_In_Line();
                Line.MatSn = MatSn;

                if (List_DB.Where(c => c.MatSn == MatSn).Any())
                {
                    Line.Quantity = List_DB.Where(c => c.MatSn == MatSn).Sum(x => x.Quantity);
                }

                Line.Max_Quantity = In_List_DB.Where(c => c.MatSn == MatSn).Sum(x => x.Quantity);
                Line.Price_Cost = In_List_DB.Where(c => c.MatSn == MatSn).FirstOrDefault().Price_Cost;
                List.Add(Line);
            }

            return List;
        }

        public void Update_WMS_Task_Out_Return(WMS_Out_Head Head, List<WMS_Out_Line> Line_List)
        {
            if (Line_List.Any() == false) { throw new Exception("未选择退货产品"); }

            if (string.IsNullOrEmpty(Head.Logistics_Mode)) { throw new Exception("未选择运输方式"); }
            
            WMS_Out_Head Head_New = db.WMS_Out_Head.Find(Head.Head_ID);
            Head_New.Logistics_Company = Head.Logistics_Company;
            Head_New.Logistics_Mode = Head.Logistics_Mode;
            Head_New.Logistics_Cost_Type = Head.Logistics_Cost_Type;
            Head_New.Out_DT_Str = Head.Out_DT.ToString("yyyy-MM-dd");
            Head_New.Return_Remark = Head.Return_Remark.Trim();
            db.Entry(Head_New).State = EntityState.Modified;

            List<WMS_Out_Line> Out_Line_List_DB = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head_New.Head_ID).ToList();
            db.WMS_Out_Line.RemoveRange(Out_Line_List_DB);

            List<WMS_In_Line> Line_List_DB = db.WMS_In_Line.Where(x => x.Link_Head_ID == Head_New.Link_WMS_Out_ID).ToList();
            WMS_In_Line Line_DB = new WMS_In_Line();
            List<WMS_Out_Line> Line_List_New = new List<WMS_Out_Line>();
            WMS_Out_Line Line = new WMS_Out_Line();
            int i = 0;
            foreach (var x in Line_List)
            {
                i++;
                Line_DB = Line_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                if (Line_DB == null) { throw new Exception("送货单中不存在此产品型号"); }

                Line = new WMS_Out_Line();
                Line.Line_ID = MyGUID.NewGUID();
                Line.MatSn = x.MatSn;
                Line.Quantity = x.Quantity;
                Line.LinkMainCID = Head_New.LinkMainCID;
                Line.Link_Head_ID = Head_New.Head_ID;
                Line.Task_Bat_No = Head_New.Task_Bat_No;
                Line.Task_Bat_No_Str = Head_New.Task_Bat_No_Str;
                Line.Create_DT = Head_New.Create_DT;
                Line.Create_Person = Head_New.Create_Person;
                Line.Line_No = i;
                Line.MatUnit = "PCS";
                Line.Price = Line_DB.Price_Cost;
                Line.Customer_Name = Head_New.Customer_Name;
                Line.Logistics_Company = Head_New.Logistics_Company;
                Line.Logistics_Mode = Head_New.Logistics_Mode;
                Line.Logistics_Cost_Type = Head_New.Logistics_Cost_Type;
                Line_List_New.Add(Line);
            }

            db.WMS_Out_Line.AddRange(Line_List_New);
            MyDbSave.SaveChange(db);
        }
    }

    //移库推荐
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Stock_Task> Get_WMS_Stock_Task_PageList_For_Move_Recommend(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock_Task.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Recommend_Status == WMS_Recommend_Status_Enum.未推荐.ToString()).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Work_Person))
            {
                query = query.Where(x => x.Work_Person.Contains(MF.Work_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
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

            List<WMS_Stock_Task> List_DB = query.ToList();
            List<WMS_Stock_Task> List = new List<WMS_Stock_Task>();

            List<Guid> ID_List_DB = List_DB.Select(x => x.Task_ID).ToList();
            List<Guid> ID_List = new List<Guid>();

            List<WMS_Profit_Loss_Other> Profit_Loss_List = db.WMS_Profit_Loss_Other.Where(x => x.Status == WMS_Profit_Loss_Status_Enum.未确定.ToString()).ToList();

            foreach (var ID in ID_List_DB)
            {
                if (Profit_Loss_List.Where(c => c.Link_TaskID == ID).Any() == false)
                {
                    ID_List.Add(ID);
                }
            }

            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => ID_List.Contains(x.Link_TaskID)).ToList();

            foreach (var x in List_DB)
            {
                if (Scan_List.Where(c => c.Link_TaskID == x.Task_ID && c.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).Any())
                {
                    List.Add(x);
                }
            }

            PageList<WMS_Stock_Task> PList = new PageList<WMS_Stock_Task>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = List.Count();
            PList.Rows = List.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public List<WMS_Stocktaking_Scan> Get_WMS_Stocktaking_Scan_List_For_Move(Guid TaskID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("动盘任务不存在"); }

            List<WMS_Stocktaking_Scan> Scan_List_DB = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID && x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).OrderByDescending(x => x.Create_DT).ToList();
            List<string> MatSn_List = Scan_List_DB.Select(x => x.MatSn).Distinct().ToList();

            List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == Task.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
            Material Mat = new Material();

            //推荐信息
            List<WMS_Location> Location_List_DB = db.WMS_Location.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Type == Type_Enum.端数.ToString()).ToList();
            List<WMS_Location> Location_List_DB_A = Location_List_DB.Where(X => X.Link_MatSn_Count == Link_MatSn_Count_Enum.八个.ToString()).ToList();
            List<WMS_Location> Location_List_DB_B = Location_List_DB.Where(X => X.Link_MatSn_Count == Link_MatSn_Count_Enum.不限.ToString()).ToList();
            List<string> Loc_Str_List_DB_A = Location_List_DB_A.Select(x => x.Location).ToList();
            List<string> Loc_Str_List_DB_B = Location_List_DB_B.Select(x => x.Location).ToList();

            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Task.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
            List<WMS_Stock> Stock_List_A = new List<WMS_Stock>();
            List<string> Loc_Str_List_A = new List<string>();
            List<WMS_Stock> Stock_List_B = new List<WMS_Stock>();
            List<string> Loc_Str_List_B = new List<string>();

            List<string> Loc_Str_List = new List<string>();

            List<WMS_Stock_Record> Record_List_DB = db.WMS_Stock_Record.Where(x => x.LinkMainCID == Task.LinkMainCID && Loc_Str_List_DB_A.Contains(x.Location)).ToList();
            Record_List_DB = Record_List_DB.Where(x => x.Remark == WMS_Stock_Record_Remark_Enum.订单出库.ToString()).ToList();
            WMS_Stock_Record Record = new WMS_Stock_Record();
            List<WMS_Stock_Record> Record_List = new List<WMS_Stock_Record>();

            Recommend_Move_Info Info = new Recommend_Move_Info();

            int i = 0;
            foreach (var x in Scan_List_DB)
            {
                Stock_List_A = Stock_List_DB.Where(c => Loc_Str_List_DB_A.Contains(c.Location) && c.MatSn == x.MatSn).ToList();
                Loc_Str_List_A = Stock_List_A.Select(c => c.Location).Distinct().ToList();
                foreach (var Loc in Loc_Str_List_A)
                {
                    i++;
                    Info = new Recommend_Move_Info();
                    Info.No = i;
                    Info.Location = Loc;
                    Info.Quantity = Stock_List_A.Where(c => c.Location == Loc).Sum(c => c.Quantity);
                    x.Recommend_Info_List.Add(Info);
                }

                if (x.Recommend_Info_List.Count() < 5)
                {
                    Record_List = Record_List_DB.Where(c => c.MatSn == x.MatSn && Loc_Str_List_DB_A.Contains(c.Location)).OrderByDescending(c => c.Create_DT).ToList();
                    Loc_Str_List = Record_List.Select(c => c.Location).Distinct().ToList();

                    foreach (var Loc in Loc_Str_List)
                    {
                        i++;
                        Info = new Recommend_Move_Info();
                        Info.No = i;
                        Info.Location = Loc;
                        Info.Quantity = 0;
                        if (x.Recommend_Info_List.Where(c => c.Location == Loc).Any() == false)
                        {
                            x.Recommend_Info_List.Add(Info);
                        }
                    }

                    if (x.Recommend_Info_List.Count() < 5)
                    {
                        Stock_List_B = Stock_List_DB.Where(c => Loc_Str_List_DB_B.Contains(c.Location) && c.MatSn == x.MatSn).ToList();

                        Loc_Str_List_B = Stock_List_B.Select(c => c.Location).Distinct().ToList();
                        foreach (var Loc in Loc_Str_List_B)
                        {
                            i++;
                            Info = new Recommend_Move_Info();
                            Info.No = i;
                            Info.Location = Loc;
                            Info.Quantity = Stock_List_B.Where(c => c.Location == Loc).Sum(c => c.Quantity);
                            if (x.Recommend_Info_List.Where(c => c.Location == Loc).Any() == false)
                            {
                                x.Recommend_Info_List.Add(Info);
                            }
                        }

                        if (x.Recommend_Info_List.Count() < 5)
                        {
                            Record_List = Record_List_DB.Where(c => c.MatSn == x.MatSn && Loc_Str_List_DB_B.Contains(c.Location)).OrderByDescending(c => c.Create_DT).ToList();
                            Loc_Str_List = Record_List.Select(c => c.Location).Distinct().ToList();

                            foreach (var Loc in Loc_Str_List)
                            {
                                i++;
                                Info = new Recommend_Move_Info();
                                Info.No = i;
                                Info.Location = Loc;
                                Info.Quantity = 0;
                                if (x.Recommend_Info_List.Where(c => c.Location == Loc).Any() == false)
                                {
                                    x.Recommend_Info_List.Add(Info);
                                }
                            }
                        }
                    }
                }

                x.Recommend_Info_List = x.Recommend_Info_List.Take(5).ToList();

                Mat = Mat_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                if (Mat != null)
                {
                    x.Pack_Qty = Mat.Pack_Qty;
                }
                else
                {
                    x.Pack_Qty = 0;
                }

            }

            return Scan_List_DB;
        }

        public void Finish_WMS_Stocktaking_Scan_Recommend(Guid TaskID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }

            if (db.WMS_Move.Where(x => x.Link_HeadID == Task.Task_ID && x.Move_Status == WMS_Move_Status_Enum.待移库.ToString()).Any())
            {
                throw new Exception("存在移库任务未完成");
            }

            if (Task.Recommend_Status == WMS_Recommend_Status_Enum.未推荐.ToString())
            {
                Task.Recommend_Status = WMS_Recommend_Status_Enum.已推荐.ToString();
            }
         
            db.Entry(Task).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Create_WMS_Move_From_WMS_Stocktaking_Scan_Recommend(Guid TaskID, WMS_Move_Scan Move_Scan)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }

            WMS_Move Move = new WMS_Move();
            Move.Move_ID = MyGUID.NewGUID();
            Move.LinkMainCID = Task.LinkMainCID;
            Move.Create_DT = DateTime.Now;
            Move.Out_Location = Task.Location;
            Move.In_Location = Move_Scan.In_Location;
            Move.Move_Status = WMS_Move_Status_Enum.待移库.ToString();
            Move.Work_Person = Task.Work_Person;
            Move.Link_HeadID = Task.Task_ID;
            db.WMS_Move.Add(Move);

            if (db.WMS_Location.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Move.Out_Location).Any() == false) { throw new Exception("系统中不存在该库位"); }

            if (db.WMS_Stock.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Move.Out_Location).Any() == false) { throw new Exception("该库位无产品"); }
            
            WMS_Stock Stock_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).FirstOrDefault();
            
            Move_Scan.Scan_ID = MyGUID.NewGUID();
            Move_Scan.Create_DT = DateTime.Now;
            Move_Scan.Link_TaskID = Move.Move_ID;
            Move_Scan.LinkMainCID = Move.LinkMainCID;
            Move_Scan.MatBrand = Stock_DB.MatBrand;
            Move_Scan.Out_Location = Move.Out_Location;
            Move_Scan.In_Location = Move.In_Location;
            Move_Scan.Scan_Source = Move_Scan.MatSn;
            Move_Scan.Package_Type = WMS_Stock_Package_Enum.零头.ToString();
            Move_Scan.Status = WMS_Move_Status_Enum.待移库.ToString();
            db.WMS_Move_Scan.Add(Move_Scan);

            //移库任务判断
            List<WMS_Move> Move_List_DB = db.WMS_Move.Where(x => x.Link_HeadID == Task.Task_ID).ToList();
            if (Move_List_DB.Any())
            {
                List<Guid> Move_ID_List = Move_List_DB.Select(x => x.Move_ID).ToList();
                if (db.WMS_Move_Scan.Where(x => Move_ID_List.Contains(x.Link_TaskID) && x.MatSn == Move_Scan.MatSn).Any())
                {
                    throw new Exception("该库位中的产品型号已创建待移库任务");
                }
            }

            MyDbSave.SaveChange(db);
        }

        public List<WMS_Move> Get_WMS_Move_List_By_Link_HeadID(Guid Link_HeadID)
        {
            return db.WMS_Move.Where(x => x.Link_HeadID == Link_HeadID).ToList();
        }

        public List<WMS_Move_Scan> Get_WMS_Move_Scan_List(Guid Move_ID)
        {
            return db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move_ID).ToList();
        }

        public void Delete_WMS_Move_From_WMS_Stocktaking_Scan_Recommend(Guid Move_ID)
        {
            WMS_Move Move = db.WMS_Move.Find(Move_ID);
            if (Move == null) { throw new Exception("WMS_Move is null"); }

            if (Move.Move_Status == WMS_Move_Status_Enum.已移库.ToString())
            {
                throw new Exception("该移库任务已完成，不支持删除");
            }

            List<WMS_Move_Scan> Scan_List = db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID).ToList();
            db.WMS_Move_Scan.RemoveRange(Scan_List);
            db.WMS_Move.Remove(Move);

            MyDbSave.SaveChange(db);
        }

        public void Finish_WMS_Move_Task_From_Recommend(Guid Move_ID)
        {
            WMS_Move Move = db.WMS_Move.Find(Move_ID);
            if (Move == null) { throw new Exception("WMS_Move is null"); }

            if (Move.Move_Status == WMS_Move_Status_Enum.已移库.ToString())
            {
                throw new Exception("该移库任务已完成，不支持操作");
            }

            List<WMS_Move_Scan> Scan_List = db.WMS_Move_Scan.Where(x => x.Link_TaskID == Move.Move_ID).ToList();
            foreach (var x in Scan_List)
            {
                x.Status = WMS_Move_Status_Enum.已移库.ToString();
                db.Entry(x).State = EntityState.Modified;
            }

            Move.Move_Status = WMS_Move_Status_Enum.已移库.ToString();
            db.Entry(Move).State = EntityState.Modified;

            List<string> MatSn_List = Scan_List.Select(x => x.MatSn).Distinct().ToList();
            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).ToList();
            Stock_List_DB = Stock_List_DB.Where(x => MatSn_List.Contains(x.MatSn)).ToList();

            WMS_Stock Stock_DB = new WMS_Stock();
            WMS_Stock Stock = new WMS_Stock();
            List<WMS_Stock> Stock_List = new List<WMS_Stock>();
            List<WMS_Stock> Stock_List_Remove = new List<WMS_Stock>();

            List<WMS_Move_Record> Record_List = new List<WMS_Move_Record>();
            WMS_Move_Record Record = new WMS_Move_Record();
            DateTime DT = DateTime.Now;

            //移库记录
            foreach (var x in Scan_List)
            { 
                Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                if (Stock_DB == null) { throw new Exception("Stock_DB is null"); }

                Record = new WMS_Move_Record();
                Record.Record_ID = MyGUID.NewGUID();
                Record.Out_Location = x.Out_Location;
                Record.In_Location = x.In_Location;
                Record.Create_DT = DT;
                Record.LinkMainCID = x.LinkMainCID;
                Record.Work_Person = Move.Work_Person;
                Record.Link_TaskID = Move.Move_ID;
                Record.MatSn = x.MatSn;
                Record.Quantity = x.Scan_Quantity;
                Record.Package_Type = x.Package_Type;
                Record.MatName = Stock_DB.MatName;
                Record.MatBrand = Stock_DB.MatBrand;
                Record.MatUnit = Stock_DB.MatUnit;
                Record.Move_Type = WMS_Move_Type_Enum.移库作业.ToString();
                Record_List.Add(Record);
            }
            db.WMS_Move_Record.AddRange(Record_List);

            //库存产品变换库位
            int Quantity = 0;
            foreach (var x in Scan_List)
            {
                Quantity = x.Scan_Quantity;

                //消库
                foreach (var xx in Stock_List_DB.Where(c => c.MatSn == x.MatSn).OrderBy(c => c.Quantity).ToList())
                {
                    if (Quantity >= xx.Quantity && Quantity > 0)
                    {
                        Quantity = Quantity - xx.Quantity;
                        xx.Location = Move.In_Location;
                        xx.Package = WMS_Stock_Package_Enum.零头.ToString();
                        xx.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                        db.Entry(xx).State = EntityState.Modified;
                    }
                    else if (Quantity < xx.Quantity && Quantity > 0)
                    {
                        //更新原有库存
                        xx.Quantity = xx.Quantity - Quantity;
                        db.Entry(xx).State = EntityState.Modified;

                        Stock = new WMS_Stock();
                        Stock.Stock_ID = MyGUID.NewGUID();
                        Stock.WMS_In_DT = xx.WMS_In_DT;
                        Stock.WMS_Out_DT = xx.WMS_Out_DT;
                        Stock.MatSn = xx.MatSn;
                        Stock.MatName = xx.MatName;
                        Stock.MatBrand = xx.MatBrand;
                        Stock.MatUnit = xx.MatUnit;
                        Stock.Quantity = Quantity;
                        Stock.Package = WMS_Stock_Package_Enum.零头.ToString();
                        Stock.Price = xx.Price;
                        Stock.Location = Move.In_Location;
                        Stock.Cases = xx.Cases;
                        Stock.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                        Stock.Wms_In_Head_ID = xx.Wms_In_Head_ID;
                        Stock.LinkMainCID = xx.LinkMainCID;
                        Stock_List.Add(Stock);
                        break;
                    }
                }
                
            }

            if (Stock_List.Any())
            {
                db.WMS_Stock.AddRange(Stock_List);
            }

            MyDbSave.SaveChange(db);
        }
        
    }
}

//扫描枪交互
namespace SMART.Api
{
    public partial interface IWmsService
    {
        //配货动盘
        DataTable WMS_Out_Pick_Stocktaking_List(Guid MainCID);
        DataTable WMS_Out_Pick_Stocktaking_List_Work_Person(Guid MainCID, string Work_Person);
        DataTable WMS_Out_Pick_Stocktaking_List_Sub(string Task_ID);
        void WMS_Out_Pick_Stocktaking_List_Sub_Scan(string Task_ID, string Scan_Source);
        DataTable WMS_Out_Pick_Stocktaking_List_Sub_Other(string Task_ID);
        void WMS_Out_Pick_Stocktaking_List_Sub_Other_Scan(string Task_ID, string Scan_Source, string Quantity);

        //验货作业
        DataTable WMS_Out_Task_List_A(Guid MainCID);
        DataTable WMS_Out_Task_List_B(Guid MainCID);
        DataTable WMS_Out_Task_List_C(Guid MainCID);
        DataTable WMS_Out_Task_By_Tray_No(string HeadID, string Tray_No);
        void WMS_Out_Task_Scan_Item(string HeadID, string Tray_No, string Scan_Source);
        DataTable WMS_Out_Task_By_Tray_No_Other(string HeadID, string Tray_No, string Case_No);
        void WMS_Out_Task_Scan_Item_Other(string HeadID, string Tray_No, string Scan_Source, string Quantity, string Case_No);

        DataTable WMS_Out_Track_List(Guid MainCID);
        void WMS_Out_Track_Scan_Item(Guid MainCID, string Tray_No, string Scan_Source, string Weight);
        DataTable WMS_Out_Track_List_Sub(Guid MainCID, string Scan_Source);
        void WMS_Out_Track_Scan_Delete(Guid MainCID, string Scan_Source);
    }

    //配货动盘
    public partial class WmsService : IWmsService
    {
        public DataTable WMS_Out_Pick_Stocktaking_List(Guid MainCID)
        {
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.PageIndex = 1;
            MF.PageSize = 10000;
            MF.LinkMainCID = MainCID;
            MF.Property = WMS_Stock_Task_Property_Enum.配货动盘.ToString();
            MF.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            List<string> Work_Person_List = Get_WMS_Stock_Task_PageList_Pick(MF).Rows.Select(x => x.Work_Person).Distinct().ToList();

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序");
            dt.Columns.Add("作业人");
            int i = 0;
            foreach (var Work_Person in Work_Person_List)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序"] = i;
                dr["作业人"] = Work_Person;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable WMS_Out_Pick_Stocktaking_List_Work_Person(Guid MainCID, string Work_Person)
        {
            if (string.IsNullOrEmpty(Work_Person)) { throw new Exception("作业人为空"); }

            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.PageIndex = 1;
            MF.PageSize = 10000;
            MF.LinkMainCID = MainCID;
            MF.Property = WMS_Stock_Task_Property_Enum.配货动盘.ToString();
            MF.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            MF.Work_Person = Work_Person;
            List<WMS_Stock_Task> Task_List = Get_WMS_Stock_Task_PageList_Pick(MF).Rows;

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("序");
            dt.Columns.Add("库位");
            dt.Columns.Add("作业人");
            int i = 0;
            foreach (var x in Task_List.OrderBy(x => x.Location))
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序"] = i;
                dr["GUID"] = x.Task_ID;
                dr["库位"] = x.Location;
                dr["作业人"] = x.Work_Person;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //动盘扫描列表
        public DataTable WMS_Out_Pick_Stocktaking_List_Sub(string Task_ID)
        {
            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("动盘任务不存在"); }
            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == TaskID && x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).ToList();

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序");
            dt.Columns.Add("型号");
            dt.Columns.Add("数量");
            int i = 0;
            foreach (var x in Scan_List)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序"] = i;
                dr["型号"] = x.MatSn;
                dr["数量"] = x.Scan_Quantity;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //动盘扫描
        public void WMS_Out_Pick_Stocktaking_List_Sub_Scan(string Task_ID, string Scan_Source)
        {
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("动盘任务不存在"); }

            if (string.IsNullOrEmpty(Task.Work_Person)) { throw new Exception("未设置动盘作业人"); }

            WMS_Stocktaking_Scan Scan = new WMS_Stocktaking_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.LinkMainCID = Task.LinkMainCID;
            Scan.Link_TaskID = Task.Task_ID;
            Scan.Create_DT = DateTime.Now;
            Scan.Location = Task.Location;
            Scan.Scan_Source = Scan_Source;
            Scan.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
            Scan.Package_Type = WMS_Stock_Package_Enum.整箱.ToString();

            if (db.WMS_Location.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Scan.Location).Any() == false) { throw new Exception("系统中不存在该库位"); }

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == Scan.LinkMainCID && x.Location == Scan.Location).ToList();
            List<string> Line_MatSn_List = Stock_List.Select(x => x.MatSn).Distinct().ToList();
            if (Stock_List.Any() == false) { throw new Exception("该库位不存在产品"); }

            //执行解码
            Decode_Scan De_Scan = new Decode_Scan();
            List<string> Brand_List = new List<string>();
            Brand_List.Add(Decode_Scan_Brand.NSK.ToString());
            Brand_List.Add(Decode_Scan_Brand.NMB.ToString());
            string MatBrand = string.Empty;
            foreach (var Brand in Brand_List)
            {
                De_Scan = this.Decode_Scan_Source(Brand, Scan.Scan_Source, Line_MatSn_List);
                if (De_Scan.Is_Scan_Error == false)
                {
                    MatBrand = Brand;
                    break;
                }
            }

            if (string.IsNullOrEmpty(MatBrand))
            {
                throw new Exception("Error，未识别二维码『" + Scan_Source + "』");
            }
            else
            {
                De_Scan = this.Decode_Scan_Source(MatBrand, Scan.Scan_Source, Line_MatSn_List);
                Scan.MatBrand = MatBrand;
                Scan.MatSn = De_Scan.Decode_MatSn;

                ////验证是否系统中存在此型号
                //if (db.Material.Where(x => x.LinkMainCID == Task.LinkMainCID && x.MatBrand == Scan.MatBrand && x.MatSn == Scan.MatSn).Any() == false)
                //{
                //    throw new Exception("系统中不存在扫描型号");
                //}

                Scan.Scan_Quantity = De_Scan.Decode_Scan_Quantity;
                db.WMS_Stocktaking_Scan.Add(Scan);
                MyDbSave.SaveChange(db);
            }
        }

        //动盘扫描列表(端数)
        public DataTable WMS_Out_Pick_Stocktaking_List_Sub_Other(string Task_ID)
        {
            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("动盘任务不存在"); }
            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == TaskID && x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).ToList();

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序");
            dt.Columns.Add("型号");
            dt.Columns.Add("数量");
            int i = 0;
            foreach (var x in Scan_List)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["序"] = i;
                dr["型号"] = x.MatSn;
                dr["数量"] = x.Scan_Quantity;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //动盘扫描(端数)
        public void WMS_Out_Pick_Stocktaking_List_Sub_Other_Scan(string Task_ID, string Scan_Source, string Quantity)
        {
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("动盘任务不存在"); }

            if (string.IsNullOrEmpty(Task.Work_Person)) { throw new Exception("未设置动盘作业人"); }

            WMS_Stocktaking_Scan Scan = new WMS_Stocktaking_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.LinkMainCID = Task.LinkMainCID;
            Scan.Link_TaskID = Task.Task_ID;
            Scan.Create_DT = DateTime.Now;
            Scan.Location = Task.Location;
            Scan.Scan_Source = Scan_Source;
            Scan.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
            Scan.Package_Type = WMS_Stock_Package_Enum.零头.ToString();

            if (db.WMS_Location.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Scan.Location).Any() == false) { throw new Exception("系统中不存在该库位"); }

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == Scan.LinkMainCID && x.Location == Scan.Location).ToList();
            List<string> Line_MatSn_List = Stock_List.Select(x => x.MatSn).Distinct().ToList();
            if (Stock_List.Any() == false) { throw new Exception("该库位不存在产品"); }

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

            //无法识别
            if (Flag_Success == false) { Scan.MatSn = Scan.Scan_Source.Trim(); }

            ////验证是否系统中存在此型号
            //if (db.Material.Where(x => x.LinkMainCID == Task.LinkMainCID && x.MatSn == Scan.MatSn).Any() == false)
            //{
            //    throw new Exception("系统中不存在扫描的型号");
            //}

            //Material Mat = db.Material.Where(x => x.LinkMainCID == Task.LinkMainCID && x.MatSn == Scan.MatSn).FirstOrDefault();
            //if (Mat == null) { throw new Exception("系统中不存在扫描的型号"); }

            //Scan.MatBrand = Mat.MatBrand;

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

    //验货作业
    public partial class WmsService : IWmsService
    {
        public DataTable WMS_Out_Task_List_A(Guid MainCID)
        {
            var query = db.WMS_Out_Head.Where(x => x.LinkMainCID == MainCID && x.Status != WMS_Out_Global_State_Enum.已出库.ToString()).AsQueryable();
            query = query.Where(x => x.Status == WMS_Out_Global_State_Enum.待验货.ToString() || x.Status == WMS_Out_Global_State_Enum.待包装.ToString()).AsQueryable();
            query = query.Where(x => x.Work_Out_Person != "").AsQueryable();

            List<WMS_Out_Head> Head_List = query.Where(x => x.Logistics_Mode == Logistics_Out_Mode_Enum.自送.ToString()).ToList();
            List<Guid> ID_List = Head_List.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Out_Line> Line_List_Sub = new List<WMS_Out_Line>();
            foreach (var x in Head_List)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("任务编号");
            dt.Columns.Add("型号数");
            dt.Columns.Add("产品数");
            dt.Columns.Add("作业人");
            dt.Columns.Add("页码");
            dt.Columns.Add("运输方式");
            foreach (var x in Head_List)
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Head_ID;
                dr["任务编号"] = x.Task_Bat_No_Str;
                dr["型号数"] = x.MatSn_Count;
                dr["产品数"] = x.Quantity_Sum;
                dr["作业人"] = x.Work_Out_Person;
                if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    dr["页码"] = "0";
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    dr["页码"] = "1";
                }

                dr["运输方式"] = x.Logistics_Mode;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable WMS_Out_Task_List_B(Guid MainCID)
        {
            var query = db.WMS_Out_Head.Where(x => x.LinkMainCID == MainCID && x.Status != WMS_Out_Global_State_Enum.已出库.ToString()).AsQueryable();
            query = query.Where(x => x.Status == WMS_Out_Global_State_Enum.待验货.ToString() || x.Status == WMS_Out_Global_State_Enum.待包装.ToString()).AsQueryable();
            query = query.Where(x => x.Work_Out_Person != "").AsQueryable();

            List<WMS_Out_Head> Head_List = query.Where(x => x.Logistics_Mode == Logistics_Out_Mode_Enum.快递.ToString()).ToList();
            List<Guid> ID_List = Head_List.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Out_Line> Line_List_Sub = new List<WMS_Out_Line>();
            foreach (var x in Head_List)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("任务编号");
            dt.Columns.Add("型号数");
            dt.Columns.Add("产品数");
            dt.Columns.Add("作业人");
            dt.Columns.Add("页码");
            dt.Columns.Add("运输方式");
            foreach (var x in Head_List)
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Head_ID;
                dr["任务编号"] = x.Task_Bat_No_Str;
                dr["型号数"] = x.MatSn_Count;
                dr["产品数"] = x.Quantity_Sum;
                dr["作业人"] = x.Work_Out_Person;
                if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    dr["页码"] = "0";
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    dr["页码"] = "1";
                }

                dr["运输方式"] = x.Logistics_Mode;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable WMS_Out_Task_List_C(Guid MainCID)
        {
            var query = db.WMS_Out_Head.Where(x => x.LinkMainCID == MainCID && x.Status != WMS_Out_Global_State_Enum.已出库.ToString()).AsQueryable();
            query = query.Where(x => x.Status == WMS_Out_Global_State_Enum.待验货.ToString() || x.Status == WMS_Out_Global_State_Enum.待包装.ToString()).AsQueryable();
            query = query.Where(x => x.Work_Out_Person != "").AsQueryable();

            List<WMS_Out_Head> Head_List = query.Where(x => x.Logistics_Mode == Logistics_Out_Mode_Enum.物流.ToString()).ToList();
            List<Guid> ID_List = Head_List.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Out_Line> Line_List_Sub = new List<WMS_Out_Line>();
            foreach (var x in Head_List)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
            }

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("任务编号");
            dt.Columns.Add("型号数");
            dt.Columns.Add("产品数");
            dt.Columns.Add("作业人");
            dt.Columns.Add("页码");
            dt.Columns.Add("运输方式");
            foreach (var x in Head_List)
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Head_ID;
                dr["任务编号"] = x.Task_Bat_No_Str;
                dr["型号数"] = x.MatSn_Count;
                dr["产品数"] = x.Quantity_Sum;
                dr["作业人"] = x.Work_Out_Person;
                if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
                {
                    dr["页码"] = "0";
                }
                else if (x.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
                {
                    dr["页码"] = "1";
                }

                dr["运输方式"] = x.Logistics_Mode;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable WMS_Out_Task_By_Tray_No(string HeadID, string Tray_No)
        {
            Guid Head_ID = Guid.NewGuid();
            try { Head_ID = new Guid(HeadID); } catch { }
            List<WMS_Out_Scan> List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head_ID && x.Tray_No == Tray_No && x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).OrderByDescending(x => x.Create_DT).ToList();

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

        public void WMS_Out_Task_Scan_Item(string HeadID, string Tray_No, string Scan_Source)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Tray_No = Tray_No == null ? string.Empty : Tray_No.Trim();
            if (string.IsNullOrEmpty(Tray_No)) { throw new Exception("未填写托盘号或箱号"); }

            int TrayNo = 0;
            try { TrayNo = Convert.ToInt32(Tray_No.Trim()); } catch { throw new Exception("托盘号或箱号必须为数字"); }

            Guid Link_Head_ID = new Guid(HeadID);
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Link_Head_ID);
            if (Head == null) { throw new Exception("任务编号未匹配"); }

            if (string.IsNullOrEmpty(Head.Work_Out_Person)) { throw new Exception("未派工"); }

            if (Head.Status == WMS_Out_Global_State_Enum.待出库.ToString())
            {
                throw new Exception("已执行验货，请勿重复操作");
            }

            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            List<string> Line_MatSn_List = Line_List.Select(x => x.MatSn).ToList();
            Line_MatSn_List = Line_MatSn_List.Distinct().ToList();

            WMS_Out_Scan Scan = new WMS_Out_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.Create_DT = DateTime.Now;
            Scan.Link_Head_ID = Head.Head_ID;
            Scan.LinkMainCID = Head.LinkMainCID;
            Scan.Scan_Source = Scan_Source;
            Scan.Tray_No = Tray_No;
            Scan.Package_Type = WMS_Stock_Package_Enum.整箱.ToString();

            //执行解码
            Decode_Scan De_Scan = new Decode_Scan();
            string MatBrand = string.Empty;
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

            if (string.IsNullOrEmpty(MatBrand))
            {
                throw new Exception("Error，未识别二维码『" + Scan_Source + "』");
            }
            else
            {
                De_Scan = this.Decode_Scan_Source(MatBrand, Scan.Scan_Source, Line_MatSn_List);
                Scan.MatSn = De_Scan.Decode_MatSn;
                Scan.Scan_Quantity = De_Scan.Decode_Scan_Quantity;
                db.WMS_Out_Scan.Add(Scan);
                MyDbSave.SaveChange(db);
            }
        }

        public DataTable WMS_Out_Task_By_Tray_No_Other(string HeadID, string Tray_No, string Case_No)
        {
            Guid Head_ID = Guid.NewGuid();
            try { Head_ID = new Guid(HeadID); } catch { }

            List<WMS_Out_Scan> List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head_ID && x.Tray_No == Tray_No && x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).OrderByDescending(x => x.Create_DT).ToList();
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

        public void WMS_Out_Task_Scan_Item_Other(string HeadID, string Tray_No, string Scan_Source, string Quantity, string Case_No)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Quantity = Quantity == null ? string.Empty : Quantity.Trim();
            if (string.IsNullOrEmpty(Quantity)) { throw new Exception("未填写数量"); }

            Guid Link_Head_ID = new Guid(HeadID);
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Link_Head_ID);
            if (Head == null) { throw new Exception("任务编号未匹配"); }

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

            //验证箱号为数字

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

            if (string.IsNullOrEmpty(Head.Work_Out_Person)) { throw new Exception("未派工"); }

            if (Head.Status == WMS_Out_Global_State_Enum.待出库.ToString())
            {
                throw new Exception("已执行验货，请勿重复操作");
            }

            List<string> Line_MatSn_List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).Select(x => x.MatSn).Distinct().ToList();

            WMS_Out_Scan Scan = new WMS_Out_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.Create_DT = DateTime.Now;
            Scan.Link_Head_ID = Head.Head_ID;
            Scan.LinkMainCID = Head.LinkMainCID;
            Scan.Scan_Source = Scan_Source;
            Scan.Tray_No = Tray_No;
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

            try
            {
                Scan.Scan_Quantity = Convert.ToInt32(Quantity);
                if (Scan.Scan_Quantity <= 0) { throw new Exception("未填入正确的数字"); }
            }
            catch { throw new Exception("填入数量的格式错误"); }

            if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString() && db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No && x.MatSn == Scan.MatSn).Any())
            {
                throw new Exception("该箱中已存在相同型号的产品");
            }

            db.WMS_Out_Scan.Add(Scan);
            MyDbSave.SaveChange(db);
        }

        //获取快递单列表
        public DataTable WMS_Out_Track_List(Guid MainCID)
        {
            List<WMS_Track> List = db.WMS_Track.Where(x => x.LinkMainCID == MainCID && x.Link_Head_ID == Guid.Empty).ToList();

            DataTable dt = new DataTable("List");
            dt.Columns.Add("托/箱号");
            dt.Columns.Add("快递单号");
            dt.Columns.Add("重量");
            foreach (var x in List.OrderByDescending(x => x.Create_DT).ToList())
            {
                DataRow dr = dt.NewRow();
                dr["托/箱号"] = x.Tray_No;
                dr["快递单号"] = x.Tracking_No;
                dr["重量"] = x.Weight.ToString("N2");
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //快递单扫描
        public void WMS_Out_Track_Scan_Item(Guid MainCID, string Tray_No, string Scan_Source, string Weight)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取快递扫码内容"); }

            Tray_No = Tray_No == null ? string.Empty : Tray_No.Trim();
            if (string.IsNullOrEmpty(Tray_No)) { throw new Exception("未获取托盘号或箱号"); }

            decimal Box_Weight = 0;
            try { Box_Weight = Convert.ToDecimal(Weight.Trim()); } catch { Box_Weight = 0; }
            Box_Weight = Box_Weight <= 0 ? 0 : Box_Weight;

            if (Box_Weight == 0) { throw new Exception("请输入正确的重量"); }

            List<WMS_Track> OLD_Track_List = db.WMS_Track.Where(x => x.Link_Head_ID == Guid.Empty).ToList();
            if (OLD_Track_List.Where(x => x.Tracking_No == Scan_Source).Any()) { throw new Exception("此快递单已扫描"); }

            WMS_Track T = new WMS_Track();
            T.Tracking_ID = MyGUID.NewGUID();
            T.Tracking_No = Scan_Source;
            T.Weight = Box_Weight;
            T.Tray_No = Tray_No;
            T.Scan_PDA_Date = DateTime.Now;
            T.LinkMainCID = MainCID;
            T.Create_DT = DateTime.Now;
            T.Tracking_Type = Tracking_Type_Enum.送货.ToString();
            db.WMS_Track.Add(T);
            MyDbSave.SaveChange(db);
        }

        //快递单信息
        public DataTable WMS_Out_Track_List_Sub(Guid MainCID, string Scan_Source)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取快递扫码内容"); }

            List<WMS_Track> List = db.WMS_Track.Where(x => x.LinkMainCID == MainCID && x.Tracking_No == Scan_Source).ToList();

            DataTable dt = new DataTable("List");
            dt.Columns.Add("托/箱号");
            dt.Columns.Add("快递单号");
            dt.Columns.Add("重量");
            int i = 0;
            foreach (var x in List.OrderByDescending(x => x.Create_DT).ToList())
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["托/箱号"] = x.Tray_No;
                dr["快递单号"] = x.Tracking_No;
                dr["重量"] = x.Weight.ToString("N2");
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //快递单删除
        public void WMS_Out_Track_Scan_Delete(Guid MainCID, string Scan_Source)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取快递扫码内容"); }

            List<WMS_Track> List = db.WMS_Track.Where(x => x.LinkMainCID == MainCID && x.Tracking_No == Scan_Source).ToList();

            if (List.Any())
            {
                db.WMS_Track.RemoveRange(List);
                MyDbSave.SaveChange(db);
            }
        }
    }
}