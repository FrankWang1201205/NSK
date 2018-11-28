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
    public class Po_Head
    {
        public Po_Head()
        {
            POID = Guid.Empty;
            Po_No = string.Empty;
            Create_DT = DateTime.Now;
            SupID = Guid.Empty;
            Sup_Name = string.Empty;
            Sup_Code = string.Empty;
            Sup_Address = string.Empty;
            Sup_Person = string.Empty;
            Sup_Tel = string.Empty;
            Sup_Phone = string.Empty;
            Sup_Email = string.Empty;
            Status = string.Empty;
            Po_No = string.Empty;
            Payment_Type = string.Empty;
            Logistics_Mode = string.Empty;
            Buyer = string.Empty;
            Buyer_Tel = string.Empty;
            Buyer_Phone = string.Empty;
            Buyer_Email = string.Empty;
            Buyer_UID = Guid.Empty;
            Month = string.Empty;
            Ship_To_Address = string.Empty;
            Main_ComName = string.Empty;
            Main_Address = string.Empty;
            Main_MainInvoiceTel = string.Empty;
            Main_InvoiceAddress = string.Empty;
            Main_TaxpayerIdentificationNo = string.Empty;
            Main_MainBankInfo = string.Empty;
            Main_MainBankAccount = string.Empty;
            Main_ElectronicStamp = string.Empty;
            Main_ComLogo = string.Empty;
            LinkMainCID = Guid.Empty;
            Total_Amount = 0;
            Po_Line_List = new List<Po_Line>();
            Po_Line_List_Count = 0;
        }

        [Key]
        public Guid POID { get; set; }

        [Required]
        public string Po_No { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid SupID { get; set; }

        [Required]
        public string Sup_Name { get; set; }

        [DefaultValue("")]
        public string Sup_Code { get; set; }

        [DefaultValue("")]
        public string Sup_Address { get; set; }

        [DefaultValue("")]
        public string Sup_Person { get; set; }

        [DefaultValue("")]
        public string Sup_Tel { get; set; }

        [DefaultValue("")]
        public string Sup_Phone { get; set; }

        [DefaultValue("")]
        public string Sup_Email { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Payment_Type { get; set; }


        [Required]
        public string Logistics_Mode { get; set; }

        [Required]
        public string Buyer { get; set; }

        [Required]
        public string Buyer_Tel { get; set; }

        [Required]
        public string Buyer_Phone { get; set; }

        [Required]
        public string Buyer_Email { get; set; }

        [Required]
        public Guid Buyer_UID { get; set; }

        [Required]
        public string Month { get; set; }

        [Required]
        public string Ship_To_Address { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public string Main_ComName { get; set; }

        [NotMapped]
        public string Main_Address { get; set; }

        [NotMapped]
        public string Main_MainInvoiceTel { get; set; }

        [NotMapped]
        public string Main_InvoiceAddress { get; set; }

        [NotMapped]
        public string Main_TaxpayerIdentificationNo { get; set; }

        [NotMapped]
        public string Main_MainBankInfo { get; set; }

        [NotMapped]
        public string Main_MainBankAccount { get; set; }

        [NotMapped]
        public string Main_ElectronicStamp { get; set; }

        [NotMapped]
        public string Main_ComLogo { get; set; }

        [NotMapped]
        public List<Po_Line> Po_Line_List { get; set; }

        [NotMapped]
        public int Po_Line_List_Count { get; set; }

        [NotMapped]
        public decimal Total_Amount { get; set; }

    }

    public class Po_Line
    {
        public Po_Line()
        {
            Line_Number = 0;
            POLID = Guid.Empty;
            Create_DT = DateTime.Now;
            Link_POID = Guid.Empty;
            MatID = Guid.Empty;
            Delivery_DT = DateTime.Now;
            LinkMainCID = Guid.Empty;
            CostPrice = 0;
            Qty = 0;
            Month = string.Empty;
            Po_No = string.Empty;
            Po_Status = string.Empty;
            MatInfo = new Material();
            Link_SupID = Guid.Empty;
            Link_Sup_Name = string.Empty;
        }


        [Key]
        public Guid POLID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public int Line_Number { get; set; }

        [Required]
        public Guid Link_POID { get; set; }

        [Required]
        public Guid MatID { get; set; }

        [Required]
        public DateTime Delivery_DT { get; set; }

        [Required]
        public decimal CostPrice { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required]
        public string Month { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public Material MatInfo { get; set; }

        [NotMapped]
        public string Po_No { get; set; }

        [NotMapped]
        public string Po_Status { get; set; }

        [NotMapped]
        public Guid Link_SupID { get; set; }

        [NotMapped]
        public string Link_Sup_Name { get; set; }

    }

    [NotMapped]
    public class Po_Head_Filter
    {
        public Po_Head_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Keyword = string.Empty;
            Status = string.Empty;
            Sup_Name = string.Empty;
            LinkMainCID = Guid.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string Sup_Name { get; set; }
        public string Status { get; set; }
        public Guid LinkMainCID { get; set; }
    }


    [NotMapped]
    public class Po_Head_Plan
    {
        public Po_Head_Plan()
        {
            Month = string.Empty;
            Next_Mon_A = string.Empty;
            Next_Mon_B = string.Empty;
            Next_Mon_C = string.Empty;
            Brand_List = new List<string>();
            Sup_List = new List<Supplier>();
            Plan_Line_PageList = new PageList<PurchasePlan_Line>();
        }

        public string Month { get; set; }
        public string Next_Mon_A { get; set; }
        public string Next_Mon_B { get; set; }
        public string Next_Mon_C { get; set; }
        public List<string> Brand_List { get; set; }
        public List<Supplier> Sup_List { get; set; }
        PageList<PurchasePlan_Line> Plan_Line_PageList { get; set; }
    }

    public enum Po_Status_Enum
    {
        待采购员下单,
        待发送供应商,
        待供应商确认,
        供应商已确认,
        待付前金,
        等待提货,
        待付后金,
    }



}


namespace SMART.Api.Models
{
    [NotMapped]
    public class Po_Wait_Lading_Group
    {
        public Po_Wait_Lading_Group()
        {
            SupID = Guid.Empty;
            Sup_Name = string.Empty;
            SupplierCode = string.Empty;
            Po_Head_Count = 0;
            Po_Line_Count = 0;
            Min_Delivery = DateTime.Now;
        }

        public Guid SupID { get; set; }

        public string Sup_Name { get; set; }

        public string SupplierCode { get; set; }

        public int Po_Head_Count { get; set; }

        public int Po_Line_Count { get; set; }

        public DateTime Min_Delivery { get; set; }
    }
}