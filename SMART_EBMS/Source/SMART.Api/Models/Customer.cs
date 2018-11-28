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
    public partial class Customer
    {
        [Key]
        public Guid CID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [DefaultValue("")]
        public string Create_Person { get; set; }

        //客户代码
        [DefaultValue("")]
        public string Cust_Code { get; set; }

        //客户名称
        [Required]
        public string Cust_Name { get; set; }

        //所属地区
        [DefaultValue("")]
        public string District { get; set; }

        //所属行业
        [DefaultValue("")]
        public string Industry { get; set; }

        //客户性质、类型
        [DefaultValue("")]
        public string Cust_Type { get; set; }

        //客户来源
        [DefaultValue("")]
        public string FormBy { get; set; }

        //成立日期
        [DefaultValue("")]
        public string Establishment { get; set; }

        //销售方式
        [DefaultValue("")]
        public string Sales_Type { get; set; }


        //结算方式
        [DefaultValue("")]
        public string Settle_Accounts { get; set; }


        //支付方式
        [DefaultValue("")]
        public string Payment_Type { get; set; }

        //对账日期
        [DefaultValue("")]
        public string Statement_Day { get; set; }

        //合同类型
        [DefaultValue("")]
        public string Contract_Type { get; set; }

        //合同有效期
        [Required]
        public DateTime Contract_End_Date { get; set; }

        //是否每次报价
        [Required]
        [DefaultValue(0)]
        public int Is_Contract_RFQ { get; set; }

        //收货地址
        [DefaultValue("")]
        public string Ship_Address { get; set; }

        //收货公里数
        [DefaultValue(0)]
        public int Ship_Address_KM { get; set; }

        //发运方式
        [DefaultValue("")]
        public string Ship_Type { get; set; }

        //运费支付
        [DefaultValue("")]
        public string Ship_Pay { get; set; }

        //客户产品
        [DefaultValue("")]
        public string Cust_Product { get; set; }

        //竞争品牌
        [DefaultValue("")]
        public string Compete_Brand { get; set; }

        //竞争对手
        [DefaultValue("")]
        public string Compete_Company { get; set; }

        //开票抬头
        [DefaultValue("")]
        public string Invoice_Title { get; set; }

        [DefaultValue("")]
        public string Invoice_Identification { get; set; }

        [DefaultValue("")]
        public string Invoice_Address { get; set; }

        [DefaultValue("")]
        public string Invoice_Tel { get; set; }

        //开户行
        [DefaultValue("")]
        public string Bank_Name { get; set; }

        //开户行账号
        [DefaultValue("")]
        public string Bank_Account { get; set; }

        //联系人
        [DefaultValue("")]
        public string Buyer { get; set; }

        [DefaultValue("")]
        public string Buyer_Tel { get; set; }

        [DefaultValue("")]
        public string Buyer_Fax { get; set; }

        [DefaultValue("")]
        public string Buyer_Mail { get; set; }

        //联系人
        [DefaultValue("")]
        public string Keeper { get; set; }

        [DefaultValue("")]
        public string Keeper_Tel { get; set; }

        [DefaultValue("")]
        public string Keeper_Fax { get; set; }

        [DefaultValue("")]
        public string Keeper_Mail { get; set; }

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

        [DefaultValue("")]
        public string VIP_Type { get; set; }

        [NotMapped]
        public List<string> VIP_Type_List { get; set; }

        [DefaultValue("")]
        public string Cust_Trade_Type { get; set; }

        [NotMapped]
        public List<string> Cust_Trade_Type_List { get; set; }
        
        public Guid Sales_UID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        public Guid Link_GID { get; set; }
    }

    public partial class Customer
    {
        public Customer()
        {
            CID = Guid.Empty;
            Create_DT = DateTime.Now;
            Create_Person = string.Empty;
            Contract_End_Date = DateTime.Now;
            Cust_Code = string.Empty;
            Cust_Name = string.Empty;
            District = string.Empty;
            Industry = string.Empty;
            Cust_Type = string.Empty;
            FormBy = string.Empty;
            Establishment = string.Empty;
            Statement_Day = string.Empty;
            Payment_Type = string.Empty;
            Sales_Type = string.Empty;
            Settle_Accounts = string.Empty;
            Contract_Type = string.Empty;
            Contract_End_Date = DateTime.Now;
            Is_Contract_RFQ = 0;
            Ship_Address = string.Empty;
            Ship_Address_KM = 0;
            Ship_Type = string.Empty;
            Ship_Pay = string.Empty;
            Cust_Product = string.Empty;
            Compete_Brand = string.Empty;
            Compete_Company = string.Empty;


            Invoice_Title = string.Empty;
            Invoice_Identification = string.Empty;
            Invoice_Address = string.Empty;
            Invoice_Tel = string.Empty;
            Bank_Name = string.Empty;
            Bank_Account = string.Empty;

            Buyer = string.Empty;
            Buyer_Tel = string.Empty;
            Buyer_Fax = string.Empty;
            Buyer_Mail = string.Empty;

            Keeper = string.Empty;
            Keeper_Tel = string.Empty;
            Keeper_Fax = string.Empty;
            Keeper_Mail = string.Empty;

            Finance = string.Empty;
            Finance_Tel = string.Empty;
            Finance_Fax = string.Empty;
            Finance_Mail = string.Empty;

            GM = string.Empty;
            GM_Tel = string.Empty;
            GM_Fax = string.Empty;
            GM_Mail = string.Empty;

            Sales_UID = Guid.Empty;
            Sales_UID_Name = string.Empty;
            Sales_User_List = new List<User>();

            VIP_Type = string.Empty;
            Cust_Trade_Type = string.Empty;

            VIP_Type_List= Enum.GetNames(typeof(VIP_Type_Enum)).ToList();
            Cust_Trade_Type_List= Enum.GetNames(typeof(Cust_Trade_Type_Enum)).ToList();

            District_List = Enum.GetNames(typeof(Cust_District_Enum)).ToList();
            Industry_List = Enum.GetNames(typeof(Cust_Industry_Enum)).ToList();
            Cust_Type_List = Enum.GetNames(typeof(Cust_Type_Enum)).ToList();
            FormBy_List = Enum.GetNames(typeof(Cust_FormBy_Enum)).ToList();
            Sales_Type_List = Enum.GetNames(typeof(Cust_Sales_Type_Enum)).ToList();

            Settle_Accounts_List = new List<string>();
            Settle_Accounts_List.Add("现款");
            Settle_Accounts_List.Add("N+1");
            Settle_Accounts_List.Add("N+2");
            Settle_Accounts_List.Add("N+3");
            Settle_Accounts_List.Add("N+4");
            Settle_Accounts_List.Add("N+5");
            Settle_Accounts_List.Add("N+6");
          
            Payment_Type_List = Enum.GetNames(typeof(Cust_Payment_Type_Enum)).ToList();
            Statement_Day_List = Enum.GetNames(typeof(Cust_Statement_Day_Enum)).ToList();
            Contract_Type_List = Enum.GetNames(typeof(Cust_Contract_Type_Enum)).ToList();

            Ship_Pay_List = Enum.GetNames(typeof(Cust_Ship_Pay_Enum)).ToList();
            Ship_Type_List = Enum.GetNames(typeof(Cust_Ship_Type_Enum)).ToList();

            IsLock_Customer_Name = 0;
            IsLock_Delete = 0;
            LinkMainCID = Guid.Empty;
            Link_GID = Guid.Empty;
        }

        [NotMapped]
        public List<string> District_List { get; set; }

        [NotMapped]
        public List<string> Industry_List { get; set; }

        [NotMapped]
        public List<string> Cust_Type_List { get; set; }

        [NotMapped]
        public List<string> FormBy_List { get; set; }

        [NotMapped]
        public List<string> Sales_Type_List { get; set; }

        [NotMapped]
        public List<string> Settle_Accounts_List { get; set; }

        [NotMapped]
        public List<string> Payment_Type_List { get; set; }

        [NotMapped]
        public List<string> Statement_Day_List { get; set; }

        [NotMapped]
        public List<string> Contract_Type_List { get; set; }

        [NotMapped]
        public List<string> Ship_Pay_List { get; set; }

        [NotMapped]
        public List<string> Ship_Type_List { get; set; }

        [NotMapped]
        public int IsLock_Customer_Name { get; set; }

        [NotMapped]
        public int IsLock_Delete { get; set; }

        [NotMapped]
        public string Sales_UID_Name { get; set; }
     
        [NotMapped]
        public List<User> Sales_User_List { get; set; }
    }

    [NotMapped]
    public class Customer_Filter
    {
        public Customer_Filter()
        {
            Customer Empty_C = new Customer();

            PageIndex = 1;
            PageSize = 25;
            Cust_Code = string.Empty;
            Cust_Name = string.Empty;
            Cust_Name_Or_Code = string.Empty;

            Sales_UID = Guid.Empty;
            SalesUserList = new List<User>();

            Cust_Type = string.Empty;
            Cust_Type_List = Empty_C.Cust_Type_List;

            District = string.Empty;
            District_List = Empty_C.District_List;

            Industry = string.Empty;
            Industry_List = Empty_C.Industry_List;

            FormBy = string.Empty;
            FormBy_List = Empty_C.FormBy_List;

            Sales_Type = string.Empty;
            Sales_Type_List = Empty_C.Sales_Type_List;

            Settle_Accounts = string.Empty;
            Settle_Accounts_List = Empty_C.Settle_Accounts_List;

            Payment_Type = string.Empty;
            Payment_Type_List = Empty_C.Payment_Type_List;

            Statement_Day = string.Empty;
            Statement_Day_List = Empty_C.Statement_Day_List;

            Contract_Type = string.Empty;
            Contract_Type_List = Empty_C.Contract_Type_List;

            Ship_Type = string.Empty;
            Ship_Type_List = Empty_C.Ship_Type_List;
            LinkMainCID = Guid.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Cust_Code { get; set; }
        public string Cust_Name { get; set; }
        public string Cust_Name_Or_Code { get; set; }

        public Guid Sales_UID { get; set; }
        public List<User> SalesUserList { get; set; }

        public string Cust_Type { get; set; }
        public List<string> Cust_Type_List { get; set; }

        public string District { get; set; }
        public List<string> District_List { get; set; }

        public string Industry { get; set; }
        public List<string> Industry_List { get; set; }

        public string FormBy { get; set; }
        public List<string> FormBy_List { get; set; }

        public string Sales_Type { get; set; }
        public List<string> Sales_Type_List { get; set; }

        public string Settle_Accounts { get; set; }
        public List<string> Settle_Accounts_List { get; set; }

        public string Payment_Type { get; set; }
        public List<string> Payment_Type_List { get; set; }

        public string Statement_Day { get; set; }
        public List<string> Statement_Day_List { get; set; }

        public string Contract_Type { get; set; }
        public List<string> Contract_Type_List { get; set; }

        public string Ship_Type { get; set; }
        public List<string> Ship_Type_List { get; set; }

        public Guid LinkMainCID { get; set; }
    }

    public enum Cust_District_Enum
    {
        华东,
        华南,
        华北,
        西南,
        西北,
    }

    public enum Cust_Industry_Enum
    {
        电机,
        AM,
        OEM,
        精机,
        汽车,
        其他,
    }
    
    public enum Cust_Type_Enum
    {
        国营,
        私营,
        独资,
        合资,
    }

    public enum Cust_FormBy_Enum
    {
        客户介绍,
        客户咨询,
        自己开发,
    }

    public enum Cust_Sales_Type_Enum
    {
        现款,
        赊账,
        寄售,
    }

    public enum Cust_Payment_Type_Enum
    {
        现款,
        转账,
        承兑,
    }

    public enum Cust_Statement_Day_Enum
    {
        月中,
        月末,
        月初,
        不对账,
    }

    public enum Cust_Contract_Type_Enum
    {
        年,
        季,
        月,
        无,
    }

    public enum Cust_Ship_Type_Enum
    {
        客户自提,
        供方配送,
    }

    public enum Cust_Ship_Pay_Enum
    {
        买方付费,
        卖方付费,
    }

    public enum VIP_Type_Enum
    {
        VIP,
        NEW,
    }

    public enum Cust_Trade_Type_Enum
    {
        终端企业,
        贸易商,
    }
}

