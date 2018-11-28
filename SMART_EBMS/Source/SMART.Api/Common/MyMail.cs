using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json;
using SMART.Api.Models;

namespace SMART.Api
{
    public static class NetMail
    {
        //发送邮件并保持邮件信息进数据库
        public static void SendNetMailSingle_Save_DB(string mailToAddress, string mailSubject, string mailBody, SentEmail CEB)
        {
            string Is_Error_Info = string.Empty;
            try
            {
                SendNetMailSingle(mailToAddress, mailSubject, mailBody, CEB);
            }
            catch (Exception Ex)
            {
                Is_Error_Info = Ex.Message.ToString();
            }

            try
            {
                SentEmailRecord R = new SentEmailRecord();
                R.E_Mail = mailToAddress;
                R.Subject = mailSubject;
                R.Body = mailBody;
                R.SMTP_Json = JsonConvert.SerializeObject(CEB);
                R.LinkMainCID = CEB.MainCID;
                if (!string.IsNullOrEmpty(Is_Error_Info))
                {
                    R.Is_Error = 1;
                    R.Is_Error_Info = Is_Error_Info;
                }
                else
                {
                    R.Is_Error = 0;
                    R.Is_Error_Info = string.Empty;
                }

                SmartdbContext db = new SmartdbContext();
                db.SentEmailRecord.Add(R);
                db.SaveChanges();
            }
            catch
            {
                //不做处理
            }

        }

