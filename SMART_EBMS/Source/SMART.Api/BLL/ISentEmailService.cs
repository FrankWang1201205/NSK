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
        void Sent_To_sales_With_WMS_Out_Inspection(Guid Head_ID);
        void Sent_To_sales_With_WMS_Out_Finish(Guid Head_ID);
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
            mailBody += "<h5>收货日期：" + Head.In_DT .ToString("yyyy-MM-dd") + "</h4>";
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
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</p>";

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
            mailBody += "<h5>收货日期：" + Head.In_DT.ToString("yyyy-MM-dd") + "</h4>";
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
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</p>";

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
        public void Sent_To_sales_With_WMS_Out_Inspection(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null"); }

            string mailSubject = Head.Customer_Name + "出库验货完成";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>发货日期：" + Head.Out_DT.ToString("yyyy-MM-dd") + "</h4>";
            if (!string.IsNullOrEmpty(Head.Logistics_Company))
            {
                mailBody += "<h5>物流公司：" + Head.Logistics_Company + "</h5>";
            }
            mailBody += "<h5>运输方式：" + Head.Logistics_Mode + "</h5>";
            mailBody += "<h5>客户名称：" + Head.Customer_Name + "</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</p>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.UserFullName == Head.Create_Person).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);

            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB(mailToAddress_List, mailSubject, mailBody, SE));
        }

        public void Sent_To_sales_With_WMS_Out_Finish(Guid Head_ID)
        {
            WMS_Out_Head Head = db.WMS_Out_Head.Find(Head_ID);
            if (Head == null) { throw new Exception("WMS_Out_Head is null"); }

            string mailSubject = Head.Customer_Name + "出库送货完成";
            string mailBody = string.Empty;
            mailBody += "<h5>任务编号：" + Head.Task_Bat_No_Str + "</h5>";
            mailBody += "<h5>发货日期：" + Head.Out_DT.ToString("yyyy-MM-dd") + "</h4>";
            if (!string.IsNullOrEmpty(Head.Logistics_Company))
            {
                mailBody += "<h5>物流公司：" + Head.Logistics_Company + "</h5>";
            }
            mailBody += "<h5>运输方式：" + Head.Logistics_Mode + "</h5>";
            mailBody += "<h5>客户名称：" + Head.Customer_Name + "</h5>";
            mailBody += "<hr/>";
            mailBody += "<h5>此邮件为系统自动发送邮件，请勿回复！</p>";

            User U = db.User.Where(x => x.LinkMainCID == Head.LinkMainCID && x.UserFullName == Head.Create_Person).FirstOrDefault();

            string MailToAddress = U.Email;
            List<string> mailToAddress_List = new List<string>();
            mailToAddress_List.Add(MailToAddress);
            SentEmail SE = this.Get_SentEmail(Head.LinkMainCID);
            IWmsService IW = new WmsService();

            string ExcelPath = IW.Get_Out_Task_List_To_Excel(Head.Head_ID);
            Task.Factory.StartNew(() => NetMail.SendNetMailSingle_Save_DB_With_Excel(mailToAddress_List, mailSubject, mailBody, SE, ExcelPath));

        }
    }
}