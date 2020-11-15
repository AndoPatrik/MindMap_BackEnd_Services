using System;
using System.Net;
using System.Net.Mail;

namespace MindMap_Email_API.Utils
{
    public class EmailManager
    {
        public static void CreateEmail(string id, string targetEmail, string secret) 
        {
            string to = targetEmail;
            string content = @"Activate your account by clicking: http://localhost:5001/activate/" + id + " or http://mindmap.ddns.net:5001/activate/ " + id;
            string senderPw = secret;
            string senderEmail = "mindmapper.agent@gmail.com";

            SmtpClient smtpClient;
            MailMessage msg = new MailMessage(senderEmail, to);

            try
            {
                smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPw);

                msg.Subject = "Get started with your account!";
                msg.Body = content;
                msg.IsBodyHtml = true;

                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
            finally 
            {
                if (msg != null) msg.Dispose();
            }
        }
    }
}
