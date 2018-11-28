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
    public class Category
    {
        public Category()
        {
            CatID = Guid.Empty;
            CatName = string.Empty;
            CatNamePath = string.Empty;
            Level = 1;
            Create_DT = DateTime.Now;
            Parent_CatID = Guid.Empty;
            LinkMainCID = Guid.Empty;
            IsLockDelete = 0;
        }

        [Key]
        public Guid CatID { get; set; }

        [Required]
        public Guid Parent_CatID { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string CatName { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public int IsLockDelete { get; set; }

        [NotMapped]
        public string CatNamePath { get; set; }
    }

    [NotMapped]
    public class CatTree
    {
        public CatTree()
        {
            Top_List = new List<Cat>();
        }
        public List<Cat> Top_List { get; set; }
    }

    [NotMapped]
    public class Cat
    {
        public Cat()
        {
            CatID = Guid.Empty;
            Cat_Name = string.Empty;
        }

        public Guid CatID { get; set; }
        public string Cat_Name { get; set; }
        public List<Sub_Cat> Sub_Cat_List { get; set; } 
    }

    [NotMapped]
    public class Sub_Cat
    {
        public Sub_Cat()
        {
            CatID = Guid.Empty;
            Cat_Name = string.Empty;
            Sub_Cat_List = new List<Cat>();
        }

        public Guid CatID { get; set; }
        public string Cat_Name { get; set; }
        public List<Cat> Sub_Cat_List { get; set; }
    }

    public enum Cat_Enum
    {
        ALL,
        NoneCat
    }
}
