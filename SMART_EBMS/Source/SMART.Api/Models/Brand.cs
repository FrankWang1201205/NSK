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
    public class Brand
    {
        public Brand()
        {
            BID = Guid.Empty;
            MatSn_Length_Min = 0;
            MatSn_Length_Max = 0;
            Create_DT = DateTime.Now;
            Brand_Name = string.Empty;
            Summary = string.Empty;
            Logo = string.Empty;
            Certificate = string.Empty;
            Mat_Count = 0;
            IsLock_Delete = 0;
            MatSn_Length_Min = 0;
            MatSn_Length_Max = 0;
            Is_Customized_Page = 0;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid BID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [DefaultValue("")]
        public string Create_Person { get; set; }

        //产品型号-最小位数
        [Required]
        public int MatSn_Length_Min { get; set; }

        //产品型号-最大位数
        [Required]
        public int MatSn_Length_Max { get; set; }

        //是否单独定制页面
        [Required]
        public int Is_Customized_Page { get; set; }
        
        [Required]
        public string Brand_Name { get; set; }
       
        [DefaultValue("")]
        public string Logo { get; set; }

        [DefaultValue("")]
        public string Summary { get; set; }

        [DefaultValue("")]
        public string Certificate { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public int Mat_Count { get; set; }

        [NotMapped]
        public int IsLock_Delete { get; set; }
    }

    public class Brand_Filter
    {
        public Brand_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Keyword = string.Empty;
            LinkMainCID = Guid.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int SDFSDF { get; set; }
        public string Keyword { get; set; }
        public Guid LinkMainCID { get; set; }
    }
}
