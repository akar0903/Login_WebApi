using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace CommonLayer
{
    public class MailSend
    {
        public string SendMail(string ToEmail, string Token)
        {
            string FromEmail = "arunkarthik2018@gmail.com";
            MailMessage Message = new MailMessage(FromEmail, ToEmail);
            string MailBody = "Token generated: " + Token;
            Message.Subject = "Token generated for Forget Password";
            Message.BodyEncoding = Encoding.UTF8;
            Message.IsBodyHtml = true;
            SmtpClient SmtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credential = new NetworkCredential("av0019@srmist.edu.in", "ryca winh uwhu gzdu");
            SmtpClient.EnableSsl = true;
            SmtpClient.UseDefaultCredentials = true;
            SmtpClient.Credentials = credential;
            SmtpClient.Send(Message);
            return ToEmail;
        }
    }
}
