using EmpireOneRestAPIFHS.DataManager;
using EmpireOneRestAPIFHS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace EmpireOneRestAPIFHS.Services
{
    public class EmailSendService
    {
        public string SendEmail_SMTP_Register(string email, string userid, string useralias)
        {


            string smtppw = ConfigurationSettings.AppSettings["SMTPPW"];
            string htmlBody = BuildEmailBody(userid, useralias);

            try
            {
                EmailSender_Snm.Send(
                    toEmail: email,
                    subject: "Welcome to FireHorse Saloon",
                    htmlBody: htmlBody,
                    smtpPassword: smtppw,
                    fromEmail: "support@firehorseusa.com",  // Must match username
                    username: "support@firehorseusa.com",   // Must match fromEmail
                    port: 25,
                    enableSsl: false
                );

                // Use the passed DataAccess instance
               

                return $"✅ Email sent successfully to {email} - {useralias}";
            }
            catch (Exception ex)
            {
                return $"❌ Email failed to {email}: {ex.Message}";
            }
        }

        public string BuildEmailBody(string userId, string alias)
        {


            string formattedBody = string.Empty;

            string imageloc = "https://firehorsesaloon.com/wp-content/uploads/2024/07/firehorse_logo_stacked_text-copy-1024x927.png";


            // Define the HTML template with placeholders for the data.
            string emailTemplate = @"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif;
                    background-color: #f4f4f4; margin: 0; padding: 0; }}
                    .container {{ max-width: 600px; margin: 20px auto;
                    background-color: #ffffff; padding: 20px; border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                    .header {{ background-color: #000000; color: #ffffff;
                    padding: 10px; text-align: center; border-radius: 8px 8px 0 0; }}
                    .header img {{ max-width: 200px; height: auto; margin: 10px 0; }}
                    .content {{ padding: 20px; }}
                    .content p {{ line-height: 1.6; color: #333333; }}
                    .content strong {{ color: #000000; }}
                    .numbers {{ background-color: #f8f9fa; padding: 10px;
                    border-radius: 5px; margin: 10px 0; }}
                    .match-count {{ font-size: 18px; color: #007bff; font-weight: bold; }}
                    .footer {{ text-align: center; margin-top: 20px;
                    font-size: 12px; color: #888888; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                    <img src='{11}' alt='FireHorse Saloon Logo' />
                        <h2>FireHorse Saloon Welcome Email</h2>
                    </div>
                    <div class='content'>
                        <p><strong>Date:</strong> {9}</p>
                        <p>Welcome {1},</p>
                        
                        <p>This email is to confirm you have registered for FireHorse Saloon Membership </p>
                        
                        <p>FireHorse Saloon.com</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; {8} FireHorseSaloon.com. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";

            try
            {
                formattedBody = string.Format(
                     emailTemplate,
                     DateTime.Now.ToShortDateString(),     // {0}
                     alias,    // {1}
                     userId,     // {2}
     string.Empty,         // {3}
  string.Empty,            // {4}
        string.Empty,// {5}
     string.Empty,         // {6}
       string.Empty,      // {7}
     DateTime.Now.Year.ToString(),     // {8}
    DateTime.Now.ToShortDateString(),     // {9}
      string.Empty,   // {10}
   imageloc          // {11}
         );
            }
         catch (Exception ex)
    {
             string msg = ex.Message;
          }

            return formattedBody;
        }

        public static class EmailSender_Snm
        {
            public static void Send(
                string toEmail,
                string subject,
                string htmlBody,
                string smtpPassword,
                string fromEmail = "support@firehorseusa.com",  // Changed to match username
                string fromName = "FireHorse Notifications",
                string host = "firehorseusa.com",
                int port = 25, // you listed 25; try 587 or 465 if available
                string username = "support@firehorseusa.com",
                bool enableSsl = false // true for 587 (STARTTLS) or 465 (SSL); false for 25
            )
            {
                var from = new MailAddress(fromEmail, fromName);
                var to = new MailAddress(toEmail);

                using (var msg = new MailMessage(from, to))
                {
                    msg.Subject = subject;
                    msg.Body = htmlBody;
                    msg.IsBodyHtml = true;

                    using (var smtp = new System.Net.Mail.SmtpClient(host, port))
                    {
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = enableSsl;
                        smtp.Credentials = new NetworkCredential(username, smtpPassword);
                        smtp.Timeout = 15000;

                        smtp.Send(msg);
                    }
                }
            }

        }
    }
}