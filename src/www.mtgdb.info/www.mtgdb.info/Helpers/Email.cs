using System;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace MtgDb.Info
{
    public class Email
    {
        private static string from = "planeswalker@mtgdb.info";
        private static string password = "password";
       
        public static void send(string to, string subject, string body){
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, password)
            };

            using (var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body
                })
            {

                ServicePointManager.ServerCertificateValidationCallback = 
                    delegate(object s, X509Certificate certificate, X509Chain chain, 
                        SslPolicyErrors sslPolicyErrors) 
                { return true; };

                smtp.Send(message);
            }
        }
    }
}

