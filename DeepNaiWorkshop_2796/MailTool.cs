using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796
{
    class MailTool
    {
        public const int MAIL_TYPE_QQ = 1;//qq渠道邮件

        public void sendExceptionTo(int mailType,String title,String content)
        {

        }

        public static bool sendQQMail(string toMail, string body, string title, string Fname)
        {
            bool retrunBool = false;
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            string strFromEmail = Const.FROM_MAIL;//发送人邮箱  
            string strEmailPassword = "dgjpcwxnccdhdiih";//QQPOP3/SMTP服务码  
            try
            {
                mail.From = new MailAddress(strFromEmail);
                mail.To.Add(new MailAddress(toMail));
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Priority = MailPriority.Normal;
                mail.Body = body;
                mail.Subject = title;
                smtp.Host = "smtp.qq.com";
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(strFromEmail, strEmailPassword);
                //发送邮件  
                smtp.Send(mail);   //同步发送  
                retrunBool = true;
            }
            catch (Exception ex)
            {
                retrunBool = false;
            }
            // smtp.SendAsync(mail, mail.To); //异步发送 （异步发送时页面上要加上Async="true" ）  
            return retrunBool;
        }
    }
}
