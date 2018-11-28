using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Newtonsoft.Json;
using SMART.Api.Models;
using System.Web;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using System.Threading;
using System.Data;

namespace SMART.Api
{
    public partial interface IRFQService
    {
        RFQ_Head Get_RFQ_Head_DB(Guid RID);
        RFQ_Head Get_RFQ_Head_Item(Guid RID);
        List<RFQ_Head_Line> Get_RFQ_Line_List(Guid RID);
        PageList<RFQ_Head> Get_RFQ_Head_PageList(RFQ_Head_Filter MF);
        PageList<RFQ_Head> Get_RFQ_Head_By_Wait_PageList(RFQ_Head_Filter MF);
        RFQ_Head_Line Get_RFQ_Line_By_Line_ID(Guid Line_ID);

        Guid Create_RFQ(RFQ_Head Head);
        void Set_RFQ_Head_Base(Guid RID, RFQ_Head Head);
        void Delete_RFQ_Item(Guid RID);

        void Create_RFQ_Line(Guid RID, RFQ_Head_Line Line);
        void Delete_RFQ_Line(Guid Line_ID);
        void Set_RFQ_Line(Guid Line_ID, RFQ_Head_Line Line);
        void Set_RFQ_Line_More(Guid Line_ID, RFQ_Head_Line Line);
        string RFQ_Excel_Template();
        void RFQ_Excel_Template_Upload(HttpPostedFileBase ExcelFile, Guid RID);
        void RFQ_Sent_To_Sales(Guid RID);

        void Set_RFQ_Line_MatID(Guid Line_ID, Guid MatID);
        void Set_RFQ_Line_MatID_Cancel(Guid Line_ID);
    }

    public partial class RFQService : IRFQService { SmartdbContext db = new SmartdbContext(); }

    //新增询价
    public partial class RFQService : IRFQService
    {
        public PageList<RFQ_Head> Get_RFQ_Head_By_Wait_PageList(RFQ_Head_Filter MF)
        {
            var query = db.RFQ_Head.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Status == RFQ_Status_Enum.待发送.ToString()).AsQueryable();
            if (!string.IsNullOrEmpty(MF.Cust_Name))
            {
                query = query.Where(x => x.Cust_Name.Contains(MF.Cust_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.RFQ_No))
            {
                query = query.Where(x => x.RFQ_No.Contains(MF.RFQ_No)).AsQueryable();
            }

            PageList<RFQ_Head> PList = new PageList<RFQ_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> RIDList = PList.Rows.Select(x => x.RID).ToList();
            List<RFQ_Head_Line> LineList = db.RFQ_Head_Line.Where(x => RIDList.Contains(x.Link_RID)).ToList();
            foreach (var x in PList.Rows)
            {
                x.Line_List = LineList.Where(c => c.Link_RID == x.RID).ToList();
            }
            return PList;
        }

        public Guid Create_RFQ(RFQ_Head Head)
        {
            User Ass_U = db.User.Find(Head.Ass_UID);
            Head.LinkMainCID = Ass_U.LinkMainCID;
            Head.Ass_UID = Ass_U.UID;
            Head.Ass_Full_Name = Ass_U.UserFullName;
            Head.Cust_Name = Head.Cust_Name.Trim();
            Head.Compete_Brand = Head.Compete_Brand == null ? string.Empty : Head.Compete_Brand.Trim();
            Head.Compete_Company = Head.Compete_Company == null ? string.Empty : Head.Compete_Company.Trim();
            Head.Buyer = Head.Buyer == null ? string.Empty : Head.Buyer.Trim();
            Head.Buyer_Tel = Head.Buyer_Tel == null ? string.Empty : Head.Buyer_Tel.Trim();
            Head.Buyer_Fax = Head.Buyer_Fax == null ? string.Empty : Head.Buyer_Fax.Trim();
            Head.Buyer_Mail = Head.Buyer_Mail == null ? string.Empty : Head.Buyer_Mail.Trim();

            Customer C = this.Check_RFQ_Customer(Head);
            Head.RID = MyGUID.NewGUID();
            Head.Create_DT = DateTime.Now;

            Head.Status = RFQ_Status_Enum.待发送.ToString();
            Head.RFQ_No = this.Auto_Get_RFQ_No(C.LinkMainCID);
            Head.CID = C.CID;
            Head.C_Settle_Accounts = C.Settle_Accounts;
            Head.C_VIP_Type = C.VIP_Type;


            Head.RFQ_Remark = Head.RFQ_Remark == null ? string.Empty : Head.RFQ_Remark.Trim();
            User Sales_U = db.User.Find(C.Sales_UID);
            Sales_U = Sales_U == null ? new User() : Sales_U;
            Head.UID = Sales_U.UID;
            Head.UID_FullName = Sales_U.UserFullName;
            Head.UID_Tel = Sales_U.Tel;
            Head.UID_Email = Sales_U.Email;

            db.RFQ_Head.Add(Head);
            MyDbSave.SaveChange(db);
            return Head.RID;
        }

