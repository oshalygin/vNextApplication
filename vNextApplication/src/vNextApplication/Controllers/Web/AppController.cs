using Microsoft.AspNet.Mvc;
using vNextApplication.Services;
using vNextApplication.ViewModels;

namespace vNextApplication.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailservice;

        public AppController(IMailService service)
        {
            _mailservice = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Could not send email, configuration problem");
                }


                if (_mailservice.SendMail(
                    email,
                    model.Email,
                    $"Contact Page from {model.Name} ({email})",
                    model.Message))
                {
                    ModelState.Clear();

                    ViewBag.Message = "Mail Sent. Thanks!";
                }
            }


            return View();
        }
    }
}