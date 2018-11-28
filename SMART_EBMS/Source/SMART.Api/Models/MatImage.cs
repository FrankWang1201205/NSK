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
    public class MatImage
    {
        public MatImage()
        {
            IMGID = Guid.Empty;
            Create_DT = DateTime.Now;
            Create_Person = string.Empty;
            MatName = string.Empty;
            MatBrand = string.Empty;
            MatThumbImgPath = string.Empty;
            MatImgPath = string.Empty;
            MatSourceImgPath = string.Empty;
            LinkMainCID = Guid.Empty;
            Detail_List = new List<MatImage_Detail>();
            MatCount = 0;
        }

        [Key]
        public Guid IMGID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        public string MatName { get; set; }

        [DefaultValue("")]
        public string MatBrand { get; set; }

        [Required]
        public string Create_Person { get; set; }

        //缩略图路径
        [DefaultValue("")]
        public string MatThumbImgPath { get; set; }

        //展示图片
        [DefaultValue("")]
        public string MatImgPath { get; set; }

        //原始图片
        [DefaultValue("")]
        public string MatSourceImgPath { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public int MatCount { get; set; }

        [NotMapped]
        public List<MatImage_Detail> Detail_List { get; set; }
    }

    public class MatImage_Detail
    {
        public MatImage_Detail()
        {
            IMDID = Guid.Empty;
            Create_DT = DateTime.Now;
            Detail_Html_Str = string.Empty;
            LinkMainCID = Guid.Empty;
            Link_IMGID = Guid.Empty;
        }

        [Key]
        public Guid IMDID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [DefaultValue("")]
        public string Detail_Html_Str { get; set; }

        [Required]
        public Guid Link_IMGID { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }
}