        private Customer Check_RFQ_Customer(RFQ_Head Head)
        {
            Customer C = db.Customer.Where(x => x.Cust_Name == Head.Cust_Name && x.LinkMainCID == Head.LinkMainCID).FirstOrDefault();
            if (C == null)
            {
                C = new Customer();
                C.CID = MyGUID.NewGUID();
                C.Cust_Name = Head.Cust_Name;
                C.FormBy = Head.C_FormBy;
                C.Industry = Head.C_Industry;
                C.Compete_Brand = Head.Compete_Brand;
                C.Compete_Company = Head.Compete_Company;
                C.Create_Person = Head.Ass_Full_Name;
                C.VIP_Type = VIP_Type_Enum.NEW.ToString();

                C.Buyer = Head.Buyer;
                C.Buyer_Tel = Head.Buyer_Tel;
                C.Buyer_Mail = Head.Buyer_Mail;
                C.Buyer_Fax = Head.Buyer_Fax;
                C.LinkMainCID = Head.LinkMainCID;
                db.Customer.Add(C);
            }
            return C;
        }

        public string Auto_Get_RFQ_No(Guid LineMainCID)
        {
            //初始化数字编码
            string SysIntNo = string.Empty;

            //最大系统编码号
            IMaxIntService IMax = new MaxIntService();
            MaxInt Max = IMax.GetMaxIntByApp(MaxIntAppName.RFQ_Head.ToString());

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
            string NewSysNo = Max.PreCode + DateTime.Now.ToString("yyyyMMdd") + SysIntNo;
            return NewSysNo;
        }

