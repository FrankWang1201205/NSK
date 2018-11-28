using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMART.Api
{
    public class MyGUID
    {
        public static Guid NewGUID()
        {
            string TimeStr = DateTime.Now.ToString("yyyyMMdd-HHmm");
            Guid New_G = new Guid(TimeStr + Guid.NewGuid().ToString().Substring(13, 23));
            return New_G;
        }
    }
}