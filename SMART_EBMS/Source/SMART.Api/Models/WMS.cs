using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//库位
namespace SMART.Api.Models
{
    //仓库储位表
    public class WMS_Location
    {
        public WMS_Location()
        {
            Loc_ID = Guid.Empty;
            Location = string.Empty;
            Floor_Height = string.Empty;
            LinkMainCID = Guid.Empty;
            QRCode_Path = string.Empty;
            Height_Enum_List = Enum.GetNames(typeof(Height_Enum)).ToList();
            Type = string.Empty;
            Type_Enum_List = Enum.GetNames(typeof(Type_Enum)).ToList();
            Remark = string.Empty;
            Link_MatSn_Count = string.Empty;
            Link_MatSn_Count_Enum_List = Enum.GetNames(typeof(Link_MatSn_Count_Enum)).ToList();
        }

        [Key]
        public Guid Loc_ID { get; set; }

        //库位编号
        [Required]
        public string Location { get; set; }

        public string Floor_Height { get; set; }

        public string Type { get; set; }

        public string Remark { get; set; }

        public string Link_MatSn_Count{ get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
        
        public string QRCode_Path { get; set; }
     
        [NotMapped]
        public List<string> Height_Enum_List { get; set; }

        [NotMapped]
        public List<string> Type_Enum_List { get; set; }

        [NotMapped]
        public List<string> Link_MatSn_Count_Enum_List { get; set; }
    }

    [NotMapped]
    public class WMS_Location_Filter
    {
        public WMS_Location_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Keyword = string.Empty;
            LinkMainCID = Guid.Empty;
            Height_Enum_List= Enum.GetNames(typeof(Height_Enum)).ToList();
            Type = string.Empty;
            Type_Enum_List = Enum.GetNames(typeof(Type_Enum)).ToList();
            Link_MatSn_Count = string.Empty;
            Link_MatSn_Count_Enum_List= Enum.GetNames(typeof(Link_MatSn_Count_Enum)).ToList();
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public Guid LinkMainCID { get; set; }
        public List<string> Height_Enum_List { get; set; }
        public string Type { get; set; }
        public List<string> Type_Enum_List { get; set; }
        public string Link_MatSn_Count { get; set; }
        public List<string> Link_MatSn_Count_Enum_List { get; set; }
    }

    public enum Height_Enum
    {
        一层,
        两层,
        三层,
        四层,
        五层,
    }

    public enum Type_Enum
    {
        整箱,
        端数,
    }

    public enum Link_MatSn_Count_Enum
    {
        八个,
        不限,
    }

    public enum Currency_Enum
    {
        CNY,
        USD,
        EUR,
        JPY,
    }
    
}

//收货任务
namespace SMART.Api.Models
{
    //收货任务
    [NotMapped]
    public class WMS_In_Task
    {
        public WMS_In_Task()
        {
            Head_ID = Guid.Empty;
            Task_Bat_No_Str = string.Empty;
            Create_DT = DateTime.Now;
            Create_Person = string.Empty;
            Logistics_Company = string.Empty;
            Logistics_Mode = string.Empty;
            MatType = string.Empty;
            Brand = string.Empty;
            Logistics_Cost_Type = string.Empty;
            Customer_Name = string.Empty;
            Supplier_Name = string.Empty;
            Global_State = string.Empty;
            MatSn_Count = 0;
            Line_Count = 0;
            Line_Quantity_Sum = 0;
            Line_List = new List<WMS_In_Task_Line>();
            Track_List = new List<WMS_Track>();
            WMS_In_Line_List = new List<WMS_In_Line>();
            Group_Tray_List = new List<WMS_In_Task_Group_Tray>();
            Scan_Error_List = new List<WMS_In_Scan_Error>();
            Scan_List = new List<WMS_In_Scan>();
            Work_Person = string.Empty;
            Driver_Name = string.Empty;
            Scan_Mat_Type = string.Empty;
            Link_WMS_In_No = string.Empty;
            Sup_ID = Guid.Empty;
            Head_Type = string.Empty;
        }

        public Guid Head_ID { get; set; }
        public string Task_Bat_No_Str { get; set; }
        public DateTime Create_DT { get; set; }
        public string Create_Person { get; set; }
        public string Logistics_Company { get; set; }
        public string Logistics_Mode { get; set; }
        public string MatType { get; set; }
        public string Brand { get; set; }
        public string Logistics_Cost_Type { get; set; }
        public string Customer_Name { get; set; }
        public string Supplier_Name { get; set; }
        public string Global_State { get; set; }
        public string Scan_Mat_Type { get; set; }
        public string Link_WMS_In_No { get; set; }
        public Guid Sup_ID { get; set; }

        //型号数
        public int MatSn_Count { get; set; }

        //总行数
        public int Line_Count { get; set; }

        //到货总数
        public int Line_Quantity_Sum { get; set; }

        //合并产品型号列表
        public List<WMS_In_Task_Line> Line_List { get; set; }

        //快递费用记录
        public List<WMS_Track> Track_List { get; set; }

        //原单据信息
        public List<WMS_In_Line> WMS_In_Line_List { get; set; }

        //托盘信息
        public List<WMS_In_Task_Group_Tray> Group_Tray_List { get; set; }

        //扫码
        public List<WMS_In_Scan> Scan_List { get; set; }

        //错误的扫码
        public List<WMS_In_Scan_Error> Scan_Error_List { get; set; }

        //收货作业人
        public string Work_Person { get; set; }

        //驾驶员
        public string Driver_Name { get; set; }

        public string Head_Type { get; set; }
    }

    [NotMapped]
    public class WMS_In_Task_Line
    {
        public WMS_In_Task_Line()
        {
            Line_No = 0;
            MatSn = string.Empty;
            MatSn_Real = string.Empty;
            Line_Count = 0;
            Quantity_Sum = 0;
            Quantity_Sum_Scan = 0;
            Cases_Scan_Count = 0;
            Tray_No_List = new List<string>();
            Tray_No_List_Str = string.Empty;
            Track_No_List = new List<string>();
            Track_No_List_Str = string.Empty;
            Line_State = string.Empty;
        }
        
        public int Line_No { get; set; }
        public string MatSn { get; set; }
        public string MatSn_Real { get; set; }
        public int Line_Count { get; set; }
        public int Quantity_Sum { get; set; }
        public int Quantity_Sum_Scan { get; set; }
        public int Cases_Scan_Count { get; set; }
        public List<string> Tray_No_List { get; set; }
        public string Tray_No_List_Str { get; set; }
        public List<string> Track_No_List { get; set; }
        public string Track_No_List_Str { get; set; }
        public string Line_State { get; set; }
    }

    [NotMapped]
    public class WMS_In_Task_Group_Tray
    {
        public WMS_In_Task_Group_Tray()
        {
            Tray_No = string.Empty;
            Scan_List = new List<WMS_In_Scan>();
            Box_Count = 0;
            MatSn_Count = 0;
        }

        public string Tray_No { get; set; }
        public int Box_Count { get; set; }
        public int MatSn_Count { get; set; }
        public List<WMS_In_Scan> Scan_List { get; set; }
    }
   
    public enum WMS_In_Line_State_Enum
    {
        数量一致,
        超出到货,
        低于到货,
        还未扫码,
        多出型号,
    }

    public enum WMS_In_Global_State_Enum
    {
        等待收货,
        通知采购,
        完成入库,
    }

    //收货通知单
    public class WMS_In_Head
    {
        public WMS_In_Head()
        {
            Head_ID = Guid.Empty;
            Task_Bat_No = 0;
            Task_Bat_No_Str = string.Empty;
            Create_DT = DateTime.Now;
            Create_Person = string.Empty;
            Line_Count = 0;
            Line_Count_Scan_Not = 0;
            Line_Count_Wait_To_Location = 0;
            Line_Count_End_To_Location = 0;
            Logistics_Company = string.Empty;
            Logistics_Mode = string.Empty;
            MatType = string.Empty;
            Brand = string.Empty;
            MatSn_Count = 0;
            Quantity_Count = 0;
            Tray_Count = 0;
            Track_Count = 0;
            Logistics_Cost_Type = string.Empty;
            Status = string.Empty;
            LinkMainCID = Guid.Empty;
            Line_List = new List<WMS_In_Line>();
            Supplier_Name = string.Empty;
            Logistics_Cost_Total = 0;
            Track_List = new List<WMS_Track>();
            Work_Person = string.Empty;
            Driver_Name = string.Empty;
            Sup_ID = Guid.Empty;
            In_DT = DateTime.Now;
            In_DT_Str = string.Empty;
            Scan_Mat_Type = string.Empty;
            Work_Person_List = new List<string>();
            Head_Type = string.Empty;
            Return_Remark = string.Empty;
            Link_WMS_In_ID = Guid.Empty;
            Link_WMS_In_No = string.Empty;
        }

