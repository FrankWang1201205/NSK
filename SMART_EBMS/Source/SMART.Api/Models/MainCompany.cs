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
    public class MainCompany
    {
        public MainCompany()
        {
            MainCID = Guid.Empty;
            MainCompanyName = string.Empty;
            MainTel = string.Empty;
            MainEmail = string.Empty;
            MainInvoiceAddress = string.Empty;
            MainAddress = string.Empty;
            TaxpayerIdentificationNo = string.Empty;
            MainBankInfo = string.Empty;
            ElectronicStamp = string.Empty;
            ComLogo = string.Empty;
            CreateDate = DateTime.Now;
            PinCode = string.Empty;
        }

        [Key]
        public Guid MainCID { get; set; }

        //创建日期
        [Required]
        public DateTime CreateDate { get; set; }

        //企业代码
        [Required]
        public string PinCode { get; set; }

        //经营主体全称
        [Required]
        public string MainCompanyName { get; set; }

        //电话
        [DefaultValue("")]
        public string MainTel { get; set; }

        //电话
        [DefaultValue("")]
        public string MainEmail { get; set; }

        //营业执照开票地址
        [DefaultValue("")]
        [MaxLength(255)]
        public string MainInvoiceAddress { get; set; }

        //公司地址
        [DefaultValue("")]
        public string MainAddress { get; set; }

        //纳税人识别号
        [DefaultValue("")]
        public string TaxpayerIdentificationNo { get; set; }

        //开户行
        [DefaultValue("")]
        public string MainBankInfo { get; set; }

        //银行账号
        [DefaultValue("")]
        public string MainBankAccount { get; set; }

        //电子图章
        [DefaultValue("")]
        public string ElectronicStamp { get; set; }

        //电子Logo
        [DefaultValue("")]
        public string ComLogo { get; set; }

        [NotMapped]
        public string Admin_User_Name { get; set; }

        [NotMapped]
        public string Admin_User_Password { get; set; }
    }


}
