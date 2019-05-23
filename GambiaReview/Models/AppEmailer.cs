using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GambiaReview.Models
{
    public class AppEmailer
    {
        public static void SendEmail(string from_email, string from_name, string to_email, string to_name, string to_subject, string h1_text, string h2_text, 
            string paragraph_1, string paragraph_2, string salutation, string sender_name, string sender_position, string sender_number, string sender_email)
        {
            string button_css = @"border: none;
                                color: white;
                                padding: 15px 32px;
                                text-align: center;
                                text-decoration: none;
                                display: inline-block;
                                font-size: 16px;
                                margin: 4px 2px;
                                cursor: pointer;";

            string dev_enviroment = Enviroment("Testing"); //Testing OR Production
            String todays_date = DateTime.Now.ToString("yyyy.MM.dd");
            //Send Email
            GMailer.GmailUsername = GMailer.GetSMTPUserName();
            GMailer.GmailPassword = GMailer.GetSMTPPassword();

            GMailer mailer = new GMailer();

            mailer.ToEmail = to_email;
            mailer.Subject = to_subject;
            var message_body = "";
            message_body += "<table rules='all' cellpadding='10' border='1'  style='border:2px solid #FFEBCD; max-width:600px; '>";
            message_body += "<tr><td><a href='#' target='_blank'><div align='center' style='border:2px solid #f5deb3; background-color:#855927; color:white; '><img src='https://via.placeholder.com/728x90.png/09f/fff?text=Gambia+Review' alt='GambiaReview Logo'></div></a></td></tr>";
            //--//--//
            if (!string.IsNullOrEmpty(h1_text))
            {
                message_body += "<tr style='background: #f5f5f5;'><td colspan='2'> <h2 style='color:#000000; text-align: center;'>" + h1_text + "</h2> </td></tr>";
                message_body += "<br/> ";
            }
            //--//--//
            if (!string.IsNullOrEmpty(h2_text))
            {
                message_body += "<h4 style='color:#000000'>" + h2_text + "</h4>";
                message_body += "<br/> ";
            }
            //--//--//
            if (!string.IsNullOrEmpty(to_name))
            {
                message_body += "Dear <strong style='color:#000000'>" + to_name + ",</strong> ";
                message_body += "<br/><br/> ";
            }
            //--//--//
            if (!string.IsNullOrEmpty(paragraph_1))
            {
                message_body += "<p>" + paragraph_1 + "</p>";
                message_body += "<br/> ";
            }
            //--//--//
            if (!string.IsNullOrEmpty(paragraph_2))
            {
                message_body += "<p>" + paragraph_2 + "</p> ";
                message_body += "<br/> ";
            }
            //--//--//
            if (!string.IsNullOrEmpty(salutation))
            {
                message_body += "<p>" + salutation + "</p>";
            }
            else
            {
                message_body += "<p> Regards, <br/> Development Team</p>";
            }
            //--//--//
            if (!string.IsNullOrEmpty(sender_name))
            {
                message_body += "<p> Regards, <br/> " + sender_name + "</p>";
            }
            else
            {
                message_body += "<p>Gambia Review Team</p>";
            }
            //--//--//
            if (!string.IsNullOrEmpty(sender_position))
            {
                message_body += "<p>" + sender_position + "</p>";
            }
            //--//--//
            if (!string.IsNullOrEmpty(sender_number))
            {
                message_body += "<p> Tel. " + sender_number + "</p>";
            }
            else
            {
                message_body += "<p>Tel. +90 531 49 50226</p>";
            }
            //--//--//
            if (!string.IsNullOrEmpty(sender_email))
            {
                message_body += "<p> Tel. " + sender_email + "</p>";
            }
            else
            {
                message_body += "<p>Email. <a href='mailto:developerkassama@gmail.com?Subject=User%20Enquiry' target='_top'>developerkassama@gmail.com</a></p>";
            }
            message_body += "<p>Website. <a href='https://www.gambiareview.com' target='_blank'>https://www.gambiareview.com</a></p>";
            message_body += "<br/> ";
            message_body += "<br/> ";
            message_body += "<a href='#'><button style='"+button_css+ " background-color: #3b5998;'>Facebook</button></a>";
            message_body += "<a href='#'><button style='" + button_css + " background-color: #c08d64;'>Instagram</button></a>";
            message_body += "</table>";
            message_body += "</body></html>";
            mailer.Body = message_body;
            mailer.IsHtml = true;
            try
            {
                mailer.Send();
            }
            catch (Exception ex)
            {
                //Log Error
                SecurityFunctions.LogError(ex, from_email, "SendEmail", null);
            }
        }

        //Get dev. enviroment
        public static string Enviroment(string development)
        {

            if (development == "Testing")
            {
                return "http://localhost:56227/";
            }
            else if (development == "Production")
            {
                return "http://www.gambiareview.com/";
            }

            return "http://www.gambiareview.com/";
        }
    }
}