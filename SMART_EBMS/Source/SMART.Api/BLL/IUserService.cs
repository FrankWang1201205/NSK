using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;
using System.Data.Entity;

namespace SMART.Api
{
    public interface IUserService
    {
        PageList<User> Get_User_PageList(User_Filter MF);
        User Get_User_Item(Guid UID);
        User Get_User_Item_More(Guid UID);
        User Get_User_By_Controller(string UIDStr);
        User Get_Empty_User(Guid MainCID);
        Guid User_Login(string UserName, string Password, string PinCode);

        void Create_User(Guid MainCID, string Create_Person, User U);
        void Set_User(Guid UID, User U);
        void Set_User_Base(Guid UID, User U);
        void Set_User_Password(Guid UID, string Password);
        void Delete_User(Guid UID);

        List<string> Get_Department_Name_List(Guid MainCID);
        List<Department> Get_Department_List(Guid MainCID);
        Department Get_Department_Item(Guid DepID);
        Guid Create_Department(Guid MainCID, Department Dep);
        void Set_Department(Guid DepID, Department Dep);
        void Delete_Department(Guid DepID);
    }

    public partial class UserService : IUserService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class UserService : IUserService
    {
        public PageList<User> Get_User_PageList(User_Filter MF)
        {
            var query = db.User.AsQueryable();
            if(MF.LinkMainCID != Guid.Empty)
            {
                query = query.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            }

            if(!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(MF.Keyword) || x.UserFullName.Contains(MF.Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.RoleTitle))
            {
                query = query.Where(x => x.RoleTitle == MF.RoleTitle).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.DepName))
            {
                query = query.Where(x => x.Department == MF.DepName).AsQueryable();
            }

            if (MF.IsFrozen == User_Frozen.冻结.ToString())
            {
                query = query.Where(x => x.IsFrozen == 1).AsQueryable();
            }

            if (MF.IsFrozen == User_Frozen.启用.ToString())
            {
                query = query.Where(x => x.IsFrozen == 0).AsQueryable();
            }

            List<User> RowList = query.OrderByDescending(x => x.CreateDate).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<User> PList = new PageList<User>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowList;
            return PList;
        }

        public User Get_User_Item_More(Guid UID)
        {
            User U = db.User.Find(UID);
            U = U == null ? new User() : U;
            U.ALL_Role_List = Enum.GetNames(typeof(User_RoleTitle_Emun)).ToList();
            U.ALL_DepName_List = this.Get_Department_Name_List(U.LinkMainCID);

            U.U_Pro = db.User_Profile.Find(U.UID);
            U.U_Pro = U.U_Pro == null ? new User_Profile() : U.U_Pro;

            return U;
        }

        public User Get_User_Item(Guid UID)
        {
            User U = db.User.Find(UID);
            U = U == null ? new User() : U;
            U.ALL_Role_List = Enum.GetNames(typeof(User_RoleTitle_Emun)).ToList();
            return U;
        }

        public User Get_User_By_Controller(string UIDStr)
        {
            User U = new User();
            Guid UID_GUID = Guid.Empty;
            try { UID_GUID = new Guid(UIDStr); } catch { }
            if (UID_GUID != Guid.Empty)
            {
                U = this.Get_User_Item(UID_GUID);
            }
            return U;
        }

        public User Get_Empty_User(Guid MainCID)
        {
            User U = new User();
            U.ALL_Role_List = Enum.GetNames(typeof(User_RoleTitle_Emun)).ToList();
            U.ALL_DepName_List = this.Get_Department_Name_List(MainCID);
            return U;
        }

