using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;
using System.Data.Entity;

namespace SMART.Api
{
    public interface IMainCompanyService
    {
        string Get_MainCompany_Name(Guid MainCID);
        List<MainCompany> Get_MainCompany_List();
        MainCompany Get_MainCompany(Guid MainCID);
        void Set_MainCom_Base(Guid MainCID, MainCompany MC);
        void Set_MainCom_ElectronicStamp(Guid MainCID, string ElectronicStamp);
        void Delete_MainCom_ElectronicStamp(Guid MainCID);
        void Set_MainCom_ComLogo(Guid MainCID, string ComLogo);
        void Delete_MainCom_ComLogo(Guid MainCID);
        void Create_MainCom(MainCompany MC);
    }

    public partial class MainCompanyService : IMainCompanyService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class MainCompanyService : IMainCompanyService
    {
        public MainCompany Get_MainCompany(Guid MainCID)
        {
            MainCompany MC = db.MainCompany.Find(MainCID);
            MC = MC == null ? new MainCompany() : MC;
            return MC;
        }

        public string Get_MainCompany_Name(Guid MainCID)
        {
            string MainCompanyName = string.Empty;
            try { MainCompanyName = db.MainCompany.Find(MainCID).MainCompanyName; } catch { }
            return MainCompanyName;
        }

        public List<MainCompany> Get_MainCompany_List()
        {
            List<MainCompany> List = db.MainCompany.OrderBy(x => x.CreateDate).ToList();
            List<User> UList = db.User.Where(x => x.RoleTitle == User_RoleTitle_Emun.系统管理员.ToString()).ToList();
            User U = new User();

            foreach (var x in List)
            {
                U = UList.Where(c => c.LinkMainCID == x.MainCID).FirstOrDefault();
                U = U == null ? new User() : U;
                x.Admin_User_Name = U.UserName;
                x.Admin_User_Password = U.Password;
            }
            return List;
        }
    }

    public partial class MainCompanyService : IMainCompanyService
    {
        public void Set_MainCom_Base(Guid MainCID, MainCompany MC)
        {
            MainCompany OLD_MC = db.MainCompany.Find(MainCID);
            OLD_MC.PinCode = MC.PinCode == null ? string.Empty : MC.PinCode.Trim();
            OLD_MC.MainTel = MC.MainTel == null ? string.Empty : MC.MainTel.Trim();
            OLD_MC.MainEmail = MC.MainEmail == null ? string.Empty : MC.MainEmail.Trim();
            OLD_MC.MainInvoiceAddress = MC.MainInvoiceAddress == null ? string.Empty : MC.MainInvoiceAddress.Trim();
            OLD_MC.MainAddress = MC.MainAddress == null ? string.Empty : MC.MainAddress.Trim();
            OLD_MC.TaxpayerIdentificationNo = MC.TaxpayerIdentificationNo == null ? string.Empty : MC.TaxpayerIdentificationNo.Trim();
            OLD_MC.MainBankInfo = MC.MainBankInfo == null ? string.Empty : MC.MainBankInfo.Trim();
            OLD_MC.MainBankAccount = MC.MainBankAccount == null ? string.Empty : MC.MainBankAccount.Trim();

            if(string.IsNullOrEmpty(OLD_MC.PinCode))
            {
                throw new Exception("企业代码不能为空");
            }

            if (db.MainCompany.Where(x=>x.PinCode == OLD_MC.PinCode && x.MainCID != OLD_MC.MainCID).Any())
            {
                throw new Exception("企业代码不可用");
            }

            db.Entry(OLD_MC).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Set_MainCom_ElectronicStamp(Guid MainCID, string ElectronicStamp)
        {
            MainCompany OLD_MC = db.MainCompany.Find(MainCID);
            OLD_MC.ElectronicStamp = ElectronicStamp;
            db.Entry(OLD_MC).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_MainCom_ElectronicStamp(Guid MainCID)
        {
            MainCompany OLD_MC = db.MainCompany.Find(MainCID);
            OLD_MC.ElectronicStamp = string.Empty;
            db.Entry(OLD_MC).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Set_MainCom_ComLogo(Guid MainCID, string ComLogo)
        {
            MainCompany OLD_MC = db.MainCompany.Find(MainCID);
            OLD_MC.ComLogo = ComLogo;
            db.Entry(OLD_MC).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_MainCom_ComLogo(Guid MainCID)
        {
            MainCompany OLD_MC = db.MainCompany.Find(MainCID);
            OLD_MC.ComLogo = string.Empty;
            db.Entry(OLD_MC).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Create_MainCom(MainCompany MC)
        {
            MC.MainCID = MyGUID.NewGUID();
            MC.MainCompanyName = MC.MainCompanyName.Trim();
            MC.PinCode = MC.PinCode.Trim();

            if (string.IsNullOrEmpty(MC.MainCompanyName))
            {
                throw new Exception("信息不能为空");
            }

            if (string.IsNullOrEmpty(MC.PinCode))
            {
                throw new Exception("企业代码不能为空");
            }

            if (db.MainCompany.Where(x => x.PinCode == MC.PinCode).Any())
            {
                throw new Exception("企业代码重复");
            }

            if (db.MainCompany.Where(x => x.MainCompanyName == MC.MainCompanyName).Any())
            {
                throw new Exception("公司名称重复");
            }

     
            db.MainCompany.Add(MC);

            User AdminUser = new User();
            AdminUser.UID = MyGUID.NewGUID();
            AdminUser.RoleTitle = User_RoleTitle_Emun.系统管理员.ToString();

            AdminUser.UserName = "Admin";
            AdminUser.UserFullName = "管理员";
            AdminUser.Password = "123456";
            AdminUser.Email = "n/a";
            AdminUser.LinkMainCID = MC.MainCID;
            db.User.Add(AdminUser);

            MyDbSave.SaveChange(db);
        }

    }
}
