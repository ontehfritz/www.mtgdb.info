using System;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Configuration;

namespace MtgDb.Info
{
    public class Email
    {
        private static string _from =        ConfigurationManager.AppSettings.Get("smtp:Logon");
        private static string _password =    ConfigurationManager.AppSettings.Get("smtp:Password");
        private static string _smtp =        ConfigurationManager.AppSettings.Get("smtp:Server");
        private static string _port =        ConfigurationManager.AppSettings.Get("smtp:Port");
       
        public static void Send(string to, string subject, string body){
            var smtp = new SmtpClient
            {
                Host = _smtp,
                Port = int.Parse(_port),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_from, _password)
            };

            using (var message = new MailMessage(_from, to)
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

