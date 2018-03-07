using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication;
using Microsoft.AspNet.Identity;

namespace DEA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApplication.BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            // Get the exception object.
            Exception exc = Server.GetLastError();

            string message = exc.Message;
            string stacktrace = exc.StackTrace;
            var userName = User.Identity.GetUserName();
            var ipaddress = GetIP();

            string MailMessage = "<div><b>UserName:</b> " + userName + " <br /><b>IpAddress:</b> " + ipaddress + " <br /><b>Source:</b> " + exc.Source + " <br /><b>Exception:</b> " + exc.Message+ "// //" + exc.Data + " <br /><b>InnerException:</b> " + exc.InnerException+ " <br /><b>StackTrace:</b> " + exc.StackTrace+" </div>";

            var result = SendEmail("mr.salman.rao@gmail.com", "exception of SEA", MailMessage);
        }

        public string GetIP()
        {
            string Str = "";
            Str = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Str);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();

        }

        public bool SendEmail(string toEmail, string subject, string emailBody)
        {
            try
            {
                //var senderEmail = "azeemazeem187@gmail.com";
                //var senderPassword = "*********";

                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                client.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