        public static bool SendNetMailSingle(string mailToAddress, string mailSubject, string mailBody, SentEmail CEB)
        {
            string Domain = CEB.SMTP;
            string Port = CEB.Port;
            string MailName = CEB.MailName;
            string UserName = CEB.UserName;
            string Password = CEB.Password;

            //mailFromAddress 发件人地址
            //mailToAddress 收件人地址
            MailAddress MailFrom = new MailAddress(UserName, MailName);
            MailMessage message = new MailMessage();
            message.From = MailFrom;
            message.To.Add(mailToAddress);

            //设置邮件标题及编码及内容格式
            message.Subject = mailSubject;  //设置邮件标题
            message.Body = mailBody;  //设置邮件正文内容
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            //创建一个SmtpClient 类的新实例,并初始化实例的SMTP 事务的服务器
            SmtpClient client = new SmtpClient(Domain, Convert.ToInt32(Port));

            //身份认证
            client.Credentials = new System.Net.NetworkCredential(UserName, Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(message);
                return true;
            }
            catch (SmtpException Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
        }

        //发送邮件并保持邮件信息进数据库(多人接收)
        public static void SendNetMailSingle_Save_DB(List<string> mailToAddress_List, string mailSubject, string mailBody, SentEmail CEB)
        {
            string Is_Error_Info = string.Empty;
            try
            {
                SendNetMailSingle(mailToAddress_List, mailSubject, mailBody, CEB);
            }
            catch (Exception Ex)
            {
                Is_Error_Info = Ex.Message.ToString();
            }

            try
            {
                SentEmailRecord R = new SentEmailRecord();
                foreach (var mailToAddress in mailToAddress_List)
                {
                    R.E_Mail = mailToAddress + "/";
                }
                R.Subject = mailSubject;
                R.Body = mailBody;
                R.SMTP_Json = JsonConvert.SerializeObject(CEB);
                R.LinkMainCID = CEB.MainCID;
                if (!string.IsNullOrEmpty(Is_Error_Info))
                {
                    R.Is_Error = 1;
                    R.Is_Error_Info = Is_Error_Info;
                }
                else
                {
                    R.Is_Error = 0;
                    R.Is_Error_Info = string.Empty;
                }

                SmartdbContext db = new SmartdbContext();
                db.SentEmailRecord.Add(R);
                db.SaveChanges();
            }
            catch
            {
                //不做处理
            }

        }

        public static bool SendNetMailSingle(List<string> mailToAddress_List, string mailSubject, string mailBody, SentEmail CEB)
        {
            string Domain = CEB.SMTP;
            string Port = CEB.Port;
            string MailName = CEB.MailName;
            string UserName = CEB.UserName;
            string Password = CEB.Password;

            //mailFromAddress 发件人地址
            //mailToAddress 收件人地址
            MailAddress MailFrom = new MailAddress(UserName, MailName);
            MailMessage message = new MailMessage();
            message.From = MailFrom;
            foreach (var mailToAddress in mailToAddress_List)
            {
                message.To.Add(mailToAddress);
            }

            //设置邮件标题及编码及内容格式
            message.Subject = mailSubject;  //设置邮件标题
            message.Body = mailBody;  //设置邮件正文内容
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            //创建一个SmtpClient 类的新实例,并初始化实例的SMTP 事务的服务器
            SmtpClient client = new SmtpClient(Domain, Convert.ToInt32(Port));

            //身份认证
            client.Credentials = new System.Net.NetworkCredential(UserName, Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(message);
                return true;
            }
            catch (SmtpException Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
        }

        //发送邮件并保持邮件信息进数据库(含CC、Excel)
        public static void SendNetMailSingle_Save_DB_With_CC(List<string> mailToAddress_List, string mailSubject, string mailBody, SentEmail CEB, List<string> CC_List, string ExcelPath)
        {
            string Is_Error_Info = string.Empty;
            try
            {
                SendNetMailSingle_With_CC(mailToAddress_List, mailSubject, mailBody, CEB, CC_List, ExcelPath);
            }
            catch (Exception Ex)
            {
                Is_Error_Info = Ex.Message.ToString();
            }

            try
            {
                SentEmailRecord R = new SentEmailRecord();
                foreach (var mailToAddress in mailToAddress_List)
                {
                    R.E_Mail = mailToAddress + "/";
                }
                R.E_Mail = R.E_Mail.Substring(0, R.E_Mail.Length - 1);
                R.Subject = mailSubject;
                R.Body = mailBody;
                R.SMTP_Json = JsonConvert.SerializeObject(CEB);
                R.LinkMainCID = CEB.MainCID;
                if (!string.IsNullOrEmpty(Is_Error_Info))
                {
                    R.Is_Error = 1;
                    R.Is_Error_Info = Is_Error_Info;
                }
                else
                {
                    R.Is_Error = 0;
                    R.Is_Error_Info = string.Empty;
                }

                SmartdbContext db = new SmartdbContext();
                db.SentEmailRecord.Add(R);
                db.SaveChanges();
            }
            catch
            {
                //不做处理
            }

        }

        public static bool SendNetMailSingle_With_CC(List<string> mailToAddress_List, string mailSubject, string mailBody, SentEmail CEB, List<string> CC_List, string ExcelPath)
        {
            string Domain = CEB.SMTP;
            string Port = CEB.Port;
            string MailName = CEB.MailName;
            string UserName = CEB.UserName;
            string Password = CEB.Password;

            //mailFromAddress 发件人地址
            //mailToAddress 收件人地址
            MailAddress MailFrom = new MailAddress(UserName, MailName);
            MailMessage message = new MailMessage();
            message.From = MailFrom;
            foreach (var mailToAddress in mailToAddress_List)
            {
                message.To.Add(mailToAddress);
            }

            foreach (var CC in CC_List)
            {
                message.CC.Add(new MailAddress(CC));
            }

            //设置邮件标题及编码及内容格式
            message.Subject = mailSubject;  //设置邮件标题
            message.Body = mailBody;  //设置邮件正文内容
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(ExcelPath))
            {
                message.Attachments.Add(new Attachment(ExcelPath));
            }

            //创建一个SmtpClient 类的新实例,并初始化实例的SMTP 事务的服务器
            SmtpClient client = new SmtpClient(Domain, Convert.ToInt32(Port));

            //身份认证
            client.Credentials = new System.Net.NetworkCredential(UserName, Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(message);
                return true;
            }
            catch (SmtpException Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
        }

        //发送邮件并保持邮件信息进数据库(含Excel)
        public static void SendNetMailSingle_Save_DB_With_Excel(List<string> mailToAddress_List, string mailSubject, string mailBody, SentEmail CEB, string ExcelPath)
        {
            string Is_Error_Info = string.Empty;
            try
            {
                SendNetMailSingle_With_Excel(mailToAddress_List, mailSubject, mailBody, CEB,ExcelPath);
            }
            catch (Exception Ex)
            {
                Is_Error_Info = Ex.Message.ToString();
            }

            try
            {
                SentEmailRecord R = new SentEmailRecord();
                foreach (var mailToAddress in mailToAddress_List)
                {
                    R.E_Mail = mailToAddress + "/";
                }
                R.E_Mail = R.E_Mail.Substring(0, R.E_Mail.Length - 1);
                R.Subject = mailSubject;
                R.Body = mailBody;
                R.SMTP_Json = JsonConvert.SerializeObject(CEB);
                R.LinkMainCID = CEB.MainCID;
                if (!string.IsNullOrEmpty(Is_Error_Info))
                {
                    R.Is_Error = 1;
                    R.Is_Error_Info = Is_Error_Info;
                }
                else
                {
                    R.Is_Error = 0;
                    R.Is_Error_Info = string.Empty;
                }

                SmartdbContext db = new SmartdbContext();
                db.SentEmailRecord.Add(R);
                db.SaveChanges();
            }
            catch
            {
                //不做处理
            }

        }

        public static bool SendNetMailSingle_With_Excel(List<string> mailToAddress_List, string mailSubject, string mailBody, SentEmail CEB,string ExcelPath)
        {
            string Domain = CEB.SMTP;
            string Port = CEB.Port;
            string MailName = CEB.MailName;
            string UserName = CEB.UserName;
            string Password = CEB.Password;

            //mailFromAddress 发件人地址
            //mailToAddress 收件人地址
            MailAddress MailFrom = new MailAddress(UserName, MailName);
            MailMessage message = new MailMessage();
            message.From = MailFrom;
            foreach (var mailToAddress in mailToAddress_List)
            {
                message.To.Add(mailToAddress);
            }

            //设置邮件标题及编码及内容格式
            message.Subject = mailSubject;  //设置邮件标题
            message.Body = mailBody;  //设置邮件正文内容
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(ExcelPath))
            {
                message.Attachments.Add(new Attachment(ExcelPath));
            }

            //创建一个SmtpClient 类的新实例,并初始化实例的SMTP 事务的服务器
            SmtpClient client = new SmtpClient(Domain, Convert.ToInt32(Port));

            //身份认证
            client.Credentials = new System.Net.NetworkCredential(UserName, Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(message);
                return true;
            }
            catch (SmtpException Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
        }

    }

}