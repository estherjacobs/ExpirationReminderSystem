using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateRepository;
using System.Net.Mail;
using System.Net;
using System.IO;
using RazorEngine;
using RazorEngine.Templating;



namespace TaskScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            //EmailNotification s = new EmailNotification
            //{
            //    Name = ,
            //    Category = ,
            //    ExpirationDate = ,
            //    FrontImage = ,
            //};
            //var template = File.ReadAllText("EmailBodyFormatted.cshtml");
            //string result = Engine.Razor.RunCompile(template, "templateKey", null, s);
            //Console.WriteLine(result);
            //Console.ReadKey(true);

            var mgr = new TaskRepository();
            IEnumerable<Reminder> Items = mgr.GetAllTodaysReminders();
            foreach (Reminder u in Items)
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
                var emailHtmlBody = Engine.Razor.RunCompile(template, "templateKey", null, e);
                var fromAddress = new MailAddress("estherjacobs110@gmail.com", "From Esther");
                var toAddress = new MailAddress(/*u.User.Email*/ u.User.Email, "To " + u.User.FullName);
                const string fromPassword = "Esther110";
                const string subject = "Expiration Reminder";
                string body = emailHtmlBody;

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
            Console.ReadKey(true);

        }

        //dont uncomment!!


        //public string PopulateBody(string name, DateTime date, string category)
        //{
        //    string body = string.Empty;
        //    using (StreamReader reader = new StreamReader(Server.MapPath("/EmailBodyFormatted.html")))
        //    {
        //        body = reader.ReadToEnd();
        //    }
        //    body = body.Replace("{UserName}", name);
        //    body = body.Replace("{ItemCategory}", category);
        //    body = body.Replace("{ExpirationDate}", date.ToLongDateString());
        //    return body;
        //}
    }
    
}
