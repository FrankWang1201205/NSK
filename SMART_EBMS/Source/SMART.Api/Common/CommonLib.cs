using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SMART.Api
{
    public class CommonLib
    {
        public static string GetProperties<T>(T Obj)
        {
            string ResultStr = string.Empty;
            if (Obj == null)
            {
                return ResultStr;
            }

            Type t = Obj.GetType();//获得该类的Type

            //再用Type.GetProperties获得PropertyInfo[],然后就可以用foreach 遍历了
            string Value = string.Empty;
            string Name = string.Empty;
            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (pi.PropertyType.Name.StartsWith("String") || pi.PropertyType.Name.StartsWith("Int") || pi.PropertyType.Name.StartsWith("Guid"))
                {
                    //用pi.GetValue获得值
                    Value = pi.GetValue(Obj, null).ToString();

                    //获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                    Name = pi.Name;

                    //进行你想要的操作
                    ResultStr += "&" + Name + "=" + Value;
                }
            }
            return ResultStr;
        }

        public static string CompanyName_Replace_and_ToLower(string CompanyName)
        {
            string NewComName = CompanyName;
            try
            {
                NewComName = NewComName.Replace("(", "");
                NewComName = NewComName.Replace("（", "");
                NewComName = NewComName.Replace("）", "");
                NewComName = NewComName.Replace(")", "");
                NewComName = NewComName.ToLower();
            }
            catch { }
            return NewComName;
        }

        //获取字符串内整数
        public static int GetNumberInt(string str)
        {
            int result = 0;
            try
            {
                if (str != null && str != string.Empty)
                {
                    // 正则表达式剔除非数字字符（不包含小数点.） 
                    str = Regex.Replace(str, @"[^\d.\d]", "");
                    // 如果是数字，则转换为decimal类型 
                    if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                    {
                        result = int.Parse(str);
                    }
                }
            }
            catch
            {
                result = 0;
            }

            return result;
        }

        //含税换算未税
        public static decimal VatPriceToPrice(decimal VatPrice, int TaxRate)
        {
            decimal Price = 0;
            if (TaxRate > 0 && TaxRate <= 17)
            {
                Price = VatPrice / ((decimal)1 + (TaxRate / (decimal)100));
            }
            return Price;
        }

        //未税换算含税
        public static decimal PriceToVatPrice(decimal Price, int TaxRate)
        {
            decimal VatPrice = 0;
            if (TaxRate > 0 && TaxRate <= 17)
            {
                VatPrice = Price * ((decimal)1 + (TaxRate / (decimal)100));
            }
            return VatPrice;
        }

        public static List<Guid> GuidListStrToGuidArray(string GuidListStr)
        {

            //以,作为分离器
            char[] separator = { ',' };

            //以,拆分字符串为字符串数组
            string[] PPIDStrList;
            PPIDStrList = GuidListStr.Split(separator);
            List<Guid> GuidList = new List<Guid>();
            foreach (string GuidStr in PPIDStrList)
            {
                try
                {
                    GuidList.Add(new Guid(GuidStr));
                }
                catch
                {
                    GuidList.Add(Guid.Empty);
                }

            }
            return GuidList;
        }

        public static List<string> StringListStrToStringArray(string StringListStr)
        {
            //以,作为分离器
            char[] separator = { ',' };

            //以,拆分字符串为字符串数组
            string[] StrList;
            List<string> StringList = new List<string>();

            try
            {
                StrList = StringListStr.Split(separator);
                foreach (string Str in StrList)
                {
                    StringList.Add(Str);
                }
            }
            catch
            {
                StringList = new List<string>();
            }
            return StringList;
        }

        //替换最后一个字符
        public static string Trim_End_Char(string STR)
        {
            string New_STR = string.Empty;
            try { New_STR = STR.Substring(0, STR.Length - 1); } catch { New_STR = STR; }
            return New_STR;
        }

        //不允许汉字
        public static bool Is_Not_Contains_Chinese(string Input_Str)
        {
            bool Flag = Regex.IsMatch(Input_Str, "^[^\u4e00-\u9fa5]+$");
            return Flag;
        }

        //只允许数字和字母
        public static bool Is_Letter_Or_Number(string Input_Str)
        {
            bool Flag = Regex.IsMatch(Input_Str, "^[0-9a-zA-Z]+$");
            return Flag;
        }

        //只允许数字和字母和汉字
        public static bool Is_Letter_Or_Number_Or_Chinese(string Input_Str)
        {
            bool Flag = Regex.IsMatch(Input_Str, "^[0-9a-zA-Z\u4e00-\u9fa5]+$");
            return Flag;
        }
    }

    public class RMBCapitalization
    {
        public static string MoneyToChinese(string strAmount)
        {
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (strAmount.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                strAmount = strAmount.Trim().Remove(0, 1);
                IsNegative = true;
            }

            string strLower = null;
            string strUpart = null;
            string strUpper = null;

            int iTemp = 0;
            // 保留两位小数123.489→123.49　　123.4→123.4

            strAmount = Math.Round(double.Parse(strAmount), 2).ToString();
            if (strAmount.IndexOf(".") > 0)
            {
                if (strAmount.IndexOf(".") == strAmount.Length - 2)
                {
                    strAmount = strAmount + "0";
                }
            }
            else
            {
                strAmount = strAmount + ".00";
            }

            strLower = strAmount;
            iTemp = 1;
            strUpper = "";

            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;

                    case "0":
                        strUpart = "零";
                        break;

                    case "1":
                        strUpart = "壹";
                        break;

                    case "2":
                        strUpart = "贰";
                        break;

                    case "3":
                        strUpart = "叁";
                        break;

                    case "4":
                        strUpart = "肆";
                        break;

                    case "5":
                        strUpart = "伍";
                        break;

                    case "6":
                        strUpart = "陆";
                        break;

                    case "7":
                        strUpart = "柒";
                        break;

                    case "8":
                        strUpart = "捌";
                        break;

                    case "9":
                        strUpart = "玖";
                        break;
                }



                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;

                    case 2:
                        strUpart = strUpart + "角";
                        break;

                    case 3:
                        strUpart = strUpart + "";
                        break;

                    case 4:
                        strUpart = strUpart + "";
                        break;

                    case 5:
                        strUpart = strUpart + "拾";
                        break;

                    case 6:
                        strUpart = strUpart + "佰";
                        break;

                    case 7:
                        strUpart = strUpart + "仟";
                        break;

                    case 8:
                        strUpart = strUpart + "万";
                        break;

                    case 9:
                        strUpart = strUpart + "拾";
                        break;

                    case 10:
                        strUpart = strUpart + "佰";
                        break;

                    case 11:
                        strUpart = strUpart + "仟";
                        break;

                    case 12:
                        strUpart = strUpart + "亿";
                        break;

                    case 13:
                        strUpart = strUpart + "拾";
                        break;

                    case 14:
                        strUpart = strUpart + "佰";
                        break;

                    case 15:
                        strUpart = strUpart + "仟";
                        break;

                    case 16:
                        strUpart = strUpart + "万";
                        break;

                    default:
                        strUpart = strUpart + "";
                        break;
                }
                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }

            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }

            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }

            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }

            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }
    }
}