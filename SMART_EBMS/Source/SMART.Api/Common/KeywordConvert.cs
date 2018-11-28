using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace System
{
    public class KeywordConvert
    {
        static public List<string> CoverToKeyList(string Keyword)
        {
            List<string> List = new List<string>();
            if (!string.IsNullOrEmpty(Keyword))
            {
                Keyword = Regex.Replace(Keyword, @"( )\1+", "$1", RegexOptions.None);
                string[] KeywordStr = Keyword.Split(' ');
                if (KeywordStr.Count() == 1)
                {
                    List.Add(KeywordStr[0]);
                }
                else if (KeywordStr.Count() == 2)
                {
                    List.Add(KeywordStr[0]);
                    List.Add(KeywordStr[1]);
                }
                else if (KeywordStr.Count() == 3)
                {
                    List.Add(KeywordStr[0]);
                    List.Add(KeywordStr[1]);
                    List.Add(KeywordStr[2]);
                }
                else if (KeywordStr.Count() > 3)
                {
                    List.Add(KeywordStr[0]);
                    List.Add(KeywordStr[1]);
                    List.Add(KeywordStr[2]);
                }
            }
            return List;
        }
    }

}
