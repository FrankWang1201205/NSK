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
    public class Purchase_Temp
    {
        public Purchase_Temp()
        {
            PTID = Guid.Empty;
            Cust_Code = string.Empty;
            MatSn = string.Empty;
            MatBrand = string.Empty;
            Cust_Short_Name = string.Empty;
            Sales_Rep_Name = string.Empty;
            PC_Code = string.Empty;
            AP_Sup_Code = string.Empty;
            Currency_Code = string.Empty;
            Contract_Price = 0;
            Link_MainCID = Guid.Empty;
            Create_DT = DateTime.Now;
        }

        [Key]
        public Guid PTID { get; set; }

        [Required]
        public string Cust_Code { get; set; }

        [Required]
        public string MatSn { get; set; }

        public string MatBrand { get; set; }

        [Required]
        public string Cust_Short_Name { get; set; }

        [Required]
        public string Sales_Rep_Name { get; set; }

        [Required]
        public string PC_Code { get; set; }

        public string AP_Sup_Code { get; set; }

        [Required]
        public string Currency_Code { get; set; }

        [Required]
        public decimal Contract_Price { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Link_MainCID { get; set; }

    }

    public class Purchase_Temp_Search
    {
        public Purchase_Temp_Search()
        {
            PTS_ID = Guid.Empty;
            MatBrand = string.Empty;
            MatSn = string.Empty;
            Quantity = 0;
            Quantity_Request = 0;
            Quantity_Request_More = 0;
            Price_Cost = 0;
            Sales_Person = string.Empty;
            LinkMainCID = Guid.Empty;
            Customer_Name = string.Empty;
            Supplier_Name = string.Empty;
            Cust_Code = string.Empty;
            Contract_Price = 0;
            QRCode_Path = string.Empty;
        }

        [Key]
        public Guid PTS_ID { get; set; }

        [Required]
        public string MatSn { get; set; }

        public string MatBrand { get; set; }

        //到货数量
        public int Quantity { get; set; }

        //需求数量
        public int Quantity_Request { get; set; }

        //备货数量
        public int Quantity_Request_More { get; set; }

        //调货价格
        public decimal Price_Cost { get; set; }

        //调货人
        public string Sales_Person { get; set; }

        public string Customer_Name { get; set; }
        
        public string Supplier_Name { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public string Cust_Code { get; set; }

        [NotMapped]
        public decimal Contract_Price { get; set; }

        [NotMapped]
        public string QRCode_Path { get; set; }
    }

    [NotMapped]
    public class Purchase_Temp_Filter
    {
        public Purchase_Temp_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            LinkMainCID = Guid.Empty;
            MatSn = string.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string MatSn { get; set; }
        public Guid LinkMainCID { get; set; }
    }
}