        [Key]
        public Guid Head_ID { get; set; }

        [Required]
        public long Task_Bat_No { get; set; }

        [Required]
        public string Task_Bat_No_Str { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string Create_Person { get; set; }

        [NotMapped]
        public DateTime In_DT { get; set; }
        
        public string In_DT_Str { get; set; }

        [NotMapped]
        public int Line_Count { get; set; }

        [NotMapped]
        public int Line_Count_Scan_Not { get; set; }

        [NotMapped]
        public int Line_Count_Wait_To_Location { get; set; }

        [NotMapped]
        public int Line_Count_End_To_Location { get; set; }

        [NotMapped]
        public int Tray_Count { get; set; }

        [NotMapped]
        public int Track_Count { get; set; }
        
        public string Logistics_Company { get; set; }
        
        public string Logistics_Mode { get; set; }

        [NotMapped]
        public decimal Logistics_Cost_Total { get; set; }
        
        public string MatType { get; set; }

        public string Brand { get; set; }

        public string Scan_Mat_Type { get; set; }

        [NotMapped]
        public int MatSn_Count { get; set; }

        [NotMapped]
        public int Quantity_Count { get; set; }
        
        public string Logistics_Cost_Type { get; set; }

        [Required]
        public string Status { get; set; }
        
        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public List<WMS_In_Line> Line_List { get; set; }

        [NotMapped]
        public List<WMS_Track> Track_List { get; set; }

        public Guid Sup_ID { get; set; }

        public string Supplier_Name { get; set; }

        public string Work_Person { get; set; }

        public string Driver_Name { get; set; }
    
        [Required]
        public string Head_Type { get; set; }

        //退货理由
        public string Return_Remark { get; set; }

        //收货ID
        public Guid Link_WMS_In_ID { get; set; }

        //收货批号
        public string Link_WMS_In_No { get; set; }

        [NotMapped]
        public List<string> Work_Person_List { get; set; }

        [NotMapped]
        public List<string> Driver_Person_List { get; set; }
    }

    //收货通知单清单
    public class WMS_In_Line
    {
        public WMS_In_Line()
        {
            Line_ID = Guid.Empty;
            Task_Bat_No = 0;
            Task_Bat_No_Str = string.Empty;
            Create_DT = DateTime.Now;
            Order_DT = DateTime.Now;
            Create_Person = string.Empty;
            Line_No = 0;
            MatName = string.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            MatUnit = string.Empty;
            Quantity = 0;
            Quantity_Request = 0;
            Quantity_Request_More = 0;
            Price_Cost = 0;
            Sales_Person = string.Empty;
            Currency = string.Empty;
            Contract_Number = string.Empty;
            Logistics_Company = string.Empty;
            Logistics_Mode = string.Empty;
            Lading_Bill_No = string.Empty;
            PC_Month = string.Empty;
            Order_Win = string.Empty;
            Order_Win_Code = string.Empty;
            LinkMainCID = Guid.Empty;
            Delivery_DT = DateTime.Now;
            Customer_Name = string.Empty;
            Supplier_Name = string.Empty;
            MatType = string.Empty;
            Logistics_Cost_Type = string.Empty;
            QRCode_Path = string.Empty;
            Link_Head_ID = Guid.Empty;
            Receipt_DT = DateTime.Now;
            Payment_DT = DateTime.Now;
            Max_Quantity = 0;
        }

        [Key]
        public Guid Line_ID { get; set; }

        //任务编号：格式：(YYYYMMdd)+(0001)
        [Required]
        public long Task_Bat_No { get; set; }

        [Required]
        public string Task_Bat_No_Str { get; set; }

        //格式：YYYY-MM-dd HH:mm:ss
        [Required]
        public DateTime Create_DT { get; set; }

        //下单日期
        public DateTime Order_DT { get; set; }

        [Required]
        public string Create_Person { get; set; }

        [Required]
        public int Line_No { get; set; }

        public string MatName { get; set; }

        [Required]
        public string MatSn { get; set; }

        public string MatBrand { get; set; }

        public string MatUnit { get; set; }

        //到货数量Or调货数量
        [Required]
        public int Quantity { get; set; }

        //需求数量
        [Required]
        public int Quantity_Request { get; set; }

        //备货数量
        [Required]
        public int Quantity_Request_More { get; set; }

        //未税单价/调货价格
        [Required]
        public decimal Price_Cost { get; set; }

        //调货人
        [DefaultValue("")]
        public string Sales_Person { get; set; }

        [Required]
        public string Currency { get; set; }

        public string Customer_Name { get; set; }

        public string Supplier_Name { get; set; }

        //预计到货时间
        public DateTime Delivery_DT { get; set; }

        //合同号
        public string Contract_Number { get; set; }

        //出货单号、提单编号
        public string Lading_Bill_No { get; set; }

        //指令月
        public string PC_Month { get; set; }

        //订货窗口
        public string Order_Win { get; set; }

        //订货窗口编号
        public string Order_Win_Code { get; set; }
        
        //物流公司
        public string Logistics_Company { get; set; }

        //运输方式
        public string Logistics_Mode { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
        
        public string MatType { get; set; }
        
        public string Logistics_Cost_Type { get; set; }

        [NotMapped]
        public string QRCode_Path { get; set; }

        //发票日
        public DateTime Receipt_DT { get; set; }

        //付款日
        public DateTime Payment_DT { get; set; }

        [Required]
        public Guid Link_Head_ID { get; set; }

        [NotMapped]
        public int Max_Quantity { get; set; }
    }

