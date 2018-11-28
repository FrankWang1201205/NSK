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
    public partial interface IPurchaseTempService
    {
        //价格比对
        PageList<Purchase_Temp> Get_Purchase_Temp_PageList(Purchase_Temp_Filter MF);
        void Batch_Create_Purchase_Temp(HttpPostedFileBase ExcelFile, User U);
        void Batch_Search_Purchase_Temp(HttpPostedFileBase ExcelFile, User U);
        void Abandon_Search_Purchase_Temp(User U);
        List<Purchase_Temp_Search> Get_Purchase_Temp_Search_List(User U);
        string Get_Batch_Search_Purchase_Temp_To_Excel(User U);

        //型号对比
        void Batch_Search_Purchase_Temp_MatSn(HttpPostedFileBase ExcelFile, User U);
    }

    public partial class PurchaseTempService : IPurchaseTempService
    {
        SmartdbContext db = new SmartdbContext();
    }

    //价格比对
    public partial class PurchaseTempService : IPurchaseTempService
    {
        public PageList<Purchase_Temp> Get_Purchase_Temp_PageList(Purchase_Temp_Filter MF)
        {
            var query = db.Purchase_Temp.Where(x => x.Link_MainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(x => x.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            PageList<Purchase_Temp> PList = new PageList<Purchase_Temp>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Cust_Code).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public List<Purchase_Temp_Search> Get_Purchase_Temp_Search_List(User U)
        {
            List<Purchase_Temp_Search> List = db.Purchase_Temp_Search.Where(x => x.LinkMainCID == U.LinkMainCID).OrderBy(x=>x.MatSn).ToList();
            return List;
        }

        public void Batch_Create_Purchase_Temp(HttpPostedFileBase ExcelFile, User U)
        {
            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Purchase_Temp/" + U.UID);

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //读取Excel列，装箱数据
            List<Purchase_Temp> Line_List = new List<Purchase_Temp>();
            Purchase_Temp Line = new Purchase_Temp();
            ISheet sheet = workbook.GetSheetAt(0);

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            IWmsService IW = new WmsService();

            DateTime Create_DT = DateTime.Now;
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                Line = new Purchase_Temp();
                Line.PTID = MyGUID.NewGUID();
                Line.Create_DT = Create_DT;
                Line.Link_MainCID = U.LinkMainCID;

                try { Line.Cust_Code = row.GetCell(0).ToString().Trim(); } catch { Line.Cust_Code = string.Empty; }
                try { Line.MatSn = row.GetCell(1).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }
                try { Line.MatBrand = row.GetCell(2).ToString().Trim(); } catch { Line.MatBrand = string.Empty; }
                try { Line.Cust_Short_Name = row.GetCell(3).ToString().Trim(); } catch { Line.Cust_Short_Name = string.Empty; }
                try { Line.Sales_Rep_Name = row.GetCell(4).ToString().Trim(); } catch { Line.Sales_Rep_Name = string.Empty; }
                try { Line.PC_Code = row.GetCell(5).ToString().Trim(); } catch { Line.PC_Code = string.Empty; }
                try { Line.AP_Sup_Code = row.GetCell(6).ToString().Trim(); } catch { Line.AP_Sup_Code = string.Empty; }
                try { Line.Currency_Code = row.GetCell(7).ToString().Trim(); } catch { Line.Currency_Code = string.Empty; }
                try { Line.Contract_Price = Convert.ToDecimal(row.GetCell(8).ToString().Trim()); } catch { Line.Contract_Price = 0; }

                //过滤换行符
                Line.Cust_Code = Line.Cust_Code.Replace(Environment.NewLine, "");
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = IW.MatSn_Check_And_Replace(Line.MatSn);
                Line.MatBrand = Line.MatBrand.Replace(Environment.NewLine, "");
                Line.Cust_Short_Name = Line.Cust_Short_Name.Replace(Environment.NewLine, "");
                Line.Sales_Rep_Name = Line.Sales_Rep_Name.Replace(Environment.NewLine, "");
                Line.PC_Code = Line.PC_Code.Replace(Environment.NewLine, "");
                Line.AP_Sup_Code = Line.AP_Sup_Code.Replace(Environment.NewLine, "");
                Line.Currency_Code = Line.Currency_Code.Replace(Environment.NewLine, "");

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

                //判断单元格是否有公式
                for (int j = 0; j < 9; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Line.MatSn) && Line.Contract_Price >= 0
                    && !string.IsNullOrEmpty(Line.MatBrand) && !string.IsNullOrEmpty(Line.Cust_Code)
                    && !string.IsNullOrEmpty(Line.Cust_Short_Name) && !string.IsNullOrEmpty(Line.Sales_Rep_Name)
                    && !string.IsNullOrEmpty(Line.PC_Code) && !string.IsNullOrEmpty(Line.Currency_Code))
                {
                    Line_List.Add(Line);
                }
            }

            if (Line_List.Any() == false)
            {
                throw new Exception("Excel中不存在可导入内容");
            }
            else
            {
                db.Purchase_Temp.AddRange(Line_List);
                MyDbSave.SaveChange(db);
            }
        }

        public void Batch_Search_Purchase_Temp(HttpPostedFileBase ExcelFile, User U)
        {
            List<Purchase_Temp_Search> Search_List = db.Purchase_Temp_Search.Where(x => x.LinkMainCID == U.LinkMainCID).ToList();
            if (Search_List.Any())
            {
                db.Purchase_Temp_Search.RemoveRange(Search_List);
                MyDbSave.SaveChange(db);
                Thread.Sleep(500);
            }

            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Purchase_Temp_Search/" + U.UID);

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //读取Excel列，装箱数据
            List<Purchase_Temp_Search> Line_List = new List<Purchase_Temp_Search>();
            Purchase_Temp_Search Line = new Purchase_Temp_Search();
            ISheet sheet = workbook.GetSheetAt(0);

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            IWmsService IW = new WmsService();

            DateTime Create_DT = DateTime.Now;
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                Line = new Purchase_Temp_Search();
                Line.PTS_ID = MyGUID.NewGUID();
                Line.Create_DT = Create_DT;
                Line.LinkMainCID = U.LinkMainCID;

                try { Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }
                try { Line.MatBrand = row.GetCell(1).ToString().Trim(); } catch { Line.MatBrand = string.Empty; }
                try { Line.Quantity_Request = Convert.ToInt32(row.GetCell(2).ToString().Trim()); } catch { Line.Quantity_Request = 0; }
                try { Line.Quantity_Request_More = Convert.ToInt32(row.GetCell(3).ToString().Trim()); } catch { Line.Quantity_Request_More = 0; }
                try { Line.Customer_Name = row.GetCell(4).ToString().Trim(); } catch { Line.Customer_Name = string.Empty; }
                try { Line.Sales_Person = row.GetCell(5).ToString().Trim(); } catch { Line.Sales_Person = string.Empty; }
                try { Line.Price_Cost = Convert.ToDecimal(row.GetCell(6).ToString().Trim()); } catch { Line.Price_Cost = 0; }
                try { Line.Quantity = Convert.ToInt32(row.GetCell(7).ToString().Trim()); } catch { Line.Quantity = 0; }
                try { Line.Supplier_Name = row.GetCell(8).ToString().Trim(); } catch { Line.Supplier_Name = string.Empty; }

                //过滤换行符
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = IW.MatSn_Check_And_Replace(Line.MatSn);
                Line.MatBrand = Line.MatBrand.Replace(Environment.NewLine, "");
                Line.Customer_Name = Line.Customer_Name.Replace(Environment.NewLine, "");
                Line.Sales_Person = Line.Sales_Person.Replace(Environment.NewLine, "");
                Line.Supplier_Name = Line.Supplier_Name.Replace(Environment.NewLine, "");

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

                //判断单元格是否有公式
                for (int j = 0; j < 9; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Line.MatSn) && Line.Price_Cost >= 0
                    && !string.IsNullOrEmpty(Line.MatBrand) && !string.IsNullOrEmpty(Line.Customer_Name)
                    && !string.IsNullOrEmpty(Line.Sales_Person) && !string.IsNullOrEmpty(Line.Supplier_Name)
                    && Line.Quantity > 0)
                {
                    Line_List.Add(Line);
                }
            }

            if (Line_List.Any())
            {
                db.Purchase_Temp_Search.AddRange(Line_List);
                MyDbSave.SaveChange(db);
            }
        }

        public void Abandon_Search_Purchase_Temp(User U)
        {
            List<Purchase_Temp_Search> Search_List = db.Purchase_Temp_Search.Where(x => x.LinkMainCID == U.LinkMainCID).ToList();
            if (Search_List.Any())
            {
                db.Purchase_Temp_Search.RemoveRange(Search_List);
                MyDbSave.SaveChange(db);
            }
        }

        public string Get_Batch_Search_Purchase_Temp_To_Excel(User U)
        {
            List<Purchase_Temp_Search> List_Search_DB = db.Purchase_Temp_Search.Where(x => x.LinkMainCID == U.LinkMainCID).ToList();
            List<string> MatSn_List = List_Search_DB.Select(x => x.MatSn).Distinct().ToList();
            List<Purchase_Temp> List_DB = db.Purchase_Temp.Where(x => x.Link_MainCID == U.LinkMainCID && MatSn_List.Contains(x.MatSn)).ToList();
            List<Purchase_Temp> List_DB_Sub = new List<Purchase_Temp>();

            List<Purchase_Temp_Search> List_Search = new List<Purchase_Temp_Search>();
            Purchase_Temp_Search Search = new Purchase_Temp_Search();

            foreach (var x in List_Search_DB)
            {
                List_DB_Sub = List_DB.Where(c => c.MatSn == x.MatSn).ToList();
                foreach (var xx in List_DB_Sub)
                {
                    Search = new Purchase_Temp_Search();
                    Search.MatSn = x.MatSn;
                    Search.MatBrand = x.MatBrand;
                    Search.Quantity_Request = x.Quantity_Request;
                    Search.Quantity_Request_More = x.Quantity_Request_More;
                    Search.Quantity = x.Quantity;
                    Search.Customer_Name = x.Customer_Name;
                    Search.Sales_Person = x.Sales_Person;
                    Search.Price_Cost = x.Price_Cost;
                    Search.Supplier_Name = x.Supplier_Name;
                    Search.Cust_Code = xx.Cust_Code;
                    Search.Contract_Price = xx.Contract_Price;
                    List_Search.Add(Search);
                }
            }

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("产品型号");
            TableHeads.Add("品牌");
            TableHeads.Add("实际需求数");
            TableHeads.Add("备货数");
            TableHeads.Add("客户名");
            TableHeads.Add("调货人");
            TableHeads.Add("调货价");
            TableHeads.Add("调货数量");
            TableHeads.Add("调货处");
            TableHeads.Add("订货窗口编号");
            TableHeads.Add("订货价格");

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
            foreach (var x in List_Search)
            {
                i++;
                newRow = DT.NewRow();
                newRow["产品型号"] = x.MatSn;
                newRow["品牌"] = x.MatBrand;
                newRow["实际需求数"] = x.Quantity_Request;
                newRow["备货数"] = x.Quantity_Request_More;
                newRow["客户名"] = x.Customer_Name;
                newRow["调货人"] = x.Sales_Person;
                newRow["调货价"] = x.Price_Cost.ToString("N4");
                newRow["调货数量"] = x.Quantity;
                newRow["调货处"] = x.Supplier_Name;
                newRow["订货窗口编号"] = x.Cust_Code;
                newRow["订货价格"] = x.Contract_Price.ToString("N4");
                DT.Rows.Add(newRow);
            }
            Path = MyExcel.CreateNewExcel(DT);
            return Path;
        }
    }

    //型号对比
    public partial class PurchaseTempService : IPurchaseTempService
    {
        public void Batch_Search_Purchase_Temp_MatSn(HttpPostedFileBase ExcelFile, User U)
        {
            List<Purchase_Temp_Search> Search_List = db.Purchase_Temp_Search.Where(x => x.LinkMainCID == U.LinkMainCID).ToList();
            if (Search_List.Any())
            {
                db.Purchase_Temp_Search.RemoveRange(Search_List);
                MyDbSave.SaveChange(db);
                Thread.Sleep(500);
            }

            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Purchase_Temp_MatSn_Search/" + U.UID);

            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //读取Excel列，装箱数据
            List<Purchase_Temp_Search> Line_List = new List<Purchase_Temp_Search>();
            Purchase_Temp_Search Line = new Purchase_Temp_Search();
            ISheet sheet = workbook.GetSheetAt(0);

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            IWmsService IW = new WmsService();

            DateTime Create_DT = DateTime.Now;
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                Line = new Purchase_Temp_Search();
                Line.PTS_ID = MyGUID.NewGUID();
                Line.Create_DT = Create_DT;
                Line.LinkMainCID = U.LinkMainCID;

                try { Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }
                try { Line.MatBrand = row.GetCell(1).ToString().Trim(); } catch { Line.MatBrand = string.Empty; }

                //过滤换行符
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = IW.MatSn_Check_And_Replace(Line.MatSn);
                Line.MatBrand = Line.MatBrand.Replace(Environment.NewLine, "");

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

                if (!string.IsNullOrEmpty(Line.MatSn))
                {
                    if (db.Material.Where(x => x.LinkMainCID == U.LinkMainCID && x.MatSn == Line.MatSn).Any())
                    {
                        Line_List.Add(Line);
                    }
                }
            }

            if (Line_List.Any())
            {
                db.Purchase_Temp_Search.AddRange(Line_List);
                MyDbSave.SaveChange(db);
            }
        }

    }

}
