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
    public partial class Supplier
    {
        [Key]
        public Guid SupID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string SupplierCode { get; set; }

        [DefaultValue("")]
        public string Create_Person { get; set; }

        [Required]
        public string Sup_Name { get; set; }

        [Required]
        public string Sup_Short_Name { get; set; }

        [Required]
        [DefaultValue("")]
        public string Sup_Level { get; set; }

        [Required]
        [DefaultValue("")]
        public string District { get; set; }

        [Required]
        [DefaultValue("")]
        public string Qualification { get; set; }

        [Required]
        [DefaultValue("")]
        public string Type { get; set; }

        [Required]
        public string Act_Status { get; set; }

        [DefaultValue("")]
        public string Act_Status_Reason { get; set; }

        [Required]
        [DefaultValue("")]
        public string Tax_Rate { get; set; }

        //结算方式
        [Required]
        [DefaultValue("")]
        public string Settle_Accounts_Json { get; set; }

        [NotMapped]
        public List<string> Settle_Accounts_List_Json { get; set; }

        //付款方式
        [Required]
        [DefaultValue("")]
        public string Payment_Json { get; set; }

        [NotMapped]
        public List<string> Payment_List_Json { get; set; }

        //主营业务
        [DefaultValue("")]
        public string Main_Business_Json { get; set; }

        [NotMapped]
        public List<string> Main_Business_List_Json { get; set; }

        //主营品牌
        [DefaultValue("")]
        public string Main_Brand_Json { get; set; }

        [NotMapped]
        public List<string> Main_Brand_List_Json { get; set; }

        [DefaultValue("")]
        public string Address { get; set; }

        [DefaultValue("")]
        public string Invoice_Title { get; set; }

        [DefaultValue("")]
        public string Invoice_Identification { get; set; }

        [DefaultValue("")]
        public string Invoice_Address { get; set; }

        [DefaultValue("")]
        public string Invoice_Tel { get; set; }

        [DefaultValue("")]
        public string Bank_Name { get; set; }

        [DefaultValue("")]
        public string Bank_Account { get; set; }

        [DefaultValue("")]
        public string Remark { get; set; }

        //联系人
        [DefaultValue("")]
        public string Sales_Man { get; set; }

        [DefaultValue("")]
        public string Sales_Man_Tel { get; set; }

        [DefaultValue("")]
        public string Sales_Man_Fax { get; set; }

        [DefaultValue("")]
        public string Sales_Man_Mail { get; set; }

        //联系人
        [DefaultValue("")]
        public string Sales_Manager { get; set; }

        [DefaultValue("")]
        public string Sales_Manager_Tel { get; set; }

        [DefaultValue("")]
        public string Sales_Manager_Fax { get; set; }

        [DefaultValue("")]
        public string Sales_Manager_Mail { get; set; }

        //联系人
        [DefaultValue("")]
        public string Finance { get; set; }

        [DefaultValue("")]
        public string Finance_Tel { get; set; }

        [DefaultValue("")]
        public string Finance_Fax { get; set; }

        [DefaultValue("")]
        public string Finance_Mail { get; set; }

        //联系人
        [DefaultValue("")]
        public string GM { get; set; }

        [DefaultValue("")]
        public string GM_Tel { get; set; }

        [DefaultValue("")]
        public string GM_Fax { get; set; }

        [DefaultValue("")]
        public string GM_Mail { get; set; }


        [Required]
        public Guid LinkMainCID { get; set; }

    }

    public partial class Supplier
    {
        public Supplier()
        {
            SupID = Guid.Empty;
            Create_DT = DateTime.Now;
            SupplierCode = string.Empty;
            Sup_Name = string.Empty;
            Sup_Short_Name = string.Empty;
            Act_Status = string.Empty;
            Tax_Rate = string.Empty;
            Address = string.Empty;
            Bank_Name = string.Empty;
            Bank_Account = string.Empty;
            Remark = string.Empty;

            Sales_Man = string.Empty;
            Sales_Man_Tel = string.Empty;
            Sales_Man_Fax = string.Empty;
            Sales_Man_Mail = string.Empty;
            Sales_Manager = string.Empty;
            Sales_Manager_Tel = string.Empty;
            Sales_Manager_Fax = string.Empty;
            Sales_Manager_Mail = string.Empty;
            Finance = string.Empty;
            Finance_Tel = string.Empty;
            Finance_Fax = string.Empty;
            Finance_Mail = string.Empty;
            GM = string.Empty;
            GM_Tel = string.Empty;
            GM_Fax = string.Empty;
            GM_Mail = string.Empty;


            LinkMainCID = Guid.Empty;

            District_List = Enum.GetNames(typeof(Sup_District_Enum)).ToList();
            Sup_Level_List = Enum.GetNames(typeof(Sup_Level_Enum)).ToList();
            Qualification_List = Enum.GetNames(typeof(Sup_Qualification_Enum)).ToList();
            Type_List = Enum.GetNames(typeof(Sup_Type_Enum)).ToList();
            Act_Status_List = Enum.GetNames(typeof(Sup_Act_Status_Enum)).ToList();
            Act_Status_Reason_List = Enum.GetNames(typeof(Sup_Act_Status_Reason_Enum)).ToList();
            Payment_List = Enum.GetNames(typeof(Sup_Payment_Enum)).ToList();
            Settle_Accounts_List = Enum.GetNames(typeof(Sup_Settle_Accounts_Enum)).ToList();

            Payment_List_Json = new List<string>();
            Main_Business_List_Json = new List<string>();
            Main_Brand_List_Json = new List<string>();
            Settle_Accounts_List_Json = new List<string>();

            Main_Brand_List = new List<string>();

            Tax_Rate_List = new List<SelectOption>();
            foreach(var Tax in Enum.GetNames(typeof(Sup_Tax_Rate_Enum)).ToList())
            {
                Tax_Rate_List.Add(new SelectOption { Key_STR = Tax.ToString(), Name = Tax.ToString() + "%" });
            }
        }

        [NotMapped]
        public List<string> District_List { get; set; }

        [NotMapped]
        public List<string> Sup_Level_List { get; set; }

        [NotMapped]
        public List<string> Qualification_List { get; set; }

        [NotMapped]
        public List<string> Type_List { get; set; }

        [NotMapped]
        public List<string> Payment_List { get; set; }

        [NotMapped]
        public List<string> Settle_Accounts_List { get; set; }

        [NotMapped]
        public List<string> Main_Brand_List { get; set; }

        [NotMapped]
        public List<string> Main_Business_List { get; set; }

        [NotMapped]
        public List<string> Act_Status_List { get; set; }

        [NotMapped]
        public List<string> Act_Status_Reason_List { get; set; }

        [NotMapped]
        public List<SelectOption> Tax_Rate_List { get; set; }

        [NotMapped]
        public int IsLock_Sup_Name { get; set; }

        [NotMapped]
        public int IsLock_Delete { get; set; }

        [NotMapped]
        public List<string> Logistics_Mode_List { get; set; }
    }

    public enum Sup_Tax_Rate_Enum
    {
        VAT3,
        VAT6,
        VAT11,
        VAT16,
        VAT17,
    }

    public enum Sup_Act_Status_Enum
    {
        OPEN,
        CLOSE,
    }

    public enum Sup_Act_Status_Reason_Enum
    {
        品质问题,
        渠道不明,
        货期无法保证,
    }

    public enum Sup_Type_Enum
    {
        国营,
        私营,
        独资,
        合资,
    }

    public enum Sup_Qualification_Enum
    {
        授权经销商,
        非授权经销商,
    }

    public enum Sup_District_Enum
    {
        华东,
        华南,
        华北,
        西南,
        西北,
    }

    public enum Sup_Level_Enum
    {
        首选,
        优选,
        备选,
    }

    public enum Sup_Payment_Enum
    {
        现金,
        银行转账,
        承兑,
    }

    public enum Sup_Settle_Accounts_Enum
    {
        现金,
        月结N_1,
        月结N_2,
        月结N_3,
        月结N_4,
        月结N_5,
        月结N_6,
        银行转账,
        承兑,
    }

    public class Supplier_Filter
    {
        public Supplier_Filter()
        {
            Supplier Empty_Sup = new Supplier();

            PageIndex = 1;
            PageSize = 25;
            Keyword = string.Empty;

            Type = string.Empty;
            Qualification = string.Empty;
            Sup_Level = string.Empty;
            District = string.Empty;
            Sup_Brand = string.Empty;
            Act_Status = string.Empty;

            Type_List = Empty_Sup.Type_List;
            Qualification_List = Empty_Sup.Qualification_List;
            Level_List = Empty_Sup.Sup_Level_List;
            District_List = Empty_Sup.District_List;
            Act_Status_List = Empty_Sup.Act_Status_List;
            LinkMainCID = Guid.Empty;

        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }

        public string Type { get; set; }
        public string Qualification { get; set; }
        public string Sup_Level { get; set; }
        public string Sup_Brand { get; set; }
        public string District { get; set; }
        public string Act_Status { get; set; }

        public List<string> Type_List { get; set; }
        public List<string> Qualification_List { get; set; }
        public List<string> Level_List { get; set; }
        public List<string> District_List { get; set; }
        //public List<string> Sup_Brand_List { get; set; }
        public List<string> Act_Status_List { get; set; }


        public Guid LinkMainCID { get; set; }
    }
}
