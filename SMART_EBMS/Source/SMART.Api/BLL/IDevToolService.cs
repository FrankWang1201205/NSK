using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;
using System.Data.Entity;

namespace SMART.Api
{
    public interface IDevToolService
    {
        void Clean_Material();
        void Clean_WMS_In();
        void Set_Tracking_Info();
        void Set_Supplier_Name();
        void Create_WMS_Move(Guid MainCID);
        void Refresh_WMS_Stock(Guid MainCID);
        void Refresh_WMS_Stock_Record(Guid MainCID);

        void Add_Out_In_Record_From_First_Stocktaking(Guid LinkMainCID);
        void Reset_Out_In_Record_From_WMS_In(Guid LinkMainCID);
        void Reset_WMS_Stock_Task_Type(Guid LinkMainCID);
    }

    public partial class DevToolService : IDevToolService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class DevToolService : IDevToolService
    {
        public void Clean_Material()
        {
            db.BulkDelete(db.Material.ToList());
            db.BulkDelete(db.Material_CODE.ToList());
            db.BulkDelete(db.MatImage.ToList());
            db.BulkDelete(db.MatImage_Detail.ToList());
            db.BulkDelete(db.Mat_Excel.ToList());
            db.BulkDelete(db.Mat_Excel_Line.ToList());
            db.BulkSaveChanges();
        }

        public void Clean_WMS_In()
        {
            db.WMS_In_Head.RemoveRange(db.WMS_In_Head.ToList());
            db.WMS_In_Line.RemoveRange(db.WMS_In_Line.ToList());
            db.WMS_Track.RemoveRange(db.WMS_Track.ToList());
            db.WMS_In_Scan.RemoveRange(db.WMS_In_Scan.ToList());
            db.WMS_Stock.RemoveRange(db.WMS_Stock.ToList());
            MyDbSave.SaveChange(db);
        }

        public void Set_Tracking_Info()
        {
            List<WMS_Track> List = db.WMS_Track.ToList();
            List<Guid> Guid_List = List.Select(x => x.Link_Head_ID).Distinct().ToList();
            List<WMS_In_Head> Head_List = db.WMS_In_Head.Where(x => Guid_List.Contains(x.Head_ID)).ToList();
            foreach (var x in List)
            {
                x.Tracking_Type = Tracking_Type_Enum.收货.ToString();
                x.Link_Head_Com_Name = Head_List.Where(c => c.Head_ID == x.Link_Head_ID).FirstOrDefault().Supplier_Name;
                db.Entry(x).State = System.Data.Entity.EntityState.Modified;
            }

            MyDbSave.SaveChange(db);
        }

        public void Set_Supplier_Name()
        {
            List<WMS_In_Head> Head_List = db.WMS_In_Head.Where(x => x.MatType == WMS_In_Type_Enum.常规期货.ToString()).ToList();
            foreach (var x in Head_List)
            {
                x.Supplier_Name = x.Brand;
                db.Entry(x).State = System.Data.Entity.EntityState.Modified;
            }

            List<Guid> Guid_List = Head_List.Select(x => x.Head_ID).Distinct().ToList();
            List<WMS_In_Line> Line_List = db.WMS_In_Line.Where(x => Guid_List.Contains(x.Link_Head_ID)).ToList();

            foreach (var x in Line_List)
            {
                x.Supplier_Name = x.MatBrand;
                db.Entry(x).State = System.Data.Entity.EntityState.Modified;
            }

            MyDbSave.SaveChange(db);
        }

        public void Create_WMS_Move(Guid MainCID)
        {
            List<WMS_Move> Move_List_DB = db.WMS_Move.Where(x => x.LinkMainCID == MainCID).ToList();
            db.WMS_Move.RemoveRange(Move_List_DB);

            List<WMS_Stock> Stock_List = db.WMS_Stock.Where(x => x.LinkMainCID == MainCID && x.Location_Type == WMS_Stock_Location_Type_Enum.临时库位.ToString()).ToList();
            List<string> Location_List = Stock_List.Select(x => x.Location).Distinct().ToList();
            List<Guid> ID_List_DB = Stock_List.Select(x => x.Wms_In_Head_ID).Distinct().ToList();
            List<WMS_In_Head> Head_list = db.WMS_In_Head.Where(x => ID_List_DB.Contains(x.Head_ID)).ToList();

            WMS_In_Head Head = new WMS_In_Head();

            WMS_Move Move = new WMS_Move();
            List<WMS_Move> Move_List = new List<WMS_Move>();

            foreach (var Location in Location_List)
            {
                foreach (var ID in Stock_List.Where(x => x.Location == Location).Select(x => x.Wms_In_Head_ID).Distinct().ToList())
                {
                    Move = new WMS_Move();
                    Head = Head_list.Where(c => c.Head_ID == ID).FirstOrDefault();
                    Head = Head == null ? new WMS_In_Head() : Head;
                    if (Head == null)
                    {
                        throw new Exception("Head is null");
                    }
                    Move.Move_ID = MyGUID.NewGUID();
                    Move.Out_Location = Location;
                    Move.Task_Bat_No = Head.Task_Bat_No_Str;
                    Move.Supplier_Name = Head.Supplier_Name;
                    Move.Create_DT = DateTime.Now;
                    Move.Move_Status = WMS_Move_Status_Enum.待移库.ToString();
                    Move.LinkMainCID = Head.LinkMainCID;
                    Move.Link_HeadID = Head.Head_ID;
                    Move_List.Add(Move);
                }
            }

            db.WMS_Move.AddRange(Move_List);
            MyDbSave.SaveChange(db);
        }

