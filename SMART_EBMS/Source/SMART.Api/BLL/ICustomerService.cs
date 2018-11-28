using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;
using System.Data.Entity;

namespace SMART.Api
{
    public interface ICustomerService
    {
        PageList<Customer> Get_Customer_PageList(Customer_Filter MF);
        List<Customer> Get_Customer_List(Customer_Filter MF);
        Customer Get_Customer_Empty(Guid LinkMainCID);
        Customer Get_Customer_Item(Guid CID);

        Guid Create_Customer(Customer C, Guid LinkMainCID);
        void Set_Customer_Base(Guid CID, Customer C);
        void Delete_Customer(Guid CID);

        List<User> Get_User_By_Sales_List(Guid LinkMainCID);

        PageList<Customer_Group> Get_Customer_Group_PageList(Customer_Group_Filter MF);
        Customer_Group Get_Customer_Group_Empty();
        Customer_Group Get_Customer_Group_Item(Guid GID);
        Customer_Group Get_Customer_Group_DB(Guid GID);

        Guid Create_Customer_Group(Customer_Group CG, Guid LinkMainCID);
        void Set_Customer_Group_Base(Guid GID, Customer_Group CG);
        void Delete_Customer_Group(Guid GID);

        void Add_Customer_To_Group(Guid CID, Guid GID);
        void Delete_Customer_From_Group(Guid CID);

    }

    public partial class CustomerService : ICustomerService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class CustomerService : ICustomerService
    {
        public List<Customer> Get_Customer_List(Customer_Filter MF)
        {
            var query = db.Customer.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            if(MF.Sales_UID != Guid.Empty)
            {
                query = query.Where(x => x.Sales_UID == MF.Sales_UID || x.Sales_UID == Guid.Empty).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Cust_Name))
            {
                query = query.Where(x => x.Cust_Name.Contains(MF.Cust_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Cust_Name_Or_Code))
            {
                query = query.Where(x => x.Cust_Name.Contains(MF.Cust_Name_Or_Code) || x.Cust_Code.Contains(MF.Cust_Name_Or_Code)).AsQueryable();
            }

            return query.OrderBy(x => x.Cust_Name).Take(50).ToList();
        }

