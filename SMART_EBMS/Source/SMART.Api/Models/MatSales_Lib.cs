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
    public class MatSales_Lib
    {
        public MatSales_Lib()
        {
            MSLID = Guid.Empty;
            MatID = Guid.Empty;
            Create_DT = DateTime.Now;
            Sales_UID = Guid.Empty;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid MSLID { get; set; }

        [Required]
        public Guid MatID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public Guid Sales_UID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }

    public enum MySales_Enum
    {
        IsMySales,
        IsNotMySales,
    }
}
