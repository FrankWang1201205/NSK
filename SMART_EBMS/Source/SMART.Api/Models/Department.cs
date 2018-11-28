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
    public class Department
    {
        public Department()
        {
            DepID = Guid.Empty;
            Create_DT = DateTime.Now;
            DepName = string.Empty;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public Guid DepID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string DepName { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }
}
