using System;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace Core.EmailManager
{
    public class EmailManager
    {
        public static WedcooEmailVM info = new WedcooEmailVM { Email = "info@wedcoo.com", Password = "KeEHq6816prR" };
        public static WedcooEmailVM sales = new WedcooEmailVM { Email = "sales@wedcoo.com", Password = "" };
        public static WedcooEmailVM noreply = new WedcooEmailVM { Email = "noreply@wedcoo.com", Password = "" };

        #region Admin
        public static void AdminVerifyEmail(string Email, string link, string templatePath)
        {
            try
            {
                string body = string.Empty;
                body = File.ReadAllText(Path.Combine(templatePath + "AdminEmailVerification.html")).ToString();
                body = body.Replace("{Wedcoo-Link}", link);
                body = body.Replace("{Wedcoo-Email}", Email);
                MailMessage message = new MailMessage("info@wedcoo.com", Email, "Wedcoo Account Created !", body);
                SmtpClient client = new SmtpClient("smtp.zoho.com", 587);
                //SmtpClient client = new SmtpClient("smtppro.zoho.com", 465);
                client.EnableSsl = true;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("info@wedcoo.com", "KeEHq6816prR"); // These are Real Credentials For Now
                client.Send(message);
                Console.WriteLine("email success");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ForgetPassword(string Email, string link,string templatePath)
        {
            try
            {
                string body = string.Empty;
                body = File.ReadAllText(Path.Combine(templatePath + "ForgetPassword.html")).ToString();
                body = body.Replace("{Wedcoo-link}", link);
                MailMessage message = new MailMessage(info.Email, Email, "Reset Password !", body);
                SmtpClient client = new SmtpClient("smtp.zoho.com", 587);
                client.EnableSsl = true;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(info.Email, info.Password); // These are Real Credentials For Now
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Public

        public static string SendUserVerificationEmail(string Email, string link, string templatePath, string subject)
        {
            try
            {
                string body = string.Empty;
                body = File.ReadAllText(Path.Combine(templatePath + "UserVerification.html")).ToString();
                body = body.Replace("{VerificationLinkToUpdate}", link);
                body = body.Replace("{userEmail}", Email);
                MailMessage message = new MailMessage("info@wedcoo.com", Email, subject, body);
                SmtpClient client = new SmtpClient("smtp.zoho.com", 587);
                //SmtpClient client = new SmtpClient("smtppro.zoho.com", 465);
                client.EnableSsl = true;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("info@wedcoo.com", "KeEHq6816prR"); // These are Real Credentials For Now
                client.Send(message);
                Console.WriteLine("email success");
                return "email success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                //Console.WriteLine(ex.Message);
            }
        }

        #endregion

    }

    public class WedcooEmailVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
