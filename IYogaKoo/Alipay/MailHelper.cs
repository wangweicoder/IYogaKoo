using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo
{
    public static class MailHelper
    {
        /// <summary>
        /// 调用NetMail的发送邮件方法
        /// </summary>
        /// <param name="Pto">发送给。。</param>
        /// <param name="PTitle">邮件标题</param>
        /// <param name="Pbody">邮件内容</param>
        /// <returns></returns>
        public static bool SendNetMail(SendEmailModel email)
        {
            ////ArrayList Ppaths为附件信息如果有附件则添加附件参数
            string from = ConfigurationManager.AppSettings["Email"];//发件箱
            string pwd = ConfigurationManager.AppSettings["EmailPWD"];//发件箱密码
            string smtp = ConfigurationManager.AppSettings["SMTP"];//邮件服务器SMTP

            SmtpClient client = new SmtpClient();
            client.Host = smtp;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(from, pwd);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 25;
            client.EnableSsl = false;
            client.Timeout = 9999;

            MailMessage message = new MailMessage(from, email.To);
            message.Subject = email.Title;
            message.Body = email.Body;
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.From = new MailAddress(from, email.SenderName);
            try
            {
                client.Send(message);

                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}
