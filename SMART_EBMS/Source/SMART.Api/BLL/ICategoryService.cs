using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;

namespace SMART.Api
{
    public interface ICategoryService
    {
        CatTree Get_CatTree(Guid LinkMainCID);
        List<Category> Get_Category_List(Guid LinkMainCID);
        Category Get_Category_Item(Guid CatID);
        void Create_Category(Category Cat, Guid LinkMainCID);
        void Set_Category(Guid CatID, Category Cat);
        void Delete_Category(Guid CatID);
    }

    public partial class CategoryService : ICategoryService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class CategoryService : ICategoryService
    {
        public CatTree Get_CatTree(Guid LinkMainCID)
        {
            List<Category> ALL_Cat_List = db.Category.Where(x => x.LinkMainCID == LinkMainCID).ToList();
            List<Category> A_List = db.Category.Where(x => x.Level == 1).ToList();
            List<Category> B_List = db.Category.Where(x => x.Level == 2).ToList();
            List<Category> C_List = db.Category.Where(x => x.Level == 3).ToList();

            CatTree CT = new CatTree();
            CT.Top_List = new List<Cat>();
            Cat A_Cat = new Cat();
            Cat B_Cat = new Cat();
            Cat C_Cat = new Cat();

            Sub_Cat B_Sub_Cat = new Sub_Cat();
            foreach (var x in A_List.OrderBy(x=>x.Create_DT).ToList())
            {
                A_Cat = new Cat();
                A_Cat.CatID = x.CatID;
                A_Cat.Cat_Name = x.CatName;
                A_Cat.Sub_Cat_List = new List<Sub_Cat>();
                foreach(var xx in B_List.Where(xx=>xx.Parent_CatID == x.CatID).OrderBy(xx => xx.Create_DT).ToList())
                {
                    B_Sub_Cat = new Sub_Cat();
                    B_Sub_Cat.CatID = xx.CatID;
                    B_Sub_Cat.Cat_Name = xx.CatName;
                    B_Sub_Cat.Sub_Cat_List = new List<Cat>();
                    foreach(var xxx in C_List.Where(xxx => xxx.Parent_CatID == xx.CatID).OrderBy(xxx => xxx.Create_DT).ToList())
                    {
                        C_Cat = new Cat();
                        C_Cat.CatID = xxx.CatID;
                        C_Cat.Cat_Name = xxx.CatName;
                        B_Sub_Cat.Sub_Cat_List.Add(C_Cat);
                    }
                    A_Cat.Sub_Cat_List.Add(B_Sub_Cat);
                }
                CT.Top_List.Add(A_Cat);
            }
            return CT;
        }

        public List<Category> Get_Category_List(Guid LinkMainCID)
        {
            List<Category> ALL_Cat_List = db.Category.Where(x => x.LinkMainCID == LinkMainCID).ToList();
            List<Category> A_List = db.Category.Where(x => x.Level == 1).ToList();
            List<Category> B_List = db.Category.Where(x => x.Level == 2).ToList();
            List<Category> C_List = db.Category.Where(x => x.Level == 3).ToList();

            List<Category> CatList = new List<Category>();
            foreach (var x in A_List.OrderBy(x => x.Create_DT).ToList())
            {
                x.CatNamePath = x.CatName;
                CatList.Add(x);
                foreach (var xx in B_List.Where(xx => xx.Parent_CatID == x.CatID).OrderBy(xx => xx.Create_DT).ToList())
                {
                    xx.CatNamePath = x.CatName + "/" + xx.CatName;
                    CatList.Add(xx);
                    foreach (var xxx in C_List.Where(xxx => xxx.Parent_CatID == xx.CatID).OrderBy(xxx => xxx.Create_DT).ToList())
                    {
                        xxx.CatNamePath = x.CatName + "/" + xx.CatName + "/" + xxx.CatName;
                        CatList.Add(xxx);
                    }
                }
            }
            return CatList;
        }

        public Category Get_Category_Item(Guid CatID)
        {
            Category Cat = db.Category.Find(CatID);
            Cat = Cat == null ? new Category() : Cat;

            if(db.Category.Where(x => x.Parent_CatID == Cat.CatID).Any())
            {
                Cat.IsLockDelete = 1;
            }
            return Cat;
        }

        public void Create_Category(Category Cat, Guid LinkMainCID)
        {
            Cat.CatID = MyGUID.NewGUID();
            Cat.LinkMainCID = LinkMainCID;
            if (Cat.LinkMainCID == Guid.Empty) { throw new Exception("LinkMainCID Is Empty"); }

            Cat.CatName = Cat.CatName == null ? string.Empty : Cat.CatName.Trim();
            if (string.IsNullOrEmpty(Cat.CatName)) { throw new Exception("CatName Is Empty"); }

            Category Parent_Cat = new Category();
            Parent_Cat = db.Category.Where(x => x.LinkMainCID == LinkMainCID && x.CatID == Cat.Parent_CatID).FirstOrDefault();
            Parent_Cat = Parent_Cat == null ? new Category() : Parent_Cat;

            if (Parent_Cat.CatID != Guid.Empty)
            {
                Cat.Level = Parent_Cat.Level + 1;
            }
            else
            {
                Cat.Level = 1;
            }
           
            if (db.Category.Where(x => x.LinkMainCID == LinkMainCID && x.Parent_CatID == Cat.Parent_CatID && x.CatID != Cat.CatID && x.CatName == Cat.CatName).Any())
            {
                throw new Exception("同级目录名称重复");
            }

            Cat.Create_DT = DateTime.Now;
            Cat.LinkMainCID = LinkMainCID;

            db.Category.Add(Cat);
            MyDbSave.SaveChange(db);

        }

        public void Set_Category(Guid CatID, Category Cat)
        {
            Category OLD_Cat = db.Category.Find(CatID);
            OLD_Cat.CatName = Cat.CatName.Trim();
            if (db.Category.Where(x => x.LinkMainCID == OLD_Cat.LinkMainCID && x.Parent_CatID == OLD_Cat.Parent_CatID && x.CatID != OLD_Cat.CatID && x.CatName == OLD_Cat.CatName).Any())
            {
                throw new Exception("同级目录名称重复");
            }
            MyDbSave.SaveChange(db);

        }

        public void Delete_Category(Guid CatID)
        {
            Category Cat = db.Category.Find(CatID);
            if(db.Category.Where(x=>x.LinkMainCID == Cat.LinkMainCID && x.Parent_CatID == Cat.CatID).Any())
            {
                throw new Exception("请先删除子目录");
            }
            db.Category.Remove(Cat);

            foreach(var x in db.Material.Where(x=>x.LinkMainCID == Cat.LinkMainCID && x.CatID == Cat.CatID).ToList())
            {
                x.CatID = Guid.Empty;
            }

            MyDbSave.SaveChange(db);

        }
    }

}