        public void Refresh_WMS_Stock(Guid MainCID)
        {
            List<Material> Mat_List_DB = db.Material.Where(x => x.LinkMainCID == MainCID).ToList();
            List<WMS_Stock> Stock_List_DB = db.WMS_Stock.Where(x => x.LinkMainCID == MainCID).ToList();
            Material Mat = new Material();
            foreach (var x in Stock_List_DB)
            {
                Mat = Mat_List_DB.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                Mat = Mat == null ? new Material() : Mat;

                x.MatBrand = Mat.MatBrand;
                x.MatName = Mat.MatName;
            }

            MyDbSave.SaveChange(db);
        }

        public void Refresh_WMS_Stock_Record(Guid MainCID)
        {
            List<WMS_Stock_Record> Record_List = db.WMS_Stock_Record.Where(x => x.LinkMainCID == MainCID).ToList();
            List<Guid> In_ID = Record_List.Where(x => x.Wms_In_Head_ID != Guid.Empty).Select(x => x.Wms_In_Head_ID).Distinct().ToList();
            List<Guid> Out_ID = Record_List.Where(x => x.Wms_In_Head_ID == Guid.Empty).Select(x => x.Wms_Out_Head_ID).Distinct().ToList();
            List<WMS_In_Head> In_Head_List = db.WMS_In_Head.Where(x => In_ID.Contains(x.Head_ID)).ToList();
            List<WMS_Out_Head> Out_Head_List = db.WMS_Out_Head.Where(x => Out_ID.Contains(x.Head_ID)).ToList();
            WMS_In_Head In_Head = new WMS_In_Head();
            WMS_Out_Head Out_Head = new WMS_Out_Head();
            foreach (var x in Record_List)
            {
                if (x.Wms_In_Head_ID != Guid.Empty)
                {
                    In_Head = In_Head_List.Where(c => c.Head_ID == x.Wms_In_Head_ID).FirstOrDefault();
                    In_Head = In_Head == null ? new WMS_In_Head() : In_Head;
                    x.Work_Person = In_Head.Work_Person;
                }
                else if (x.Wms_In_Head_ID == Guid.Empty)
                {
                    Out_Head = Out_Head_List.Where(c => c.Head_ID == x.Wms_Out_Head_ID).FirstOrDefault();
                    Out_Head = Out_Head == null ? new WMS_Out_Head() : Out_Head;

                    if (Out_Head.Logistics_Mode == Logistics_Out_Mode_Enum.自提.ToString())
                    {
                        x.Work_Person = Out_Head.Work_Down_Person;
                    }
                    else
                    {
                        x.Work_Person = Out_Head.Work_Out_Person;
                    }
                }
            }

            MyDbSave.SaveChange(db);
        }
    }

    public partial class DevToolService : IDevToolService
    {
        //首次入库转为入库记录
        public void Add_Out_In_Record_From_First_Stocktaking(Guid LinkMainCID)
        {
            List<WMS_Stock_Task> Stock_Task_List = db.WMS_Stock_Task.Where(x => x.LinkMainCID == LinkMainCID && x.Property == WMS_Stock_Task_Property_Enum.首次盘库.ToString() && x.Status == WMS_Stock_Task_Enum.已盘库.ToString()).ToList();
            List<Guid> Stock_Task_ID_List = Stock_Task_List.Select(x => x.Task_ID).Distinct().ToList();
            List<WMS_Stocktaking_Scan> Taking_Scan_List = db.WMS_Stocktaking_Scan.Where(x => Stock_Task_ID_List.Contains(x.Link_TaskID)).ToList();

            List<WMS_Stock_Record> Record_List = new List<WMS_Stock_Record>();
            WMS_Stock_Record Record = new WMS_Stock_Record();

            foreach (var x in Taking_Scan_List)
            {
                Record = new WMS_Stock_Record();
                Record.Stock_ID = x.Scan_ID;
                Record.Create_DT = x.Create_DT;
                Record.MatSn = x.MatSn;
                Record.MatBrand = x.MatBrand;
                Record.MatUnit = "PCS";
                Record.Quantity = x.Scan_Quantity;
                Record.Package = x.Package_Type;
                Record.Location_Type = WMS_Stock_Location_Type_Enum.标准库位.ToString();
                Record.Location = x.Location;
                Record.Wms_In_Head_ID = x.Link_TaskID;
                Record.LinkMainCID = x.LinkMainCID;
                Record.Remark = WMS_Stock_Record_Remark_Enum.首次盘库.ToString();
                Record.Work_Person = "首次盘库";
                Record_List.Add(Record);
            }

            db.WMS_Stock_Record.AddRange(Record_List);
            MyDbSave.SaveChange(db);
        }

