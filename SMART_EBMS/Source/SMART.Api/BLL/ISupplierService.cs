using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;
using System.Data.Entity;

namespace SMART.Api
{
    public interface ISupplierService
    {
        List<Supplier> Get_Supplier_List(Supplier_Filter MF);
        PageList<Supplier> Get_Supplier_PageList(Supplier_Filter MF);
        Supplier Get_Supplier_Empty();
        Supplier Get_Supplier_Item(Guid SupID);
        Guid Create_Supplier(Supplier Sup, Guid LinkMainCID);
        void Set_Supplier_Base(Guid SupID, Supplier Sup);
        void Delete_Supplier(Guid SupID);
    }

    public partial class SupplierService : ISupplierService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class SupplierService : ISupplierService
    {
        public List<Supplier> Get_Supplier_List(Supplier_Filter MF)
        {
            var query = db.Supplier.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.SupplierCode.Contains(MF.Keyword) || x.Sup_Name.Contains(MF.Keyword)).AsQueryable();
            }
            return query.OrderBy(x => x.Sup_Name).ToList();
        }

        public PageList<Supplier> Get_Supplier_PageList(Supplier_Filter MF)
        {
            var query = db.Supplier.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.SupplierCode.Contains(MF.Keyword) || x.Sup_Name.Contains(MF.Keyword)).AsQueryable();
            }

            if(!string.IsNullOrEmpty(MF.Type))
            {
                query = query.Where(x => x.Type == MF.Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Act_Status))
            {
                query = query.Where(x => x.Act_Status == MF.Act_Status).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Qualification))
            {
                query = query.Where(x => x.Qualification == MF.Qualification).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Sup_Level))
            {
                query = query.Where(x => x.Sup_Level == MF.Sup_Level).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Sup_Brand))
            {
                query = query.Where(x => x.Main_Brand_Json.Contains(MF.Sup_Brand)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.District))
            {
                query = query.Where(x => x.District == MF.District).AsQueryable();
            }

            List<Supplier> RowsList = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<Supplier> PList = new PageList<Supplier>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowsList;
            return PList;
        }

        public Supplier Get_Supplier_Empty()
        {
            Supplier Sup = new Supplier();
            return Sup;
        }

        public Supplier Get_Supplier_Item(Guid SupID)
        {
            Supplier Sup = db.Supplier.Find(SupID);
            Sup = Sup == null ? new Supplier() : Sup;
            Sup.Main_Brand_List_Json = CommonLib.StringListStrToStringArray(Sup.Main_Brand_Json);
            Sup.Main_Business_List_Json = CommonLib.StringListStrToStringArray(Sup.Main_Business_Json);
            Sup.Payment_List_Json = CommonLib.StringListStrToStringArray(Sup.Payment_Json);
            Sup.Settle_Accounts_List_Json = CommonLib.StringListStrToStringArray(Sup.Settle_Accounts_Json);

            return Sup;
        }

        public Guid Create_Supplier(Supplier Sup, Guid UID)
        {
            User U = db.User.Find(UID);
            if(U == null)
            {
                throw new Exception("User Is Null");
            }

            Sup.SupID = MyGUID.NewGUID();
            Sup.Create_DT = DateTime.Now;
            Sup.Create_Person = U.UserFullName;
            Sup.SupplierCode = Sup.SupplierCode == null ? string.Empty : Sup.SupplierCode.Trim();
            Sup.Sup_Name = Sup.Sup_Name == null ? string.Empty : Sup.Sup_Name.Trim();
            Sup.Sup_Short_Name = Sup.Sup_Short_Name == null ? string.Empty : Sup.Sup_Short_Name.Trim();
            Sup.District = Sup.District == null ? string.Empty : Sup.District.Trim();
            Sup.Address = Sup.Address == null ? string.Empty : Sup.Address.Trim();
            Sup.Qualification = Sup.Qualification == null ? string.Empty : Sup.Qualification.Trim();
            Sup.Type = Sup.Type == null ? string.Empty : Sup.Type.Trim();
            Sup.Sup_Level = Sup.Sup_Level == null ? string.Empty : Sup.Sup_Level.Trim();
            Sup.Tax_Rate = Sup.Tax_Rate == null ? string.Empty : Sup.Tax_Rate.Trim();

            Sup.Main_Business_Json = Sup.Main_Business_Json == null ? string.Empty : Sup.Main_Business_Json.Trim();
            Sup.Main_Brand_Json = Sup.Main_Brand_Json == null ? string.Empty : Sup.Main_Brand_Json.Trim();
            Sup.Payment_Json = Sup.Payment_Json == null ? string.Empty : Sup.Payment_Json.Trim();
            Sup.Settle_Accounts_Json = Sup.Settle_Accounts_Json == null ? string.Empty : Sup.Settle_Accounts_Json.Trim();

            Sup.Invoice_Title = Sup.Invoice_Title == null ? string.Empty : Sup.Invoice_Title.Trim();
            Sup.Invoice_Identification = Sup.Invoice_Identification == null ? string.Empty : Sup.Invoice_Identification.Trim();
            Sup.Invoice_Address = Sup.Invoice_Address == null ? string.Empty : Sup.Invoice_Address.Trim();
            Sup.Invoice_Tel = Sup.Invoice_Tel == null ? string.Empty : Sup.Invoice_Tel.Trim();
            Sup.Bank_Name = Sup.Bank_Name == null ? string.Empty : Sup.Bank_Name.Trim();
            Sup.Bank_Account = Sup.Bank_Account == null ? string.Empty : Sup.Bank_Account.Trim();

            Sup.Sales_Man = Sup.Sales_Man == null ? string.Empty : Sup.Sales_Man.Trim();
            Sup.Sales_Man_Tel = Sup.Sales_Man_Tel == null ? string.Empty : Sup.Sales_Man_Tel.Trim();
            Sup.Sales_Man_Fax = Sup.Sales_Man_Fax == null ? string.Empty : Sup.Sales_Man_Fax.Trim();
            Sup.Sales_Man_Mail = Sup.Sales_Man_Mail == null ? string.Empty : Sup.Sales_Man_Mail.Trim();

            Sup.Sales_Manager = Sup.Sales_Manager == null ? string.Empty : Sup.Sales_Manager.Trim();
            Sup.Sales_Manager_Tel = Sup.Sales_Manager_Tel == null ? string.Empty : Sup.Sales_Manager_Tel.Trim();
            Sup.Sales_Manager_Fax = Sup.Sales_Manager_Fax == null ? string.Empty : Sup.Sales_Manager_Fax.Trim();
            Sup.Sales_Manager_Mail = Sup.Sales_Manager_Mail == null ? string.Empty : Sup.Sales_Manager_Mail.Trim();

            Sup.Finance = Sup.Finance == null ? string.Empty : Sup.Finance.Trim();
            Sup.Finance_Tel = Sup.Finance_Tel == null ? string.Empty : Sup.Finance_Tel.Trim();
            Sup.Finance_Fax = Sup.Finance_Fax == null ? string.Empty : Sup.Finance_Fax.Trim();
            Sup.Finance_Mail = Sup.Finance_Mail == null ? string.Empty : Sup.Finance_Mail.Trim();

            Sup.GM = Sup.GM == null ? string.Empty : Sup.GM.Trim();
            Sup.GM_Tel = Sup.GM_Tel == null ? string.Empty : Sup.GM_Tel.Trim();
            Sup.GM_Fax = Sup.GM_Fax == null ? string.Empty : Sup.GM_Fax.Trim();
            Sup.GM_Mail = Sup.GM_Mail == null ? string.Empty : Sup.GM_Mail.Trim();

            Sup.Act_Status = Sup_Act_Status_Enum.OPEN.ToString();

            Sup.Remark = Sup.Remark == null ? string.Empty : Sup.Remark.Trim();
            Sup.LinkMainCID = U.LinkMainCID;

            this.Check_Supplier_Base(Sup);

            db.Supplier.Add(Sup);
            MyDbSave.SaveChange(db);
            return Sup.SupID;
        }

        public void Set_Supplier_Base(Guid SupID, Supplier Sup)
        {
            Supplier OLD_Sup = db.Supplier.Find(SupID);

            OLD_Sup.SupplierCode = Sup.SupplierCode == null ? string.Empty : Sup.SupplierCode.Trim();
            OLD_Sup.Sup_Short_Name = Sup.Sup_Short_Name == null ? string.Empty : Sup.Sup_Short_Name.Trim();
            OLD_Sup.Sup_Name = Sup.Sup_Name == null ? string.Empty : Sup.Sup_Name.Trim();
            OLD_Sup.District = Sup.District == null ? string.Empty : Sup.District.Trim();
            OLD_Sup.Address = Sup.Address == null ? string.Empty : Sup.Address.Trim();
            OLD_Sup.Qualification = Sup.Qualification == null ? string.Empty : Sup.Qualification.Trim();
            OLD_Sup.Type = Sup.Type == null ? string.Empty : Sup.Type.Trim();

            OLD_Sup.Sup_Level = Sup.Sup_Level == null ? string.Empty : Sup.Sup_Level.Trim();
            OLD_Sup.Tax_Rate = Sup.Tax_Rate == null ? string.Empty : Sup.Tax_Rate.Trim();

            OLD_Sup.Main_Business_Json = Sup.Main_Business_Json == null ? string.Empty : Sup.Main_Business_Json.Trim();
            OLD_Sup.Main_Brand_Json = Sup.Main_Brand_Json == null ? string.Empty : Sup.Main_Brand_Json.Trim();
            OLD_Sup.Payment_Json = Sup.Payment_Json == null ? string.Empty : Sup.Payment_Json.Trim();
            OLD_Sup.Settle_Accounts_Json = Sup.Settle_Accounts_Json == null ? string.Empty : Sup.Settle_Accounts_Json.Trim();

            OLD_Sup.Invoice_Title = Sup.Invoice_Title == null ? string.Empty : Sup.Invoice_Title.Trim();
            OLD_Sup.Invoice_Identification = Sup.Invoice_Identification == null ? string.Empty : Sup.Invoice_Identification.Trim();
            OLD_Sup.Invoice_Address = Sup.Invoice_Address == null ? string.Empty : Sup.Invoice_Address.Trim();
            OLD_Sup.Invoice_Tel = Sup.Invoice_Tel == null ? string.Empty : Sup.Invoice_Tel.Trim();
            OLD_Sup.Bank_Name = Sup.Bank_Name == null ? string.Empty : Sup.Bank_Name.Trim();
            OLD_Sup.Bank_Account = Sup.Bank_Account == null ? string.Empty : Sup.Bank_Account.Trim();

            OLD_Sup.Sales_Man = Sup.Sales_Man == null ? string.Empty : Sup.Sales_Man.Trim();
            OLD_Sup.Sales_Man_Tel = Sup.Sales_Man_Tel == null ? string.Empty : Sup.Sales_Man_Tel.Trim();
            OLD_Sup.Sales_Man_Fax = Sup.Sales_Man_Fax == null ? string.Empty : Sup.Sales_Man_Fax.Trim();
            OLD_Sup.Sales_Man_Mail = Sup.Sales_Man_Mail == null ? string.Empty : Sup.Sales_Man_Mail.Trim();

            OLD_Sup.Sales_Manager = Sup.Sales_Manager == null ? string.Empty : Sup.Sales_Manager.Trim();
            OLD_Sup.Sales_Manager_Tel = Sup.Sales_Manager_Tel == null ? string.Empty : Sup.Sales_Manager_Tel.Trim();
            OLD_Sup.Sales_Manager_Fax = Sup.Sales_Manager_Fax == null ? string.Empty : Sup.Sales_Manager_Fax.Trim();
            OLD_Sup.Sales_Manager_Mail = Sup.Sales_Manager_Mail == null ? string.Empty : Sup.Sales_Manager_Mail.Trim();

            OLD_Sup.Finance = Sup.Finance == null ? string.Empty : Sup.Finance.Trim();
            OLD_Sup.Finance_Tel = Sup.Finance_Tel == null ? string.Empty : Sup.Finance_Tel.Trim();
            OLD_Sup.Finance_Fax = Sup.Finance_Fax == null ? string.Empty : Sup.Finance_Fax.Trim();
            OLD_Sup.Finance_Mail = Sup.Finance_Mail == null ? string.Empty : Sup.Finance_Mail.Trim();

            OLD_Sup.GM = Sup.GM == null ? string.Empty : Sup.GM.Trim();
            OLD_Sup.GM_Tel = Sup.GM_Tel == null ? string.Empty : Sup.GM_Tel.Trim();
            OLD_Sup.GM_Fax = Sup.GM_Fax == null ? string.Empty : Sup.GM_Fax.Trim();
            OLD_Sup.GM_Mail = Sup.GM_Mail == null ? string.Empty : Sup.GM_Mail.Trim();

            OLD_Sup.Act_Status = Sup.Act_Status == null ? string.Empty : Sup.Act_Status.Trim();
            OLD_Sup.Act_Status_Reason = Sup.Act_Status_Reason == null ? string.Empty : Sup.Act_Status_Reason.Trim();

            this.Check_Supplier_Base(OLD_Sup);
            db.Entry(OLD_Sup).State = EntityState.Modified;

            MyDbSave.SaveChange(db);
        }

        public void Delete_Supplier(Guid SupID)
        {
            if(db.Po_Head.Where(x=>x.SupID == SupID).Any())
            {
                throw new Exception("已发生采购，无法删除供应商");
            }

            Supplier OLD_Sup = db.Supplier.Find(SupID);
            db.Supplier.Remove(OLD_Sup);
            MyDbSave.SaveChange(db);
        }

        private void Check_Supplier_Base(Supplier Sup)
        {
            if (string.IsNullOrEmpty(Sup.SupplierCode))
            {
                throw new Exception("供应商编号未填写");
            }

            if (string.IsNullOrEmpty(Sup.Sup_Name))
            {
                throw new Exception("供应商名称未填写");
            }

            if (string.IsNullOrEmpty(Sup.Sup_Short_Name))
            {
                throw new Exception("供应商简称未填写");
            }

            var query = db.Supplier.Where(x => x.LinkMainCID == Sup.LinkMainCID && x.SupID != Sup.SupID).AsQueryable();

            if (query.Where(x => x.SupplierCode == Sup.SupplierCode).Any())
            {
                throw new Exception("供应商编号重复");
            }

            if (query.Where(x => x.Sup_Name == Sup.Sup_Name).Any())
            {
                throw new Exception("供应商名称重复");
            }

            if (query.Where(x => x.Sup_Short_Name == Sup.Sup_Short_Name).Any())
            {
                throw new Exception("供应商简称重复");
            }
        }

    }

    
}
