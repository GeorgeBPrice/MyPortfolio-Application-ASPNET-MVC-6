using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using PortfolioApp1.Models;
using Microsoft.Build.Framework;
using System.Net.NetworkInformation;
using System;
using Microsoft.Extensions.Options;
using System.Drawing.Text;
using System.Net;

namespace PortfolioApp1.Controllers
{
    public class ContactController : Controller
    {

        // Default contact view (loads the contact view)
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OnGet() 
        {
            return View();
        }

        // declare and inject the GoogleCaptchaKeys from GoogleCaptchaService
        private readonly GoogleCaptchaService _captchaService;

        // declare and inject the EmailAuthetication from appsetting JSON, to access it's paremeters
        private readonly IOptionsMonitor<EmailConfig> _emailConfig;

        // assign to locals
        public ContactController(GoogleCaptchaService captchaService, IOptionsMonitor<EmailConfig> emailConfig)
        {
            _captchaService = captchaService;
            _emailConfig = emailConfig;
        }

        // process POST contact-form processing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EmailModel formData)
        {
            // validate if all fields correctly filled
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            // then validate if Google reCAPTCHA reponse is valid (passed in with form data)
            var captchaResult = await _captchaService.VerifyCaptchaToken(formData.Token);
            
            // if status is not OK, return back to view and do not process sending email
            if (!captchaResult)
            {
                TempData["CaptchaError"] = "Sorry! Google anti-spam cannot verify your device. Please try again on another browser or device";
                return View(formData);
            }

            // once all validations pass, attempt to send the contact form message as an email
            try
            {
                // send email using Mailkit Smtp and Mimekit
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.mailtrap.io", 2525);
                    client.Authenticate(_emailConfig.CurrentValue.Account, _emailConfig.CurrentValue.Password);

                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = $"<p>Email: {formData.email}</p> <p>Phone: {formData.phone}</p> <p>Name: {formData.name}</p> <p>Message:</p> <p>{formData.message}</p>",
                        TextBody = "{formData.email} \r\n {formData.phone} \r\n {formData.name} \r\n {formData.message}"
                    };

                    var message = new MimeMessage
                    {
                        Body = bodyBuilder.ToMessageBody()
                    };
                    message.From.Add(new MailboxAddress(formData.name, formData.email));
                    message.To.Add(new MailboxAddress("Portfolio Message", "noreply@gprice.online"));

                    if (formData.subject == null)
                    {
                        formData.subject = "Portfolio Message";
                    }
                    message.Subject = formData.subject;

                    client.Send(message);
                    client.Disconnect(true);
                }

                // success message 
                TempData["Success"] = "Thank you " + formData.name + ", your message was successfully sent!";

            }
            // output error message if sending fails
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                TempData["Fail"] = "Sorry something went wrong, your message was not sent!";
            }

            // finally return focus back to the contact form view
            return RedirectToAction("Index");
        }
    }
}
