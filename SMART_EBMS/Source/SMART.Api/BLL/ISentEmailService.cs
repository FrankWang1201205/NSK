using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using System.Threading;
using SMART.Api.Models;

namespace SMART.Api
{
    public partial interface ISentEmailService
    {
        PageList<SentEmailRecord> Get_SentEmailRecord_PageList(SentEmail_Filter MF);
        SentEmail Get_SentEmail(Guid MainCID);
        void Set_SentEmail(Guid MainCID, SentEmail CW);
        void Sent_Email_For_Test(User U, string MailToAddress);
    }

    public partial class SentEmailService : ISentEmailService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class SentEmailService : ISentEmailService
    {
        public SentEmail Get_SentEmail(Guid MainCID)
        {
            SentEmail CM = db.SentEmail.Find(MainCID);
            CM = CM == null ? new SentEmail() : CM;
            return CM;
        }

        public void Set_SentEmail(Guid MainCID, SentEmail CW)
        {
            SentEmail OLD_CM = db.SentEmail.Find(MainCID);
            if (OLD_CM != null)
            {
                OLD_CM.SMTP = CW.SMTP.Trim();
                OLD_CM.Port = CW.Port.Trim();
                OLD_CM.MailName = CW.MailName.Trim();
                OLD_CM.UserName = CW.UserName.Trim();
                OLD_CM.Password = CW.Password.Trim();
                db.Entry(OLD_CM).State = EntityState.Modified;
            }
            else
            {
                OLD_CM = new SentEmail();
                OLD_CM.MainCID = MainCID;
                OLD_CM.MailType = CW.MailType.Trim();
                OLD_CM.SMTP = CW.SMTP.Trim();
                OLD_CM.Port = CW.Port.Trim();
                OLD_CM.MailName = CW.MailName.Trim();
                OLD_CM.UserName = CW.UserName.Trim();
                OLD_CM.Password = CW.Password.Trim();
                db.SentEmail.Add(OLD_CM);
            }
            MyDbSave.SaveChange(db);
        }

        public void Sent_Email_For_Test(User U, string MailToAddress)
        {
            string mailSubject = "SMART系统发件箱测试邮件";
            string mailBody = "这是在测试系统发件箱设置，由 " + U.UserFullName + " 自动发送的电子邮件。";
            SentEmail SE = this.Get_SentEmail(U.LinkMainCID);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB(MailToAddress, mailSubject, mailBody, SE));
        }

