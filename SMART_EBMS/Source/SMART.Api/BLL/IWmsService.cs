using System;
using System.Collections.Generic;
using System.Linq;
using SMART.Api.Models;
using System.Data.Entity;
using System.Web;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Threading;
using NPOI.HSSF.UserModel;

namespace SMART.Api
{
    public partial interface IWmsService
    {
        //仓库储位
        PageList<WMS_Location> Get_WMS_Location_PageList(WMS_Location_Filter MF);
        WMS_Location Get_WMS_Location_Empty();
        WMS_Location Get_WMS_Location_Item(Guid Loc_ID);
        Guid Create_WMS_Location(WMS_Location WL);
        void Batch_Create_WMS_Location(Guid LinkMainCID);
        void Batch_Create_WMS_Location_Temp(Guid LinkMainCID);
        void Set_WMS_Location_Item(Guid Loc_ID, WMS_Location WL);
        void Delete_WMS_Location(Guid Loc_ID);

        //当前库存
        PageList<WMS_Stock_Group> Get_WMS_Stock_Group_PageList(WMS_Stock_Filter MF);
        PageList<WMS_Stock_Group_Location> Get_WMS_Stock_By_Location_List(WMS_Stock_Filter MF);
        PageList<WMS_Stock_Group_Location> Get_WMS_Stock_By_Location_List_For_Move(WMS_Stock_Filter MF);
        List<WMS_Stock_Group> Get_WMS_Stock_Group_List_For_Move(WMS_Stock_Filter MF);
        List<WMS_Stock_Group> Get_WMS_Stock_Group_List(WMS_Stock_Filter MF);
        WMS_Stock Get_WMS_Stock_Item(Guid Stock_ID);
        List<WMS_Stock_Group> Get_WMS_Stock_Group_List_By_Case(WMS_Stock_Filter MF);
        PageList<WMS_Stock_Record> Get_WMS_Stock_Record_PageList(WMS_Stock_Filter MF);

        List<WMS_Stock_Group> Get_WMS_Stock_List(WMS_Stock_Filter MF);
        List<WMS_Out_Line> Get_WMS_Out_Line_List_For_Stock(WMS_Stock_Filter MF);
        List<WMS_Stock_Group> Get_WMS_Stock_Temp_List_For_Stock(WMS_Stock_Filter MF);
        List<WMS_Stock_Record_Info> Get_WMS_Stock_Record_Info_List(WMS_Stock_Filter MF);
        string Get_WMS_Stock_To_Excel(Guid LinkMainCID);
        string Get_WMS_Stock_All_To_Excel(Guid LinkMainCID);

        List<WMS_Stock_Group> Get_WMS_Stock_Group_List_For_WMS_Up(Guid Move_ID);
        List<WMS_Stock_Group> Get_WMS_Stock_Group_List_For_WMS_Move(Guid Move_ID);

        void Create_WMS_Move_From_WMS_Move_Recommend(string Location, Guid Link_MainCID);

        //盘库任务
        PageList<WMS_Location> Get_WMS_Stocktaking_Task_PageList_Notice(WMS_Stock_Filter MF);
        int Get_Total_WMS_Stock_Quantity(Guid LinkMainCID);
        PageList<WMS_Stock_Task> Get_WMS_Stock_Task_PageList(WMS_Stock_Filter MF);
        PageList<WMS_Stock_Task> Get_WMS_Stock_Task_PageList_Pick(WMS_Stock_Filter MF);
        WMS_Stock_Task Get_WMS_Stock_Task_Item_DB(Guid TaskID);
        List<WMS_Stocktaking_Scan> Get_WMS_Stocktaking_Scan_List(Guid TaskID, string MatSn);
        WMS_Stock_Task Get_WMS_Stock_Task_Item(Guid TaskID);
        void Set_WMS_Stock_Task_Work_Person(WMS_Stock_Task Task);
        void Batch_Create_WMS_Stocktaking_With_Work_Person(List<Guid> Task_IDList, List<string> Work_Person_List);
        void Reset_WMS_Stocktaking_Task_Scan_By_MatSn(Guid TaskID, string MatSn);
        void Finish_WMS_Stocktaking_Task(Guid TaskID, User U);
        void Delete_WMS_Stocktaking_Task(Guid TaskID);
        string Get_WMS_Stocktaking_To_Excel(Guid LinkMainCID, string Location);
        WMS_Stocktaking Get_WMS_Stocktaking_Item(Guid TaskID, string MatSn);
        void Set_WMS_Stocktaking_Task_For_MatSn(Guid TaskID, string MatSn, int Quantity);
        void Reset_WMS_Stocktaking_Task_Scan(Guid TaskID);

        //动盘（端数库位）
        WMS_Stock_Task Get_WMS_Stock_Task_Item_Other(Guid TaskID);
        void Finish_WMS_Stocktaking_Task_Other(Guid TaskID);

        //临时批量生成首次盘库记录（按库位）
        void Batch_Create_WMS_Stock_Record_By_Location(HttpPostedFileBase ExcelFile, string Location, User U);
        PageList<WMS_Stock_Record> Get_WMS_Stock_Record_PageList_Temp(WMS_Stock_Filter MF);
        void Batch_Create_WMS_Stock_Record(HttpPostedFileBase ExcelFile, User U);

        //盈亏记录
        PageList<WMS_Profit_Loss> Get_WMS_Profit_Loss_PageList(WMS_Stock_Filter MF);
        PageList<WMS_Profit_Loss_Other> Get_WMS_Profit_Loss_Other_PageList(WMS_Stock_Filter MF);
        void Confirm_WMS_Profit_Loss_Other_Item(Guid Line_ID, User U);

        //首次盘库
        WMS_Stock_Task Get_WMS_Stock_Task_Item_First(Guid TaskID);
        void Finish_WMS_Stocktaking_Task_First(Guid TaskID);
        string Get_WMS_Stocktaking_List_To_Excel(Guid LinkMainCID);
        PageList<WMS_Stock_Group> Get_WMS_Stock_Group_First_PageList(WMS_Stock_Filter MF);
        WMS_Stocktaking_Scan Get_WMS_Stocktaking_Scan_Item_DB(Guid Scan_ID);
        void Batch_Create_WMS_Stock_By_Location(HttpPostedFileBase ExcelFile, string Location, User U);
        void Batch_Create_WMS_Stock(HttpPostedFileBase ExcelFile, User U);
        string Get_WMS_Stocktaking_List_All_To_Excel(Guid LinkMainCID);

        //快递费用
        PageList<WMS_Track> Get_WMS_In_Track_PageList(WMS_In_Filter MF);
        PageList<WMS_Track> Get_WMS_Out_Track_PageList(WMS_Out_Filter MF);
        WMS_Track Get_WMS_Track_Empty();
        WMS_Track Get_WMS_Track_Item(Guid Tracking_ID);

        void Add_WMS_In_Track(WMS_Track Track);
        void Set_WMS_In_Track_Item(WMS_Track Track);
        void Add_WMS_Out_Track(WMS_Track Track);
        void Set_WMS_Out_Track_Item(WMS_Track Track);
        void Delete_WMS_In_Track(Guid Tracking_ID);
        void Delete_WMS_Out_Track(Guid Tracking_ID);

        //快递资料
        PageList<WMS_Track_Info> Get_WMS_Track_Info_PageList(Track_Info_Filter MF);

        //快递费用统计
        Logistics_Cost_Year GetLogistics_Cost_YearList(Logistics_Cost_Filter MF);
        List<WMS_Track> Get_WMS_In_Track_By_Month(DateTime SD, DateTime ED, Guid Logistics_ID, Guid LinkMainCID);
        string Get_WMS_In_Track_By_Month_To_Excel(DateTime SD, DateTime ED, Guid Logistics_ID, Guid LinkMainCID);
        PageList<WMS_Track> Get_WMS_Out_Track_PageList(Logistics_Cost_Filter MF);
        string Get_WMS_Out_Track_To_Excel(Logistics_Cost_Filter MF);

        //驾驶员
        PageList<WMS_Track> Get_WMS_In_Track_With_Driver(Logistics_Cost_Filter MF);
        WMS_Track_Info Get_WMS_Track_Info_DB(Guid InfoID);
        string Get_WMS_In_Track_With_Driver_To_Excel(Logistics_Cost_Filter MF);
        List<WMS_Track_Info> Get_WMS_Track_Info_List_From_Upload(HttpPostedFileBase ExcelFile, User U);
        void Batch_Create_WMS_Track_Info(List<WMS_Track_Info> List, User U);

        //报废出库
        PageList<WMS_Waste_Head> Get_WMS_Waste_Head_PageList(WMS_Waste_Filter MF);
        PageList<WMS_Waste_Head> Get_WMS_Waste_Head_PageList_Sub(WMS_Waste_Filter MF);
        Guid Create_WMS_Waste_Head(User U);
        WMS_Waste_Head Get_WMS_Waste_Head_DB(Guid Head_ID);
        WMS_Waste_Head Get_WMS_Waste_Head_Item(Guid Head_ID);
        void Delete_WMS_Waste_Head(Guid Head_ID);
        List<WMS_Stock_Group> Get_WMS_Stock_Group_List_For_Waste(WMS_Stock_Filter MF);
        void Create_WMS_Waste_Line(WMS_Waste_Line Line);
        List<WMS_Waste_Line> Get_WMS_Waste_Line_List(Guid HeadID);
        void Delete_WMS_Waste_Line_List(Guid HeadID);
        void Delete_WMS_Waste_Line(Guid LineID);
        void Set_WMS_Task_Waste(Guid HeadID);
        void Set_WMS_Waste_Line(WMS_Waste_Line Line);
        string Get_WMS_Waste_Line_By_Head_To_Excel(List<WMS_Waste_Line> Line_List);
        void Confirm_WMS_Task_Waste(Guid HeadID, User U);
        void Confirm_WMS_Task_Waste_By_Accounting(Guid HeadID, User U);
        void Refuse_WMS_Task_Waste(Guid HeadID, string Remark, User U);
        void Finish_WMS_Task_Waste(Guid HeadID);
        void Link_WMS_Task_Waste(Guid HeadID, Guid Link_HeadID);
        void Cancel_Link_WMS_Task_Waste(Guid Link_HeadID);

        //盈亏审核
        PageList<WMS_Profit_Loss_Head> Get_WMS_Profit_Loss_Head_PageList(WMS_Profit_Loss_Filter MF);
        PageList<WMS_Profit_Loss_Head> Get_WMS_Profit_Loss_Head_PageList_Sub(WMS_Profit_Loss_Filter MF);
        PageList<WMS_Profit_Loss_Head> Get_WMS_Profit_Loss_Head_PageList_Record(WMS_Profit_Loss_Filter MF);
        WMS_Profit_Loss_Head Get_WMS_Profit_Loss_Head_DB(Guid Head_ID);
        WMS_Profit_Loss_Head Get_WMS_Profit_Loss_Head_Item(Guid Head_ID);
        string Get_WMS_Profit_Loss_Line_By_Head_To_Excel(List<WMS_Profit_Loss_Line> PL_List);
        void Confirm_WMS_Profit_Loss_Head(Guid HeadID, User U);
        void Refuse_WMS_Profit_Loss_Head(Guid HeadID, string Remark, User U);
        void Confirm_WMS_Profit_Loss_Head_By_Accounting(Guid HeadID, User U);
        void Finish_WMS_Profit_Loss_Head(Guid HeadID);
    }

    public partial class WmsService : IWmsService
    {
        SmartdbContext db = new SmartdbContext();
    }

    //仓库储位
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Location> Get_WMS_Location_PageList(WMS_Location_Filter MF)
        {
            var query = db.WMS_Location.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.Location.Contains(MF.Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Type))
            {
                query = query.Where(x => x.Type == MF.Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Link_MatSn_Count))
            {
                query = query.Where(x => x.Link_MatSn_Count == MF.Link_MatSn_Count).AsQueryable();
            }

