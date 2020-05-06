using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DigiFyy.DataService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DigiFyy.Helpers
{
    public  class EmailSender
    {
        //public async Task SendEmailAWS(string subject, string htmlBody, string textBody, List<string> recipients)
        //{
        //    await SendEmailFromClient(subject, htmlBody, recipients);
          
        //    using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUNorth1))
        //    {
        //        var sendRequest = new SendEmailRequest
        //        {
        //            Source = Constants.EmailSenderAddress,
                    
        //            Destination = new Destination
        //            {
        //                ToAddresses = recipients
        //            },
        //            Message = new Message
        //            { 
                         
        //                Subject = new Content(subject),
        //                Body = new Body
        //                {
        //                    Html = new Content
        //                    {
        //                        Charset = "UTF-8",
        //                        Data = htmlBody
        //                    },
        //                    Text = new Content
        //                    {
        //                        Charset = "UTF-8",
        //                        Data = textBody
        //                    }
        //                }
        //            },
        //            // If you are not using a configuration set, comment
        //            // or remove the following line 
        //            //ConfigurationSetName = configSet
        //        };
        //        try
        //        {
        //            Console.WriteLine("Sending email using Amazon SES...");
        //            var response = await client.SendEmailAsync(sendRequest);
        //            Console.WriteLine("The email was sent successfully.");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("The email was not sent.");
        //            Console.WriteLine("Error message: " + ex.Message);

        //        }
        //    }

        //}

        /*
         *  ses-smtp-user.20200505-212151 
SMTP Username: 
AKIAVDQTIQUA5BCTA7YB 
SMTP Password: 
BEKMmT7ECLgld436Gcfh54LRxD5u9VzsWuCqnZLK1oX3 */

        public bool SendEmailSMTP(string subject, string htmlBody,  List<string> recipients)
        {
            MailMessage message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(Constants.EmailSenderAddress),
                Subject = subject,
                Body = htmlBody
            };
            foreach (string recipient in recipients)
                message.To.Add(new MailAddress(recipient));

            // Comment or delete the next line if you are not using a configuration set
            //message.Headers.Add("X-SES-CONFIGURATION-SET", "ConfigSet");

            using (var client = new System.Net.Mail.SmtpClient(Constants.SmtpHost, Constants.SmtpPort))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(Constants.SmtpUsername, Constants.SmtpPassword);
                // Enable SSL encryption
                client.EnableSsl = true;
                // Try to send the message. Show status in console.
                try
                {
                    Console.WriteLine("Attempting to send email...");
                    client.Send(message);
                    Console.WriteLine("Email sent!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);
                    return false;
                }

                return true;
            }


        }


        public async Task SendEmailFromClient(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }
    }
}