        public PageList<SentEmailRecord> Get_SentEmailRecord_PageList(SentEmail_Filter MF)
        {
            var query = db.SentEmailRecord.AsQueryable();

            if (!string.IsNullOrEmpty(MF.Email))
            {
                query = query.Where(x => x.E_Mail.Contains(MF.Email)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Subject))
            {
                query = query.Where(x => x.Subject.Contains(MF.Subject)).AsQueryable();
            }

            List<SentEmailRecord> RowList = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            PageList<SentEmailRecord> PList = new PageList<SentEmailRecord>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowList;
            return PList;

        }
    }

    //业务通知邮件
    public partial interface ISentEmailService
    {
        //收货邮件
        void Sent_To_Buyer_With_WMS_In_Abnormal(Guid Head_ID);
        void Sent_To_Buyer_With_WMS_In(Guid Head_ID);

        //出库发货邮件
        void Sent_To_Sales_With_WMS_Out_Inspection(Guid Head_ID);
        void Sent_To_Sales_With_WMS_Out_Finish(Guid Head_ID);
        void Batch_Sent_To_Sales_With_WMS_Out_Finish_With_Tracking_No(List<WMS_Out_Head> Head_List, List<WMS_Track> Track_List, List<WMS_Track_Info> Track_Info_List, User U);

        //盘库盈亏邮件
        void Sent_To_Manager_With_WMS_Profit_Loss_Head(WMS_Profit_Loss_Head Head, List<WMS_Profit_Loss_Line> Line_List);
        void Sent_To_Accounting_Staff_With_WMS_Profit_Loss_Head(WMS_Profit_Loss_Head Head, List<WMS_Profit_Loss_Line> Line_List);
        void Sent_To_WMS_Staff_With_Refused_WMS_Profit_Loss_Head(WMS_Profit_Loss_Head Head, List<WMS_Profit_Loss_Line> Line_List);
        void Sent_To_WMS_Staff_With_Comfirmed_WMS_Profit_Loss_Head(WMS_Profit_Loss_Head Head, List<WMS_Profit_Loss_Line> Line_List);

        //报废邮件提醒 
        void Sent_To_Manager_With_WMS_Waste_Task(WMS_Waste_Head Head, List<WMS_Waste_Line> Line_List);
        void Sent_To_Accounting_Staff_With_WMS_Waste_Task(WMS_Waste_Head Head, List<WMS_Waste_Line> Line_List);
        void Sent_To_WMS_Staff_With_Refused_WMS_Waste_Task(WMS_Waste_Head Head, List<WMS_Waste_Line> Line_List);
        void Sent_To_WMS_Staff_With_Comfirmed_WMS_Waste_Task(WMS_Waste_Head Head, List<WMS_Waste_Line> Line_List);
    }

    //收货邮件
    public partial class SentEmailService : ISentEmailService
    {
        public void Sent_To_Buyer_With_WMS_In_Abnormal(Guid Head_ID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_In_Head is null"); }

            string mailSubject = Head.Supplier_Name + "仓库收货异常";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>收货日期：" + Head.In_DT_Str + "</h5>";
            if (!string.IsNullOrEmpty(Head.Logistics_Company))
            {
                mailBody += "<h5>物流公司：" + Head.Logistics_Company + "</h5>";
            }
            mailBody += "<h5>运输方式：" + Head.Logistics_Mode + "</h5>";
            if (!string.IsNullOrEmpty(Head.Supplier_Name))
            {
                mailBody += "<h5>供应商：" + Head.Supplier_Name + "</h5>";
            }

            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.UserFullName == Head.Create_Person).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);

            IWmsService IW = new WmsService();

            string ExcelPath = IW.Get_WMS_In_Line_List_To_Excel_With_Abnormal(Head);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));
        }

        public void Sent_To_Buyer_With_WMS_In(Guid Head_ID)
        {
            WMS_In_Head Head = db.WMS_In_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_In_Head is null"); }

            string mailSubject = Head.Supplier_Name + "仓库收货完成";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>收货日期：" + Head.In_DT_Str + "</h5>";
            if (!string.IsNullOrEmpty(Head.Logistics_Company))
            {
                mailBody += "<h5>物流公司：" + Head.Logistics_Company + "</h5>";
            }
            mailBody += "<h5>运输方式：" + Head.Logistics_Mode + "</h5>";
            if (!string.IsNullOrEmpty(Head.Supplier_Name))
            {
                mailBody += "<h5>供应商：" + Head.Supplier_Name + "</h5>";
            }
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.UserFullName == Head.Create_Person).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);

            IWmsService IW = new WmsService();

            string ExcelPath = IW.Get_Task_List_To_Excel_Temp(Head.Head_ID);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));
        }
    }

    //出库发货邮件
    public partial class SentEmailService : ISentEmailService
    {
        public void Sent_To_Sales_With_WMS_Out_Inspection(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null"); }

            string mailSubject = Head.Customer_Name + "出库验货完成";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>发货日期：" + Head.Out_DT_Str + "</h5>";
            if (!string.IsNullOrEmpty(Head.Logistics_Company))
            {
                mailBody += "<h5>物流公司：" + Head.Logistics_Company + "</h5>";
            }
            mailBody += "<h5>运输方式：" + Head.Logistics_Mode + "</h5>";
            mailBody += "<h5>客户名称：" + Head.Customer_Name + "</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.UserFullName == Head.Create_Person).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);

            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB(mailToAddress_List, mailSubject, mailBody, SE));
        }

        public void Sent_To_Sales_With_WMS_Out_Finish(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null"); }

            string mailSubject = Head.Customer_Name + "出库送货完成";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>发货日期：" + Head.Out_DT_Str + "</h5>";
            if (!string.IsNullOrEmpty(Head.Logistics_Company))
            {
                mailBody += "<h5>物流公司：" + Head.Logistics_Company + "</h5>";
            }
            mailBody += "<h5>运输方式：" + Head.Logistics_Mode + "</h5>";
            mailBody += "<h5>客户名称：" + Head.Customer_Name + "</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.UserFullName == Head.Create_Person).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();

            string ExcelPath = IW.Get_Out_Task_List_To_Excel(Head.Head_ID);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));

        }

        public void Batch_Sent_To_Sales_With_WMS_Out_Finish_With_Tracking_No(List<WMS_Out_Head> Head_List, List<WMS_Track> Track_List, List<WMS_Track_Info> Track_Info_List, User U)
        {
            List<Guid> Head_ID_List = Track_List.Select(x => x.Link_Head_ID).Distinct().ToList();
            List<string> User_List_Str = Head_List.Select(x => x.Create_Person).Distinct().ToList();
            List<User> User_List = db.User.Where(x => x.LinkMainCID == U.LinkMainCID && User_List_Str.Contains(x.UserFullName)).ToList();
            List<WMS_Out_Scan> Scan_List = db.WMS_Out_Scan.Where(x => Head_ID_List.Contains(x.Link_Head_ID)).ToList();

            User U_Sub = new User();
            List<WMS_Out_Scan> Scan_List_Sub = new List<WMS_Out_Scan>();
            List<WMS_Track> Track_List_Sub = new List<WMS_Track>();
            List<WMS_Track_Info> Track_Info_List_Sub = new List<WMS_Track_Info>();
            List<string> Tracking_No_List_Sub = new List<string>();

            string MailToAddress = string.Empty;
            SentEmail SE = this.Get_SentEmail(U.LinkMainCID);
            IWmsService IW = new WmsService();

            SentEmail_Info Info = new SentEmail_Info();
            List<SentEmail_Info> Info_List = new List<SentEmail_Info>();
            foreach (var x in Head_List)
            {
                Info = new SentEmail_Info();
                Info.mailSubject = x.Customer_Name + "送货快递信息补发";
                Info.mailBody = string.Empty;
                Info.mailBody += "<h5>任务编号：" + x.Task_Bat_No_Str + "</h5>";
                Info.mailBody += "<h5>发货日期：" + x.Out_DT_Str + "</h5>";
                Info.mailBody += "<h5>运输方式：" + x.Logistics_Mode + "</h5>";
                Info.mailBody += "<h5>客户名称：" + x.Customer_Name + "</h5>";
                Info.mailBody += "<hr/>";
                Info.mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

                U_Sub = User_List.Where(c => c.UserFullName == x.Create_Person).FirstOrDefault();
                Scan_List_Sub = Scan_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                Track_List_Sub = Track_List.Where(c => c.Link_Head_ID == x.Head_ID).ToList();
                Tracking_No_List_Sub = Track_List_Sub.Select(c => c.Tracking_No).Distinct().ToList();
                Track_Info_List_Sub = Track_Info_List.Where(c => Tracking_No_List_Sub.Contains(c.Tracking_No)).ToList();

                MailToAddress = U_Sub.Email;
                Info.mailToAddress_List.Add(MailToAddress);

                Info.ExcelPath = IW.Get_Out_Task_List_To_Excel_With_Tracking_No(x, Scan_List_Sub, Track_List_Sub, Track_Info_List_Sub);
                Info_List.Add(Info);
            }
            NetMail.SendNetMailSingle_Save_DB_With_Excel_Foreach(Info_List, SE);
        }
    }

    //盘库盈亏邮件
    public partial class SentEmailService : ISentEmailService
    {
        public void Sent_To_Manager_With_WMS_Profit_Loss_Head(WMS_Profit_Loss_Head Head, List<WMS_Profit_Loss_Line> Line_List)
        {
            string mailSubject = "盘库盈亏审核单";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>申请时间：" + Head.Create_DT.ToString("yyyy-MM-dd HH:mm:ss") + "</h5>";
            mailBody += "<h5>申请人：" + Head.Create_Person + "</h5>";
            mailBody += "<h5>状态：等待公司经理审核</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.RoleTitle == User_RoleTitle_Emun.公司经理.ToString()).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();
            string ExcelPath = IW.Get_WMS_Profit_Loss_Line_By_Head_To_Excel(Line_List);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));

        }

        public void Sent_To_Accounting_Staff_With_WMS_Profit_Loss_Head(WMS_Profit_Loss_Head Head, List<WMS_Profit_Loss_Line> Line_List)
        {
            string mailSubject = "盘库盈亏审核单";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>申请时间：" + Head.Create_DT.ToString("yyyy-MM-dd HH:mm:ss") + "</h5>";
            mailBody += "<h5>申请人：" + Head.Create_Person + "</h5>";
            mailBody += "<h5>状态：公司经理已审核通过</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.RoleTitle == User_RoleTitle_Emun.财务审计.ToString()).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();
            string ExcelPath = IW.Get_WMS_Profit_Loss_Line_By_Head_To_Excel(Line_List);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));
        }

        public void Sent_To_WMS_Staff_With_Refused_WMS_Profit_Loss_Head(WMS_Profit_Loss_Head Head, List<WMS_Profit_Loss_Line> Line_List)
        {
            string mailSubject = "盘库盈亏审核单驳回";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>申请时间：" + Head.Create_DT.ToString("yyyy-MM-dd HH:mm:ss") + "</h5>";
            mailBody += "<h5>申请人：" + Head.Create_Person + "</h5>";
            mailBody += "<h5>状态：公司经理驳回</h5>";
            mailBody += "<h5>驳回理由：" + Head.Refuse_Remark + "</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.RoleTitle == User_RoleTitle_Emun.仓管主管.ToString()).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();
            string ExcelPath = IW.Get_WMS_Profit_Loss_Line_By_Head_To_Excel(Line_List);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));
        }

        public void Sent_To_WMS_Staff_With_Comfirmed_WMS_Profit_Loss_Head(WMS_Profit_Loss_Head Head, List<WMS_Profit_Loss_Line> Line_List)
        {
            string mailSubject = "盘库盈亏审核单通过";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>申请时间：" + Head.Create_DT.ToString("yyyy-MM-dd HH:mm:ss") + "</h5>";
            mailBody += "<h5>申请人：" + Head.Create_Person + "</h5>";
            mailBody += "<h5>状态：公司经理已通过，财务确认盈亏</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.RoleTitle == User_RoleTitle_Emun.仓管主管.ToString()).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();
            string ExcelPath = IW.Get_WMS_Profit_Loss_Line_By_Head_To_Excel(Line_List);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));
        }
    }

    //报废邮件提醒 
    public partial class SentEmailService : ISentEmailService
    {
        public void Sent_To_Manager_With_WMS_Waste_Task(WMS_Waste_Head Head, List<WMS_Waste_Line> Line_List)
        {
            string mailSubject = "库存报废申请单";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>申请时间：" + Head.Create_DT.ToString("yyyy-MM-dd HH:mm:ss") + "</h5>";
            mailBody += "<h5>申请人：" + Head.Create_Person + "</h5>";
            mailBody += "<h5>状态：等待公司经理审核</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.RoleTitle == User_RoleTitle_Emun.公司经理.ToString()).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();
            string ExcelPath = IW.Get_WMS_Waste_Line_By_Head_To_Excel(Line_List);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));

        }

        public void Sent_To_Accounting_Staff_With_WMS_Waste_Task(WMS_Waste_Head Head, List<WMS_Waste_Line> Line_List)
        {
            string mailSubject = "库存报废申请单";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>申请时间：" + Head.Create_DT.ToString("yyyy-MM-dd HH:mm:ss") + "</h5>";
            mailBody += "<h5>申请人：" + Head.Create_Person + "</h5>";
            mailBody += "<h5>状态：公司经理已审核通过</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.RoleTitle == User_RoleTitle_Emun.财务审计.ToString()).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();
            string ExcelPath = IW.Get_WMS_Waste_Line_By_Head_To_Excel(Line_List);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));
        }

        public void Sent_To_WMS_Staff_With_Refused_WMS_Waste_Task(WMS_Waste_Head Head, List<WMS_Waste_Line> Line_List)
        {
            string mailSubject = "库存报废申请单驳回";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>申请时间：" + Head.Create_DT.ToString("yyyy-MM-dd HH:mm:ss") + "</h5>";
            mailBody += "<h5>申请人：" + Head.Create_Person + "</h5>";
            mailBody += "<h5>状态：公司经理驳回</h5>";
            mailBody += "<h5>驳回理由：" + Head.Refuse_Remark + "</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.RoleTitle == User_RoleTitle_Emun.仓管主管.ToString()).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();
            string ExcelPath = IW.Get_WMS_Waste_Line_By_Head_To_Excel(Line_List);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));

        }

        public void Sent_To_WMS_Staff_With_Comfirmed_WMS_Waste_Task(WMS_Waste_Head Head, List<WMS_Waste_Line> Line_List)
        {
            string mailSubject = "库存报废申请单通过";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>申请时间：" + Head.Create_DT.ToString("yyyy-MM-dd HH:mm:ss") + "</h5>";
            mailBody += "<h5>申请人：" + Head.Create_Person + "</h5>";
            mailBody += "<h5>状态：公司经理已通过，财务确认报废出库</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</h5>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.RoleTitle == User_RoleTitle_Emun.仓管主管.ToString()).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();
            string ExcelPath = IW.Get_WMS_Waste_Line_By_Head_To_Excel(Line_List);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));

        }
    }
}