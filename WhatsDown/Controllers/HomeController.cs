using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WhatsDown.Core.Domain;
using WhatsDown.Persistence;

namespace WhatsDown.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Application DB context
        /// </summary>
        protected WhatsDownDBContext ApplicationDbContext
        {
            get; set;
        }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<User> UserManager
        {
            get; set;
        }

        public HomeController()
        {
            this.ApplicationDbContext = new WhatsDownDBContext();
            this.UserManager = new UserManager<User>(new UserStore<User>(this.ApplicationDbContext));
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            WhatsDownDBContext context = new WhatsDownDBContext();
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            context.Tests.Add(new Test()
            {
                Title = "blaa blaa"
            });

            context.SaveChanges();

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}