        public Guid User_Login(string UserName, string Password, string PinCode)
        {
            MainCompany MC = db.MainCompany.Where(x => x.PinCode == PinCode).FirstOrDefault();
            if(MC == null)
            {
                throw new Exception("企业代码错误");
            }

            User U = db.User.Where(x => x.UserName == UserName && x.Password == Password && x.LinkMainCID == MC.MainCID).FirstOrDefault();
            if(U == null)
            {
                throw new Exception("用户名或密码错误");
            }

            if(U.IsFrozen == 1)
            {
                throw new Exception("此用户已被冻结");
            }

            U.LastLoginDate = DateTime.Now;
            MyDbSave.SaveChange(db);
            return U.UID;
        }
    }

    public partial class UserService : IUserService
    {
        public void Create_User(Guid MainCID, string Create_Person, User U)
        {
            MainCompany MC = db.MainCompany.Find(MainCID);
            if(MC == null)
            {
                throw new Exception("MainCompany Is Null");
            }

            U.UID = MyGUID.NewGUID();
            U.CreateDate = DateTime.Now;
            U.Create_Person = Create_Person;
            U.UserName = U.UserName.Trim();
            U.UserFullName = U.UserFullName.Trim();
            U.Password = U.Password.Trim();
            U.RoleTitle = U.RoleTitle == null ? string.Empty : U.RoleTitle.Trim();
            U.Department = U.Department == null ? string.Empty : U.Department.Trim();
            U.Tel = U.Tel == null ? string.Empty : U.Tel.Trim();
            U.MobilePhone = U.MobilePhone == null ? string.Empty : U.MobilePhone.Trim();
            U.Email = U.Email == null ? string.Empty : U.Email.Trim();

            U.U_Status = U.U_Status == null ? string.Empty : U.U_Status.Trim();
            U.U_Type = U.U_Type == null ? string.Empty : U.U_Type.Trim();
            U.Sex = U.Sex == null ? string.Empty : U.Sex.Trim();
            U.Education = U.Education == null ? string.Empty : U.Education.Trim();
            U.Origin = U.Origin == null ? string.Empty : U.Origin.Trim();
            U.Branch = U.Branch == null ? string.Empty : U.Branch.Trim();

            U.LinkMainCID = MC.MainCID;

            User_Profile UPro = new User_Profile();
            UPro.UID = U.UID;
            UPro.Birth_DT = U.U_Pro.Birth_DT;
            UPro.Entry_DT = U.U_Pro.Entry_DT;

            UPro.Remark = U.U_Pro.Remark == null ? string.Empty : U.U_Pro.Remark.Trim();
            UPro.Identity_Card = U.U_Pro.Identity_Card == null ? string.Empty : U.U_Pro.Identity_Card.Trim();
            UPro.Identity_Social = U.U_Pro.Identity_Social == null ? string.Empty : U.U_Pro.Identity_Social.Trim();
            UPro.Family_Tel = U.U_Pro.Family_Tel == null ? string.Empty : U.U_Pro.Family_Tel.Trim();
            UPro.Family_Address = U.U_Pro.Family_Address == null ? string.Empty : U.U_Pro.Family_Address.Trim();
            UPro.EMD = U.U_Pro.EMD == null ? string.Empty : U.U_Pro.EMD.Trim();
            UPro.EMD_Tel = U.U_Pro.EMD_Tel == null ? string.Empty : U.U_Pro.EMD_Tel.Trim();


            if (string.IsNullOrEmpty(U.Password))
            {
                throw new Exception("密码不能为空");
            }

            this.Check_User_Info(U);
            db.User.Add(U);
            db.User_Profile.Add(UPro);
            MyDbSave.SaveChange(db);
        }

        public void Set_User_Password(Guid UID, string Password)
        {
            User Old_U = db.User.Find(UID);
            Old_U.Password = Password == null ? string.Empty : Password.Trim();

            if (string.IsNullOrEmpty(Old_U.Password))
            {
                throw new Exception("密码不能为空");
            }

            db.Entry(Old_U).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Set_User(Guid UID, User U)
        {
            User Old_U = db.User.Find(UID);

            Old_U.UserName = U.UserName.Trim();
            Old_U.UserFullName = U.UserFullName.Trim();
            Old_U.Tel = U.Tel == null ? string.Empty : U.Tel.Trim();
            Old_U.MobilePhone = U.MobilePhone == null ? string.Empty : U.MobilePhone.Trim();
            Old_U.Email = U.Email == null ? string.Empty : U.Email.Trim();
            Old_U.Department = U.Department == null ? string.Empty : U.Department.Trim();

            if (Old_U.RoleTitle != User_RoleTitle_Emun.系统管理员.ToString())
            {
                if (Enum.GetNames(typeof(User_RoleTitle_Emun)).ToList().Where(x => x == U.RoleTitle).Any() == false)
                {
                    throw new Exception("角色不匹配");
                }
                else
                {
                    Old_U.RoleTitle = U.RoleTitle;
                }
            }

            Old_U.U_Status = U.U_Status == null ? string.Empty : U.U_Status.Trim();
            Old_U.U_Type = U.U_Type == null ? string.Empty : U.U_Type.Trim();
            Old_U.Sex = U.Sex == null ? string.Empty : U.Sex.Trim();
            Old_U.Education = U.Education == null ? string.Empty : U.Education.Trim();
            Old_U.Origin = U.Origin == null ? string.Empty : U.Origin.Trim();
            Old_U.Branch = U.Branch == null ? string.Empty : U.Branch.Trim();

            Old_U.IsFrozen = Old_U.U_Status == User_Status_Enum.在职.ToString() ? 0 : 1;

            User_Profile Old_UPre = db.User_Profile.Find(Old_U.UID);
            if(Old_UPre == null)
            {
                Old_UPre = new User_Profile();
                Old_UPre.UID = Old_U.UID;
                Old_UPre.Birth_DT = U.U_Pro.Birth_DT;
                Old_UPre.Entry_DT = U.U_Pro.Entry_DT;
                Old_UPre.Remark = U.U_Pro.Remark == null ? string.Empty : U.U_Pro.Remark.Trim();
                Old_UPre.Identity_Card = U.U_Pro.Identity_Card == null ? string.Empty : U.U_Pro.Identity_Card.Trim();
                Old_UPre.Identity_Social = U.U_Pro.Identity_Social == null ? string.Empty : U.U_Pro.Identity_Social.Trim();
                Old_UPre.Family_Tel = U.U_Pro.Family_Tel == null ? string.Empty : U.U_Pro.Family_Tel.Trim();
                Old_UPre.Family_Address = U.U_Pro.Family_Address == null ? string.Empty : U.U_Pro.Family_Address.Trim();
                Old_UPre.EMD = U.U_Pro.EMD == null ? string.Empty : U.U_Pro.EMD.Trim();
                Old_UPre.EMD_Tel = U.U_Pro.EMD_Tel == null ? string.Empty : U.U_Pro.EMD_Tel.Trim();
                db.User_Profile.Add(Old_UPre);
            }else
            {
                Old_UPre.Birth_DT = U.U_Pro.Birth_DT;
                Old_UPre.Entry_DT = U.U_Pro.Entry_DT;
                Old_UPre.Remark = U.U_Pro.Remark == null ? string.Empty : U.U_Pro.Remark.Trim();
                Old_UPre.Identity_Card = U.U_Pro.Identity_Card == null ? string.Empty : U.U_Pro.Identity_Card.Trim();
                Old_UPre.Identity_Social = U.U_Pro.Identity_Social == null ? string.Empty : U.U_Pro.Identity_Social.Trim();
                Old_UPre.Family_Tel = U.U_Pro.Family_Tel == null ? string.Empty : U.U_Pro.Family_Tel.Trim();
                Old_UPre.Family_Address = U.U_Pro.Family_Address == null ? string.Empty : U.U_Pro.Family_Address.Trim();
                Old_UPre.EMD = U.U_Pro.EMD == null ? string.Empty : U.U_Pro.EMD.Trim();
                Old_UPre.EMD_Tel = U.U_Pro.EMD_Tel == null ? string.Empty : U.U_Pro.EMD_Tel.Trim();
            }

            this.Check_User_Info(Old_U);
            db.Entry(Old_U).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Set_User_Base(Guid UID, User U)
        {
            User Old_U = db.User.Find(UID);

            Old_U.UserFullName = U.UserFullName.Trim();
            Old_U.Tel = U.Tel == null ? string.Empty : U.Tel.Trim();
            Old_U.MobilePhone = U.MobilePhone == null ? string.Empty : U.MobilePhone.Trim();
            Old_U.Email = U.Email == null ? string.Empty : U.Email.Trim();
            this.Check_User_Info(Old_U);

            db.Entry(Old_U).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_User(Guid UID)
        {
            User U = db.User.Find(UID);
            if(U.RoleTitle == User_RoleTitle_Emun.系统管理员.ToString())
            {
                throw new Exception("系统管理员，不允许删除");
            }

            if(db.Customer.Where(x=>x.LinkMainCID == U.LinkMainCID && x.Sales_UID == U.UID).Any())
            {
                throw new Exception("此用户已设为客户销售经理，不允许删除");
            }

            User_Profile UPro = db.User_Profile.Find(U.UID);
            if(UPro != null)
            {
                db.User_Profile.Remove(UPro);
            }

            db.User.Remove(U);
            MyDbSave.SaveChange(db);
        }

        private void Check_User_Info(User U)
        {
            if (db.User.Where(x => x.UID != U.UID && x.UserName == U.UserName && x.LinkMainCID == U.LinkMainCID).Any())
            {
                throw new Exception("员工代码重复");
            }

            if (db.User.Where(x => x.UID != U.UID && x.UserFullName == U.UserFullName && x.LinkMainCID == U.LinkMainCID).Any())
            {
                throw new Exception("员工姓名重复");
            }

            if (string.IsNullOrEmpty(U.RoleTitle))
            {
                throw new Exception("RoleTitle Is Null");
            }
        }

    }

    public partial class UserService
    {
        public List<string> Get_Department_Name_List(Guid MainCID)
        {
            return this.Get_Department_List(MainCID).Select(x => x.DepName).ToList();
        }

        public List<Department> Get_Department_List(Guid MainCID)
        {
            return db.Department.Where(x => x.LinkMainCID == MainCID).OrderBy(x => x.Create_DT).ToList();
        }

        public Department Get_Department_Item(Guid DepID)
        {
            Department Dep = db.Department.Find(DepID);
            Dep = Dep == null ? new Department() : Dep;
            return Dep;
        }

        public Guid Create_Department(Guid MainCID , Department Dep)
        {
            MainCompany MC = db.MainCompany.Find(MainCID);
            if (MC == null)
            {
                throw new Exception("MainCompany Is Null");
            }
            Dep.DepID = MyGUID.NewGUID();
            Dep.Create_DT = DateTime.Now;
            Dep.DepName = Dep.DepName == null ? string.Empty : Dep.DepName.Trim();
            Dep.LinkMainCID = MC.MainCID;

            this.Check_Department(Dep);

            db.Department.Add(Dep);
            MyDbSave.SaveChange(db);
            return Dep.DepID;
        }

        public void Set_Department(Guid DepID, Department Dep)
        {
            Department OLD_Dep = db.Department.Find(DepID);
            OLD_Dep.DepName = Dep.DepName == null ? string.Empty : Dep.DepName.Trim();
            this.Check_Department(OLD_Dep);

            //同步刷新用户部门
            List<User> UList = db.User.Where(x => x.Department == OLD_Dep.DepName && x.LinkMainCID == OLD_Dep.LinkMainCID).ToList();
            foreach(var x in UList)
            {
                x.Department = OLD_Dep.DepName;
            }
            MyDbSave.SaveChange(db);
        }

        public void Delete_Department(Guid DepID)
        {
            Department Dep = db.Department.Find(DepID);
            db.Department.Remove(Dep);

            //同步刷新用户部门
            List<User> UList = db.User.Where(x => x.Department == Dep.DepName && x.LinkMainCID == Dep.LinkMainCID).ToList();
            foreach (var x in UList)
            {
                x.Department = string.Empty;
            }
            MyDbSave.SaveChange(db);
        }



        private void Check_Department(Department Dep)
        {
            if(string.IsNullOrEmpty(Dep.DepName))
            {
                throw new Exception("部门名称未填写");
            }

            if (db.Department.Where(x=>x.LinkMainCID == Dep.LinkMainCID && x.DepName == Dep.DepName && x.DepID != Dep.DepID).Any())
            {
                throw new Exception("部门名称重复");
            }
        }

    }
}
