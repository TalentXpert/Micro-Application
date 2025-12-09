using BaseLibrary.Utilities.Emails;
using BaseLibrary.Utilities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Services
{
    public interface IEmailApplicationService
    {
        void SendActivationEmail(ApplicationUser user, string newPassword);
        // void SendEmailAssignedTesterToTestCycle(ApplicationUser sender, ApplicationUser reciever, TestCycle testCycle);
        void SendEmailToNewlyAddedUserInApplication(string title, ApplicationUser loggedInUser, ApplicationUser user, string newPassword);
        void SendResetPasswordEmail(ApplicationUser user, string newPassword);
        // void SentEmailToSupport(ContactUsVM contactUsVM);
        void SendEmailAddedUserInApplication(string title, ApplicationUser loggedInUser, ApplicationUser applicationUser);
    }
    public class EmailApplicationService(IEmailSender emailSender) : IEmailApplicationService
    {
        private readonly IEmailSender emailSender = emailSender;
        private readonly string emailTemplate = new AssemblyEmbededFileReader().GetScript(Assembly.GetExecutingAssembly(), "EmailTemplate.txt");

        private string PrepareEmailBody(string username, string message)
        {
            var upatedTemplate = this.emailTemplate.Replace("[{(UserName)}]", username);
            upatedTemplate = upatedTemplate.Replace("[{(UserMessage)}]", message);
            return upatedTemplate;
        }

        public void SendActivationEmail(ApplicationUser user, string newPassword)
        {
            var body = EmailMessageTemplate.WelcomeEmailForNewUser(user, newPassword);
            body = PrepareEmailBody(user.Name, body);
            emailSender.Send(null, user.Email, "BioMeta AI Registration", body);
        }
        //public void SendEmailAssignedTesterToTestCycle(ApplicationUser sender, ApplicationUser reciever, TestCycle testCycle)
        //{
        //    var body = EmailMessageTemplate.AssignTestCycle(sender.Name, testCycle.Name);
        //    body = PrepareEmailBody(reciever.Name, body);
        //    emailSender.Send(null, reciever.LoginEmail, "Test cycle Assignment", body);
        //}

        public void SendEmailToNewlyAddedUserInApplication(string title, ApplicationUser loggedInUser, ApplicationUser user, string newPassword)
        {
            var body = EmailMessageTemplate.AddedToAnApplication(loggedInUser.Name, title, user, newPassword);
            body = PrepareEmailBody(user.Name, body);
            emailSender.Send(null, user.Email, $"Invitation to join {title} team", body);
        }

        public void SendResetPasswordEmail(ApplicationUser user, string newPassword)
        {
            var body = EmailMessageTemplate.ResetPasswordEmail(user, newPassword);
            body = PrepareEmailBody(user.Name, body);
            emailSender.Send(null, user.Email, "BioMeta Reset Password", body);
        }

        //public void SentEmailToSupport(ContactUsVM contactUsVM)
        //{
        //    var body = $"{contactUsVM.Name} having mobile {contactUsVM.Mobile} send this email from {contactUsVM.Email} <br/><br/>{contactUsVM.Description}";
        //    emailSender.Send(null, "support@almxpert.com", "Email From Contact Us Page", body);
        //}

        public void SendEmailAddedUserInApplication(string title, ApplicationUser loggedInUser, ApplicationUser applicationUser)
        {
            var body = EmailMessageTemplate.ExistingUserAddedToAnApplication(loggedInUser.Name, title, applicationUser);
            body = PrepareEmailBody(applicationUser.Name, body);
            emailSender.Send(null, applicationUser.Email, $"Invitation to join {title} team", body);
        }
    }
    public class EmailMessageTemplate
    {
        public static string WelcomeEmailForNewUser(ApplicationUser user, string newPassword)
        {
            string body;
            body = "Thank you for signing up and joining  BioMeta AI — Redefining Protocol-to-Database Intelligence application network.<br/>";
            body += "We have finished setting up your new account which you can now login with below information.<br/><br/>";
            body += "\t Your login Id: <b> Your Registered Email Id </b><br/>";
            body += "\t Your Password: <b>" + newPassword + "</b><br/><br/>";
            body += "Please change your temporary password after first login for security reasons. <br/>";
            body += "<br/><br/>";
            return body;
        }

        public static string ResetPasswordEmail(ApplicationUser user, string newPassword)
        {
            string body;
            body = "We have received a request to reset your password.If you did not make the request, Please contact our support team or your administrator immediately.<br/>";
            body += "Below is your new login password<br/><br/>";
            body += "\t Your login Id: <b> Your Registered Email Id </b><br/>";
            body += "\t Your Password: <b>" + newPassword + "</b><br/><br/>";
            body += "Please change your temporary password after first login for security reasons. <br/>";
            body += "<br/><br/>";
            return body;
        }

        public static string AddedToAnApplication(string sendBy, string study, ApplicationUser user, string newPassword)
        {
            string body;
            body = $"{sendBy}, invite you to join {study} team on BioMeta AI — Redefining Protocol-to-Database Intelligence application.<br/>";
            body += "We have finished setting up your new account which you can now login with below information.<br/><br/>";
            body += "\t Your login Id: <b> Your Registered Email Id </b><br/>";
            body += "\t Your Password: <b>" + newPassword + "</b><br/><br/>";
            body += "Please change your temporary password after first login for security reasons. <br/>";
            body += "<br/><br/>";
            return body;
        }

        public static string ExistingUserAddedToAnApplication(string sendBy, string study, ApplicationUser user)
        {
            string body;
            body = $"{sendBy}, invite you to join {study} team on BioMeta AI — Redefining Protocol-to-Database Intelligence application.<br/>";
            body += "We have finished setting up your new subscription which you can now login with below login.<br/><br/>";
            body += "\t Your login Id: <b> Your Registered Email Id </b><br/>";

            body += "In case you forgot your password Please use forgot password option to get new password.<br/>";
            body += "<br/><br/>";
            return body;
        }


        private static string PrepareLink(string url, string anchorText)
        {
            return $"<a target = '_blank' href = \"{url}\" align = \"left\" style = \"border-radius:4px; background: #02669a; padding: 20px 10px; color: #ffffff;\"> {anchorText}</a>";
        }

        public static string AssignTestCycle(string testLead, string testCycle)
        {
            string body;
            body = $"You have been assigned a test cycle by {testLead}<br/><br/>";
            body += $"\t Test Cycle: <b>{testCycle}</b><br/>";
            body += "<br/><br/>";
            return body;
        }
    }
}