        public PageList<Customer> Get_Customer_PageList(Customer_Filter MF)
        {
            var query = db.Customer.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            List<Guid> SalesUIDList = query.Select(x => x.Sales_UID).ToList();
            SalesUIDList = SalesUIDList.Where(x => x != Guid.Empty).Distinct().ToList();

            if (!string.IsNullOrEmpty(MF.Cust_Name))
            {
                query = query.Where(x => x.Cust_Name.Contains(MF.Cust_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Cust_Code))
            {
                query = query.Where(x => x.Cust_Code.Contains(MF.Cust_Code)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Cust_Type))
            {
                query = query.Where(x => x.Cust_Type == MF.Cust_Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.District))
            {
                query = query.Where(x => x.District == MF.District).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Industry))
            {
                query = query.Where(x => x.Industry == MF.Industry).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.FormBy))
            {
                query = query.Where(x => x.FormBy == MF.FormBy).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Sales_Type))
            {
                query = query.Where(x => x.Sales_Type == MF.Sales_Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Settle_Accounts))
            {
                query = query.Where(x => x.Settle_Accounts == MF.Settle_Accounts).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Payment_Type))
            {
                query = query.Where(x => x.Payment_Type == MF.Payment_Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Statement_Day))
            {
                query = query.Where(x => x.Statement_Day == MF.Statement_Day).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Contract_Type))
            {
                query = query.Where(x => x.Contract_Type == MF.Contract_Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Ship_Type))
            {
                query = query.Where(x => x.Ship_Type == MF.Ship_Type).AsQueryable();
            }

            if (MF.Sales_UID != Guid.Empty)
            {
                query = query.Where(x => x.Sales_UID == MF.Sales_UID).AsQueryable();
            }

            List<Customer> RowsList = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<User> UList = db.User.Where(x => x.LinkMainCID == MF.LinkMainCID).ToList();
            User SalesUser = new User();
            foreach(var x in RowsList)
            {
                SalesUser = UList.Where(c => c.UID == x.Sales_UID).FirstOrDefault();
                x.Sales_UID_Name = SalesUser != null ? SalesUser.UserFullName : string.Empty;
            }

            MF.SalesUserList = UList.Where(x => SalesUIDList.Contains(x.UID)).OrderBy(x=>x.UserFullName).ToList();

            PageList<Customer> PList = new PageList<Customer>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowsList;
            return PList;
        }

        public Customer Get_Customer_Empty(Guid LinkMainCID)
        {
            Customer C = new Customer();
            C.Sales_User_List = this.Get_User_By_Sales_List(LinkMainCID);
            return C;
        }

        public Customer Get_Customer_Item(Guid CID)
        {
            Customer C = db.Customer.Find(CID);
            C = C == null ? new Customer() : C;

            User SalesUser = db.User.Find(C.Sales_UID);
            SalesUser = SalesUser == null ? new User() : SalesUser;

            C.Sales_User_List = new List<User>();
            if (SalesUser.UID != Guid.Empty)
            {
                C.Sales_User_List.Add(SalesUser);
            }

            C.Sales_User_List.AddRange(this.Get_User_By_Sales_List(C.LinkMainCID).Where(x => x.UID != SalesUser.UID));
            C.Sales_UID_Name = SalesUser.UserFullName;
            return C;
        }

        public Guid Create_Customer(Customer C, Guid LinkMainCID)
        {
            C.CID = MyGUID.NewGUID();
            C.Create_DT = DateTime.Now;

            C.Cust_Code = C.Cust_Code == null ? string.Empty : C.Cust_Code.Trim();
            C.Cust_Name = C.Cust_Name == null ? string.Empty : C.Cust_Name.Trim();
            //C.District = C.District == null ? string.Empty : C.District.Trim();
            //C.Industry = C.Industry == null ? string.Empty : C.Industry.Trim();
            //C.Cust_Type = C.Cust_Type == null ? string.Empty : C.Cust_Type.Trim();
            //C.VIP_Type = C.VIP_Type == null ? string.Empty : C.VIP_Type.Trim();
            //C.Cust_Trade_Type = C.Cust_Trade_Type == null ? string.Empty : C.Cust_Trade_Type.Trim();
            //C.FormBy = C.FormBy == null ? string.Empty : C.FormBy.Trim();
            //C.Establishment = C.Establishment == null ? string.Empty : C.Establishment.Trim();
            //C.Statement_Day = C.Statement_Day == null ? string.Empty : C.Statement_Day.Trim();
            //C.Payment_Type = C.Payment_Type == null ? string.Empty : C.Payment_Type.Trim();
            //C.Sales_Type = C.Sales_Type == null ? string.Empty : C.Sales_Type.Trim();
            //C.Settle_Accounts = C.Settle_Accounts == null ? string.Empty : C.Settle_Accounts.Trim();
            //C.Contract_Type = C.Contract_Type == null ? string.Empty : C.Contract_Type.Trim();
            //C.Ship_Address = C.Ship_Address == null ? string.Empty : C.Ship_Address.Trim();
            //C.Ship_Type = C.Ship_Type == null ? string.Empty : C.Ship_Type.Trim();
            //C.Ship_Pay = C.Ship_Pay == null ? string.Empty : C.Ship_Pay.Trim();
            //C.Cust_Product = C.Cust_Product == null ? string.Empty : C.Cust_Product.Trim();
            //C.Compete_Brand = C.Compete_Brand == null ? string.Empty : C.Compete_Brand.Trim();
            //C.Compete_Company = C.Compete_Company == null ? string.Empty : C.Compete_Company.Trim();
            //C.Invoice_Title = C.Invoice_Title == null ? string.Empty : C.Invoice_Title.Trim();
            //C.Invoice_Identification = C.Invoice_Identification == null ? string.Empty : C.Invoice_Identification.Trim();
            //C.Invoice_Address = C.Invoice_Address == null ? string.Empty : C.Invoice_Address.Trim();
            //C.Invoice_Tel = C.Invoice_Tel == null ? string.Empty : C.Invoice_Tel.Trim();
            //C.Bank_Name = C.Bank_Name == null ? string.Empty : C.Bank_Name.Trim();
            //C.Bank_Account = C.Bank_Account == null ? string.Empty : C.Bank_Account.Trim();

            //C.Buyer = C.Buyer == null ? string.Empty : C.Buyer.Trim();
            //C.Buyer_Tel = C.Buyer_Tel == null ? string.Empty : C.Buyer_Tel.Trim();
            //C.Buyer_Fax = C.Buyer_Fax == null ? string.Empty : C.Buyer_Fax.Trim();
            //C.Buyer_Mail = C.Buyer_Mail == null ? string.Empty : C.Buyer_Mail.Trim();

            //C.Keeper = C.Keeper == null ? string.Empty : C.Keeper.Trim();
            //C.Keeper_Tel = C.Keeper_Tel == null ? string.Empty : C.Keeper_Tel.Trim();
            //C.Keeper_Fax = C.Keeper_Fax == null ? string.Empty : C.Keeper_Fax.Trim();
            //C.Keeper_Mail = C.Keeper_Mail == null ? string.Empty : C.Keeper_Mail.Trim();

            //C.Finance = C.Finance == null ? string.Empty : C.Finance.Trim();
            //C.Finance_Tel = C.Finance_Tel == null ? string.Empty : C.Finance_Tel.Trim();
            //C.Finance_Fax = C.Finance_Fax == null ? string.Empty : C.Finance_Fax.Trim();
            //C.Finance_Mail = C.Finance_Mail == null ? string.Empty : C.Finance_Mail.Trim();

            //C.GM = C.GM == null ? string.Empty : C.GM.Trim();
            //C.GM_Tel = C.GM_Tel == null ? string.Empty : C.GM_Tel.Trim();
            //C.GM_Fax = C.GM_Fax == null ? string.Empty : C.GM_Fax.Trim();
            //C.GM_Mail = C.GM_Mail == null ? string.Empty : C.GM_Mail.Trim();

            //C.Sales_UID = C.Sales_UID == null ? Guid.Empty : C.Sales_UID;
            C.LinkMainCID = LinkMainCID;

            this.Check_Customer_Base(C);
            db.Customer.Add(C);
            MyDbSave.SaveChange(db);
            return C.CID;
        }

        public void Set_Customer_Base(Guid CID, Customer C)
        {
            Customer OLD_C = db.Customer.Find(CID);

            OLD_C.Cust_Code = C.Cust_Code == null ? string.Empty : C.Cust_Code.Trim();
            OLD_C.Cust_Name = C.Cust_Name == null ? string.Empty : C.Cust_Name.Trim();
            //OLD_C.District = C.District == null ? string.Empty : C.District.Trim();
            //OLD_C.Industry = C.Industry == null ? string.Empty : C.Industry.Trim();
            //OLD_C.Cust_Type = C.Cust_Type == null ? string.Empty : C.Cust_Type.Trim();
            //OLD_C.VIP_Type = C.VIP_Type == null ? string.Empty : C.VIP_Type.Trim();
            //OLD_C.Cust_Trade_Type = C.Cust_Trade_Type == null ? string.Empty : C.Cust_Trade_Type.Trim();
            //OLD_C.FormBy = C.FormBy == null ? string.Empty : C.FormBy.Trim();
            //OLD_C.Establishment = C.Establishment == null ? string.Empty : C.Establishment.Trim();
            //OLD_C.Statement_Day = C.Statement_Day == null ? string.Empty : C.Statement_Day.Trim();
            //OLD_C.Payment_Type = C.Payment_Type == null ? string.Empty : C.Payment_Type.Trim();
            //OLD_C.Sales_Type = C.Sales_Type == null ? string.Empty : C.Sales_Type.Trim();
            //OLD_C.Settle_Accounts = C.Settle_Accounts == null ? string.Empty : C.Settle_Accounts.Trim();
            //OLD_C.Contract_Type = C.Contract_Type == null ? string.Empty : C.Contract_Type.Trim();
            //OLD_C.Contract_End_Date = C.Contract_End_Date;

            //OLD_C.Ship_Address = C.Ship_Address == null ? string.Empty : C.Ship_Address.Trim();
            //OLD_C.Ship_Type = C.Ship_Type == null ? string.Empty : C.Ship_Type.Trim();
            //OLD_C.Ship_Pay = C.Ship_Pay == null ? string.Empty : C.Ship_Pay.Trim();
            //OLD_C.Cust_Product = C.Cust_Product == null ? string.Empty : C.Cust_Product.Trim();
            //OLD_C.Compete_Brand = C.Compete_Brand == null ? string.Empty : C.Compete_Brand.Trim();
            //OLD_C.Compete_Company = C.Compete_Company == null ? string.Empty : C.Compete_Company.Trim();
            //OLD_C.Invoice_Title = C.Invoice_Title == null ? string.Empty : C.Invoice_Title.Trim();
            //OLD_C.Invoice_Identification = C.Invoice_Identification == null ? string.Empty : C.Invoice_Identification.Trim();
            //OLD_C.Invoice_Address = C.Invoice_Address == null ? string.Empty : C.Invoice_Address.Trim();
            //OLD_C.Invoice_Tel = C.Invoice_Tel == null ? string.Empty : C.Invoice_Tel.Trim();
            //OLD_C.Bank_Name = C.Bank_Name == null ? string.Empty : C.Bank_Name.Trim();
            //OLD_C.Bank_Account = C.Bank_Account == null ? string.Empty : C.Bank_Account.Trim();

            //OLD_C.Buyer = C.Buyer == null ? string.Empty : C.Buyer.Trim();
            //OLD_C.Buyer_Tel = C.Buyer_Tel == null ? string.Empty : C.Buyer_Tel.Trim();
            //OLD_C.Buyer_Fax = C.Buyer_Fax == null ? string.Empty : C.Buyer_Fax.Trim();
            //OLD_C.Buyer_Mail = C.Buyer_Mail == null ? string.Empty : C.Buyer_Mail.Trim();

            //OLD_C.Keeper = C.Keeper == null ? string.Empty : C.Keeper.Trim();
            //OLD_C.Keeper_Tel = C.Keeper_Tel == null ? string.Empty : C.Keeper_Tel.Trim();
            //OLD_C.Keeper_Fax = C.Keeper_Fax == null ? string.Empty : C.Keeper_Fax.Trim();
            //OLD_C.Keeper_Mail = C.Keeper_Mail == null ? string.Empty : C.Keeper_Mail.Trim();

            //OLD_C.Finance = C.Finance == null ? string.Empty : C.Finance.Trim();
            //OLD_C.Finance_Tel = C.Finance_Tel == null ? string.Empty : C.Finance_Tel.Trim();
            //OLD_C.Finance_Fax = C.Finance_Fax == null ? string.Empty : C.Finance_Fax.Trim();
            //OLD_C.Finance_Mail = C.Finance_Mail == null ? string.Empty : C.Finance_Mail.Trim();

            //OLD_C.GM = C.GM == null ? string.Empty : C.GM.Trim();
            //OLD_C.GM_Tel = C.GM_Tel == null ? string.Empty : C.GM_Tel.Trim();
            //OLD_C.GM_Fax = C.GM_Fax == null ? string.Empty : C.GM_Fax.Trim();
            //OLD_C.GM_Mail = C.GM_Mail == null ? string.Empty : C.GM_Mail.Trim();

            //OLD_C.Sales_UID = C.Sales_UID == null ? Guid.Empty : C.Sales_UID;
            this.Check_Customer_Base(OLD_C);
            db.Entry(OLD_C).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_Customer(Guid CID)
        {
            Customer OLD_C = db.Customer.Find(CID);
            db.Customer.Remove(OLD_C);
            MyDbSave.SaveChange(db);
        }

        private void Check_Customer_Base(Customer C)
        {
            if(string.IsNullOrEmpty(C.Cust_Code))
            {
                throw new Exception("客户代码未填写!");
            }

            if (string.IsNullOrEmpty(C.Cust_Name))
            {
                throw new Exception("客户名称未填写!");
            }

            var query = db.Customer.Where(x => x.LinkMainCID == C.LinkMainCID && x.CID != C.CID).AsQueryable();

            List<string> Cust_Code_List = query.Select(x => x.Cust_Code).ToList();
            if (Cust_Code_List.Where(x=>x == C.Cust_Code).Any())
            {
                throw new Exception("客户代码重复!");
            }

            List<string> Cust_Name_List = query.Select(x => x.Cust_Name).ToList();
            List<string> Cust_Name_List_Rep = new List<string>();
            foreach (var Name in Cust_Name_List)
            {
                Cust_Name_List_Rep.Add(CommonLib.CompanyName_Replace_and_ToLower(Name));
            }

            string Cust_Name_Rep = CommonLib.CompanyName_Replace_and_ToLower(C.Cust_Name);
            if (Cust_Name_List_Rep.Where(x => x == Cust_Name_Rep).Any())
            {
                throw new Exception("客户名称重复!");
            }
        }

        public List<User> Get_User_By_Sales_List(Guid LinkMainCID)
        {
            List<User> List = db.User.Where(x => x.LinkMainCID == LinkMainCID && x.RoleTitle == User_RoleTitle_Emun.销售经理.ToString()).ToList();
            List = List.OrderBy(x => x.UserFullName).ToList();
            return List;
        }
    }

    public partial class CustomerService : ICustomerService
    {
        public PageList<Customer_Group> Get_Customer_Group_PageList(Customer_Group_Filter MF)
        {
            var query = db.Customer_Group.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            
            if (!string.IsNullOrEmpty(MF.Cust_Group_Name))
            {
                query = query.Where(x => x.Group_Name.Contains(MF.Cust_Group_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Cust_Group_Code))
            {
                query = query.Where(x => x.Group_Code.Contains(MF.Cust_Group_Code)).AsQueryable();
            }
           
            List<Customer_Group> RowList = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            List<Guid> Guid_List = RowList.Select(x => x.GID).ToList();

            List<Guid> Cust_ID_List = db.Customer.Where(x => x.LinkMainCID == MF.LinkMainCID && Guid_List.Contains(x.Link_GID)).Select(x=>x.Link_GID).ToList();

            foreach (var x in RowList)
            {
                x.Cust_List_Count = Cust_ID_List.Where(c => c == x.GID).Count();
            }

            PageList<Customer_Group> PList = new PageList<Customer_Group>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = RowList.Count();
            PList.Rows = RowList;
            return PList;
        }

        public Customer_Group Get_Customer_Group_Empty()
        {
            Customer_Group CG = new Customer_Group();
            return CG;
        }

        public Customer_Group Get_Customer_Group_Item(Guid GID)
        {
            Customer_Group CG = db.Customer_Group.Find(GID);
            CG = CG == null ? new Customer_Group() : CG;
            CG.Cust_List = db.Customer.Where(x => x.LinkMainCID == CG.LinkMainCID && x.Link_GID == CG.GID).ToList();
            return CG;
        }

        public Customer_Group Get_Customer_Group_DB(Guid GID)
        {
            Customer_Group CG = db.Customer_Group.Find(GID);
            CG = CG == null ? new Customer_Group() : CG;
            return CG;
        }

        public Guid Create_Customer_Group(Customer_Group CG, Guid LinkMainCID)
        {
            CG.GID = MyGUID.NewGUID();
            CG.Create_DT = DateTime.Now;

            CG.Group_Code = CG.Group_Code == null ? string.Empty : CG.Group_Code.Trim();
            CG.Group_Name = CG.Group_Name == null ? string.Empty : CG.Group_Name.Trim();

            CG.LinkMainCID = LinkMainCID;

            this.Check_Customer_Group_Base(CG);
            db.Customer_Group.Add(CG);
            MyDbSave.SaveChange(db);
            return CG.GID;
        }

        public void Set_Customer_Group_Base(Guid GID, Customer_Group CG)
        {
            Customer_Group OLD_C = db.Customer_Group.Find(GID);

            OLD_C.Group_Code = CG.Group_Code == null ? string.Empty : CG.Group_Code.Trim();
            OLD_C.Group_Name = CG.Group_Name == null ? string.Empty : CG.Group_Name.Trim();
           
            this.Check_Customer_Group_Base(OLD_C);
            db.Entry(OLD_C).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_Customer_Group(Guid GID)
        {
            Customer_Group CG = db.Customer_Group.Find(GID);

            if (db.Customer.Where(x => x.LinkMainCID == CG.LinkMainCID && x.Link_GID == CG.GID).Any())
            {
                throw new Exception("该集团已关联客户，不支持删除！");
            }

            db.Customer_Group.Remove(CG);
            MyDbSave.SaveChange(db);
        }

        private void Check_Customer_Group_Base(Customer_Group CG)
        {
            if (string.IsNullOrEmpty(CG.Group_Code))
            {
                throw new Exception("集团代码未填写!");
            }

            if (string.IsNullOrEmpty(CG.Group_Name))
            {
                throw new Exception("集团名称未填写!");
            }

            var query = db.Customer_Group.Where(x => x.LinkMainCID == CG.LinkMainCID && x.GID != CG.GID).AsQueryable();

            List<string> Code_List = query.Select(x => x.Group_Code).ToList();
            if (Code_List.Where(x => x == CG.Group_Code).Any())
            {
                throw new Exception("集团代码重复!");
            }

            List<string> Name_List = query.Select(x => x.Group_Name).ToList();
            List<string> Name_List_Rep = new List<string>();
            foreach (var Name in Name_List)
            {
                Name_List_Rep.Add(CommonLib.CompanyName_Replace_and_ToLower(Name));
            }

            string Name_Rep = CommonLib.CompanyName_Replace_and_ToLower(CG.Group_Name);
            if (Name_List_Rep.Where(x => x == Name_Rep).Any())
            {
                throw new Exception("集团名称重复!");
            }
        }

        public void Add_Customer_To_Group(Guid CID, Guid GID)
        {
            Customer C = db.Customer.Find(CID);

            if (db.Customer.Where(x => x.Link_GID == GID && x.CID == CID).Any())
            {
                throw new Exception("该客户已添加到本集团！");
            }

            if (C == null)
            {
                throw new Exception("该客户不存在！");
            }
            else
            {
                C.Link_GID = GID;
                db.Entry(C).State = EntityState.Modified;
            }
            MyDbSave.SaveChange(db);
        }

        public void Delete_Customer_From_Group(Guid CID)
        {
            Customer C = db.Customer.Find(CID);
            if (C == null)
            {
                throw new Exception("该客户不存在！");
            }
            else
            {
                C.Link_GID = Guid.Empty;
                db.Entry(C).State = EntityState.Modified;
            }
            MyDbSave.SaveChange(db);
        }
    }
}
 