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
    public interface IMaterialService
    {
        Material Get_Material_Item_DB(Guid MatID);
        Material Get_Material_Item_With_QRCodePath(Guid MatID, int Quantity);
        Material Get_Material_Item_With_QRCodePath(Guid LinkMainCID, string MatSn);
        Material Get_Material_Item(Guid MatID);
        Material Get_Material_Empty(Guid BID);
        string Get_Material_QRCodePath(string MatSn);
        List<Material> Get_Material_List_By_MatSn(Material_Filter MF);
        PageList<Material> Get_Material_Stand_PageList(Material_Filter MF);
        PageList<Material> Get_Material_PageList(Material_Filter MF);
        PageList<Material> Get_Material_PageList_For_MySale(Material_Filter MF);
        PageList<Material> Get_Material_PageList_For_MatImg(Material_Filter MF);

        Guid Create_Material(Material Mat, User U);
        void Set_Material_Base(Guid MatID, Material Mat, User U);
        void Set_Material_Price(Guid MatID, Material Mat, List<Material_CODE> CODE_List, User U);
        void Delete_Material(Guid MatID);

        void Get_Material_List_Img_And_Cat(List<Material> RowList);
        void Get_Material_List_Wms_Info(List<Material> RowList);

        void Set_Mat_CatID_Batch(List<Guid> MatIDList, Guid CatID);

        string Get_Material_Price_List_ToExcel(Material_Filter MF);
        string Get_Mat_Excel_ToExcel(List<Mat_Excel_Line> LineList);
        PageList<Mat_Excel> Get_Mat_Excel_PageList(Material_Filter MF);
        Mat_Excel Get_Mat_Excel(Guid MEID, Material_Filter MF);
        List<Mat_Price_Change> Get_Materil_List_For_Price_Change(Guid MatID, int TakeCount);

        Guid Mat_Excel_UpLoad(HttpPostedFileBase ExcelFile, Guid UID, Guid BID);
        void Mat_Excel_UpLoad_To_DB(Guid MEID);
        Guid Mat_Price_Excel_UpLoad(HttpPostedFileBase ExcelFile, Guid UID, Guid BID);
        void Mat_Price_Excel_UpLoad_To_DB(Guid MEID);

        //产品名称设置
        Material_Name Get_Material_Name_Item_DB(Guid Name_ID);
        PageList<Material_Name> Get_Material_Name_PageList(Material_Filter MF);
        List<string> Get_Material_Name_Str_List(Guid LinkMainCID);
        void Create_Mat_Name(Material_Name Mat, Guid UID);
        void Set_Material_Name_Item(Guid Name_ID, Material_Name Mat);
        void Delete_Material_Name(Guid Name_ID);


        //产品创建（临时）
        Guid Create_Material_Temp(Material Mat, User U);
        void Set_Material_Base_Temp(Guid MatID, Material Mat, User U);
        void Delete_Material_Temp(Guid MatID);
        void Mat_Excel_UpLoad_Temp_Various_Brands(HttpPostedFileBase ExcelFile, Guid UID);
        void Mat_Excel_UpLoad_Temp(HttpPostedFileBase ExcelFile, Guid UID, Guid BID, string MatName);
        PageList<Material> Get_Material_PageList_Temp(Material_Filter MF);
        void Set_Mat_Other_MatSn_ALL(Guid LinkMainCID);
        string Get_Mat_List_ToExcel(Guid MainCID, string Brand);
    }

    public partial class MaterialService : IMaterialService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class MaterialService : IMaterialService
    {
        public Material Get_Material_Item_DB(Guid MatID)
        {
            Material Mat = db.Material.Find(MatID);
            Mat = Mat == null ? new Material() : Mat;
            return Mat;
        }

        public Material Get_Material_Item_With_QRCodePath(Guid LinkMainCID, string MatSn)
        {
            Material Mat = db.Material.Where(x => x.LinkMainCID == LinkMainCID && x.MatSn == MatSn).FirstOrDefault();
            Mat = Mat == null ? new Material() : Mat;
            string Str = "HONGEN/" + Mat.MatSn + "/" + Mat.Pack_Qty + "/" + Mat.MatBrand;
            Mat.QRCode_Path = QRCode.CreateQRCode_With_No(Str, Mat.MatID);
            return Mat;
        }


        public Material Get_Material_Item_With_QRCodePath(Guid MatID, int Quantity)
        {
            if (Quantity <= 0) { throw new Exception("产品数量不小于0"); }

            Material Mat = db.Material.Find(MatID);
            Mat = Mat == null ? new Material() : Mat;
            Mat.WMS_Stock_Qty = Quantity;
            string Str = "HONGEN/" + Mat.MatSn + "/" + Mat.WMS_Stock_Qty + "/" + Mat.MatBrand;
            Mat.QRCode_Path = QRCode.CreateQRCode_With_No(Str, Mat.MatID);
            return Mat;
        }

        public string Get_Material_QRCodePath(string MatSn)
        {
            Guid TempID = MyGUID.NewGUID();
            MatSn = MatSn.Trim();
            string QRCode_Path = QRCode.CreateQRCode_With_No(MatSn, TempID);
            return QRCode_Path;
        }

        public Material Get_Material_Item(Guid MatID)
        {
            Material Mat = db.Material.Find(MatID);
            Mat = Mat == null ? new Material() : Mat;

            Brand B = db.Brand.Find(Mat.Link_BID);
            B = B == null ? new Brand() : B;
            Mat.MatSn_Length_Min = B.MatSn_Length_Min;
            Mat.MatSn_Length_Max = B.MatSn_Length_Max;

            //WMS_Stock WS = db.WMS_Stock.Find(MatID);
            //WS = WS == null ? new WMS_Stock() : WS;
            //Mat.WMS_Stock_Qty = WS.Quantity;

            MatImage MatIMG = db.MatImage.Find(Mat.Link_IMGID);
            MatIMG = MatIMG == null ? new MatImage() : MatIMG;
            Mat.MatSourceImgPath = MatIMG.MatSourceImgPath;
            Mat.MatImgPath = MatIMG.MatImgPath;
            Mat.MatThumbImgPath = MatIMG.MatThumbImgPath;

            Mat.Price_Cost_Ref_QY = Mat.Price_Cost_Ref;

            try { Mat.Price_Retail = Mat.Price_Cost_Ref_Vat * Mat.Price_Retail_Rate; } catch { }
            try { Mat.Price_Trade_A = Mat.Price_Cost_Ref_Vat * Mat.Price_Trade_A_Rate; } catch { }
            try { Mat.Price_Trade_B = Mat.Price_Cost_Ref_Vat * Mat.Price_Trade_B_Rate; } catch { }
            try { Mat.Price_Trade_NoTax = Mat.Price_Cost_Ref_Vat * Mat.Price_Trade_NoTax_Rate; } catch { }

            Mat.Mat_CODE_List = db.Material_CODE.Where(x => x.Link_MatID == Mat.MatID).ToList().OrderBy(x => x.Create_DT).ToList();
            int Line_Number = 0;
            foreach (var x in Mat.Mat_CODE_List)
            {
                Line_Number++;
                x.Line_Number = Line_Number;
            }

            MatImage_Detail Detail = db.MatImage_Detail.Find(Mat.Link_IMDID);
            Detail = Detail == null ? new MatImage_Detail() : Detail;
            Mat.MoreDetail = Detail.Detail_Html_Str;

            Mat.Mat_Name_List = this.Get_Material_Name_Str_List(Mat.LinkMainCID);

            return Mat;
        }

        public Material Get_Material_Empty(Guid BID)
        {
            Brand B = db.Brand.Find(BID);
            B = B == null ? new Brand() : B;

            Material Mat = new Material();
            Mat.Link_BID = B.BID;
            Mat.MatBrand = B.Brand_Name;
            Mat.MatSn_Length_Min = B.MatSn_Length_Min;
            Mat.MatSn_Length_Max = B.MatSn_Length_Max;

            Mat.Mat_Name_List = this.Get_Material_Name_Str_List(B.LinkMainCID);

            return Mat;
        }

        public PageList<Material> Get_Material_Stand_PageList(Material_Filter MF)
        {
            var query = db.Material.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            var query_MatSales = db.MatSales_Lib.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Sales_UID == MF.Sales_UID).AsQueryable();
            if (MF.IsMySales == MySales_Enum.IsMySales.ToString())
            {
                List<Guid> SalesMatIDList = query_MatSales.Select(x => x.MatID).ToList();
                query = query.Where(x => SalesMatIDList.Contains(x.MatID)).AsQueryable();
            }
            else if (MF.IsMySales == MySales_Enum.IsNotMySales.ToString())
            {
                List<Guid> SalesMatIDList = query_MatSales.Select(x => x.MatID).ToList();
                query = query.Where(x => SalesMatIDList.Contains(x.MatID) == false).AsQueryable();
            }

            if (MF.CatID != Guid.Empty)
            {
                query = query.Where(x => x.CatID == MF.CatID).AsQueryable();
            }
            else if (MF.CatID_Str == Cat_Enum.NoneCat.ToString())
            {
                query = query.Where(x => x.CatID == Guid.Empty).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                query = query.Where(m => m.MatBrand == MF.MatBrand).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Other_MatSn))
            {
                query = query.Where(m => m.Other_MatSn.Contains(MF.Other_MatSn)).AsQueryable();
            }

            if (MF.Link_BID != Guid.Empty)
            {
                query = query.Where(m => m.Link_BID == MF.Link_BID).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(m => m.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatName))
            {
                query = query.Where(m => m.MatName.Contains(MF.MatName)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.CODE))
            {
                List<Guid> CODE_MatID_List = db.Material_CODE.Where(x => x.CODE.Contains(MF.CODE)).Select(x => x.Link_MatID).Distinct().ToList();
                if (CODE_MatID_List.Any())
                {
                    query = query.Where(m => CODE_MatID_List.Contains(m.MatID)).AsQueryable();
                }
            }

            if (!string.IsNullOrEmpty(MF.PC))
            {
                query = query.Where(m => m.PC.Contains(MF.PC)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.PC_Place))
            {
                query = query.Where(m => m.PC_Place.Contains(MF.PC_Place)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.PC_Mon_Type))
            {
                query = query.Where(m => m.PC_Mon_Type.Contains(MF.PC_Mon_Type)).AsQueryable();
            }

            if (MF.Price_Is_AM == Price_Is_AM_Emun.是.ToString() || MF.Price_Is_AM == Price_Is_AM_Emun.否.ToString())
            {
                query = query.Where(m => m.Price_Is_AM == MF.Price_Is_AM).AsQueryable();
            }

            if (MF.Is_Stock == Is_Stock_Emun.重点备货.ToString())
            {
                query = query.Where(m => m.Is_Stock == 1).AsQueryable();
            }
            else if (MF.Is_Stock == Is_Stock_Emun.非重点备货.ToString())
            {
                query = query.Where(m => m.Is_Stock == 0).AsQueryable();
            }

            List<Material> RowList = query.OrderByDescending(x => x.CreateTime).ThenBy(s => s.MatID).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            this.Get_Material_List_CODE_Info(RowList, MF.CODE);

            PageList<Material> PList = new PageList<Material>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowList;
            return PList;
        }

        public List<Material> Get_Material_List_By_MatSn(Material_Filter MF)
        {
            var query = db.Material.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(m => m.MatSn.Contains(MF.MatSn)).AsQueryable();
            }
            return query.Take(MF.PageSize).OrderBy(x => x.MatSn).ToList();
        }

        private void Get_Material_List_CODE_Info(List<Material> RowList, string CODE)
        {
            List<Guid> MatIDList = RowList.Select(x => x.MatID).ToList();
            List<Material_CODE> CODE_List = db.Material_CODE.Where(x => MatIDList.Contains(x.Link_MatID)).ToList();
            foreach (var x in RowList)
            {
                x.Price_Retail = x.Price_Cost_Ref_Vat * x.Price_Retail_Rate;
                x.Price_Trade_A = x.Price_Cost_Ref_Vat * x.Price_Trade_A_Rate;
                x.Price_Trade_B = x.Price_Cost_Ref_Vat * x.Price_Trade_B_Rate;
                x.Price_Trade_NoTax = x.Price_Cost_Ref_Vat * x.Price_Trade_NoTax_Rate;
                x.Mat_CODE_List = CODE_List.Where(c => c.Link_MatID == x.MatID).OrderBy(c => c.Create_DT).ToList();

                if (!string.IsNullOrEmpty(CODE))
                {
                    x.Mat_CODE_List = x.Mat_CODE_List.Where(c => c.CODE.ToLower().Contains(CODE.ToLower())).ToList();
                }

                foreach (var xx in x.Mat_CODE_List)
                {
                    x.Mat_CODE_List_STR += xx.CODE + " ";
                }
                x.Mat_CODE_List_STR = x.Mat_CODE_List_STR.Trim();

                x.Mat_CODE_First = x.Mat_CODE_List.FirstOrDefault();
                x.Mat_CODE_First = x.Mat_CODE_First == null ? new Material_CODE() : x.Mat_CODE_First;
            }
        }

        public PageList<Material> Get_Material_PageList(Material_Filter MF)
        {
            var query = db.Material.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            var query_MatSales = db.MatSales_Lib.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Sales_UID == MF.Sales_UID).AsQueryable();
            if (MF.IsMySales == MySales_Enum.IsMySales.ToString())
            {
                List<Guid> SalesMatIDList = query_MatSales.Select(x => x.MatID).ToList();
                query = query.Where(x => SalesMatIDList.Contains(x.MatID)).AsQueryable();
            }
            else if (MF.IsMySales == MySales_Enum.IsNotMySales.ToString())
            {
                List<Guid> SalesMatIDList = query_MatSales.Select(x => x.MatID).ToList();
                query = query.Where(x => SalesMatIDList.Contains(x.MatID) == false).AsQueryable();
            }

            if (MF.CatID != Guid.Empty)
            {
                query = query.Where(x => x.CatID == MF.CatID).AsQueryable();
            }
            else if (MF.CatID_Str == Cat_Enum.NoneCat.ToString())
            {
                query = query.Where(x => x.CatID == Guid.Empty).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                query = query.Where(m => m.MatBrand == MF.MatBrand).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(m => m.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatName))
            {
                query = query.Where(m => m.MatName.Contains(MF.MatName)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.CODE))
            {
                List<Guid> CODE_MatID_List = db.Material_CODE.Where(x => x.CODE.Contains(MF.CODE)).Select(x => x.Link_MatID).Distinct().ToList();
                if (CODE_MatID_List.Any())
                {
                    query = query.Where(m => CODE_MatID_List.Contains(m.MatID)).AsQueryable();
                }
            }

            if (!string.IsNullOrEmpty(MF.PC))
            {
                query = query.Where(m => m.PC.Contains(MF.PC)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.PC_Place))
            {
                query = query.Where(m => m.PC_Place.Contains(MF.PC_Place)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.PC_Mon_Type))
            {
                query = query.Where(m => m.PC_Mon_Type.Contains(MF.PC_Mon_Type)).AsQueryable();
            }

            if (MF.Price_Is_AM == Price_Is_AM_Emun.是.ToString() || MF.Price_Is_AM == Price_Is_AM_Emun.否.ToString())
            {
                query = query.Where(m => m.Price_Is_AM == MF.Price_Is_AM).AsQueryable();
            }

            if (MF.Is_Stock == Is_Stock_Emun.重点备货.ToString())
            {
                query = query.Where(m => m.Is_Stock == 1).AsQueryable();
            }
            else if (MF.Is_Stock == Is_Stock_Emun.非重点备货.ToString())
            {
                query = query.Where(m => m.Is_Stock == 0).AsQueryable();
            }

            List<Material> RowList = query.OrderByDescending(x => x.CreateTime).ThenBy(s => s.MatID).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            this.Get_Material_List_Img_And_Cat(RowList);
            this.Get_Material_List_Wms_Info(RowList);

            PageList<Material> PList = new PageList<Material>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowList;
            return PList;
        }

        public PageList<Material> Get_Material_PageList_For_MatImg(Material_Filter MF)
        {
            var query = db.Material.Where(x => x.LinkMainCID == MF.LinkMainCID && x.Link_IMGID == MF.Link_IMGID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                query = query.Where(m => m.MatBrand == MF.MatBrand).AsQueryable();
            }

            List<Material> RowList = query.OrderByDescending(x => x.CreateTime).ThenBy(s => s.MatID).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            this.Get_Material_List_Img_And_Cat(RowList);
            PageList<Material> PList = new PageList<Material>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowList;
            return PList;
        }

        public PageList<Material> Get_Material_PageList_For_MySale(Material_Filter MF)
        {
            PageList<Material> PList = this.Get_Material_PageList(MF);

            List<Mat_Mon_Sale> MonSalesList = new List<Mat_Mon_Sale>();
            Mat_Mon_Sale Mon = new Mat_Mon_Sale();

            DateTime SD = DTList.LastMonth().SD;
            DateTime ED = DTList.ThisMonth().ED.AddMonths(3);
            string ThisMon = DTList.ThisMonth().SD.ToString("yyyy-MM");

            List<DTSD> DTSD_List = DTList.MonthsByTimeSlot(SD, ED);
            foreach (var x in DTSD_List)
            {
                Mon = new Mat_Mon_Sale();
                Mon.Mon = Mon.Mon_SD.ToString("YYYY-MM-dd");
                Mon.Mon_SD = x.SD;
                Mon.Mon_ED = x.ED;
                Mon.IsThisMon = ThisMon == Mon.Mon_SD.ToString("yyyy-MM") ? 1 : 0;
                Mon.Sales_Sum_Qty = 0;
                MonSalesList.Add(Mon);
            }
            MF.MonSalesList = MonSalesList;

            foreach (var x in PList.Rows)
            {
                x.MonSaleList = MonSalesList;
            }
            return PList;
        }

        public void Get_Material_List_Img_And_Cat(List<Material> RowList)
        {
            Guid LinkMainCID = Guid.Empty;
            if (RowList.Any())
            {
                LinkMainCID = RowList.FirstOrDefault().LinkMainCID;
            }

            ICategoryService ICat = new CategoryService();
            List<Category> CatList = ICat.Get_Category_List(LinkMainCID);
            Category Cat = new Category();

            List<Guid> GIDList = RowList.Select(x => x.Link_IMGID).ToList();
            GIDList = GIDList.Where(x => x != Guid.Empty).Distinct().ToList();
            List<MatImage> MatIMGList = db.MatImage.Where(x => GIDList.Contains(x.IMGID)).ToList();


            MatImage MatIMG = new MatImage();
            foreach (var x in RowList)
            {
                Cat = CatList.Where(c => c.CatID == x.CatID).FirstOrDefault();
                if (Cat != null)
                {
                    x.Cat_Name = Cat.CatName;
                    x.Cat_Name_Path = Cat.CatNamePath;
                }

                MatIMG = MatIMGList.Where(c => c.IMGID == x.Link_IMGID).FirstOrDefault();
                if (MatIMG != null)
                {
                    x.MatImgPath = MatIMG.MatImgPath;
                    x.MatThumbImgPath = MatIMG.MatThumbImgPath;
                    x.MatSourceImgPath = MatIMG.MatSourceImgPath;
                }
            }
        }

        public void Get_Material_List_Wms_Info(List<Material> RowList)
        {
            //List<Guid> MatIDList = RowList.Select(x => x.MatID).ToList();
            //List<WMS_Stock> WS_List = db.WMS_Stock.Where(x => MatIDList.Contains(x.MatID)).ToList();
            //WMS_Stock WS = new WMS_Stock();

            //foreach (var x in RowList)
            //{
            //    WS = WS_List.Where(c => c.MatID == x.MatID).FirstOrDefault();
            //    WS = WS == null ? new WMS_Stock() : WS;
            //    x.WMS_Stock_Qty = WS.Quantity;
            //}
        }
    }

    //产品创建及更新
    public partial class MaterialService : IMaterialService
    {
        public Guid Create_Material(Material Mat, User U)
        {
            Mat.MatID = MyGUID.NewGUID();
            Mat.MatSn = Mat.MatSn.Trim();
            Mat.LinkMainCID = U.LinkMainCID;

            Brand B = db.Brand.Find(Mat.Link_BID);
            this.Check_MatSn(B, Mat.MatSn);

            Mat.MatName = Mat.MatName == null ? string.Empty : Mat.MatName.Trim();
            Mat.MatSpecifications = Mat.MatSpecifications == null ? string.Empty : Mat.MatSpecifications.Trim();
            Mat.MatBrand = B.Brand_Name;
            Mat.MatUnit = Mat.MatUnit == null ? string.Empty : Mat.MatUnit.Trim();

            Mat.PC = Mat.PC == null ? string.Empty : Mat.PC.Trim();
            Mat.PC_Place = Mat.PC_Place == null ? string.Empty : Mat.PC_Place.Trim();
            Mat.PC_Mon_Type = Mat.PC_Mon_Type == null ? string.Empty : Mat.PC_Mon_Type.Trim();

            if (Mat.Is_Stock != 0 && Mat.Is_Stock != 1)
            {
                throw new Exception("重点备货，未选择");
            }

            if (string.IsNullOrEmpty(Mat.Price_Is_AM))
            {
                throw new Exception("定价品，未选择");
            }

            Mat.Link_BID = B.BID;
            Mat.CreateTime = DateTime.Now;
            Mat.LastUpdateTime = Mat.CreateTime;
            Mat.CreatePerson = U.UserFullName;
            Mat.LastUpdatePerson = string.Empty;

            this.Check_Mat_Info(Mat);

            //订货信息
            int Seconds = 0;
            List<Material_CODE> New_CODE_List = Mat.Mat_CODE_List.Where(x => x.CODE != string.Empty).ToList();
            if (New_CODE_List.Any())
            {
                foreach (var x in New_CODE_List)
                {
                    Seconds++;
                    x.Mat_CODE_ID = MyGUID.NewGUID();
                    x.Link_MatID = Mat.MatID;
                    x.Create_DT = Mat.CreateTime.AddSeconds(Seconds);
                }
                db.Material_CODE.AddRange(New_CODE_List);
            }

            db.Material.Add(Mat);
            MyDbSave.SaveChange(db);
            return Mat.MatID;
        }

        public void Set_Material_Base(Guid MatID, Material Mat, User U)
        {
            Material OLD_Mat = db.Material.Find(MatID);

            Mat.MatSn = Mat.MatSn.Trim();
            Brand B = db.Brand.Find(OLD_Mat.Link_BID);
            this.Check_MatSn(B, Mat.MatSn);

            OLD_Mat.MatSn = Mat.MatSn;
            OLD_Mat.MatName = Mat.MatName == null ? string.Empty : Mat.MatName.Trim();
            OLD_Mat.MatSpecifications = Mat.MatSpecifications == null ? string.Empty : Mat.MatSpecifications.Trim();
            OLD_Mat.MatBrand = B.Brand_Name;
            OLD_Mat.MatUnit = Mat.MatUnit == null ? string.Empty : Mat.MatUnit.Trim();
            OLD_Mat.MOQ = Mat.MOQ;
            OLD_Mat.Pack_Qty = Mat.Pack_Qty;
            OLD_Mat.Weight = Mat.Weight;

            OLD_Mat.PC = Mat.PC == null ? string.Empty : Mat.PC.Trim();
            OLD_Mat.PC_Place = Mat.PC_Place == null ? string.Empty : Mat.PC_Place.Trim();
            OLD_Mat.PC_Day = Mat.PC_Day == null ? string.Empty : Mat.PC_Day.Trim();
            OLD_Mat.PC_Mon = Mat.PC_Mon == null ? string.Empty : Mat.PC_Mon.Trim();
            OLD_Mat.PC_Mon_Type = Mat.PC_Mon_Type == null ? string.Empty : Mat.PC_Mon_Type.Trim();

            OLD_Mat.Is_Stock = Mat.Is_Stock;

            this.Check_Mat_Info(OLD_Mat);
            MyDbSave.SaveChange(db);
        }

        public void Set_Material_Price(Guid MatID, Material Mat, List<Material_CODE> CODE_List, User U)
        {
            Material OLD_Mat = db.Material.Find(MatID);

            //定价信息记录
            Material OLD_Mat_Price = JsonConvert.DeserializeObject<Material>(JsonConvert.SerializeObject(OLD_Mat));

            OLD_Mat.Price_Is_AM = Mat.Price_Is_AM == null ? Price_Is_AM_Emun.否.ToString() : Mat.Price_Is_AM.ToString();

            OLD_Mat.Price_AM = Mat.Price_AM;
            OLD_Mat.Price_AM_Rate = Mat.Price_AM_Rate;
            OLD_Mat.Price_Cost_Ref = Mat.Price_Cost_Ref;
            OLD_Mat.Price_Cost_Ref_Vat = Mat.Price_Cost_Ref_Vat;
            OLD_Mat.Price_Cost_Target = Mat.Price_Cost_Target;

            OLD_Mat.Price_Retail_Rate = Mat.Price_Retail_Rate;
            OLD_Mat.Price_Trade_A_Rate = Mat.Price_Trade_A_Rate;
            OLD_Mat.Price_Trade_B_Rate = Mat.Price_Trade_B_Rate;
            OLD_Mat.Price_Trade_NoTax_Rate = Mat.Price_Trade_NoTax_Rate;

            OLD_Mat.LastUpdateTime = DateTime.Now;
            OLD_Mat.LastUpdatePerson = U.UserFullName;

            //订货信息
            List<Material_CODE> OLD_List = db.Material_CODE.Where(x => x.Link_MatID == MatID).ToList();
            Material_CODE CODE = new Material_CODE();
            //更新现有CODE
            foreach (var x in OLD_List)
            {
                CODE = CODE_List.Where(c => c.Mat_CODE_ID == x.Mat_CODE_ID).FirstOrDefault();
                if (CODE != null)
                {
                    x.CODE = CODE.CODE == null ? string.Empty : CODE.CODE.Trim();
                    x.Order_Window = CODE.Order_Window == null ? string.Empty : CODE.Order_Window.Trim();
                    x.Order_Price = CODE.Order_Price;
                }
            }

            //清理空CODE
            List<Material_CODE> OLD_List_Delete = OLD_List.Where(x => x.CODE == string.Empty).ToList();
            if (OLD_List_Delete.Any())
            {
                db.Material_CODE.RemoveRange(OLD_List_Delete);
            }

            //新增CODE
            DateTime NowDT = DateTime.Now;
            List<Guid> CODEID_OLD = OLD_List.Select(x => x.Mat_CODE_ID).ToList();
            List<Material_CODE> OLD_List_New = CODE_List.Where(x => CODEID_OLD.Contains(x.Mat_CODE_ID) == false).ToList();
            int Seconds = 0;
            foreach (var New_x in OLD_List_New)
            {
                Seconds++;
                New_x.Create_DT = NowDT.AddSeconds(Seconds);
                New_x.CODE = New_x.CODE == null ? string.Empty : New_x.CODE.Trim();
                New_x.Order_Window = New_x.Order_Window == null ? string.Empty : New_x.Order_Window.Trim();
                New_x.Order_Price = New_x.Order_Price;
                New_x.Link_MatID = MatID;
            }

            OLD_List_New = OLD_List_New.Where(x => x.CODE != string.Empty).ToList();
            if (OLD_List_New.Any())
            {
                db.Material_CODE.AddRange(OLD_List_New);
            }

            MyDbSave.SaveChange(db);

            //异步添加新记录
            Thread.Sleep(500);

            List<Material> OLD_Mat_List = new List<Material>();
            OLD_Mat_List.Add(OLD_Mat_Price);

            List<Material> New_Mat_List = new List<Material>();
            New_Mat_List.Add(OLD_Mat);

            Task.Factory.StartNew(() => this.Add_Mat_Price_Set_Record(OLD_Mat_List, New_Mat_List, OLD_Mat.LastUpdatePerson));
        }

        private void Check_MatSn(Brand B, string MatSn)
        {
            if (B == null) { throw new Exception("品牌未填写"); }

            if (B.MatSn_Length_Min == 0 || B.MatSn_Length_Max == 0)
            {
                throw new Exception("MatSn_Length_Min or MatSn_Length_Max Undefined");
            }

            if (B.MatSn_Length_Min == B.MatSn_Length_Max && B.MatSn_Length_Max != MatSn.Length)
            {
                throw new Exception("产品型号长度必须为" + B.MatSn_Length_Max.ToString() + "位");
            }
            else if (B.MatSn_Length_Min > MatSn.Length)
            {
                throw new Exception("产品型号长度必须大于" + B.MatSn_Length_Min + "位");
            }
            else if (B.MatSn_Length_Max < MatSn.Length)
            {
                throw new Exception("产品型号长度必须小于" + B.MatSn_Length_Max + "位");
            }
        }

        private void Check_Mat_Info(Material Mat)
        {
            if (Mat.MatBrand == "NSK" && Mat.MatSn.Length != 27)
            {
                throw new Exception("产品型号不符合标准27位");
            }

            if (string.IsNullOrEmpty(Mat.MatSn))
            {
                throw new Exception("产品型号 - 未填写");
            }

            if (string.IsNullOrEmpty(Mat.MatUnit))
            {
                throw new Exception("单位 - 未填写");
            }

            if (db.Material.Where(x => x.MatSn == Mat.MatSn && x.LinkMainCID == Mat.LinkMainCID && x.Link_BID == Mat.Link_BID && x.MatID != Mat.MatID).Any())
            {
                throw new Exception("当前品牌项下产品型号重复");
            }
        }

        public void Delete_Material(Guid MatID)
        {
            //if (db.WMS_Stock.Find(MatID) != null)
            //{
            //    throw new Exception("此产品已建立库存，无法删除！");
            //}

            List<Material_CODE> CODE_List = db.Material_CODE.Where(x => x.Link_MatID == MatID).ToList();
            if (CODE_List.Any())
            {
                db.Material_CODE.RemoveRange(CODE_List);
            }

            Material Mat = db.Material.Find(MatID);
            db.Material.Remove(Mat);
            MyDbSave.SaveChange(db);
        }
    }

    //产品批量创建
    public partial class MaterialService : IMaterialService
    {
        public Guid Mat_Excel_UpLoad(HttpPostedFileBase ExcelFile, Guid UID, Guid BID)
        {
            User U = db.User.Find(UID);
            if (U == null)
            {
                throw new Exception("User Is Null");
            }

            Brand B = db.Brand.Find(BID);
            if (B == null)
            {
                throw new Exception("Brand Is Null");
            }

            if (B.MatSn_Length_Min == 0 || B.MatSn_Length_Max == 0)
            {
                throw new Exception("MatSn_Length_Min or MatSn_Length_Max Undefined");
            }

            //执行清理上次未导入的文件
            List<Mat_Excel> DEL_List = db.Mat_Excel.Where(x => x.Link_UID == U.UID && x.LinkMainCID == U.LinkMainCID && x.Is_Input == 0 && x.File_Type == Mat_Excel_File_Type.批量导入.ToString()).ToList();
            db.BulkDelete(DEL_List);

            List<Guid> IDList = DEL_List.Select(x => x.MEID).ToList();
            List<Mat_Excel_Line> DEL_LineList = db.Mat_Excel_Line.Where(x => IDList.Contains(x.Link_MEID)).ToList();
            db.BulkDelete(DEL_LineList);
            Thread.Sleep(500);

            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名

            Mat_Excel Head = new Mat_Excel();
            Head.MEID = MyGUID.NewGUID();
            Head.BID = B.BID;
            Head.Brand_Name = B.Brand_Name;
            Head.Create_DT = DateTime.Now;
            Head.Upload_Person = U.UserFullName;
            Head.File_Name = FileName == null ? string.Empty : FileName.Trim();
            Head.File_Type = Mat_Excel_File_Type.批量导入.ToString();
            Head.Is_Input = 0;
            Head.Link_UID = U.UID;
            Head.LinkMainCID = U.LinkMainCID;

            MyNormalUploadFile MF = new MyNormalUploadFile();
            //创建上传文件
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Excel_Temp/" + U.LinkMainCID);

            //根据路径通过已存在的excel来创建XSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);
            List<Mat_Excel_Line> Excel_Line_List = new List<Mat_Excel_Line>();
            Mat_Excel_Line Mat_Line = new Mat_Excel_Line();
            int Seconds = 0;
            for (int i = 2; i <= sheet.LastRowNum; i++)
            {
                Seconds++;
                IRow row = sheet.GetRow(i);
                Mat_Line = new Mat_Excel_Line();

                Mat_Line.LineID = MyGUID.NewGUID();
                Mat_Line.Link_MEID = Head.MEID;
                Mat_Line.Upload_Person = Head.Upload_Person;
                Mat_Line.LineNumber = Seconds;

                Mat_Line.MatID = Guid.Empty;
                try { Mat_Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Mat_Line.MatSn = string.Empty; }
                try { Mat_Line.MatName = row.GetCell(1).ToString().Trim(); } catch { Mat_Line.MatName = string.Empty; }
                try { Mat_Line.MatSpecifications = row.GetCell(2).ToString().Trim(); } catch { Mat_Line.MatSpecifications = string.Empty; }
                try { Mat_Line.MatBrand = row.GetCell(3).ToString().Trim(); } catch { Mat_Line.MatBrand = string.Empty; }

                if (Mat_Line.MatBrand == B.Brand_Name)
                {
                    Mat_Line.Link_BID = B.BID;
                }
                else
                {
                    Mat_Line.Link_BID = Guid.Empty;
                    Mat_Line.MatBrand = string.Empty;
                }

                try { Mat_Line.PC_Place = row.GetCell(4).ToString().Trim(); } catch { Mat_Line.PC_Place = string.Empty; }
                try { Mat_Line.MatUnit = row.GetCell(5).ToString().Trim(); } catch { Mat_Line.MatUnit = string.Empty; }
                Mat_Line.MatUnit = Mat_Line.MatUnit == string.Empty ? "PCS" : Mat_Line.MatUnit;

                try { Mat_Line.Weight = Convert.ToDecimal(row.GetCell(6).ToString().Trim()); } catch { Mat_Line.Weight = 0; }
                try { Mat_Line.MOQ = row.GetCell(7).ToString().Trim(); } catch { Mat_Line.MOQ = string.Empty; }
                try { Mat_Line.Pack_Qty = Convert.ToInt32(row.GetCell(8).ToString().Trim()); } catch { Mat_Line.Pack_Qty = 0; }
                try { Mat_Line.PC = row.GetCell(9).ToString().Trim(); } catch { Mat_Line.PC = string.Empty; }
                try { Mat_Line.PC_Day = row.GetCell(10).ToString().Trim(); } catch { Mat_Line.PC_Day = string.Empty; }
                try { Mat_Line.PC_Mon = row.GetCell(11).ToString().Trim(); } catch { Mat_Line.PC_Mon = string.Empty; }
                try { Mat_Line.PC_Mon_Type = row.GetCell(12).ToString().Trim(); } catch { Mat_Line.PC_Mon_Type = string.Empty; }
                try { Mat_Line.Is_Stock = row.GetCell(13).ToString().Trim() == "是" ? 1 : 0; } catch { Mat_Line.Is_Stock = 0; }

                //定价产品、折扣率
                try { Mat_Line.Price_AM = Convert.ToDecimal(row.GetCell(14).ToString().Trim()); } catch { Mat_Line.Price_AM = 0; }
                try { Mat_Line.Price_AM_Rate = Convert.ToDecimal(row.GetCell(15).ToString().Trim()); } catch { Mat_Line.Price_AM_Rate = 0; }
                try { Mat_Line.Price_Cost_Ref = Convert.ToDecimal(row.GetCell(16).ToString().Trim()); } catch { Mat_Line.Price_Cost_Ref = 0; }
                try { Mat_Line.Price_Cost_Target = Convert.ToDecimal(row.GetCell(17).ToString().Trim()); } catch { Mat_Line.Price_Cost_Target = 0; }


                try { Mat_Line.Price_Retail_Rate = Convert.ToDecimal(row.GetCell(18).ToString().Trim()); } catch { Mat_Line.Price_Retail_Rate = 0; }
                try { Mat_Line.Price_Trade_A_Rate = Convert.ToDecimal(row.GetCell(19).ToString().Trim()); } catch { Mat_Line.Price_Trade_A_Rate = 0; }
                try { Mat_Line.Price_Trade_B_Rate = Convert.ToDecimal(row.GetCell(20).ToString().Trim()); } catch { Mat_Line.Price_Trade_B_Rate = 0; }
                try { Mat_Line.Price_Trade_NoTax_Rate = Convert.ToDecimal(row.GetCell(21).ToString().Trim()); } catch { Mat_Line.Price_Trade_NoTax_Rate = 0; }

                try { Mat_Line.CODE = row.GetCell(22).ToString().Trim(); } catch { Mat_Line.CODE = string.Empty; }
                try { Mat_Line.CODE_Order_Window = row.GetCell(23).ToString().Trim(); } catch { Mat_Line.CODE_Order_Window = string.Empty; }
                try { Mat_Line.CODE_Order_Price = Convert.ToDecimal(row.GetCell(24).ToString().Trim()); } catch { Mat_Line.CODE_Order_Price = 0; }

                if (row.GetCell(24).CellType == CellType.Formula)
                {
                    throw new Exception(row.GetCell(24).RowIndex.ToString() + "行，" + row.GetCell(24).ColumnIndex + "列" + " - 内含公式，无法导入！");
                }

                Mat_Line.File_Type = Mat_Excel_File_Type.批量导入.ToString();
                if (Seconds <= 10000)
                {
                    Excel_Line_List.Add(Mat_Line);
                }

            }

            //保存导入的Excel数据
            if (Excel_Line_List.Any())
            {
                db.Mat_Excel.Add(Head);
                db.Mat_Excel_Line.AddRange(Excel_Line_List);
                MyDbSave.SaveChange(db);

                ////检查Excel产品数据正确性
                if (Excel_Line_List.Any())
                {
                    Thread.Sleep(500);
                    this.Mat_Excel_UpLoad_Check(Head.MEID);
                }
            }
            return Head.MEID;
        }

        private void Mat_Excel_UpLoad_Check(Guid MEID)
        {
            Mat_Excel Head = db.Mat_Excel.Find(MEID);
            Brand B = db.Brand.Find(Head.BID);

            List<Mat_Excel_Line> LineList = db.Mat_Excel_Line.Where(x => x.Link_MEID == Head.MEID).OrderBy(x => x.LineNumber).ToList();

            List<string> MatSn_List = LineList.Select(x => x.MatSn).Distinct().ToList();

            var Mat_Query_Select = db.Material.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Link_BID == B.BID).Select(x => new { MatID = x.MatID, MatSn = x.MatSn }).AsQueryable();
            List<Material> MatList = Mat_Query_Select.ToList().Select(x => new Material { MatID = x.MatID, MatSn = x.MatSn }).ToList();

            Material MatDB = new Material();
            foreach (var x in LineList)
            {
                MatDB = MatList.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                if (MatDB != null)
                {
                    x.MatID = MatDB.MatID;
                }
                else
                {
                    x.MatID = Guid.Empty;
                }

                //检查位数
                if (B.MatSn_Length_Min == B.MatSn_Length_Max && B.MatSn_Length_Max != x.MatSn.Length)
                {
                    x.Is_Error_MatSn = 1;
                }
                else if (B.MatSn_Length_Min > x.MatSn.Length)
                {
                    x.Is_Error_MatSn = 1;
                }
                else if (B.MatSn_Length_Max < x.MatSn.Length)
                {
                    x.Is_Error_MatSn = 1;
                }

                if (x.Link_BID == Guid.Empty)
                {
                    x.Is_Error_Brand = 1;
                }

                if (x.Is_Error_Brand == 1 || x.Is_Error_MatSn == 1)
                {
                    x.Is_Error = 1;
                }

                MyDbSave.SaveChange(db);
            }
        }

        public void Mat_Excel_UpLoad_To_DB(Guid MEID)
        {
            Mat_Excel Head = db.Mat_Excel.Find(MEID);
            List<Mat_Excel_Line> LineList = db.Mat_Excel_Line.Where(x => x.Link_MEID == Head.MEID).OrderBy(x => x.LineNumber).ToList();

            //新品导入，群组产品型号
            List<Material> New_Mat_List = new List<Material>();
            Material New_Mat = new Material();

            List<Mat_Excel_Line> New_Line_List = LineList.Where(x => x.Is_Input == 0 && x.Is_Error == 0 && x.MatID == Guid.Empty && x.Link_BID == Head.BID).ToList();
            List<Mat_Excel_Line> Sub_New_Line_List = new List<Mat_Excel_Line>();
            Mat_Excel_Line New_Line = new Mat_Excel_Line();
            List<string> MatSn_New_List = New_Line_List.Select(x => x.MatSn).Distinct().ToList();

            List<Material_CODE> Mat_CODE_List = new List<Material_CODE>();
            Material_CODE New_CODE = new Material_CODE();

            DateTime Now_DT = DateTime.Now;
            int i = 0;
            decimal TaxRate = (decimal)1.17;
            foreach (var MatSn in MatSn_New_List)
            {

                i++;
                New_Mat = new Material();
                New_Mat.MatID = MyGUID.NewGUID();
                New_Mat.MatSn = MatSn;

                New_Mat.Link_BID = Head.BID;
                New_Mat.MatBrand = Head.Brand_Name;

                Sub_New_Line_List = New_Line_List.Where(c => c.MatSn == New_Mat.MatSn).ToList();
                New_Line = Sub_New_Line_List.FirstOrDefault();

                New_Mat.MatName = New_Line.MatName;
                New_Mat.MatSpecifications = New_Line.MatSpecifications;
                New_Mat.MatName = New_Line.MatName;
                New_Mat.MatUnit = New_Line.MatUnit;
                New_Mat.Weight = New_Line.Weight;
                New_Mat.MOQ = New_Line.MOQ;
                New_Mat.Pack_Qty = New_Line.Pack_Qty;
                New_Mat.Is_Stock = New_Line.Is_Stock;
                New_Mat.PC = New_Line.PC;
                New_Mat.PC_Place = New_Line.PC_Place;
                New_Mat.PC_Day = New_Line.PC_Day;
                New_Mat.PC_Mon = New_Line.PC_Mon;
                New_Mat.PC_Mon_Type = New_Line.PC_Mon_Type;

                New_Mat.Price_AM = New_Line.Price_AM;
                New_Mat.Price_AM_Rate = New_Line.Price_AM_Rate;
                New_Mat.Price_Is_AM = New_Mat.Price_AM > 0 ? Price_Is_AM_Emun.是.ToString() : Price_Is_AM_Emun.否.ToString();

                if (New_Mat.Price_Is_AM == Price_Is_AM_Emun.是.ToString())
                {
                    New_Mat.Price_Cost_Ref = New_Mat.Price_AM * New_Mat.Price_AM_Rate / (decimal)100;
                }
                else
                {
                    New_Mat.Price_Cost_Ref = New_Line.Price_Cost_Ref;
                }

                New_Mat.Price_Cost_Ref_Vat = New_Mat.Price_Cost_Ref * TaxRate;
                New_Mat.Price_Cost_Target = New_Line.Price_Cost_Target;

                New_Mat.Price_Retail_Rate = New_Line.Price_Retail_Rate;
                New_Mat.Price_Trade_A_Rate = New_Line.Price_Trade_A_Rate;
                New_Mat.Price_Trade_B_Rate = New_Line.Price_Trade_B_Rate;
                New_Mat.Price_Trade_NoTax_Rate = New_Line.Price_Trade_NoTax_Rate;

                New_Mat.CreateTime = Now_DT.AddSeconds(i);
                New_Mat.CreatePerson = Head.Upload_Person;
                New_Mat.LastUpdateTime = New_Mat.CreateTime;
                New_Mat.LastUpdatePerson = string.Empty;
                New_Mat.LinkMainCID = Head.LinkMainCID;
                New_Mat_List.Add(New_Mat);

                foreach (var x in Sub_New_Line_List)
                {
                    x.MatID = New_Mat.MatID;
                    x.Is_Input = 1;
                    x.Create_DT = Now_DT;

                    New_CODE = new Material_CODE();
                    New_CODE.Mat_CODE_ID = MyGUID.NewGUID();
                    New_CODE.Link_MatID = New_Mat.MatID;
                    New_CODE.Create_DT = New_Mat.CreateTime.AddSeconds(1);
                    New_CODE.CODE = x.CODE;
                    New_CODE.Order_Window = x.CODE_Order_Window;
                    New_CODE.Order_Price = x.CODE_Order_Price;
                    if (!string.IsNullOrEmpty(New_CODE.CODE))
                    {
                        Mat_CODE_List.Add(New_CODE);
                    }
                }
            }

            if (New_Mat_List.Any())
            {
                Head.Is_Input = 1;
                Head.Create_DT = Now_DT;
                MyDbSave.SaveChange(db);

                db.BulkUpdate(New_Line_List);
                db.BulkInsert(New_Mat_List);
                if (Mat_CODE_List.Any())
                {
                    db.BulkInsert(Mat_CODE_List);
                }
                db.BulkSaveChanges();
            }
            else
            {
                throw new Exception("未导入任何产品！");
            }
        }

        public Mat_Excel Get_Mat_Excel(Guid MEID, Material_Filter MF)
        {
            Mat_Excel Head = db.Mat_Excel.Find(MEID);
            List<Mat_Excel_Line> LineList = db.Mat_Excel_Line.Where(x => x.Link_MEID == Head.MEID).OrderBy(x => x.LineNumber).ToList();

            if (Head.File_Type == Mat_Excel_File_Type.价格维护.ToString())
            {
                Head.Is_Can_Input_Count = LineList.Where(x => x.Is_Input == 0 && x.Is_Error == 0 && x.MatID != Guid.Empty).Count();
            }
            else
            {
                Head.Is_Can_Input_Count = LineList.Where(x => x.Is_Input == 0 && x.Is_Error == 0 && x.MatID == Guid.Empty).Count();
            }

            if (!string.IsNullOrEmpty(MF.PC_Place))
            {
                LineList = LineList.Where(x => x.PC_Place.ToLower().Contains(MF.PC_Place.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(MF.PC_Mon_Type))
            {
                LineList = LineList.Where(x => x.PC_Mon_Type.ToLower().Contains(MF.PC_Mon_Type.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(MF.PC))
            {
                LineList = LineList.Where(x => x.PC.ToLower().Contains(MF.PC.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(MF.CODE))
            {
                LineList = LineList.Where(x => x.CODE.ToLower().Contains(MF.CODE.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                LineList = LineList.Where(x => x.MatSn.ToLower().Contains(MF.MatSn.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(MF.MatName))
            {
                LineList = LineList.Where(x => x.MatName.ToLower().Contains(MF.MatName.ToLower())).ToList();
            }

            if (MF.Is_Stock == Is_Stock_Emun.重点备货.ToString())
            {
                LineList = LineList.Where(x => x.Is_Stock == 1).ToList();
            }
            else if (MF.Is_Stock == Is_Stock_Emun.非重点备货.ToString())
            {
                LineList = LineList.Where(x => x.Is_Stock == 0).ToList();
            }


            if (MF.Price_Is_AM == Price_Is_AM_Emun.是.ToString())
            {
                LineList = LineList.Where(x => x.Price_AM > 0).ToList();
            }
            else if (MF.Price_Is_AM == Price_Is_AM_Emun.否.ToString())
            {
                LineList = LineList.Where(x => x.Price_AM <= 0).ToList();
            }

            if (MF.Excel_Mat_Status == Mat_Excel_Mat_Status_Enum.完成导入产品.ToString())
            {
                LineList = LineList.Where(x => x.Is_Input == 1).ToList();
            }
            else if (MF.Excel_Mat_Status == Mat_Excel_Mat_Status_Enum.检查错误产品.ToString())
            {
                LineList = LineList.Where(x => x.Is_Error == 1).ToList();
            }
            else if (MF.Excel_Mat_Status == Mat_Excel_Mat_Status_Enum.允许导入产品.ToString())
            {
                if (Head.File_Type == Mat_Excel_File_Type.价格维护.ToString())
                {
                    LineList = LineList.Where(x => x.Is_Input == 0 && x.Is_Error == 0 && x.MatID != Guid.Empty).ToList();
                }
                else
                {
                    LineList = LineList.Where(x => x.Is_Input == 0 && x.Is_Error == 0 && x.MatID == Guid.Empty).ToList();
                }
            }
            else if (MF.Excel_Mat_Status == Mat_Excel_Mat_Status_Enum.系统已有产品.ToString())
            {
                LineList = LineList.Where(x => x.Is_Input == 0 && x.Is_Error == 0 && x.MatID != Guid.Empty).ToList();
            }

            Head.Mat_Excel_Line_PageList = new PageList<Mat_Excel_Line>();
            Head.Mat_Excel_Line_PageList.PageIndex = MF.PageIndex;
            Head.Mat_Excel_Line_PageList.PageSize = MF.PageSize;
            Head.Mat_Excel_Line_PageList.TotalRecord = LineList.Count();
            Head.Mat_Excel_Line_PageList.Rows = LineList.OrderBy(x => x.LineNumber).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            decimal TaxRate = (decimal)1.17;
            foreach (var x in Head.Mat_Excel_Line_PageList.Rows)
            {
                if (x.Price_AM > 0)
                {
                    x.Price_Cost_Ref = x.Price_AM * x.Price_AM_Rate / (decimal)100;
                }

                x.Price_Cost_Ref_Vat = x.Price_Cost_Ref * TaxRate;
            }
            return Head;
        }

        public PageList<Mat_Excel> Get_Mat_Excel_PageList(Material_Filter MF)
        {
            var query = db.Mat_Excel.Where(x => x.BID == MF.Link_BID && x.Is_Input == 1 && x.File_Type == MF.Excel_File_Type).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.File_Name.Contains(MF.Keyword)).AsQueryable();
            }

            PageList<Mat_Excel> PList = new PageList<Mat_Excel>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public List<Mat_Price_Change> Get_Materil_List_For_Price_Change(Guid MatID, int TakeCount)
        {
            int TakeCount_DB = TakeCount * 2;

            List<Mat_Excel_Line> Excel_Line = db.Mat_Excel_Line.Where(x => x.MatID == MatID && x.Is_Input == 1 && (x.File_Type == Mat_Excel_File_Type.价格变动_新.ToString() || x.File_Type == Mat_Excel_File_Type.价格变动_旧.ToString())).OrderByDescending(x => x.Create_DT).Take(TakeCount_DB).ToList();
            List<Material> Mat_List = new List<Material>();
            Material Mat = new Material();
            decimal TaxRate = (decimal)1.17;

            List<Mat_Price_Change> Change_List = new List<Mat_Price_Change>();
            Mat_Price_Change Change = new Mat_Price_Change();

            Mat_Excel_Line Excel_Line_OLD = new Mat_Excel_Line();
            Mat_Excel_Line Excel_Line_NEW = new Mat_Excel_Line();

            Material Mat_OLD = new Material();
            Material Mat_NEW = new Material();

            foreach (var Create_DT in Excel_Line.GroupBy(x => x.Create_DT).Select(x => x.Key))
            {

                Excel_Line_NEW = Excel_Line.Where(c => c.Create_DT == Create_DT && c.File_Type == Mat_Excel_File_Type.价格变动_新.ToString()).FirstOrDefault();
                Excel_Line_OLD = Excel_Line.Where(c => c.Create_DT == Create_DT && c.File_Type == Mat_Excel_File_Type.价格变动_旧.ToString()).FirstOrDefault();

                if (Excel_Line_NEW != null && Excel_Line_OLD != null)
                {
                    Change = new Mat_Price_Change();
                    Change.Person = Excel_Line_NEW.Upload_Person;
                    Change.Create_DT = Excel_Line_NEW.Create_DT;


                    Change.Mat_New = new Material();
                    Change.Mat_New.Price_AM = Excel_Line_NEW.Price_AM;
                    Change.Mat_New.Price_Is_AM = Change.Mat_New.Price_AM <= 0 ? Price_Is_AM_Emun.否.ToString() : Price_Is_AM_Emun.是.ToString();
                    Change.Mat_New.Price_Cost_Ref = Excel_Line_NEW.Price_Cost_Ref;

                    if (Change.Mat_New.Price_Is_AM == Price_Is_AM_Emun.是.ToString())
                    {
                        Change.Mat_New.Price_Cost_Ref_QY = Change.Mat_New.Price_Cost_Ref;
                        Change.Mat_New.Price_AM_Rate = Excel_Line_NEW.Price_AM_Rate;
                    }
                    else
                    {
                        Change.Mat_New.Price_Cost_Ref_QY = (decimal)0;
                        Change.Mat_New.Price_AM_Rate = (decimal)0;
                    }

                    Change.Mat_New.Price_Cost_Ref_Vat = Change.Mat_New.Price_Cost_Ref * TaxRate;
                    Change.Mat_New.Price_Cost_Target = Excel_Line_NEW.Price_Cost_Target;

                    Change.Mat_New.Price_Retail_Rate = Excel_Line_NEW.Price_Retail_Rate;
                    Change.Mat_New.Price_Trade_A_Rate = Excel_Line_NEW.Price_Trade_A_Rate;
                    Change.Mat_New.Price_Trade_B_Rate = Excel_Line_NEW.Price_Trade_B_Rate;
                    Change.Mat_New.Price_Trade_NoTax_Rate = Excel_Line_NEW.Price_Trade_NoTax_Rate;

                    Change.Mat_New.Price_Retail = Change.Mat_New.Price_Cost_Ref_Vat * Change.Mat_New.Price_Retail_Rate;
                    Change.Mat_New.Price_Trade_A = Change.Mat_New.Price_Cost_Ref_Vat * Change.Mat_New.Price_Trade_A_Rate;
                    Change.Mat_New.Price_Trade_B = Change.Mat_New.Price_Cost_Ref_Vat * Change.Mat_New.Price_Trade_B_Rate;
                    Change.Mat_New.Price_Trade_NoTax = Change.Mat_New.Price_Cost_Ref_Vat * Change.Mat_New.Price_Trade_NoTax_Rate;

                    Change.Mat_OLD = new Material();
                    Change.Mat_OLD.Price_AM = Excel_Line_OLD.Price_AM;
                    Change.Mat_OLD.Price_Is_AM = Change.Mat_OLD.Price_AM <= 0 ? Price_Is_AM_Emun.否.ToString() : Price_Is_AM_Emun.是.ToString();
                    Change.Mat_OLD.Price_Cost_Ref = Excel_Line_OLD.Price_Cost_Ref;

                    if (Change.Mat_OLD.Price_Is_AM == Price_Is_AM_Emun.是.ToString())
                    {
                        Change.Mat_OLD.Price_Cost_Ref_QY = Change.Mat_OLD.Price_Cost_Ref;
                        Change.Mat_OLD.Price_AM_Rate = Excel_Line_OLD.Price_AM_Rate;
                    }
                    else
                    {
                        Change.Mat_OLD.Price_Cost_Ref_QY = (decimal)0;
                        Change.Mat_OLD.Price_AM_Rate = (decimal)0;
                    }

                    Change.Mat_OLD.Price_AM_Rate = Excel_Line_OLD.Price_AM_Rate;
                    Change.Mat_OLD.Price_Cost_Ref_QY = Change.Mat_OLD.Price_Cost_Ref;
                    Change.Mat_OLD.Price_Cost_Ref_Vat = Change.Mat_OLD.Price_Cost_Ref * TaxRate;
                    Change.Mat_OLD.Price_Cost_Target = Excel_Line_OLD.Price_Cost_Target;

                    Change.Mat_OLD.Price_Retail_Rate = Excel_Line_OLD.Price_Retail_Rate;
                    Change.Mat_OLD.Price_Trade_A_Rate = Excel_Line_OLD.Price_Trade_A_Rate;
                    Change.Mat_OLD.Price_Trade_B_Rate = Excel_Line_OLD.Price_Trade_B_Rate;
                    Change.Mat_OLD.Price_Trade_NoTax_Rate = Excel_Line_OLD.Price_Trade_NoTax_Rate;

                    Change.Mat_OLD.Price_Retail = Change.Mat_OLD.Price_Cost_Ref_Vat * Change.Mat_OLD.Price_Retail_Rate;
                    Change.Mat_OLD.Price_Trade_A = Change.Mat_OLD.Price_Cost_Ref_Vat * Change.Mat_OLD.Price_Trade_A_Rate;
                    Change.Mat_OLD.Price_Trade_B = Change.Mat_OLD.Price_Cost_Ref_Vat * Change.Mat_OLD.Price_Trade_B_Rate;
                    Change.Mat_OLD.Price_Trade_NoTax = Change.Mat_OLD.Price_Cost_Ref_Vat * Change.Mat_OLD.Price_Trade_NoTax_Rate;

                    Change_List.Add(Change);
                }
            }
            return Change_List.OrderByDescending(x => x.Create_DT).ToList();
        }

        public string MatSn_Check_And_Replace(string MatSn)
        {
            //System.Web.HttpUtility.HtmlEncode(MatSn);
            MatSn = MatSn.Replace(" ", " "); //&#160;
            return MatSn;
        }
    }

    //产品价格批量维护
    public partial class MaterialService : IMaterialService
    {
        public string Get_Material_Price_List_ToExcel(Material_Filter MF)
        {
            var query = db.Material.Where(x => x.Link_BID == MF.Link_BID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(m => m.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatName))
            {
                query = query.Where(m => m.MatName.Contains(MF.MatName)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.CODE))
            {
                List<Guid> CODE_MatID_List = db.Material_CODE.Where(x => x.CODE.Contains(MF.CODE)).Select(x => x.Link_MatID).Distinct().ToList();
                if (CODE_MatID_List.Any())
                {
                    query = query.Where(m => CODE_MatID_List.Contains(m.MatID)).AsQueryable();
                }
            }

            if (!string.IsNullOrEmpty(MF.PC))
            {
                query = query.Where(m => m.PC.Contains(MF.PC)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.PC_Place))
            {
                query = query.Where(m => m.PC_Place.Contains(MF.PC_Place)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.PC_Mon_Type))
            {
                query = query.Where(m => m.PC_Mon_Type.Contains(MF.PC_Mon_Type)).AsQueryable();
            }

            if (MF.Price_Is_AM == Price_Is_AM_Emun.是.ToString() || MF.Price_Is_AM == Price_Is_AM_Emun.否.ToString())
            {
                query = query.Where(m => m.Price_Is_AM == MF.Price_Is_AM).AsQueryable();
            }

            if (MF.Is_Stock == Is_Stock_Emun.重点备货.ToString())
            {
                query = query.Where(m => m.Is_Stock == 1).AsQueryable();
            }
            else if (MF.Is_Stock == Is_Stock_Emun.非重点备货.ToString())
            {
                query = query.Where(m => m.Is_Stock == 0).AsQueryable();
            }

            List<Material> List = query.OrderBy(x => x.CreateTime).ToList();
            this.Get_Material_List_CODE_Info(List, MF.CODE);

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Temp_Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("产品型号");
            TableHeads.Add("产品名称");
            TableHeads.Add("产品规格");
            TableHeads.Add("品牌");
            TableHeads.Add("产地");
            TableHeads.Add("单位");
            TableHeads.Add("重量(KG)");
            TableHeads.Add("起订量");
            TableHeads.Add("装箱数");
            TableHeads.Add("PC");
            TableHeads.Add("生产周期(天)");
            TableHeads.Add("指令月");
            TableHeads.Add("生产月");
            TableHeads.Add("重点备货");
            TableHeads.Add("AM定价");
            TableHeads.Add("折扣率");
            TableHeads.Add("未税契约单价");
            TableHeads.Add("未税目标进价");
            TableHeads.Add("零售价%");
            TableHeads.Add("批发价1%");
            TableHeads.Add("批发价2%");
            TableHeads.Add("不含税价%");
            TableHeads.Add("CODE");
            TableHeads.Add("订货窗口");
            TableHeads.Add("未税订货价格");
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

            try
            {
                string CODE_List_Str = string.Empty;
                DataRow newRow;
                foreach (var x in List)
                {
                    newRow = DT.NewRow();
                    newRow["产品型号"] = x.MatSn;
                    newRow["产品名称"] = x.MatName;
                    newRow["产品规格"] = x.MatSpecifications;
                    newRow["品牌"] = x.MatBrand;
                    newRow["产地"] = x.PC_Place;
                    newRow["单位"] = x.MatUnit;
                    newRow["重量(KG)"] = x.Weight.ToString("0.00");
                    newRow["起订量"] = x.MOQ;
                    newRow["装箱数"] = x.Pack_Qty;
                    newRow["PC"] = x.PC;
                    newRow["生产周期(天)"] = x.PC_Day;
                    newRow["指令月"] = x.PC_Mon;
                    newRow["生产月"] = x.PC_Mon_Type;
                    newRow["重点备货"] = x.Is_Stock == 0 ? "是" : "否";
                    newRow["AM定价"] = x.Price_AM.ToString("0.0000");
                    newRow["折扣率"] = x.Price_AM_Rate.ToString("0.00");
                    newRow["未税契约单价"] = x.Price_Cost_Ref.ToString("0.0000");
                    newRow["未税目标进价"] = x.Price_Cost_Target.ToString("0.0000");
                    newRow["零售价%"] = x.Price_Retail_Rate.ToString("0.000");
                    newRow["批发价1%"] = x.Price_Trade_A_Rate.ToString("0.000");
                    newRow["批发价2%"] = x.Price_Trade_B_Rate.ToString("0.000");
                    newRow["不含税价%"] = x.Price_Trade_NoTax_Rate.ToString("0.000");
                    newRow["CODE"] = x.Mat_CODE_First.CODE;
                    newRow["订货窗口"] = x.Mat_CODE_First.Order_Window;
                    newRow["未税订货价格"] = x.Mat_CODE_First.Order_Price <= 0 ? "" : x.Mat_CODE_First.Order_Price.ToString("0.0000");
                    DT.Rows.Add(newRow);

                    foreach (var xx in x.Mat_CODE_List.Where(c => c.Mat_CODE_ID != x.Mat_CODE_First.Mat_CODE_ID).ToList())
                    {
                        newRow = DT.NewRow();
                        newRow["产品型号"] = x.MatSn;
                        newRow["产品名称"] = x.MatName;
                        newRow["产品规格"] = x.MatSpecifications;
                        newRow["品牌"] = x.MatBrand;
                        newRow["产地"] = x.PC_Place;
                        newRow["单位"] = x.MatUnit;
                        newRow["重量(KG)"] = x.Weight.ToString("0.00");
                        newRow["起订量"] = x.MOQ;
                        newRow["装箱数"] = x.Pack_Qty;
                        newRow["PC"] = x.PC;
                        newRow["生产周期(天)"] = x.PC_Day;
                        newRow["指令月"] = x.PC_Mon;
                        newRow["生产月"] = x.PC_Mon_Type;
                        newRow["重点备货"] = x.Is_Stock == 0 ? "是" : "否";
                        newRow["AM定价"] = x.Price_AM.ToString("0.0000");
                        newRow["折扣率"] = x.Price_AM_Rate.ToString("0.00");
                        newRow["未税契约单价"] = x.Price_Cost_Ref.ToString("0.0000");
                        newRow["未税目标进价"] = x.Price_Cost_Target.ToString("0.0000");
                        newRow["零售价%"] = x.Price_Retail_Rate.ToString("0.000");
                        newRow["批发价1%"] = x.Price_Trade_A_Rate.ToString("0.000");
                        newRow["批发价2%"] = x.Price_Trade_B_Rate.ToString("0.000");
                        newRow["不含税价%"] = x.Price_Trade_NoTax_Rate.ToString("0.000");
                        newRow["CODE"] = xx.CODE;
                        newRow["订货窗口"] = xx.Order_Window;
                        newRow["未税订货价格"] = xx.Order_Price <= 0 ? "" : xx.Order_Price.ToString("0.0000");
                        DT.Rows.Add(newRow);
                    }
                }
                Path = MyExcel.CreateNewExcel(DT);
            }
            catch
            {
                Path = MyExcel.CreateNewExcel(DT);
            }

            return Path;
        }

        public string Get_Mat_Excel_ToExcel(List<Mat_Excel_Line> LineList)
        {
            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Temp_Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("产品型号");
            TableHeads.Add("产品名称");
            TableHeads.Add("产品规格");
            TableHeads.Add("品牌");
            TableHeads.Add("产地");
            TableHeads.Add("单位");
            TableHeads.Add("重量(KG)");
            TableHeads.Add("起订量");
            TableHeads.Add("装箱数");
            TableHeads.Add("PC");
            TableHeads.Add("生产周期(天)");
            TableHeads.Add("指令月");
            TableHeads.Add("生产月");
            TableHeads.Add("重点备货");
            TableHeads.Add("AM定价");
            TableHeads.Add("折扣率");
            TableHeads.Add("未税契约单价");
            TableHeads.Add("未税目标进价");
            TableHeads.Add("零售价%");
            TableHeads.Add("批发价1%");
            TableHeads.Add("批发价2%");
            TableHeads.Add("不含税价%");
            TableHeads.Add("CODE");
            TableHeads.Add("订货窗口");
            TableHeads.Add("未税订货价格");
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

            try
            {
                DataRow newRow;
                foreach (var x in LineList)
                {
                    newRow = DT.NewRow();
                    newRow["产品型号"] = x.MatSn;
                    newRow["产品名称"] = x.MatName;
                    newRow["产品规格"] = x.MatSpecifications;
                    newRow["品牌"] = x.MatBrand;
                    newRow["产地"] = x.PC_Place;
                    newRow["单位"] = x.MatUnit;
                    newRow["重量(KG)"] = x.Weight.ToString("0.00");
                    newRow["起订量"] = x.MOQ;
                    newRow["装箱数"] = x.Pack_Qty;
                    newRow["PC"] = x.PC;
                    newRow["生产周期(天)"] = x.PC_Day;
                    newRow["指令月"] = x.PC_Mon;
                    newRow["生产月"] = x.PC_Mon_Type;
                    newRow["重点备货"] = x.Is_Stock == 0 ? "是" : "否";
                    newRow["AM定价"] = x.Price_AM.ToString("0.0000");
                    newRow["折扣率"] = x.Price_AM_Rate.ToString("0.00");
                    newRow["未税契约单价"] = x.Price_Cost_Ref.ToString("0.0000");
                    newRow["未税目标进价"] = x.Price_Cost_Target.ToString("0.0000");
                    newRow["零售价%"] = x.Price_Retail_Rate.ToString("0.000");
                    newRow["批发价1%"] = x.Price_Trade_A_Rate.ToString("0.000");
                    newRow["批发价2%"] = x.Price_Trade_B_Rate.ToString("0.000");
                    newRow["不含税价%"] = x.Price_Trade_NoTax_Rate.ToString("0.000");
                    newRow["CODE"] = x.CODE;
                    newRow["订货窗口"] = x.CODE_Order_Window;
                    newRow["未税订货价格"] = x.CODE_Order_Price <= 0 ? "" : x.CODE_Order_Price.ToString("0.0000");
                    DT.Rows.Add(newRow);
                }
                Path = MyExcel.CreateNewExcel(DT);
            }
            catch
            {
                Path = MyExcel.CreateNewExcel(DT);
            }
            return Path;
        }

        public Guid Mat_Price_Excel_UpLoad(HttpPostedFileBase ExcelFile, Guid UID, Guid BID)
        {
            User U = db.User.Find(UID);
            if (U == null)
            {
                throw new Exception("User Is Null");
            }

            Brand B = db.Brand.Find(BID);
            if (B == null)
            {
                throw new Exception("Brand Is Null");
            }


            //执行清理上次未导入的文件
            List<Mat_Excel> DEL_List = db.Mat_Excel.Where(x => x.Link_UID == U.UID && x.LinkMainCID == U.LinkMainCID && x.Is_Input == 0 && x.File_Type == Mat_Excel_File_Type.价格维护.ToString()).ToList();
            db.Mat_Excel.RemoveRange(DEL_List);

            List<Guid> IDList = DEL_List.Select(x => x.MEID).ToList();
            List<Mat_Excel_Line> DEL_LineList = db.Mat_Excel_Line.Where(x => IDList.Contains(x.Link_MEID)).ToList();
            db.Mat_Excel.RemoveRange(DEL_List);
            db.Mat_Excel_Line.RemoveRange(DEL_LineList);
            MyDbSave.SaveChange(db);
            Thread.Sleep(500);

            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名

            Mat_Excel Head = new Mat_Excel();
            Head.MEID = MyGUID.NewGUID();
            Head.BID = B.BID;
            Head.Brand_Name = B.Brand_Name;
            Head.Create_DT = DateTime.Now;
            Head.Upload_Person = U.UserFullName;
            Head.File_Name = FileName == null ? string.Empty : FileName.Trim();
            Head.File_Type = Mat_Excel_File_Type.价格维护.ToString();
            Head.Is_Input = 0;
            Head.Link_UID = U.UID;
            Head.LinkMainCID = U.LinkMainCID;

            MyNormalUploadFile MF = new MyNormalUploadFile();
            //创建上传文件
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Excel_Temp/" + U.LinkMainCID);

            //根据路径通过已存在的excel来创建XSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);
            List<Mat_Excel_Line> Excel_Line_List = new List<Mat_Excel_Line>();
            Mat_Excel_Line Mat_Line = new Mat_Excel_Line();
            int Seconds = 0;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                Seconds++;
                IRow row = sheet.GetRow(i);
                Mat_Line = new Mat_Excel_Line();

                Mat_Line.LineID = MyGUID.NewGUID();
                Mat_Line.Link_MEID = Head.MEID;
                Mat_Line.Upload_Person = Head.Upload_Person;
                Mat_Line.LineNumber = Seconds;

                Mat_Line.MatID = Guid.Empty;
                try { Mat_Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Mat_Line.MatSn = string.Empty; }
                try { Mat_Line.MatName = row.GetCell(1).ToString().Trim(); } catch { Mat_Line.MatName = string.Empty; }
                try { Mat_Line.MatSpecifications = row.GetCell(2).ToString().Trim(); } catch { Mat_Line.MatSpecifications = string.Empty; }
                try { Mat_Line.MatBrand = row.GetCell(3).ToString().Trim(); } catch { Mat_Line.MatBrand = string.Empty; }

                if (Mat_Line.MatBrand != B.Brand_Name)
                {
                    throw new Exception("错误 - " + Mat_Line.MatBrand + "匹配错误");
                }
                Mat_Line.Link_BID = B.BID;

                try { Mat_Line.PC_Place = row.GetCell(4).ToString().Trim(); } catch { Mat_Line.PC_Place = string.Empty; }
                try { Mat_Line.MatUnit = row.GetCell(5).ToString().Trim(); } catch { Mat_Line.MatUnit = string.Empty; }
                try { Mat_Line.Weight = Convert.ToDecimal(row.GetCell(6).ToString().Trim()); } catch { Mat_Line.Weight = 0; }
                try { Mat_Line.MOQ = row.GetCell(7).ToString().Trim(); } catch { Mat_Line.MOQ = string.Empty; }
                try { Mat_Line.Pack_Qty = Convert.ToInt32(row.GetCell(8).ToString().Trim()); } catch { Mat_Line.Pack_Qty = 0; }

                try { Mat_Line.PC = row.GetCell(9).ToString().Trim(); } catch { Mat_Line.PC = string.Empty; }
                try { Mat_Line.PC_Day = row.GetCell(10).ToString().Trim(); } catch { Mat_Line.PC_Day = string.Empty; }
                try { Mat_Line.PC_Mon = row.GetCell(11).ToString().Trim(); } catch { Mat_Line.PC_Mon = string.Empty; }
                try { Mat_Line.PC_Mon_Type = row.GetCell(12).ToString().Trim(); } catch { Mat_Line.PC_Mon_Type = string.Empty; }
                Mat_Line.Is_Stock = row.GetCell(13).ToString() == "是" ? 1 : 0;


                //定价产品、折扣率
                try { Mat_Line.Price_AM = Convert.ToDecimal(row.GetCell(14).ToString().Trim()); } catch { Mat_Line.Price_AM = 0; }
                try { Mat_Line.Price_AM_Rate = Convert.ToDecimal(row.GetCell(15).ToString().Trim()); } catch { Mat_Line.Price_AM_Rate = 0; }
                try { Mat_Line.Price_Cost_Ref = Convert.ToDecimal(row.GetCell(16).ToString().Trim()); } catch { Mat_Line.Price_Cost_Ref = 0; }
                try { Mat_Line.Price_Cost_Target = Convert.ToDecimal(row.GetCell(17).ToString().Trim()); } catch { Mat_Line.Price_Cost_Target = 0; }

                try { Mat_Line.Price_Retail_Rate = Convert.ToDecimal(row.GetCell(18).ToString().Trim()); } catch { Mat_Line.Price_Retail_Rate = 0; }
                try { Mat_Line.Price_Trade_A_Rate = Convert.ToDecimal(row.GetCell(19).ToString().Trim()); } catch { Mat_Line.Price_Trade_A_Rate = 0; }
                try { Mat_Line.Price_Trade_B_Rate = Convert.ToDecimal(row.GetCell(20).ToString().Trim()); } catch { Mat_Line.Price_Trade_B_Rate = 0; }
                try { Mat_Line.Price_Trade_NoTax_Rate = Convert.ToDecimal(row.GetCell(21).ToString().Trim()); } catch { Mat_Line.Price_Trade_NoTax_Rate = 0; }

                try { Mat_Line.CODE = row.GetCell(22).ToString().Trim(); } catch { Mat_Line.CODE = string.Empty; }
                try { Mat_Line.CODE_Order_Window = row.GetCell(23).ToString().Trim(); } catch { Mat_Line.CODE_Order_Window = string.Empty; }
                try { Mat_Line.CODE_Order_Price = Convert.ToDecimal(row.GetCell(24).ToString().Trim()); } catch { Mat_Line.CODE_Order_Price = 0; }

                Mat_Line.File_Type = Mat_Excel_File_Type.价格维护.ToString();

                if (row.GetCell(14).CellType == CellType.Formula)
                {
                    throw new Exception(row.GetCell(14).RowIndex.ToString() + "行，" + row.GetCell(14).ColumnIndex + "列" + " - 内含公式，无法导入！");
                }

                if (row.GetCell(15).CellType == CellType.Formula)
                {
                    throw new Exception(row.GetCell(15).RowIndex.ToString() + "行，" + row.GetCell(15).ColumnIndex + "列" + " - 内含公式，无法导入！");
                }

                if (row.GetCell(16).CellType == CellType.Formula)
                {
                    throw new Exception(row.GetCell(16).RowIndex.ToString() + "行，" + row.GetCell(16).ColumnIndex + "列" + " - 内含公式，无法导入！");
                }

                if (row.GetCell(21).CellType == CellType.Formula)
                {
                    throw new Exception(row.GetCell(21).RowIndex.ToString() + "行，" + row.GetCell(21).ColumnIndex + "列" + " - 内含公式，无法导入！");
                }

                if (row.GetCell(24).CellType == CellType.Formula)
                {
                    throw new Exception(row.GetCell(24).RowIndex.ToString() + "行，" + row.GetCell(24).ColumnIndex + "列" + " - 内含公式，无法导入！");
                }

                if (Seconds <= 100000)
                {
                    Excel_Line_List.Add(Mat_Line);
                }
            }

            //保存导入的Excel数据
            if (Excel_Line_List.Any())
            {
                db.Mat_Excel.Add(Head);
                MyDbSave.SaveChange(db);

                db.BulkInsert(Excel_Line_List);
                db.BulkSaveChanges();

                ////检查Excel产品数据正确性
                if (Excel_Line_List.Any())
                {
                    Thread.Sleep(500);
                    this.Mat_Price_Excel_UpLoad_Check(Head.MEID);
                }
            }
            return Head.MEID;
        }

        private void Mat_Price_Excel_UpLoad_Check(Guid MEID)
        {
            Mat_Excel Head = db.Mat_Excel.Find(MEID);
            Brand B = db.Brand.Find(Head.BID);

            List<Mat_Excel_Line> LineList = db.Mat_Excel_Line.Where(x => x.Link_MEID == Head.MEID).OrderBy(x => x.LineNumber).ToList();
            List<string> MatSn_List = LineList.Select(x => x.MatSn).Distinct().ToList();

            var Mat_Query_Select = db.Material.Where(x => x.LinkMainCID == Head.LinkMainCID && x.Link_BID == B.BID).Select(x => new { MatID = x.MatID, MatSn = x.MatSn }).AsQueryable();
            List<Material> MatList = Mat_Query_Select.ToList().Select(x => new Material { MatID = x.MatID, MatSn = x.MatSn }).ToList();

            Material MatDB = new Material();
            foreach (var x in LineList)
            {
                MatDB = MatList.Where(c => c.MatSn == x.MatSn).FirstOrDefault();
                if (MatDB != null)
                {
                    x.MatID = MatDB.MatID;
                }
                else
                {
                    x.MatID = Guid.Empty;
                }

                if (x.MatID == Guid.Empty)
                {
                    x.Is_Error = 1;
                }
            }

            db.BulkUpdate(LineList);
            db.BulkSaveChanges();
        }

        public void Mat_Price_Excel_UpLoad_To_DB(Guid MEID)
        {
            Mat_Excel Head = db.Mat_Excel.Find(MEID);
            if (Head.File_Type != Mat_Excel_File_Type.价格维护.ToString())
            {
                throw new Exception("File_Type Is Underfined");
            }

            if (Head.Is_Input == 1)
            {
                throw new Exception("此清单已导入系统，请勿重复操作");
            }

            Head.Is_Input = 1;
            List<Mat_Excel_Line> LineList = db.Mat_Excel_Line.Where(x => x.Link_MEID == Head.MEID && x.Is_Error == 0 && x.Is_Input == 0 && x.MatID != Guid.Empty).OrderBy(x => x.LineNumber).ToList();
            List<Guid> MatID_List = LineList.Select(x => x.MatID).ToList();
            List<Material> Mat_List = db.Material.Where(x => MatID_List.Contains(x.MatID)).ToList();

            DateTime Now_DT = DateTime.Now;
            decimal TaxRate = (decimal)1.17;
            foreach (var x in LineList)
            {
                x.Is_Input = 1;
                x.Create_DT = Now_DT;
            }

            Mat_Excel_Line Excel_Line = new Mat_Excel_Line();
            foreach (var Mat in Mat_List)
            {
                if (LineList.Where(x => x.MatID == Mat.MatID).Any())
                {
                    //优先处理 AM 定价品
                    if (LineList.Where(x => x.MatID == Mat.MatID && x.Price_AM > 0).Any())
                    {
                        Excel_Line = LineList.Where(x => x.MatID == Mat.MatID && x.Price_AM > 0).FirstOrDefault();
                        Mat.Price_AM = Excel_Line.Price_AM;
                        Mat.Price_AM_Rate = Excel_Line.Price_AM_Rate;
                        Mat.Price_Is_AM = Price_Is_AM_Emun.是.ToString();
                        //未税参考进价
                        Mat.Price_Cost_Ref = Mat.Price_AM * Mat.Price_AM_Rate / (decimal)100;
                    }
                    else
                    {
                        Excel_Line = LineList.Where(x => x.MatID == Mat.MatID).FirstOrDefault();
                        Mat.Price_AM = 0;
                        Mat.Price_AM_Rate = 0;
                        Mat.Price_Is_AM = Price_Is_AM_Emun.否.ToString();
                        //未税参考进价
                        Mat.Price_Cost_Ref = Excel_Line.Price_Cost_Ref;
                    }

                    //未税目标进价
                    Mat.Price_Cost_Target = Excel_Line.Price_Cost_Target;

                    //含税参考进价
                    Mat.Price_Cost_Ref_Vat = Mat.Price_Cost_Ref * TaxRate;

                    Mat.Price_Retail_Rate = Excel_Line.Price_Retail_Rate;
                    Mat.Price_Trade_A_Rate = Excel_Line.Price_Trade_A_Rate;
                    Mat.Price_Trade_B_Rate = Excel_Line.Price_Trade_B_Rate;
                    Mat.Price_Trade_NoTax_Rate = Excel_Line.Price_Trade_NoTax_Rate;
                    Mat.LastUpdateTime = Now_DT;
                    Mat.LastUpdatePerson = Head.Upload_Person;
                }
            }

            if (LineList.Any())
            {
                Head.Is_Input = 1;
                Head.Create_DT = Now_DT;
                db.Entry(Head).State = EntityState.Modified;
                MyDbSave.SaveChange(db);

                db.BulkUpdate(LineList);
                db.BulkUpdate(Mat_List);
                db.BulkSaveChanges();
            }
            else
            {
                throw new Exception("未更新任何产品！");
            }
        }

        private void Add_Mat_Price_Set_Record(List<Material> OLD_Mat_List, List<Material> New_Mat_List, string LastUpdatePerson)
        {
            Material Mat_OLD = new Material();
            Material Mat_New = new Material();
            Mat_Excel_Line Excel_Line = new Mat_Excel_Line();
            Mat_Excel_Line Excel_Line_New = new Mat_Excel_Line();
            List<Mat_Excel_Line> Excel_Line_List = new List<Mat_Excel_Line>();

            DateTime Now_DT = DateTime.Now;
            int LineNumber = 0;
            foreach (var x in OLD_Mat_List)
            {
                Mat_OLD = OLD_Mat_List.Where(c => c.MatID == x.MatID).FirstOrDefault();
                Mat_New = New_Mat_List.Where(c => c.MatID == x.MatID).FirstOrDefault();

                if (Mat_OLD.Price_Is_AM != Mat_New.Price_Is_AM
                 || Mat_OLD.Price_AM != Mat_New.Price_AM
                 || Mat_OLD.Price_AM_Rate != Mat_New.Price_AM_Rate
                 || Mat_OLD.Price_Cost_Ref != Mat_New.Price_Cost_Ref
                 || Mat_OLD.Price_Cost_Target != Mat_New.Price_Cost_Target
                 || Mat_OLD.Price_Retail_Rate != Mat_New.Price_Retail_Rate
                 || Mat_OLD.Price_Trade_A_Rate != Mat_New.Price_Trade_A_Rate
                 || Mat_OLD.Price_Trade_B_Rate != Mat_New.Price_Trade_B_Rate
                 || Mat_OLD.Price_Trade_NoTax_Rate != Mat_New.Price_Trade_NoTax_Rate
                 )
                {
                    LineNumber++;

                    //原始价格
                    Excel_Line = new Mat_Excel_Line();
                    Excel_Line.LineID = MyGUID.NewGUID();
                    Excel_Line.Link_MEID = Guid.Empty;
                    Excel_Line.Create_DT = Now_DT;
                    Excel_Line.File_Type = Mat_Excel_File_Type.价格变动_旧.ToString();
                    Excel_Line.Upload_Person = LastUpdatePerson;
                    Excel_Line.LineNumber = LineNumber;

                    Excel_Line.MatID = Mat_OLD.MatID;
                    Excel_Line.MatSn = Mat_OLD.MatSn;
                    Excel_Line.MatName = Mat_OLD.MatName;
                    Excel_Line.MatSpecifications = Mat_OLD.MatSpecifications;
                    Excel_Line.MatBrand = Mat_OLD.MatBrand;
                    Excel_Line.Link_BID = Mat_OLD.Link_BID;
                    Excel_Line.MatUnit = Mat_OLD.MatUnit;
                    Excel_Line.Weight = Mat_OLD.Weight;
                    Excel_Line.MOQ = Mat_OLD.MOQ;
                    Excel_Line.Pack_Qty = Mat_OLD.Pack_Qty;
                    Excel_Line.PC = Mat_OLD.PC;
                    Excel_Line.PC_Place = Mat_OLD.PC_Place;
                    Excel_Line.PC_Day = Mat_OLD.PC_Day;
                    Excel_Line.PC_Mon = Mat_OLD.PC_Mon;
                    Excel_Line.PC_Mon_Type = Mat_OLD.PC_Mon_Type;
                    Excel_Line.Is_Stock = Mat_OLD.Is_Stock;
                    Excel_Line.Price_AM = Mat_OLD.Price_AM;
                    Excel_Line.Price_AM_Rate = Mat_OLD.Price_AM_Rate;
                    Excel_Line.Price_Cost_Ref = Mat_OLD.Price_Cost_Ref;
                    Excel_Line.Price_Cost_Target = Mat_OLD.Price_Cost_Target;
                    Excel_Line.Price_Retail_Rate = Mat_OLD.Price_Retail_Rate;
                    Excel_Line.Price_Trade_A_Rate = Mat_OLD.Price_Trade_A_Rate;
                    Excel_Line.Price_Trade_B_Rate = Mat_OLD.Price_Trade_B_Rate;
                    Excel_Line.Price_Trade_NoTax_Rate = Mat_OLD.Price_Trade_NoTax_Rate;
                    Excel_Line.CODE = string.Empty;
                    Excel_Line.CODE_Order_Window = string.Empty;
                    Excel_Line.Is_Input = 1;
                    Excel_Line_List.Add(Excel_Line);

                    //新价格
                    Excel_Line_New = new Mat_Excel_Line();
                    Excel_Line_New.LineID = MyGUID.NewGUID();
                    Excel_Line_New.Link_MEID = Guid.Empty;
                    Excel_Line_New.Create_DT = Now_DT;
                    Excel_Line_New.File_Type = Mat_Excel_File_Type.价格变动_新.ToString();
                    Excel_Line_New.Upload_Person = LastUpdatePerson;
                    Excel_Line_New.LineNumber = LineNumber;

                    Excel_Line_New.MatID = Mat_New.MatID;
                    Excel_Line_New.MatSn = Mat_New.MatSn;
                    Excel_Line_New.MatName = Mat_New.MatName;
                    Excel_Line_New.MatSpecifications = Mat_New.MatSpecifications;
                    Excel_Line_New.MatBrand = Mat_New.MatBrand;
                    Excel_Line_New.Link_BID = Mat_New.Link_BID;
                    Excel_Line_New.MatUnit = Mat_New.MatUnit;
                    Excel_Line_New.Weight = Mat_New.Weight;
                    Excel_Line_New.MOQ = Mat_New.MOQ;
                    Excel_Line_New.Pack_Qty = Mat_New.Pack_Qty;
                    Excel_Line_New.PC = Mat_New.PC;
                    Excel_Line_New.PC_Place = Mat_New.PC_Place;
                    Excel_Line_New.PC_Day = Mat_New.PC_Day;
                    Excel_Line_New.PC_Mon = Mat_New.PC_Mon;
                    Excel_Line_New.PC_Mon_Type = Mat_New.PC_Mon_Type;
                    Excel_Line_New.Is_Stock = Mat_New.Is_Stock;
                    Excel_Line_New.Price_AM = Mat_New.Price_AM;
                    Excel_Line_New.Price_AM_Rate = Mat_New.Price_AM_Rate;
                    Excel_Line_New.Price_Cost_Ref = Mat_New.Price_Cost_Ref;
                    Excel_Line_New.Price_Cost_Target = Mat_New.Price_Cost_Target;
                    Excel_Line_New.Price_Retail_Rate = Mat_New.Price_Retail_Rate;
                    Excel_Line_New.Price_Trade_A_Rate = Mat_New.Price_Trade_A_Rate;
                    Excel_Line_New.Price_Trade_B_Rate = Mat_New.Price_Trade_B_Rate;
                    Excel_Line_New.Price_Trade_NoTax_Rate = Mat_New.Price_Trade_NoTax_Rate;
                    Excel_Line_New.CODE = string.Empty;
                    Excel_Line_New.CODE_Order_Window = string.Empty;
                    Excel_Line_New.Is_Input = 1;
                    Excel_Line_List.Add(Excel_Line_New);
                }
            }

            if (Excel_Line_List.Any())
            {
                db.BulkInsert(Excel_Line_List);
                db.BulkSaveChanges();
            }

        }

    }

    //产品目录批量设置
    public partial class MaterialService : IMaterialService
    {
        public void Set_Mat_CatID_Batch(List<Guid> MatIDList, Guid CatID)
        {
            List<Material> MatList = db.Material.Where(x => MatIDList.Contains(x.MatID)).ToList();
            foreach (var x in MatList)
            {
                x.CatID = CatID;
            }
            db.BulkUpdate(MatList);
            db.BulkSaveChanges();
        }
    }

    //产品名称设置
    public partial class MaterialService : IMaterialService
    {
        public Material_Name Get_Material_Name_Item_DB(Guid Name_ID)
        {
            Material_Name Mat = db.Material_Name.Find(Name_ID);
            Mat = Mat == null ? new Material_Name() : Mat;
            return Mat;
        }

        public PageList<Material_Name> Get_Material_Name_PageList(Material_Filter MF)
        {
            var query = db.Material_Name.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatName))
            {
                query = query.Where(x => x.Mat_Name.Contains(MF.MatName)).AsQueryable();
            }

            PageList<Material_Name> PList = new PageList<Material_Name>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = query.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            return PList;
        }

        public List<string> Get_Material_Name_Str_List(Guid LinkMainCID)
        {
            List<string> List = db.Material_Name.Where(x => x.LinkMainCID == LinkMainCID).OrderBy(x=>x.Create_DT).Select(x => x.Mat_Name).ToList();
            return List;
        }

        public void Create_Mat_Name(Material_Name Mat,Guid UID)
        {

            User U = db.User.Find(UID);
            if (U == null) { throw new Exception("User Is Null"); }

            Mat.Name_ID = MyGUID.NewGUID();
            Mat.LinkMainCID = U.LinkMainCID;
            Mat.Mat_Name = Mat.Mat_Name == null ? string.Empty : Mat.Mat_Name.Trim();

            Mat.Create_DT = DateTime.Now;
            Mat.Create_Person = U.UserFullName;

            this.Check_Material_Name_Item(Mat);
            db.Material_Name.Add(Mat);
            MyDbSave.SaveChange(db);
        }

        private void Check_Material_Name_Item(Material_Name Mat)
        {
            if (Mat.LinkMainCID == Guid.Empty)
            {
                throw new Exception("MainCID Is Empty");
            }

            if (string.IsNullOrEmpty(Mat.Mat_Name))
            {
                throw new Exception("产品名称不能为空");
            }

            if (db.Material_Name.Where(x => x.LinkMainCID == Mat.LinkMainCID && x.Mat_Name == Mat.Mat_Name && x.Name_ID != Mat.Name_ID).Any())
            {
                throw new Exception("产品名称重复");
            }
        }

        public void Set_Material_Name_Item(Guid Name_ID, Material_Name Mat)
        {
            Material_Name OLD_Mat = db.Material_Name.Find(Name_ID);

            if (db.Material.Where(x => x.LinkMainCID == OLD_Mat.LinkMainCID && x.MatName == OLD_Mat.Mat_Name).Any())
            {
                throw new Exception("此产品名称已关联产品，不支持更新");
            }

            OLD_Mat.Mat_Name = Mat.Mat_Name == null ? string.Empty : Mat.Mat_Name.Trim();

            this.Check_Material_Name_Item(OLD_Mat);
            db.Entry(OLD_Mat).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_Material_Name(Guid Name_ID)
        {
            Material_Name Mat = db.Material_Name.Find(Name_ID);
            if (db.Material.Where(x => x.LinkMainCID == Mat.LinkMainCID && x.MatName == Mat.Mat_Name).Any())
            {
                throw new Exception("此产品名称已关联产品，无法删除");
            }
            db.Material_Name.Remove(Mat);
            MyDbSave.SaveChange(db);
        }
    }

    //产品创建（临时）
    public partial class MaterialService : IMaterialService
    {
        public Guid Create_Material_Temp(Material Mat, User U)
        {
            Mat.MatID = MyGUID.NewGUID();
            Mat.MatSn = Mat.MatSn.Trim();
            Mat.LinkMainCID = U.LinkMainCID;

            Brand B = db.Brand.Find(Mat.Link_BID);
            if (B == null) { throw new Exception("品牌未选择"); }

            Mat.MatName = Mat.MatName == null ? string.Empty : Mat.MatName.Trim();
            Mat.MatBrand = B.Brand_Name;
            Mat.MatUnit = "PCS";
            Mat.Pack_Qty = Mat.Pack_Qty == 0 ? 1 : Mat.Pack_Qty;
            Mat.Link_BID = B.BID;
            Mat.CreateTime = DateTime.Now;
            Mat.CreatePerson = U.UserFullName;
            if (B.Brand_Name == "NMB")
            {
                Mat.Other_MatSn = Mat.Other_MatSn.Trim();
            }
            else
            {
                Mat.Other_MatSn = Mat.MatSn.Trim();
            }

            this.Check_Mat_Info(Mat);

            db.Material.Add(Mat);
            MyDbSave.SaveChange(db);
            return Mat.MatID;
        }

        public void Set_Material_Base_Temp(Guid MatID, Material Mat, User U)
        {
            Material OLD_Mat = db.Material.Find(MatID);

            Mat.MatSn = Mat.MatSn.Trim();
            Brand B = db.Brand.Find(OLD_Mat.Link_BID);
            if (B == null) { throw new Exception("品牌未选择"); }

            OLD_Mat.MatSn = Mat.MatSn;
            OLD_Mat.MatName = Mat.MatName == null ? string.Empty : Mat.MatName.Trim();
            OLD_Mat.MatBrand = B.Brand_Name;
            OLD_Mat.Pack_Qty = Mat.Pack_Qty;
            OLD_Mat.LastUpdateTime = DateTime.Now;
            OLD_Mat.LastUpdatePerson = U.UserFullName;

            if (B.Brand_Name == "NMB")
            {
                OLD_Mat.Other_MatSn = Mat.Other_MatSn.Trim();
            }
            else
            {
                OLD_Mat.Other_MatSn = Mat.MatSn.Trim();
            }

            this.Check_Mat_Info(OLD_Mat);
            MyDbSave.SaveChange(db);
        }

        public void Delete_Material_Temp(Guid MatID)
        {
            Material Mat = db.Material.Find(MatID);
            if (db.WMS_Stock.Where(x => x.MatBrand == Mat.MatBrand && x.MatSn == Mat.MatSn).Any())
            {
                throw new Exception("此产品已建立库存，无法删除！");
            }
          
            db.Material.Remove(Mat);
            MyDbSave.SaveChange(db);
        }

        public PageList<Material> Get_Material_PageList_Temp(Material_Filter MF)
        {
            var query = db.Material.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                query = query.Where(m => m.MatBrand == MF.MatBrand).AsQueryable();
            }

            if (MF.Link_BID != Guid.Empty)
            {
                query = query.Where(m => m.Link_BID == MF.Link_BID).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatSn))
            {
                query = query.Where(m => m.MatSn.Contains(MF.MatSn)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(m => m.MatSn.Contains(MF.Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatName))
            {
                query = query.Where(m => m.MatName.Contains(MF.MatName)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Other_MatSn))
            {
                query = query.Where(m => m.Other_MatSn.Contains(MF.Other_MatSn)).AsQueryable();
            }

            List<Material> RowList = query.OrderByDescending(x => x.CreateTime).ThenBy(s => s.MatID).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            PageList<Material> PList = new PageList<Material>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowList;
            return PList;
        }

        public void Mat_Excel_UpLoad_Temp_Various_Brands(HttpPostedFileBase ExcelFile, Guid UID)
        {
            User U = db.User.Find(UID);
            if (U == null)
            {
                throw new Exception("User Is Null");
            }

            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Material/" + U.UID + "多品牌");

            //根据路径通过已存在的excel来创建XSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);

            List<Material> Line_List = new List<Material>();
            Material Line = new Material();

            DateTime Now_DT = DateTime.Now;

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            int Line_No = 0;
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                Line_No++;
                Line = new Material();
                Line.MatID = MyGUID.NewGUID();
                Line.MatUnit = "PCS";
                Line.CreateTime = Now_DT;
                Line.LinkMainCID = U.LinkMainCID;
                Line.MatName = "轴承";
                try { Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }

                try { Line.PC = row.GetCell(1).ToString().Trim(); } catch { Line.PC = string.Empty; }
                try { Line.Pack_Qty = Convert.ToInt32(row.GetCell(2).ToString().Trim()); } catch { Line.Pack_Qty = 0; }
                try { Line.PC_Day = row.GetCell(3).ToString().Trim(); } catch { Line.PC_Day = string.Empty; }
                try { Line.PC_Mon_Type = row.GetCell(4).ToString().Trim(); } catch { Line.PC_Mon_Type = string.Empty; }
                try { Line.Other_MatSn = row.GetCell(5).ToString().Trim(); } catch { Line.Other_MatSn = string.Empty; }
                try { Line.MatBrand = row.GetCell(6).ToString().Trim(); } catch { Line.MatBrand = string.Empty; }

                //过滤换行符
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);
                Line.PC = Line.PC.Replace(Environment.NewLine, "");
                Line.PC_Day = Line.PC_Day.Replace(Environment.NewLine, "");
                Line.PC_Mon_Type = Line.PC_Mon_Type.Replace(Environment.NewLine, "");
                Line.Other_MatSn = Line.Other_MatSn.Replace(Environment.NewLine, "");
                Line.MatBrand = Line.MatBrand.Replace(Environment.NewLine, "");

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

                //判断单元格是否有公式
                for (int j = 0; j < 7; j++)
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
                if (!string.IsNullOrEmpty(Line.MatSn) && Line_List.Where(c => c.MatSn == Line.MatSn).Any() == false && Line_No <= 10000)
                {
                    Line_List.Add(Line);
                }

            }

            //与系统中的品牌进行比对
            List<string> Brand_List_Str = Line_List.Select(x => x.MatBrand).Distinct().ToList();
            List<Brand> MatBrand_List = db.Brand.Where(x => x.LinkMainCID == U.LinkMainCID && Brand_List_Str.Contains(x.Brand_Name)).ToList();
            if (MatBrand_List.Count() != Brand_List_Str.Count())
            {
                throw new Exception("Excel中存在与系统中不同的品牌！");
            }

            //判断本次导入在数据库中是否有重复型号
            List<string> MatSn_list = Line_List.Select(x => x.MatSn).Distinct().ToList();
            List<Material> Mat_List_DB = db.Material.Where(x => x.LinkMainCID == U.LinkMainCID && MatSn_list.Contains(x.MatSn)).ToList();
            Mat_List_DB = Mat_List_DB.Where(x => Brand_List_Str.Contains(x.MatBrand)).ToList();

            List<Material> Line_List_Temp = new List<Material>();
            foreach (var x in Line_List)
            {
                if (Mat_List_DB.Where(c => c.MatSn == x.MatSn).Any() == false)
                {
                    Line_List_Temp.Add(x);
                }
            }

            //品牌关联
            foreach (var x in MatBrand_List)
            {
                foreach (var xx in Line_List_Temp.Where(xx => xx.MatBrand == x.Brand_Name).ToList())
                {
                    xx.MatBrand = x.Brand_Name;
                    xx.Link_BID = x.BID;

                    if (xx.MatBrand == "NSK" && xx.MatSn.Length != 27)
                    {
                        throw new Exception("品牌NSK中存在产品型号不符合标准27位");
                    }

                    if (xx.MatBrand == "NMB")
                    {
                        xx.Other_MatSn = xx.Other_MatSn.Trim();
                    }
                    else
                    {
                        xx.Other_MatSn = xx.MatSn.Trim();
                    }
                }
            }

            if (Line_List_Temp.Any())
            {
                db.Material.AddRange(Line_List_Temp);
                MyDbSave.SaveChange(db);
            }
        }

        public void Mat_Excel_UpLoad_Temp(HttpPostedFileBase ExcelFile, Guid UID, Guid BID, string MatName)
        {
            MatName = MatName.Trim();
            if (string.IsNullOrEmpty(MatName))
            {
                throw new Exception("未选择产品名称");
            }

            User U = db.User.Find(UID);
            if (U == null)
            {
                throw new Exception("User Is Null");
            }

            Brand B = db.Brand.Find(BID);
            if (B == null)
            {
                throw new Exception("Brand Is Null");
            }

            //创建上传文件
            string FileName = Path.GetFileName(ExcelFile.FileName);   //获取文件名
            MyNormalUploadFile MF = new MyNormalUploadFile();
            string ExcelFilePath = MF.NormalUpLoadFileProcess(ExcelFile, "Material/" + U.UID);

            //根据路径通过已存在的excel来创建XSSFWorkbook，即整个excel文档
            XSSFWorkbook workbook = new XSSFWorkbook(new FileStream(HttpRuntime.AppDomainAppPath.ToString() + ExcelFilePath, FileMode.Open, FileAccess.Read));

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);

            List<Material> Line_List = new List<Material>();
            Material Line = new Material();

            DateTime Now_DT = DateTime.Now;

            if (workbook.NumberOfSheets > 1)
            {
                throw new Exception("Excel中Sheet的数量大于1, 无法判断数据表来源！");
            }

            int Line_No = 0;
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                Line_No++;
                Line = new Material();
                Line.MatID = MyGUID.NewGUID();
                Line.MatUnit = "PCS";
                Line.MatBrand = B.Brand_Name;
                Line.Link_BID = B.BID;
                Line.CreateTime = Now_DT;
                Line.LinkMainCID = U.LinkMainCID;
                Line.MatName = MatName;
                try { Line.MatSn = row.GetCell(0).ToString().Trim(); } catch { Line.MatSn = string.Empty; }
                if (string.IsNullOrEmpty(Line.MatSn)) { break; }

                try { Line.PC = row.GetCell(1).ToString().Trim(); } catch { Line.PC = string.Empty; }
                try { Line.Pack_Qty = Convert.ToInt32(row.GetCell(2).ToString().Trim()); } catch { Line.Pack_Qty = 0; }
                try { Line.PC_Day = row.GetCell(3).ToString().Trim(); } catch { Line.PC_Day = string.Empty; }
                try { Line.PC_Mon_Type = row.GetCell(4).ToString().Trim(); } catch { Line.PC_Mon_Type = string.Empty; }
                try { Line.Other_MatSn = row.GetCell(5).ToString().Trim(); } catch { Line.Other_MatSn = string.Empty; }

                //过滤换行符
                Line.MatSn = Line.MatSn.Replace(Environment.NewLine, "");
                Line.MatSn = this.MatSn_Check_And_Replace(Line.MatSn);
                Line.PC = Line.PC.Replace(Environment.NewLine, "");
                Line.PC_Day = Line.PC_Day.Replace(Environment.NewLine, "");
                Line.PC_Mon_Type = Line.PC_Mon_Type.Replace(Environment.NewLine, "");
                Line.Other_MatSn = Line.Other_MatSn.Replace(Environment.NewLine, "");

                //判断是否存在汉字
                if (CommonLib.Is_Not_Contains_Chinese(Line.MatSn) == false)
                {
                    throw new Exception("产品型号中不允许存在汉字！");
                }

                //判断单元格是否有公式
                for (int j = 0; j < 6; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (row.GetCell(j).CellType == CellType.Formula)
                        {
                            throw new Exception(row.GetCell(j).RowIndex.ToString() + "行，" + row.GetCell(j).ColumnIndex + "列" + "-内含公式，无法导入！");
                        }
                    }
                }

                //if (Line.MatBrand == "NSK" && Line.MatSn.Length != 27)
                //{
                //    throw new Exception("产品型号不符合标准27位");
                //}

                if (B.Brand_Name == "NMB")
                {
                    Line.Other_MatSn = Line.Other_MatSn.Trim();
                }
                else
                {
                    Line.Other_MatSn = Line.MatSn.Trim();
                }

                //判断本次导入是否有重复型号
                if (!string.IsNullOrEmpty(Line.MatSn) && Line_List.Where(c => c.MatSn == Line.MatSn).Any() == false && Line_No <= 10000)
                {
                    Line_List.Add(Line);
                }
                
            }

            //判断本次导入在数据库中是否有重复型号
            List<string> MatSn_list = Line_List.Select(x => x.MatSn).Distinct().ToList();
            List<Material> Mat_List_DB = db.Material.Where(x => x.LinkMainCID == U.LinkMainCID && x.MatBrand == B.Brand_Name && MatSn_list.Contains(x.MatSn)).ToList();

            List<Material> Line_List_Temp = new List<Material>();

            foreach (var x in Line_List)
            {
                if (Mat_List_DB.Where(c => c.MatSn == x.MatSn).Any() == false)
                {
                    Line_List_Temp.Add(x);
                }
            }

            if (Line_List_Temp.Any())
            {
                db.Material.AddRange(Line_List_Temp);
                MyDbSave.SaveChange(db);
            }
        }

        public void Set_Mat_Other_MatSn_ALL(Guid LinkMainCID)
        {
            List<Material> List = db.Material.Where(x => x.LinkMainCID == LinkMainCID).ToList();

            foreach (var x in List)
            {
                if (x.MatBrand != "NMB")
                {
                    x.Other_MatSn = x.MatSn;
                }
            }

            MyDbSave.SaveChange(db);
        }

        public string Get_Mat_List_ToExcel(Guid MainCID, string Brand)
        {
            List<Material> List = db.Material.Where(x => x.LinkMainCID == MainCID && x.MatBrand == Brand).ToList();

            string Path = string.Empty;
            //设定表头
            DataTable DT = new DataTable("Temp_Excel");
            //设定dataTable表头
            DataColumn myDataColumn = new DataColumn();
            List<string> TableHeads = new List<string>();
            TableHeads.Add("产品型号");
            TableHeads.Add("品牌");
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

            try
            {
                DataRow newRow;
                foreach (var x in List)
                {
                    newRow = DT.NewRow();
                    newRow["产品型号"] = x.MatSn;
                    newRow["品牌"] = x.MatBrand;
                    DT.Rows.Add(newRow);
                }
                Path = MyExcel.CreateNewExcel(DT);
            }
            catch
            {
                Path = MyExcel.CreateNewExcel(DT);
            }
            return Path;
        }
    }
}
