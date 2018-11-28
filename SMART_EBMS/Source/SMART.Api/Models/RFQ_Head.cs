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
    public class RFQ_Head
    {
        [Key]
        public Guid RID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string RFQ_No { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public Guid CID { get; set; }

        [Required]
        [DefaultValue("")]
        public string Cust_Name { get; set; }

        //结算方式
        [DefaultValue("")]
        public string C_Settle_Accounts { get; set; }

        [NotMapped]
        public List<string> C_Settle_Accounts_List { get; set; }

        //所属行业
        [DefaultValue("")]
        public string C_Industry { get; set; }
        
        [NotMapped]
        public List<string> C_Industry_List { get; set; }

        //客户来源
        [DefaultValue("")]
        public string C_FormBy { get; set; }

        //客户类型
        [DefaultValue("")]
        public string C_VIP_Type { get; set; }

        [NotMapped]
        public List<string> C_FormBy_List { get; set; }

        //竞争对手
        [DefaultValue("")]
        public string Compete_Company { get; set; }

        //竞争品牌
        [DefaultValue("")]
        public string Compete_Brand { get; set; }

        [DefaultValue("")]
        public string Buyer { get; set; }

        [DefaultValue("")]
        public string Buyer_Tel { get; set; }

        [DefaultValue("")]
        public string Buyer_Fax { get; set; }

        [DefaultValue("")]
        public string Buyer_Mail { get; set; }

        [Required]
        public Guid Ass_UID { get; set; }

        //客服助理
        [DefaultValue("")]
        public string Ass_Full_Name { get; set; }

        [Required]
        public Guid UID { get; set; }

        [DefaultValue("")]
        public string UID_FullName { get; set; }

        [DefaultValue("")]
        public string UID_Tel { get; set; }

        [DefaultValue("")]
        public string UID_Email { get; set; }

        [DefaultValue("")]
        public string UID_Fax { get; set; }

        [DefaultValue("")]
        public string RFQ_Remark { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        [DefaultValue("")]
        public string Cust_Code { get; set; }

        [NotMapped]
        public List<RFQ_Head_Line> Line_List { get; set; }

        public RFQ_Head()
        {
            RID = Guid.Empty;
            CID = Guid.Empty;
            Create_DT = DateTime.Now;
            RFQ_No = string.Empty;
            Cust_Name = string.Empty;
            Cust_Code = string.Empty;
            C_Settle_Accounts = string.Empty;
            Buyer = string.Empty;
            Buyer_Tel = string.Empty;
            Buyer_Fax = string.Empty;
            Buyer_Mail = string.Empty;
            UID = Guid.Empty;
            UID_FullName = string.Empty;
            UID_Tel = string.Empty;
            UID_Email = string.Empty;
            UID_Fax = string.Empty;
            RFQ_Remark = string.Empty;
            LinkMainCID = Guid.Empty;
            Line_List = new List<RFQ_Head_Line>();
            C_Settle_Accounts_List = new Customer().Settle_Accounts_List;
            C_Industry_List = new Customer().Industry_List;
            C_FormBy_List = new Customer().FormBy_List;
        }
    }

    public class RFQ_Head_Line
    {
        [Key]
        public Guid Line_ID { get; set; }

        [Required]
        public Guid Link_RID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Line_Number { get; set; }

        [DefaultValue("")]
        public string Cust_Mat_Sn { get; set; }

        [DefaultValue("")]
        public string Cust_Mat_Describe { get; set; }

        [Required]
        public string Cust_Mat_Unit { get; set; }

        [DefaultValue("")]
        public string Cust_Mat_Sn_INFO_STR { get; set; }

        [Required]
        public Guid CID { get; set; }

        [DefaultValue("")]
        public string Mat_Sn { get; set; }

        [Required]
        public Guid MatID { get; set; }

        [Required]
        public Guid BID { get; set; }

        [Required]
        public string BID_Name { get; set; }

        [Required]
        public int Lead_Time { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Sales_Price { get; set; }

        //紧急程度
        [DefaultValue("")]
        public string Urgency { get; set; }

        //行备注
        [DefaultValue("")]
        public string Line_Remark { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public decimal Sub_Amount { get; set; }

        [NotMapped]
        public Material Mat_Info { get; set; }

        [NotMapped]
        public List<RFQ_Head_Line_To_Buyer> For_Buyer_List { get; set; }

        [NotMapped]
        public string His_RFQ_Price { get; set; }

        [NotMapped]
        public string Head_RFQ_No { get; set; }

        [NotMapped]
        public string Head_Cust_Name { get; set; }

        [NotMapped]
        public string Head_Buyer { get; set; }

        [NotMapped]
        public string Head_UID_FullName { get; set; }

        [NotMapped]
        public string Buyer_Status { get; set; }

        public RFQ_Head_Line()
        {
            Mat_Sn = string.Empty;
            Cust_Mat_Sn = string.Empty;
            Cust_Mat_Describe = string.Empty;
            Cust_Mat_Unit = string.Empty;
            Line_ID = Guid.Empty;
            Link_RID = Guid.Empty;
            Create_DT = DateTime.Now;
            MatID = Guid.Empty;
            BID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            Lead_Time = 0;
            Qty = 0;
            Sales_Price = 0;
            Sub_Amount = 0;
            Cust_Mat_Sn_INFO_STR = string.Empty;
            Urgency = string.Empty;
            Line_Remark = string.Empty;
            Line_Number = 0;
            Mat_Info = new Material();
            For_Buyer_List = new List<RFQ_Head_Line_To_Buyer>();
            His_RFQ_Price = string.Empty;
            Head_RFQ_No = string.Empty;
            Head_Cust_Name = string.Empty;
            Head_Buyer = string.Empty;
            Head_UID_FullName = string.Empty;
            Buyer_Status = string.Empty;
        }
    }
    
    public class RFQ_Head_Line_To_Buyer
    {
        public RFQ_Head_Line_To_Buyer()
        {
            RFQ_Line_For_BID = Guid.Empty;
            RFQ_Line_ID = Guid.Empty;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid RFQ_Line_For_BID { get; set; }

        [DefaultValue("")]
        public string Sup_Name { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Cost_Price { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Qty { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Lead_Time { get; set; }

        [Required]
        public Guid RFQ_Line_ID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

    }

    [NotMapped]
    public class RFQ_Head_Filter
    {
        public RFQ_Head_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Cust_Name = string.Empty;
            RFQ_No = string.Empty;
            Status_List = new List<string>();
            LinkMainCID = Guid.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Cust_Name { get; set; }
        public string RFQ_No { get; set; }
        public List<string> Status_List { get; set; }
        public Guid LinkMainCID { get; set; }
    }

    public enum RFQ_Status_Enum
    {
        待发送,
        进行中,
        待审核,
        被退回,
        已审核,
    }

    public enum Mat_Sn_INFO_Enum
    {
        油脂不限,
        游隙不限,
        产地不限,
    }

    public enum RFQ_Line_Buyer_Status
    {
        发送,
        待受理,
        已答复,
    }
}
