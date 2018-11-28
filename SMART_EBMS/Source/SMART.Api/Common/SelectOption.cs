using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace System
{
    [NotMapped]
    public class SelectOption
    {
        public SelectOption()
        {
            Key_GUID = Guid.Empty;
            Key_INT = 0;
            Key_STR = string.Empty;
            Name = string.Empty;
        }

        public Guid Key_GUID { get; set; }
        public int Key_INT { get; set; }
        public string Key_STR { get; set; }
        public string Name { get; set; }
    }

    [NotMapped]
    public class Normal_Filter
    {
        public Normal_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Keyword = string.Empty;
            Keyword_Sup = string.Empty;
            Status = string.Empty;
            TypeInfo = string.Empty;
            OutObj = new Normal_Filter_OutObj();
            Key_A = Guid.Empty;
            Key_B = Guid.Empty;
            Key_C = Guid.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string Keyword_Sup { get; set; }
        public string Status { get; set; }
        public string TypeInfo { get; set; }
        public Guid Key_A { get; set; }
        public Guid Key_B { get; set; }
        public Guid Key_C { get; set; }
        public Normal_Filter_OutObj OutObj { get; set; }
    }

    [NotMapped]
    public class Normal_Filter_OutObj
    {
        public Normal_Filter_OutObj()
        {
            List_Str_A = new List<string>();
            List_Str_B = new List<string>();
            List_Str_C = new List<string>();
            List_A = new List<SelectOption>();
            List_B = new List<SelectOption>();
            List_C = new List<SelectOption>();
        }

        public List<string> List_Str_A { get; set; }
        public List<string> List_Str_B { get; set; }
        public List<string> List_Str_C { get; set; }
        public List<string> List_Str_Type { get; set; }
        public List<SelectOption> List_A { get; set; }
        public List<SelectOption> List_B { get; set; }
        public List<SelectOption> List_C { get; set; }
    }

    public static class MyMonth
    {
        public static List<SelectOption> GetMonthList()
        {
            List<SelectOption> List = new List<SelectOption>();
            List.Add(new SelectOption { Key_INT = 1, Name = "Jan" });
            List.Add(new SelectOption { Key_INT = 2, Name = "Feb" });
            List.Add(new SelectOption { Key_INT = 3, Name = "Mar" });
            List.Add(new SelectOption { Key_INT = 4, Name = "Apr" });
            List.Add(new SelectOption { Key_INT = 5, Name = "May" });
            List.Add(new SelectOption { Key_INT = 6, Name = "June" });
            List.Add(new SelectOption { Key_INT = 7, Name = "July" });
            List.Add(new SelectOption { Key_INT = 8, Name = "Aug" });
            List.Add(new SelectOption { Key_INT = 9, Name = "Sept" });
            List.Add(new SelectOption { Key_INT = 10, Name = "Oct" });
            List.Add(new SelectOption { Key_INT = 11, Name = "Nov" });
            List.Add(new SelectOption { Key_INT = 12, Name = "Dec" });
            return List;
        }

    }


}