        public void Set_RFQ_Head_Base(Guid RID, RFQ_Head Head)
        {
            RFQ_Head OLD_Head = db.RFQ_Head.Find(RID);

            OLD_Head.C_Settle_Accounts = Head.C_Settle_Accounts == null ? string.Empty : Head.C_Settle_Accounts.Trim();
            OLD_Head.C_Industry = Head.C_Industry == null ? string.Empty : Head.C_Industry.Trim();
            OLD_Head.C_FormBy = Head.C_FormBy == null ? string.Empty : Head.C_FormBy.Trim();
            OLD_Head.Compete_Brand = Head.Compete_Brand == null ? string.Empty : Head.Compete_Brand.Trim();
            OLD_Head.Compete_Company = Head.Compete_Company == null ? string.Empty : Head.Compete_Company.Trim();

            OLD_Head.RFQ_Remark = Head.RFQ_Remark == null ? string.Empty : Head.RFQ_Remark.Trim();

            OLD_Head.Buyer = Head.Buyer == null ? string.Empty : Head.Buyer.Trim();
            OLD_Head.Buyer_Tel = Head.Buyer_Tel == null ? string.Empty : Head.Buyer_Tel.Trim();
            OLD_Head.Buyer_Fax = Head.Buyer_Fax == null ? string.Empty : Head.Buyer_Fax.Trim();
            OLD_Head.Buyer_Mail = Head.Buyer_Mail == null ? string.Empty : Head.Buyer_Mail.Trim();

            OLD_Head.UID_FullName = Head.UID_FullName == null ? string.Empty : Head.UID_FullName.Trim();
            OLD_Head.UID_Fax = Head.UID_Fax == null ? string.Empty : Head.UID_Fax.Trim();
            OLD_Head.UID_Tel = Head.UID_Tel == null ? string.Empty : Head.UID_Tel.Trim();
            OLD_Head.UID_Email = Head.UID_Email == null ? string.Empty : Head.UID_Email.Trim();

            db.Entry(OLD_Head).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_RFQ_Item(Guid RID)
        {
            RFQ_Head OLD_Head = db.RFQ_Head.Find(RID);
            db.RFQ_Head.Remove(OLD_Head);

            List<RFQ_Head_Line> Line_List = db.RFQ_Head_Line.Where(x => x.Link_RID == OLD_Head.RID).ToList();
            if (Line_List.Any())
            {
                db.RFQ_Head_Line.RemoveRange(Line_List);
            }

            List<Guid> LineID_List = Line_List.Select(x => x.Line_ID).ToList();
            List<RFQ_Head_Line_To_Buyer> Line_For_Buyer_List = db.RFQ_Head_Line_To_Buyer.Where(x => LineID_List.Contains(x.RFQ_Line_ID)).ToList();
            if (Line_For_Buyer_List.Any())
            {
                db.RFQ_Head_Line_To_Buyer.RemoveRange(Line_For_Buyer_List);
            }

            this.Delete_Customer_By_RFQ(OLD_Head.CID, OLD_Head.RID);
            MyDbSave.SaveChange(db);
        }

        public void Delete_Customer_By_RFQ(Guid CID, Guid RID)
        {
            int Is_Can_Delete = 1;
            if(db.RFQ_Head.Where(x=>x.CID == CID && x.RID != RID).Any())
            {
                Is_Can_Delete = 0;
            }
            
            if(Is_Can_Delete == 1)
            {
                Customer C = db.Customer.Find(CID);
                if(C != null)
                {
                    db.Customer.Remove(C);
                }
            }
        }

        public void Create_RFQ_Line(Guid RID, RFQ_Head_Line Line)
        {
            RFQ_Head Head = db.RFQ_Head.Find(RID);

            Line.Line_ID = MyGUID.NewGUID();
            Line.Mat_Sn = string.Empty;
            Line.Cust_Mat_Sn = Line.Cust_Mat_Sn == null ? string.Empty : Line.Cust_Mat_Sn.Trim();
            Line.Cust_Mat_Sn_INFO_STR = Line.Cust_Mat_Sn_INFO_STR == null ? string.Empty : Line.Cust_Mat_Sn_INFO_STR.Trim();
            Line.Cust_Mat_Describe = Line.Cust_Mat_Describe == null ? string.Empty : Line.Cust_Mat_Describe.Trim();
            Line.Cust_Mat_Unit = Line.Cust_Mat_Unit == null ? string.Empty : Line.Cust_Mat_Unit.Trim();

            if (Line.BID == Guid.Empty) { throw new Exception("品牌未选择"); }
            Line.BID_Name = db.Brand.Find(Line.BID).Brand_Name;

            if (string.IsNullOrEmpty(Line.Cust_Mat_Sn)) { throw new Exception("询价型号未填写"); }

            if (string.IsNullOrEmpty(Line.Cust_Mat_Unit)) { throw new Exception("单位未填写"); }


            Line.Line_Number = db.RFQ_Head_Line.Where(x => x.Link_RID == Head.RID).Count() + 1;
            Line.Link_RID = Head.RID;
            Line.CID = Head.CID;
            Line.Create_DT = Head.Create_DT;
            Line.MatID = Guid.Empty;
            Line.Qty = Line.Qty <= 0 ? 1 : Line.Qty;
            Line.Urgency = Line.Urgency == null ? string.Empty : Line.Urgency.Trim();
            Line.Line_Remark = Line.Line_Remark == null ? string.Empty : Line.Line_Remark.Trim();
            Line.LinkMainCID = Head.LinkMainCID;
            db.RFQ_Head_Line.Add(Line);
            MyDbSave.SaveChange(db);
        }

        public void Delete_RFQ_Line(Guid Line_ID)
        {
            RFQ_Head_Line Line = db.RFQ_Head_Line.Find(Line_ID);
            if (Line != null)
            {
                db.RFQ_Head_Line.Remove(Line);

                List<RFQ_Head_Line> RFQ_Line_list = db.RFQ_Head_Line.Where(x => x.Link_RID == Line.Link_RID && x.Line_ID != Line.Line_ID).OrderBy(x => x.Line_Number).ToList();
                int i = 0;
                foreach (var x in RFQ_Line_list)
                {
                    i++;
                    x.Line_Number = i;
                }

                List<RFQ_Head_Line_To_Buyer> Line_For_Buyer_List = db.RFQ_Head_Line_To_Buyer.Where(x => x.RFQ_Line_ID == Line.Line_ID).ToList();
                if (Line_For_Buyer_List.Any())
                {
                    db.RFQ_Head_Line_To_Buyer.RemoveRange(Line_For_Buyer_List);
                }
                MyDbSave.SaveChange(db);
            }
        }

        public void Set_RFQ_Line(Guid Line_ID, RFQ_Head_Line Line)
        {
            RFQ_Head_Line OLD_Line = db.RFQ_Head_Line.Find(Line_ID);
            OLD_Line.Qty = Line.Qty <= 0 ? 1 : Line.Qty;
            OLD_Line.Urgency = Line.Urgency == null ? string.Empty : Line.Urgency.Trim();
            db.Entry(OLD_Line).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Set_RFQ_Line_More(Guid Line_ID, RFQ_Head_Line Line)
        {
            RFQ_Head_Line OLD_Line = db.RFQ_Head_Line.Find(Line_ID);
            OLD_Line.Cust_Mat_Sn = Line.Cust_Mat_Sn == null ? string.Empty : Line.Cust_Mat_Sn.Trim();
            OLD_Line.Cust_Mat_Describe = Line.Cust_Mat_Describe == null ? string.Empty : Line.Cust_Mat_Describe.Trim();
            OLD_Line.Cust_Mat_Unit = Line.Cust_Mat_Unit == null ? string.Empty : Line.Cust_Mat_Unit.Trim();
            OLD_Line.Cust_Mat_Sn_INFO_STR = Line.Cust_Mat_Sn_INFO_STR == null ? string.Empty : Line.Cust_Mat_Sn_INFO_STR.Trim();

            if (string.IsNullOrEmpty(OLD_Line.Cust_Mat_Sn))
            {
                throw new Exception("询价型号未填写");
            }

            if (string.IsNullOrEmpty(OLD_Line.Cust_Mat_Unit))
            {
                throw new Exception("单位未填写");
            }

            OLD_Line.Qty = Line.Qty <= 0 ? 1 : Line.Qty;
            OLD_Line.Urgency = Line.Urgency == null ? string.Empty : Line.Urgency.Trim();
            OLD_Line.Line_Remark = Line.Line_Remark == null ? string.Empty : Line.Line_Remark.Trim();
            db.Entry(OLD_Line).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public string RFQ_Excel_Template()
        {
            string Path = string.Empty;

            //设定表头
            DataTable DT = new DataTable("TempInvoice");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("询价型号");
            TableHeads.Add("客品描述");
            TableHeads.Add("品牌");
            TableHeads.Add("单位");
            TableHeads.Add("数量");
            TableHeads.Add("紧急度");
            TableHeads.Add("备注信息");

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
            for (int i = 0; i <= 50; i++)
            {
                newRow = DT.NewRow();
                newRow["询价型号"] = string.Empty;
                newRow["客品描述"] = string.Empty;
                newRow["品牌"] = string.Empty;
                newRow["单位"] = string.Empty;
                newRow["数量"] = string.Empty;
                newRow["紧急度"] = string.Empty;
                newRow["备注信息"] = string.Empty;
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }

        public void RFQ_Excel_Template_Upload(HttpPostedFileBase ExcelFile, Guid RID)
        {
            MyNormalUploadFile MF = new MyNormalUploadFile();
            //创建上传文件
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Excel_Temp/RFQ/" + MyGUID.NewGUID());

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);

            RFQ_Head Head = db.RFQ_Head.Find(RID);
            List<Brand> B_List = db.Brand.Where(x => x.LinkMainCID == Head.LinkMainCID).ToList();
            Brand B = new Brand();

            List<RFQ_Head_Line> Excel_Line_List = new List<RFQ_Head_Line>();
            RFQ_Head_Line Line = new RFQ_Head_Line();
            int Seconds = db.RFQ_Head_Line.Where(x => x.Link_RID == RID).Count();
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                Seconds++;
                IRow row = sheet.GetRow(i);
                Line = new RFQ_Head_Line();

                Line.Line_ID = MyGUID.NewGUID();
                Line.Link_RID = Head.RID;
                Line.Line_Number = Seconds;
                try { Line.Cust_Mat_Sn = row.GetCell(0).ToString().Trim(); } catch { Line.Cust_Mat_Sn = string.Empty; }
                try { Line.Cust_Mat_Describe = row.GetCell(1).ToString().Trim(); } catch { Line.Cust_Mat_Describe = string.Empty; }
                try { Line.BID_Name = row.GetCell(2).ToString().Trim(); } catch { Line.BID_Name = string.Empty; }
                try { Line.Cust_Mat_Unit = row.GetCell(3).ToString().Trim(); } catch { Line.Cust_Mat_Unit = string.Empty; }
                try { Line.Qty = Convert.ToInt32(row.GetCell(4).ToString().Trim()); } catch { Line.Qty = 0; }
                try { Line.Urgency = row.GetCell(5).ToString().Trim(); } catch { Line.Urgency = string.Empty; }
                try { Line.Line_Remark = row.GetCell(6).ToString().Trim(); } catch { Line.Line_Remark = string.Empty; }

                B = B_List.Where(c => c.Brand_Name == Line.BID_Name).FirstOrDefault();
                B = B == null ? new Brand() : B;
                Line.BID = B.BID;
                Line.BID_Name = B.Brand_Name;
                Line.LinkMainCID = Head.LinkMainCID;
                if (Line.BID != Guid.Empty && !string.IsNullOrEmpty(Line.Cust_Mat_Unit) && !string.IsNullOrEmpty(Line.Cust_Mat_Describe) && !string.IsNullOrEmpty(Line.Cust_Mat_Sn))
                {
                    Excel_Line_List.Add(Line);
                }
            }

            if (Excel_Line_List.Any())
            {
                db.RFQ_Head_Line.AddRange(Excel_Line_List);
                MyDbSave.SaveChange(db);
            }
        }

        public void RFQ_Sent_To_Sales(Guid RID)
        {
            RFQ_Head Head = db.RFQ_Head.Find(RID);
            Head.Status = RFQ_Status_Enum.进行中.ToString();
            MyDbSave.SaveChange(db);
        }
    }


    //报价更新
    public partial class RFQService
    {
        public void Set_RFQ_Line_MatID(Guid Line_ID, Guid MatID)
        {
            RFQ_Head_Line OLD_Line = db.RFQ_Head_Line.Find(Line_ID);
            Material Mat = db.Material.Find(MatID);
            Mat = Mat == null ? new Material() : Mat;
            OLD_Line.MatID = Mat.MatID;
            OLD_Line.Mat_Sn = Mat.MatSn;

            if(OLD_Line.BID != Mat.Link_BID)
            {
                throw new Exception("品牌不匹配");
            }
            MyDbSave.SaveChange(db);
        }

        public void Set_RFQ_Line_MatID_Cancel(Guid Line_ID)
        {
            RFQ_Head_Line OLD_Line = db.RFQ_Head_Line.Find(Line_ID);
            OLD_Line.MatID = Guid.Empty;
            OLD_Line.Mat_Sn = string.Empty;
            MyDbSave.SaveChange(db);
        }

        public RFQ_Head Get_RFQ_Head_Item(Guid RID)
        {
            RFQ_Head Head = db.RFQ_Head.Find(RID);
            Head = Head == null ? new RFQ_Head() : Head;
            Head.Line_List = this.Get_RFQ_Line_List(RID);
            return Head;
        }

        public RFQ_Head Get_RFQ_Head_DB(Guid RID)
        {
            RFQ_Head Head = db.RFQ_Head.Find(RID);
            Head = Head == null ? new RFQ_Head() : Head;
            return Head;
        }

        public List<RFQ_Head_Line> Get_RFQ_Line_List(Guid RID)
        {
            RFQ_Head Head = db.RFQ_Head.Find(RID);
            List<RFQ_Head_Line> Line_List = db.RFQ_Head_Line.Where(x => x.Link_RID == RID).ToList().OrderBy(x => x.Line_Number).ToList();

            List<Guid> MatID_List = Line_List.Select(x => x.MatID).Where(x => x != Guid.Empty).Distinct().ToList();
            List<Material> Mat_List = db.Material.Where(x => MatID_List.Contains(x.MatID)).ToList();
            foreach(var x in Mat_List)
            {
                x.Price_Retail = x.Price_Cost_Ref_Vat * x.Price_Retail_Rate;
                x.Price_Trade_A = x.Price_Cost_Ref_Vat * x.Price_Trade_A_Rate;
                x.Price_Trade_B = x.Price_Cost_Ref_Vat * x.Price_Trade_B_Rate;
                x.Price_Trade_NoTax = x.Price_Cost_Ref_Vat * x.Price_Trade_NoTax_Rate;
            }


            Material Mat = new Material();
            foreach (var x in Line_List)
            {
                Mat = Mat_List.Where(c => c.MatID == x.MatID).FirstOrDefault();
                Mat = Mat == null ? new Material() : Mat;
                x.Mat_Info = Mat;
            }
            return Line_List;
        }

        public RFQ_Head_Line Get_RFQ_Line_By_Line_ID(Guid Line_ID)
        {
            RFQ_Head_Line Line = db.RFQ_Head_Line.Find(Line_ID);
            Line = Line == null ? new RFQ_Head_Line() : Line;

            IMaterialService IMat = new MaterialService();
            Material Mat = IMat.Get_Material_Item(Line.MatID);
            return Line;
        }

        public PageList<RFQ_Head> Get_RFQ_Head_PageList(RFQ_Head_Filter MF)
        {
            var query = db.RFQ_Head.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            if (!string.IsNullOrEmpty(MF.Cust_Name))
            {
                query = query.Where(x => x.Cust_Name.Contains(MF.Cust_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.RFQ_No))
            {
                query = query.Where(x => x.RFQ_No.Contains(MF.RFQ_No)).AsQueryable();
            }

            if (MF.Status_List.Any())
            {
                query = query.Where(x => MF.Status_List.Contains(x.Status)).AsQueryable();
            }

            PageList<RFQ_Head> PList = new PageList<RFQ_Head>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Guid> RIDList = PList.Rows.Select(x => x.RID).ToList();
            List<RFQ_Head_Line> LineList = db.RFQ_Head_Line.Where(x => RIDList.Contains(x.Link_RID)).ToList();
            foreach (var x in PList.Rows)
            {
                x.Line_List = LineList.Where(c => c.Link_RID == x.RID).ToList();
            }
            return PList;
        }

    }

}
