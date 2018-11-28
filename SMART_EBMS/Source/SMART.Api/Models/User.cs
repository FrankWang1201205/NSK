using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.Api.Models
{
    public class User
    {
        public User()
        {
            UID = Guid.Empty;
            UserName = string.Empty;
            UserFullName = string.Empty;
            Sex = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            MobilePhone = string.Empty;
            Tel = string.Empty;
            RoleTitle = string.Empty;
            U_Status = string.Empty;
            U_Type = string.Empty;
            Education = string.Empty;
            Department = string.Empty;
            Origin = string.Empty;
            Branch = string.Empty;
            CreateDate = DateTime.Now;
            Create_Person = string.Empty;
            LastLoginDate = DateTime.Now;
            IsFrozen = 0;
            LinkMainCID = Guid.Empty;
            ALL_Role_List = new List<string>();
            ALL_DepName_List = new List<string>();

            Sex_List = new List<string>();
            Sex_List.Add("男");
            Sex_List.Add("女");

            ALL_U_Status_List = Enum.GetNames(typeof(User_Status_Enum)).ToList();
            ALL_U_Type_List = Enum.GetNames(typeof(User_Type_Enum)).ToList();
            ALL_Education_List = Enum.GetNames(typeof(User_Education_Enum)).ToList();

            U_Pro = new User_Profile();
        }

        [Key]
        public Guid UID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserFullName { get; set; }

        [Required]
        public string Password { get; set; }

        [DefaultValue("")]
        public string Sex { get; set; }

        [DefaultValue("")]
        public string Email { get; set; }

        [DefaultValue("")]
        public string MobilePhone { get; set; }

        [DefaultValue("")]
        public string Tel { get; set; }

        [DefaultValue("")]
        public string RoleTitle { get; set; }

        [DefaultValue("")]
        public string Department { get; set; }

        [DefaultValue("")]
        public string U_Status { get; set; }

        [DefaultValue("")]
        public string U_Type { get; set; }

        [DefaultValue("")]
        public string Education { get; set; }

        [DefaultValue("")]
        public string Origin { get; set; }

        //分支机构
        [DefaultValue("")]
        public string Branch { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [DefaultValue("")]
        public string Create_Person { get; set; }

        [Required]
        public DateTime LastLoginDate { get; set; }

        [DefaultValue(0)]
        public int IsFrozen { get; set; }

        [Required]
        public Guid LinkMainCID { get; set; }

        [NotMapped]
        public List<string> Sex_List { get; set; }

        [NotMapped]
        public List<string> ALL_U_Status_List { get; set; }

        [NotMapped]
        public List<string> ALL_U_Type_List { get; set; }

        [NotMapped]
        public List<string> ALL_Role_List { get; set; }

        [NotMapped]
        public List<string> ALL_DepName_List { get; set; }

        [NotMapped]
        public List<string> ALL_Education_List { get; set; }

        [NotMapped]
        public User_Profile U_Pro { get; set; }
    }

    public class User_Profile
    {
        public User_Profile()
        {
            UID = Guid.Empty;
            Birth_DT = DateTime.Now;
            Entry_DT = DateTime.Now;
            Identity_Card = string.Empty;
            Identity_Social = string.Empty;
            Family_Tel = string.Empty;
            Family_Address = string.Empty;
            EMD = string.Empty;
            EMD_Tel = string.Empty;
            Remark = string.Empty;
        }

        [Key]
        public Guid UID { get; set; }

        //出生日期
        [Required]
        public DateTime Birth_DT { get; set; }

        //入职日期
        [Required]
        public DateTime Entry_DT { get; set; }

        //身份证编号
        [DefaultValue("")]
        public string Identity_Card { get; set; }

        //社保编号
        [DefaultValue("")]
        public string Identity_Social { get; set; }

        [DefaultValue("")]
        public string Family_Tel { get; set; }

        [DefaultValue("")]
        public string Family_Address { get; set; }

        //紧急联络人
        [DefaultValue("")]
        public string EMD { get; set; }

        //紧急联络人_电话
        [DefaultValue("")]
        public string EMD_Tel { get; set; }

        [DefaultValue("")]
        public string Remark { get; set; }


    }

    [NotMapped]
    public class User_Filter
    {
        public User_Filter()
        {
            PageIndex = 1;
            PageSize = 25;
            Keyword = string.Empty;
            RoleTitle = string.Empty;
            LinkMainCID = Guid.Empty;
            FrozenList = new List<string>();
            FrozenList.Add(User_Frozen.启用.ToString());
            FrozenList.Add(User_Frozen.冻结.ToString());
            DepNameList = new List<string>();
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string RoleTitle { get; set; }
        public string DepName { get; set; }
        public string IsFrozen { get; set; }
        public List<string> FrozenList { get; set; }
        public List<string> DepNameList { get; set; }
        public Guid LinkMainCID { get; set; }
    }

    public enum User_Frozen
    {
        启用,
        冻结,
    }

    public enum User_Status_Enum
    {
        在职,
        离职,
        停薪留职,
        产假期间,
    }

    public enum User_Type_Enum
    {
        正式员工,
        临时员工,
        实习员工,
        退休返聘员工,
        其他,
    }

    public enum User_Education_Enum
    {
        初中,
        高中,
        大专,
        本科,
        本科以上
    }

    public enum User_RoleTitle_Emun
    {
        系统管理员,
        公司经理,
        计划专员,
        销售经理,
        客服专员,
        采购专员,
        采购主管,
        财务会计,
        仓管主管,
        仓管专员,
    }
}
