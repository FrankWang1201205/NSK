using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace SMART.EBMS
{
    public class CommonFunctionLib
    {
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
    }

    
}