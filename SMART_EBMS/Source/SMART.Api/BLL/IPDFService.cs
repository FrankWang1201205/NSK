using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.IO;
using SMART.Api.Models;

namespace SMART.Api
{
    public interface IPDFService
    {
        //产品型号标签
        string Create_Material_Label(string MatSn);

        //产品型号比对标签
        string Create_Material_Label_List(Guid LinkMainCID, List<Guid> IDList);
        string Create_Material_Label_List(Guid TaskID);

        //收货打印
        string Create_PDF_For_WMS_In_By_TrayNo(Guid Head_ID, string Tray_No);
        string Create_PDF_For_WMS_In_By_TrayNo_With_CaseNo(Guid Head_ID, string Tray_No, string Case_No);

        string Create_PDF_For_WMS_In_Label(WMS_In_Line Line);
        string Create_PDF_For_Material_Label(Material M);

        //库位打印
        string Create_PDF_For_WMS_Location(WMS_Location Loc);
        string Create_PDF_For_WMS_Location_List(Guid LinkMainCID, List<Guid> IDList);

        //出库送货单
        string Create_PDF_For_WMS_Out_Task(WMS_Out_Head Head);

        //出库任务明细(托盘或箱)
        string Create_PDF_For_WMS_Out_Task_By_Tray(WMS_Out_Head Head, string Tray_No);

        string Create_PDF_For_WMS_Out_Task_By_Tray_Label(WMS_Out_Head Head, string Tray_No);
        string Create_PDF_For_WMS_Out_Task_By_Tray_Label_With_Case(WMS_Out_Head Head, string Tray_No, string Case_No);
    }

    public partial class PDFService : IPDFService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class PDFService : IPDFService
    {
        public string Create_Material_Label(string MatSn)
        {
            IMaterialService IMat = new MaterialService();
            string QRCode_Path = IMat.Get_Material_QRCodePath(MatSn);

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\产品标签.pdf";

            Rectangle pageSize = new Rectangle(400, 200);
            Document document = new Document(pageSize, 10, 10, 10, 10);

            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            document.Open();

            //产品型号+二维码
            //创建Table布局
            PdfPTable TableHead = new PdfPTable(1);
            TableHead.WidthPercentage = 100;
            TableHead.DefaultCell.Border = 0;

            PdfPCell TableTitlecell = new PdfPCell();
            TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
            TableTitlecell.Border = 0;
            string Host = HttpRuntime.AppDomainAppPath.ToString();
            Image Img = Image.GetInstance(Host + QRCode_Path);
            Img.Alignment = Image.ALIGN_CENTER;
            Img.Border = 0;
            Img.WidthPercentage = 30;
            TableTitlecell.AddElement(Img);
            TableHead.AddCell(TableTitlecell);
            document.Add(TableHead);

            PdfPTable TableBody = new PdfPTable(1);
            TableBody.WidthPercentage = 100;

            PdfPCell Tablecell = new PdfPCell();

            Tablecell = new PdfPCell(new Phrase(0, MatSn, this.GetBaseFontItem("consola", 24)));
            Tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
            Tablecell.BorderWidth = 0;
            TableBody.AddCell(Tablecell);

            document.Add(TableBody);

            document.NewPage();

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;

        }

        public string Create_Material_Label_List(Guid LinkMainCID, List<Guid> IDList)
        {
            List<Purchase_Temp_Search> List = db.Purchase_Temp_Search.Where(x => x.LinkMainCID == LinkMainCID && IDList.Contains(x.PTS_ID)).ToList();

            foreach (var x in List)
            {
                x.QRCode_Path = QRCode.CreateQRCode_Location(x.MatSn, x.PTS_ID);
            }

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\产品标签.pdf";

            Rectangle pageSize = new Rectangle(400, 200);
            Document document = new Document(pageSize, 10, 10, 10, 10);

            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            document.Open();
            foreach (var x in List)
            {
                //产品型号+二维码
                //创建Table布局
                PdfPTable TableHead = new PdfPTable(1);
                TableHead.WidthPercentage = 100;
                TableHead.DefaultCell.Border = 0;

                PdfPCell TableTitlecell = new PdfPCell();
                TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
                TableTitlecell.Border = 0;
                string Host = HttpRuntime.AppDomainAppPath.ToString();
                Image Img = Image.GetInstance(Host + x.QRCode_Path);
                Img.Alignment = Image.ALIGN_CENTER;
                Img.Border = 0;
                Img.WidthPercentage = 30;
                TableTitlecell.AddElement(Img);
                TableHead.AddCell(TableTitlecell);
                document.Add(TableHead);

                PdfPTable TableBody = new PdfPTable(1);
                TableBody.WidthPercentage = 100;

                PdfPCell Tablecell = new PdfPCell();

                Tablecell = new PdfPCell(new Phrase(0, x.MatSn, this.GetBaseFontItem("consola", 24)));
                Tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                Tablecell.BorderWidth = 0;
                TableBody.AddCell(Tablecell);

                document.Add(TableBody);

                document.NewPage();
            }

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;

        }

        public string Create_Material_Label_List(Guid TaskID)
        {
            WMS_Stock_Task Task = db.WMS_Stock_Task.Find(TaskID);
            if (Task == null) { throw new Exception("Task is null"); }

            List<WMS_Stocktaking_Scan> Scan_List = db.WMS_Stocktaking_Scan.Where(x => x.Link_TaskID == Task.Task_ID).ToList();
            
            foreach (var x in Scan_List)
            {
                x.QRCode_Path = QRCode.CreateQRCode_Location(x.MatSn, x.Scan_ID);
            }

            var Group = from x in Scan_List
                        group x by x.MatSn into g
                        select new
                        {
                            MatSn = g.Key,
                            QRCode_Path = g.FirstOrDefault().QRCode_Path,
                        };

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\端数产品标签.pdf";

            Rectangle pageSize = new Rectangle(300, 200);
            Document document = new Document(pageSize, 10, 10, 18, 10);

            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            document.Open();

            foreach (var x in Group)
            {
                //产品型号+二维码
                //创建Table布局
                PdfPTable TableHead = new PdfPTable(1);
                TableHead.WidthPercentage = 100;
                TableHead.DefaultCell.Border = 0;

                PdfPCell TableTitlecell = new PdfPCell();
                TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
                TableTitlecell.Border = 0;
                string Host = HttpRuntime.AppDomainAppPath.ToString();
                Image Img = Image.GetInstance(Host + x.QRCode_Path);
                Img.Alignment = Image.ALIGN_CENTER;
                Img.Border = 0;
                Img.WidthPercentage = 45;
                TableTitlecell.AddElement(Img);
                TableHead.AddCell(TableTitlecell);
                document.Add(TableHead);

                PdfPTable TableBody = new PdfPTable(1);
                TableBody.WidthPercentage = 100;

                PdfPCell Tablecell = new PdfPCell();

                Tablecell = new PdfPCell(new Phrase(0, x.MatSn, this.GetBaseFontItem("consola", 18)));
                Tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                Tablecell.BorderWidth = 0;
                TableBody.AddCell(Tablecell);

                document.Add(TableBody);

                document.NewPage();
            }

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;
        }

