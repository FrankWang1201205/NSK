using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;
using System.Data.Entity;

namespace SMART.Api
{
    public interface IBrandService
    {
        List<string> Get_Brand_Name_List(Guid LinkMainCID);
        PageList<Brand> Get_Brand_PageList(Brand_Filter MF);
        List<Brand> Get_Brand_List(Guid LinkMainCID);
        Brand Get_Brand_Item(Guid BID);
        Brand Get_Brand_DB(Guid BID);

        Guid Create_Brand(Brand B, Guid UID);
        void Set_Brand_Item(Guid BID, Brand B);
        void Upload_Logo(Guid BID, string Logo);
        void Delete_Logo(Guid BID);
        void Upload_Certificate(Guid BID, string Certificate);
        void Delete_Certificate(Guid BID);
        void Delete_Brand(Guid BID);
    }

    public partial class BrandService : IBrandService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class BrandService : IBrandService
    {
        public List<string> Get_Brand_Name_List(Guid LinkMainCID)
        {
           return db.Brand.Where(x => x.LinkMainCID == LinkMainCID).OrderBy(x => x.Create_DT).Select(x => x.Brand_Name).ToList();
        }

        public PageList<Brand> Get_Brand_PageList(Brand_Filter MF)
        {
            var query = db.Brand.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.Brand_Name.Contains(MF.Keyword)).AsQueryable();
            }

            List<Brand> RowsList = query.OrderBy(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<Brand> PList = new PageList<Brand>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowsList;
            return PList;
        }

        public List<Brand> Get_Brand_List(Guid LinkMainCID)
        {
            var query = db.Brand.Where(x => x.LinkMainCID == LinkMainCID).AsQueryable();
            return query.OrderBy(x => x.Create_DT).ToList();
        }

        public Brand Get_Brand_DB(Guid BID)
        {
            Brand B = db.Brand.Find(BID);
            B = B == null ? new Brand() : B;
            return B;
        }

        public Brand Get_Brand_Item(Guid BID)
        {
            Brand B = db.Brand.Find(BID);
            B = B == null ? new Brand() : B;

            if(db.Material.Where(x=>x.LinkMainCID == B.LinkMainCID && x.MatBrand == B.Brand_Name).Any())
            {
                B.IsLock_Delete = 1;
            }
            return B;
        }

        public void Set_Brand_Item(Guid BID, Brand B)
        {
            Brand OLD_B = db.Brand.Find(BID);

            if(db.Material.Where(x => x.LinkMainCID == B.LinkMainCID && x.MatBrand == B.Brand_Name).Any() == false)
            {
                OLD_B.Brand_Name = B.Brand_Name == null ? string.Empty : B.Brand_Name.Trim();
            }

            OLD_B.Summary = B.Summary == null ? string.Empty : B.Summary.Trim();
            this.Check_Brand_Item(OLD_B);

            db.Entry(OLD_B).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Upload_Logo(Guid BID, string Logo)
        {
            Brand OLD_B = db.Brand.Find(BID);
            OLD_B.Logo = Logo == null ? string.Empty : Logo;
            db.Entry(OLD_B).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_Logo(Guid BID)
        {
            Brand OLD_B = db.Brand.Find(BID);
            OLD_B.Logo = string.Empty;
            db.Entry(OLD_B).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Upload_Certificate(Guid BID, string Certificate)
        {
            Brand OLD_B = db.Brand.Find(BID);
            OLD_B.Certificate = Certificate == null ? string.Empty : Certificate;
            db.Entry(OLD_B).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_Certificate(Guid BID)
        {
            Brand OLD_B = db.Brand.Find(BID);
            OLD_B.Certificate = string.Empty;
            db.Entry(OLD_B).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }
 
        public Guid Create_Brand(Brand B, Guid UID)
        {
            User U = db.User.Find(UID);
            if(U == null)
            {
                throw new Exception("User Is Null");
            }

            B.BID = MyGUID.NewGUID();
            B.LinkMainCID = U.LinkMainCID;
            B.Brand_Name = B.Brand_Name == null ? string.Empty : B.Brand_Name.Trim();
            B.Summary = B.Summary == null ? string.Empty : B.Summary.Trim();
            B.Logo = B.Logo == null ? string.Empty : B.Logo;
            B.Certificate = string.Empty;
            B.Create_DT = DateTime.Now;
            B.Create_Person = U.UserFullName;

            this.Check_Brand_Item(B);
            db.Brand.Add(B);
            MyDbSave.SaveChange(db);
            return B.BID;
        }

        public void Delete_Brand(Guid BID)
        {
            Brand B = db.Brand.Find(BID);
            if (db.Material.Where(x => x.MatBrand == B.Brand_Name && x.LinkMainCID == B.LinkMainCID).Any())
            {
                throw new Exception("此品牌已关联产品，无法删除");
            }
            db.Brand.Remove(B);
            MyDbSave.SaveChange(db);
        }

        private void Check_Brand_Item(Brand B)
        {
            if (B.LinkMainCID == Guid.Empty)
            {
                throw new Exception("MainCID Is Empty");
            }

            if(string.IsNullOrEmpty(B.Brand_Name))
            {
                throw new Exception("品牌名称不能为空");
            }

            if (db.Brand.Where(x=>x.LinkMainCID == B.LinkMainCID && x.Brand_Name == B.Brand_Name && x.BID != B.BID).Any())
            {
                throw new Exception("品牌名称重复");
            }
        }

    }

}