        //反刷未写入出入库记录的入库订单
        public void Reset_Out_In_Record_From_WMS_In(Guid LinkMainCID)
        {
            List<WMS_In_Head> Head_List = db.WMS_In_Head.Where(x => x.LinkMainCID == LinkMainCID && x.Status == WMS_In_Global_State_Enum.完成入库.ToString()).ToList();
            List<Guid> HeadID_List_DB = Head_List.Select(x => x.Head_ID).Distinct().ToList();
            List<Guid> RecordID_List = db.WMS_Stock_Record.Where(x => x.Remark == WMS_Stock_Record_Remark_Enum.订单入库.ToString()).Select(x => x.Wms_In_Head_ID).Distinct().ToList();

            List<Guid> HeadID_List = new List<Guid>();
            foreach (var ID in HeadID_List_DB)
            {
                if (RecordID_List.Where(c => c == ID).Any() == false)
                {
                    HeadID_List.Add(ID);
                }
            }

            List<WMS_In_Scan> Scan_List = db.WMS_In_Scan.Where(x => HeadID_List.Contains(x.Link_Head_ID)).ToList();
            List<WMS_In_Scan> Scan_List_Sub = new List<WMS_In_Scan>();

            List<WMS_Stock_Record> Stock_Record_List = new List<WMS_Stock_Record>();
            WMS_Stock_Record S_Record = new WMS_Stock_Record();

            List<WMS_In_Line> Line_List = db.WMS_In_Line.Where(x => HeadID_List.Contains(x.Link_Head_ID)).ToList();
            WMS_In_Line Line = new WMS_In_Line();
            WMS_In_Head Head = new WMS_In_Head();
            foreach (var ID in HeadID_List)
            {
                Head = Head_List.Where(c => c.Head_ID == ID).FirstOrDefault();
                Scan_List_Sub = Scan_List.Where(c => c.Link_Head_ID == ID).ToList();
                foreach (var x in Scan_List_Sub)
                {
                    Line = Line_List.Where(c => c.Link_Head_ID == ID && c.MatSn == x.MatSn).FirstOrDefault();
                    Line = Line == null ? new WMS_In_Line() : Line;

                    S_Record = new WMS_Stock_Record();
                    S_Record.Stock_ID = MyGUID.NewGUID();
                    S_Record.Quantity = x.Scan_Quantity;
                    S_Record.Package = x.Package_Type;
                    S_Record.Location_Type = WMS_Stock_Location_Type_Enum.临时库位.ToString();
                    S_Record.Location = x.Tray_No;
                    S_Record.Create_DT = Line.Delivery_DT;
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

            }

            if (Stock_Record_List.Any())
            {
                db.WMS_Stock_Record.AddRange(Stock_Record_List);
                MyDbSave.SaveChange(db);
            }

        }

        //反刷盘库任务类型
        public void Reset_WMS_Stock_Task_Type(Guid LinkMainCID)
        {
            List<WMS_Stock_Task> Task_List = db.WMS_Stock_Task.Where(x => x.LinkMainCID == LinkMainCID).ToList();
            List<string> Loc_Str_List = Task_List.Select(x => x.Location).Distinct().ToList();
            List<WMS_Location> Loc_List = db.WMS_Location.Where(x => x.LinkMainCID == LinkMainCID&& Loc_Str_List.Contains(x.Location)).ToList();
            WMS_Location Loc = new WMS_Location();
            foreach (var x in Task_List)
            {
                Loc = Loc_List.Where(c => c.Location == x.Location).FirstOrDefault();
                Loc = Loc == null ? new WMS_Location() : Loc;
                x.Type = Loc.Type;
                db.Entry(x).State = EntityState.Modified;
            }

            MyDbSave.SaveChange(db);
        }
    }
}
