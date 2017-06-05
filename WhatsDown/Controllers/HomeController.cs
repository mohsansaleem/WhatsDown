using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WhatsDown.Core.Domain;
using WhatsDown.Persistence;
using WhatsDown.Models;

namespace WhatsDown.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Application DB context
        /// </summary>
        protected UnitOfWork UnitOfWork
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
            var dbContext = new WhatsDownDBContext();
            this.UnitOfWork = new UnitOfWork(dbContext);
            this.UserManager = new UserManager<User>(new UserStore<User>(dbContext));
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<ConversationNode> Conversations = new List<ConversationNode>();

                var id = User.Identity.GetUserId();

                Conversations = UnitOfWork.GetAllConversationNodesForUser(id);
                
                return View(Conversations);
            }
            
            return View();
        }

        public ActionResult CreateNewConversation()
        {
            var id = User.Identity.GetUserId();

            var listSelectListItems = UnitOfWork.Users.GetAll().Where(u => !u.Id.Equals(id)).Select(usr => new SelectListItem()
            {
                Disabled = false,
                Selected = false,
                Text = usr.FirstName + " " + usr.LastName,
                Value = usr.Id
            }).ToList();
    
            SelectUsersModel model = new SelectUsersModel()
            {
                SelectedIds = new List<string>(),
                Title = "",
                Users = listSelectListItems
            };
            

            return View(model);
        }

        [HttpPost]
        public void CreateNewConversation(SelectUsersModel selectUsers)
        {
            if (User.Identity.IsAuthenticated && selectUsers != null && selectUsers.SelectedIds.Any())
            {

                var id = User.Identity.GetUserId();
                var userAdmin = UnitOfWork.Users.Find(usr => usr.Id.Equals(id)).FirstOrDefault();
                List<string> selectedIds = selectUsers.SelectedIds.ToList();

                Conversation conversation = null;
                // Dealing with Single Person Conversation
                if (selectUsers.SelectedIds.Count() == 1)
                {
                    var otherUser = UnitOfWork.Users.GetAll().FirstOrDefault(usr => usr.Id.Equals(selectedIds[0]));
                    var prevThread = UnitOfWork.Conversations.GetAll().FirstOrDefault(conv => conv.ConversationUsers.Count(convusr => convusr.User.Equals(userAdmin) || convusr.User.Equals(otherUser)) == 2);

                    conversation = prevThread;
                }

                if (conversation == null)
                {
                    if (selectUsers.Title == null)
                        selectUsers.Title = "";

                    List<User> usersInConv = UnitOfWork.Users.Find(usr => selectedIds.Contains(usr.Id)).ToList();
                    usersInConv.Add(userAdmin);

                    conversation = UnitOfWork.CreateNewConversation(userAdmin, usersInConv,selectUsers.Title);
                }
                else
                {
                    Debug.WriteLine("User Conversation Already Exist.");
                }
                
                // Redirect to User Conversation.
                // conversation
                return;
            }
        }

        public ActionResult Submit(Automobile auto)
        {
            return View("Index", auto);
        }

        //public ActionResult Submit(SelectedUsersModel selectedUsers)
        //{
        //    return View("Index", selectedUsers);
        //}

        public ActionResult Test()
        {
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();

            for (int i = 0; i < 10; i++)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = "Username: "+i.ToString(),
                    Value = "Id: "+i.ToString(),
                    Selected = false
                };
                listSelectListItems.Add(selectList);
            }

            MyViewModel mm = new MyViewModel();
            mm.SelectedIds = new List<string>();
            mm.Items = listSelectListItems;

            return View(mm);
        }

        public ActionResult About()
        {
        //    var user = UserManager.FindById(User.Identity.GetUserId());
        //    WhatsDownDBContext context = new WhatsDownDBContext();
        //    //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        //    context.Tests.Add(new Test()
        //    {
        //        Title = "blaa blaa"
        //    });

        //    context.SaveChanges();

        //    ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}