using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.Api.Models
{
    //基本信息
    public partial class Material
    {
        [Key]
        public Guid MatID { get; set; }

        //产品型号（唯一项）
        [Required]
        public string MatSn { get; set; }

        [NotMapped]
        public int MatSn_Length_Min { get; set; }

        [NotMapped]
        public int MatSn_Length_Max { get; set; }

        //物料名称
        [DefaultValue("")]
        public string MatName { get; set; }

        //代号
        public string Other_MatSn { get; set; }

        //规格
        [DefaultValue("")]
        public string MatSpecifications { get; set; }

        //所属品牌
        [DefaultValue("")]
        public string MatBrand { get; set; }

        [Required]
        public Guid Link_BID { get; set; }

        //计量单位
        [Required]
        public string MatUnit { get; set; }

        //产线
        [DefaultValue("")]
        public string PC { get; set; }

        //生产周期
        [DefaultValue("")]
        public string PC_Day { get; set; }

        //指令月
        [DefaultValue("")]
        public string PC_Mon { get; set; }

        //生产月
        [DefaultValue("")]
        public string PC_Mon_Type { get; set; }

        //产地
        [DefaultValue("")]
        public string PC_Place { get; set; }

        //起订量
        [DefaultValue("")]
        public string MOQ { get; set; }

        //装箱数
        [Required]
        [DefaultValue(0)]
        public int Pack_Qty { get; set; }

        //重量(KG)
        [DefaultValue(0)]
        public decimal Weight { get; set; }

        //是否重点备货
        [DefaultValue(0)]
        public int Is_Stock { get; set; }

        //商品简述
        [DefaultValue("")]
        public string MatSummary { get; set; }

        //二维码路径
        public string QRCode_Path { get; set; }

        //创建时间
        [Required]
        public DateTime CreateTime { get; set; }

        [DefaultValue("")]
        public string CreatePerson { get; set; }

        //最后更新时间
        [Required]
        public DateTime LastUpdateTime { get; set; }

        //最后更新人信息
        [DefaultValue("")]
        public string LastUpdatePerson { get; set; }

        //产品目录
        public Guid CatID { get; set; }
        
        public Guid Link_IMGID { get; set; }
        
        public Guid Link_IMDID { get; set; }

        //关联的经营主体id
        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public List<string> Mat_Name_List { get; set; }
    }

    //定价信息
    public partial class Material
    {
        //产品价格信息
        [MaxLength(64)]
        public string Price_Is_AM { get; set; }

        //未税参考进价
        [DefaultValue(0)]
        public decimal Price_Cost_Ref { get; set; }

        //未税契约单价
        [NotMapped]
        [DefaultValue(0)]
        public decimal Price_Cost_Ref_QY { get; set; }

        //未税目标进价
        [DefaultValue(0)]
        public decimal Price_Cost_Target { get; set; }

        //含税参考进价
        [DefaultValue(0)]
        public decimal Price_Cost_Ref_Vat { get; set; }

        //AM厂商面价
        [DefaultValue(0)]
        public decimal Price_AM { get; set; }

        //折扣率
        [DefaultValue(0)]
        public decimal Price_AM_Rate { get; set; }

        [NotMapped]
        public string Price_Set_File_Type { get; set; }

        [NotMapped]
        public List<string> Price_Is_AM_List { get; set; }

        [NotMapped]
        public List<decimal> Price_AM_Rate_List { get; set; }

        //零售价
        [NotMapped]
        [DefaultValue(0)]
        public decimal Price_Retail { get; set; }

        //折扣率
        [DefaultValue(0)]
        public decimal Price_Retail_Rate { get; set; }

        //批发价1
        [NotMapped]
        [DefaultValue(0)]
        public decimal Price_Trade_A { get; set; }

        //折扣率
        [DefaultValue(0)]
        public decimal Price_Trade_A_Rate { get; set; }

        //批发价2
        [NotMapped]
        [DefaultValue(0)]
        public decimal Price_Trade_B { get; set; }

        //折扣率
        [DefaultValue(0)]
        public decimal Price_Trade_B_Rate { get; set; }

        //不含税价
        [NotMapped]
        [DefaultValue(0)]
        public decimal Price_Trade_NoTax { get; set; }

        //折扣率
        [DefaultValue(0)]
        public decimal Price_Trade_NoTax_Rate { get; set; }

        [NotMapped]
        public Material_CODE Mat_CODE_First { get; set; }
    }

    public partial class Material
    {
        [NotMapped]
        public int WMS_Stock_Qty { get; set; }

        [NotMapped]
        public string Cat_Name { get; set; }

        [NotMapped]
        public string Cat_Name_Path { get; set; }

        [NotMapped]
        public int MatchDegree { get; set; }

        //缩略图路径
        [NotMapped]
        public string MatThumbImgPath { get; set; }

        //展示图片
        [NotMapped]
        public string MatImgPath { get; set; }

        //原始图片
        [NotMapped]
        public string MatSourceImgPath { get; set; }

        //商品详情
        [NotMapped]
        public string MoreDetail { get; set; }

        [NotMapped]
        public List<string> PC_Mon_List { get; set; }

        [NotMapped]
        public List<Mat_Mon_Sale> MonSaleList { get; set; }

        [NotMapped]
        public List<Material_CODE> Mat_CODE_List { get; set; }

        [NotMapped]
        public string Mat_CODE_List_STR { get; set; }
    }

    public partial class Material
    {
        public Material()
        {
            MatID = Guid.Empty;
            CatID = Guid.Empty;
            Link_BID = Guid.Empty;
            MatSn = string.Empty;
            Other_MatSn = string.Empty;
            MatSn_Length_Min = 0;
            MatSn_Length_Max = 0;
            MatName = string.Empty;
            MatSpecifications = string.Empty;
            MatBrand = string.Empty;
            MatUnit = string.Empty;
            MOQ = string.Empty;
            PC_Day = string.Empty;
            PC = string.Empty;
            PC_Mon = string.Empty;
            PC_Mon_Type = string.Empty;
            PC_Place = string.Empty;

            Weight = 0;
            MatThumbImgPath = string.Empty;
            MatImgPath = string.Empty;
            MatSourceImgPath = string.Empty;
            MoreDetail = string.Empty;
            MatSummary = string.Empty;
            CreateTime = DateTime.Now;
            CreatePerson = string.Empty;
            LastUpdateTime = DateTime.Now;
            LastUpdatePerson = string.Empty;
            Link_IMGID = Guid.Empty;
            Link_IMDID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            MatchDegree = 0;
            Cat_Name = string.Empty;
            Cat_Name_Path = string.Empty;
            MonSaleList = new List<Mat_Mon_Sale>();
            Mat_CODE_First = new Material_CODE();
            Mat_CODE_List = new List<Material_CODE>();
            Mat_CODE_List_STR = string.Empty;
            WMS_Stock_Qty = 0;
            QRCode_Path = string.Empty;

            PC_Mon_List = new List<string>();
            PC_Mon_List.Add("N+1");
            PC_Mon_List.Add("N+2");
            PC_Mon_List.Add("N+3");
            PC_Mon_List.Add("N+4");
            PC_Mon_List.Add("N+5");
            PC_Mon_List.Add("N+6");

            Price_Is_AM_List = new List<string>();
            Price_Is_AM_List.Add(Price_Is_AM_Emun.是.ToString());
            Price_Is_AM_List.Add(Price_Is_AM_Emun.否.ToString());

            Price_AM_Rate_List = new List<decimal>();

            Price_Is_AM = string.Empty;
            Price_Cost_Ref_Vat = 0;
            Price_Cost_Ref = 0;
            Price_Cost_Ref_QY = 0;
            Price_Set_File_Type = string.Empty;
        }

    }

    public class Material_CODE
    {
        public Material_CODE()
        {
            Mat_CODE_ID = Guid.Empty;
            Link_MatID = Guid.Empty;
            Create_DT = DateTime.Now;
            CODE = string.Empty;
            Order_Window = string.Empty;
            Order_Price = 0;
            Line_Number = 0;
        }

        [Key]
        public Guid Mat_CODE_ID { get; set; }

        [Required]
        public Guid Link_MatID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        [DefaultValue("")]
        public string CODE { get; set; }

        [DefaultValue("")]
        public string Order_Window { get; set; }

        [Required]
        public decimal Order_Price { get; set; }

        [NotMapped]
        public int Line_Number { get; set; }

    }

    public class Material_Name
    {
        public Material_Name()
        {
            Name_ID = Guid.Empty;
            Mat_Name = string.Empty;
            LinkMainCID = Guid.Empty;
            Create_DT = DateTime.Now;
            Create_Person = string.Empty;
        }

        [Key]
        public Guid Name_ID { get; set; }

        [Required]
        public string Mat_Name { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string Create_Person { get; set; }

    }

    public class Material_Temp_Img
    {
        public Material_Temp_Img()
        {
            UID = Guid.Empty;
            MatThumbImgPath = string.Empty;
            MatImgPath = string.Empty;
            MatSourceImgPath = string.Empty;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid UID { get; set; }

        //缩略图路径
        [DefaultValue("")]
        public string MatThumbImgPath { get; set; }

        //展示图片
        [DefaultValue("")]
        public string MatImgPath { get; set; }

        //原始图片
        [DefaultValue("")]
        public string MatSourceImgPath { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }

    [NotMapped]
    public class Mat_Mon_Sale
    {
        public string Mon { get; set; }
        public DateTime Mon_SD { get; set; }
        public DateTime Mon_ED { get; set; }
        public int IsThisMon { get; set; }
        public int Sales_Sum_Qty { get; set; }
    }

    [NotMapped]
    public class Material_Filter
    {
        public Material_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Keyword = string.Empty;
            MatBrand = string.Empty;
            Sales_UID = Guid.Empty;
            Link_BID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            CatID = Guid.Empty;
            BrandList = new List<string>();
            MonSalesList = new List<Mat_Mon_Sale>();
            IsMySalesList = new List<SelectOption>();
            IsMySalesList.Add(new SelectOption { Key_STR = MySales_Enum.IsMySales.ToString(), Name = "已添加产品" });
            IsMySalesList.Add(new SelectOption { Key_STR = MySales_Enum.IsNotMySales.ToString(), Name = "待添加产品" });

            Excel_Mat_Status = string.Empty;
            Excel_Mat_Status_List = new List<string>();
            foreach(var x in Enum.GetNames(typeof(Mat_Excel_Mat_Status_Enum)).ToList())
            {
                Excel_Mat_Status_List.Add(x);
            }

            IsPublicList = new List<string>();
            IsPublicList.Add(Mat_Public.未上架.ToString());
            IsPublicList.Add(Mat_Public.已上架.ToString());

            Price_Is_AM_List = new List<string>();
            Price_Is_AM_List.Add(Price_Is_AM_Emun.是.ToString());
            Price_Is_AM_List.Add(Price_Is_AM_Emun.否.ToString());

            Is_Stock_List = new List<string>();
            Is_Stock_List.Add(Is_Stock_Emun.重点备货.ToString());
            Is_Stock_List.Add(Is_Stock_Emun.非重点备货.ToString());
            Other_MatSn = string.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string MatSn { get; set; }
        public string Other_MatSn { get; set; }
        public string MatName { get; set; }

        public string CODE { get; set; }
        public string PC_Place { get; set; }
        public string PC { get; set; }
        public string PC_Day { get; set; }
        public string PC_Mon_Type { get; set; }
        public string Price_Is_AM { get; set; }
        public string Is_Stock { get; set; }
        
        public string Keyword { get; set; }
        public string MatBrand { get; set; }
        public Guid Link_IMGID { get; set; }
        public Guid Sales_UID { get; set; }
        public Guid CatID { get; set; }
        public string CatID_Str { get; set; }
        public string IsMySales { get; set; }
        public string IsPublic { get; set; }
        public List<SelectOption> IsMySalesList { get; set; }
        public List<Mat_Mon_Sale> MonSalesList { get; set; }
        public List<string> BrandList { get; set; }
        public List<string> IsPublicList { get; set; }
        public List<string> Price_Is_AM_List { get; set; }
        public List<string> Is_Stock_List { get; set; }
        public string Excel_Mat_Status { get; set; }
        public string Excel_File_Type { get; set; }
        public List<string> Excel_Mat_Status_List { get; set; }
        public Guid Link_BID { get; set; }
        public Guid LinkMainCID { get; set; }
    }

    public enum Price_Is_AM_Emun
    {
        是,
        否,
    }

    public enum Is_Stock_Emun
    {
        重点备货,
        非重点备货,
    }

    public enum Mat_Public
    {
        已上架,
        未上架,
    }
}

namespace SMART.Api.Models
{
    [NotMapped]
    public class Mat_Price_Change
    {
        public string Person { get; set; }

        public DateTime Create_DT { get; set; }

        public Material Mat_OLD { get; set; }

        public Material Mat_New { get; set; }
    }
}

namespace SMART.Api.Models
{
    public class Mat_Excel
    {
        public Mat_Excel()
        {
            MEID = Guid.Empty;
            BID = Guid.Empty;
            Brand_Name = string.Empty;
            Create_DT = DateTime.Now;
            Upload_Person = string.Empty;
            File_Name = string.Empty;
            File_Type = string.Empty;
            Is_Input = 0;
            Link_UID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            Is_Can_Input_Count = 0;
            Mat_Excel_Line_PageList = new PageList<Mat_Excel_Line>();
        }

        [Key]
        public Guid MEID { get; set; }

        [Required]
        public Guid BID { get; set; }

        [Required]
        public string Brand_Name { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        [DefaultValue("")]
        public string Upload_Person { get; set; }

        [Required]
        [DefaultValue("")]
        public string File_Name { get; set; }

        [Required]
        [DefaultValue("")]
        public string File_Type { get; set; }

        [DefaultValue(0)]
        public int Is_Input { get; set; }

        [Required]
        public Guid Link_UID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public int Is_Can_Input_Count { get; set; }

        [NotMapped]
        public PageList<Mat_Excel_Line> Mat_Excel_Line_PageList { get; set; }
    }

    public class Mat_Excel_Line
    {
        public Mat_Excel_Line()
        {
            MatID = Guid.Empty;
            Link_MEID = Guid.Empty;
            Create_DT = DateTime.Now;
            File_Type = string.Empty;
            Upload_Person = string.Empty;
            CODE = string.Empty;
            CODE_Order_Window = string.Empty;
            CODE_Order_Price = 0;

            MatSn = string.Empty;
            MatName = string.Empty;
            MatSpecifications = string.Empty;
            MatBrand = string.Empty;
            PC = string.Empty;
            PC_Place = string.Empty;
            PC_Day = string.Empty;
            PC_Mon = string.Empty;
            PC_Mon_Type = string.Empty;

            Link_BID = Guid.Empty;
            MatUnit = string.Empty;
            Weight = 0;
            MOQ = string.Empty;

            LineNumber = 0;
            Is_Input = 0;

            Is_Error_MatSn = 0;
            Is_Error_Brand = 0;
            Is_Error = 0;
        }

        [Key]
        public Guid LineID { get; set; }

        [Required]
        public Guid Link_MEID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [DefaultValue("")]
        public string File_Type { get; set; }

        [Required]
        [DefaultValue("")]
        public string Upload_Person { get; set; }

        [DefaultValue(0)]
        public int LineNumber { get; set; }

        [Required]
        public Guid MatID { get; set; }

        [DefaultValue("")]
        public string CODE { get; set; }

        [DefaultValue("")]
        public string CODE_Order_Window { get; set; }

        [DefaultValue(0)]
        public decimal CODE_Order_Price { get; set; }

        //商品编号
        [Required]
        public string MatSn { get; set; }

        //物料名称
        [DefaultValue("")]
        public string MatName { get; set; }

        //规格
        [DefaultValue("")]
        public string MatSpecifications { get; set; }

        //所属品牌
        [DefaultValue("")]
        public Guid Link_BID { get; set; }

        //所属品牌
        [DefaultValue("")]
        public string MatBrand { get; set; }

        //计量单位
        [Required]
        public string MatUnit { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Weight { get; set; }

        [DefaultValue("")]
        public string MOQ { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Pack_Qty { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Is_Stock { get; set; }

        [DefaultValue("")]
        public string PC { get; set; }

        [DefaultValue("")]
        public string PC_Place { get; set; }

        [DefaultValue("")]
        public string PC_Day { get; set; }

        [DefaultValue("")]
        public string PC_Mon { get; set; }

        [DefaultValue("")]
        public string PC_Mon_Type { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price_AM { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price_AM_Rate { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price_Cost_Ref { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price_Cost_Target { get; set; }

        [NotMapped]
        [DefaultValue(0)]
        public decimal Price_Cost_Ref_Vat { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price_Retail_Rate { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price_Trade_A_Rate { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price_Trade_B_Rate { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price_Trade_NoTax_Rate { get; set; }

        [DefaultValue(0)]
        public int Is_Input { get; set; }

        [DefaultValue(0)]
        public int Is_Error_MatSn { get; set; }

        [DefaultValue(0)]
        public int Is_Error_Brand { get; set; }

        [DefaultValue(0)]
        public int Is_Error { get; set; }

    }

    public enum Mat_Excel_Mat_Status_Enum
    {
        全部产品,
        允许导入产品,
        系统已有产品,
        检查错误产品,
        完成导入产品,
    }

    public enum Mat_Excel_File_Type
    {
        批量导入,
        价格维护,
        价格变动_旧,
        价格变动_新,
    }
}

