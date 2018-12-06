using SMART.Api;
using SMART.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;

namespace SMART.EBMS
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://ksnskebms.mro1598.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    //收货作业
    public partial class MyWebService : WebService
    {
        IWmsService IW = new WmsService();
        private Guid MainCID = new Guid("20171010-1552-46ad-a514-3af95417ec4e");

        [WebMethod]
        public DataTable WMS_In_Task_List_A()
        {
            return IW.WMS_In_Task_List_A(MainCID);
        }

        [WebMethod]
        public DataTable WMS_In_Task_List_B()
        {
            return IW.WMS_In_Task_List_B(MainCID);
        }

        [WebMethod]
        public DataTable WMS_In_Task_List_C()
        {
            return IW.WMS_In_Task_List_C(MainCID);
        }

        [WebMethod]
        public DataTable WMS_In_Task_List_D()
        {
            return IW.WMS_In_Task_List_D(MainCID);
        }

        [WebMethod]
        public DataTable WMS_In_Task_By_Tray_No(string HeadID, string Tray_No)
        {
            return IW.WMS_In_Task_By_Tray_No(HeadID, Tray_No);
        }

        [WebMethod]
        public DataTable WMS_In_Task_By_Tray_No_Other(string HeadID, string Tray_No, string Case_No)
        {
            return IW.WMS_In_Task_By_Tray_No_Other(HeadID, Tray_No, Case_No);
        }

        [WebMethod]
        public string WMS_In_Task_Scan_Item(string HeadID, string Tray_No, string ScanStr, string Track_No)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_In_Task_Scan_Item(HeadID, Tray_No, ScanStr, Track_No);

            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public string WMS_In_Task_Scan_Item_Other(string HeadID, string Tray_No, string ScanStr, string Track_No, string Quantity, string Case_No)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_In_Task_Scan_Item_Other(HeadID, Tray_No, ScanStr, Track_No, Quantity, Case_No);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_In_Track_List(string HeadID)
        {
            return IW.WMS_In_Track_List(HeadID);
        }

        [WebMethod]
        public string WMS_In_Track_Scan_Item(string HeadID, string ScanStr, string Cost)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_In_Track_Scan_Item(HeadID, ScanStr, Cost);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public string WMS_In_Track_Scan_Delete(string HeadID)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_In_Track_Scan_Delete(HeadID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //上架作业
    public partial class MyWebService : WebService
    {
        [WebMethod]
        public DataTable WMS_Up_List()
        {
            return IW.WMS_Up_List(MainCID);
        }

        [WebMethod]
        public DataTable WMS_Up_List_Sub(string Head_ID)
        {
            return IW.WMS_Up_List_Sub(Head_ID);
        }

        [WebMethod]
        public string WMS_Up_Process(string Move_ID, string Scan_Source)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Up_Process(Move_ID, Scan_Source);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Up_List_With_Location(string Move_ID)
        {
            return IW.WMS_Up_List_With_Location(Move_ID);
        }

        [WebMethod]
        public DataTable WMS_Up_Scan_List_With_Location(string Move_ID)
        {
            return IW.WMS_Up_Scan_List_With_Location(Move_ID);
        }

        [WebMethod]
        public string WMS_Up_Scan_Item_With_Location(string Move_ID, string Scan_Source)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Up_Scan_Item_With_Location(Move_ID, Scan_Source);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Up_Scan_List_With_Location_Other(string Move_ID)
        {
            return IW.WMS_Up_Scan_List_With_Location_Other(Move_ID);
        }

        [WebMethod]
        public string WMS_Up_Scan_Item_With_Location_Other(string Move_ID, string Scan_Source, string Quantity)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Up_Scan_Item_With_Location_Other(Move_ID, Scan_Source, Quantity);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public string WMS_Up_Scan_List_With_Location_Delete(string Move_ID)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Up_Scan_List_With_Location_Delete(Move_ID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Up_Stocktaking_Scan_List(string Move_ID)
        {
            return IW.WMS_Up_Stocktaking_Scan_List(Move_ID);
        }

        [WebMethod]
        public string WMS_Up_Stocktaking_Scan_Item(string Move_ID, string Scan_Source)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Up_Stocktaking_Scan_Item(Move_ID, Scan_Source);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Up_Stocktaking_Scan_List_Other(string Move_ID)
        {
            return IW.WMS_Up_Stocktaking_Scan_List_Other(Move_ID);
        }

        [WebMethod]
        public string WMS_Up_Stocktaking_Scan_Item_Other(string Move_ID, string Scan_Source, string Quantity)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Up_Stocktaking_Scan_Item_Other(Move_ID, Scan_Source, Quantity);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //移库作业
    public partial class MyWebService : WebService
    {
        [WebMethod]
        public DataTable WMS_Move_List()
        {
            return IW.WMS_Move_List(MainCID);
        }

        [WebMethod]
        public string WMS_Move_Create(string Scan_Source)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Move_Create(MainCID, Scan_Source);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

    }

    //配货作业
    public partial class MyWebService : WebService
    {
        [WebMethod]
        public DataTable WMS_Out_Pick_Stocktaking_List()
        {
            return IW.WMS_Out_Pick_Stocktaking_List(MainCID);
        }

        [WebMethod]
        public DataTable WMS_Out_Pick_Stocktaking_List_Work_Person(string Work_Person)
        {
            return IW.WMS_Out_Pick_Stocktaking_List_Work_Person(MainCID, Work_Person);
        }

        [WebMethod]
        public DataTable WMS_Out_Pick_Stocktaking_List_Sub(string Task_ID)
        {
            return IW.WMS_Out_Pick_Stocktaking_List_Sub(Task_ID);
        }

        [WebMethod]
        public string WMS_Out_Pick_Stocktaking_List_Sub_Scan(string Task_ID, string Scan_Source)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Out_Pick_Stocktaking_List_Sub_Scan(Task_ID, Scan_Source);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Out_Pick_Stocktaking_List_Sub_Other(string Task_ID)
        {
            return IW.WMS_Out_Pick_Stocktaking_List_Sub_Other(Task_ID);
        }

        [WebMethod]
        public string WMS_Out_Pick_Stocktaking_List_Sub_Other_Scan(string Task_ID, string Scan_Source, string Quantity)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Out_Pick_Stocktaking_List_Sub_Other_Scan(Task_ID, Scan_Source,Quantity);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //验货作业
    public partial class MyWebService : WebService
    {
        [WebMethod]
        public DataTable WMS_Out_Task_List_A()
        {
            return IW.WMS_Out_Task_List_A(MainCID);
        }

        [WebMethod]
        public DataTable WMS_Out_Task_List_B()
        {
            return IW.WMS_Out_Task_List_B(MainCID);
        }

        [WebMethod]
        public DataTable WMS_Out_Task_List_C()
        {
            return IW.WMS_Out_Task_List_C(MainCID);
        }

        [WebMethod]
        public DataTable WMS_Out_Task_By_Tray_No(string HeadID, string Tray_No)
        {
            return IW.WMS_Out_Task_By_Tray_No(HeadID, Tray_No);
        }
      
        [WebMethod]
        public string WMS_Out_Task_Scan_Item(string HeadID, string Tray_No, string ScanStr)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Out_Task_Scan_Item(HeadID, Tray_No, ScanStr);

            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Out_Task_By_Tray_No_Other(string HeadID, string Tray_No, string Case_No)
        {
            return IW.WMS_Out_Task_By_Tray_No_Other(HeadID, Tray_No, Case_No);
        }

        [WebMethod]
        public string WMS_Out_Task_Scan_Item_Other(string HeadID, string Tray_No, string ScanStr, string Quantity, string Case_No)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Out_Task_Scan_Item_Other(HeadID, Tray_No, ScanStr, Quantity, Case_No);

            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Out_Task_List_With_Tray_No(string HeadID)
        {
            return IW.WMS_Out_Task_List_With_Tray_No(HeadID);
        }

        [WebMethod]
        public DataTable WMS_Out_Track_List(string HeadID, string Tray_No)
        {
            return IW.WMS_Out_Track_List(HeadID,Tray_No);
        }

        [WebMethod]
        public string WMS_Out_Track_Scan_Item(string HeadID, string Tray_No, string ScanStr, string Cost)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Out_Track_Scan_Item(HeadID, Tray_No, ScanStr, Cost);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public string WMS_Out_Track_Scan_Delete(string HeadID, string Tray_No)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Out_Track_Scan_Delete(HeadID, Tray_No);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //盘库作业
    public partial class MyWebService : WebService
    {
        [WebMethod]
        public DataTable WMS_Stock_Task_List()
        {
            return IW.WMS_Stock_Task_List(MainCID);
        }

        [WebMethod]
        public string WMS_Stock_Task_List_Create(string Scan_Source)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Stock_Task_List_Create(MainCID, Scan_Source);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Stock_Task_Scan_List(string Task_ID)
        {
            return IW.WMS_Stock_Task_Scan_List(Task_ID);
        }

        [WebMethod]
        public string WMS_Stock_Task_Scan_List_Create(string Task_ID, string Scan_Source)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Stock_Task_Scan_List_Create(Task_ID, Scan_Source);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Stock_Task_Scan_List_Other(string Task_ID)
        {
            return IW.WMS_Stock_Task_Scan_List_Other(Task_ID);
        }

        [WebMethod]
        public string WMS_Stock_Task_Scan_List_Create_Other(string Task_ID, string Scan_Source, string Quantity)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Stock_Task_Scan_List_Create_Other(Task_ID, Scan_Source, Quantity);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //首次盘库
    public partial class MyWebService : WebService
    {
        [WebMethod]
        public DataTable WMS_Stock_Task_List_First()
        {
            return IW.WMS_Stock_Task_List_First(MainCID);
        }

        [WebMethod]
        public string WMS_Stock_Task_List_Create_First(string Scan_Source)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Stock_Task_List_Create_First(MainCID, Scan_Source);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Stock_Task_Scan_List_First(string Task_ID)
        {
            return IW.WMS_Stock_Task_Scan_List_First(Task_ID);
        }

        [WebMethod]
        public string WMS_Stock_Task_Scan_List_Create_First(string Task_ID, string Scan_Source)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Stock_Task_Scan_List_Create_First(Task_ID, Scan_Source);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        [WebMethod]
        public DataTable WMS_Stock_Task_Scan_List_Other_First(string Task_ID)
        {
            return IW.WMS_Stock_Task_Scan_List_Other_First(Task_ID);
        }

        [WebMethod]
        public string WMS_Stock_Task_Scan_List_Create_Other_First(string Task_ID, string Scan_Source, string Quantity)
        {
            string result = string.Empty;
            try
            {
                IW.WMS_Stock_Task_Scan_List_Create_Other_First(Task_ID, Scan_Source, Quantity);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }
}