            PageList<WMS_Location> PList = new PageList<WMS_Location>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Location).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public WMS_Location Get_WMS_Location_Empty()
        {
            WMS_Location WL = new WMS_Location();
            return WL;
        }

        public WMS_Location Get_WMS_Location_Item(Guid Loc_ID)
        {
            WMS_Location WL = db.WMS_Location.Find(Loc_ID);
            WL = WL == null ? new WMS_Location() : WL;

            //创建
            if (string.IsNullOrEmpty(WL.QRCode_Path) || !File.Exists(WL.QRCode_Path))
            {
                WL.QRCode_Path = QRCode.CreateQRCode_Location(WL.Location, WL.Loc_ID);
                db.Entry(WL).State = EntityState.Modified;
                MyDbSave.SaveChange(db);
            }

            return WL;
        }

        public Guid Create_WMS_Location(WMS_Location WL)
        {
            WL.Loc_ID = MyGUID.NewGUID();
            WL.Location = WL.Location.Trim();
            WL.LinkMainCID = WL.LinkMainCID;

            WL.Type = WL.Type;

            if (WL.Type == Type_Enum.端数.ToString() && string.IsNullOrEmpty(WL.Link_MatSn_Count))
            {
                throw new Exception("端数库位类型，请选择关联型号数");
            }

            if (WL.Type == Type_Enum.整箱.ToString() && !string.IsNullOrEmpty(WL.Link_MatSn_Count))
            {
                throw new Exception("整箱库位类型，不支持关联型号");
            }

            WL.Link_MatSn_Count = WL.Link_MatSn_Count;
            WL.Remark = WL.Remark;
            this.Check_WMS_Location_Item(WL);

            db.WMS_Location.Add(WL);
            MyDbSave.SaveChange(db);
            return WL.Loc_ID;
        }

        public void Batch_Create_WMS_Location(Guid LinkMainCID)
        {
            db.WMS_Location.RemoveRange(db.WMS_Location.ToList());
            MyDbSave.SaveChange(db);
            Thread.Sleep(300);

            string Location = string.Empty;
            List<string> Location_List = new List<string>();

            string[] Char1 = { "Y", "Z" };
            string[] Char2 = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11" };
            string[] Char3 = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string[] Char4 = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] Char5 = { "L", "R" };

            for (int i = 0; i < Char1.Length; i++)
            {
                for (int j = 0; j < Char2.Length; j++)
                {
                    for (int z = 0; z < Char3.Length; z++)
                    {
                        for (int x = 0; x < Char4.Length; x++)
                        {
                            for (int w = 0; w < Char5.Length; w++)
                            {
                                Location = Char1[i] + Char2[j] + Char3[z] + Char4[x] + Char5[w];
                                Location_List.Add(Location);
                            }
                        }
                    }
                }
            }

            List<char> Char_List = new List<char>();
            string Location_Temp = string.Empty;
            List<string> Location_Temp_List = new List<string>();
            foreach (var x in Location_List)
            {
                Char_List = x.ToList();

                if (Char_List[1] == '0')
                {
                    Location_Temp = Char_List[0].ToString() + Char_List[2].ToString() + Char_List[3].ToString() + Char_List[4].ToString() + Char_List[5].ToString();
                }
                else
                {
                    Location_Temp = Char_List[0].ToString() + Char_List[1].ToString() + Char_List[2].ToString() + Char_List[3].ToString() + Char_List[4].ToString() + Char_List[5].ToString();
                }
                Location_Temp_List.Add(Location_Temp);
            }

            WMS_Location Loc = new WMS_Location();
            List<WMS_Location> Loc_List = new List<WMS_Location>();

            foreach (var x in Location_Temp_List)
            {
                Loc = new WMS_Location();
                Loc.Loc_ID = MyGUID.NewGUID();
                Loc.Location = x;
                Loc.LinkMainCID = LinkMainCID;
                Loc_List.Add(Loc);
            }

            db.WMS_Location.AddRange(Loc_List);
            MyDbSave.SaveChange(db);
        }

        public void Batch_Create_WMS_Location_Temp(Guid LinkMainCID)
        {
            string Location = string.Empty;
            List<string> Location_List = new List<string>();

            for (int i = 1; i < 100; i++)
            {
                Location = "T" + i.ToString().PadLeft(5, '0');
                Location_List.Add(Location);
            }

            WMS_Location Loc = new WMS_Location();
            List<WMS_Location> Loc_List = new List<WMS_Location>();

            foreach (var x in Location_List)
            {
                Loc = new WMS_Location();
                Loc.Loc_ID = MyGUID.NewGUID();
                Loc.Location = x;
                Loc.LinkMainCID = LinkMainCID;
                Loc_List.Add(Loc);
            }

            db.WMS_Location.AddRange(Loc_List);
            MyDbSave.SaveChange(db);
        }

        public void Set_WMS_Location_Item(Guid Loc_ID, WMS_Location WL)
        {
            WMS_Location OLD_WL = db.WMS_Location.Find(Loc_ID);
            if (OLD_WL == null) { throw new Exception("WMS_Location is null!"); }
            WL.Location = WL.Location.Trim();
            if (db.WMS_Stock.Where(x => x.Location == OLD_WL.Location).Any() && OLD_WL.Location != WL.Location)
            {
                throw new Exception("该库位当前存在库存，不支持更新名称！");
            }

            OLD_WL.Location = WL.Location.Trim();

            OLD_WL.Type = WL.Type;

            if (OLD_WL.Type == Type_Enum.端数.ToString())
            {
                if (string.IsNullOrEmpty(WL.Link_MatSn_Count))
                {
                    throw new Exception("端数库位类型，请选择关联型号数");
                }

                OLD_WL.Link_MatSn_Count = WL.Link_MatSn_Count;
            }
            else if (OLD_WL.Type == Type_Enum.整箱.ToString())
            {
                OLD_WL.Link_MatSn_Count = string.Empty;

            }

            OLD_WL.Remark = WL.Remark;
            this.Check_WMS_Location_Item(OLD_WL);

            db.Entry(OLD_WL).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_WMS_Location(Guid Loc_ID)
        {
            WMS_Location WL = db.WMS_Location.Find(Loc_ID);
            if (WL == null) { throw new Exception("WMS_Location is null!"); }

            if (db.WMS_Stock.Where(x => x.Location == WL.Location).Any())
            {
                throw new Exception("该库位当前存在库存，不支持删除！");
            }

            db.WMS_Location.Remove(WL);
            MyDbSave.SaveChange(db);
        }

        private void Check_WMS_Location_Item(WMS_Location WL)
        {

            if (WL.LinkMainCID == Guid.Empty)
            {
                throw new Exception("MainCID Is Empty!");
            }

            string Location = WL.Location;

            if (string.IsNullOrEmpty(Location))
            {
                throw new Exception("库位编号不能为空!");
            }

            //if (Location.Length != 6)
            //{
            //    throw new Exception("库位编号必须为7位！");
            //}

            //if (Location.Substring(0, 1) != "Y" && Location.Substring(0, 1) != "Z")
            //{
            //    throw new Exception("主货架编码必须为Y或Z！");
            //}

            //char Char1 = Location.Substring(1, 1).FirstOrDefault();
            //char Char2 = Location.Substring(2, 1).FirstOrDefault();

            //if (Char1 == '0')
            //{
            //    if ((Char2 >= '1' && Char2 <= '9') == false)
            //    {
            //        throw new Exception("货架排号编码必须为01~11！");
            //    }
            //}
            //else if (Char1 == '1')
            //{
            //    if (Char2 != '0' && Char2 != '1')
            //    {
            //        throw new Exception("货架排号编码必须为01~11！");
            //    }
            //}
            //else
            //{
            //    throw new Exception("货架排号编码必须为01~11！");
            //}

            //char Char3 = Location.Substring(3, 1).FirstOrDefault();

            //if ((Char3 >= 'A' && Char3 <= 'Z') == false)
            //{
            //    throw new Exception("货位号编码必须为A~Z！");
            //}

            //char Char4 = Location.Substring(4, 1).FirstOrDefault();

            //if ((Char4 >= '1' && Char4 <= '9') == false)
            //{
            //    throw new Exception("层号编码必须为1~9！");
            //}

            //if (Location.Substring(5, 1) != "L" && Location.Substring(5, 1) != "R")
            //{
            //    throw new Exception("左右编码必须为L或R！");
            //}

            if (db.WMS_Location.Where(x => x.LinkMainCID == WL.LinkMainCID && x.Loc_ID != WL.Loc_ID && x.Location == WL.Location).Any())
            {
                throw new Exception("库位编号重复!");
            }
        }

    }

    //当前库存
    public partial class WmsService : IWmsService
    {
        //按产品
        public PageList<WMS_Stock_Group> Get_WMS_Stock_Group_PageList(WMS_Stock_Filter MF)
        {
            //库存信息
            var query = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn_A))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn_A)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn_B))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn_B)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.MatSn.Contains(MF.Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                query = query.Where(x => x.MatBrand.Contains(MF.MatBrand)).AsQueryable();
            }

            List<WMS_Stock> Stock_List = query.ToList();
            var Group = from x in Stock_List
                        group x by x.MatSn into g
                        select new
                        {
                            MatSn = g.Key,
                            MatName = g.Select(c => c.MatName).FirstOrDefault(),
                            MatBrand = g.Select(c => c.MatBrand).FirstOrDefault(),
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = x.MatSn;
                Stock_Group.MatName = x.MatName;
                Stock_Group.MatBrand = x.MatBrand;
                Group_List.Add(Stock_Group);
            }

            //临时库存信息
            List<string> MatSn_List_Temp = Group_List.Select(x => x.MatSn).Distinct().ToList();
            var query_Temp = db.WMS_Stock_Temp.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query_Temp = query_Temp.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn_A))
            {
                query_Temp = query_Temp.Where(x => x.MatSn.Contains(MF.MatSn_A)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn_B))
            {
                query_Temp = query_Temp.Where(x => x.MatSn.Contains(MF.MatSn_B)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query_Temp = query_Temp.Where(x => x.MatSn.Contains(MF.Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                query_Temp = query_Temp.Where(x => x.MatBrand.Contains(MF.MatBrand)).AsQueryable();
            }

            List<WMS_Stock_Temp> Stock_Temp_List_DB = query_Temp.ToList();

            List<WMS_Stock_Temp> Stock_Temp_List_Search = Stock_Temp_List_DB.Where(x => MatSn_List_Temp.Contains(x.MatSn) == false).ToList();
            if (Stock_Temp_List_Search.Any())
            {
                var Group_Temp = from x in Stock_Temp_List_Search
                                 group x by x.MatSn into g
                                 select new
                                 {
                                     MatSn = g.Key,
                                     MatName = g.Select(c => c.MatName).FirstOrDefault(),
                                     MatBrand = g.Select(c => c.MatBrand).FirstOrDefault(),
                                 };
                foreach (var x in Group_Temp)
                {
                    Stock_Group = new WMS_Stock_Group();
                    Stock_Group.MatSn = x.MatSn;
                    Stock_Group.MatName = x.MatName;
                    Stock_Group.MatBrand = x.MatBrand;
                    Group_List.Add(Stock_Group);
                }
            }

            PageList<WMS_Stock_Group> PList = new PageList<WMS_Stock_Group>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Group_List.Count();

            Group_List = Group_List.Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            List<string> MatSn_List = Group_List.Select(x => x.MatSn).Distinct().ToList();

            //预占用库存
            List<Guid> Out_HeadID_List_DB = db.WMS_Out_Head.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status == WMS_Out_Global_State_Enum.待配货.ToString()).Select(x => x.Head_ID).ToList();
            List<Guid> Out_HeadID_List = new List<Guid>();
            List<WMS_Stock_Temp> Stock_Temp_List_Sub = Stock_Temp_List_DB.Where(x => Out_HeadID_List_DB.Contains(x.WMS_Out_Head_ID)).ToList();

            foreach (var ID in Out_HeadID_List_DB)
            {
                if (Stock_Temp_List_Sub.Where(c => c.WMS_Out_Head_ID == ID).Any() == false)
                {
                    Out_HeadID_List.Add(ID);
                }
            }

            List<WMS_Out_Line> Out_Line_List = db.WMS_Out_Line.Where(x => Out_HeadID_List.Contains(x.Link_Head_ID)).ToList();
            Out_Line_List = Out_Line_List.Where(x => MatSn_List.Contains(x.MatSn)).ToList();

            //已占用库存
            List<WMS_Stock_Temp> Stock_Temp_List = Stock_Temp_List_DB.Where(x => MatSn_List.Contains(x.MatSn)).ToList();

            foreach (var x in Group_List)
            {
                x.Quantity_Preoccupancy = Out_Line_List.Where(c => c.MatSn == x.MatSn).Sum(c => c.Quantity);
                x.Quantity_Occupied = Stock_Temp_List.Where(c => c.MatSn == x.MatSn).Sum(c => c.Quantity);
                x.Quantity_Avaliable = Stock_List.Where(c => c.MatSn == x.MatSn).Sum(c => c.Quantity) - x.Quantity_Preoccupancy;
                x.Quantity_Sum = x.Quantity_Occupied + x.Quantity_Avaliable + x.Quantity_Preoccupancy;
            }

            //计算差异数
            WMS_Stock_Record_Info Record = new WMS_Stock_Record_Info();
            List<WMS_Stock_Record_Info> List = new List<WMS_Stock_Record_Info>();

            //盘库盈亏记录
            List<WMS_Profit_Loss> Profit_Loss_List = db.WMS_Profit_Loss.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status == WMS_Profit_Loss_Status_Enum.已确定.ToString() && MatSn_List.Contains(x.MatSn)).OrderBy(x => x.Create_DT).ToList();
            foreach (var x in Profit_Loss_List)
            {
                x.Diff_Quantity = x.New_Quantity - x.Old_Quantity;

                Record = new WMS_Stock_Record_Info();
                Record.MatSn = x.MatSn;
                Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                Record.Record_Type = "盈亏记录";
                Record.Quantity = x.Diff_Quantity;
                List.Add(Record);
            }

            //出库入库记录
            List<WMS_Stock_Record> Record_List = db.WMS_Stock_Record.Where(x => x.LinkMainCID == MF.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
            foreach (var x in Record_List)
            {
                Record = new WMS_Stock_Record_Info();
                Record.MatSn = x.MatSn;
                if (x.Remark == WMS_Stock_Record_Remark_Enum.订单入库.ToString())
                {
                    Record.Record_Type = WMS_Stock_Record_Remark_Enum.订单入库.ToString();
                    Record.Quantity = x.Quantity;
                    Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                }
                else if (x.Remark == WMS_Stock_Record_Remark_Enum.订单出库.ToString())
                {
                    Record.Record_Type = WMS_Stock_Record_Remark_Enum.订单出库.ToString();
                    Record.Quantity = x.Quantity;
                    Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                }
                else if (x.Remark == WMS_Stock_Record_Remark_Enum.首次盘库.ToString())
                {
                    Record.Record_Type = WMS_Stock_Record_Remark_Enum.首次盘库.ToString();
                    Record.Quantity = x.Quantity;
                    Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                }
                else if (x.Remark == WMS_Stock_Record_Remark_Enum.报废出库.ToString())
                {
                    Record.Record_Type = WMS_Stock_Record_Remark_Enum.报废出库.ToString();
                    Record.Quantity = x.Quantity;
                    Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                }

                List.Add(Record);
            }

            List<string> DT_List = new List<string>();

            WMS_Stock_Record_Info Record_Final = new WMS_Stock_Record_Info();
            List<WMS_Stock_Record_Info> List_Final = new List<WMS_Stock_Record_Info>();
            List<WMS_Stock_Record_Info> List_Sub = new List<WMS_Stock_Record_Info>();
            List<WMS_Stock_Record_Info> List_Sub_Sub = new List<WMS_Stock_Record_Info>();

            foreach (var MatSn in MatSn_List)
            {
                List_Sub = List.Where(c => c.MatSn == MatSn).ToList();
                DT_List = List_Sub.Select(x => x.Act_Date_Str).Distinct().ToList();
                foreach (var DT in DT_List)
                {
                    List_Sub_Sub = List_Sub.Where(c => c.Act_Date_Str == DT).ToList();
                    Record_Final = new WMS_Stock_Record_Info();
                    Record_Final.MatSn = MatSn;
                    Record_Final.Act_Date_Str = DT;
                    Record_Final.First_Quantity = List_Sub_Sub.Where(c => c.Record_Type == WMS_Stock_Record_Remark_Enum.首次盘库.ToString()).Sum(c => c.Quantity);
                    Record_Final.In_Quantity = List_Sub_Sub.Where(c => c.Record_Type == WMS_Stock_Record_Remark_Enum.订单入库.ToString()).Sum(c => c.Quantity);
                    Record_Final.Out_Quantity = List_Sub_Sub.Where(c => c.Record_Type == WMS_Stock_Record_Remark_Enum.订单出库.ToString()).Sum(c => c.Quantity);
                    Record_Final.Waste_Out_Quantity = List_Sub_Sub.Where(c => c.Record_Type == WMS_Stock_Record_Remark_Enum.报废出库.ToString()).Sum(c => c.Quantity);
                    Record_Final.Profit_Loss_Quantity = List_Sub_Sub.Where(c => c.Record_Type == "盈亏记录").Sum(c => c.Quantity);
                    List_Final.Add(Record_Final);
                }
            }

            int Quantity_Record_Sum = 0;
            foreach (var x in Group_List)
            {
                foreach (var xx in List_Final.Where(c => c.MatSn == x.MatSn).ToList())
                {
                    Quantity_Record_Sum += xx.First_Quantity + xx.In_Quantity - xx.Out_Quantity - xx.Waste_Out_Quantity + xx.Profit_Loss_Quantity;
                }
                x.Quantity_Diff = Quantity_Record_Sum - x.Quantity_Sum;
                Quantity_Record_Sum = 0;
            }

            PList.Rows = Group_List;
            return PList;
        }

        public List<WMS_Out_Line> Get_WMS_Out_Line_List_For_Stock(WMS_Stock_Filter MF)
        {
            List<WMS_Out_Head> Head_List = db.WMS_Out_Head.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status == WMS_Out_Global_State_Enum.待配货.ToString()).ToList();
            List<Guid> HeadID_List_DB = Head_List.Select(x => x.Head_ID).ToList();
            List<Guid> HeadID_List = new List<Guid>();
            List<WMS_Stock_Temp> Stock_Temp_List_Sub = db.WMS_Stock_Temp.Where(x => HeadID_List_DB.Contains(x.WMS_Out_Head_ID)).ToList();

            foreach (var ID in HeadID_List_DB)
            {
                if (Stock_Temp_List_Sub.Where(c => c.WMS_Out_Head_ID == ID).Any() == false)
                {
                    HeadID_List.Add(ID);
                }
            }

            List<WMS_Out_Line> Line_List_DB = db.WMS_Out_Line.Where(x => HeadID_List.Contains(x.Link_Head_ID) && x.MatSn == MF.MatSn).ToList();

            foreach (var x in Line_List_DB)
            {
                x.Out_DT_Str = Head_List.Where(c => c.Head_ID == x.Link_Head_ID).FirstOrDefault().Out_DT_Str;
            }

            return Line_List_DB;
        }

        public List<WMS_Stock_Group> Get_WMS_Stock_Temp_List_For_Stock(WMS_Stock_Filter MF)
        {
            List<WMS_Stock_Temp> Stock_Temp_List_DB = db.WMS_Stock_Temp.Where(x => x.LinkMainCID == MF.LinkMainCID && x.MatSn == MF.MatSn).ToList();
            List<WMS_Stock_Temp> Stock_Temp_List = new List<WMS_Stock_Temp>();
            WMS_Stock_Temp Temp = new WMS_Stock_Temp();

            var Group = from x in Stock_Temp_List_DB
                        group x by new { x.Location, x.Package, x.Price } into g
                        select new
                        {
                            Location = g.Key.Location,
                            Package = g.Key.Package,
                            Price = g.Key.Price,
                            Line_Count = g.Count(),
                            Quantity_Sum = g.Sum(c => c.Quantity),
                            In_DT = g.FirstOrDefault().WMS_In_DT,
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = MF.MatSn;
                Stock_Group.Location = x.Location;
                if (x.Package == WMS_Stock_Package_Enum.整箱.ToString())
                {
                    Stock_Group.Line_Count = x.Line_Count;
                }
                else
                {
                    Stock_Group.Line_Count = 0;
                }
                Stock_Group.Quantity_Sum = x.Quantity_Sum;
                Stock_Group.In_DT = x.In_DT;
                Stock_Group.Price = x.Price;
                Group_List.Add(Stock_Group);
            }

            return Group_List;
        }

        public string Get_WMS_Stock_To_Excel(Guid LinkMainCID)
        {
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.LinkMainCID = LinkMainCID;
            MF.PageIndex = 1;
            MF.PageSize = 100000;
            PageList<WMS_Stock_Group> Group_List = Get_WMS_Stock_Group_PageList(MF);

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();

            TableHeads.Add("品牌");
            TableHeads.Add("产品型号");
            TableHeads.Add("在库数量");
            TableHeads.Add("待配货数");
            TableHeads.Add("已配货数");
            TableHeads.Add("可用余量");
            TableHeads.Add("账面数量");
            TableHeads.Add("在库差异数");
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
            foreach (var x in Group_List.Rows.Where(c => c.Quantity_Diff != 0).ToList())
            {
                i++;
                newRow = DT.NewRow();
                newRow["品牌"] = x.MatBrand;
                newRow["产品型号"] = x.MatSn;
                newRow["在库数量"] = x.Quantity_Sum;
                newRow["待配货数"] = x.Quantity_Preoccupancy;
                newRow["已配货数"] = x.Quantity_Occupied;
                newRow["可用余量"] = x.Quantity_Avaliable;
                newRow["账面数量"] = x.Quantity_Sum + x.Quantity_Diff;
                newRow["在库差异数"] = x.Quantity_Diff;
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public string Get_WMS_Stock_All_To_Excel(Guid LinkMainCID)
        {
            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == LinkMainCID).ToList();
            var Group = from x in Stock_List
                        group x by x.MatSn into g
                        select new
                        {
                            MatSn = g.Key,
                            MatBrand = g.Select(c => c.MatBrand).FirstOrDefault(),
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = x.MatSn;
                Stock_Group.MatBrand = x.MatBrand;
                Group_List.Add(Stock_Group);
            }

            //已占用库存
            List<WMS_Stock_Temp> Stock_Temp_List = db.WMS_Stock_Temp.Where(x => x.LinkMainCID == LinkMainCID).ToList();

            foreach (var x in Group_List)
            {
                x.Quantity_Occupied = Stock_Temp_List.Where(c => c.MatSn == x.MatSn).Sum(c => c.Quantity);
                x.Quantity_Avaliable = Stock_List.Where(c => c.MatSn == x.MatSn).Sum(c => c.Quantity);
                x.Quantity_Sum = x.Quantity_Occupied + x.Quantity_Avaliable;
            }

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();

            TableHeads.Add("品牌");
            TableHeads.Add("产品型号");
            TableHeads.Add("库位数量");
            TableHeads.Add("已配货数");
            TableHeads.Add("总库存数");
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
            foreach (var x in Group_List)
            {
                i++;
                newRow = DT.NewRow();
                newRow["品牌"] = x.MatBrand;
                newRow["产品型号"] = x.MatSn;
                newRow["库位数量"] = x.Quantity_Avaliable;
                newRow["已配货数"] = x.Quantity_Occupied;
                newRow["总库存数"] = x.Quantity_Sum;
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public List<WMS_Stock_Group> Get_WMS_Stock_List(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn == MF.MatSn).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location == MF.Location).AsQueryable();
            }

            if (MF.Link_HeadID != Guid.Empty)
            {
                query = query.Where(x => x.Wms_In_Head_ID == MF.Link_HeadID).AsQueryable();
            }

            List<WMS_Stock> Stock_List = query.ToList();

            var Group = from x in Stock_List
                        group x by new { x.Location, x.Package, x.Price } into g
                        select new
                        {
                            Location = g.Key.Location,
                            Package = g.Key.Package,
                            Price = g.Key.Price,
                            Line_Count = g.Count(),
                            Quantity_Sum = g.Sum(c => c.Quantity),
                            In_DT = g.FirstOrDefault().WMS_In_DT,
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = MF.MatSn;
                Stock_Group.Location = x.Location;
                if (x.Package == WMS_Stock_Package_Enum.整箱.ToString())
                {
                    Stock_Group.Line_Count = x.Line_Count;
                }
                else
                {
                    Stock_Group.Line_Count = 0;
                }
                Stock_Group.Quantity_Sum = x.Quantity_Sum;
                Stock_Group.In_DT = x.In_DT;
                Stock_Group.Price = x.Price;
                Group_List.Add(Stock_Group);
            }

            return Group_List;
        }

        //历史追溯
        public List<WMS_Stock_Record_Info> Get_WMS_Stock_Record_Info_List(WMS_Stock_Filter MF)
        {
            WMS_Stock_Record_Info Record = new WMS_Stock_Record_Info();
            List<WMS_Stock_Record_Info> List = new List<WMS_Stock_Record_Info>();
            List<WMS_Stock_Record_Info> List_Sub = new List<WMS_Stock_Record_Info>();

            //盘库盈亏记录
            List<WMS_Profit_Loss> Profit_Loss_List = db.WMS_Profit_Loss.Where(x => x.LinkMainCID == MF.LinkMainCID && x.MatSn == MF.MatSn && x.Status == WMS_Profit_Loss_Status_Enum.已确定.ToString()).OrderBy(x => x.Create_DT).ToList();
            foreach (var x in Profit_Loss_List)
            {
                x.Diff_Quantity = x.New_Quantity - x.Old_Quantity;

                Record = new WMS_Stock_Record_Info();
                Record.MatSn = x.MatSn;
                Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                Record.Record_Type = "盈亏记录";
                Record.Quantity = x.Diff_Quantity;
                List.Add(Record);
            }

            //出库入库记录
            List<WMS_Stock_Record> Record_List = db.WMS_Stock_Record.Where(x => x.LinkMainCID == MF.LinkMainCID && x.MatSn == MF.MatSn).ToList();
            foreach (var x in Record_List)
            {
                Record = new WMS_Stock_Record_Info();
                Record.MatSn = x.MatSn;
                if (x.Remark == WMS_Stock_Record_Remark_Enum.订单入库.ToString())
                {
                    Record.Record_Type = WMS_Stock_Record_Remark_Enum.订单入库.ToString();
                    Record.Quantity = x.Quantity;
                    Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                }
                else if (x.Remark == WMS_Stock_Record_Remark_Enum.订单出库.ToString())
                {
                    Record.Record_Type = WMS_Stock_Record_Remark_Enum.订单出库.ToString();
                    Record.Quantity = x.Quantity;
                    Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                }
                else if (x.Remark == WMS_Stock_Record_Remark_Enum.首次盘库.ToString())
                {
                    Record.Record_Type = WMS_Stock_Record_Remark_Enum.首次盘库.ToString();
                    Record.Quantity = x.Quantity;
                    Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                }
                else if (x.Remark == WMS_Stock_Record_Remark_Enum.报废出库.ToString())
                {
                    Record.Record_Type = WMS_Stock_Record_Remark_Enum.报废出库.ToString();
                    Record.Quantity = x.Quantity;
                    Record.Act_Date_Str = x.Create_DT.ToString("yyyy-MM-dd");
                }

                List.Add(Record);
            }

            List<string> DT_List = List.Select(x => x.Act_Date_Str).Distinct().ToList();

            WMS_Stock_Record_Info Record_Final = new WMS_Stock_Record_Info();
            List<WMS_Stock_Record_Info> List_Final = new List<WMS_Stock_Record_Info>();
            foreach (var DT in DT_List)
            {
                List_Sub = List.Where(c => c.Act_Date_Str == DT).ToList();
                Record_Final = new WMS_Stock_Record_Info();
                Record_Final.MatSn = List.FirstOrDefault().MatSn;
                Record_Final.Act_Date_Str = DT;
                Record_Final.First_Quantity = List_Sub.Where(c => c.Record_Type == WMS_Stock_Record_Remark_Enum.首次盘库.ToString()).Sum(c => c.Quantity);
                Record_Final.In_Quantity = List_Sub.Where(c => c.Record_Type == WMS_Stock_Record_Remark_Enum.订单入库.ToString()).Sum(c => c.Quantity);
                Record_Final.Out_Quantity = List_Sub.Where(c => c.Record_Type == WMS_Stock_Record_Remark_Enum.订单出库.ToString()).Sum(c => c.Quantity);
                Record_Final.Waste_Out_Quantity = List_Sub.Where(c => c.Record_Type == WMS_Stock_Record_Remark_Enum.报废出库.ToString()).Sum(c => c.Quantity);
                Record_Final.Profit_Loss_Quantity = List_Sub.Where(c => c.Record_Type == "盈亏记录").Sum(c => c.Quantity);
                List_Final.Add(Record_Final);
            }
            return List_Final;
        }

        //库存总数
        public int Get_Total_WMS_Stock_Quantity(Guid LinkMainCID)
        {
            int Total = db.WMS_Stock.Where(x => x.LinkMainCID == LinkMainCID).Sum(x => x.Quantity) + db.WMS_Stock_Temp.Where(x => x.LinkMainCID == LinkMainCID).Sum(x => x.Quantity);
            return Total;
        }

        //按库位
        public PageList<WMS_Stock_Group_Location> Get_WMS_Stock_By_Location_List(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location_Type))
            {
                query = query.Where(x => x.Location_Type == MF.Location_Type).AsQueryable();
            }

            List<WMS_Stock> Stock_List = query.ToList();

            var Group = from x in Stock_List
                        group x by x.Location into g
                        select new
                        {
                            Location = g.Key,
                        };

            List<WMS_Stock_Group_Location> List = new List<WMS_Stock_Group_Location>();
            WMS_Stock_Group_Location T = new WMS_Stock_Group_Location();
            foreach (var x in Group)
            {
                T = new WMS_Stock_Group_Location();
                T.Location = x.Location;
                T.Stock_List = Stock_List.Where(c => c.Location == x.Location).ToList();
                List.Add(T);
            }

            List = List.OrderBy(x => x.Location).ToList();

            PageList<WMS_Stock_Group_Location> PList = new PageList<WMS_Stock_Group_Location>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = List.Count();
            PList.Rows = List.Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public List<WMS_Stock_Group> Get_WMS_Stock_Group_List_For_WMS_Up(Guid Move_ID)
        {
            WMS_Move Move = db.WMS_Move.Find(Move_ID);
            if (Move == null) { throw new Exception("上架任务不存在"); }

            var query = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).AsQueryable();

            List<WMS_Stock> Stock_List = query.ToList();
            var Group = from x in Stock_List
                        group x by new { x.MatSn, x.Package } into g
                        select new
                        {
                            MatSn = g.Key.MatSn,
                            Package = g.Key.Package,
                            Line_Count = g.Count(),
                            Quantity_Sum = g.Sum(c => c.Quantity),
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = x.MatSn;
                if (x.Package == WMS_Stock_Package_Enum.整箱.ToString())
                {
                    Stock_Group.Line_Count = x.Line_Count;
                }
                else
                {
                    Stock_Group.Line_Count = 0;
                }
                Stock_Group.Quantity_Sum = x.Quantity_Sum;
                Group_List.Add(Stock_Group);
            }

            return Group_List;
        }

        public List<WMS_Stock_Group> Get_WMS_Stock_Group_List_For_WMS_Move(Guid Move_ID)
        {
            WMS_Move Move = db.WMS_Move.Find(Move_ID);
            if (Move == null) { throw new Exception("移库任务不存在"); }

            var query = db.WMS_Stock.Where(x => x.LinkMainCID == Move.LinkMainCID && x.Location == Move.Out_Location).AsQueryable();

            List<WMS_Stock> Stock_List = query.ToList();
            var Group = from x in Stock_List
                        group x by new { x.MatSn, x.Package } into g
                        select new
                        {
                            MatSn = g.Key.MatSn,
                            Package = g.Key.Package,
                            Line_Count = g.Count(),
                            Quantity_Sum = g.Sum(c => c.Quantity),
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = x.MatSn;
                if (x.Package == WMS_Stock_Package_Enum.整箱.ToString())
                {
                    Stock_Group.Line_Count = x.Line_Count;
                }
                else
                {
                    Stock_Group.Line_Count = 0;
                }
                Stock_Group.Quantity_Sum = x.Quantity_Sum;
                Group_List.Add(Stock_Group);
            }

            return Group_List;
        }

        public List<WMS_Stock_Group> Get_WMS_Stock_Group_List(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Location == MF.Location).AsQueryable();

            if (MF.Link_HeadID != Guid.Empty)
            {
                query = query.Where(x => x.Wms_In_Head_ID == MF.Link_HeadID).AsQueryable();
            }

            List<WMS_Stock> Stock_List = query.ToList();
            var Group = from x in Stock_List
                        group x by new { x.MatSn, x.Package, x.Price } into g
                        select new
                        {
                            MatSn = g.Key.MatSn,
                            Package = g.Key.Package,
                            Price = g.Key.Price,
                            Line_Count = g.Count(),
                            Quantity_Sum = g.Sum(c => c.Quantity),
                            In_DT = g.FirstOrDefault().WMS_In_DT,
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = x.MatSn;
                if (x.Package == WMS_Stock_Package_Enum.整箱.ToString())
                {
                    Stock_Group.Line_Count = x.Line_Count;
                }
                else
                {
                    Stock_Group.Line_Count = 0;
                }
                Stock_Group.Quantity_Sum = x.Quantity_Sum;
                Stock_Group.In_DT = x.In_DT;
                Stock_Group.Price = x.Price;
                Group_List.Add(Stock_Group);
            }

            return Group_List;
        }

        public List<WMS_Stock_Group> Get_WMS_Stock_Group_List_By_Case(WMS_Stock_Filter MF)
        {
            if (MF.LinkMainCID == Guid.Empty) { throw new Exception("LinkMainCID is null!"); }

            if (string.IsNullOrEmpty(MF.Location)) { throw new Exception("无库位传入！"); }

            if (string.IsNullOrEmpty(MF.Case)) { throw new Exception("无箱号传入！"); }

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Location == MF.Location && x.Cases == MF.Case).ToList();
            var Group = from x in Stock_List
                        group x by x.MatSn into g
                        select new
                        {
                            MatSn = g.Key,
                            Quantity_Sum = g.Sum(c => c.Quantity),
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = x.MatSn;
                Stock_Group.Quantity_Sum = x.Quantity_Sum;
                Group_List.Add(Stock_Group);
            }

            return Group_List;
        }

        public WMS_Stock Get_WMS_Stock_Item(Guid Stock_ID)
        {
            WMS_Stock Stock = db.WMS_Stock.Find(Stock_ID);
            Stock = Stock == null ? new WMS_Stock() : Stock;
            Stock.QRCode_Path = QRCode.CreateQRCode(Stock.MatSn, Stock.Stock_ID);
            return Stock;
        }

        public PageList<WMS_Stock_Record> Get_WMS_Stock_Record_PageList(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock_Record.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Customer))
            {
                query = query.Where(x => x.Customer.Contains(MF.Customer)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Supplier))
            {
                query = query.Where(x => x.Supplier.Contains(MF.Supplier)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location_Type))
            {
                query = query.Where(x => x.Location_Type == MF.Location_Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Person))
            {
                query = query.Where(x => x.Work_Person.Contains(MF.Work_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Remark))
            {
                query = query.Where(x => x.Remark == MF.Remark).AsQueryable();
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

            PageList<WMS_Stock_Record> PList = new PageList<WMS_Stock_Record>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public PageList<WMS_Stock_Group_Location> Get_WMS_Stock_By_Location_List_For_Move(WMS_Stock_Filter MF)
        {
            List<WMS_Location> Loc_List_DB = db.WMS_Location.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Type == Type_Enum.端数.ToString()).ToList();

            var query = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
            }

            List<WMS_Stock> Stock_List_DB = query.ToList();
            List<string> Loc_Str_List_DB = Stock_List_DB.Select(x => x.Location).Distinct().ToList();
            List<string> Loc_Str_List = new List<string>();
            foreach (var Loc in Loc_Str_List_DB)
            {
                if (Loc_List_DB.Where(c => c.Location == Loc).Any())
                {
                    Loc_Str_List.Add(Loc);
                }
            }

            Stock_List_DB = Stock_List_DB.Where(x => Loc_Str_List.Contains(x.Location)).ToList();
            List<string> Mat_List_Str_DB = Stock_List_DB.Select(x => x.MatSn).Distinct().ToList();

            var Mat_List_Query = db.Material.Where(x => x.LinkMainCID == MF.LinkMainCID).Select(x => new { x.MatSn, x.Pack_Qty }).ToList();
            List<Material> Mat_List = Mat_List_Query.Select(x => new Material { MatSn = x.MatSn, Pack_Qty = x.Pack_Qty }).ToList();

            Material Mat = new Material();
            List<WMS_Stock> Stock_List = new List<WMS_Stock>();
            List<WMS_Stock> Stock_List_Sub = new List<WMS_Stock>();
            List<string> MatSn_List_Str_DB = new List<string>();
            List<string> Loc_List_Str = new List<string>();

            int Sum = 0;
            foreach (var Loc in Loc_Str_List)
            {
                Stock_List = Stock_List_DB.Where(c => c.Location == Loc).ToList();
                MatSn_List_Str_DB = Stock_List.Select(c => c.MatSn).Distinct().ToList();

                foreach (var MatSn in MatSn_List_Str_DB)
                {
                    Mat = Mat_List.Where(c => c.MatSn == MatSn).FirstOrDefault();
                    if (Mat != null)
                    {
                        Sum = Stock_List.Where(c => c.MatSn == MatSn).Sum(c => c.Quantity);
                        if (Mat.Pack_Qty > 0 && Mat.Pack_Qty < Sum)
                        {
                            Loc_List_Str.Add(Loc);
                            break;
                        }
                    }
                }
            }

            List<WMS_Stock_Group_Location> List = new List<WMS_Stock_Group_Location>();
            WMS_Stock_Group_Location T = new WMS_Stock_Group_Location();
            foreach (var Location in Loc_List_Str.Distinct())
            {
                T = new WMS_Stock_Group_Location();
                T.Location = Location;
                List.Add(T);
            }

            PageList<WMS_Stock_Group_Location> PList = new PageList<WMS_Stock_Group_Location>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = List.Count();
            PList.Rows = List.OrderBy(x => x.Location).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public List<WMS_Stock_Group> Get_WMS_Stock_Group_List_For_Move(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Location == MF.Location).AsQueryable();

            List<WMS_Stock> Stock_List_DB = query.ToList();
            List<string> Mat_List_Str_DB = Stock_List_DB.Select(x => x.MatSn).Distinct().ToList();
            List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == MF.LinkMainCID && Mat_List_Str_DB.Contains(x.MatSn)).ToList();
            Material Mat = new Material();
            List<string> Loc_List_Str_DB = new List<string>();
            List<string> Mat_List_Str = new List<string>();
            int Sum = 0;
            foreach (var MatSn in Mat_List_Str_DB)
            {
                Mat = Mat_List.Where(c => c.MatSn == MatSn).FirstOrDefault();
                if (Mat != null)
                {
                    Sum = Stock_List_DB.Where(c => c.MatSn == MatSn).Sum(c => c.Quantity);
                    if (Mat.Pack_Qty >= 0 && Mat.Pack_Qty < Sum)
                    {
                        Mat_List_Str.Add(MatSn);
                    }
                }
            }

            List<WMS_Stock> Stock_List_Recommend = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID && Mat_List_Str.Contains(x.MatSn)).ToList();

            List<string> Loc_Str_List_DB = db.WMS_Location.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Type == Type_Enum.整箱.ToString()).Select(x => x.Location).Distinct().ToList();

            Stock_List_Recommend = Stock_List_Recommend.Where(x => Loc_Str_List_DB.Contains(x.Location)).ToList();

            var Group = from x in Stock_List_DB.Where(x => Mat_List_Str.Contains(x.MatSn))
                        group x by new { x.MatSn } into g
                        select new
                        {
                            MatSn = g.Key.MatSn,
                            Quantity_Sum = g.Sum(c => c.Quantity),
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = x.MatSn;
                Stock_Group.Quantity_Sum = x.Quantity_Sum;
                Group_List.Add(Stock_Group);
            }

            foreach (var x in Group_List)
            {
                Mat = Mat_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                if (Mat != null)
                {
                    x.Pack_Qty = Mat.Pack_Qty;
                }
                x.Loc_List = Stock_List_Recommend.Where(c => c.MatSn == x.MatSn).Select(c => c.Location).Distinct().ToList();
            }

            return Group_List;
        }

        public void Create_WMS_Move_From_WMS_Move_Recommend(string Location, Guid Link_MainCID)
        {
            Location = Location.Trim();
            if (db.WMS_Location.Where(x => x.LinkMainCID == Link_MainCID && x.Location == Location).Any() == false) { throw new Exception("系统中不存在该库位"); }

            WMS_Move Move = new WMS_Move();
            Move.Move_ID = MyGUID.NewGUID();
            Move.LinkMainCID = Link_MainCID;
            Move.Create_DT = DateTime.Now;
            Move.Out_Location = Location;

            if (db.WMS_Stock.Where(x => x.LinkMainCID == Link_MainCID && x.Location == Move.Out_Location).Any() == false) { throw new Exception("该库位无产品"); }

            if (db.WMS_Move.Where(x => x.Move_ID != Move.Move_ID && x.Out_Location == Move.Out_Location && x.Move_Status == WMS_Move_Status_Enum.待移库.ToString()).Any())
            {
                throw new Exception("该库位已创建待移库任务");
            }

            Move.Move_Status = WMS_Move_Status_Enum.待移库.ToString();
            db.WMS_Move.Add(Move);
            MyDbSave.SaveChange(db);
        }

    }

    //盘库任务
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Location> Get_WMS_Stocktaking_Task_PageList_Notice(WMS_Stock_Filter MF)
        {
            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();
            List<string> Location_Str_List = Stocktaking_List.Select(x => x.Location).Distinct().ToList();

            List<string> Pick_Scan_Loc_List = db.WMS_Out_Pick_Scan.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status == WMS_Out_Scan_Status_Enum.未完成.ToString()).Select(x => x.Scan_Location).Distinct().ToList();

            Location_Str_List.AddRange(Pick_Scan_Loc_List);
            Location_Str_List = Location_Str_List.Distinct().ToList();
            if (!string.IsNullOrEmpty(MF.Location))
            {
                Location_Str_List = Location_Str_List.Where(x => x.Contains(MF.Location)).ToList();
            }

            List<WMS_Location> Location_List_DB = db.WMS_Location.Where(x => x.LinkMainCID == MF.LinkMainCID && Location_Str_List.Contains(x.Location)).ToList();
            List<WMS_Location> Location_List = new List<WMS_Location>();
            foreach (var x in Location_List_DB)
            {
                if (Location_Str_List.Where(c => c == x.Location).Any())
                {
                    Location_List.Add(x);
                }
            }

            if (!string.IsNullOrEmpty(MF.Type))
            {
                Location_List = Location_List.Where(x => x.Type == MF.Type).ToList();
            }

            PageList<WMS_Location> PList = new PageList<WMS_Location>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Location_List.Count();
            PList.Rows = Location_List.Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public PageList<WMS_Stock_Task> Get_WMS_Stock_Task_PageList(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock_Task.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Property))
            {
                query = query.Where(x => x.Property == MF.Property).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Person))
            {
                query = query.Where(x => x.Work_Person.Contains(MF.Work_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Type))
            {
                query = query.Where(x => x.Type == MF.Type).AsQueryable();
            }

            if (Enum.GetNames(typeof(WMS_Stock_Task_Enum)).ToList().Where(x => x == MF.Status).Any())
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
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

            List<WMS_Stock_Task> List = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            PageList<WMS_Stock_Task> PList = new PageList<WMS_Stock_Task>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = List;

            List<string> Location_List = List.Select(x => x.Location).Distinct().ToList();
            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID && Location_List.Contains(x.Location)).ToList();
            List<WMS_Stock> Stock_List_Sub = new List<WMS_Stock>();

            foreach (var x in List)
            {
                Stock_List_Sub = Stock_List.Where(c => c.Location == x.Location).ToList();
                x.MatSn_Count = Stock_List_Sub.Select(c => c.MatSn).Distinct().Count();
                x.Quantity_Sum = Stock_List_Sub.Sum(c => c.Quantity);
            }
            return PList;
        }

        public PageList<WMS_Stock_Task> Get_WMS_Stock_Task_PageList_Pick(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock_Task.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Property == WMS_Stock_Task_Property_Enum.配货动盘.ToString()).AsQueryable();

            if (Enum.GetNames(typeof(WMS_Stock_Task_Enum)).ToList().Where(x => x == MF.Status).Any())
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Person))
            {
                query = query.Where(x => x.Work_Person.Contains(MF.Work_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Type))
            {
                query = query.Where(x => x.Type.Contains(MF.Type)).AsQueryable();
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
            List<string> Location_List = List_DB.Select(x => x.Location).Distinct().ToList();
            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID && Location_List.Contains(x.Location)).ToList();
            List<WMS_Stock> Stock_List_Sub = new List<WMS_Stock>();
            List<WMS_Stock_Task> List = new List<WMS_Stock_Task>();

            foreach (var x in List_DB)
            {
                Stock_List_Sub = Stock_List.Where(c => c.Location == x.Location).ToList();
                if (Stock_List_Sub.Any())
                {
                    x.MatSn_Count = Stock_List_Sub.Select(c => c.MatSn).Distinct().Count();
                    x.Quantity_Sum = Stock_List_Sub.Sum(c => c.Quantity);
                    List.Add(x);
                }
            }

            List<Guid> Remove_ID_List = List.Select(x => x.Task_ID).ToList();
            List<WMS_Stock_Task> Remove_List = List_DB.Where(x => Remove_ID_List.Contains(x.Task_ID) == false).ToList();
            db.WMS_Stock_Task.RemoveRange(Remove_List);
            MyDbSave.SaveChange(db);

            PageList<WMS_Stock_Task> PList = new PageList<WMS_Stock_Task>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = List.Count();
            PList.Rows = List.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            return PList;
        }

        public WMS_Stock_Task Get_WMS_Stock_Task_Item_DB(Guid TaskID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            Task = Task == null ? new WMS_Stock_Task() : Task;
            return Task;
        }

        public List<WMS_Stocktaking_Scan> Get_WMS_Stocktaking_Scan_List(Guid TaskID, string MatSn)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }
            if (string.IsNullOrEmpty(MatSn)) { throw new Exception("产品型号为空"); }
            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID && x.MatSn == MatSn).ToList();
            return Scan_List;
        }

        public WMS_Stock_Task Get_WMS_Stock_Task_Item(Guid TaskID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }

            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Task.Location).ToList();
            List<string> MatSn_List = Stock_List_DB.Select(x => x.MatSn).Distinct().ToList();

            Task.Line_List = new List<WMS_Stock_Task_Line>();

            var Line_Group = from x in Stock_List_DB
                             group x by x.MatSn into G
                             select new
                             {
                                 MatSn = G.Key,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                             };

            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID).ToList();
            List<WMS_Stocktaking_Scan> Scan_List_Sub = new List<WMS_Stocktaking_Scan>();
            WMS_Stock_Task_Line Line = new WMS_Stock_Task_Line();
            int i = 0;
            foreach (var x in Line_Group.ToList())
            {
                i++;
                Scan_List_Sub = Scan_List.Where(c => c.MatSn == x.MatSn).ToList();
                Line = new WMS_Stock_Task_Line();
                Line.Line_No = i;
                Line.MatSn = x.MatSn;
                Line.Quantity_Sum = x.Quantity_Sum;
                Line.Quantity_Scan_Sum = Scan_List_Sub.Sum(c => c.Scan_Quantity);
                if (Line.Quantity_Sum == Line.Quantity_Scan_Sum)
                {
                    Line.Status = WMS_Stock_Task_Line_State_Enum.数量一致.ToString();
                }
                else if (Line.Quantity_Sum > Line.Quantity_Scan_Sum && Line.Quantity_Scan_Sum > 0)
                {
                    Line.Status = WMS_Stock_Task_Line_State_Enum.低于系统.ToString();
                }
                else if (Line.Quantity_Sum < Line.Quantity_Scan_Sum && Line.Quantity_Scan_Sum > 0)
                {
                    Line.Status = WMS_Stock_Task_Line_State_Enum.超出系统.ToString();
                }
                else
                {
                    Line.Status = WMS_Stock_Task_Line_State_Enum.还未扫码.ToString();
                }
                Task.Line_List.Add(Line);
            }

            //获取未匹配扫码信息
            List<string> MatSn_Line_ALL = Line_Group.Select(x => x.MatSn).ToList();
            List<WMS_Stocktaking_Scan> List_Scan_Other = Scan_List.Where(x => MatSn_Line_ALL.Contains(x.MatSn) == false).ToList();
            foreach (var MatSn in List_Scan_Other.Select(x => x.MatSn).Distinct().ToList())
            {
                i++;
                Scan_List_Sub = Scan_List.Where(c => c.MatSn == MatSn).ToList();
                Line = new WMS_Stock_Task_Line();
                Line.Line_No = i;
                Line.MatSn = MatSn;
                Line.Quantity_Sum = 0;
                Line.Quantity_Scan_Sum = Scan_List_Sub.Sum(c => c.Scan_Quantity);
                Line.Status = WMS_Stock_Task_Line_State_Enum.多出型号.ToString();
                Task.Line_List.Add(Line);
            }

            Task.Scan_List = Scan_List.OrderByDescending(x => x.Create_DT).ToList();
            return Task;
        }

        public WMS_Stock_Task Get_WMS_Stock_Task_Item_Other(Guid TaskID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }

            List<WMS_Stocktaking> List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Task.Task_ID).ToList();
            List<string> MatSn_List = List.Select(x => x.MatSn).Distinct().ToList();

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Task.Location).ToList();
            if (Task.Type == Type_Enum.端数.ToString())
            {
                Stock_List = Stock_List.Where(x => MatSn_List.Contains(x.MatSn)).ToList();
            }

            Task.Line_List = new List<WMS_Stock_Task_Line>();

            var Line_Group = from x in Stock_List
                             group x by x.MatSn into G
                             select new
                             {
                                 MatSn = G.Key,
                                 Quantity_Sum = G.Sum(c => c.Quantity),
                             };

            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID).ToList();
            List<WMS_Stocktaking_Scan> Scan_List_Sub = new List<WMS_Stocktaking_Scan>();
            WMS_Stock_Task_Line Line = new WMS_Stock_Task_Line();
            int i = 0;
            foreach (var x in Line_Group.ToList())
            {
                i++;
                Scan_List_Sub = Scan_List.Where(c => c.MatSn == x.MatSn).ToList();
                Line = new WMS_Stock_Task_Line();
                Line.Line_No = i;
                Line.MatSn = x.MatSn;
                Line.Quantity_Sum = x.Quantity_Sum;
                Line.Quantity_Scan_Sum = Scan_List_Sub.Sum(c => c.Scan_Quantity);
                if (Line.Quantity_Sum == Line.Quantity_Scan_Sum)
                {
                    Line.Status = WMS_Stock_Task_Line_State_Enum.数量一致.ToString();
                }
                else if (Line.Quantity_Sum > Line.Quantity_Scan_Sum && Line.Quantity_Scan_Sum > 0)
                {
                    Line.Status = WMS_Stock_Task_Line_State_Enum.低于系统.ToString();
                }
                else if (Line.Quantity_Sum < Line.Quantity_Scan_Sum && Line.Quantity_Scan_Sum > 0)
                {
                    Line.Status = WMS_Stock_Task_Line_State_Enum.超出系统.ToString();
                }
                else
                {
                    Line.Status = WMS_Stock_Task_Line_State_Enum.还未扫码.ToString();
                }
                Task.Line_List.Add(Line);
            }

            Task.Scan_List = Scan_List.OrderByDescending(x => x.Create_DT).ToList();
            return Task;
        }

        public void Set_WMS_Stock_Task_Work_Person(WMS_Stock_Task Task)
        {
            if (string.IsNullOrEmpty(Task.Work_Person)) { throw new Exception("未填写作业人！"); }
            WMS_Stock_Task Task_DB = db.WMS_Stock_Task.Find(Task.Task_ID);
            if (Task_DB == null) { throw new Exception("WMS_Stock_Task is null"); }
            Task_DB.Work_Person = Task.Work_Person.Trim();
            db.Entry(Task_DB).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Batch_Create_WMS_Stocktaking_With_Work_Person(List<Guid> Task_IDList, List<string> Work_Person_List)
        {
            if (Work_Person_List.Count() <= 0) { throw new Exception("请填写作业人！"); }

            List<WMS_Stock_Task> List = db.WMS_Stock_Task.Where(x => Task_IDList.Contains(x.Task_ID)).ToList();

            if (List.Where(x => x.Status == WMS_Stock_Task_Enum.已盘库.ToString()).Any())
            {
                throw new Exception("存在已盘库任务，不支持派工!");
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

        public void Reset_WMS_Stocktaking_Task_Scan_By_MatSn(Guid TaskID, string MatSn)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }
            if (string.IsNullOrEmpty(MatSn)) { throw new Exception("产品型号为空"); }
            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID && x.MatSn == MatSn).ToList();

            if (Task.Status == WMS_Stock_Task_Enum.已盘库.ToString())
            {
                throw new Exception("产品已完成盘库，不支持重置扫描");
            }

            if (db.WMS_Profit_Loss_Head.Where(x => x.Link_HeadID == Task.Task_ID && x.Status != WMS_Profit_Loss_Head_Status_Enum.已退回.ToString()).Any())
            {
                throw new Exception("此盘库任务存在盈亏审核单正在审核，不支持重置扫描！");
            }

            if (Scan_List.Where(x => x.Status == WMS_Stocktaking_Status_Enum.已底盘.ToString()).Any())
            {
                throw new Exception("产品已完成盘库，不支持重置扫描");
            }

            if (db.WMS_Profit_Loss.Where(x => x.Link_TaskID == Task.Task_ID && x.MatSn == MatSn && x.Status == WMS_Profit_Loss_Status_Enum.未确定.ToString()).Any())
            {
                throw new Exception("已生成盈亏记录，不支持删除");
            }

            db.WMS_Stocktaking_Scan.RemoveRange(Scan_List);
            MyDbSave.SaveChange(db);
        }

        public void Reset_WMS_Stocktaking_Task_Scan(Guid TaskID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }
            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID).ToList();

            if (Task.Status == WMS_Stock_Task_Enum.已盘库.ToString())
            {
                throw new Exception("产品已完成盘库，不支持重置扫描");
            }

            if (db.WMS_Profit_Loss_Head.Where(x => x.Link_HeadID == Task.Task_ID && x.Status != WMS_Profit_Loss_Head_Status_Enum.已退回.ToString()).Any())
            {
                throw new Exception("此盘库任务存在盈亏审核单正在审核，不支持重置扫描！");
            }

            if (Scan_List.Where(x => x.Status == WMS_Stocktaking_Status_Enum.已底盘.ToString()).Any())
            {
                throw new Exception("产品已完成盘库，不支持重置扫描");
            }

            if (db.WMS_Profit_Loss.Where(x => x.Link_TaskID == Task.Task_ID && x.Status == WMS_Profit_Loss_Status_Enum.未确定.ToString()).Any())
            {
                throw new Exception("已生成盈亏记录，不支持重置扫描");
            }

            db.WMS_Stocktaking_Scan.RemoveRange(Scan_List);
            MyDbSave.SaveChange(db);
        }

        public void Finish_WMS_Stocktaking_Task(Guid TaskID, User U)
        {
            WMS_Stock_Task Task = Get_WMS_Stock_Task_Item(TaskID);

            if (Task.Status == WMS_Stock_Task_Enum.已盘库.ToString())
            {
                throw new Exception("该库位已完成盘库");
            }

            List<string> MatSn_List = Task.Line_List.Select(x => x.MatSn).Distinct().ToList();

            //系统库存
            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Task.Location && MatSn_List.Contains(x.MatSn)).ToList();
            WMS_Stock Stock_DB = new WMS_Stock();

            List<WMS_Stocktaking> List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Task.Task_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();

            //盘库扫描
            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();

            //库存盈亏审核单创建
            WMS_Profit_Loss_Line Line = new WMS_Profit_Loss_Line();
            List<WMS_Profit_Loss_Line> Line_List = new List<WMS_Profit_Loss_Line>();
            
            //低于系统数量
            foreach (var x in Task.Line_List.Where(x => x.Status == WMS_Stock_Task_Line_State_Enum.低于系统.ToString()).ToList())
            {
                Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_DB = Stock_DB == null ? new WMS_Stock() : Stock_DB;

                Line = new WMS_Profit_Loss_Line();
                Line.Line_ID = MyGUID.NewGUID();
                Line.Old_Quantity = x.Quantity_Sum;
                Line.MatSn = x.MatSn;
                Line.New_Quantity = x.Quantity_Scan_Sum;
                Line.MatBrand = Stock_DB.MatBrand;
                Line.MatName = Stock_DB.MatName;
                Line.MatUnit = Stock_DB.MatUnit;
                Line.Unit_Price = Stock_DB.Price;
                Line_List.Add(Line);
            }

            //还未扫码
            foreach (var x in Task.Line_List.Where(x => x.Status == WMS_Stock_Task_Line_State_Enum.还未扫码.ToString()).ToList())
            {
                Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_DB = Stock_DB == null ? new WMS_Stock() : Stock_DB;

                Line = new WMS_Profit_Loss_Line();
                Line.Line_ID = MyGUID.NewGUID();
                Line.Old_Quantity = x.Quantity_Sum;
                Line.MatSn = x.MatSn;
                Line.New_Quantity = 0;
                Line.MatBrand = Stock_DB.MatBrand;
                Line.MatName = Stock_DB.MatName;
                Line.MatUnit = Stock_DB.MatUnit;
                Line.Unit_Price = Stock_DB.Price;
                Line_List.Add(Line);
            }

            //多出型号
            List<string> MatSn_Scan = Task.Line_List.Where(x => x.Status == WMS_Stock_Task_Line_State_Enum.多出型号.ToString()).Select(x => x.MatSn).Distinct().ToList();
            List<WMS_Stock> Stock_DB_Price_List = db.WMS_Stock.Where(x => x.LinkMainCID == Task.LinkMainCID && MatSn_Scan.Contains(x.MatSn)).ToList();
            WMS_Stock Stock_DB_Price = new WMS_Stock();

            foreach (var x in Task.Line_List.Where(x => x.Status == WMS_Stock_Task_Line_State_Enum.多出型号.ToString()).ToList())
            {
                Stock_DB_Price = Stock_DB_Price_List.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_DB_Price = Stock_DB_Price == null ? new WMS_Stock() : Stock_DB_Price;

                Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_DB = Stock_DB == null ? new WMS_Stock() : Stock_DB;

                Line = new WMS_Profit_Loss_Line();
                Line.Line_ID = MyGUID.NewGUID();
                Line.Old_Quantity = 0;
                Line.New_Quantity = x.Quantity_Scan_Sum;
                Line.MatSn = x.MatSn;
                Line.MatBrand = Stock_DB.MatBrand;
                Line.MatName = Stock_DB.MatName;
                Line.MatUnit = Stock_DB.MatUnit;
                Line.Unit_Price = Stock_DB_Price.Price;//不是很准确
                Line_List.Add(Line);
            }

            //超出系统数量
            foreach (var x in Task.Line_List.Where(x => x.Status == WMS_Stock_Task_Line_State_Enum.超出系统.ToString()).ToList())
            {
                Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_DB = Stock_DB == null ? new WMS_Stock() : Stock_DB;

                Line = new WMS_Profit_Loss_Line();
                Line.Line_ID = MyGUID.NewGUID();
                Line.Old_Quantity = x.Quantity_Sum;
                Line.MatSn = x.MatSn;
                Line.New_Quantity = x.Quantity_Scan_Sum;
                Line.MatBrand = Stock_DB.MatBrand;
                Line.MatName = Stock_DB.MatName;
                Line.MatUnit = Stock_DB.MatUnit;
                Line.Unit_Price = Stock_DB.Price;
                Line_List.Add(Line);
            }

            //是否产生盈亏审核单
            if (Line_List.Any())
            {
                WMS_Profit_Loss_Head Head = new WMS_Profit_Loss_Head();
                Head.Head_ID = MyGUID.NewGUID();
                Head.Task_Bat_No = this.Auto_Create_Task_Bat_WMS_Profit_Loss_Head(U);
                Head.Task_Bat_No_Str = "PL" + Head.Task_Bat_No;
                Head.Create_DT = DateTime.Now;
                Head.Create_Person = U.UserFullName;
                Head.Status = WMS_Profit_Loss_Head_Status_Enum.待审核.ToString();
                Head.LinkMainCID = U.LinkMainCID;
                Head.Link_HeadID = Task.Task_ID;
                Head.Location = Task.Location;

                if (db.WMS_Profit_Loss_Head.Where(x => x.Link_HeadID == Task.Task_ID && x.Status != WMS_Profit_Loss_Head_Status_Enum.已退回.ToString()).Any())
                {
                    throw new Exception("该盘库任务存在盈亏审核单待审核，不支持操作");
                }

                foreach (var x in Line_List)
                {
                    x.Task_Bat_No = Head.Task_Bat_No;
                    x.Task_Bat_No_Str = Head.Task_Bat_No_Str;
                    x.Create_DT = Head.Create_DT;
                    x.Create_Person = Head.Create_Person;
                    x.LinkMainCID = Head.LinkMainCID;
                    x.Link_Head_ID = Head.Head_ID;
                    x.Location = Head.Location;
                }

                db.WMS_Profit_Loss_Head.Add(Head);
                db.WMS_Profit_Loss_Line.AddRange(Line_List);

                ISentEmailService IS = new SentEmailService();
                IS.Sent_To_Manager_With_WMS_Profit_Loss_Head(Head, Line_List);
            }
            else
            {
                Task.Status = WMS_Stock_Task_Enum.已盘库.ToString();
                db.Entry(Task).State = EntityState.Modified;

                //更新底盘状态
                foreach (var x in List)
                {
                    x.Status = WMS_Stocktaking_Status_Enum.已底盘.ToString();
                    db.Entry(x).State = EntityState.Modified;
                }

                //更新底盘扫描状态
                foreach (var x in Scan_List)
                {
                    x.Status = WMS_Stocktaking_Status_Enum.已底盘.ToString();
                    db.Entry(x).State = EntityState.Modified;
                }

                //更新同库位的底盘任务
                //List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString() && x.Location == Task.Location).ToList();
                //if (Stocktaking_List.Any())
                //{
                //    foreach (var x in Stocktaking_List)
                //    {
                //        x.Status = WMS_Stocktaking_Status_Enum.已底盘.ToString();
                //        x.Work_Person = Task.Work_Person;
                //        db.Entry(x).State = EntityState.Modified;
                //    }
                //}

                //更新配货记录状态
                List<WMS_Out_Pick_Scan> Pick_Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Status == WMS_Out_Scan_Status_Enum.未完成.ToString() && x.Scan_Location == Task.Location).ToList();
                if (Pick_Scan_List.Any())
                {
                    foreach (var x in Pick_Scan_List)
                    {
                        x.Status = WMS_Out_Scan_Status_Enum.已完成.ToString();
                        db.Entry(x).State = EntityState.Modified;
                    }
                }

                WMS_Stock Stock = new WMS_Stock();
                List<WMS_Stock> Stock_List = new List<WMS_Stock>();

                foreach (var x in Scan_List.OrderBy(x => x.Create_DT).ToList())
                {
                    Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                    Stock_DB = Stock_DB == null ? new WMS_Stock() : Stock_DB;

                    Stock = new WMS_Stock();
                    Stock.Stock_ID = MyGUID.NewGUID();
                    Stock.Quantity = x.Scan_Quantity;
                    Stock.Package = x.Package_Type;
                    Stock.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                    Stock.Location = Task.Location;
                    Stock.WMS_In_DT = Stock_DB.WMS_In_DT;
                    Stock.MatSn = x.MatSn;
                    Stock.MatName = Stock_DB.MatName;
                    Stock.MatUnit = Stock_DB.MatUnit;
                    Stock.MatBrand = Stock_DB.MatBrand;
                    Stock.Price = Stock_DB.Price;
                    Stock.Wms_In_Head_ID = Stock_DB.Wms_In_Head_ID;
                    Stock.LinkMainCID = Stock_DB.LinkMainCID;
                    Stock_List.Add(Stock);
                }

                db.WMS_Stock.AddRange(Stock_List);
                db.WMS_Stock.RemoveRange(Stock_List_DB);
            }

            MyDbSave.SaveChange(db);
        }

        public void Finish_WMS_Stocktaking_Task_Other(Guid TaskID)
        {
            WMS_Stock_Task Task = Get_WMS_Stock_Task_Item_Other(TaskID);

            if (Task.Status == WMS_Stock_Task_Enum.已盘库.ToString())
            {
                throw new Exception("该库位已完成盘库");
            }

            List<string> MatSn_List = Task.Line_List.Select(x => x.MatSn).Distinct().ToList();

            //系统库存
            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Task.Location && MatSn_List.Contains(x.MatSn)).ToList();
            WMS_Stock Stock_DB = new WMS_Stock();

            List<WMS_Stocktaking> List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Task.Task_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();

            //盘库扫描
            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString()).ToList();

            //库存盈亏创建
            WMS_Profit_Loss_Other PL = new WMS_Profit_Loss_Other();
            List<WMS_Profit_Loss_Other> PL_List = new List<WMS_Profit_Loss_Other>();
            DateTime Create_DT = DateTime.Now;

            WMS_Out_Head Head = db.WMS_Out_Head.Find(Task.Link_HeadID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null"); }

            //低于系统数量
            foreach (var x in Task.Line_List.Where(x => x.Status == WMS_Stock_Task_Line_State_Enum.低于系统.ToString()).ToList())
            {
                Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_DB = Stock_DB == null ? new WMS_Stock() : Stock_DB;

                PL = new WMS_Profit_Loss_Other();
                PL.Line_ID = MyGUID.NewGUID();
                PL.Old_Quantity = x.Quantity_Sum;
                PL.MatSn = x.MatSn;
                PL.New_Quantity = x.Quantity_Scan_Sum;
                PL.Location = Task.Location;
                PL.Create_DT = Create_DT;
                PL.MatBrand = Stock_DB.MatBrand;
                PL.LinkMainCID = Task.LinkMainCID;
                PL.Link_TaskID = Task.Task_ID;
                PL.Price = Stock_DB.Price;
                PL.Work_Person = Head.Work_Down_Person;
                PL.Status = WMS_Profit_Loss_Status_Enum.未确定.ToString();
                PL_List.Add(PL);
            }

            //还未扫码
            foreach (var x in Task.Line_List.Where(x => x.Status == WMS_Stock_Task_Line_State_Enum.还未扫码.ToString()).ToList())
            {
                Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_DB = Stock_DB == null ? new WMS_Stock() : Stock_DB;

                PL = new WMS_Profit_Loss_Other();
                PL.Line_ID = MyGUID.NewGUID();
                PL.Old_Quantity = x.Quantity_Sum;
                PL.MatSn = x.MatSn;
                PL.New_Quantity = 0;
                PL.Location = Task.Location;
                PL.Create_DT = Create_DT;
                PL.MatBrand = Stock_DB.MatBrand;
                PL.LinkMainCID = Task.LinkMainCID;
                PL.Link_TaskID = Task.Task_ID;
                PL.Price = Stock_DB.Price;
                PL.Work_Person = Head.Work_Down_Person;
                PL.Status = WMS_Profit_Loss_Status_Enum.未确定.ToString();
                PL_List.Add(PL);
            }

            //超出系统数量
            foreach (var x in Task.Line_List.Where(x => x.Status == WMS_Stock_Task_Line_State_Enum.超出系统.ToString()).ToList())
            {
                Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Stock_DB = Stock_DB == null ? new WMS_Stock() : Stock_DB;

                PL = new WMS_Profit_Loss_Other();
                PL.Line_ID = MyGUID.NewGUID();
                PL.Old_Quantity = x.Quantity_Sum;
                PL.MatSn = x.MatSn;
                PL.MatBrand = Stock_DB.MatBrand;
                PL.Location = Task.Location;
                PL.Create_DT = Create_DT;
                PL.LinkMainCID = Task.LinkMainCID;
                PL.Link_TaskID = Task.Task_ID;
                PL.New_Quantity = x.Quantity_Scan_Sum;
                PL.Price = Stock_DB.Price;
                PL.Work_Person = Head.Work_Down_Person;
                PL.Status = WMS_Profit_Loss_Status_Enum.未确定.ToString();
                PL_List.Add(PL);
            }

            //是否产生盈亏记录
            if (PL_List.Any())
            {
                List<string> MatSn_List_Temp = PL_List.Select(x => x.MatSn).Distinct().ToList();

                if (db.WMS_Profit_Loss_Other.Where(x => x.Link_TaskID == Task.Task_ID && x.Status == WMS_Profit_Loss_Status_Enum.未确定.ToString() && MatSn_List_Temp.Contains(x.MatSn)).Any())
                {
                    throw new Exception("该动盘任务存在动盘盈亏记录，不支持生成新的动盘盈亏记录");
                }

                db.WMS_Profit_Loss_Other.AddRange(PL_List);
            }

            Task.Status = WMS_Stock_Task_Enum.已盘库.ToString();
            if (Task.Type == Type_Enum.整箱.ToString() && Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).Any())
            {
                Task.Recommend_Status = WMS_Recommend_Status_Enum.未推荐.ToString();
            }

            db.Entry(Task).State = EntityState.Modified;

            //更新底盘状态
            foreach (var x in List)
            {
                x.Status = WMS_Stocktaking_Status_Enum.已底盘.ToString();
                db.Entry(x).State = EntityState.Modified;
            }

            //更新底盘扫描状态
            foreach (var x in Scan_List)
            {
                x.Status = WMS_Stocktaking_Status_Enum.已底盘.ToString();
                db.Entry(x).State = EntityState.Modified;
            }

            //更新配货记录状态
            List<WMS_Out_Pick_Scan> Pick_Scan_List = db.WMS_Out_Pick_Scan.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Status == WMS_Out_Scan_Status_Enum.未完成.ToString() && x.Scan_Location == Task.Location).ToList();
            if (Pick_Scan_List.Any())
            {
                foreach (var x in Pick_Scan_List)
                {
                    x.Status = WMS_Out_Scan_Status_Enum.已完成.ToString();
                    db.Entry(x).State = EntityState.Modified;
                }
            }

            MyDbSave.SaveChange(db);
        }

        public void Delete_WMS_Stocktaking_Task(Guid TaskID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }

            if (Task.Status == WMS_Stock_Task_Enum.已盘库.ToString())
            {
                throw new Exception("产品已完成盘库，不支持重置扫描");
            }

            if (db.WMS_Profit_Loss_Head.Where(x => x.Link_HeadID == Task.Task_ID).Any())
            {
                throw new Exception("此盘库任务存在盈亏审核单，不支持删除！");
            }

            if (db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID).Any())
            {
                throw new Exception("此盘库任务存在扫描，不支持删除！");
            }

            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => x.Link_TaskID == Task.Task_ID).ToList();
            db.WMS_Stocktaking.RemoveRange(Stocktaking_List);

            db.WMS_Stock_Task.Remove(Task);
            MyDbSave.SaveChange(db);
        }

        public string Get_WMS_Stocktaking_To_Excel(Guid LinkMainCID, string Location)
        {
            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == LinkMainCID && x.Location == Location).ToList();
            List<string> MatSn_List = Stock_List_DB.Select(x => x.MatSn).Distinct().ToList();

            List<WMS_Stocktaking> Stocktaking_List = db.WMS_Stocktaking.Where(x => x.LinkMainCID == LinkMainCID && x.Status == WMS_Stocktaking_Status_Enum.待底盘.ToString() && MatSn_List.Contains(x.MatSn)).ToList();

            List<WMS_Stock> Stock_List = new List<WMS_Stock>();
            WMS_Stock Stock = new WMS_Stock();
            foreach (var MatSn in MatSn_List)
            {
                Stock = new WMS_Stock();
                Stock.MatSn = MatSn;
                Stock.Quantity = Stock_List_DB.Where(c => c.MatSn == MatSn).Sum(c => c.Quantity);
                Stock_List.Add(Stock);
            }

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();

            TableHeads.Add("产品型号");
            TableHeads.Add("是否底盘");
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
            foreach (var x in Stock_List.OrderBy(x => x.MatSn).ToList())
            {
                i++;
                newRow = DT.NewRow();
                newRow["产品型号"] = x.MatSn;

                if (Stocktaking_List.Where(c => c.MatSn == x.MatSn).Any())
                {
                    newRow["是否底盘"] = "是";
                    newRow["数量"] = "";
                }
                else
                {
                    newRow["是否底盘"] = "";
                    newRow["数量"] = x.Quantity;
                }


                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public WMS_Stocktaking Get_WMS_Stocktaking_Item(Guid TaskID, string MatSn)
        {
            WMS_Stocktaking Stocktaking = db.WMS_Stocktaking.Where(x => x.Link_TaskID == TaskID && x.MatSn == MatSn).FirstOrDefault();
            return Stocktaking;
        }

        public void Set_WMS_Stocktaking_Task_For_MatSn(Guid TaskID, string MatSn, int Quantity)
        {
            if (Quantity <= 0) { throw new Exception("请输入不小于0的数字"); }

            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("任务不存在"); }

            if (string.IsNullOrEmpty(Task.Work_Person)) { throw new Exception("未设置作业人"); }

            WMS_Stocktaking_Scan Scan = new WMS_Stocktaking_Scan();
            Scan.Scan_ID = MyGUID.NewGUID();
            Scan.LinkMainCID = Task.LinkMainCID;
            Scan.Link_TaskID = Task.Task_ID;
            Scan.Create_DT = DateTime.Now;
            Scan.Location = Task.Location;
            Scan.Scan_Source = MatSn.Trim();
            Scan.MatSn = MatSn.Trim();
            Scan.Scan_Quantity = Quantity;
            Scan.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
            Scan.Package_Type = WMS_Stock_Package_Enum.零头.ToString();
            Scan.Scan_Quantity = Quantity;

            if (db.WMS_Location.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Scan.Location).Any() == false) { throw new Exception("系统中不存在该库位"); }

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == Scan.LinkMainCID && x.Location == Scan.Location).ToList();
            if (Stock_List.Any() == false) { throw new Exception("该库位不存在产品"); }

            db.WMS_Stocktaking_Scan.Add(Scan);
            MyDbSave.SaveChange(db);
        }

        //临时批量生成首次盘库记录（按库位）
        public void Batch_Create_WMS_Stock_Record_By_Location(HttpPostedFileBase ExcelFile, string Location, User U)
        {
            Location = Location.Trim();
            if (string.IsNullOrEmpty(Location)) { throw new Exception("未填写库位"); }

            WMS_Location Loc = db.WMS_Location.Where(x => x.LinkMainCID == U.LinkMainCID && x.Location == Location).FirstOrDefault();

            if (Loc == null)
            {
                throw new Exception("系统中不存在此库位");
            }

            if (db.WMS_Stock_Record.Where(x => x.Wms_In_Head_ID == Loc.Loc_ID).Any())
            {
                throw new Exception("该库位已导入记录");
            }

            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "WMS_Stock_Record/" + Location);

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);

            List<WMS_Stock_Record> Line_List = new List<WMS_Stock_Record>();
            WMS_Stock_Record Line = new WMS_Stock_Record();

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            DateTime DT = DateTime.Now;
            int Line_No = 0;
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                Line_No++;
                Line = new WMS_Stock_Record();
                Line.Stock_ID = MyGUID.NewGUID();
                Line.MatUnit = "PCS";
                Line.Price = 0;
                Line.LinkMainCID = U.LinkMainCID;
                Line.Location = Loc.Location;
                Line.Create_DT = DT;
                Line.Wms_In_Head_ID = Loc.Loc_ID;
                Line.Remark = WMS_Stock_Record_Remark_Enum.首次盘库.ToString();
                Line.Package = WMS_Stock_Package_Enum.零头.ToString();
                Line.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();

                try { Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }

                try { Line.Quantity = Convert.ToInt32(row.GetCell(1).ToString().Trim()); } catch { Line.Quantity = 0; }

                //过滤换行符
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

                //判断单元格是否有公式
                for (int j = 0; j < 3; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                //判断本次导入是否有重复型号
                if (!string.IsNullOrEmpty(Line.MatSn) && Line_List.Where(c => c.MatSn == Line.MatSn).Any())
                {
                    throw new Exception("导入数据存在重复");
                }

                Line_List.Add(Line);
            }

            if (Line_No > 10000)
            {
                throw new Exception("导入产品数据已超过1000条");
            }

            if (Line_List.Any())
            {
                db.WMS_Stock_Record.AddRange(Line_List);
                MyDbSave.SaveChange(db);
            }
        }

        //临时批量生成首次盘库记录
        public void Batch_Create_WMS_Stock_Record(HttpPostedFileBase ExcelFile, User U)
        {
            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "WMS_Stock_Record/");

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);

            List<WMS_Stock_Record> Line_List = new List<WMS_Stock_Record>();
            WMS_Stock_Record Line = new WMS_Stock_Record();

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            DateTime DT = DateTime.Now;
            int Line_No = 0;
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                Line_No++;
                Line = new WMS_Stock_Record();
                Line.Stock_ID = MyGUID.NewGUID();
                Line.MatUnit = "PCS";
                Line.Price = 0;
                Line.LinkMainCID = U.LinkMainCID;
                Line.Create_DT = DT;
                Line.Remark = WMS_Stock_Record_Remark_Enum.首次盘库.ToString();
                Line.Package = WMS_Stock_Package_Enum.零头.ToString();
                Line.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();

                try { Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }

                try { Line.Quantity = Convert.ToInt32(row.GetCell(1).ToString().Trim()); } catch { Line.Quantity = 0; }
                try { Line.Location = row.GetCell(2).ToString().Trim(); } catch { Line.Location = string.Empty; }

                //过滤换行符
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);
                Line.Location = Line.Location.Replace(Environment.NewLine, "");

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

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

                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(Line.Location))
                {
                    throw new Exception(Line.MatSn + "的库位为空");
                }

                if (!string.IsNullOrEmpty(Line.MatSn) && Line.Quantity <= 0)
                {
                    throw new Exception(Line.Location + "中的" + Line.MatSn + "的数量格式不正确");
                }

                //判断同一库位是否有重复型号
                if (!string.IsNullOrEmpty(Line.MatSn) && Line_List.Where(c => c.MatSn == Line.MatSn && c.Location == Line.Location).Any())
                {
                    throw new Exception(Line.MatSn + "在库位" + Line.Location + "中存在重复型号");
                }
                Line_List.Add(Line);
            }

            if (Line_No > 10000)
            {
                throw new Exception("导入产品数据已超过1000条");
            }

            List<string> Location_Str_List = Line_List.Select(x => x.Location).Distinct().ToList();

            //判断库存中是否已存在产品信息
            List<WMS_Stock_Record> Record_List = db.WMS_Stock_Record.Where(x => x.LinkMainCID == U.LinkMainCID && x.Create_DT == DT).ToList();
            Record_List = Record_List.Where(x => Location_Str_List.Contains(x.Location)).ToList();
            foreach (var Location in Location_Str_List)
            {
                if (Record_List.Where(x => x.Location == Location).Any())
                {
                    throw new Exception("库位" + Location + "中已存在产品信息");
                }
            }

            //判断库位是否与系统中库存一致
            List<WMS_Location> Location_List = db.WMS_Location.Where(x => x.LinkMainCID == U.LinkMainCID && Location_Str_List.Contains(x.Location)).ToList();

            WMS_Location Loc = new WMS_Location();
            foreach (var x in Line_List)
            {
                Loc = Location_List.Where(c => c.Location == x.Location).FirstOrDefault();
                if (Loc == null) { throw new Exception("系统中不存在库位" + x.Location); }
                x.Wms_In_Head_ID = Loc.Loc_ID;
            }

            //与系统中的型号进行比对
            List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();
            List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == U.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
            if (Mat_List.Count() != MatSn_List.Count())
            {
                throw new Exception("Excel中存在与系统中不同的产品型号！");
            }

            if (Line_List.Any())
            {
                db.WMS_Stock_Record.AddRange(Line_List);
                MyDbSave.SaveChange(db);
            }
        }

        public PageList<WMS_Stock_Record> Get_WMS_Stock_Record_PageList_Temp(WMS_Stock_Filter MF)
        {
            string WMS_In_DT = DateTime.Now.ToString("2018-09-30 00:00:00");

            var query = db.WMS_Stock_Record.Where(x => x.LinkMainCID == MF.LinkMainCID && x.WMS_In_DT == WMS_In_DT && x.Remark == WMS_Stock_Record_Remark_Enum.首次盘库.ToString()).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
            }

            List<WMS_Stock_Record> List = query.ToList();

            PageList<WMS_Stock_Record> PList = new PageList<WMS_Stock_Record>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = List.Count();
            PList.Rows = List.OrderByDescending(x => x.Location).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

    }

    //盈亏记录
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Profit_Loss> Get_WMS_Profit_Loss_PageList(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Profit_Loss.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Person))
            {
                query = query.Where(x => x.Work_Person.Contains(MF.Work_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status.Contains(MF.Status)).AsQueryable();
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

            PageList<WMS_Profit_Loss> PList = new PageList<WMS_Profit_Loss>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            return PList;
        }
        
        public PageList<WMS_Profit_Loss_Other> Get_WMS_Profit_Loss_Other_PageList(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Profit_Loss_Other.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Work_Person))
            {
                query = query.Where(x => x.Work_Person.Contains(MF.Work_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status.Contains(MF.Status)).AsQueryable();
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

            PageList<WMS_Profit_Loss_Other> PList = new PageList<WMS_Profit_Loss_Other>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            return PList;
        }

        public void Confirm_WMS_Profit_Loss_Other_Item(Guid Line_ID, User U)
        {
            WMS_Profit_Loss_Other PL = db.WMS_Profit_Loss_Other.Find(Line_ID);
            if (PL == null) { throw new Exception("WMS_Profit_Loss_Other is null"); }

            if (PL.Status == WMS_Profit_Loss_Status_Enum.已确定.ToString())
            {
                throw new Exception("该盈亏记录已确认生成，请勿重复操作");
            }

            if (db.WMS_Stock_Task.Where(x => x.LinkMainCID == PL.LinkMainCID && x.Status == WMS_Stock_Task_Enum.未盘库.ToString() && x.Location == PL.Location).Any() == false)
            {
                Create_WMS_Stock_Task_Item(PL);
            }

            PL.Status = WMS_Profit_Loss_Status_Enum.已确定.ToString();
            db.Entry(PL).State = EntityState.Modified;
            MyDbSave.SaveChange(db);

        }

        //创建盘库任务
        private void Create_WMS_Stock_Task_Item(WMS_Profit_Loss_Other PL)
        {
            WMS_Location Loc = db.WMS_Location.Where(x => x.LinkMainCID == PL.LinkMainCID && x.Location == PL.Location).FirstOrDefault();
            if (Loc == null) { throw new Exception("系统中不存在该库位"); }

            WMS_Stock_Task Task = new WMS_Stock_Task();
            Task.Task_ID = MyGUID.NewGUID();
            Task.LinkMainCID = PL.LinkMainCID;
            Task.Create_DT = DateTime.Now;
            Task.Location = Loc.Location;
            Task.Type = Loc.Type;
            Task.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            Task.Property = WMS_Stock_Task_Property_Enum.日常盘库.ToString();
            db.WMS_Stock_Task.Add(Task);

            //创建底盘信息
            List<WMS_Stocktaking> Stocktaking_List = new List<WMS_Stocktaking>();
            WMS_Stocktaking Stocktaking = new WMS_Stocktaking();

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == PL.LinkMainCID && x.Location == PL.Location).ToList();

            DateTime Create_DT = DateTime.Now;
            foreach (var x in Stock_List)
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
                Stocktaking.Task_Bat_No = "";
                Stocktaking.Work_Person = Task.Work_Person;
                Stocktaking.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
                Stocktaking_List.Add(Stocktaking);
            }

            if (Stocktaking_List.Any())
            {
                db.WMS_Stocktaking.AddRange(Stocktaking_List);
            }

            MyDbSave.SaveChange(db);
        }
    }

    //首次盘库
    public partial class WmsService : IWmsService
    {
        public WMS_Stocktaking_Scan Get_WMS_Stocktaking_Scan_Item_DB(Guid Scan_ID)
        {
            WMS_Stocktaking_Scan Scan = db.WMS_Stocktaking_Scan.Find(Scan_ID);
            Scan = Scan == null ? new WMS_Stocktaking_Scan() : Scan;
            return Scan;
        }

        public WMS_Stock_Task Get_WMS_Stock_Task_Item_First(Guid TaskID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }

            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID).ToList();
            List<WMS_Stocktaking_Scan> Scan_List_Sub = new List<WMS_Stocktaking_Scan>();
            List<string> MatSn_List = Scan_List.Select(x => x.MatSn).Distinct().ToList();
            WMS_Stock_Task_Line Line = new WMS_Stock_Task_Line();
            int i = 0;
            foreach (var MatSn in MatSn_List)
            {
                i++;
                Scan_List_Sub = Scan_List.Where(c => c.MatSn == MatSn).ToList();
                Line = new WMS_Stock_Task_Line();
                Line.Line_No = i;
                Line.MatSn = MatSn;
                Line.Line_ID = Scan_List_Sub.FirstOrDefault().Scan_ID;
                Line.Quantity_Scan_Sum = Scan_List_Sub.Sum(c => c.Scan_Quantity);
                Task.Line_List.Add(Line);
            }

            Task.Scan_List = Scan_List.OrderByDescending(x => x.Create_DT).ToList();
            return Task;
        }

        public void Finish_WMS_Stocktaking_Task_First(Guid TaskID)
        {
            WMS_Stock_Task Task = Get_WMS_Stock_Task_Item_First(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }

            if (Task.Scan_List.Any() == false)
            {
                throw new Exception("未扫描产品");
            }

            if (db.WMS_Stock.Where(x => x.LinkMainCID == Task.LinkMainCID && x.Location == Task.Location).Any())
            {
                throw new Exception("此库位中已存在产品，不支持首次盘库");
            }

            Task.Status = WMS_Stock_Task_Enum.已盘库.ToString();
            db.Entry(Task).State = EntityState.Modified;

            List<WMS_Stock> Stock_List = new List<WMS_Stock>();
            WMS_Stock Stock = new WMS_Stock();
            DateTime DT = DateTime.Now;

            //更新底盘扫描状态,并添加到系统库存
            foreach (var x in Task.Scan_List.OrderBy(x => x.Create_DT))
            {
                Stock = new WMS_Stock();
                Stock.Stock_ID = MyGUID.NewGUID();
                Stock.Quantity = x.Scan_Quantity;
                Stock.Package = x.Package_Type;
                Stock.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                Stock.Location = Task.Location;
                Stock.WMS_In_DT = DT.ToString("yyyy-MM-dd HH:mm");
                Stock.MatSn = x.MatSn;
                Stock.MatName = " ";
                Stock.MatUnit = "PCS";
                Stock.MatBrand = x.MatBrand;
                Stock.Price = 0;
                Stock.Wms_In_Head_ID = Guid.Empty;
                Stock.LinkMainCID = Task.LinkMainCID;
                Stock_List.Add(Stock);

                x.Status = WMS_Stocktaking_Status_Enum.已底盘.ToString();
                db.Entry(x).State = EntityState.Modified;
            }

            db.WMS_Stock.AddRange(Stock_List);

            MyDbSave.SaveChange(db);
        }

        public string Get_WMS_Stocktaking_List_To_Excel(Guid Task_ID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(Task_ID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }

            List<string> MatSn_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID).Select(x => x.MatSn).Distinct().ToList();

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();

            TableHeads.Add("产品型号");
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
            foreach (var MatSn in MatSn_List)
            {
                i++;
                newRow = DT.NewRow();
                newRow["产品型号"] = MatSn;
                newRow["数量"] = "";
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public PageList<WMS_Stock_Group> Get_WMS_Stock_Group_First_PageList(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            List<WMS_Stock> Stock_List = query.ToList();

            var Group = from x in Stock_List
                        group x by x.MatSn into g
                        select new
                        {
                            MatSn = g.Key,
                            GroupID = g.FirstOrDefault().Stock_ID,
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();

            List<string> MatSn_List = Stock_List.Select(x => x.MatSn).Distinct().ToList();
            List<string> Location_List = new List<string>();
            foreach (var x in Group)
            {
                Location_List = Stock_List.Where(c => c.MatSn == x.MatSn).Select(c => c.Location).Distinct().ToList();
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = x.MatSn;
                Stock_Group.GroupID = x.GroupID;

                foreach (var Location in Location_List)
                {
                    Stock_Group.Location += Location + ",";
                }
                Stock_Group.Location = CommonLib.Trim_End_Char(Stock_Group.Location);
                Stock_Group.Line_Count = Location_List.Count();
                Group_List.Add(Stock_Group);
            }

            PageList<WMS_Stock_Group> PList = new PageList<WMS_Stock_Group>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = Group_List.Count();
            PList.Rows = Group_List.Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        //临时批量导入库存（按库位）
        public void Batch_Create_WMS_Stock_By_Location(HttpPostedFileBase ExcelFile, string Location, User U)
        {
            Location = Location.Trim();
            if (string.IsNullOrEmpty(Location)) { throw new Exception("未填写库位"); }

            if (db.WMS_Location.Where(x => x.LinkMainCID == U.LinkMainCID && x.Location == Location).Any() == false)
            {
                throw new Exception("系统中不存在此库位");
            }

            if (db.WMS_Stock.Where(x => x.LinkMainCID == U.LinkMainCID && x.Location == Location).Any())
            {
                throw new Exception("该库位已存在产品");
            }

            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Stock/" + Location);

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);

            List<WMS_Stock> Line_List = new List<WMS_Stock>();
            WMS_Stock Line = new WMS_Stock();

            string WMS_In_DT = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            int Line_No = 0;
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                Line_No++;
                Line = new WMS_Stock();
                Line.Stock_ID = MyGUID.NewGUID();
                Line.MatUnit = "PCS";
                Line.MatBrand = "";
                Line.LinkMainCID = U.LinkMainCID;
                Line.WMS_In_DT = WMS_In_DT;
                Line.MatName = "";
                Line.Package = WMS_Stock_Package_Enum.零头.ToString();
                Line.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                Line.Location = Location;
                try { Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }

                try { Line.Quantity = Convert.ToInt32(row.GetCell(1).ToString().Trim()); } catch { Line.Quantity = 0; }

                //过滤换行符
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

                //判断单元格是否有公式
                for (int j = 0; j < 3; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                //判断本次导入是否有重复型号
                if (!string.IsNullOrEmpty(Line.MatSn) && Line_List.Where(c => c.MatSn == Line.MatSn).Any())
                {
                    throw new Exception("导入数据存在重复");
                }

                Line_List.Add(Line);
            }

            if (Line_No > 10000)
            {
                throw new Exception("导入产品数据已超过1000条");
            }

            //与系统中的型号进行比对
            List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();
            List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == U.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
            if (Mat_List.Count() != MatSn_List.Count())
            {
                throw new Exception("Excel中存在与系统中不同的产品型号！");
            }

            if (Line_List.Any())
            {
                db.WMS_Stock.AddRange(Line_List);
                MyDbSave.SaveChange(db);
            }
        }

        //临时批量导入库存（全部）
        public void Batch_Create_WMS_Stock(HttpPostedFileBase ExcelFile, User U)
        {
            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Stock/Excel");

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);

            List<WMS_Stock> Line_List = new List<WMS_Stock>();
            WMS_Stock Line = new WMS_Stock();

            string WMS_In_DT = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            int Line_No = 0;
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                Line_No++;
                Line = new WMS_Stock();
                Line.Stock_ID = MyGUID.NewGUID();
                Line.MatUnit = "PCS";
                Line.MatBrand = "";
                Line.LinkMainCID = U.LinkMainCID;
                Line.WMS_In_DT = WMS_In_DT;
                Line.MatName = "";
                Line.Package = WMS_Stock_Package_Enum.零头.ToString();
                Line.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                try { Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }
                try { Line.Quantity = Convert.ToInt32(row.GetCell(1).ToString().Trim()); } catch { Line.Quantity = 0; }
                try { Line.Location = row.GetCell(2).ToString().Trim(); } catch { Line.Location = string.Empty; }

                //过滤换行符
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);
                Line.Location = Line.Location.Replace(Environment.NewLine, "");

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

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

                if (!string.IsNullOrEmpty(Line.MatSn) && string.IsNullOrEmpty(Line.Location))
                {
                    throw new Exception(Line.MatSn + "的库位为空");
                }

                if (!string.IsNullOrEmpty(Line.MatSn) && Line.Quantity <= 0)
                {
                    throw new Exception(Line.Location + "中的" + Line.MatSn + "的数量格式不正确");
                }

                //判断同一库位是否有重复型号
                if (!string.IsNullOrEmpty(Line.MatSn) && Line_List.Where(c => c.MatSn == Line.MatSn && c.Location == Line.Location).Any())
                {
                    throw new Exception(Line.MatSn + "在库位" + Line.Location + "中存在重复型号");
                }

                Line_List.Add(Line);
            }

            if (Line_No > 10000)
            {
                throw new Exception("导入产品数据已超过1000条");
            }

            //判断库位是否与系统中库存一致
            List<string> Location_Str_List = Line_List.Select(x => x.Location).Distinct().ToList();
            List<WMS_Location> Location_List = db.WMS_Location.Where(x => x.LinkMainCID == U.LinkMainCID && Location_Str_List.Contains(x.Location)).ToList();
            if (Location_List.Count() != Location_Str_List.Count())
            {
                throw new Exception("Excel中存在与系统中不同的库位");
            }

            ////判断库存中是否已存在产品
            //List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == U.LinkMainCID && Location_Str_List.Contains(x.Location)).ToList();
            //foreach (var Location in Location_Str_List)
            //{
            //    if (Stock_List.Where(x => x.Location == Location).Any())
            //    {
            //        throw new Exception("库位" + Location + "中已存在产品");
            //    }
            //}

            //与系统中的型号进行比对
            List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();
            List<Material> Mat_List = db.Material.Where(x => x.LinkMainCID == U.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
            if (Mat_List.Count() != MatSn_List.Count())
            {
                throw new Exception("Excel中存在与系统中不同的产品型号！");
            }

            if (Line_List.Any())
            {
                db.WMS_Stock.AddRange(Line_List);


                MyDbSave.SaveChange(db);
            }
        }

        public string Get_WMS_Stocktaking_List_All_To_Excel(Guid LinkMainCID)
        {
            List<WMS_Stocktaking_Scan> List_DB = db.WMS_Stocktaking_Scan.Where(x => x.LinkMainCID == LinkMainCID).ToList();
            List<string> Location_List = List_DB.Select(x => x.Location).Distinct().ToList();

            List<string> MatSn_List = new List<string>();

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();

            TableHeads.Add("产品型号");
            TableHeads.Add("数量");
            TableHeads.Add("库位");
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

            foreach (var Location in Location_List)
            {
                MatSn_List = List_DB.Where(x => x.Location == Location).Select(x => x.MatSn).Distinct().ToList();
                foreach (var MatSn in MatSn_List)
                {
                    i++;
                    newRow = DT.NewRow();
                    newRow["产品型号"] = MatSn;
                    newRow["数量"] = "";
                    newRow["库位"] = Location;
                    DT.Rows.Add(newRow);
                }
            }

            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }
    }

    //快递费用
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Track> Get_WMS_In_Track_PageList(WMS_In_Filter MF)
        {
            var query = db.WMS_Track.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (MF.LinkHeadID != Guid.Empty)
            {
                query = query.Where(x => x.Link_Head_ID == MF.LinkHeadID).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Company))
            {
                query = query.Where(x => x.Logistics_Company.Contains(MF.Logistics_Company)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Tracking_No))
            {
                query = query.Where(x => x.Tracking_No.Contains(MF.Tracking_No)).AsQueryable();
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

                query = query.Where(x => x.Scan_PDA_Date >= Start_DT && x.Scan_PDA_Date <= End_DT).AsQueryable();
            }

            PageList<WMS_Track> PList = new PageList<WMS_Track>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Scan_PDA_Date).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public PageList<WMS_Track> Get_WMS_Out_Track_PageList(WMS_Out_Filter MF)
        {
            var query = db.WMS_Track.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (MF.LinkHeadID != Guid.Empty)
            {
                query = query.Where(x => x.Link_Head_ID == MF.LinkHeadID).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Logistics_Company))
            {
                query = query.Where(x => x.Logistics_Company.Contains(MF.Logistics_Company)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Tracking_No))
            {
                query = query.Where(x => x.Tracking_No.Contains(MF.Tracking_No)).AsQueryable();
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

                query = query.Where(x => x.Scan_PDA_Date >= Start_DT && x.Scan_PDA_Date <= End_DT).AsQueryable();
            }

            PageList<WMS_Track> PList = new PageList<WMS_Track>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Scan_PDA_Date).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public WMS_Track Get_WMS_Track_Empty()
        {
            WMS_Track Track = new WMS_Track();
            Track = Track == null ? new WMS_Track() : Track;
            return Track;
        }

        public WMS_Track Get_WMS_Track_Item(Guid Tracking_ID)
        {
            WMS_Track Track = db.WMS_Track.Find(Tracking_ID);
            Track = Track == null ? new WMS_Track() : Track;
            return Track;
        }

        public void Add_WMS_In_Track(WMS_Track Track)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Track.Link_Head_ID);
            Head = Head == null ? new WMS_In_Head() : Head;

            if (Head.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("收货任务已完成，不支持添加");
            }

            if (string.IsNullOrEmpty(Head.Driver_Name)) { throw new Exception("未设置驾驶员！"); }

            Track.Tracking_ID = MyGUID.NewGUID();
            Track.Logistics_Company = Head.Logistics_Company.Trim();
            Track.Logistics_Mode = Head.Logistics_Mode.Trim();
            Track.Link_Head_ID = Head.Head_ID;
            Track.LinkMainCID = Head.LinkMainCID;
            Track.Create_DT = DateTime.Now;
            Track.Tracking_Type = Tracking_Type_Enum.收货.ToString();
            Track.Link_Head_Com_Name = Head.Supplier_Name;
            try
            {
                Track.Logistics_Cost = Convert.ToDecimal(Track.Logistics_Cost);
                Track.Kilometers = Convert.ToDecimal(Track.Kilometers);
            }
            catch { }

            if (Track.Logistics_Cost < 0) { throw new Exception("请输入正确的金额！"); }
            if (Track.Kilometers < 0) { throw new Exception("请输入正确的公里数！"); }

            Track.Tracking_No = Track.Tracking_No.Trim();

            if (db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tracking_ID != Track.Tracking_ID && x.Tracking_No == Track.Tracking_No).Any())
            {
                throw new Exception("车牌号信息重复！");
            }

            Track.Driver_Name = Head.Driver_Name;
            db.WMS_Track.Add(Track);
            MyDbSave.SaveChange(db);
        }

        public void Add_WMS_Out_Track(WMS_Track Track)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Track.Link_Head_ID);
            Head = Head == null ? new WMS_Out_Head() : Head;

            if (Head.Status == WMS_Out_Global_State_Enum.已出库.ToString())
            {
                throw new Exception("出库任务已完成，不支持添加");
            }

            if (string.IsNullOrEmpty(Head.Driver_Name)) { throw new Exception("未设置驾驶员！"); }

            Track.Tracking_ID = MyGUID.NewGUID();
            Track.Logistics_Company = Head.Logistics_Company.Trim();
            Track.Logistics_Mode = Head.Logistics_Mode.Trim();
            Track.Link_Head_ID = Head.Head_ID;
            Track.LinkMainCID = Head.LinkMainCID;
            Track.Create_DT = DateTime.Now;
            Track.Tracking_Type = Tracking_Type_Enum.送货.ToString();
            Track.Link_Head_Com_Name = Head.Customer_Name;
            try
            {
                Track.Logistics_Cost = Convert.ToDecimal(Track.Logistics_Cost);
                Track.Kilometers = Convert.ToDecimal(Track.Kilometers);
            }
            catch { }

            if (Track.Logistics_Cost < 0) { throw new Exception("请输入正确的金额！"); }
            if (Track.Kilometers < 0) { throw new Exception("请输入正确的公里数！"); }

            Track.Tracking_No = Track.Tracking_No.Trim();

            if (db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tracking_ID != Track.Tracking_ID && x.Tracking_No == Track.Tracking_No).Any())
            {
                throw new Exception("车牌号信息重复！");
            }

            Track.Driver_Name = Head.Driver_Name;
            db.WMS_Track.Add(Track);
            MyDbSave.SaveChange(db);
        }

        public void Set_WMS_In_Track_Item(WMS_Track Track)
        {
            if (Track.Logistics_Cost <= 0) { throw new Exception("请输入正确的金额！"); }
            if (Track.Kilometers <= 0) { throw new Exception("请输入正确的公里数！"); }

            WMS_Track Track_DB = db.WMS_Track.Find(Track.Tracking_ID);
            if (Track_DB == null) { throw new Exception("WMS_Track is null!"); }
            WMS_In_Head Head = db.WMS_In_Head.Find(Track_DB.Link_Head_ID);
            if (Head == null) { throw new Exception("WMS_In_Head is null!"); }

            if (Head.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("已完成收货，不支持更新！");
            }

            Track_DB.Kilometers = Track.Kilometers;
            Track_DB.Logistics_Cost = Track.Logistics_Cost;
            db.Entry(Track_DB).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Set_WMS_Out_Track_Item(WMS_Track Track)
        {
            if (Track.Logistics_Cost <= 0) { throw new Exception("请输入正确的金额！"); }
            if (Track.Kilometers <= 0) { throw new Exception("请输入正确的公里数！"); }

            WMS_Track Track_DB = db.WMS_Track.Find(Track.Tracking_ID);
            if (Track_DB == null) { throw new Exception("WMS_Track is null!"); }
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Track_DB.Link_Head_ID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null!"); }

            if (Head.Status == WMS_Out_Global_State_Enum.已出库.ToString())
            {
                throw new Exception("已完成出库，不支持更新！");
            }

            Track_DB.Kilometers = Track.Kilometers;
            Track_DB.Logistics_Cost = Track.Logistics_Cost;
            db.Entry(Track_DB).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_WMS_In_Track(Guid Tracking_ID)
        {
            WMS_Track Track_DB = db.WMS_Track.Find(Tracking_ID);
            if (Track_DB == null) { throw new Exception("WMS_In_Track is null!"); }

            WMS_In_Head Head = db.WMS_In_Head.Find(Track_DB.Link_Head_ID);
            if (Head == null) { throw new Exception("WMS_In_Head is null!"); }

            if (Head.Status == WMS_In_Global_State_Enum.完成入库.ToString())
            {
                throw new Exception("该任务已完成入库，不支持删除！");
            }

            db.WMS_Track.Remove(Track_DB);
            MyDbSave.SaveChange(db);
        }

        public void Delete_WMS_Out_Track(Guid Tracking_ID)
        {
            WMS_Track Track_DB = db.WMS_Track.Find(Tracking_ID);
            if (Track_DB == null) { throw new Exception("WMS_In_Track is null!"); }

            WMS_Out_Head Head = db.WMS_Out_Head.Find(Track_DB.Link_Head_ID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null!"); }

            if (Head.Status == WMS_Out_Global_State_Enum.已出库.ToString())
            {
                throw new Exception("该任务已出库，不支持删除！");
            }

            db.WMS_Track.Remove(Track_DB);
            MyDbSave.SaveChange(db);
        }
    }

    //快递费用统计
    public partial class WmsService : IWmsService
    {
        //获取当前订单及开票统计数据
        public Logistics_Cost_Year GetLogistics_Cost_YearList(Logistics_Cost_Filter MF)
        {
            List<Logistics_Cost_Year> List = new List<Logistics_Cost_Year>();
            DateTime ST = Convert.ToDateTime(MF.Year + "-1-1 00:00:00");
            DateTime ET = Convert.ToDateTime(MF.Year + "-12-31 23:59:59");

            List<Logistics_Cost_Year_Sub_By_Logistics> Logistics_List = this.Get_Tracking_No_By_Year(MF);

            //拆分为12个月
            List<Logistics_Cost_Year_Sub> SubList = new List<Logistics_Cost_Year_Sub>();
            Logistics_Cost_Year_Sub Sub = new Logistics_Cost_Year_Sub();

            foreach (var DT in DTList.YearAllMonths(MF.Year))
            {
                Sub = new Logistics_Cost_Year_Sub();
                Sub.SD = DT.SD;
                Sub.ED = DT.ED;
                Sub.MonthStr = DT.SD.Month.ToString();
                Sub.Price_Amount = 0;
                Sub.Tracking_No_Amount = 0;

                foreach (var xx in Logistics_List)
                {
                    foreach (var xxx in xx.SubList.Where(c => c.MonthStr == Sub.MonthStr).ToList())
                    {
                        Sub.Price_Amount += xxx.Price_Amount;
                        Sub.Tracking_No_Amount += xxx.Tracking_No_Amount;
                    }
                }

                SubList.Add(Sub);
            }

            Logistics_Cost_Year Logistics_Cost = new Logistics_Cost_Year();
            Logistics_Cost.Year = MF.Year;
            Logistics_Cost.LinkMainCID = MF.LinkMainCID;
            Logistics_Cost.YearList = new List<string>();

            int SDYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            try { SDYear = Convert.ToInt32(db.WMS_Track.Where(c => c.LinkMainCID == MF.LinkMainCID).Min(c => c.Create_DT).ToString("yyyy")); } catch { }
            int EDYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

            for (int i = SDYear; i <= EDYear; i++)
            {
                Logistics_Cost.YearList.Add(i.ToString());
            }

            Logistics_Cost.SubList = SubList;
            Logistics_Cost.SubCustList = Logistics_List;
            return Logistics_Cost;
        }

        private List<Logistics_Cost_Year_Sub_By_Logistics> Get_Tracking_No_By_Year(Logistics_Cost_Filter MF)
        {
            DateTime ST = Convert.ToDateTime(MF.Year + "-1-1 00:00:00");
            DateTime ET = Convert.ToDateTime(MF.Year + "-12-31 23:59:59");

            List<WMS_Logistics> Logistics_List = db.WMS_Logistics.Where(x => x.MatType == WMS_In_Type_Enum.零星调货.ToString()).ToList();

            //获取当年所有快递单
            var query = db.WMS_Track.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Create_DT >= ST && x.Create_DT <= ET && x.Driver_Name == "").AsQueryable();

            if (!string.IsNullOrEmpty(MF.TrackingType))
            {
                query = query.Where(x => x.Tracking_Type == MF.TrackingType).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.Logistics_Company.Contains(MF.Keyword)).AsQueryable();
                Logistics_List = Logistics_List.Where(x => x.Company_Name.Contains(MF.Keyword)).ToList();
            }

            List<WMS_Track> Track_List = query.ToList();

            List<Logistics_Cost_Year_Sub_By_Logistics> List = new List<Logistics_Cost_Year_Sub_By_Logistics>();
            Logistics_Cost_Year_Sub_By_Logistics Sub = new Logistics_Cost_Year_Sub_By_Logistics();
            foreach (var x in Logistics_List)
            {
                if (Track_List.Where(c => c.Logistics_Company.Contains(x.Company_Name)).Any())
                {
                    Sub = new Logistics_Cost_Year_Sub_By_Logistics();
                    Sub.Logistics_ID = x.Log_ID;
                    Sub.Logistics_Name = x.Company_Name;

                    try
                    {
                        Sub.Tracking_No_Amount = Track_List.Where(c => c.Logistics_Company.Contains(x.Company_Name)).Count();
                        Sub.Price_Amount = Track_List.Where(c => c.Logistics_Company.Contains(x.Company_Name)).Sum(c => c.Logistics_Cost);
                    }
                    catch
                    {
                        Sub.Tracking_No_Amount = 0;
                        Sub.Price_Amount = 0;
                    }
                    Sub.SubList = Get_Logistics_Cost_Year_Sub_By_Logistics_Sub_List(MF.Year, Sub.Logistics_ID, Track_List.Where(c => c.Logistics_Company.Contains(x.Company_Name)).ToList());
                    List.Add(Sub);
                }
            }

            return List.OrderByDescending(c => c.Price_Amount).ToList();
        }

        private List<Logistics_Cost_Year_Sub_By_Logistics_Sub> Get_Logistics_Cost_Year_Sub_By_Logistics_Sub_List(string Year, Guid Logistics_ID, List<WMS_Track> Track_List)
        {
            List<Logistics_Cost_Year_Sub_By_Logistics_Sub> List = new List<Logistics_Cost_Year_Sub_By_Logistics_Sub>();
            Logistics_Cost_Year_Sub_By_Logistics_Sub Sub = new Logistics_Cost_Year_Sub_By_Logistics_Sub();
            foreach (var DT in DTList.YearAllMonths(Year))
            {
                Sub = new Logistics_Cost_Year_Sub_By_Logistics_Sub();
                Sub.Logistics_ID = Logistics_ID;
                Sub.MonthStr = DT.SD.Month.ToString();
                Sub.MonthStr_SD = DT.SD.ToString("yyyy-MM-dd");
                Sub.MonthStr_ED = DT.ED.ToString("yyyy-MM-dd");
                try
                {
                    Sub.Tracking_No_Amount = Track_List.Where(x => x.Create_DT >= DT.SD && x.Create_DT <= DT.ED).Count();
                    Sub.Price_Amount = Track_List.Where(x => x.Create_DT >= DT.SD && x.Create_DT <= DT.ED).Sum(c => c.Logistics_Cost);
                }
                catch
                {
                    Sub.Tracking_No_Amount = 0;
                    Sub.Price_Amount = 0;
                }

                List.Add(Sub);
            }
            return List;
        }

        //统计当月
        public List<WMS_Track> Get_WMS_In_Track_By_Month(DateTime SD, DateTime ED, Guid Logistics_ID, Guid LinkMainCID)
        {
            WMS_Logistics Log = db.WMS_Logistics.Find(Logistics_ID);
            Log = Log == null ? new WMS_Logistics() : Log;

            var query = db.WMS_Track.Where(x => x.LinkMainCID == LinkMainCID && x.Create_DT >= SD && x.Create_DT <= ED && x.Driver_Name == "").AsQueryable();
            if (Log != null)
            {
                query = query.Where(x => x.Logistics_Company.Contains(Log.Company_Name)).AsQueryable();
            }

            List<WMS_Track> Track_List = query.ToList();

            return Track_List;
        }

        public string Get_WMS_In_Track_By_Month_To_Excel(DateTime SD, DateTime ED, Guid Logistics_ID, Guid LinkMainCID)
        {
            List<WMS_Track> Track_List = Get_WMS_In_Track_By_Month(SD, ED, Logistics_ID, LinkMainCID);

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            if (Logistics_ID == Guid.Empty)
            {
                TableHeads.Add("快递物流");
            }

            TableHeads.Add("单号");
            TableHeads.Add("金额");
            TableHeads.Add("公司名称");
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
            foreach (var x in Track_List)
            {
                i++;
                newRow = DT.NewRow();
                if (Logistics_ID == Guid.Empty)
                {
                    newRow["快递物流"] = x.Logistics_Company;
                }

                newRow["单号"] = x.Tracking_No;
                newRow["金额"] = x.Logistics_Cost.ToString("N2");
                newRow["公司名称"] = x.Link_Head_Com_Name;

                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public PageList<WMS_Track> Get_WMS_In_Track_With_Driver(Logistics_Cost_Filter MF)
        {
            var query = db.WMS_Track.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Driver_Name != "").AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.Driver_Name.Contains(MF.Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Tracking_No))
            {
                query = query.Where(x => x.Tracking_No.Contains(MF.Tracking_No)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Com_Name))
            {
                query = query.Where(x => x.Link_Head_Com_Name.Contains(MF.Com_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.TrackingType))
            {
                query = query.Where(x => x.Tracking_Type.Contains(MF.TrackingType)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.SD) && !string.IsNullOrEmpty(MF.ED))
            {
                DateTime Start_DT = Convert.ToDateTime((MF.SD));
                DateTime End_DT = Convert.ToDateTime((MF.ED));

                if (DateTime.Compare(Start_DT, End_DT) > 0)
                {
                    throw new Exception("起始时间不可大于结束时间！");
                }

                query = query.Where(x => x.Scan_PDA_Date >= Start_DT && x.Scan_PDA_Date <= End_DT).AsQueryable();
            }

            PageList<WMS_Track> PList = new PageList<WMS_Track>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Scan_PDA_Date).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public PageList<WMS_Track> Get_WMS_Out_Track_PageList(Logistics_Cost_Filter MF)
        {
            var query = db.WMS_Track.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Driver_Name == "").AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.Tracking_No.Contains(MF.Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.TrackingType))
            {
                query = query.Where(x => x.Tracking_Type.Contains(MF.TrackingType)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Com_Name))
            {
                query = query.Where(x => x.Link_Head_Com_Name.Contains(MF.Com_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.SD) && !string.IsNullOrEmpty(MF.ED))
            {
                DateTime Start_DT = Convert.ToDateTime((MF.SD));
                DateTime End_DT = Convert.ToDateTime((MF.ED));

                if (DateTime.Compare(Start_DT, End_DT) > 0)
                {
                    throw new Exception("起始时间不可大于结束时间！");
                }

                query = query.Where(x => x.Scan_PDA_Date >= Start_DT && x.Scan_PDA_Date <= End_DT).AsQueryable();
            }

            PageList<WMS_Track> PList = new PageList<WMS_Track>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public string Get_WMS_In_Track_With_Driver_To_Excel(Logistics_Cost_Filter MF)
        {
            List<WMS_Track> Track_List = Get_WMS_In_Track_With_Driver(MF).Rows;

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();

            TableHeads.Add("驾驶员");
            TableHeads.Add("车牌号");
            TableHeads.Add("公里数");
            TableHeads.Add("金额");
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
            foreach (var x in Track_List)
            {
                i++;
                newRow = DT.NewRow();
                newRow["驾驶员"] = x.Driver_Name;
                newRow["车牌号"] = x.Tracking_No;
                newRow["公里数"] = x.Kilometers.ToString("N2");
                newRow["金额"] = x.Logistics_Cost.ToString("N2");

                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public string Get_WMS_Out_Track_To_Excel(Logistics_Cost_Filter MF)
        {
            List<WMS_Track> Track_List = Get_WMS_Out_Track_PageList(MF).Rows;

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();

            TableHeads.Add("单号");
            TableHeads.Add("重量");
            TableHeads.Add("客户名称");
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
            foreach (var x in Track_List)
            {
                i++;
                newRow = DT.NewRow();
                newRow["单号"] = x.Tracking_No;
                newRow["重量"] = x.Weight;
                newRow["客户名称"] = x.Link_Head_Com_Name;

                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }
    }

    //快递资料
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Track_Info> Get_WMS_Track_Info_PageList(Track_Info_Filter MF)
        {
            var query = db.WMS_Track_Info.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Logistics_Company))
            {
                query = query.Where(x => x.Logistics_Company.Contains(MF.Logistics_Company)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Sender_Name))
            {
                query = query.Where(x => x.Sender_Name.Contains(MF.Sender_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Receiver_Name))
            {
                query = query.Where(x => x.Receiver_Name.Contains(MF.Receiver_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Tracking_No))
            {
                query = query.Where(x => x.Tracking_No.Contains(MF.Tracking_No)).AsQueryable();
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

            PageList<WMS_Track_Info> PList = new PageList<WMS_Track_Info>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public WMS_Track_Info Get_WMS_Track_Info_DB(Guid InfoID)
        {
            WMS_Track_Info Info = db.WMS_Track_Info.Find(InfoID);
            Info = Info == null ? new WMS_Track_Info() : Info;
            return Info;
        }

        public List<WMS_Track_Info> Get_WMS_Track_Info_List_From_Upload(HttpPostedFileBase ExcelFile, User U)
        {
            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "WMS_Track_Info/" + U.UID);

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            HSSFWorkbook workbook = new HSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //读取Excel列，装箱数据
            List<WMS_Track_Info> Line_List = new List<WMS_Track_Info>();
            WMS_Track_Info Line = new WMS_Track_Info();
            ISheet sheet = workbook.GetSheetAt(0);

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                Line = new WMS_Track_Info();

                try { Line.Sender_Name = row.GetCell(1).ToString().Trim(); } catch { Line.Sender_Name = string.Empty; }
                if (string.IsNullOrEmpty(Line.Sender_Name)) { break; }
                try { Line.Sender_Phone = row.GetCell(2).ToString().Trim(); } catch { Line.Sender_Phone = string.Empty; }
                try { Line.Sender_Tel = row.GetCell(3).ToString().Trim(); } catch { Line.Sender_Tel = string.Empty; }
                try { Line.Sender_Address = row.GetCell(4).ToString().Trim(); } catch { Line.Sender_Address = string.Empty; }
                try { Line.Receiver_Name = row.GetCell(5).ToString().Trim(); } catch { Line.Receiver_Name = string.Empty; }
                try { Line.Receiver_Phone = row.GetCell(6).ToString().Trim(); } catch { Line.Receiver_Phone = string.Empty; }
                try { Line.Receiver_Tel = row.GetCell(7).ToString().Trim(); } catch { Line.Receiver_Tel = string.Empty; }
                try { Line.Receiver_Address = row.GetCell(8).ToString().Trim(); } catch { Line.Receiver_Address = string.Empty; }
                try { Line.Item_Info = row.GetCell(9).ToString().Trim(); } catch { Line.Item_Info = string.Empty; }
                try { Line.Logistics_Company = row.GetCell(10).ToString().Trim(); } catch { Line.Logistics_Company = string.Empty; }
                try { Line.Logistics_Company_Loc = row.GetCell(11).ToString().Trim(); } catch { Line.Logistics_Company_Loc = string.Empty; }
                try { Line.Tracking_No = row.GetCell(12).ToString().Trim(); } catch { Line.Tracking_No = string.Empty; }

                //过滤换行符
                Line.Sender_Name = Line.Sender_Name.Replace(Environment.NewLine, "");
                Line.Sender_Phone = Line.Sender_Phone.Replace(Environment.NewLine, "");
                Line.Sender_Tel = Line.Sender_Tel.Replace(Environment.NewLine, "");
                Line.Sender_Address = Line.Sender_Address.Replace(Environment.NewLine, "");
                Line.Receiver_Name = Line.Receiver_Name.Replace(Environment.NewLine, "");
                Line.Receiver_Phone = Line.Receiver_Phone.Replace(Environment.NewLine, "");
                Line.Receiver_Tel = Line.Receiver_Tel.Replace(Environment.NewLine, "");
                Line.Receiver_Address = Line.Receiver_Address.Replace(Environment.NewLine, "");
                Line.Item_Info = Line.Item_Info.Replace(Environment.NewLine, "");
                Line.Logistics_Company = Line.Logistics_Company.Replace(Environment.NewLine, "");
                Line.Logistics_Company_Loc = Line.Logistics_Company_Loc.Replace(Environment.NewLine, "");
                Line.Tracking_No = Line.Tracking_No.Replace(Environment.NewLine, "");

                //判断单元格是否有公式
                for (int j = 0; j < 13; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                if (string.IsNullOrEmpty(Line.Sender_Name) || string.IsNullOrEmpty(Line.Sender_Address)
                    || string.IsNullOrEmpty(Line.Receiver_Name) || string.IsNullOrEmpty(Line.Receiver_Address)
                    || string.IsNullOrEmpty(Line.Item_Info) || string.IsNullOrEmpty(Line.Logistics_Company)
                    || string.IsNullOrEmpty(Line.Tracking_No))
                {
                    throw new Exception("Excel中" + Line.Tracking_No + "信息不全");
                }

                if (Line_List.Where(c => c.Tracking_No == Line.Tracking_No).Any())
                {
                    throw new Exception("单号" + Line.Tracking_No + "在Excel中存在重复");
                }

                Line_List.Add(Line);
            }

            List<string> Tracking_No_List = Line_List.Select(x => x.Tracking_No).Distinct().ToList();
            if (db.WMS_Track_Info.Where(x => x.LinkMainCID == U.LinkMainCID && Tracking_No_List.Contains(x.Tracking_No)).Any())
            {
                throw new Exception("Excel中存在与系统重复的快递单号");
            }

            return Line_List;
        }

        public void Batch_Create_WMS_Track_Info(List<WMS_Track_Info> List, User U)
        {
            DateTime DT = DateTime.Now;
            foreach (var x in List)
            {
                x.Info_ID = MyGUID.NewGUID();
                x.LinkMainCID = U.LinkMainCID;
                x.Create_Person = U.UserFullName;
                x.Create_DT = DT;
            }

            db.WMS_Track_Info.AddRange(List);

            //批量发送邮件
            ISentEmailService IS = new SentEmailService();
            IS.Batch_Sent_To_Sales_With_WMS_Out_Finish_With_Tracking_No(List, U);

            MyDbSave.SaveChange(db);
        }
    }

    //报废出库
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Waste_Head> Get_WMS_Waste_Head_PageList(WMS_Waste_Filter MF)
        {
            var query = db.WMS_Waste_Head.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_No_Str))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_No_Str)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person.Contains(MF.Create_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Auditor))
            {
                query = query.Where(x => x.Auditor.Contains(MF.Auditor)).AsQueryable();
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

            PageList<WMS_Waste_Head> PList = new PageList<WMS_Waste_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> ID_List = PList.Rows.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Waste_Line> Line_List = db.WMS_Waste_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Waste_Line> Line_List_Sub = new List<WMS_Waste_Line>();
            foreach (var x in PList.Rows)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                if (Line_List_Sub.Any())
                {
                    x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                    x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
                }
            }

            return PList;
        }

        public PageList<WMS_Waste_Head> Get_WMS_Waste_Head_PageList_Sub(WMS_Waste_Filter MF)
        {
            var query = db.WMS_Waste_Head.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            query = query.Where(x => x.Status != WMS_Waste_Head_Status_Enum.已消库.ToString()).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_No_Str))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_No_Str)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person.Contains(MF.Create_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Auditor))
            {
                query = query.Where(x => x.Auditor.Contains(MF.Auditor)).AsQueryable();
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

            PageList<WMS_Waste_Head> PList = new PageList<WMS_Waste_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> ID_List = PList.Rows.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Waste_Line> Line_List = db.WMS_Waste_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Waste_Line> Line_List_Sub = new List<WMS_Waste_Line>();
            foreach (var x in PList.Rows)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                if (Line_List_Sub.Any())
                {
                    x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                    x.Quantity_Sum = Line_List_Sub.Sum(c => c.Quantity);
                }
            }

            return PList;
        }

        public Guid Create_WMS_Waste_Head(User U)
        {
            WMS_Waste_Head Head = new WMS_Waste_Head();
            Head.Head_ID = MyGUID.NewGUID();
            Head.Task_Bat_No = this.Auto_Create_Task_Bat_Waste(U);
            Head.Task_Bat_No_Str = "W" + Head.Task_Bat_No;
            Head.Create_DT = DateTime.Now;
            Head.Create_Person = U.UserFullName;
            Head.Status = WMS_Waste_Head_Status_Enum.待编辑.ToString();
            Head.LinkMainCID = U.LinkMainCID;
            db.WMS_Waste_Head.Add(Head);
            MyDbSave.SaveChange(db);
            return Head.Head_ID;
        }

        public WMS_Waste_Head Get_WMS_Waste_Head_DB(Guid Head_ID)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Waste_Head() : Head;
            return Head;
        }

        public WMS_Waste_Head Get_WMS_Waste_Head_Item(Guid Head_ID)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Waste_Head() : Head;
            Head.Line_List = db.WMS_Waste_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            Head.MatSn_Count = Head.Line_List.Select(x => x.MatSn).Distinct().Count();
            Head.Quantity_Sum = Head.Line_List.Sum(x => x.Quantity);
            return Head;
        }

        public void Delete_WMS_Waste_Head(Guid Head_ID)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Waste_Head() : Head;

            if (Head.Status != WMS_Waste_Head_Status_Enum.待编辑.ToString() && Head.Status != WMS_Waste_Head_Status_Enum.已退回.ToString())
            {
                throw new Exception("报废申请单当前状态不支持删除");
            }

            List<WMS_Waste_Line> Line_List = db.WMS_Waste_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            db.WMS_Waste_Line.RemoveRange(Line_List);
            db.WMS_Waste_Head.Remove(Head);
            MyDbSave.SaveChange(db);
        }

        public List<WMS_Stock_Group> Get_WMS_Stock_Group_List_For_Waste(WMS_Stock_Filter MF)
        {
            var query = db.WMS_Stock.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn_A))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn_A)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn_B))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn_B)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Location))
            {
                query = query.Where(x => x.Location.Contains(MF.Location)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                query = query.Where(x => x.MatBrand.Contains(MF.MatBrand)).AsQueryable();
            }

            List<WMS_Stock> Stock_List = query.ToList();
            var Group = from x in Stock_List
                        group x by new { x.MatSn, x.Location } into g
                        select new
                        {
                            MatSn = g.Key.MatSn,
                            Location = g.Key.Location,
                            MatBrand = g.Select(c => c.MatBrand).FirstOrDefault(),
                            Quantity = g.Sum(c => c.Quantity),
                        };

            List<WMS_Stock_Group> Group_List = new List<WMS_Stock_Group>();
            WMS_Stock_Group Stock_Group = new WMS_Stock_Group();
            foreach (var x in Group)
            {
                Stock_Group = new WMS_Stock_Group();
                Stock_Group.MatSn = x.MatSn;
                Stock_Group.MatBrand = x.MatBrand;
                Stock_Group.Location = x.Location;
                Stock_Group.Quantity_Sum = x.Quantity;
                Group_List.Add(Stock_Group);
            }

            Group_List = Group_List.Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return Group_List;
        }

        private long Auto_Create_Task_Bat_Waste(User U)
        {
            long Task_Bat_No_Min = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + "0001");
            long Task_Bat_No_Max = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + "9999");

            long Task_Bat_No = 0;
            if (db.WMS_Waste_Head.Where(x => x.LinkMainCID == U.LinkMainCID && x.Task_Bat_No >= Task_Bat_No_Min).Any())
            {
                Task_Bat_No = db.WMS_Waste_Head.Where(x => x.LinkMainCID == U.LinkMainCID).Max(x => x.Task_Bat_No) + 1;
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

        public void Create_WMS_Waste_Line(WMS_Waste_Line Line)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(Line.Link_Head_ID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status != WMS_Waste_Head_Status_Enum.待编辑.ToString() && Head.Status != WMS_Waste_Head_Status_Enum.已退回.ToString())
            {
                throw new Exception("报废申请单当前状态不支持添加产品");
            }

            if (db.WMS_Waste_Line.Where(x => x.Link_Head_ID == Head.Head_ID && x.MatSn == Line.MatSn && x.Location == Line.Location).Any())
            {
                throw new Exception("该产品记录已添加，不支持重复添加");
            }

            WMS_Stock Stock = db.WMS_Stock.Where(x => x.LinkMainCID == Line.LinkMainCID && x.MatSn == Line.MatSn && x.Location == Line.Location).FirstOrDefault();
            if (Stock == null) { throw new Exception("WMS_Stock is null"); }

            Line.Line_ID = MyGUID.NewGUID();
            Line.Task_Bat_No = Head.Task_Bat_No;
            Line.Task_Bat_No_Str = Head.Task_Bat_No_Str;
            Line.Create_DT = Head.Create_DT;
            Line.Create_Person = Head.Create_Person;
            Line.MatUnit = Stock.MatUnit;
            Line.Unit_Price = Stock.Price;
            Line.MatName = Stock.MatName;
            Line.MatBrand = Stock.MatBrand;
            db.WMS_Waste_Line.Add(Line);
            MyDbSave.SaveChange(db);
        }

        public List<WMS_Waste_Line> Get_WMS_Waste_Line_List(Guid HeadID)
        {
            return db.WMS_Waste_Line.Where(x => x.Link_Head_ID == HeadID).ToList();
        }

        public void Delete_WMS_Waste_Line_List(Guid HeadID)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status != WMS_Waste_Head_Status_Enum.待编辑.ToString() && Head.Status != WMS_Waste_Head_Status_Enum.已退回.ToString())
            {
                throw new Exception("报废申请单当前状态不支持删除产品");
            }

            List<WMS_Waste_Line> List = db.WMS_Waste_Line.Where(x => x.Link_Head_ID == HeadID).ToList();
            db.WMS_Waste_Line.RemoveRange(List);
            MyDbSave.SaveChange(db);
        }

        public void Delete_WMS_Waste_Line(Guid LineID)
        {
            WMS_Waste_Line Line = db.WMS_Waste_Line.Find(LineID);
            if (Line == null) { throw new Exception("WMS_Waste_Line is null"); }

            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(Line.Link_Head_ID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status != WMS_Waste_Head_Status_Enum.待编辑.ToString() && Head.Status != WMS_Waste_Head_Status_Enum.已退回.ToString())
            {
                throw new Exception("报废申请单当前状态不支持删除产品");
            }

            db.WMS_Waste_Line.Remove(Line);
            MyDbSave.SaveChange(db);
        }

        public void Set_WMS_Task_Waste(Guid HeadID)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status == WMS_Waste_Head_Status_Enum.待审核.ToString())
            {
                throw new Exception("报废单已递交，不支持重复操作");
            }

            if (Head.Status != WMS_Waste_Head_Status_Enum.待编辑.ToString())
            {
                throw new Exception("报废单状态异常");
            }

            Head.Status = WMS_Waste_Head_Status_Enum.待审核.ToString();

            List<WMS_Waste_Line> Line_List = db.WMS_Waste_Line.Where(x => x.Link_Head_ID == HeadID).ToList();
            if (Line_List.Where(x => x.Quantity <= 0).Any())
            {
                throw new Exception("存在产品报废数量小于等于零");
            }

            //需要发邮件
            ISentEmailService IS = new SentEmailService();
            IS.Sent_To_Manager_With_WMS_Waste_Task(Head, Line_List);

            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Link_WMS_Task_Waste(Guid HeadID, Guid Link_HeadID)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(Link_HeadID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status != WMS_Waste_Head_Status_Enum.待编辑.ToString() && Head.Status != WMS_Waste_Head_Status_Enum.已退回.ToString())
            {
                throw new Exception("报废单已递交，不支持操作");
            }

            if (Head.Link_HeadID != Guid.Empty)
            {
                throw new Exception("报废单已关联单据，不支持重复操作");
            }

            if (db.WMS_In_Head.Where(x => x.Head_ID == HeadID).Any() == false)
            {
                throw new Exception("关联失败");
            }

            Head.Link_HeadID = HeadID;
            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Cancel_Link_WMS_Task_Waste(Guid Link_HeadID)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(Link_HeadID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status != WMS_Waste_Head_Status_Enum.待编辑.ToString() && Head.Status != WMS_Waste_Head_Status_Enum.已退回.ToString())
            {
                throw new Exception("报废单已递交，不支持操作");
            }

            Head.Link_HeadID = Guid.Empty;
            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public string Get_WMS_Waste_Line_By_Head_To_Excel(List<WMS_Waste_Line> Line_List)
        {
            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("序");
            TableHeads.Add("产品型号");
            TableHeads.Add("报废数量");
            TableHeads.Add("库位");
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
            foreach (var x in Line_List)
            {
                i++;
                newRow = DT.NewRow();
                newRow["序"] = i;
                newRow["产品型号"] = x.MatSn;
                newRow["报废数量"] = x.Quantity;
                newRow["库位"] = x.Location;
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public void Set_WMS_Waste_Line(WMS_Waste_Line Line)
        {
            WMS_Waste_Line Old_Line = db.WMS_Waste_Line.Find(Line.Line_ID);
            if (Old_Line == null) { throw new Exception("WMS_Waste_Line is null"); }

            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(Old_Line.Link_Head_ID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status == WMS_Waste_Head_Status_Enum.已审核.ToString() || Head.Status == WMS_Waste_Head_Status_Enum.待审核.ToString())
            {
                throw new Exception("报废申请单当前状态不支持更改产品");
            }

            Old_Line.Quantity = Line.Quantity;
            db.Entry(Old_Line).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Confirm_WMS_Task_Waste(Guid HeadID, User U)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status == WMS_Waste_Head_Status_Enum.已审核.ToString())
            {
                throw new Exception("报废单已审核，不支持重复操作");
            }

            if (Head.Status != WMS_Waste_Head_Status_Enum.待审核.ToString())
            {
                throw new Exception("报废单状态异常");
            }

            Head.Status = WMS_Waste_Head_Status_Enum.已审核.ToString();
            Head.Audit_DT = DateTime.Now;
            Head.Auditor = U.UserFullName;

            List<WMS_Waste_Line> Line_List = db.WMS_Waste_Line.Where(x => x.Link_Head_ID == HeadID).ToList();
            if (Line_List.Where(x => x.Quantity <= 0).Any())
            {
                throw new Exception("存在产品报废数量小于等于零");
            }

            //需要发邮件
            ISentEmailService IS = new SentEmailService();
            IS.Sent_To_Accounting_Staff_With_WMS_Waste_Task(Head, Line_List);

            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Confirm_WMS_Task_Waste_By_Accounting(Guid HeadID, User U)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status == WMS_Waste_Head_Status_Enum.待执行.ToString())
            {
                throw new Exception("报废单等待仓库执行，不支持重复操作");
            }

            if (Head.Status != WMS_Waste_Head_Status_Enum.已审核.ToString())
            {
                throw new Exception("报废单状态异常");
            }

            Head.Status = WMS_Waste_Head_Status_Enum.待执行.ToString();
            Head.Approve_DT = DateTime.Now;
            Head.Approver = U.UserFullName;

            List<WMS_Waste_Line> Line_List = db.WMS_Waste_Line.Where(x => x.Link_Head_ID == HeadID).ToList();
            if (Line_List.Where(x => x.Quantity <= 0).Any())
            {
                throw new Exception("存在产品报废数量小于等于零");
            }

            //需要发邮件
            ISentEmailService IS = new SentEmailService();
            IS.Sent_To_WMS_Staff_With_Comfirmed_WMS_Waste_Task(Head, Line_List);

            db.Entry(Head).State = EntityState.Modified;

            MyDbSave.SaveChange(db);
        }

        public void Refuse_WMS_Task_Waste(Guid HeadID, string Remark, User U)
        {
            if (string.IsNullOrEmpty(Remark)) { throw new Exception("未添加驳回理由"); }

            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status == WMS_Waste_Head_Status_Enum.已退回.ToString())
            {
                throw new Exception("报废单已退回，不支持重复操作");
            }

            if (Head.Status != WMS_Waste_Head_Status_Enum.待审核.ToString())
            {
                throw new Exception("报废单状态异常");
            }

            Head.Status = WMS_Waste_Head_Status_Enum.已退回.ToString();
            Head.Audit_DT = DateTime.Now;
            Head.Auditor = U.UserFullName;
            Head.Refuse_Remark = Remark.Trim();

            List<WMS_Waste_Line> Line_List = db.WMS_Waste_Line.Where(x => x.Link_Head_ID == HeadID).ToList();
            if (Line_List.Where(x => x.Quantity <= 0).Any())
            {
                throw new Exception("存在产品报废数量小于等于零");
            }

            //需要发邮件
            ISentEmailService IS = new SentEmailService();
            IS.Sent_To_WMS_Staff_With_Refused_WMS_Waste_Task(Head, Line_List);

            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Finish_WMS_Task_Waste(Guid HeadID)
        {
            WMS_Waste_Head Head = db.WMS_Waste_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Waste_Head is null"); }

            if (Head.Status == WMS_Waste_Head_Status_Enum.已消库.ToString())
            {
                throw new Exception("报废单已消库，不支持重复操作");
            }

            if (Head.Status != WMS_Waste_Head_Status_Enum.待执行.ToString())
            {
                throw new Exception("报废单状态异常");
            }

            Head.Status = WMS_Waste_Head_Status_Enum.已消库.ToString();
            Head.Finish_DT = DateTime.Now;

            List<WMS_Waste_Line> Line_List = db.WMS_Waste_Line.Where(x => x.Link_Head_ID == HeadID).ToList();
            if (Line_List.Where(x => x.Quantity <= 0).Any())
            {
                throw new Exception("存在产品报废数量小于等于零");
            }

            List<string> MatSn_List = Line_List.Select(x => x.MatSn).Distinct().ToList();
            List<string> Loc_List = Line_List.Select(x => x.Location).Distinct().ToList();

            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && Loc_List.Contains(x.Location)).ToList();
            Stock_List_DB = Stock_List_DB.Where(x => MatSn_List.Contains(x.MatSn)).ToList();

            List<WMS_Stock_Record> Record_List = new List<WMS_Stock_Record>();
            WMS_Stock_Record Record = new WMS_Stock_Record();

            List<WMS_Stock> Stock_List_DB_Sub = new List<WMS_Stock>();
            List<WMS_Stock> Stock_List = new List<WMS_Stock>();
            WMS_Stock Stock = new WMS_Stock();

            int Quantity = 0;
            DateTime DT = DateTime.Now;
            string WMS_Out_DT = DT.ToString("yyyy-MM-dd HH:mm");
            foreach (var x in Line_List)
            {
                Quantity = x.Quantity;
                Stock_List_DB_Sub = Stock_List_DB.Where(c => c.MatSn == x.MatSn && c.Location == x.Location).ToList();
                if (Stock_List_DB_Sub.Any() == false)
                {
                    throw new Exception("库存中不存在报废产品");
                }

                //报废记录
                Record = new WMS_Stock_Record();
                Record.Stock_ID = MyGUID.NewGUID();
                Record.WMS_Out_DT = WMS_Out_DT;
                Record.MatSn = x.MatSn;
                Stock = Stock_List_DB_Sub.FirstOrDefault();
                Record.MatName = Stock.MatName;
                Record.MatBrand = Stock.MatBrand;
                Record.Price = Stock.Price;
                Record.MatUnit = Stock.MatUnit;
                Record.Quantity = x.Quantity;
                Record.Location = x.Location;
                Record.Wms_Out_Head_ID = Head.Head_ID;
                Record.LinkMainCID = Head.LinkMainCID;
                Record.Remark = WMS_Stock_Record_Remark_Enum.报废出库.ToString();
                Record.Work_Person = Head.Create_Person;
                Record.Create_DT = DT;
                Record_List.Add(Record);

                //消库
                foreach (var xx in Stock_List_DB_Sub.OrderBy(c => c.Quantity))
                {
                    if (Quantity >= xx.Quantity && Quantity > 0)
                    {
                        Quantity = Quantity - xx.Quantity;
                        Stock_List.Add(xx);
                    }
                    else if (Quantity < xx.Quantity && Quantity > 0)
                    {
                        //更新原有库存
                        xx.Quantity = xx.Quantity - Quantity;
                        xx.WMS_Out_DT = WMS_Out_DT;
                        db.Entry(xx).State = EntityState.Modified;
                        break;
                    }
                }
            }

            if (Stock_List.Any())
            {
                db.WMS_Stock.RemoveRange(Stock_List);
            }

            db.WMS_Stock_Record.AddRange(Record_List);
            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

    }

    //盈亏审核
    public partial class WmsService : IWmsService
    {
        public PageList<WMS_Profit_Loss_Head> Get_WMS_Profit_Loss_Head_PageList(WMS_Profit_Loss_Filter MF)
        {
            var query = db.WMS_Profit_Loss_Head.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_No_Str))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_No_Str)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person.Contains(MF.Create_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Auditor))
            {
                query = query.Where(x => x.Auditor.Contains(MF.Auditor)).AsQueryable();
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

            PageList<WMS_Profit_Loss_Head> PList = new PageList<WMS_Profit_Loss_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> ID_List = PList.Rows.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Profit_Loss_Line> Line_List = db.WMS_Profit_Loss_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Profit_Loss_Line> Line_List_Sub = new List<WMS_Profit_Loss_Line>();
            foreach (var x in PList.Rows)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                if (Line_List_Sub.Any())
                {
                    x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                }
            }

            return PList;
        }

        public PageList<WMS_Profit_Loss_Head> Get_WMS_Profit_Loss_Head_PageList_Sub(WMS_Profit_Loss_Filter MF)
        {
            var query = db.WMS_Profit_Loss_Head.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            query = query.Where(x => x.Status != WMS_Profit_Loss_Head_Status_Enum.已执行.ToString() && x.Status != WMS_Profit_Loss_Head_Status_Enum.已退回.ToString()).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_No_Str))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_No_Str)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person.Contains(MF.Create_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Auditor))
            {
                query = query.Where(x => x.Auditor.Contains(MF.Auditor)).AsQueryable();
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

            PageList<WMS_Profit_Loss_Head> PList = new PageList<WMS_Profit_Loss_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> ID_List = PList.Rows.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Profit_Loss_Line> Line_List = db.WMS_Profit_Loss_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Profit_Loss_Line> Line_List_Sub = new List<WMS_Profit_Loss_Line>();
            foreach (var x in PList.Rows)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                if (Line_List_Sub.Any())
                {
                    x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                }
            }

            return PList;
        }

        public PageList<WMS_Profit_Loss_Head> Get_WMS_Profit_Loss_Head_PageList_Record(WMS_Profit_Loss_Filter MF)
        {
            var query = db.WMS_Profit_Loss_Head.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            query = query.Where(x => x.Status == WMS_Profit_Loss_Head_Status_Enum.已执行.ToString() || x.Status == WMS_Profit_Loss_Head_Status_Enum.已退回.ToString()).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Task_No_Str))
            {
                query = query.Where(x => x.Task_Bat_No_Str.Contains(MF.Task_No_Str)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Create_Person))
            {
                query = query.Where(x => x.Create_Person.Contains(MF.Create_Person)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Auditor))
            {
                query = query.Where(x => x.Auditor.Contains(MF.Auditor)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
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

            PageList<WMS_Profit_Loss_Head> PList = new PageList<WMS_Profit_Loss_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> ID_List = PList.Rows.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_Profit_Loss_Line> Line_List = db.WMS_Profit_Loss_Line.Where(x => ID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_Profit_Loss_Line> Line_List_Sub = new List<WMS_Profit_Loss_Line>();
            foreach (var x in PList.Rows)
            {
                Line_List_Sub = Line_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                if (Line_List_Sub.Any())
                {
                    x.MatSn_Count = Line_List_Sub.Select(c => c.MatSn).Distinct().Count();
                }
            }

            return PList;
        }

        public WMS_Profit_Loss_Head Get_WMS_Profit_Loss_Head_DB(Guid Head_ID)
        {
            WMS_Profit_Loss_Head Head = db.WMS_Profit_Loss_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Profit_Loss_Head() : Head;
            return Head;
        }

        public WMS_Profit_Loss_Head Get_WMS_Profit_Loss_Head_Item(Guid Head_ID)
        {
            WMS_Profit_Loss_Head Head = db.WMS_Profit_Loss_Head.Find(Head_ID);
            Head = Head == null ? new WMS_Profit_Loss_Head() : Head;
            Head.Line_List = db.WMS_Profit_Loss_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            Head.MatSn_Count = Head.Line_List.Select(x => x.MatSn).Distinct().Count();
            return Head;
        }

        private long Auto_Create_Task_Bat_WMS_Profit_Loss_Head(User U)
        {
            long Task_Bat_No_Min = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + "0001");
            long Task_Bat_No_Max = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + "9999");

            long Task_Bat_No = 0;
            if (db.WMS_Profit_Loss_Head.Where(x => x.LinkMainCID == U.LinkMainCID && x.Task_Bat_No >= Task_Bat_No_Min).Any())
            {
                Task_Bat_No = db.WMS_Profit_Loss_Head.Where(x => x.LinkMainCID == U.LinkMainCID).Max(x => x.Task_Bat_No) + 1;
            }
            else
            {
                Task_Bat_No = Task_Bat_No_Min;
            }

            if (Task_Bat_No > Task_Bat_No_Max)
            {
                throw new Exception("已经超过最大值，请联系管理人员");
            }
            return Task_Bat_No;
        }

        public void Confirm_WMS_Profit_Loss_Head(Guid HeadID, User U)
        {
            WMS_Profit_Loss_Head Head = db.WMS_Profit_Loss_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Profit_Loss_Head is null"); }

            if (Head.Status == WMS_Profit_Loss_Head_Status_Enum.已审核.ToString())
            {
                throw new Exception("盈亏审核单已审核，不支持重复操作");
            }

            if (Head.Status != WMS_Profit_Loss_Head_Status_Enum.待审核.ToString())
            {
                throw new Exception("盈亏审核单状态异常");
            }

            Head.Status = WMS_Profit_Loss_Head_Status_Enum.已审核.ToString();
            Head.Audit_DT = DateTime.Now;
            Head.Auditor = U.UserFullName;

            List<WMS_Profit_Loss_Line> Line_List = db.WMS_Profit_Loss_Line.Where(x => x.Link_Head_ID == HeadID).ToList();

            //需要发邮件
            ISentEmailService IS = new SentEmailService();
            IS.Sent_To_Manager_With_WMS_Profit_Loss_Head(Head, Line_List);

            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Confirm_WMS_Profit_Loss_Head_By_Accounting(Guid HeadID, User U)
        {
            WMS_Profit_Loss_Head Head = db.WMS_Profit_Loss_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Profit_Loss_Head is null"); }

            if (Head.Status == WMS_Profit_Loss_Head_Status_Enum.待执行.ToString())
            {
                throw new Exception("盈亏审核单等待仓库执行，不支持重复操作");
            }

            if (Head.Status != WMS_Profit_Loss_Head_Status_Enum.已审核.ToString())
            {
                throw new Exception("盈亏审核单状态异常");
            }

            Head.Status = WMS_Profit_Loss_Head_Status_Enum.待执行.ToString();
            Head.Approve_DT = DateTime.Now;
            Head.Approver = U.UserFullName;

            List<WMS_Profit_Loss_Line> Line_List = db.WMS_Profit_Loss_Line.Where(x => x.Link_Head_ID == HeadID).ToList();

            //需要发邮件
            ISentEmailService IS = new SentEmailService();
            IS.Sent_To_WMS_Staff_With_Comfirmed_WMS_Profit_Loss_Head(Head, Line_List);

            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Refuse_WMS_Profit_Loss_Head(Guid HeadID, string Remark, User U)
        {
            if (string.IsNullOrEmpty(Remark)) { throw new Exception("未添加驳回理由"); }

            WMS_Profit_Loss_Head Head = db.WMS_Profit_Loss_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Profit_Loss_Head is null"); }

            if (Head.Status == WMS_Profit_Loss_Head_Status_Enum.已退回.ToString())
            {
                throw new Exception("盈亏审核单已退回，不支持重复操作");
            }

            if (Head.Status != WMS_Profit_Loss_Head_Status_Enum.待审核.ToString())
            {
                throw new Exception("盈亏审核单状态异常");
            }

            Head.Status = WMS_Profit_Loss_Head_Status_Enum.已退回.ToString();
            Head.Audit_DT = DateTime.Now;
            Head.Auditor = U.UserFullName;
            Head.Refuse_Remark = Remark.Trim();

            List<WMS_Profit_Loss_Line> Line_List = db.WMS_Profit_Loss_Line.Where(x => x.Link_Head_ID == HeadID).ToList();

            //需要发邮件
            ISentEmailService IS = new SentEmailService();
            IS.Sent_To_WMS_Staff_With_Refused_WMS_Profit_Loss_Head(Head, Line_List);

            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Finish_WMS_Profit_Loss_Head(Guid HeadID)
        {
            WMS_Profit_Loss_Head Head = db.WMS_Profit_Loss_Head.Find(HeadID);
            if (Head == null) { throw new Exception("WMS_Profit_Loss_Head is null"); }

            if (Head.Status == WMS_Profit_Loss_Head_Status_Enum.已执行.ToString())
            {
                throw new Exception("盈亏审核单已执行，不支持重复操作");
            }

            if (Head.Status != WMS_Profit_Loss_Head_Status_Enum.待执行.ToString())
            {
                throw new Exception("盈亏审核单状态异常");
            }

            Head.Status = WMS_Profit_Loss_Head_Status_Enum.已执行.ToString();
            Head.Finish_DT = DateTime.Now;

            //更新库存
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(Head.Link_HeadID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }

            List<WMS_Stocktaking_Scan> Stocktaking_Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID).ToList();

            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Location == Task.Location).ToList();
            WMS_Stock Stock_DB = new WMS_Stock();

            WMS_Stock Stock_DB_Null = new WMS_Stock();
            Stock_DB_Null.WMS_In_DT = DateTime.Now.ToString("yyyy-MM-dd");
            Stock_DB_Null.MatName = string.Empty;
            Stock_DB_Null.MatUnit = "PCS";
            Stock_DB_Null.Price = 0;
            Stock_DB_Null.Wms_In_Head_ID = Guid.Empty;
            Stock_DB_Null.LinkMainCID = Task.LinkMainCID;

            List<WMS_Stock> Stock_List = new List<WMS_Stock>();
            WMS_Stock Stock = new WMS_Stock();
            foreach (var x in Stocktaking_Scan_List.OrderBy(x => x.Create_DT).ToList())
            {
                Stock_DB = Stock_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                if (Stock_DB == null)
                {
                    Stock_DB = Stock_DB_Null;
                }

                Stock = new WMS_Stock();
                Stock.Stock_ID = MyGUID.NewGUID();
                Stock.Quantity = x.Scan_Quantity;
                Stock.Package = x.Package_Type;
                Stock.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                Stock.Location = Task.Location;
                Stock.WMS_In_DT = Stock_DB.WMS_In_DT;
                Stock.MatSn = x.MatSn;
                Stock.MatName = Stock_DB.MatName;
                Stock.MatUnit = Stock_DB.MatUnit;
                Stock.MatBrand = Stock_DB.MatBrand;
                Stock.Price = Stock_DB.Price;
                Stock.Wms_In_Head_ID = Stock_DB.Wms_In_Head_ID;
                Stock.LinkMainCID = Stock_DB.LinkMainCID;
                Stock_List.Add(Stock);
            }

            db.WMS_Stock.RemoveRange(Stock_List_DB);
            db.WMS_Stock.AddRange(Stock_List);

            //生成盈亏记录
            List<WMS_Profit_Loss_Line> Line_List = db.WMS_Profit_Loss_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();
            List<WMS_Profit_Loss> Record_List = new List<WMS_Profit_Loss>();
            WMS_Profit_Loss Record = new WMS_Profit_Loss();
            
            DateTime DT = DateTime.Now;
            foreach (var x in Line_List)
            {
                Record = new WMS_Profit_Loss();
                Record.Line_ID = MyGUID.NewGUID();
                Record.MatSn = x.MatSn;
                Record.MatBrand = x.MatBrand;
                Record.MatName = x.MatName;
                Record.MatUnit = x.MatUnit;
                Record.Old_Quantity = x.Old_Quantity;
                Record.New_Quantity = x.New_Quantity;
                Record.Price = x.Unit_Price;
                Record.Location = x.Location;
                Record.Status = WMS_Profit_Loss_Status_Enum.已确定.ToString();
                Record.Work_Person = Task.Work_Person;
                Record.Create_DT = DT;
                Record.Link_TaskID = Task.Task_ID;
                Record.LinkMainCID = Task.LinkMainCID;
                Record_List.Add(Record);
            }

            db.WMS_Profit_Loss.AddRange(Record_List);

            db.Entry(Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public string Get_WMS_Profit_Loss_Line_By_Head_To_Excel(List<WMS_Profit_Loss_Line> PL_List)
        {
            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            
            TableHeads.Add("产品型号");
            TableHeads.Add("品牌");
            TableHeads.Add("库位");
            TableHeads.Add("调整前");
            TableHeads.Add("调整后");
            TableHeads.Add("差异数");
            TableHeads.Add("差异金额");
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
            foreach (var x in PL_List.OrderBy(x => x.MatSn).ToList())
            {
                x.Diff_Quantity = x.New_Quantity - x.Old_Quantity;
                x.Total_Price = x.Diff_Quantity * x.Unit_Price;

                newRow = DT.NewRow();
                newRow["产品型号"] = x.MatSn;
                newRow["品牌"] = x.MatBrand;
                newRow["库位"] = x.Location;
                newRow["调整前"] = x.Old_Quantity.ToString("N0");
                newRow["调整后"] = x.New_Quantity.ToString("N0");
                newRow["差异数"] = x.Diff_Quantity.ToString("N0");
                newRow["差异金额"] = x.Total_Price.ToString("N4");
                DT.Rows.Add(newRow);
            }

            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }
    }
}

//扫描枪交互
namespace SMART.Api
{
    public partial interface IWmsService
    {
        //盘库扫描
        DataTable WMS_Stock_Task_List(Guid MainCID);
        DataTable WMS_Stock_Task_Scan_List(string Task_ID);
        void WMS_Stock_Task_List_Create(Guid MainCID, string Scan_Source);
        void WMS_Stock_Task_Scan_List_Create(string Task_ID, string Scan_Source);
        DataTable WMS_Stock_Task_Scan_List_Other(string Task_ID);
        void WMS_Stock_Task_Scan_List_Create_Other(string Task_ID, string Scan_Source, string Quantity);

        //首次盘库
        DataTable WMS_Stock_Task_List_First(Guid MainCID);
        DataTable WMS_Stock_Task_Scan_List_First(string Task_ID);
        void WMS_Stock_Task_List_Create_First(Guid MainCID, string Scan_Source);
        void WMS_Stock_Task_Scan_List_Create_First(string Task_ID, string Scan_Source);
        DataTable WMS_Stock_Task_Scan_List_Other_First(string Task_ID);
        void WMS_Stock_Task_Scan_List_Create_Other_First(string Task_ID, string Scan_Source, string Quantity);
    }

    //盘库扫描
    public partial class WmsService : IWmsService
    {
        public DataTable WMS_Stock_Task_List(Guid MainCID)
        {
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.PageIndex = 1;
            MF.PageSize = 10000;
            MF.LinkMainCID = MainCID;
            MF.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            MF.Property = WMS_Stock_Task_Property_Enum.日常盘库.ToString();
            List<WMS_Stock_Task> List = Get_WMS_Stock_Task_PageList(MF).Rows;

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("库位");
            dt.Columns.Add("型号数");
            dt.Columns.Add("产品数");
            dt.Columns.Add("作业人");
            foreach (var x in List)
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Task_ID;
                dr["库位"] = x.Location;
                dr["型号数"] = x.MatSn_Count;
                dr["产品数"] = x.Quantity_Sum;
                dr["作业人"] = x.Work_Person;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //创建盘库任务
        public void WMS_Stock_Task_List_Create(Guid MainCID, string Scan_Source)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未扫描库位"); }

            WMS_Location Loc = db.WMS_Location.Where(x => x.LinkMainCID == MainCID && x.Location == Scan_Source).FirstOrDefault();
            if (Loc == null) { throw new Exception("系统中不存在该库位"); }

            WMS_Stock_Task Task = new WMS_Stock_Task();
            Task.Task_ID = MyGUID.NewGUID();
            Task.LinkMainCID = MainCID;
            Task.Create_DT = DateTime.Now;
            Task.Location = Loc.Location;
            Task.Type = Loc.Type;
            Task.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            Task.Property = WMS_Stock_Task_Property_Enum.日常盘库.ToString();
            db.WMS_Stock_Task.Add(Task);

            if (db.WMS_Stock_Task.Where(x => x.LinkMainCID == MainCID && x.Status == WMS_Stock_Task_Enum.未盘库.ToString() && x.Location == Task.Location).Any())
            {
                throw new Exception("该库位存在未盘库任务");
            }

            //存在移库任务或配货任务，不允许盘库
            if (db.WMS_Move.Where(x => x.Out_Location == Task.Location && x.Move_Status == WMS_Move_Status_Enum.待移库.ToString()).Any())
            {
                throw new Exception("该库位存在移库任务，不支持盘库");
            }

            //创建底盘信息
            List<WMS_Stocktaking> Stocktaking_List = new List<WMS_Stocktaking>();
            WMS_Stocktaking Stocktaking = new WMS_Stocktaking();

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == MainCID && x.Location == Scan_Source).ToList();

            DateTime Create_DT = DateTime.Now;
            foreach (var x in Stock_List)
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
                Stocktaking.Task_Bat_No = "";
                Stocktaking.Work_Person = Task.Work_Person;
                Stocktaking.Status = WMS_Stocktaking_Status_Enum.待底盘.ToString();
                Stocktaking_List.Add(Stocktaking);
            }

            if (Stocktaking_List.Any())
            {
                db.WMS_Stocktaking.AddRange(Stocktaking_List);
            }
            MyDbSave.SaveChange(db);
        }

        public DataTable WMS_Stock_Task_Scan_List(string Task_ID)
        {
            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("盘库任务不存在"); }
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

        //盘库扫描
        public void WMS_Stock_Task_Scan_List_Create(string Task_ID, string Scan_Source)
        {
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("盘库任务不存在"); }

            if (string.IsNullOrEmpty(Task.Work_Person)) { throw new Exception("未设置盘库作业人"); }

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

        public DataTable WMS_Stock_Task_Scan_List_Other(string Task_ID)
        {
            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }
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

        //盘库扫描(端数)
        public void WMS_Stock_Task_Scan_List_Create_Other(string Task_ID, string Scan_Source, string Quantity)
        {
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("盘库任务不存在"); }

            if (string.IsNullOrEmpty(Task.Work_Person)) { throw new Exception("未设置盘库作业人"); }

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

    //首次盘库
    public partial class WmsService : IWmsService
    {
        public DataTable WMS_Stock_Task_List_First(Guid MainCID)
        {
            WMS_Stock_Filter MF = new WMS_Stock_Filter();
            MF.PageIndex = 1;
            MF.PageSize = 10000;
            MF.LinkMainCID = MainCID;
            MF.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            MF.Property = WMS_Stock_Task_Property_Enum.首次盘库.ToString();
            List<WMS_Stock_Task> List = Get_WMS_Stock_Task_PageList(MF).Rows;

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("GUID");
            dt.Columns.Add("库位");
            dt.Columns.Add("型号数");
            dt.Columns.Add("产品数");
            foreach (var x in List.OrderByDescending(x => x.Create_DT).ToList())
            {
                DataRow dr = dt.NewRow();
                dr["GUID"] = x.Task_ID;
                dr["库位"] = x.Location;
                dr["型号数"] = x.MatSn_Count;
                dr["产品数"] = x.Quantity_Sum;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //创建盘库任务
        public void WMS_Stock_Task_List_Create_First(Guid MainCID, string Scan_Source)
        {
            Scan_Source = Scan_Source == null ? string.Empty : Scan_Source.Trim();
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未扫描库位"); }

            if (db.WMS_Location.Where(x => x.LinkMainCID == MainCID && x.Location == Scan_Source).Any() == false) { throw new Exception("系统中不存在该库位"); }

            WMS_Stock_Task Task = new WMS_Stock_Task();
            Task.Task_ID = MyGUID.NewGUID();
            Task.LinkMainCID = MainCID;
            Task.Create_DT = DateTime.Now;
            Task.Location = Scan_Source;
            Task.Status = WMS_Stock_Task_Enum.未盘库.ToString();
            Task.Property = WMS_Stock_Task_Property_Enum.首次盘库.ToString();
            Task.Work_Person = "首次盘库";
            db.WMS_Stock_Task.Add(Task);

            if (db.WMS_Stock.Where(x => x.LinkMainCID == MainCID && x.Location == Scan_Source).Any())
            {
                throw new Exception("该库位存在产品，不支持首次盘库");
            }

            if (db.WMS_Stock_Task.Where(x => x.LinkMainCID == MainCID && x.Status == WMS_Stock_Task_Enum.未盘库.ToString() && x.Location == Task.Location).Any())
            {
                throw new Exception("该库位存在未盘库任务");
            }

            MyDbSave.SaveChange(db);
        }

        public DataTable WMS_Stock_Task_Scan_List_First(string Task_ID)
        {
            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }
            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == TaskID && x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).ToList();

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序");
            dt.Columns.Add("型号");
            dt.Columns.Add("数量");
            int i = 0;
            foreach (var x in Scan_List.OrderByDescending(x => x.Create_DT).ToList())
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

        //盘库扫描
        public void WMS_Stock_Task_Scan_List_Create_First(string Task_ID, string Scan_Source)
        {
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("盘库任务不存在"); }

            if (string.IsNullOrEmpty(Task.Work_Person)) { throw new Exception("未设置盘库作业人"); }

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

            //执行解码
            List<string> Line_MatSn_List = new List<string>();
            Decode_Scan De_Scan = new Decode_Scan();
            List<string> Brand_List = new List<string>();
            Brand_List.Add(Decode_Scan_Brand.NMB.ToString());
            Brand_List.Add(Decode_Scan_Brand.NSK.ToString());
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

            De_Scan = this.Decode_Scan_Source(MatBrand, Scan.Scan_Source, Line_MatSn_List);
            Scan.MatBrand = MatBrand;
            Scan.MatSn = De_Scan.Decode_MatSn;
            Scan.Scan_Quantity = De_Scan.Decode_Scan_Quantity;

            ////验证是否系统中存在此型号
            //if (db.Material.Where(x => x.LinkMainCID == Task.LinkMainCID && x.MatBrand == Scan.MatBrand && x.MatSn == Scan.MatSn).Any() == false)
            //{
            //    throw new Exception("系统中不存在扫描型号");
            //}

            db.WMS_Stocktaking_Scan.Add(Scan);
            MyDbSave.SaveChange(db);
        }

        public DataTable WMS_Stock_Task_Scan_List_Other_First(string Task_ID)
        {
            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("WMS_Stock_Task is null"); }
            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == TaskID && x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).ToList();

            DataTable dt = new DataTable("DT");
            dt.Columns.Add("序");
            dt.Columns.Add("型号");
            dt.Columns.Add("数量");
            int i = 0;
            foreach (var x in Scan_List.OrderByDescending(x => x.Create_DT).ToList())
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

        //盘库扫描(端数)
        public void WMS_Stock_Task_Scan_List_Create_Other_First(string Task_ID, string Scan_Source, string Quantity)
        {
            if (string.IsNullOrEmpty(Scan_Source)) { throw new Exception("未获取扫码内容"); }

            Guid TaskID = Guid.NewGuid();
            try { TaskID = new Guid(Task_ID); } catch { }
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("盘库任务不存在"); }

            if (string.IsNullOrEmpty(Task.Work_Person)) { throw new Exception("未设置盘库作业人"); }

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

            Scan.MatSn = Scan.Scan_Source.Trim();

            ////验证是否系统中存在此型号
            //if (db.Material.Where(x => x.LinkMainCID == Task.LinkMainCID && x.MatSn == Scan.MatSn).Any() == false)
            //{
            //    throw new Exception("系统中不存在扫描的型号");
            //}

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
}