        //收货打印
        public string Create_PDF_For_WMS_In_By_TrayNo(Guid Head_ID, string Tray_No)
        {
            Tray_No = Tray_No.Trim();
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            List<WMS_In_Scan> Scan_List = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No).ToList();
            var Group = from x in Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).ToList()
                        group x by new { x.MatSn } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Box_Count = G.Count(),
                            Scan_Quantity_Sum = G.Sum(c => c.Scan_Quantity)
                        };

            var Group_Other = from x in Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).ToList()
                              group x by new { x.MatSn, x.Case_No } into G
                              select new
                              {
                                  MatSn = G.Key.MatSn,
                                  Case_No = G.Key.Case_No,
                                  Scan_Quantity_Sum = G.Sum(c => c.Scan_Quantity)
                              };

            MainCompany Com = db.MainCompany.Find(Head.LinkMainCID);
            Com = Com == null ? new MainCompany() : Com;

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();

            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\出库作业_产品标签.pdf";

            //创建一个Document   并设定它的页面大小和边距
            Document document = new Document(PageSize.A4, 20, 20, 20, 10);

            //创建一个Writer实例
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //打开Document
            document.Open();

            //标题
            Paragraph Row = new Paragraph(Com.MainCompanyName, this.GetBaseFontItem("STZHONGS", 20));
            Row.Alignment = Element.ALIGN_CENTER;
            Row.SpacingAfter = 10f;
            Row.SpacingBefore = 10f;
            document.Add(Row);

            //托盘号
            Row = new Paragraph("第 " + Tray_No + " 托", this.GetBaseFontItem("STZHONGS", 17));
            Row.Alignment = Element.ALIGN_CENTER;
            Row.SpacingAfter = 2f;
            Row.SpacingBefore = 2f;
            document.Add(Row);

            if (Group.Any())
            {
                Paragraph Title = new Paragraph("整箱信息", this.GetBaseFontItem("STZHONGS", 15));
                Title.Alignment = Element.ALIGN_LEFT;
                Title.SpacingAfter = 10f;
                Title.SpacingBefore = 10f;
                document.Add(Title);

                //整箱表格
                PdfPTable TableWhole = new PdfPTable(new float[] { 6, 2, 2 });
                TableWhole.TotalWidth = 550;
                TableWhole.WidthPercentage = 100;
                TableWhole.LockedWidth = true;

                PdfPCell cell1 = new PdfPCell(new Paragraph("产品型号", this.GetBaseFontItem("STZHONGS", 14)));
                cell1.PaddingLeft = 8;
                cell1.MinimumHeight = 25;
                cell1.NoWrap = true;
                cell1.SetLeading(1f, 1f);
                TableWhole.AddCell(cell1);

                cell1 = new PdfPCell(new Paragraph("整箱数", this.GetBaseFontItem("STZHONGS", 14)));
                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell1.PaddingRight = 8;
                cell1.NoWrap = true;
                cell1.SetLeading(1f, 1f);
                TableWhole.AddCell(cell1);

                cell1 = new PdfPCell(new Paragraph("数量", this.GetBaseFontItem("STZHONGS", 14)));
                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell1.PaddingRight = 8;
                cell1.NoWrap = true;
                cell1.SetLeading(1f, 1f);
                TableWhole.AddCell(cell1);

                foreach (var x in Group)
                {

                    cell1 = new PdfPCell(new Paragraph(x.MatSn, this.GetBaseFontItem("consola", 14)));
                    cell1.PaddingLeft = 8;
                    cell1.MinimumHeight = 25;
                    cell1.NoWrap = true;
                    cell1.SetLeading(1f, 1f);
                    TableWhole.AddCell(cell1);

                    cell1 = new PdfPCell(new Paragraph(x.Box_Count.ToString(), this.GetBaseFontItem("STZHONGS", 14)));
                    cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell1.PaddingRight = 8;
                    cell1.NoWrap = true;
                    cell1.SetLeading(1f, 1f);
                    TableWhole.AddCell(cell1);

                    cell1 = new PdfPCell(new Paragraph(x.Scan_Quantity_Sum.ToString(), this.GetBaseFontItem("STZHONGS", 14)));
                    cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell1.PaddingRight = 8;
                    cell1.NoWrap = true;
                    cell1.SetLeading(1f, 1f);
                    TableWhole.AddCell(cell1);
                }

                document.Add(TableWhole);
            }

            if (Group_Other.Any())
            {
                Paragraph Title2 = new Paragraph("零头箱信息", this.GetBaseFontItem("STZHONGS", 15));
                Title2.Alignment = Element.ALIGN_LEFT;
                Title2.SpacingAfter = 10f;
                Title2.SpacingBefore = 10f;
                document.Add(Title2);

                //端数箱表格
                PdfPTable TableSub = new PdfPTable(new float[] { 6, 2, 2 });
                TableSub.TotalWidth = 550;
                TableSub.WidthPercentage = 100;
                TableSub.LockedWidth = true;

                PdfPCell cell2 = new PdfPCell(new Paragraph("产品型号", this.GetBaseFontItem("STZHONGS", 14)));
                cell2.PaddingLeft = 8;
                cell2.MinimumHeight = 25;
                cell2.NoWrap = true;
                cell2.SetLeading(1f, 1f);
                TableSub.AddCell(cell2);

                cell2 = new PdfPCell(new Paragraph("箱号", this.GetBaseFontItem("STZHONGS", 14)));
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.PaddingRight = 8;
                cell2.NoWrap = true;
                cell2.SetLeading(1f, 1f);
                TableSub.AddCell(cell2);

                cell2 = new PdfPCell(new Paragraph("数量", this.GetBaseFontItem("STZHONGS", 14)));
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.PaddingRight = 8;
                cell2.NoWrap = true;
                cell2.SetLeading(1f, 1f);
                TableSub.AddCell(cell2);

                foreach (var x in Group_Other)
                {
                    cell2 = new PdfPCell(new Paragraph(x.MatSn, this.GetBaseFontItem("consola", 14)));
                    cell2.PaddingLeft = 8;
                    cell2.MinimumHeight = 25;
                    cell2.NoWrap = true;
                    cell2.SetLeading(1f, 1f);
                    TableSub.AddCell(cell2);

                    cell2 = new PdfPCell(new Paragraph(x.Case_No, this.GetBaseFontItem("STZHONGS", 14)));
                    cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell2.PaddingRight = 8;
                    cell2.NoWrap = true;
                    cell2.SetLeading(1f, 1f);
                    TableSub.AddCell(cell2);

                    cell2 = new PdfPCell(new Paragraph(x.Scan_Quantity_Sum.ToString(), this.GetBaseFontItem("STZHONGS", 14)));
                    cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell2.PaddingRight = 8;
                    cell2.NoWrap = true;
                    cell2.SetLeading(1f, 1f);
                    TableSub.AddCell(cell2);
                }

                document.Add(TableSub);
            }

            //空一行
            Row = new Paragraph("\n", this.GetBaseFontItem("arialbd", 14));
            document.Add(Row);

            int TotalQuantity = Scan_List.Sum(x => x.Scan_Quantity);
            int TotalBox = 0;

            int Box_Count = 0;
            int Box_Count_Other = 0;
            if (Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).Any())
            {
                Box_Count = Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.整箱.ToString()).Count();
            }

            if (Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).Any())
            {
                Box_Count_Other = Scan_List.Where(x => x.Package_Type == WMS_Stock_Package_Enum.零头.ToString()).Select(x => x.Case_No).Distinct().Count();
            }

            TotalBox = Box_Count + Box_Count_Other;

            //汇总表格
            PdfPTable TableSum = new PdfPTable(new float[] { 6, 2, 2 });
            TableSum.HorizontalAlignment = Element.ALIGN_RIGHT;
            TableSum.TotalWidth = 550;
            TableSum.WidthPercentage = 100;
            TableSum.LockedWidth = true;

            PdfPCell cell = new PdfPCell(new Paragraph(" ", this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingRight = 8;
            cell.MinimumHeight = 25;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            cell.Border = 0;
            TableSum.AddCell(cell);

            cell = new PdfPCell(new Paragraph("总数量", this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingRight = 8;
            cell.MinimumHeight = 25;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            TableSum.AddCell(cell);

            cell = new PdfPCell(new Paragraph(TotalQuantity.ToString(), this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingRight = 8;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            TableSum.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" ", this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingRight = 8;
            cell.MinimumHeight = 25;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            cell.Border = 0;
            TableSum.AddCell(cell);

            cell = new PdfPCell(new Paragraph("总箱数", this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingRight = 8;
            cell.MinimumHeight = 25;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            TableSum.AddCell(cell);

            cell = new PdfPCell(new Paragraph(TotalBox.ToString(), this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingRight = 8;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            TableSum.AddCell(cell);

            if (Box_Count > 0)
            {
                cell = new PdfPCell(new Paragraph(" ", this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                cell.Border = 0;
                TableSum.AddCell(cell);

                cell = new PdfPCell(new Paragraph("整箱数", this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableSum.AddCell(cell);

                cell = new PdfPCell(new Paragraph(Box_Count.ToString(), this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 8;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableSum.AddCell(cell);
            }

            if (Box_Count_Other > 0)
            {
                cell = new PdfPCell(new Paragraph(" ", this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                cell.Border = 0;
                TableSum.AddCell(cell);

                cell = new PdfPCell(new Paragraph("零头箱数", this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableSum.AddCell(cell);

                cell = new PdfPCell(new Paragraph(Box_Count_Other.ToString(), this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 8;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableSum.AddCell(cell);
            }

            document.Add(TableSum);

            //将表格添加到pdf文档中
            //document.Add();

            //给PDF文件添加标题
            document.AddTitle(Com.MainCompanyName);
            //创建一个新页面
            document.NewPage();

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;
        }

        //收货打印
        public string Create_PDF_For_WMS_In_By_TrayNo_With_CaseNo(Guid Head_ID, string Tray_No, string Case_No)
        {
            Tray_No = Tray_No.Trim();
            Case_No = Case_No.Trim();
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            List<WMS_In_Scan> Scan_List = db.WMS_In_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No && x.Case_No == Case_No).ToList();
            var Group = from x in Scan_List
                        group x by new { x.MatSn } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Scan_Quantity_Sum = G.Sum(c => c.Scan_Quantity)
                        };

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\" + Tray_No + "_" + Case_No + "_产品标签.pdf";

            //第一步，创建一个 iTextSharp.text.Document对象的实例：
            Rectangle pageSize = new Rectangle(300, 200);
            Document document = new Document(pageSize, 20, 10, 15, 10);
            //Document document = new Document(PageSize.A7.Rotate(), 10, 10, 20, 10);

            //第二步，为该Document创建一个Writer实例： 
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //第三步，打开当前Document 
            document.Open();

            //第四步，为当前Document添加内容：

            //标题
            Paragraph Row1 = new Paragraph("虹恩端数箱（" + Head.Task_Bat_No_Str + ")_" + Case_No, this.GetBaseFontItem("STZHONGS", 13));
            Row1.Alignment = Element.ALIGN_CENTER;
            Row1.SpacingAfter = 10f;
            Row1.SpacingBefore = 10f;
            document.Add(Row1);

            int PageLineSize = 4;
            int PageIndex = 0;
            int PageTotal = Convert.ToInt32(Math.Ceiling(Group.Count() / (decimal)PageLineSize));

            for (int i = 1; i <= PageTotal; i++)
            {
                PageIndex++;

                PdfPTable Table_Test = new PdfPTable(new float[] { 8, 2 });//设置表格各列宽度  当前为4列
                Table_Test.WidthPercentage = 100;//宽度百分比
                Table_Test.TotalWidth = 280; //表格总宽度
                Table_Test.LockedWidth = true;//锁定宽度

                PdfPCell cell = new PdfPCell(new Paragraph("产品型号", this.GetBaseFontItem("STZHONGS", 11)));
                cell.NoWrap = true;
                cell.PaddingBottom = 3;
                cell.PaddingTop = 3;
                cell.SetLeading(1.2f, 1.2f);
                Table_Test.AddCell(cell);

                cell = new PdfPCell(new Paragraph("数量", this.GetBaseFontItem("STZHONGS", 11)));
                cell.NoWrap = true;
                cell.PaddingBottom = 3;
                cell.PaddingTop = 3;
                cell.HorizontalAlignment = 2;
                cell.SetLeading(1.2f, 1.2f);
                Table_Test.AddCell(cell);

                foreach (var x in Group.Skip((PageIndex - 1) * PageLineSize).Take(PageLineSize).ToList())
                {
                    cell = new PdfPCell(new Paragraph(x.MatSn, this.GetBaseFontItem("consola", 11)));
                    cell.NoWrap = true;
                    cell.PaddingBottom = 3;
                    cell.PaddingTop = 3;
                    cell.SetLeading(1.2f, 1.2f);
                    Table_Test.AddCell(cell);

                    cell = new PdfPCell(new Paragraph(x.Scan_Quantity_Sum.ToString(), this.GetBaseFontItem("STZHONGS", 11)));
                    cell.NoWrap = true;
                    cell.PaddingBottom = 3;
                    cell.PaddingTop = 3;
                    cell.HorizontalAlignment = 2;
                    cell.SetLeading(1.2f, 1.2f);
                    Table_Test.AddCell(cell);
                }

                document.Add(Table_Test);
                document.NewPage();
            }
            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;
        }

        public string Create_PDF_For_WMS_In_Label(WMS_In_Line Line)
        {
            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\" + Line.Line_ID + "_产品标签.pdf";

            //第一步，创建一个 iTextSharp.text.Document对象的实例：
            Rectangle pageSize = new Rectangle(300, 200);
            Document document = new Document(pageSize, 20, 10, 15, 10);
            //Document document = new Document(PageSize.A7.Rotate(), 10, 10, 20, 10);

            //第二步，为该Document创建一个Writer实例： 
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //第三步，打开当前Document 
            document.Open();

            //第四步，为当前Document添加内容：

            //创建Table布局2列2行
            PdfPTable TableHead = new PdfPTable(new float[] { 2.5f, 7.5f });
            TableHead.WidthPercentage = 105;
            TableHead.DefaultCell.BorderColor = BaseColor.RED;
            TableHead.DefaultCell.BorderWidth = 0;

            //左侧标签区域
            PdfPCell TableTitlecell = new PdfPCell();
            TableTitlecell.HorizontalAlignment = Element.ALIGN_LEFT;
            TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
            TableTitlecell.BorderWidth = 0;
            TableTitlecell.BorderWidthBottom = 3;
            //TableTitlecell.BorderWidthLeft = 0;
            //TableTitlecell.BorderWidthTop = 0;
            string Host = HttpRuntime.AppDomainAppPath.ToString();
            Image Img = Image.GetInstance(Host + Line.QRCode_Path);
            Img.Alignment = Image.ALIGN_LEFT;
            Img.Border = 0;
            TableTitlecell.PaddingRight = 10;
            TableTitlecell.PaddingBottom = 10;
            TableTitlecell.AddElement(Img);
            TableHead.AddCell(TableTitlecell);


            TableTitlecell = new PdfPCell();
            TableTitlecell.HorizontalAlignment = Element.ALIGN_LEFT;
            TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
            TableTitlecell.BorderColor = BaseColor.BLACK;
            TableTitlecell.BorderWidth = 3;
            TableTitlecell.BorderWidthBottom = 0;
            //TableTitlecell.BorderWidthTop = 3;
            //TableTitlecell.BorderWidthRight = 3;


            //右侧标题及产品型号
            PdfPTable TableHead_Sub = new PdfPTable(1);
            TableHead_Sub.WidthPercentage = 100;
            TableHead_Sub.DefaultCell.BorderColor = BaseColor.BLACK;
            TableHead_Sub.DefaultCell.BorderWidth = 0;

            PdfPCell Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.BorderWidth = 0;
            Table_Sub_Cell.BorderWidthBottom = 0.5f;
            Table_Sub_Cell.PaddingBottom = 10;
            Paragraph Row1 = new Paragraph("虹恩零头箱", this.GetBaseFontItem("STZHONGS", 18));
            Row1.SetLeading(0, 1);
            Table_Sub_Cell.AddElement(Row1);
            TableHead_Sub.AddCell(Table_Sub_Cell);

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            Table_Sub_Cell.BorderWidth = 0;
            Table_Sub_Cell.PaddingTop = 10;
            Paragraph Row2 = new Paragraph(Line.MatSn, this.GetBaseFontItem("arial", 13));
            Row2.SetLeading(0, 1);
            Table_Sub_Cell.NoWrap = true;
            Table_Sub_Cell.AddElement(Row2);
            TableHead_Sub.AddCell(Table_Sub_Cell);
            TableTitlecell.AddElement(TableHead_Sub);
            TableHead.AddCell(TableTitlecell);

            //第二部部分
            TableTitlecell = new PdfPCell();
            TableTitlecell.HorizontalAlignment = Element.ALIGN_LEFT;
            TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
            TableTitlecell.BorderWidth = 3;
            TableTitlecell.BorderWidthTop = 0;
            TableTitlecell.Colspan = 2;
            TableTitlecell.Padding = 0;

            //品牌与数量表格
            PdfPTable TableHead_Sub_A = new PdfPTable(new float[] { 5, 2, 3 });
            TableHead_Sub_A.WidthPercentage = 99;
            TableHead_Sub_A.DefaultCell.BorderColor = BaseColor.BLUE;
            TableHead_Sub_A.DefaultCell.BorderWidth = 0;
            TableHead_Sub_A.DefaultCell.BorderWidthBottom = 0;

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.PaddingLeft = 10;
            Paragraph Row4 = new Paragraph(Line.MatBrand, this.GetBaseFontItem("STZHONGS", 16));
            Row4.SetLeading(0, 1.2f);
            Table_Sub_Cell.AddElement(Row4);
            TableHead_Sub_A.AddCell(Table_Sub_Cell);

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.PaddingLeft = 10;
            Paragraph Row5 = new Paragraph("PCS", this.GetBaseFontItem("arial", 12));
            Table_Sub_Cell.AddElement(Row5);
            TableHead_Sub_A.AddCell(Table_Sub_Cell);

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Paragraph Row6 = new Paragraph(Line.Quantity.ToString(), this.GetBaseFontItem("arialbd", 16));
            Row6.SetLeading(0, 1.2f);
            Table_Sub_Cell.AddElement(Row6);
            TableHead_Sub_A.AddCell(Table_Sub_Cell);

            TableTitlecell.AddElement(TableHead_Sub_A);

            //到货日期
            PdfPTable TableHead_Sub_B = new PdfPTable(new float[] { 5, 5 });
            TableHead_Sub_B.WidthPercentage = 99;
            TableHead_Sub_B.DefaultCell.BorderColor = BaseColor.BLUE;
            TableHead_Sub_B.DefaultCell.BorderWidth = 0;
            TableHead_Sub_B.DefaultCell.BorderWidthBottom = 0;

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.PaddingLeft = 10;
            Paragraph Row7 = new Paragraph(" ", this.GetBaseFontItem("STZHONGS", 16));
            Row7.SetLeading(0, 1.2f);
            Table_Sub_Cell.AddElement(Row7);
            TableHead_Sub_B.AddCell(Table_Sub_Cell);

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.PaddingLeft = 10;
            Paragraph Row8 = new Paragraph(Line.Create_DT.ToString("yyyy.MM.dd"), this.GetBaseFontItem("STZHONGS", 16));
            Row8.SetLeading(0, 1.2f);
            Table_Sub_Cell.AddElement(Row8);
            TableHead_Sub_B.AddCell(Table_Sub_Cell);

            TableTitlecell.AddElement(TableHead_Sub_B);
            TableTitlecell.AddElement(new Paragraph("\n\r", this.GetBaseFontItem("arialbd", 16)));
            TableHead.AddCell(TableTitlecell);

            document.Add(TableHead);
            document.NewPage();

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;
        }

        public string Create_PDF_For_Material_Label(Material M)
        {
            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\" + M.MatID + "_" + M.WMS_Stock_Qty + "_产品标签.pdf";

            //第一步，创建一个 iTextSharp.text.Document对象的实例：
            Rectangle pageSize = new Rectangle(300, 200);
            Document document = new Document(pageSize, 20, 10, 15, 10);
            //Document document = new Document(PageSize.A7.Rotate(), 10, 10, 20, 10);

            //第二步，为该Document创建一个Writer实例： 
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //第三步，打开当前Document 
            document.Open();

            //第四步，为当前Document添加内容：

            //创建Table布局2列2行
            PdfPTable TableHead = new PdfPTable(new float[] { 2.5f, 7.5f });
            TableHead.WidthPercentage = 105;
            TableHead.DefaultCell.BorderColor = BaseColor.RED;
            TableHead.DefaultCell.BorderWidth = 0;

            //左侧标签区域
            PdfPCell TableTitlecell = new PdfPCell();
            TableTitlecell.HorizontalAlignment = Element.ALIGN_LEFT;
            TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
            TableTitlecell.BorderWidth = 0;
            TableTitlecell.BorderWidthBottom = 3;
            //TableTitlecell.BorderWidthLeft = 0;
            //TableTitlecell.BorderWidthTop = 0;
            string Host = HttpRuntime.AppDomainAppPath.ToString();
            Image Img = Image.GetInstance(Host + M.QRCode_Path);
            Img.Alignment = Image.ALIGN_LEFT;
            Img.Border = 0;
            TableTitlecell.PaddingRight = 7;
            TableTitlecell.PaddingBottom = 10;
            TableTitlecell.AddElement(Img);
            TableHead.AddCell(TableTitlecell);


            TableTitlecell = new PdfPCell();
            TableTitlecell.HorizontalAlignment = Element.ALIGN_LEFT;
            TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
            TableTitlecell.BorderColor = BaseColor.BLACK;
            TableTitlecell.BorderWidth = 3;
            TableTitlecell.BorderWidthBottom = 0;
            //TableTitlecell.BorderWidthTop = 3;
            //TableTitlecell.BorderWidthRight = 3;


            //右侧标题及产品型号
            PdfPTable TableHead_Sub = new PdfPTable(1);
            TableHead_Sub.WidthPercentage = 100;
            TableHead_Sub.DefaultCell.BorderColor = BaseColor.BLACK;
            TableHead_Sub.DefaultCell.BorderWidth = 0;

            PdfPCell Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.BorderWidth = 0;
            Table_Sub_Cell.BorderWidthBottom = 0.5f;
            Table_Sub_Cell.PaddingBottom = 10;
            Paragraph Row1 = new Paragraph("虹恩零头箱", this.GetBaseFontItem("STZHONGS", 18));
            Row1.SetLeading(0, 1);
            Table_Sub_Cell.AddElement(Row1);
            TableHead_Sub.AddCell(Table_Sub_Cell);

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            Table_Sub_Cell.BorderWidth = 0;
            Table_Sub_Cell.PaddingTop = 10;
            Paragraph Row2 = new Paragraph(M.MatSn, this.GetBaseFontItem("arial", 14));
            Row2.SetLeading(0, 1);
            Table_Sub_Cell.NoWrap = true;
            Table_Sub_Cell.AddElement(Row2);
            TableHead_Sub.AddCell(Table_Sub_Cell);
            TableTitlecell.AddElement(TableHead_Sub);
            TableHead.AddCell(TableTitlecell);

            //第二部部分
            TableTitlecell = new PdfPCell();
            TableTitlecell.HorizontalAlignment = Element.ALIGN_LEFT;
            TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
            TableTitlecell.BorderWidth = 3;
            TableTitlecell.BorderWidthTop = 0;
            TableTitlecell.Colspan = 2;
            TableTitlecell.Padding = 0;

            //品牌与数量表格
            PdfPTable TableHead_Sub_A = new PdfPTable(new float[] { 5, 2, 3 });
            TableHead_Sub_A.WidthPercentage = 99;
            TableHead_Sub_A.DefaultCell.BorderColor = BaseColor.BLUE;
            TableHead_Sub_A.DefaultCell.BorderWidth = 0;
            TableHead_Sub_A.DefaultCell.BorderWidthBottom = 0;

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.PaddingLeft = 10;
            Paragraph Row4 = new Paragraph(M.MatBrand, this.GetBaseFontItem("STZHONGS", 16));
            Row4.SetLeading(0, 1.2f);
            Table_Sub_Cell.AddElement(Row4);
            TableHead_Sub_A.AddCell(Table_Sub_Cell);

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.PaddingLeft = 10;
            Paragraph Row5 = new Paragraph("PCS", this.GetBaseFontItem("arial", 12));
            Table_Sub_Cell.AddElement(Row5);
            TableHead_Sub_A.AddCell(Table_Sub_Cell);


            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Paragraph Row6 = new Paragraph(M.WMS_Stock_Qty.ToString(), this.GetBaseFontItem("arialbd", 16));
            Row6.SetLeading(0, 1.2f);
            Table_Sub_Cell.AddElement(Row6);
            TableHead_Sub_A.AddCell(Table_Sub_Cell);

            TableTitlecell.AddElement(TableHead_Sub_A);

            //到货日期
            PdfPTable TableHead_Sub_B = new PdfPTable(new float[] { 5, 5 });
            TableHead_Sub_B.WidthPercentage = 99;
            TableHead_Sub_B.DefaultCell.BorderColor = BaseColor.BLUE;
            TableHead_Sub_B.DefaultCell.BorderWidth = 0;
            TableHead_Sub_B.DefaultCell.BorderWidthBottom = 0;

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.PaddingLeft = 10;
            Paragraph Row7 = new Paragraph(" ", this.GetBaseFontItem("STZHONGS", 16));
            Row7.SetLeading(0, 1.2f);
            Table_Sub_Cell.AddElement(Row7);
            TableHead_Sub_B.AddCell(Table_Sub_Cell);

            Table_Sub_Cell = new PdfPCell();
            Table_Sub_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Table_Sub_Cell.VerticalAlignment = Element.ALIGN_TOP;
            Table_Sub_Cell.PaddingLeft = 10;
            Paragraph Row8 = new Paragraph(DateTime.Now.ToString("yyyy.MM.dd"), this.GetBaseFontItem("STZHONGS", 16));
            Row8.SetLeading(0, 1.2f);
            Table_Sub_Cell.AddElement(Row8);
            TableHead_Sub_B.AddCell(Table_Sub_Cell);

            TableTitlecell.AddElement(TableHead_Sub_B);
            TableTitlecell.AddElement(new Paragraph("\n\r", this.GetBaseFontItem("arialbd", 16)));
            TableHead.AddCell(TableTitlecell);

            document.Add(TableHead);
            document.NewPage();

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;
        }
        
        //库位打印
        public string Create_PDF_For_WMS_Location(WMS_Location Loc)
        {
            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\" + Loc.Location + "_产品标签.pdf";

            //第一步，创建一个 iTextSharp.text.Document对象的实例：
            Rectangle pageSize = new Rectangle(370, 200);
            Document document = new Document(pageSize, 10, 10, 10, 10);

            //第二步，为该Document创建一个Writer实例： 
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //第三步，打开当前Document 
            document.Open();

            //第四步，为当前Document添加内容：

            //创建Table布局
            PdfPTable TableHead = new PdfPTable(1);
            TableHead.WidthPercentage = 100;
            TableHead.DefaultCell.Border = 0;

            PdfPCell TableTitlecell = new PdfPCell();
            TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
            TableTitlecell.Border = 0;
            string Host = HttpRuntime.AppDomainAppPath.ToString();
            Image Img = Image.GetInstance(Host + Loc.QRCode_Path);
            Img.Alignment = Image.ALIGN_CENTER;
            Img.Border = 0;
            Img.WidthPercentage = 30;
            TableTitlecell.AddElement(Img);
            TableHead.AddCell(TableTitlecell);
            document.Add(TableHead);

            PdfPTable TableBody = new PdfPTable(1);
            TableBody.WidthPercentage = 100;

            PdfPCell Tablecell = new PdfPCell();

            Tablecell = new PdfPCell(new Phrase(0, Loc.Location, this.GetBaseFontItem("STZHONGS", 66)));
            Tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
            Tablecell.VerticalAlignment = Element.ALIGN_TOP;
            Tablecell.BorderWidth = 0;
            TableBody.AddCell(Tablecell);

            document.Add(TableBody);

            document.NewPage();

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;
        }

        public string Create_PDF_For_WMS_Location_List(Guid LinkMainCID, List<Guid> IDList)
        {
            List<WMS_Location> List = db.WMS_Location.Where(x => x.LinkMainCID == LinkMainCID && IDList.Contains(x.Loc_ID)).ToList();

            foreach (var x in List)
            {
                x.QRCode_Path = QRCode.CreateQRCode_Location(x.Location, x.Loc_ID);
            }

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\库位标签.pdf";

            //第一步，创建一个 iTextSharp.text.Document对象的实例：
            Rectangle pageSize = new Rectangle(370, 200);
            Document document = new Document(pageSize, 10, 10, 10, 10);

            //第二步，为该Document创建一个Writer实例： 
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //第三步，打开当前Document 
            document.Open();

            //第四步，为当前Document添加内容：

            foreach (var x in List.OrderBy(x => x.Location).ToList())
            {
                //创建Table布局
                PdfPTable TableHead = new PdfPTable(1);
                TableHead.WidthPercentage = 100;
                TableHead.DefaultCell.Border = 0;

                PdfPCell TableTitlecell = new PdfPCell();
                TableTitlecell.VerticalAlignment = Element.ALIGN_TOP;
                TableTitlecell.Border = 0;
                string Host = HttpRuntime.AppDomainAppPath.ToString();
                Image Img = Image.GetInstance(Host + x.QRCode_Path);
                Img.Alignment = Image.ALIGN_CENTER;
                Img.Border = 0;
                Img.WidthPercentage = 30;
                TableTitlecell.AddElement(Img);
                TableHead.AddCell(TableTitlecell);
                document.Add(TableHead);

                PdfPTable TableBody = new PdfPTable(1);
                TableBody.WidthPercentage = 100;

                PdfPCell Tablecell = new PdfPCell();

                Tablecell = new PdfPCell(new Phrase(0, x.Location, this.GetBaseFontItem("STZHONGS", 66)));
                Tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                Tablecell.VerticalAlignment = Element.ALIGN_TOP;
                Tablecell.BorderWidth = 0;
                TableBody.AddCell(Tablecell);

                document.Add(TableBody);

                document.NewPage();
            }

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;
        }

        //出库送货单(总)
        public string Create_PDF_For_WMS_Out_Task(WMS_Out_Head Head)
        {
            List<WMS_Out_Line> Line_List = db.WMS_Out_Line.Where(x => x.Link_Head_ID == Head.Head_ID).ToList();

            MainCompany Com = db.MainCompany.Find(Head.LinkMainCID);
            if (Com == null) { throw new Exception("MainCompany is null"); }

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();

            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\出库任务明细.pdf";

            //创建一个Document   并设定它的页面大小和边距
            Document document = new Document(PageSize.A4, 20, 20, 20, 10);

            //创建一个Writer实例
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //打开Document
            document.Open();

            //标题
            Paragraph Row = new Paragraph("送货单明细", this.GetBaseFontItem("STZHONGS", 20));
            Row.Alignment = Element.ALIGN_CENTER;
            Row.SpacingAfter = 10f;
            Row.SpacingBefore = 10f;
            document.Add(Row);

            Row = new Paragraph(Com.MainCompanyName, this.GetBaseFontItem("STZHONGS", 18));
            Row.Alignment = Element.ALIGN_CENTER;
            Row.SpacingAfter = 10f;
            document.Add(Row);

            //客户名称  电话
            PdfPTable TableHead = new PdfPTable(new float[] { 1, 5, 1, 2 });
            TableHead.TotalWidth = 550;
            TableHead.WidthPercentage = 100;
            TableHead.LockedWidth = true;

            PdfPCell cell = new PdfPCell(new Paragraph("客户名称： ", this.GetBaseFontItem("STZHONGS", 14)));
            cell.BorderWidth = 0;
            cell.NoWrap = true;
            cell.SetLeading(1.5f, 1.5f);
            TableHead.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" "+Head.Customer_Name, this.GetBaseFontItem("STZHONGS", 14)));
            cell.BorderWidth = 0;
            cell.NoWrap = true;
            cell.SetLeading(1.5f, 1.5f);
            TableHead.AddCell(cell);

            cell = new PdfPCell(new Paragraph("电话： ", this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.NoWrap = true;
            cell.SetLeading(1.5f, 1.5f);
            TableHead.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" " + Head.Customer_Tel, this.GetBaseFontItem("STZHONGS", 14)));
            cell.BorderWidth = 0;
            cell.NoWrap = true;
            cell.SetLeading(1.5f, 1.5f);
            TableHead.AddCell(cell);

            document.Add(TableHead);

            //客户地址
            PdfPTable TableMid = new PdfPTable(new float[] { 1, 9 });
            TableMid.TotalWidth = 550;
            TableMid.WidthPercentage = 100;
            TableMid.LockedWidth = true;

            cell = new PdfPCell(new Paragraph("客户地址： ", this.GetBaseFontItem("STZHONGS", 14)));
            cell.BorderWidth = 0;
            cell.NoWrap = true;
            cell.SetLeading(1.5f, 1.5f);
            TableMid.AddCell(cell);

            cell = new PdfPCell(new Paragraph(" " + Head.Customer_Address, this.GetBaseFontItem("STZHONGS", 14)));
            cell.BorderWidth = 0;
            cell.NoWrap = true;
            cell.SetLeading(1.5f, 1.5f);
            TableMid.AddCell(cell);

            document.Add(TableMid);

            //空一行
            Row = new Paragraph("\n", this.GetBaseFontItem("arialbd", 14));
            document.Add(Row);

            //表格
            PdfPTable TableFoot = new PdfPTable(new float[] { 70, 240, 80, 70, 90 });
            TableFoot.TotalWidth = 550;
            TableFoot.WidthPercentage = 100;
            TableFoot.LockedWidth = true;

            cell = new PdfPCell(new Paragraph("产品名称", this.GetBaseFontItem("STZHONGS", 14)));
            cell.PaddingLeft = 8;
            cell.MinimumHeight = 25;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            cell.MinimumHeight = 25;
            TableFoot.AddCell(cell);

            cell = new PdfPCell(new Paragraph("产品型号", this.GetBaseFontItem("STZHONGS", 14)));
            cell.PaddingLeft = 8;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            cell.MinimumHeight = 25;
            TableFoot.AddCell(cell);

            cell = new PdfPCell(new Paragraph("数量", this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingRight = 8;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            cell.MinimumHeight = 25;
            TableFoot.AddCell(cell);

            cell = new PdfPCell(new Paragraph("品牌", this.GetBaseFontItem("STZHONGS", 14)));
            cell.PaddingLeft = 8;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            cell.MinimumHeight = 25;
            TableFoot.AddCell(cell);

            cell = new PdfPCell(new Paragraph("单价", this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingRight = 8;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            cell.MinimumHeight = 25;
            TableFoot.AddCell(cell);

            foreach (var x in Line_List)
            {
                cell = new PdfPCell(new Paragraph(x.MatName, this.GetBaseFontItem("STZHONGS", 14)));
                cell.PaddingLeft = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableFoot.AddCell(cell);

                cell = new PdfPCell(new Paragraph(x.MatSn, this.GetBaseFontItem("consola", 14)));
                cell.PaddingLeft = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableFoot.AddCell(cell);

                cell = new PdfPCell(new Paragraph(x.Quantity.ToString(), this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableFoot.AddCell(cell);

                cell = new PdfPCell(new Paragraph(x.MatBrand, this.GetBaseFontItem("STZHONGS", 14)));
                cell.PaddingLeft = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableFoot.AddCell(cell);

                cell = new PdfPCell(new Paragraph(x.Price.ToString("N2"), this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableFoot.AddCell(cell);

            }

            document.Add(TableFoot);


            //将表格添加到pdf文档中
            //document.Add();

            //给PDF文件添加标题
            document.AddTitle(Com.MainCompanyName);
            //创建一个新页面
            document.NewPage();

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;
        }

        //出库任务明细
        public string Create_PDF_For_WMS_Out_Task_By_Tray(WMS_Out_Head Head, string Tray_No)
        {
            List<WMS_Out_Scan> Line_List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No).ToList();

            List<WMS_Track> Track_List = db.WMS_Track.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No).ToList();
            WMS_Track Track = Track_List.FirstOrDefault();
            Track = Track == null ? new WMS_Track() : Track;

            MainCompany Com = db.MainCompany.Find(Head.LinkMainCID);
            if (Com == null) { throw new Exception("MainCompany is null"); }

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();

            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\" + Tray_No + "_出库明细.pdf";

            //创建一个Document   并设定它的页面大小和边距
            Document document = new Document(PageSize.A4, 20, 20, 20, 10);

            //创建一个Writer实例
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //打开Document
            document.Open();

            //标题
            Paragraph Row = new Paragraph("送货单明细", this.GetBaseFontItem("STZHONGS", 20));
            Row.Alignment = Element.ALIGN_CENTER;
            Row.SpacingAfter = 10f;
            Row.SpacingBefore = 10f;
            document.Add(Row);

            Row = new Paragraph(Com.MainCompanyName, this.GetBaseFontItem("STZHONGS", 16));
            Row.Alignment = Element.ALIGN_CENTER;
            Row.SpacingAfter = 10f;
            document.Add(Row);

            //客户名称  电话
            PdfPTable TableHead = new PdfPTable(new float[] { 1, 5, 1, 1 });
            TableHead.TotalWidth = 550;
            TableHead.WidthPercentage = 100;
            TableHead.LockedWidth = true;

            PdfPCell cell = new PdfPCell(new Paragraph("客户名称：", this.GetBaseFontItem("STZHONGS", 14)));
            cell.BorderWidth = 0;
            cell.NoWrap = true;
            cell.SetLeading(1.5f, 1.5f);
            TableHead.AddCell(cell);

            cell = new PdfPCell(new Paragraph(Head.Customer_Name, this.GetBaseFontItem("STZHONGS", 14)));
            cell.BorderWidth = 0;
            cell.NoWrap = true;
            cell.SetLeading(1.5f, 1.5f);
            TableHead.AddCell(cell);

            if (Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按托.ToString())
            {
                cell = new PdfPCell(new Paragraph("托号：", this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.BorderWidth = 0;
                cell.NoWrap = true;
                cell.SetLeading(1.5f, 1.5f);
                TableHead.AddCell(cell);
            }
            else if(Head.Scan_Mat_Type == Scan_Mat_Type_Enum.按箱.ToString())
            {
                cell = new PdfPCell(new Paragraph("箱号：", this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.BorderWidth = 0;
                cell.NoWrap = true;
                cell.SetLeading(1.5f, 1.5f);
                TableHead.AddCell(cell);
            }

            cell = new PdfPCell(new Paragraph(Tray_No, this.GetBaseFontItem("STZHONGS", 14)));
            cell.BorderWidth = 0;
            cell.NoWrap = true;
            cell.SetLeading(1.5f, 1.5f);
            TableHead.AddCell(cell);
            document.Add(TableHead);

            if (Head.Logistics_Mode != Logistics_Out_Mode_Enum.自送.ToString())
            {
                PdfPTable TableMid_C = new PdfPTable(new float[] { 1, 5, 1, 1 });
                TableMid_C.TotalWidth = 550;
                TableMid_C.WidthPercentage = 100;
                TableMid_C.LockedWidth = true;

                cell = new PdfPCell(new Paragraph("快递单号：", this.GetBaseFontItem("STZHONGS", 14)));
                cell.BorderWidth = 0;
                cell.NoWrap = true;
                cell.SetLeading(1.5f, 1.5f);
                TableMid_C.AddCell(cell);

                cell = new PdfPCell(new Paragraph(Track.Tracking_No, this.GetBaseFontItem("STZHONGS", 14)));
                cell.BorderWidth = 0;
                cell.NoWrap = true;
                cell.SetLeading(1.5f, 1.5f);
                TableMid_C.AddCell(cell);

                cell = new PdfPCell(new Paragraph("重量：", this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.BorderWidth = 0;
                cell.NoWrap = true;
                cell.SetLeading(1.5f, 1.5f);
                TableMid_C.AddCell(cell);

                cell = new PdfPCell(new Paragraph(Track.Weight.ToString("N2"), this.GetBaseFontItem("STZHONGS", 14)));
                cell.BorderWidth = 0;
                cell.NoWrap = true;
                cell.SetLeading(1.5f, 1.5f);
                TableMid_C.AddCell(cell);

                document.Add(TableMid_C);
            }

            //空一行
            Row = new Paragraph("\n", this.GetBaseFontItem("arialbd", 14));
            document.Add(Row);

            //表格
            PdfPTable TableFoot = new PdfPTable(new float[] { 5, 1 });
            TableFoot.TotalWidth = 550;
            TableFoot.WidthPercentage = 100;
            TableFoot.LockedWidth = true;

            cell = new PdfPCell(new Paragraph("产品型号", this.GetBaseFontItem("STZHONGS", 14)));
            cell.PaddingLeft = 8;
            cell.MinimumHeight = 25;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            TableFoot.AddCell(cell);

            cell = new PdfPCell(new Paragraph("数量", this.GetBaseFontItem("STZHONGS", 14)));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingRight = 8;
            cell.MinimumHeight = 25;
            cell.NoWrap = true;
            cell.SetLeading(1f, 1f);
            TableFoot.AddCell(cell);

            foreach (var x in Line_List)
            {
                cell = new PdfPCell(new Paragraph(x.MatSn, this.GetBaseFontItem("STZHONGS", 14)));
                cell.PaddingLeft = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableFoot.AddCell(cell);

                cell = new PdfPCell(new Paragraph(x.Scan_Quantity.ToString(), this.GetBaseFontItem("STZHONGS", 14)));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 8;
                cell.MinimumHeight = 25;
                cell.NoWrap = true;
                cell.SetLeading(1f, 1f);
                TableFoot.AddCell(cell);
            }

            document.Add(TableFoot);


            //将表格添加到pdf文档中
            //document.Add();

            //给PDF文件添加标题
            document.AddTitle(Com.MainCompanyName);
            //创建一个新页面
            document.NewPage();

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;

        }

        //出库标签
        public string Create_PDF_For_WMS_Out_Task_By_Tray_Label(WMS_Out_Head Head, string Tray_No)
        {
            Tray_No = Tray_No.Trim();
            List<WMS_Out_Scan> Scan_List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No).ToList();
            var Group = from x in Scan_List
                        group x by new { x.MatSn } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Scan_Quantity_Sum = G.Sum(c => c.Scan_Quantity)
                        };

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\" + Tray_No + "_产品标签.pdf";

            //第一步，创建一个 iTextSharp.text.Document对象的实例：
            Rectangle pageSize = new Rectangle(300, 200);
            Document document = new Document(pageSize, 20, 10, 15, 10);
            //Document document = new Document(PageSize.A7.Rotate(), 10, 10, 20, 10);

            //第二步，为该Document创建一个Writer实例： 
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //第三步，打开当前Document 
            document.Open();

            //第四步，为当前Document添加内容：

            int PageLineSize = 5;
            int PageIndex = 0;
            int PageTotal = Convert.ToInt32(Math.Ceiling(Group.Count() / (decimal)PageLineSize));

            for (int i = 1; i <= PageTotal; i++)
            {
                PageIndex++;

                PdfPTable Table_Test = new PdfPTable(new float[] { 8, 2 });//设置表格各列宽度  当前为4列
                Table_Test.WidthPercentage = 100;//宽度百分比
                Table_Test.TotalWidth = 280; //表格总宽度
                Table_Test.LockedWidth = true;//锁定宽度

                PdfPCell cell = new PdfPCell(new Paragraph("产品型号", this.GetBaseFontItem("STZHONGS", 11)));
                cell.NoWrap = true;
                cell.PaddingBottom = 3;
                cell.PaddingTop = 3;
                cell.SetLeading(1.2f, 1.2f);
                Table_Test.AddCell(cell);

                cell = new PdfPCell(new Paragraph("数量", this.GetBaseFontItem("STZHONGS", 11)));
                cell.NoWrap = true;
                cell.PaddingBottom = 3;
                cell.PaddingTop = 3;
                cell.HorizontalAlignment = 2;
                cell.SetLeading(1.2f, 1.2f);
                Table_Test.AddCell(cell);

                foreach (var x in Group.Skip((PageIndex - 1) * PageLineSize).Take(PageLineSize).ToList())
                {
                    cell = new PdfPCell(new Paragraph(x.MatSn, this.GetBaseFontItem("consola", 11)));
                    cell.NoWrap = true;
                    cell.PaddingBottom = 3;
                    cell.PaddingTop = 3;
                    cell.SetLeading(1.2f, 1.2f);
                    Table_Test.AddCell(cell);

                    cell = new PdfPCell(new Paragraph(x.Scan_Quantity_Sum.ToString(), this.GetBaseFontItem("STZHONGS", 11)));
                    cell.NoWrap = true;
                    cell.PaddingBottom = 3;
                    cell.PaddingTop = 3;
                    cell.HorizontalAlignment = 2;
                    cell.SetLeading(1.2f, 1.2f);
                    Table_Test.AddCell(cell);
                }

                document.Add(Table_Test);
                document.NewPage();
            }

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;

        }

        //出库标签
        public string Create_PDF_For_WMS_Out_Task_By_Tray_Label_With_Case(WMS_Out_Head Head, string Tray_No, string Case_No)
        {
            Tray_No = Tray_No.Trim();
            Case_No = Case_No.Trim();

            List<WMS_Out_Scan> Scan_List = db.WMS_Out_Scan.Where(x => x.Link_Head_ID == Head.Head_ID && x.Tray_No == Tray_No && x.Case_No == Case_No).ToList();
            var Group = from x in Scan_List
                        group x by new { x.MatSn } into G
                        select new
                        {
                            MatSn = G.Key.MatSn,
                            Scan_Quantity_Sum = G.Sum(c => c.Scan_Quantity)
                        };

            string PDFPath = string.Empty;

            //PDF文件路径
            string PDFFilePath = this.AutoCreatPDFDownloadDir();
            //PDF文件名称
            string PDFFileNameStr = PDFFilePath + "\\" + Tray_No + "_" + Case_No + "_产品标签.pdf";

            //第一步，创建一个 iTextSharp.text.Document对象的实例：
            Rectangle pageSize = new Rectangle(300, 200);
            Document document = new Document(pageSize, 20, 10, 15, 10);
            //Document document = new Document(PageSize.A7.Rotate(), 10, 10, 20, 10);

            //第二步，为该Document创建一个Writer实例： 
            PdfWriter PdfWriter = PdfWriter.GetInstance(document, new FileStream(PDFFileNameStr, FileMode.Create));

            //第三步，打开当前Document 
            document.Open();

            //第四步，为当前Document添加内容：

            int PageLineSize = 5;
            int PageIndex = 0;
            int PageTotal = Convert.ToInt32(Math.Ceiling(Group.Count() / (decimal)PageLineSize));

            for (int i = 1; i <= PageTotal; i++)
            {
                PageIndex++;

                PdfPTable Table_Test = new PdfPTable(new float[] { 8, 2 });//设置表格各列宽度  当前为4列
                Table_Test.WidthPercentage = 100;//宽度百分比
                Table_Test.TotalWidth = 280; //表格总宽度
                Table_Test.LockedWidth = true;//锁定宽度

                PdfPCell cell = new PdfPCell(new Paragraph("产品型号", this.GetBaseFontItem("STZHONGS", 11)));
                cell.NoWrap = true;
                cell.PaddingBottom = 3;
                cell.PaddingTop = 3;
                cell.SetLeading(1.2f, 1.2f);
                Table_Test.AddCell(cell);

                cell = new PdfPCell(new Paragraph("数量", this.GetBaseFontItem("STZHONGS", 11)));
                cell.NoWrap = true;
                cell.PaddingBottom = 3;
                cell.PaddingTop = 3;
                cell.HorizontalAlignment = 2;
                cell.SetLeading(1.2f, 1.2f);
                Table_Test.AddCell(cell);

                foreach (var x in Group.Skip((PageIndex - 1) * PageLineSize).Take(PageLineSize).ToList())
                {
                    cell = new PdfPCell(new Paragraph(x.MatSn, this.GetBaseFontItem("consola", 11)));
                    cell.NoWrap = true;
                    cell.PaddingBottom = 3;
                    cell.PaddingTop = 3;
                    cell.SetLeading(1.2f, 1.2f);
                    Table_Test.AddCell(cell);

                    cell = new PdfPCell(new Paragraph(x.Scan_Quantity_Sum.ToString(), this.GetBaseFontItem("STZHONGS", 11)));
                    cell.NoWrap = true;
                    cell.PaddingBottom = 3;
                    cell.PaddingTop = 3;
                    cell.HorizontalAlignment = 2;
                    cell.SetLeading(1.2f, 1.2f);
                    Table_Test.AddCell(cell);
                }

                document.Add(Table_Test);
                document.NewPage();
            }

            //第五步，关闭Document    
            document.Close();
            document.Dispose();
            PDFPath = PDFFileNameStr;
            return PDFPath;

        }

        //获取字体
        private Font GetBaseFontItem(string FontStr, int Size)
        {
            //arial 正常
            //ariali 斜体
            //arialbd 粗体
            //arialbi 粗斜体

            //FZDH 方正大黑 
            //simhei 黑体

            //simfang 新仿宋

            //consola 
            string Fontpath = string.Empty;
            Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\simfang.ttf";
            BaseFont BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            switch (FontStr)
            {
                case "arial":
                    Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\arial.ttf";
                    BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    break;

                case "arialbd":
                    Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\arialbd.ttf";
                    BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    break;

                case "arialbi":
                    Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\arialbi.ttf";
                    BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    break;

                case "ariali":
                    Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\ariali.ttf";
                    BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    break;

                case "simhei":
                    Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\simhei.ttf";
                    BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    break;

                case "FZDH":
                    Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\FZDH.ttf";
                    BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    break;

                case "FZWB":
                    Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\FZWB.ttf";
                    BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    break;

                case "STZHONGS":
                    Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\STZHONGS.ttf";
                    BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    break;

                case "consola":
                    Fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PdfFont\\consola.ttf";
                    BaseFontItem = BaseFont.CreateFont(Fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    break;

                default:
                    //DoNothing;
                    break;
            }

            Font F = new Font(BaseFontItem, Size);
            return F;
        }

        //自动检测上传目录，并创建后返回物理路径字符串
        private string AutoCreatPDFDownloadDir()
        {
            //获取应用根目录
            string upLoadGlobDirInfo = HttpRuntime.AppDomainAppPath.ToString() + "Temp_PDF";

            //创建对应子文件目录
            string UpLoadFilePath = upLoadGlobDirInfo + "\\" + DateTime.Now.ToString("yyyyMMdd");

            //验证目录是否存在，如不存在则自动进行创建
            if (!Directory.Exists(UpLoadFilePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(UpLoadFilePath);
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }
            }
            return UpLoadFilePath;
        }
    }
}
