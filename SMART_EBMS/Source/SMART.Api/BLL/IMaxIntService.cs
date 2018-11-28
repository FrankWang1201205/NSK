using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SMART.Api.Models;

namespace SMART.Api
{
    public interface IMaxIntService
    {
        List<MaxInt> GetMaxIntList();
        MaxInt GetMaxIntByApp(string AppName);
        MaxInt GetMaxIntByApp(string AppName, string Now_DT);
    }

    public class MaxIntService: IMaxIntService
    {
        SmartdbContext db = new SmartdbContext();

        public List<MaxInt> GetMaxIntList()
        {
            List<MaxInt> List = new List<MaxInt>();
            MaxInt Max = new MaxInt();

            Max = new MaxInt();
            Max.MaxID = Guid.Empty;
            Max.AppName = MaxIntAppName.Material.ToString();
            Max.PreCode = "K";
            Max.MaxNo = 1000;
            List.Add(Max);

            Max = new MaxInt();
            Max.MaxID = Guid.Empty;
            Max.AppName = MaxIntAppName.Customer.ToString();
            Max.PreCode = "C";
            Max.MaxNo = 1000;
            List.Add(Max);

            Max = new MaxInt();
            Max.MaxID = Guid.Empty;
            Max.AppName = MaxIntAppName.Supplier.ToString();
            Max.PreCode = "S";
            Max.MaxNo = 1000;
            List.Add(Max);

            Max = new MaxInt();
            Max.MaxID = Guid.Empty;
            Max.AppName = MaxIntAppName.SalesPlan.ToString();
            Max.PreCode = "P";
            Max.MaxNo = 1000;
            List.Add(Max);

            Max = new MaxInt();
            Max.MaxID = Guid.Empty;
            Max.AppName = MaxIntAppName.RFQ_Head.ToString();
            Max.PreCode = "R";
            Max.MaxNo = 1000;
            List.Add(Max);

            Max = new MaxInt();
            Max.MaxID = Guid.Empty;
            Max.AppName = MaxIntAppName.WMS_In.ToString();
            Max.PreCode = string.Empty;
            Max.MaxNo = 1000;
            List.Add(Max);

            Max = new MaxInt();
            Max.MaxID = Guid.Empty;
            Max.AppName = MaxIntAppName.WMS_Out_Doc.ToString();
            Max.PreCode = string.Empty;
            Max.MaxNo = 1000;
            List.Add(Max);

            List<MaxInt> List_DB = db.MaxInt.ToList();
            MaxInt Max_DB = new MaxInt();
            foreach (var App in List)
            {
                Max_DB = List_DB.Where(x => x.AppName == App.AppName).FirstOrDefault();
                if(Max_DB != null)
                {
                    App.MaxID = Max_DB.MaxID;
                    App.PreCode = Max_DB.PreCode;
                    App.MaxNo = Max_DB.MaxNo;
                }
            }
            return List;
        }

        public MaxInt GetMaxIntByApp(string AppName)
        {
            MaxInt M = db.MaxInt.Where(x => x.AppName == AppName).FirstOrDefault();
            if (M == null)
            {
                M = new MaxInt();
                M.MaxID = MyGUID.NewGUID();
                M.MaxNo = 1001;
                M.AppName = AppName;

                MaxInt De_Max = this.GetMaxIntList().Where(x => x.AppName == AppName).FirstOrDefault();
                if (De_Max == null)
                {
                    throw new Exception("自动编码器未匹配");
                }
                M.PreCode = De_Max.PreCode;
                db.MaxInt.Add(M);
                db.SaveChanges();
            }
            else
            {
                M.MaxNo = M.MaxNo + 1;
                db.Entry(M).State = EntityState.Modified;
                db.SaveChanges();
            }
            return M;
        }

        public MaxInt GetMaxIntByApp(string AppName,string Now_DT)
        {
            string AppName_DT = AppName + "-" + Now_DT;
            MaxInt M = db.MaxInt.Where(x => x.AppName == AppName_DT).FirstOrDefault();
            if (M == null)
            {
                M = new MaxInt();
                M.MaxID = MyGUID.NewGUID();
                M.MaxNo = 1;
                M.AppName = AppName_DT;

                MaxInt De_Max = GetMaxIntList().Where(x => x.AppName == AppName).FirstOrDefault();
                if (De_Max == null)
                {
                    throw new Exception("自动编码器未匹配");
                }
                M.PreCode = De_Max.PreCode;
                db.MaxInt.Add(M);
                db.SaveChanges();
            }
            else
            {
                M.MaxNo = M.MaxNo + 1;
                db.Entry(M).State = EntityState.Modified;
                db.SaveChanges();
            }
            return M;
        }
    }


}
