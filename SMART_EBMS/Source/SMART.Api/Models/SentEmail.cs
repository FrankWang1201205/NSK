using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMART.Api.Models
{
    public class SentEmail
    {
        public SentEmail()
        {
            Port = "25";
            MailType = "POP3";
            MainCID = Guid.Empty;
        }

        [Key]
        public Guid MainCID { get; set; }

        [Required]
        [MaxLength(256)]
        public string MailType { get; set; }

        [Required]
        [MaxLength(256)]
        public string SMTP { get; set; }

        [Required]
        [MaxLength(256)]
        public string Port { get; set; }

        [Required]
        [MaxLength(256)]
        public string MailName { get; set; }

        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
    }

    public class SentEmailRecord
    {
        public SentEmailRecord()
        {
            Is_Error = 0;
            SMTP_Json = string.Empty;
            Create_DT = DateTime.Now;
            Subject = string.Empty;
            Body = string.Empty;
            Is_Error_Info = string.Empty;
            LinkMainCID = Guid.Empty;
        }

        [Key]
        public int ESID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [DefaultValue("")]
        public string E_Mail { get; set; }

        [DefaultValue("")]
        public string Subject { get; set; }

        [DefaultValue("")]
        public string Body { get; set; }

        [DefaultValue("")]
        public string SMTP_Json { get; set; }

        [DefaultValue(0)]
        public int Is_Error { get; set; }

        [DefaultValue("")]
        public string Is_Error_Info { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }
    }

    [NotMapped]
    public class SentEmail_Filter
    {
        public SentEmail_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Email = string.Empty;
            Subject = string.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
    }

}