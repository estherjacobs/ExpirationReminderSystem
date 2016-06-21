using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CertificateRepository;
using System.IO;
using RazorEngine;
using RazorEngine.Templating;

namespace Inspinia_MVC5.Models
{
    public class EmailManager
    {
        public void ResetPassword(string email)
        {
            var mgr = new ResetPasswordRepository();
            PasswordToken p = mgr.CreateToken(email);
            User u = mgr.GetUser(email);
            SendPasswordEmail(p.guid, u.FullName, u.Email);
        }
        public void SendPasswordEmail(string Token, string name, string email)
        {
            var fromAddress = new MailAddress("expirationtracking@gmail.com", "Expiration Tracking App");
            var toAddress = new MailAddress(email, "To " + name);
            const string fromPassword = "Esther110";
            string subject = "Reset Password Link";
            string body = "Hi " + name + ", to reset your password please click the following link " + "http://localhost:58893/pages/resetauth?token=" + Token;
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
        public void SendWelcomeEmail(string name, string email)
        {
            //var template = File.ReadAllText("WelcomeEmail.cshtml");
            //var result = Engine.Razor.RunCompile(template, "templateKey", null, new { Name = name });
            var fromAddress = new MailAddress("expirationtracking@gmail.com", "Expiration Tracking App");
            var toAddress = new MailAddress(email, "To " + name);
            const string fromPassword = "Esther110";
            string subject = "Welcome to Expiration Tracking App";
            string body = "Hi " + name + ", Welcome to Expiration Tracking App. You’ve officially taken the first step toward the pleasure of sitting back knowing your expiration dates are handled. You’ll have more accountability, a happier and more productive team, and save time and frustration. Our Tracking tool is incredibly easy-to-use and easy-to-learn." +
            " View the entire list of expiration items, change expiration dates, add new ones, attach documents and images related to the item and feel free that everything will" +
            " be tracked and properly notified.If you run into any snags along the way, let us know.We’re here to help. Don’t hesitate to email us if you have any questions." +
            " Happy expiration tracking,      Expiration Tracking App;)      expirationtracking@gmail.com";

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
        public void SendInvitationMemberEmail(string email, string token, Organization o)
        {
            var fromAddress = new MailAddress("expirationtracking@gmail.com", "Expiration Tracking App");
            var toAddress = new MailAddress(email, "To new member");
            const string fromPassword = "Esther110";
            string subject = "Invitation to create user";
            string body = "Hi, you have been invited by " + o.Name + " to become a member of their organization at Expiration Tracking App. Follow this link to setup your account. http://certs.lakewoodfirstaid.org/members/link?token=" + token;
            
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