    //扫码任务表
    public class WMS_In_Scan
    {
        public WMS_In_Scan()
        {
            Scan_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Link_Head_ID = Guid.Empty;
            MatSn = string.Empty;
            Scan_Quantity = 0;
            Scan_Source = string.Empty;
            Track_No = string.Empty;
            Tray_No = string.Empty;
            Case_No = string.Empty;
            Package_Type = string.Empty;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid Scan_ID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Link_Head_ID { get; set; }

        [Required]
        public string MatSn { get; set; }

        [Required]
        public int Scan_Quantity { get; set; }
        
        [DefaultValue("")]
        public string Scan_Source { get; set; }

        [DefaultValue("")]
        public string Track_No { get; set; }

        [DefaultValue("")]
        public string Tray_No { get; set; }

        [DefaultValue("")]
        public string Case_No { get; set; }

        [Required]
        public string Package_Type { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }

    //扫码任务表-无法识别二维
    public class WMS_In_Scan_Error
    {
        public WMS_In_Scan_Error()
        {
            Scan_EID = Guid.Empty;
            Create_DT = DateTime.Now;
            Link_Head_ID = Guid.Empty;
            Scan_Source = string.Empty;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid Scan_EID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Link_Head_ID { get; set; }

        [DefaultValue("")]
        public string Scan_Source { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }

    //手工填写到货型号
    public class WMS_In_Line_Other
    {
        public WMS_In_Line_Other()
        {
            LineID = Guid.Empty;
            Create_DT = DateTime.Now;
            Link_Head_ID = Guid.Empty;
            MatSn = string.Empty;
            New_MatSn = string.Empty;
            MatSn_Length_Max = 0;
            MatSn_Length_Min = 0;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid LineID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Link_Head_ID { get; set; }

        [Required]
        public string MatSn { get; set; }

        [NotMapped]
        public int MatSn_Length_Max { get; set; }

        [NotMapped]
        public int MatSn_Length_Min { get; set; }

        [Required]
        public string New_MatSn { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }

    //二维码解码装箱
    [NotMapped]
    public class Decode_Scan
    {
        public Decode_Scan()
        {
            Decode_MatSn = string.Empty;
            Decode_Scan_Quantity = 0;
            Is_Scan_Error = false;
        }

        public string Decode_MatSn { get; set; }
        public int Decode_Scan_Quantity { get; set; }
        public bool Is_Scan_Error { get; set; }
    }

    public enum Decode_Scan_Brand
    {
        NSK,
        NMB,
    }

    public enum Logistics_Mode_Enum
    {
        自提,
        快递,
        物流,
        自送,
    }

    public enum WMS_In_Type_Enum
    {
        常规期货,
        零星调货,
    }

    public enum WMS_In_Head_Type_Enum
    {
        订单收货,
        订单退货,   
    }

    public enum Scan_Mat_Type_Enum
    {
        按托,
        按箱,
    }

    public enum Logistics_Cost_Type_Enum
    {
        有快递,
        无快递,
    }

    public enum WMS_Work_Distribution_State_Enum
    {
        已派工,
        未派工,
    }

    [NotMapped]
    public class WMS_In_Filter
    {
        public WMS_In_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Task_Bat_No = string.Empty;
            LinkMainCID = Guid.Empty;
            LinkHeadID = Guid.Empty;
            MatSn = string.Empty;
            Global_State = string.Empty;
            Line_Status = string.Empty;
            Brand = string.Empty;
            MatType = string.Empty;
            Create_Person = string.Empty;
            Logistics_Company = string.Empty;
            Logistics_Mode = string.Empty;
            Line_Status_List = Enum.GetNames(typeof(WMS_In_Line_State_Enum)).ToList();
            Global_State_List = Enum.GetNames(typeof(WMS_In_Global_State_Enum)).ToList();
            Logistics_Mode_List = Enum.GetNames(typeof(Logistics_Mode_Enum)).ToList();
            MatType_List = Enum.GetNames(typeof(WMS_In_Type_Enum)).ToList();
            Logistics_Cost_Type = string.Empty;
            Logistics_Cost_Type_List = Enum.GetNames(typeof(Logistics_Cost_Type_Enum)).ToList();
            Time_Start = string.Empty;
            Time_End = string.Empty;
            Tracking_No = string.Empty;
            Tray_No = string.Empty;
            Brand_List = new List<string>();
            Return_Info = string.Empty;
            Supplier = string.Empty;
            Work_Distribution_Status = string.Empty;
            Work_Distribution_Status_List = Enum.GetNames(typeof(WMS_Work_Distribution_State_Enum)).ToList();
            Head_Type = string.Empty;
            Head_Type_List = Enum.GetNames(typeof(WMS_In_Head_Type_Enum)).ToList();
            Work_Person = string.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Task_Bat_No { get; set; }
        public string Create_Person { get; set; }
        public string Global_State { get; set; }
        public List<string> Global_State_List { get; set; }
        public string Line_Status { get; set; }
        public List<string> Line_Status_List { get; set; }
        public List<string> Logistics_Mode_List { get; set; }
        public string Logistics_Company { get; set; }
        public string Logistics_Mode { get; set; }
        public string Supplier { get; set; }
        public Guid LinkMainCID { get; set; }
        public Guid LinkHeadID { get; set; }
        public string MatSn { get; set; }
        public string Brand { get; set; }
        public string MatType { get; set; }
        public string Logistics_Cost_Type { get; set; }
        public List<string> Logistics_Cost_Type_List { get; set; }
        public List<string> MatType_List { get; set; }
        public List<string> Brand_List { get; set; }
        public string Time_Start { get; set; }
        public string Time_End { get; set; }
        public string Tracking_No { get; set; }
        public string Tray_No { get; set; }
        public string Return_Info { get; set; }
        public string Work_Distribution_Status { get; set; }
        public List<string> Work_Distribution_Status_List { get; set; }
        public string Head_Type { get; set; }
        public List<string> Head_Type_List { get; set; }
        public string Work_Person { get; set; }
    }

    public class WMS_Logistics
    {
        public WMS_Logistics()
        {
            Log_ID = Guid.Empty;
            Company_Code = string.Empty;
            Company_Name = string.Empty;
            LinkMainCID = Guid.Empty;
            Create_DT = DateTime.Now;
            MatType = string.Empty;
        }

        [Key]
        public Guid Log_ID { get; set; }

        [Required]
        public string Company_Code { get; set; }

        [Required]
        public string Company_Name { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string MatType { get; set; }

    }

    [NotMapped]
    public class WMS_Logistics_Filter
    {
        public WMS_Logistics_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            LinkMainCID = Guid.Empty;
            Keyword = string.Empty;
            Company_Code = string.Empty;
            MatType = string.Empty;
            MatType_List = Enum.GetNames(typeof(WMS_In_Type_Enum)).ToList();
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string Company_Code { get; set; }
        public Guid LinkMainCID { get; set; }
        public string MatType { get; set; }
        public List<string> MatType_List { get; set; }
    }

    public class WMS_Work_Person
    {
        public WMS_Work_Person()
        {
            Person_ID = Guid.Empty;
            Person_Name = string.Empty;
            LinkMainCID = Guid.Empty;
            Create_DT = DateTime.Now;
            Choose = false;
        }

        [Key]
        public Guid Person_ID { get; set; }

        [Required]
        public string Person_Name { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public bool Choose { get; set; }
    }

    [NotMapped]
    public class WMS_Work_Person_Filter
    {
        public WMS_Work_Person_Filter()
        {
            PageIndex = 1;
            PageSize = 50;
            LinkMainCID = Guid.Empty;
            Keyword = string.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public Guid LinkMainCID { get; set; }
    }
}

//库存表
namespace SMART.Api.Models
{
    public class WMS_Stock
    {
        public WMS_Stock()
        {
            Stock_ID = Guid.Empty;
            WMS_In_DT = string.Empty;
            WMS_Out_DT = string.Empty;
            MatSn = string.Empty;
            MatName = string.Empty;
            MatBrand = string.Empty;
            MatUnit = string.Empty;
            Quantity = 0;
            Package = string.Empty;
            Location = string.Empty;
            Cases = string.Empty;
            Location_Type = string.Empty;
            QRCode_Path = string.Empty;
            Wms_In_Head_ID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            Price = 0;
        }

        [Key]
        public Guid Stock_ID { get; set; }

        //最近入库时间（YYYY-MM-dd HH:mm）
        public string WMS_In_DT { get; set; }

        //最近出库时间（YYYY-MM-dd HH:mm）
        public string WMS_Out_DT { get; set; }

        [Required]
        public string MatSn { get; set; }

        [DefaultValue("")]
        public string MatName { get; set; }

        [DefaultValue("")]
        public string MatBrand { get; set; }
        
        public string MatUnit { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Package { get; set; }

        public decimal Price { get; set; }

        //库位号
        [Required]
        public string Location { get; set; }

        public string Cases { get; set; }

        [Required]
        public string Location_Type { get; set; }

        [NotMapped]
        public string QRCode_Path { get; set; }

        [Required]
        public Guid Wms_In_Head_ID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

    }

    public class WMS_Stock_Temp
    {
        public WMS_Stock_Temp()
        {
            Stock_Temp_ID = Guid.Empty;
            WMS_In_DT = string.Empty;
            WMS_Out_DT = string.Empty;
            MatSn = string.Empty;
            MatName = string.Empty;
            MatBrand = string.Empty;
            MatUnit = string.Empty;
            Quantity = 0;
            Package = string.Empty;
            Location = string.Empty;
            Case = string.Empty;
            Location_Type = string.Empty;
            WMS_In_Head_ID = Guid.Empty;
            WMS_Out_Head_ID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            Link_Stock_ID = Guid.Empty;
            Price = 0;
        }

        [Key]
        public Guid Stock_Temp_ID { get; set; }

        //最近入库时间（YYYY-MM-dd HH:mm）
        public string WMS_In_DT { get; set; }

        //最近出库时间（YYYY-MM-dd HH:mm）
        public string WMS_Out_DT { get; set; }

        [Required]
        public string MatSn { get; set; }

        [DefaultValue("")]
        public string MatName { get; set; }

        [DefaultValue("")]
        public string MatBrand { get; set; }
        
        public string MatUnit { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Package { get; set; }

        public decimal Price { get; set; }

        //库位号
        [Required]
        public string Location { get; set; }

        public string Case { get; set; }

        [Required]
        public string Location_Type { get; set; }

        [Required]
        public Guid WMS_In_Head_ID { get; set; }

        [Required]
        public Guid WMS_Out_Head_ID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        public Guid Link_Stock_ID { get; set; }
    }
 
    [NotMapped]
    public class WMS_Stock_Filter
    {
        public WMS_Stock_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            LinkMainCID = Guid.Empty;
            MatSn = string.Empty;
            MatSn_A = string.Empty;
            MatSn_B = string.Empty;
            Keyword = string.Empty;
            Location = string.Empty;
            Case = string.Empty;
            Status = string.Empty;
            Location_Type = string.Empty;
            Location_Type_List = Enum.GetNames(typeof(WMS_Stock_Location_Type_Enum)).ToList();
            Task_Status_List = Enum.GetNames(typeof(WMS_Stock_Task_Enum)).ToList();
            Link_HeadID = Guid.Empty;
            Work_Person = string.Empty;
            Time_Start = string.Empty;
            Time_End = string.Empty;
            Property = string.Empty;
            Customer = string.Empty;
            Supplier = string.Empty;
            Quantity_Sum = 0;
            MatBrand = string.Empty;
            Remark = string.Empty;
            Remark_List= Enum.GetNames(typeof(WMS_Stock_Record_Remark_Enum)).ToList();
            Type = string.Empty;
            Type_Enum_List = Enum.GetNames(typeof(Type_Enum)).ToList();
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Guid LinkMainCID { get; set; }
        public string MatSn { get; set; }
        public string MatSn_A { get; set; }
        public string MatSn_B { get; set; }
        public string Keyword { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Location_Type { get; set; }
        public List<string> Location_Type_List { get; set; }
        public List<string> Task_Status_List { get; set; }
        public string Case { get; set; }
        public Guid Link_HeadID { get; set; }
        public string Property { get; set; }
        public string Work_Person { get; set; }
        public string Time_Start { get; set; }
        public string Time_End { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string MatBrand { get; set; }
        public int Quantity_Sum { get; set; }
        public string Remark { get; set; }
        public List<string> Remark_List { get; set; }
        public string Type { get; set; }
        public List<string> Type_Enum_List { get; set; }

    }

    [NotMapped]
    public class WMS_Stock_Group_Location
    {
        public WMS_Stock_Group_Location()
        {
            Location = string.Empty;
            Task_Bat_No = string.Empty;
            Supplier_Name = string.Empty;
            Link_HeadID = Guid.Empty;
            Stock_List = new List<WMS_Stock>();
        }

        public string Location { get; set; }
        public string Task_Bat_No { get; set; }
        public string Supplier_Name { get; set; }
        public Guid Link_HeadID { get; set; }
        public List<WMS_Stock> Stock_List { get; set; }
    }

    [NotMapped]
    public class WMS_Stock_Group
    {
        public WMS_Stock_Group()
        {
            MatName = string.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            Line_Count = 0;
            Quantity_Sum = 0;
            Quantity_Avaliable = 0;
            Quantity_Occupied = 0;
            Quantity_Preoccupancy = 0;
            Package = string.Empty;
            In_DT = string.Empty;
            Price = 0;
            Location = string.Empty;
            GroupID = Guid.Empty;
            Quantity_Diff = 0;
            Pack_Qty = 0;
            Loc_List = new List<string>();
        }
        public Guid GroupID { get; set; }
        public string MatName { get; set; }
        public string MatSn { get; set; }
        public string MatBrand { get; set; }
        public int Line_Count { get; set; }

        //在库库存
        public int Quantity_Sum { get; set; }

        //可用库存
        public int Quantity_Avaliable { get; set; }

        //未配货库存
        public int Quantity_Occupied { get; set; }

        //已配货库存
        public int Quantity_Preoccupancy { get; set; }

        //在库差异
        public int Quantity_Diff { get; set; }

        public string Package { get; set; }
        public string In_DT { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public int Pack_Qty { get; set; }
        public List<string> Loc_List { get; set; }
    }

    public enum WMS_Stock_Package_Enum
    {
        整箱,
        零头,
    }

    public enum WMS_Stock_Location_Type_Enum
    {
        临时库位,
        标准库位,
    }

    //盘库任务
    public class WMS_Stock_Task
    {
        public WMS_Stock_Task()
        {
            Task_ID = Guid.Empty;
            Location = string.Empty;
            Create_DT = DateTime.Now;
            LinkMainCID = Guid.Empty;
            Work_Person = string.Empty;
            Status = string.Empty;
            MatSn_Count = 0;
            Quantity_Sum = 0;
            Line_List = new List<WMS_Stock_Task_Line>();
            Scan_List = new List<WMS_Stocktaking_Scan>();
            Property = string.Empty;
            Type = string.Empty;
            Link_HeadID = Guid.Empty;
            Recommend_Status = string.Empty;
        }

        [Key]
        public Guid Task_ID { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        public string Work_Person { get; set; }

        public string Property { get; set; }

        public string Type { get; set; }
        
        public Guid Link_HeadID { get; set; }

        //推荐移库的状态
        public string Recommend_Status { get; set; }

        [NotMapped]
        public int MatSn_Count { get; set; }

        [NotMapped]
        public int Quantity_Sum { get; set; }

        [NotMapped]
        public List<WMS_Stock_Task_Line> Line_List { get; set; }

        [NotMapped]
        public List<WMS_Stocktaking_Scan> Scan_List { get; set; }
    }

    [NotMapped]
    public class WMS_Stock_Task_Line
    {
        public WMS_Stock_Task_Line()
        {
            Line_ID = Guid.Empty;
            Line_No = 0;
            MatSn = string.Empty;
            Quantity_Sum = 0;
            Quantity_Scan_Sum = 0;
            Status = string.Empty;
        }

        public Guid Line_ID { get; set; }
        public int Line_No { get; set; }
        public string MatSn { get; set; }
        public int Quantity_Sum { get; set; }
        public int Quantity_Scan_Sum{ get; set; }
        public string Status { get; set; }
    }

    public class WMS_Profit_Loss
    {
        public WMS_Profit_Loss()
        {
            Line_ID = Guid.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            Old_Quantity = 0;
            New_Quantity = 0;
            Diff_Quantity = 0;
            Location = string.Empty;
            Create_DT = DateTime.Now;
            Link_TaskID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            Work_Person = string.Empty;
            Status = string.Empty;
            Price = 0;
            Total_Price = 0;
        }

        [Key]
        public Guid Line_ID { get; set; }

        [Required]
        public string MatSn { get; set; }
        
        public string MatBrand { get; set; }

        [Required]
        public int Old_Quantity { get; set; }

        [Required]
        public int New_Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [NotMapped]
        public decimal Total_Price { get; set; }

        [NotMapped]
        public int Diff_Quantity { get; set; }

        [Required]
        public string Location { get; set; }
        
        public string Work_Person { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Link_TaskID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }

    public class WMS_Profit_Loss_Other
    {
        public WMS_Profit_Loss_Other()
        {
            Line_ID = Guid.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            Old_Quantity = 0;
            New_Quantity = 0;
            Diff_Quantity = 0;
            Location = string.Empty;
            Create_DT = DateTime.Now;
            Link_TaskID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            Work_Person = string.Empty;
            Status = string.Empty;
            Price = 0;
            Total_Price = 0;
        }

        [Key]
        public Guid Line_ID { get; set; }

        [Required]
        public string MatSn { get; set; }

        public string MatBrand { get; set; }

        [Required]
        public int Old_Quantity { get; set; }

        [Required]
        public int New_Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [NotMapped]
        public decimal Total_Price { get; set; }

        [NotMapped]
        public int Diff_Quantity { get; set; }

        [Required]
        public string Location { get; set; }

        public string Work_Person { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Link_TaskID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }

    public enum WMS_Stock_Task_Line_State_Enum
    {
        数量一致,
        超出系统,
        低于系统,
        还未扫码,
        多出型号,
    }

    public enum WMS_Stock_Task_Enum
    {
        未盘库,
        已盘库,
    }

    public enum WMS_Profit_Loss_Status_Enum
    {
        未确定,
        已确定,
    }

    public enum WMS_Recommend_Status_Enum
    {
        未推荐,
        已推荐,
    }

    public enum WMS_Stock_Task_Property_Enum
    {
        日常盘库,
        首次盘库,
        配货动盘,
    }
}

//出库任务
namespace SMART.Api.Models
{
    //送货任务
    [NotMapped]
    public class WMS_Out_Task
    {
        public WMS_Out_Task()
        {
            Head_ID = Guid.Empty;
            Task_Bat_No_Str = string.Empty;
            Create_DT = DateTime.Now;
            Create_Person = string.Empty;
            Logistics_Company = string.Empty;
            Logistics_Mode = string.Empty;
            Brand = string.Empty;
            Logistics_Cost_Type = string.Empty;
            Link_Cus_ID = Guid.Empty;
            Customer_Name = string.Empty;
            Customer_Address = string.Empty;
            Customer_Tel = string.Empty;
            Global_State = string.Empty;
            Scan_Mat_Type = string.Empty;
            MatSn_Count = 0;
            Line_Count = 0;
            Line_Quantity_Sum = 0;
            Line_List = new List<WMS_Out_Task_Line>();
            WMS_Out_Line_List = new List<WMS_Out_Line>();
            Group_Tray_List = new List<WMS_Out_Task_Group_Tray>();
            Track_List = new List<WMS_Track>();
            Scan_List = new List<WMS_Out_Scan>();
            Task_List = new List<WMS_Stock_Task>();
            Pick_Scan_List = new List<WMS_Out_Pick_Scan>();
            Total_Cases = 0;
            Is_Finish_Pick_Location = 0;
        }

        public Guid Head_ID { get; set; }
        public string Task_Bat_No_Str { get; set; }
        public DateTime Create_DT { get; set; }
        public string Create_Person { get; set; }
        public string Logistics_Company { get; set; }
        public string Logistics_Mode { get; set; }
        public string Brand { get; set; }
        public string Logistics_Cost_Type { get; set; }
        public Guid Link_Cus_ID { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Address { get; set; }
        public string Customer_Tel { get; set; }
        public string Global_State { get; set; }
        public string Scan_Mat_Type { get; set; }

        //型号数
        public int MatSn_Count { get; set; }

        //总行数
        public int Line_Count { get; set; }

        //配货总数
        public int Line_Quantity_Sum { get; set; }

        //合并产品型号列表
        public List<WMS_Out_Task_Line> Line_List { get; set; }

        //原单据信息
        public List<WMS_Out_Line> WMS_Out_Line_List { get; set; }

        //快递费用记录
        public List<WMS_Track> Track_List { get; set; }

        //托盘信息
        public List<WMS_Out_Task_Group_Tray> Group_Tray_List { get; set; }

        //扫描列表
        public List<WMS_Out_Scan> Scan_List { get; set; }

        //取货库位列表
        public List<WMS_Out_Pick_Scan> Pick_Scan_List { get; set; }

        //配货动盘列表
        public List<WMS_Stock_Task> Task_List { get; set; }

        //出货作业人
        public string Work_Out_Person { get; set; }

        //下架作业人
        public string Work_Down_Person { get; set; }

        public int Total_Cases { get; set; }

        public int Is_Finish_Pick_Location { get; set; }
        
    }

    [NotMapped]
    public class WMS_Out_Task_Line
    {
        public WMS_Out_Task_Line()
        {
            Line_No = 0;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            Pack_Qty = 0;
            Quantity_Sum = 0;
            Quantity_Sum_Scan = 0;
            Tray_No_List = new List<string>();
            Tray_No_List_Str = string.Empty;
            Commend_Loc_List = new List<WMS_Out_Line_Commend_Loc>();
            Scan_List = new List<WMS_Stocktaking_Scan>();
            Line_State = string.Empty;
            Link_HeadID = Guid.Empty;
            Is_Scan = false;
            Scan_Location = string.Empty;
        }

        public int Line_No { get; set; }
        public string MatSn { get; set; }
        public int Pack_Qty { get; set; }
        public string MatBrand { get; set; }
        public int Quantity_Sum { get; set; }
        public int Quantity_Sum_Scan { get; set; }
        public List<string> Tray_No_List { get; set; }
        public string Tray_No_List_Str { get; set; }
        public List<WMS_Out_Line_Commend_Loc> Commend_Loc_List { get; set; }
        public List<WMS_Stocktaking_Scan> Scan_List { get; set; }
        public string Line_State { get; set; }
        public string Scan_Location { get; set; }
        public bool Is_Scan { get; set; }
        public Guid Link_HeadID { get; set; }
    }

    [NotMapped]
    public class WMS_Out_Task_Group_Tray
    {
        public WMS_Out_Task_Group_Tray()
        {
            Tray_No = string.Empty;
            Scan_List = new List<WMS_Out_Scan>();
            Box_Count = 0;
            MatSn_Count = 0;
        }

        public string Tray_No { get; set; }
        public int Box_Count { get; set; }
        public int MatSn_Count { get; set; }
        public List<WMS_Out_Scan> Scan_List { get; set; }
    }

    //送货单
    public class WMS_Out_Head
    {
        public WMS_Out_Head()
        {
            Head_ID = Guid.Empty;
            Task_Bat_No = 0;
            Task_Bat_No_Str = string.Empty;
            Create_DT = DateTime.Now;
            Out_DT = DateTime.Now;
            Create_Person = string.Empty;
            Line_Count = 0;
            Line_Count_Scan_Not = 0;
            Logistics_Company = string.Empty;
            Logistics_Mode = string.Empty;
            Brand = string.Empty;
            MatSn_Count = 0;
            Quantity_Sum = 0;
            Logistics_Cost_Type = string.Empty;
            Status = string.Empty;
            LinkMainCID = Guid.Empty;
            Line_List = new List<WMS_Out_Line>();
            Customer_Name = string.Empty;
            Logistics_Cost_Total = 0;
            Work_Out_Person = string.Empty;
            Work_Down_Person = string.Empty;
            Link_Cus_ID = Guid.Empty;
            Customer_Address = string.Empty;
            Customer_Tel = string.Empty;
            Driver_Name = string.Empty;
            Out_DT_Str = string.Empty;
            Scan_Mat_Type = string.Empty;
            Total_Cases = 0;
            Work_Down_Person_List = new List<string>();
            Work_Out_Person_List = new List<string>();
            Driver_Person_List = new List<string>();
            Head_Type = string.Empty;
            Return_Remark = string.Empty;
            Link_WMS_Out_ID = Guid.Empty;
            Link_WMS_Out_No = string.Empty;
        }

        [Key]
        public Guid Head_ID { get; set; }

        [Required]
        public long Task_Bat_No { get; set; }

        [Required]
        public string Task_Bat_No_Str { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [NotMapped]
        public DateTime Out_DT { get; set; }
        
        public string Out_DT_Str { get; set; }

        [Required]
        public string Create_Person { get; set; }

        [NotMapped]
        public int Line_Count { get; set; }

        [NotMapped]
        public int Line_Count_Scan_Not { get; set; }

        public string Logistics_Company { get; set; }
        
        public string Logistics_Mode { get; set; }

        [NotMapped]
        public decimal Logistics_Cost_Total { get; set; }

        public string Brand { get; set; }

        public string Scan_Mat_Type { get; set; }

        [NotMapped]
        public int MatSn_Count { get; set; }

        [NotMapped]
        public int Quantity_Sum { get; set; }

        public string Logistics_Cost_Type { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
        
        public Guid Link_Cus_ID { get; set; }

        [NotMapped]
        public List<WMS_Out_Line> Line_List { get; set; }

        public string Customer_Name { get; set; }

        public string Customer_Address { get; set; }

        public string Customer_Tel { get; set; }

        public string Work_Out_Person { get; set; }

        public string Work_Down_Person { get; set; }

        public string Driver_Name { get; set; }

        public int Total_Cases { get; set; }

        [Required]
        public string Head_Type { get; set; }

        //退货理由
        public string Return_Remark { get; set; }

        //出货ID
        public Guid Link_WMS_Out_ID { get; set; }

        //出货批号
        public string Link_WMS_Out_No { get; set; }

        [NotMapped]
        public List<string> Work_Down_Person_List { get; set; }

        [NotMapped]
        public List<string> Work_Out_Person_List { get; set; }

        [NotMapped]
        public List<string> Driver_Person_List { get; set; }
    }

    //送货单清单
    public class WMS_Out_Line
    {
        public WMS_Out_Line()
        {
            Line_ID = Guid.Empty;
            Task_Bat_No = 0;
            Task_Bat_No_Str = string.Empty;
            Create_DT = DateTime.Now;
            Create_Person = string.Empty;
            Line_No = 0;
            MatName = string.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            MatUnit = string.Empty;
            Quantity = 0;
            Price = 0;
            Total_Price=0;
            Logistics_Company = string.Empty;
            Logistics_Mode = string.Empty;
            LinkMainCID = Guid.Empty;
            Customer_Name = string.Empty;
            Logistics_Cost_Type = string.Empty;
            QRCode_Path = string.Empty;
            Link_Head_ID = Guid.Empty;
            Receipt_DT = DateTime.Now;
            Payment_DT = DateTime.Now;
            Out_DT_Str = string.Empty;
            Is_Chose = 0;
            Max_Quantity = 0;
        }

        [Key]
        public Guid Line_ID { get; set; }

        //任务编号：格式：(YYYYMMdd)+(0001)
        [Required]
        public long Task_Bat_No { get; set; }

        [Required]
        public string Task_Bat_No_Str { get; set; }

        //格式：YYYY-MM-dd HH:mm:ss
        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string Create_Person { get; set; }

        [Required]
        public int Line_No { get; set; }

        public string MatName { get; set; }

        [Required]
        public string MatSn { get; set; }

        public string MatBrand { get; set; }

        public string MatUnit { get; set; }

        //配货数量
        [Required]
        public int Quantity { get; set; }

        [NotMapped]
        public int Max_Quantity { get; set; }

        //未税单价
        [Required]
        public decimal Price { get; set; }

        [NotMapped]
        public decimal Total_Price { get; set; }

        public string Customer_Name { get; set; }

        //物流公司
        public string Logistics_Company { get; set; }

        //运输方式
        public string Logistics_Mode { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        public string Logistics_Cost_Type { get; set; }

        [NotMapped]
        public string QRCode_Path { get; set; }

        //发票日
        public DateTime Receipt_DT { get; set; }

        //付款日
        public DateTime Payment_DT { get; set; }

        //页面勾选
        public int Is_Chose { get; set; }

        [Required]
        public Guid Link_Head_ID { get; set; }
       
        [NotMapped]
        public string Out_DT_Str { get; set; }
    }

    [NotMapped]
    public class WMS_Out_Line_Commend_Loc
    {
        public WMS_Out_Line_Commend_Loc()
        {
            Location = string.Empty;
            Qty_Sum = 0;
            Qty_Diff = 0;
            Box_Count = 0;
            MatchDegree = 0;
            Supplier = string.Empty;
        }

        public string Location { get; set; }
        public int Qty_Sum { get; set; }
        public int Qty_Diff { get; set; }
        public int Box_Count { get; set; }
        public int MatchDegree { get; set; }
        public string Supplier { get; set; }
    }
    
    [NotMapped]
    public class WMS_Out_Task_Group_Location
    {
        public WMS_Out_Task_Group_Location()
        {
            Location = string.Empty;
            MatSn_Count = 0;
            Stocktaking_Scan_List = new List<WMS_Stocktaking_Scan>();
        }

        public string Location { get; set; }
        public int MatSn_Count { get; set; }
        public List<WMS_Stocktaking_Scan> Stocktaking_Scan_List { get; set; }
    }

    //配货记录
    public class WMS_Out_Pick_Scan
    {
        public WMS_Out_Pick_Scan()
        {
            Scan_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Scan_Person = string.Empty;
            MatSn = string.Empty;
            Link_TaskID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            Status = string.Empty;
            Scan_Location = string.Empty;
            Quantity = 0;
            Is_Chose = 0;
            Is_Chose_Sim = 0;
            Customer = string.Empty;
            Stocktaking_ID = Guid.Empty;
            Stocktaking_Loction_Type = string.Empty;
            Pack_Qty = 0;
        }

        [Key]
        public Guid Scan_ID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }
        
        [Required]
        public string Scan_Person { get; set; }

        [Required]
        public string Scan_Location { get; set; }

        [Required]
        public string MatSn { get; set; }

        [Required]
        public int Quantity { get; set; }

        //关联任务
        [Required]
        public Guid Link_TaskID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [Required]
        public string Status { get; set; }
        
        //勾选使用(附加其他动作)
        public int Is_Chose { get; set; }

        //勾选使用
        public int Is_Chose_Sim { get; set; }

        [NotMapped]
        public Guid Stocktaking_ID { get; set; }

        [NotMapped]
        public string Stocktaking_Loction_Type { get; set; }

        [NotMapped]
        public string Customer { get; set; }

        [NotMapped]
        public int Pack_Qty { get; set; }
    }

    //验货扫描
    public class WMS_Out_Scan
    {
        public WMS_Out_Scan()
        {
            Scan_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Link_Head_ID = Guid.Empty;
            MatSn = string.Empty;
            Scan_Quantity = 0;
            Scan_Source = string.Empty;
            Package_Type = string.Empty;
            LinkMainCID = Guid.Empty;
            Track_No = string.Empty;
            Tray_No = string.Empty;
            Case_No = string.Empty;
        }

        [Key]
        public Guid Scan_ID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Link_Head_ID { get; set; }

        [Required]
        public string MatSn { get; set; }

        [DefaultValue("")]
        public string Track_No { get; set; }

        [DefaultValue("")]
        public string Tray_No { get; set; }

        [DefaultValue("")]
        public string Case_No { get; set; }

        [Required]
        public int Scan_Quantity { get; set; }

        [DefaultValue("")]
        public string Scan_Source { get; set; }
        
        public string Package_Type { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
        
    }

    [NotMapped]
    public class WMS_Out_Filter
    {
        public WMS_Out_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Task_Bat_No = string.Empty;
            LinkMainCID = Guid.Empty;
            LinkHeadID = Guid.Empty;
            MatSn = string.Empty;
            Global_State = string.Empty;
            Line_Status = string.Empty;
            Brand = string.Empty;
            Create_Person = string.Empty;
            Logistics_Company = string.Empty;
            Logistics_Mode = string.Empty;
            Customer_Name = string.Empty;
            Global_State_List = Enum.GetNames(typeof(WMS_Out_Global_State_Enum)).ToList();
            Logistics_Mode_List = Enum.GetNames(typeof(Logistics_Out_Mode_Enum)).ToList();
            Brand_List = new List<string>();
            Stock_Status = string.Empty;
            Stock_Status_List = new List<string>();
            Stock_Status_List.Add(WMS_Out_Global_State_Enum.待配货.ToString());
            Stock_Status_List.Add(WMS_Out_Global_State_Enum.待验货.ToString());
            Return_Info = string.Empty;
            Tracking_No = string.Empty;
            Time_Start = string.Empty;
            Time_End = string.Empty;
            Work_Down_Person = string.Empty;
            Work_Out_Person = string.Empty;
            Location = string.Empty;
            Head_Type = string.Empty;
            Head_Type_List = Enum.GetNames(typeof(WMS_Out_Head_Type_Enum)).ToList();
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Task_Bat_No { get; set; }
        public string Create_Person { get; set; }
        public string Global_State { get; set; }
        public List<string> Global_State_List { get; set; }
        public string Line_Status { get; set; }
        public List<string> Logistics_Mode_List { get; set; }
        public string Logistics_Mode { get; set; }
        public string Logistics_Company { get; set; }
        public string Customer_Name { get; set; }
        public Guid LinkMainCID { get; set; }
        public Guid LinkHeadID { get; set; }
        public string MatSn { get; set; }
        public string Brand { get; set; }
        public string Stock_Status { get; set; }
        public List<string> Stock_Status_List { get; set; }
        public List<string> Brand_List { get; set; }
        public string Return_Info { get; set; }
        public string Tracking_No { get; set; }
        public string Time_Start { get; set; }
        public string Time_End { get; set; }
        public string Work_Down_Person { get; set; }
        public string Work_Out_Person { get; set; }
        public string Location { get; set; }
        public string Head_Type { get; set; }
        public List<string> Head_Type_List { get; set; }
    }

    //送货单
    public enum WMS_Out_Global_State_Enum
    {
        待配货,
        待验货,
        待包装,
        待出库,
        已出库,
    }

    public enum Logistics_Out_Mode_Enum
    {
        自送,
        快递,
        物流,
        自提,
    }

    public enum WMS_Out_Scan_Status_Enum
    {
        已完成,
        未完成,
    }

    public enum WMS_Out_Head_Type_Enum
    {
        订单出货,
        订单退货,
    }

    public enum WMS_Out_Task_Line_State_Enum
    {
        数量一致,
        超出出货,
        低于出货,
        还未扫码,
        多出型号,
    }
}

//移库任务
namespace SMART.Api.Models
{
    public class WMS_Move
    {
        public WMS_Move()
        {
            Move_ID = Guid.Empty;
            Out_Location = string.Empty;
            In_Location = string.Empty;
            Move_Status = string.Empty;
            Create_DT = DateTime.Now;
            LinkMainCID = Guid.Empty;
            Supplier_Name = string.Empty;
            Work_Person = string.Empty;
            Link_HeadID = Guid.Empty;
            Is_Stocktaking = false;
            Task_Bat_No = string.Empty;
            MatSn_Count = 0;
            Quantity_Sum = 0;
        }

        [Key]
        public Guid Move_ID { get; set; }

        [Required]
        public string Out_Location { get; set; }

        public string In_Location { get; set; }

        [Required]
        public string Move_Status { get; set; }

        public DateTime Create_DT { get; set; }
        
        [Required]
        public Guid LinkMainCID { get; set; }

        public string Task_Bat_No { get; set; }

        public Guid Link_HeadID { get; set; }

        public string Work_Person { get; set; }
        
        public string Supplier_Name { get; set; }

        [NotMapped]
        public int MatSn_Count { get; set; }

        [NotMapped]
        public int Quantity_Sum { get; set; }

        [NotMapped]
        public bool Is_Stocktaking { get; set; }
    }

    public class WMS_Move_Scan
    {
        public WMS_Move_Scan()
        {
            Scan_ID = Guid.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            Scan_Quantity = 0;
            Out_Location = string.Empty;
            In_Location = string.Empty;
            Create_DT = DateTime.Now;
            LinkMainCID = Guid.Empty;
            Link_TaskID = Guid.Empty;
            Scan_Source = string.Empty;
            Package_Type = string.Empty;
            Status = string.Empty;
        }

        [Key]
        public Guid Scan_ID { get; set; }

        [Required]
        public string MatSn { get; set; }
        
        public string MatBrand { get; set; }

        [Required]
        public string Package_Type { get; set; }

        [Required]
        public int Scan_Quantity { get; set; }

        [Required]
        public string Status { get; set; }

        [DefaultValue("")]
        public string Scan_Source { get; set; }

        [Required]
        public string Out_Location { get; set; }
        
        public string In_Location { get; set; }

        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Link_TaskID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

    }

    //移库记录
    public class WMS_Move_Record
    {
        public WMS_Move_Record()
        {
            Record_ID = Guid.Empty;
            Out_Location = string.Empty;
            In_Location = string.Empty;
            Create_DT = DateTime.Now;
            LinkMainCID = Guid.Empty;
            Work_Person = string.Empty;
            Link_TaskID = Guid.Empty;
            MatSn = string.Empty;
            Quantity = 0;
            Package_Type = string.Empty;
            MatName = string.Empty;
            MatBrand = string.Empty;
            MatUnit = string.Empty;
            Move_Type = string.Empty;
        }

        [Key]
        public Guid Record_ID { get; set; }

        [Required]
        public string Out_Location { get; set; }

        [Required]
        public string In_Location { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [Required]
        public Guid Link_TaskID { get; set; }

        public string Work_Person { get; set; }

        [Required]
        public string MatSn { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Package_Type { get; set; }

        [DefaultValue("")]
        public string MatName { get; set; }

        [DefaultValue("")]
        public string MatBrand { get; set; }

        public string MatUnit { get; set; }

        [Required]
        public string Move_Type { get; set; }
    }

    [NotMapped]
    public class WMS_Move_Filter
    {
        public WMS_Move_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Location = string.Empty;
            MatSn = string.Empty;
            Work_Person = string.Empty;
            Time_End = string.Empty;
            Time_Start = string.Empty;
            LinkMainCID = Guid.Empty;
            Link_HeadID = Guid.Empty;
            Return_Info = string.Empty;
            Move_Status = string.Empty;
            Move_Status_List = Enum.GetNames(typeof(WMS_Move_Status_Enum)).ToList();
            Move_Type = string.Empty;
            In_Location = string.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Location { get; set; }
        public string In_Location { get; set; }
        public string MatSn { get; set; }
        public string Work_Person { get; set; }
        public string Time_Start { get; set; }
        public string Time_End { get; set; }
        public Guid LinkMainCID { get; set; }
        public Guid Link_HeadID { get; set; }
        public string Return_Info { get; set; }
        public string Move_Status { get; set; }
        public List<string> Move_Status_List { get; set; }
        public string Move_Type { get; set; }
    }

    public class WMS_Stocktaking
    {
        public WMS_Stocktaking()
        {
            Stocktaking_ID = Guid.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            Quantity = 0;
            Location = string.Empty;
            Create_DT = DateTime.Now;
            LinkMainCID = Guid.Empty;
            Link_TaskID = Guid.Empty;
            Task_Bat_No = string.Empty;
            Work_Person = string.Empty;
            Status = string.Empty;
        }

        [Key]
        public Guid Stocktaking_ID { get; set; }

        [Required]
        public string MatSn { get; set; }
        
        public string MatBrand { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }
        
        public string Task_Bat_No { get; set; }
        
        public string Work_Person { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public Guid Link_TaskID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }

    public class WMS_Stocktaking_Scan
    {
        public WMS_Stocktaking_Scan()
        {
            Scan_ID = Guid.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            Scan_Quantity = 0;
            Location = string.Empty;
            Create_DT = DateTime.Now;
            LinkMainCID = Guid.Empty;
            Link_TaskID = Guid.Empty;
            Scan_Source = string.Empty;
            Status = string.Empty;
            Package_Type = string.Empty;
            QRCode_Path = string.Empty;
            Recommend_Info = string.Empty;
            Recommend_Info_List = new List<Recommend_Move_Info>();
        }

        [Key]
        public Guid Scan_ID { get; set; }

        [Required]
        public string MatSn { get; set; }
        
        public string MatBrand { get; set; }

        [Required]
        public int Scan_Quantity { get; set; }

        [DefaultValue("")]
        public string Scan_Source { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Link_TaskID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [Required]
        public string Package_Type { get; set; }

        [NotMapped]
        public string QRCode_Path { get; set; }

        [NotMapped]
        public string Recommend_Info { get; set; }

        [NotMapped]
        public List<Recommend_Move_Info> Recommend_Info_List { get; set; }
    }

    [NotMapped]
    public class Recommend_Move_Info
    {
        public Recommend_Move_Info()
        {
            No = 0;
            Location = string.Empty;
            Quantity = 0;
        }

        public int No { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
    }

    //底盘
    public enum WMS_Stocktaking_Status_Enum
    {
        待底盘,
        已底盘,
    }

    //移库任务
    [NotMapped]
    public class WMS_Move_Task
    {
        public WMS_Move_Task()
        {
            Move_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Out_Location = string.Empty;
            In_Location = string.Empty;
            MatSn_Count = 0;
            Line_Count = 0;
            Line_Quantity_Sum = 0;
            Line_List = new List<WMS_Move_Task_Line>();
            Work_Person = string.Empty;
            Task_Bat_No = string.Empty;
            Supplier_Name = string.Empty;
            Scan_List = new List<WMS_Stocktaking_Scan>();
        }

        public Guid Move_ID { get; set; }
        public DateTime Create_DT { get; set; }
        public string Status { get; set; }
        public string Out_Location { get; set; }
        public string In_Location { get; set; }
        public string Task_Bat_No { get; set; }
        public string Supplier_Name { get; set; }

        //型号数
        public int MatSn_Count { get; set; }

        //总行数
        public int Line_Count { get; set; }

        //产品总数
        public int Line_Quantity_Sum { get; set; }

        //合并产品型号列表
        public List<WMS_Move_Task_Line> Line_List { get; set; }

        //扫描列表
        public List<WMS_Stocktaking_Scan> Scan_List { get; set; }

        //作业人
        public string Work_Person { get; set; }
    }

    [NotMapped]
    public class WMS_Move_Task_Line
    {
        public WMS_Move_Task_Line()
        {
            Line_No = 0;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            Line_Count = 0;
            Quantity_Sum = 0;
            Quantity_Sum_Scan = 0;
            Line_State = string.Empty;
        }

        public int Line_No { get; set; }
        public string MatSn { get; set; }
        public string MatBrand { get; set; }
        public int Line_Count { get; set; }
        public int Quantity_Sum { get; set; }
        public int Quantity_Sum_Scan { get; set; }
        public string Line_State { get; set; }
    }

    public enum WMS_Move_Type_Enum
    {
        移库作业,
        上架作业,
    }

    public enum WMS_Move_Status_Enum
    {
        待移库,
        已移库,
    }

    public enum WMS_Move_Line_State_Enum
    {
        数量一致,
        超出系统,
        低于系统,
        还未扫码,
        多出型号,
    }

}

//出入库记录
namespace SMART.Api.Models
{
    public class WMS_Stock_Record
    {
        public WMS_Stock_Record()
        {
            Stock_ID = Guid.Empty;
            WMS_In_DT = string.Empty;
            WMS_Out_DT = string.Empty;
            MatSn = string.Empty;
            MatName = string.Empty;
            MatBrand = string.Empty;
            MatUnit = string.Empty;
            Quantity = 0;
            Package = string.Empty;
            Location = string.Empty;
            Cases = string.Empty;
            Location_Type = string.Empty;
            Wms_In_Head_ID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            Price = 0;
            Wms_Out_Head_ID = Guid.Empty;
            Customer = string.Empty;
            Supplier = string.Empty;
            Remark = string.Empty;
            Work_Person = string.Empty;
            Create_DT = DateTime.Now;
        }

        [Key]
        public Guid Stock_ID { get; set; }

        //最近入库时间（YYYY-MM-dd HH:mm）
        public string WMS_In_DT { get; set; }

        //最近出库时间（YYYY-MM-dd HH:mm）
        public string WMS_Out_DT { get; set; }

        [Required]
        public string MatSn { get; set; }

        [DefaultValue("")]
        public string MatName { get; set; }

        [DefaultValue("")]
        public string MatBrand { get; set; }
        
        public string MatUnit { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Package { get; set; }

        public decimal Price { get; set; }

        //库位号
        [Required]
        public string Location { get; set; }

        public string Cases { get; set; }
        
        public string Location_Type { get; set; }
        
        public Guid Wms_In_Head_ID { get; set; }

        public Guid Wms_Out_Head_ID { get; set; }

        public string Customer { get; set; }

        public string Supplier { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        public string Remark { get; set; }

        public string Work_Person { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

    }

    public enum WMS_Stock_Record_Remark_Enum
    {
        订单入库,
        订单出库,
        首次盘库,
    }
    
    [NotMapped]
    public class WMS_Stock_Record_Info
    {
        public WMS_Stock_Record_Info()
        {
            MatSn = string.Empty;
            Act_Date = DateTime.Now;
            Act_Status = string.Empty;
            Record_Type = string.Empty;
            Location = string.Empty;
            Quantity = 0;
            Act_Date_Str = string.Empty;
            Out_Quantity = 0;
            Profit_Loss_Quantity = 0;
            In_Quantity = 0;
            First_Quantity = 0;
        }

        public string MatSn { get; set; }
        public DateTime Act_Date { get; set; }
        public string Act_Date_Str { get; set; }
        public string Act_Status { get; set; }
        public string Record_Type { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }

        //出库数量
        public int First_Quantity { get; set; }

        //出库数量
        public int Out_Quantity { get; set; }

        //入库数量
        public int In_Quantity { get; set; }

        //盈亏数量
        public int Profit_Loss_Quantity { get; set; }
    }
}

//快递费用
namespace SMART.Api.Models
{
    public class WMS_Track
    {
        public WMS_Track()
        {
            Tracking_ID = Guid.Empty;
            Logistics_Company = string.Empty;
            Logistics_Mode = string.Empty;
            Logistics_Cost = 0;
            Kilometers = 0;
            Tracking_No = string.Empty;
            Scan_PDA_Date = DateTime.Now;
            LinkMainCID = Guid.Empty;
            Link_Head_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Link_Head_Com_Name = string.Empty;
            Tracking_Type = string.Empty;
            Driver_Name = string.Empty;
            Weight = 0;
            Tray_No = string.Empty;
        }

        [Key]
        public Guid Tracking_ID { get; set; }

        public string Logistics_Company { get; set; }

        [Required]
        public string Logistics_Mode { get; set; }

        [Required]
        public decimal Logistics_Cost { get; set; }

        [Required]
        public string Tracking_No { get; set; }

        public decimal Kilometers { get; set; }

        public decimal Weight { get; set; }

        [Required]
        public DateTime Scan_PDA_Date { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [Required]
        public Guid Link_Head_ID { get; set; }

        public string Link_Head_Com_Name { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string Tracking_Type { get; set; }

        public string Driver_Name { get; set; }

        public string Tray_No { get; set; }
    }

    public enum Tracking_Type_Enum
    {
        收货,
        送货,
        发票,
        其他,
    }

    [NotMapped]
    public class Logistics_Cost_Year
    {
        public Logistics_Cost_Year()
        {
            YearList = new List<string>();
            SubList = new List<Logistics_Cost_Year_Sub>();
            SubCustList = new List<Logistics_Cost_Year_Sub_By_Logistics>();
        }
        public string Year { get; set; }
        public List<string> YearList { get; set; }
        public Guid LinkMainCID { get; set; }
        public List<Logistics_Cost_Year_Sub> SubList { get; set; }
        public List<Logistics_Cost_Year_Sub_By_Logistics> SubCustList { get; set; }
    }

    [NotMapped]
    public class Logistics_Cost_Year_Sub
    {
        public DateTime SD { get; set; }
        public DateTime ED { get; set; }

        public string MonthStr { get; set; }

        //金额合计
        public decimal Price_Amount { get; set; }

        //快递单合计
        public decimal Tracking_No_Amount { get; set; }
    }

    [NotMapped]
    public class Logistics_Cost_Year_Sub_By_Logistics
    {
        public Logistics_Cost_Year_Sub_By_Logistics()
        {
            SubList = new List<Logistics_Cost_Year_Sub_By_Logistics_Sub>();
        }

        public Guid Logistics_ID { get; set; }
        public string Logistics_Name { get; set; }

        //金额合计
        public decimal Price_Amount { get; set; }

        //快递单合计
        public decimal Tracking_No_Amount { get; set; }

        public List<Logistics_Cost_Year_Sub_By_Logistics_Sub> SubList { get; set; }
    }

    [NotMapped]
    public class Logistics_Cost_Year_Sub_By_Logistics_Sub
    {
        public Guid Logistics_ID { get; set; }

        public string MonthStr { get; set; }

        public string MonthStr_SD { get; set; }

        public string MonthStr_ED { get; set; }

        //金额合计
        public decimal Price_Amount { get; set; }

        //快递单合计
        public decimal Tracking_No_Amount { get; set; }

    }

    [NotMapped]
    public class Logistics_Cost_Filter
    {
        public Logistics_Cost_Filter()
        {
            PageIndex = 1;
            PageSize = 20;
            Keyword = string.Empty;
            Com_Name = string.Empty;
            Logistics_Name = string.Empty;
            Year = string.Empty;
            TrackingType = string.Empty;
            SD = string.Empty;
            ED = string.Empty;

        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Com_Name { get; set; }
        public string Logistics_Name { get; set; }
        public string Year { get; set; }
        public string Keyword { get; set; }
        public string TrackingType { get; set; }
        public string SD { get; set; }
        public string ED { get; set; }
        public Guid LinkMainCID { get; set; }
    }

    public class WMS_Track_Info
    {
        public WMS_Track_Info()
        {
            Info_ID = Guid.Empty;
            Sender_Name = string.Empty;
            Sender_Phone = string.Empty;
            Sender_Tel = string.Empty;
            Sender_Address = string.Empty;
            Receiver_Name = string.Empty;
            Receiver_Phone = string.Empty;
            Receiver_Tel = string.Empty;
            Receiver_Address = string.Empty;
            Item_Info = string.Empty;
            Logistics_Company = string.Empty;
            Logistics_Company_Loc = string.Empty;
            Tracking_No = string.Empty;
            LinkMainCID = Guid.Empty;
            Create_DT = DateTime.Now;
            Create_Person = string.Empty;
        }

        [Key]
        public Guid Info_ID { get; set; }

        [Required]
        public string Sender_Name { get; set; }

        public string Sender_Phone { get; set; }

        public string Sender_Tel { get; set; }

        [Required]
        public string Sender_Address { get; set; }

        [Required]
        public string Receiver_Name { get; set; }

        public string Receiver_Phone { get; set; }

        public string Receiver_Tel { get; set; }

        [Required]
        public string Receiver_Address { get; set; }

        [Required]
        public string Item_Info { get; set; }

        [Required]
        public string Logistics_Company { get; set; }
        
        public string Logistics_Company_Loc { get; set; }
        
        [Required]
        public string Tracking_No { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string Create_Person { get; set; }
    }

    [NotMapped]
    public class Track_Info_Filter
    {
        public Track_Info_Filter()
        {
            PageIndex = 1;
            PageSize = 20;
            Tracking_No = string.Empty;
            Logistics_Company = string.Empty;
            Sender_Name = string.Empty;
            Receiver_Name = string.Empty;
            LinkMainCID = Guid.Empty;
            Time_Start = string.Empty;
            Time_End = string.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Sender_Name { get; set; }
        public string Logistics_Company { get; set; }
        public string Receiver_Name { get; set; }
        public string Tracking_No { get; set; }
        public string Time_Start { get; set; }
        public string Time_End { get; set; }
        public Guid LinkMainCID { get; set; }
    }
}

//报废记录
namespace SMART.Api.Models
{
    public class WMS_Waste_Record
    {
        public WMS_Waste_Record()
        {
            Record_ID = Guid.Empty;
            Task_No_Str = string.Empty;
            Create_DT = DateTime.Now;
            Create_Person = string.Empty;
            MatName = string.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            MatUnit = string.Empty;
            Quantity = 0;
            Price_Cost = 0;
            LinkMainCID = Guid.Empty;
            Customer_Name = string.Empty;
            Supplier_Name = string.Empty;
            Link_Head_ID = Guid.Empty;
            Auditor_A = string.Empty;
            Audit_DT_A = DateTime.Now;
            Auditor_B = string.Empty;
            Audit_DT_B = DateTime.Now;
            Status = string.Empty;
        }

        [Key]
        public Guid Record_ID { get; set; }

        [DefaultValue("")]
        public string Task_No_Str { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string Create_Person { get; set; }

        [DefaultValue("")]
        public string MatName { get; set; }

        [Required]
        public string MatSn { get; set; }

        [DefaultValue("")]
        public string MatBrand { get; set; }

        [DefaultValue("")]
        public string MatUnit { get; set; }

        //到货数量Or调货数量
        [Required]
        public int Quantity { get; set; }

        //单价
        [Required]
        public decimal Price_Cost { get; set; }

        [DefaultValue("")]
        public string Customer_Name { get; set; }

        [DefaultValue("")]
        public string Supplier_Name { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    
        public Guid Link_Head_ID { get; set; }

        public string Auditor_A { get; set; }

        public DateTime Audit_DT_A { get; set; }

        public string Auditor_B { get; set; }

        public DateTime Audit_DT_B { get; set; }

        [Required]
        public string Status { get; set; }
    }

    [NotMapped]
    public class WMS_Waste_Record_Filter
    {
        public WMS_Waste_Record_Filter()
        {
            PageIndex = 1;
            PageSize = 20;
            Keyword = string.Empty;
            Time_Start = string.Empty;
            Time_End = string.Empty;
            MatSn = string.Empty;
            Task_No_Str = string.Empty;
            Status = string.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string MatSn { get; set; }
        public string Task_No_Str { get; set; }
        public string Time_Start { get; set; }
        public string Time_End { get; set; }
        public string Status { get; set; }
        public Guid LinkMainCID { get; set; }
    }

    public enum WMS_Waste_Record_Status_Enum
    {
        未审核,
        审核中,
        已审核,
    }
}
