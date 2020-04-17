using System.Web.Mvc;
using JobBoardFinalProject.UI.MVC.Models;
using System.Net;
using System.Net.Mail;
using System;

namespace JobBoardFinalProject.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        //POST action processes the form
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Contact(ContactViewModel contactForm)
        {
            if (!ModelState.IsValid)
            {
                return View(contactForm);
            }

            string message = $"Email from {contactForm.Name} with a subject of \"{contactForm.Subject}\".<br />" + $"Please respond to {contactForm.Email}. Message:" + $"<blockquote>{contactForm.Message}</blockquote><em>Generated {DateTime.Now}</em>";

            MailMessage mm = new MailMessage(
                "no-reply@emmalintz.com",
                "emma_lintz@outlook.com",
                contactForm.Subject,
                message
                );

            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.High;
            mm.ReplyToList.Add(contactForm.Email);

            SmtpClient emailer = new SmtpClient("mail.emmalintz.com");

            emailer.Credentials = new NetworkCredential(
                "no-reply@emmalintz.com",
                "Centriq#20");

            try
            {
                emailer.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Sorry, something went wrong. Please try again later, or email me directly at emma_lintz@outlook.com.<br>" + $"Error msg: <blockquote>{ex.Message} - {ex.GetType()} - {ex.StackTrace}</blockquote>";

                return View(contactForm);
            }

            return View("EmailConfirmation", contactForm);
        }


    }
}
