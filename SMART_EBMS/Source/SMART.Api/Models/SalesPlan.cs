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
    public class SalesPlan
    {
        public SalesPlan()
        {
            SPID = Guid.Empty;
            Creata_DT = DateTime.Now;
            End_Line_DT = DateTime.Now;
            Sys_No = string.Empty;
            Status = string.Empty;
            Sales_UID = Guid.Empty;
            Sales_Person = string.Empty;
            Aud_Person = string.Empty;
            Month = string.Empty;
            End_DT_Int = 0;
            End_DT_Unit = string.Empty;
            LinkMainCID = Guid.Empty;

            This_Mon = string.Empty;
            Next_Mon_A = string.Empty;
            Next_Mon_B = string.Empty;
            Next_Mon_C = string.Empty;

            This_Mon_Float = 10;
            Next_Mon_Float_A = 25;
            Next_Mon_Float_B = 50;
            Next_Mon_Float_C = 1000;

            Line_Count = 0;
        }

        [Key]
        public Guid SPID { get; set; }

        [Required]
        public DateTime Creata_DT { get; set; }

        [Required]
        public string Month { get; set; }

        [Required]
        public DateTime End_Line_DT { get; set; }

        [Required]
        public string Sys_No { get; set; }

        [DefaultValue("")]
        public string Status { get; set; }

        [Required]
        public Guid Sales_UID { get; set; }

        [Required]
        public string Sales_Person { get; set; }

        [DefaultValue("")]
        public string Aud_Person { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public int End_DT_Int { get; set; }

        [NotMapped]
        public string End_DT_Unit { get; set; }

        [NotMapped]
        public string This_Mon { get; set; }

        [NotMapped]
        public int This_Mon_Float { get; set; }

        [NotMapped]
        public string Next_Mon_A { get; set; }

        [NotMapped]
        public int Next_Mon_Float_A { get; set; }

        [NotMapped]
        public string Next_Mon_B { get; set; }

        [NotMapped]
        public int Next_Mon_Float_B { get; set; }

        [NotMapped]
        public string Next_Mon_C { get; set; }

        [NotMapped]
        public int Next_Mon_Float_C { get; set; }

        [NotMapped]
        public PageList<SalesPlan_Line> SalesPlan_Line_PageList { get; set; }

        [NotMapped]
        public int Line_Count { get; set; }

    }

    public class SalesPlan_Line
    {
        public SalesPlan_Line()
        {
            SPLineID = Guid.Empty;
            MatID = Guid.Empty;
            Month_Sales_Average = 0;
            Forecast_Rate = 0;
            Last_Mon_Qty = 0;
            Last_Mon_Float = 0;
            This_Mon_Qty = 0;
            This_Mon_Float = 0;
            Next_Mon_Qty_A = 0;
            Next_Mon_Qty_B = 0;
            Next_Mon_Qty_C = 0;
            Next_Mon_Float_A = 0;
            Next_Mon_Float_B = 0;
            Next_Mon_Float_C = 0;

            Next_Mon_CSS_A = string.Empty;
            Next_Mon_CSS_B = string.Empty;
            Next_Mon_CSS_C = string.Empty;

            IsDeclare = 0;
            POLID = Guid.Empty;
            Link_POID = Guid.Empty;
            Po_No = string.Empty;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid SPLineID { get; set; }

        [Required]
        public Guid Link_SPID { get; set; }

        [Required]
        public Guid MatID { get; set; }

        //月均销售额
        [NotMapped]
        public int Month_Sales_Average { get; set; }

        //预测精度
        [NotMapped]
        public int Forecast_Rate { get; set; }

        //上月预测
        [NotMapped]
        public int Last_Mon_Qty { get; set; }

        //上月预测-浮动
        [NotMapped]
        public int Last_Mon_Float { get; set; }

        //本月预测
        [NotMapped]
        public int This_Mon_Qty { get; set; }

        //本月预测-浮动
        [NotMapped]
        public int This_Mon_Float { get; set; }

        //下月预测
        [DefaultValue(0)]
        public int Next_Mon_Qty_A { get; set; }

        //下月预测-浮动
        [NotMapped]
        public int Next_Mon_Float_A { get; set; }

        [NotMapped]
        public string Next_Mon_CSS_A { get; set; }

        //下下月预测
        [DefaultValue(0)]
        public int Next_Mon_Qty_B { get; set; }

        //下下月预测-浮动
        [NotMapped]
        public int Next_Mon_Float_B { get; set; }

        [NotMapped]
        public string Next_Mon_CSS_B { get; set; }

        //下下下月预测
        [DefaultValue(0)]
        public int Next_Mon_Qty_C { get; set; }

        //下下下月预测-浮动
        [NotMapped]
        public int Next_Mon_Float_C { get; set; }

        [NotMapped]
        public string Next_Mon_CSS_C { get; set; }

        //是否申报
        [DefaultValue(0)]
        public int IsDeclare { get; set; }

        //关联采购行
        [Required]
        public Guid POLID { get; set; }

        [NotMapped]
        public Guid Link_POID { get; set; }

        [NotMapped]
        public string Po_No { get; set; }

        [NotMapped]
        public Material MatInfo { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

    }

    public class SalesPlan_Filter
    {
        public SalesPlan_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Keyword = string.Empty;
            MatBrand = string.Empty;
            IsDeclare = string.Empty;
            Month = string.Empty;
            Status = string.Empty;
            SPID = Guid.Empty;
            Sales_UID = Guid.Empty;
            BrandList = new List<string>();
            IsDeclareList = new List<string>();
            IsDeclareList.Add(IsDeclare_Enum.未申报.ToString());
            IsDeclareList.Add(IsDeclare_Enum.已申报.ToString());
            SalesPersonList = new List<User>();
            LinkMainCID = Guid.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string MatBrand { get; set; }
        public string IsDeclare { get; set; }
        public string Month { get; set; }
        public string Status { get; set; }
        public Guid SPID { get; set; }
        public Guid Sales_UID { get; set; }
        public List<string> BrandList { get; set; }
        public List<string> IsDeclareList { get; set; }
        public List<User> SalesPersonList { get; set; }
        public Guid LinkMainCID { get; set; }
    }

    public enum IsDeclare_Enum
    {
        已申报,
        未申报,
    }

    public enum SalesPlan_Status
    {
        待评审,
        已核准,
    }

}
