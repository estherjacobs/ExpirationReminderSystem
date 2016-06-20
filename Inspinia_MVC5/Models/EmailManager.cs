using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CertificateRepository;

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
            var fromAddress = new MailAddress("estherjacobs110@gmail.com", "Expiration Reminder App");
            var toAddress = new MailAddress(/*u.User.Email*/ "esther@1on1development.com", "To " + name);
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
            var fromAddress = new MailAddress("estherjacobs110@gmail.com", "Expiration Reminder App");
            var toAddress = new MailAddress(/*u.User.Email*/ "esther@1on1development.com", "To " + name);
            const string fromPassword = "Esther110";
            string subject = "Welcome to Exp Reminder App";
            string body = "Welcome to Expiration Reminder App! Thank you for joining us " + name + ". Here's the overview of how your Expiration Reminder account works.";
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
            var fromAddress = new MailAddress("estherjacobs110@gmail.com", "Expiration Reminder App");
            var toAddress = new MailAddress(/*u.User.Email*/ "esther@1on1development.com", "To new member");
            const string fromPassword = "Esther110";
            string subject = "Invitation to create user";
            string body = "Hi, you have been invited by " + o.Name + " to become a member of their organization at Expiration Reminder App. Follow this link to setup your account. http://localhost:58893/members/link?token=" + token;
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
