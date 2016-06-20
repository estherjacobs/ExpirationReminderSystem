using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using CertificateRepository;
using System.Net.Mail;
using System.Net;
using System.IO;
using RazorEngine;
using RazorEngine.Templating;


namespace TaskScheduler
{
    public class Program
    {
        static void Main(string[] args)
        {
            var mgr = new TaskRepository();
            IEnumerable<Reminder> Items = mgr.GetAllTodaysReminders();
            foreach (Reminder u in Items)
            {
                bool IsContact = false;

                if(u.User.Contacts.FirstOrDefault(i => i.UserId == u.User.Id) != null)
                {
                    IsContact = true;
                }

                if (u.User.ViaText == true)
                {
                    SendSMS(u.User.PhoneNumber, u.User.FullName, "This is a notification to let you know that your " + u.ExpirationItem.Category.Name + " is about to expire on " + u.ExpirationItem.ExpirationDate.ToLongDateString());

                    if (IsContact)
                    {
                        SendSMS(u.User.PhoneNumber, u.User.FullName, "This is a notification to let you know that you are a contact for " + u.User.FullName + ", who's " + u.ExpirationItem.Category.Name + " is about to expire on " + u.ExpirationItem.ExpirationDate.ToLongDateString());
                    }

                }

                if(u.User.ViaEmail == true)
                {
                    SendEmail(u);

                    if(IsContact)
                    {
                        SendEmail(u);
                    }

                }

            }
            Console.ReadKey(true);
        }

        private static void SendSMS(string phoneNumber, string name, string message)
        {
            string html = string.Empty;
            string url = "https://rest.nexmo.com/sms/json?api_key=b1ee06e1&api_secret=94dc48fb49e1dae6&from=12027353717&to=" + "17322376594" + "&text=" + message;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                html = reader.ReadToEnd();
            }
        }
        private static void SendEmail(Reminder u)
        {
            EmailNotification e = new EmailNotification
            {
                Name = u.User.FullName,
                Category = u.ExpirationItem.Category.Name,
                ExpirationDate = u.ExpirationItem.ExpirationDate.ToLongDateString(),
                ItemName = u.ExpirationItem.Name,
                Note = u.ExpirationItem.Notes,
                //FrontImage = u.ExpirationItem.Images.FirstOrDefault(i => i.ExpirationItemId == u.ExpirationItemId && i.IsBack != true).Path,
            };
            var template = File.ReadAllText("EmailBodyFormatted.cshtml");
            var result = Engine.Razor.RunCompile(template, "templateKey", null, e);
            var fromAddress = new MailAddress("estherjacobs110@gmail.com", "From Esther");
            var toAddress = new MailAddress(/*u.User.Email*/ "esther@1on1development.com", "To " + u.User.FullName);
            const string fromPassword = "Esther110";
            const string subject = "Expiration Reminder";
            var body = result;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}


