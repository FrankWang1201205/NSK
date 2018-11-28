using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMART.Api.Models
{
    public class MaxInt
    {
        public MaxInt()
        {
            MaxID = Guid.Empty;
            MaxNo = 1000;
            AppName = string.Empty;
            PreCode = string.Empty;
        }

        [Key]
        public Guid MaxID { get; set; }

        [DefaultValue(0)]
        [Required]
        public int MaxNo { get; set; }

        //业务单元
        [DefaultValue("")]
        [MaxLength(64)]
        public string AppName { get; set; }

        //编码前缀
        [DefaultValue("")]
        [MaxLength(16)]
        public string PreCode { get; set; }
    }

    public enum MaxIntAppName
    {
        Material,
        Customer,
        Supplier,
        SalesPlan,
        Po_Head,
        RFQ_Head,
        WMS_In,
        WMS_Out_Doc,
    }
}