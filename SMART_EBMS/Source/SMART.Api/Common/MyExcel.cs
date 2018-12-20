using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using NPOI;
using NPOI.POIFS;
using NPOI.HSSF;
using NPOI.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using System.Web;

namespace SMART.Api
{
    public static class MyExcel
    {
        //<remarks>NPOI认为Excel的第一个单元格是：(0，0)</remarks>
        public static string CreateNewExcel(DataTable table)
        {
            //文件路径
            //获取应用根目录
            string upLoadGlobDirInfo = HttpRuntime.AppDomainAppPath.ToString() + "Excel";

            //创建对应子文件目录
            string UpLoadFilePath = upLoadGlobDirInfo + "\\" + DateTime.Now.ToString("yyyyMMdd");

            //验证目录是否存在，如不存在则自动进行创建
            if (!Directory.Exists(UpLoadFilePath))
            {
                try
                {
                    Directory.CreateDirectory(UpLoadFilePath);
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }
            }


            string GuidStr = MyGUID.NewGUID().ToString();
            string ExcelFilePath = UpLoadFilePath;

            //文件名称
            string ExcelFileNameStr = ExcelFilePath + "\\" + GuidStr + ".xlsx";

            MemoryStream ms = new MemoryStream();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            IFont font = workbook.CreateFont();
            font.FontName = "consola";

            //定义样式
            ICellStyle CellStyle = workbook.CreateCellStyle();
            CellStyle.BorderBottom = BorderStyle.Thin;
            CellStyle.BorderLeft = BorderStyle.Thin;
            CellStyle.BorderRight = BorderStyle.Thin;
            CellStyle.BorderTop = BorderStyle.Thin;
            CellStyle.BottomBorderColor = HSSFColor.Black.Index;
            CellStyle.LeftBorderColor = HSSFColor.Black.Index;
            CellStyle.RightBorderColor = HSSFColor.Black.Index;
            CellStyle.TopBorderColor = HSSFColor.Black.Index;
            CellStyle.SetFont(font);

            // handling header.
            foreach (DataColumn column in table.Columns)
            {
                //If Caption not set, returns the ColumnName value
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                
                //定义单元格边框
                headerRow.Cells[column.Ordinal].CellStyle = CellStyle;

                // handling value.
                int rowIndex = 1;
                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn col in table.Columns)
                    {
                        
                        try
                        {
                            if (row[col].ToString().StartsWith("0."))
                            {
                                dataRow.CreateCell(col.Ordinal).SetCellValue(Convert.ToDouble(row[col].ToString()));
                            }
                            else if (row[col].ToString().StartsWith("0") == false)
                            {
                                dataRow.CreateCell(col.Ordinal).SetCellValue(Convert.ToDouble(row[col].ToString()));
                            }
                            else
                            {
                                dataRow.CreateCell(col.Ordinal).SetCellValue(row[col].ToString());
                            }
                        }
                        catch
                        {
                            dataRow.CreateCell(col.Ordinal).SetCellValue(row[col].ToString());
                        }
                        //定义单元格边框
                        dataRow.Cells[col.Ordinal].CellStyle = CellStyle;
                    }
                    rowIndex++;
                }

                //获取当前列的宽度，然后对比本列的长度，取最大值
                for (int columnNum = 0; columnNum < table.Columns.Count; columnNum++)
                {
                    int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                    for (int rowNum = 0; rowNum <= sheet.LastRowNum; rowNum++)
                    {
                        IRow currentRow;
                        //当前行未被使用过
                        if (sheet.GetRow(rowNum) == null)
                        {
                            currentRow = sheet.CreateRow(rowNum);
                        }
                        else
                        {
                            currentRow = sheet.GetRow(rowNum);
                        }

                        if (currentRow.GetCell(columnNum) != null)
                        {
                            ICell currentCell = currentRow.GetCell(columnNum);
                            int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;
                          
                            if (columnWidth < length)
                            {
                                columnWidth = length;
                            }
                        }
                    }

                    if (columnWidth * 256 < 255 * 256)
                    {
                        sheet.SetColumnWidth(columnNum, columnWidth * 256);
                    }
                    else
                    {
                        sheet.SetColumnWidth(columnNum, 255 * 256);
                    }
                }
            }

            workbook.Write(ms);
            ms.Flush();

            FileStream fs = new FileStream(ExcelFileNameStr, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();
            fs.Write(data, 0, data.Length);
            fs.Flush();
            data = null;

            ms.Close();
            ms.Dispose();
            fs.Close();
            fs.Dispose();
            return ExcelFileNameStr;
        }
    }
}