namespace SMART.Api.Models
{
    public class Customer_Group
    {
        public Customer_Group()
        {
            GID = Guid.Empty;
            Group_Code = string.Empty;
            Group_Name = string.Empty;
            LinkMainCID = Guid.Empty;
            Create_DT = DateTime.Now;
            Cust_List_Count = 0;
            Cust_List = new List<Customer>();
        }

        [Key]
        public Guid GID { get; set; }

        [Required]
        public string Group_Code { get; set; }

        [Required]
        public string Group_Name { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [NotMapped]
        public int Cust_List_Count { get; set; }

        [NotMapped]
        public List<Customer> Cust_List { get; set; }
    }

    [NotMapped]
    public class Customer_Group_Filter
    {
        public Customer_Group_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Cust_Group_Code = string.Empty;
            Cust_Group_Name = string.Empty;
            LinkMainCID = Guid.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Cust_Group_Code { get; set; }
        public string Cust_Group_Name { get; set; }
        public Guid LinkMainCID { get; set; }
    }
}

namespace SMART.Api.Models
{
    public class Customer_Mat
    {
        public Customer_Mat()
        {
            Cus_MatID = Guid.Empty;
            Other_MatSn = string.Empty;
            Link_MatID = Guid.Empty;
            Link_CID = Guid.Empty;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid Cus_MatID { get; set; }

        [Required]
        public string Other_MatSn { get; set; }

        [Required]
        public Guid Link_MatID { get; set; }

        [Required]
        public Guid Link_CID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }
}
