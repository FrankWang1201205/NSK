using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SMART.EBMS
{
    public class MyAssembly
    {
        public static List<Type> GetAssemblyTypeList(string AssemblyName)
        {
            return Assembly.Load(AssemblyName).GetTypes().ToList();
        }

        public static List<MyType> GetInterfaceList(string AssemblyName, string Namespace)
        {
            List<Type> List = GetAssemblyTypeList(AssemblyName).Where(x => x.Namespace == Namespace && x.IsInterface && x.IsPublic).ToList();

            MyType MT = new MyType();
            List<MyType> MyList = new List<MyType>();
            foreach (var x in List)
            {
                MT = new MyType();
                MT.Interface_Name = x.Name;
                MT.MethodNames = new List<MyType_Method>();
                MyType_Method MM = new MyType_Method();
                foreach (var xx in x.GetMethods().ToList())
                {
                    MM = new MyType_Method();
                    MM.MethodName = xx.Name;
                    MM.MethodReturnType = xx.ReturnType.Name;
                    MM.MethodParameters = string.Empty;
                    foreach (var xxx in xx.GetParameters())
                    {
                        MM.MethodParameters += xxx.ParameterType.Name + " " + xxx.Name + ", ";
                    }

                    if (MM.MethodParameters.Length > 1)
                    {
                        MM.MethodParameters = MM.MethodParameters.Substring(0, MM.MethodParameters.Length - 2);
                    }

                    MM.Method_Info = xx;
                    MT.MethodNames.Add(MM);
                }
                MyList.Add(MT);
            }
            return MyList;
        }

        public static List<MyType> GetModelList(string AssemblyName, string Namespace)
        {
            List<Type> List = GetAssemblyTypeList(AssemblyName).Where(x => x.Namespace == Namespace && (x.IsEnum || x.IsClass) && x.IsPublic).ToList();

            MyType MT = new MyType();
            List<MyType> MyList = new List<MyType>();
            foreach (var x in List)
            {
                MT = new MyType();
                MT.Model_Name = x.Name;
                MT.Propertys = x.GetProperties().ToList();
                if(x.IsEnum)
                {
                    MT.IsEnum = "enum";
                    MT.EnumNames = x.GetEnumNames().ToList();
                }
                MyList.Add(MT);
            }
            return MyList;
        }
    }

    public class MyType
    {
        public MyType()
        {
            EnumNames = new List<string>();
            MethodNames = new List<MyType_Method>();
        }

        public string Interface_Name { get; set; }
        public List<MyType_Method> MethodNames { get; set; }
        public string Model_Name { get; set; }
        public List<PropertyInfo> Propertys { get; set; }
        public string IsEnum { get; set; }
        public List<string> EnumNames { get; set; }
    }

    public class MyType_Method
    {
        public string MethodName { get; set; }
        public string MethodReturnType { get; set; }
        public string MethodParameters { get; set; }
        public MethodInfo Method_Info { get; set; }
    }


}