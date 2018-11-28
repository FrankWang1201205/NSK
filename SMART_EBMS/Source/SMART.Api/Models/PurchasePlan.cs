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
    [NotMapped]
    public class PurchasePlan
    {
        public PurchasePlan()
        {
            MainCID = Guid.Empty;
            Month = string.Empty;
            Next_Mon_A = string.Empty;
            Next_Mon_B = string.Empty;
            Next_Mon_C = string.Empty;
            Total_Mat_Count = 0;
            PurchasePlan_Line_PageList = new PageList<PurchasePlan_Line>();
        }

        [NotMapped]
        public Guid MainCID { get; set; }

        [DefaultValue("")]
        public string Month { get; set; }

        [NotMapped]
        public string Next_Mon_A = string.Empty;

        [NotMapped]
        public string Next_Mon_B = string.Empty;

        [NotMapped]
        public string Next_Mon_C = string.Empty;

        [NotMapped]
        public int Total_Mat_Count { get; set; }

        [NotMapped]
        public PageList<PurchasePlan_Line> PurchasePlan_Line_PageList { get; set; }

    }

    [NotMapped]
    public class PurchasePlan_Group
    {
        public PurchasePlan_Group()
        {
            Month = string.Empty;
            MainCID = Guid.Empty;
            Mat_Count = 0;
            Mat_Count_Is_Sent = 0;
            Mat_Count_Is_Not_Sent = 0;
            SalesPlanList = new List<SalesPlan>();
        }
        public Guid MainCID { get; set; }
        public string Month { get; set; }
        public int Mat_Count { get; set; }
        public int Mat_Count_Is_Sent { get; set; }
        public int Mat_Count_Is_Not_Sent { get; set; }
        public List<SalesPlan> SalesPlanList { get; set; }
    }

    public class PurchasePlan_Line
    {
        public PurchasePlan_Line()
        {
            PPID = Guid.Empty;
            MatID = Guid.Empty;
            MatInfo = new Material();
            Wms_Qty = 0;

            Month = string.Empty;
            Next_Mon_A = string.Empty;
            Next_Mon_B = string.Empty;
            Next_Mon_C = string.Empty;

            PoPlan_Next_Mon_Qty_A = 0;
            PoPlan_Next_Mon_Qty_B = 0;
            PoPlan_Next_Mon_Qty_C = 0;

            PoPlan_Next_Mon_Delivery_A = string.Empty;
            PoPlan_Next_Mon_Delivery_B = string.Empty;
            PoPlan_Next_Mon_Delivery_C = string.Empty;

            Next_Mon_Qty_A = 0;
            Next_Mon_Qty_B = 0;
            Next_Mon_Qty_C = 0;
            Wms_Qty_A = 0;
            Wms_Qty_B = 0;
            Wms_Qty_C = 0;
            PoLine_Qty_A = 0;
            PoLine_Qty_B = 0;
            PoLine_Qty_C = 0;

            T_Next_Mon_Qty_A = string.Empty;
            T_Next_Mon_Qty_B = string.Empty;
            T_Next_Mon_Qty_C = string.Empty;

            Is_Sent_Buyer = 0;
        
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid PPID { get; set; }

        [DefaultValue("")]
        public string Month { get; set; }

        [Required]
        public Guid MatID { get; set; }

        //下月预测
        [Required]
        [DefaultValue(0)]
        public int PoPlan_Next_Mon_Qty_A { get; set; }

        //下下月预测
        [Required]
        [DefaultValue(0)]
        public int PoPlan_Next_Mon_Qty_B { get; set; }

        //下下下月预测
        [Required]
        [DefaultValue(0)]
        public int PoPlan_Next_Mon_Qty_C { get; set; }

        [Required]
        public int Is_Sent_Buyer { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public Material MatInfo { get; set; }

        //可用库存
        [NotMapped]
        public int Wms_Qty { get; set; }

        //月份集合
        [NotMapped]
        public string Next_Mon_A { get; set; }
        [NotMapped]
        public string Next_Mon_B { get; set; }
        [NotMapped]
        public string Next_Mon_C { get; set; }

        //下月预测
        [NotMapped]
        public int Next_Mon_Qty_A { get; set; }
        //下下月预测
        [NotMapped]
        public int Next_Mon_Qty_B { get; set; }
        //下下下月预测
        [NotMapped]
        public int Next_Mon_Qty_C { get; set; }

        //在库可用
        [NotMapped]
        public int Wms_Qty_A { get; set; }
        [NotMapped]
        public int Wms_Qty_B { get; set; }
        [NotMapped]
        public int Wms_Qty_C { get; set; }

        //在途可用
        [NotMapped]
        public int PoLine_Qty_A { get; set; }
        [NotMapped]
        public int PoLine_Qty_B { get; set; }
        [NotMapped]
        public int PoLine_Qty_C { get; set; }

        //建议计划数
        [NotMapped]
        public int Auto_PoPlan_Next_Mon_Qty_A { get; set; }
        [NotMapped]
        public int Auto_PoPlan_Next_Mon_Qty_B { get; set; }
        [NotMapped]
        public int Auto_PoPlan_Next_Mon_Qty_C { get; set; }

        //下月预测
        [NotMapped]
        public string T_Next_Mon_Qty_A { get; set; }

        //下下月预测
        [NotMapped]
        public string T_Next_Mon_Qty_B { get; set; }

        //下下下月预测
        [NotMapped]
        public string T_Next_Mon_Qty_C { get; set; }

        //建议采购日期
        [NotMapped]
        public string PoPlan_Next_Mon_Delivery_A { get; set; }

        [NotMapped]
        public string PoPlan_Next_Mon_Delivery_B { get; set; }

        [NotMapped]
        public string PoPlan_Next_Mon_Delivery_C { get; set; }
    }

    [NotMapped]
    public class PurchasePlan_Filter
    {
        public PurchasePlan_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Keyword = string.Empty;
            MatBrand = string.Empty;
            Month = string.Empty;
            Po_Head_Status = string.Empty;
            Sales_UID = Guid.Empty;
            MatID = Guid.Empty;
            BrandList = new List<string>();
            Line_Status_List = new List<string>();
            Line_Status_List.Add(PurchasePlan_Line_Status.待下达计划.ToString());
            Line_Status_List.Add(PurchasePlan_Line_Status.已下达计划.ToString());
            LinkMainCID = Guid.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string MatBrand { get; set; }
        public string Month { get; set; }
        public string Line_Status { get; set; }
        public string Po_Head_Status { get; set; }
        public Guid Sales_UID { get; set; }
        public Guid MatID { get; set; }
        public List<string> BrandList { get; set; }
        public List<string> Line_Status_List { get; set; }
        public Guid LinkMainCID { get; set; }
    }

    public enum PurchasePlan_Line_Status
    {
        已下达计划,
        待下达计划,
    }
}
