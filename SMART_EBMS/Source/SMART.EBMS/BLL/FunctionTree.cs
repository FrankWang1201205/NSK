using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using SMART.Api;
using SMART.Api.Models;

namespace SMART.EBMS
{
    public static class GetFunctionTree
    {
        public static FunctionTree User_Tree(User U)
        {
            FunctionTree FT = Get_ALL_Tree();

            //完成三级子模块权限识别
            foreach(var x in FT.ML)
            {
                foreach(var xx in x.Sub_Menu_List)
                {
                    foreach(var xxx in xx.Sub_Menu_Top_List)
                    {
                        xxx.Active = xxx.RoleTitle.Contains(U.RoleTitle) ? 1 : 0;
                        xxx.Active = U.RoleTitle == User_RoleTitle_Emun.系统管理员.ToString() ? 1 : xxx.Active;
                        xxx.Active = xxx.RoleTitle == string.Empty ? 1 : xxx.Active;
                        xxx.Active = U.IsFrozen == 1 ? 0 : xxx.Active;
                    }
                }
            }

            //二级模块默认路径
            foreach (var x in FT.ML)
            {
                foreach (var xx in x.Sub_Menu_List)
                {
                    foreach(var xxx in xx.Sub_Menu_Top_List.Where(CC => CC.Active == 1))
                    {
                        xx.DefURL = xxx.URL;
                        xx.Active = 1;
                        break;
                    }
                }
            }

            FT.ML = FT.ML.Where(x => x.Sub_Menu_List.Where(c => c.Active == 1).Any() == true).ToList();
            return FT;
        }

        public static SubMenu User_Tree_SubMenu(User U, string ModularName, string SubMenuName)
        {
            SubMenu SubM = new SubMenu();
            try
            {
                SubM = User_Tree(U).ML.Where(x => x.ModularName == ModularName).FirstOrDefault().Sub_Menu_List.Where(x => x.Name == SubMenuName).FirstOrDefault();
                SubM = SubM == null ? new SubMenu() : SubM;
            }
            catch
            {
                SubM = new SubMenu();
            }

            SubM.Sub_Menu_Top_List = SubM.Sub_Menu_Top_List.Where(x => x.Active == 1).ToList();
            return SubM;
        }

        private static FunctionTree Get_ALL_Tree()
        {
            string XMLPath = HttpRuntime.AppDomainAppPath.ToString() + "XML_Config\\FunctionTree.xml";
            XDocument XMLDOC = XDocument.Load(XMLPath);
            var query = XMLDOC.Descendants("FunctionTree").ToList();

            List<Modular> MList = new List<Modular>();
            List<SubMenu> AllSTM = new List<SubMenu>();
            Modular Mod = new Modular();
            SubMenu Mod_Sub; new SubMenu();
            SubMenu_Top Mod_Sub_Top = new SubMenu_Top();
            int i = 0;
            int ii = 0;
            int iii = 0;
            foreach (var x in query)
            {
                i++;
                Mod = new Modular();
                Mod.ModularCode = i.ToString();
                Mod.ModularName = x.Attribute("ModularName").Value;
                Mod.ModularIcon = x.Attribute("ModularIcon").Value;
                Mod.Sub_Menu_List = new List<SubMenu>();

                ii = 0;
                foreach (var xx in x.Descendants("SubMenu").ToList())
                {
                    ii++;
                    Mod_Sub = new SubMenu();
                    Mod_Sub.Code = Mod.ModularCode + "_" + ii.ToString();
                    Mod_Sub.Name = xx.Attribute("Name").Value;
                    Mod_Sub.Sub_Menu_Top_List = new List<SubMenu_Top>();

                    iii = 0;
                    foreach (var xxx in xx.Descendants("TopMenu").ToList())
                    {
                        iii++;
                        if (xxx.Attribute("TopName") != null)
                        {
                            Mod_Sub_Top = new SubMenu_Top();
                            Mod_Sub_Top.TopCode = Mod_Sub.Code + "_" + iii.ToString();
                            Mod_Sub_Top.TopName = xxx.Attribute("TopName").Value;
                            Mod_Sub_Top.URL = xxx.Attribute("URL").Value;
                            Mod_Sub_Top.RoleTitle = xxx.Attribute("RoleTitle").Value;
                            Mod_Sub.Sub_Menu_Top_List.Add(Mod_Sub_Top);
                        }
                    }
                    Mod.Sub_Menu_List.Add(Mod_Sub);
                }
                MList.Add(Mod);
            }

            FunctionTree FT = new FunctionTree();
            FT.ML = MList;
            return FT;
        }
    }

    public class FunctionTree
    {
        public FunctionTree()
        {
            ML = new List<Modular>();
        }

        public List<Modular> ML { get; set; }
    }

    //系统业务模块
    public class Modular
    {
        public Modular()
        {
            ModularCode = string.Empty;
            ModularName = string.Empty;
            ModularIcon = string.Empty;
            DefURL = string.Empty;
            Active = 0;
            Sub_Menu_List = new List<SubMenu>();
        }

        public string ModularCode { get; set; }
        public string ModularName { get; set; }
        public string ModularIcon { get; set; }
        public string DefURL { get; set; }
        public int Active { get; set; }
        public List<SubMenu> Sub_Menu_List { get; set; }
    }

    //业务模块子项
    public class SubMenu
    {
        public SubMenu()
        {
            Code = string.Empty;
            Name = string.Empty;
            DefURL = string.Empty;
            Active = 0;
            Sub_Menu_Top_List = new List<SubMenu_Top>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string DefURL { get; set; }
        public int Active { get; set; }
        public List<SubMenu_Top> Sub_Menu_Top_List { get; set; }

    }

    //业务模块子项顶部
    public class SubMenu_Top
    {
        public SubMenu_Top()
        {
            TopCode = string.Empty;
            TopName = string.Empty;
            URL = string.Empty;
            Active = 0;
        }

        public string TopCode { get; set; }
        public string TopName { get; set; }
        public string URL { get; set; }
        public string RoleTitle { get; set; }
        public int Active { get; set; }
    }
}