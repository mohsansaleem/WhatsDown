using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WhatsDown.Core.Domain;
using WhatsDown.Hubs;
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

        //public ActionResult Index()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var id = User.Identity.GetUserId();

        //        MainViewModel viewModel = new MainViewModel()
        //        {
        //            Conversations = UnitOfWork.GetAllConversationNodesForUser(id)
        //        };

        //        viewModel.CurrentUserName = UnitOfWork.Users.Find(usr => usr.Id.Equals(id)).First().FullName;

        //        if (viewModel.Conversations.Count > 0)
        //        {
        //            int convId = viewModel.Conversations.First().ConversationId;
        //            viewModel.SelectedConversationId = convId;
        //            viewModel.SelectConversationMessages = UnitOfWork.GetAllMessageNodesForConversation(convId);
        //        }

        //        return View(viewModel);
        //    }
            
        //    return View();
        //}
        
        public ActionResult Index(int selectedConversationId = -1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();

                MainViewModel viewModel = new MainViewModel()
                {
                    Conversations = UnitOfWork.GetAllConversationNodesForUser(id)
                };

                viewModel.CurrentUserName = UnitOfWork.Users.Find(usr => usr.Id.Equals(id)).First().FullName;

                if (viewModel.Conversations.Any(cnv => cnv.ConversationId == selectedConversationId))
                {
                    viewModel.SelectedConversationId = selectedConversationId;
                    viewModel.SelectConversationMessages = UnitOfWork.GetAllMessageNodesForConversation(selectedConversationId);
                }else if (viewModel.Conversations.Count > 0)
                {
                    int convId = viewModel.Conversations.First().ConversationId;
                    viewModel.SelectedConversationId = convId;
                    viewModel.SelectConversationMessages = UnitOfWork.GetAllMessageNodesForConversation(convId);
                }

                return View(viewModel);
            }

            return View();
        }

        [HttpPost]
        public JsonResult LoadMessagesForConversation(int conversationId)
        {
            if (!User.Identity.IsAuthenticated)
                return Json("{Error}");
                
            var selectConversationMessages = UnitOfWork.GetAllMessageNodesForConversation(conversationId);

            var id = User.Identity.GetUserId();
            

            return Json(selectConversationMessages);
        }

        [HttpPost]
        public JsonResult MessagesFromUser(int conversationId, string messageText)
        {
            if (!User.Identity.IsAuthenticated)
                return Json("{Error}");

            var id = User.Identity.GetUserId();
            var sender = UnitOfWork.Users.Find(usr => usr.Id.Equals(id)).First();
            Message message = UnitOfWork.CreateNewMessage(sender, conversationId, messageText);
            
            NotificationNode notificationNode = new NotificationNode()
            {
                Type = NotificationType.NewMessage,
                BodyText = message.MessageBody,
                ConversationId = conversationId,
                MessageId = message.Id,
                Time = DateTime.Now.ToString("g"),
                Title = "Message From " + sender.FullName,
                SenderName = sender.FullName
            };

            MessageHub.SendNotification(notificationNode, UnitOfWork.GetAllUsersPerConversation(conversationId).Select(usr=> usr.Id).ToList());

            return Json(notificationNode);
        }

        // Testing
        [HttpPost]
        public JsonResult AjaxMethod(string name)
        {
            //PersonModel person = new PersonModel
            //{
            //    Name = name,
            //    DateTime = DateTime.Now.ToString()
            //};
            //return Json(person);

            Console.WriteLine("Name: "+name);
            name += "back";
            return Json(name);
        }
        
        public ActionResult CreateNewConversation(SelectUsersModel selectUsers)
        {
            if (!User.Identity.IsAuthenticated)
                return View();

            var id = User.Identity.GetUserId();

            if (selectUsers != null && selectUsers.SelectedIds.Any())
            {

                var userAdmin = UnitOfWork.Users.Find(usr => usr.Id.Equals(id)).First();
                List<string> selectedIds = selectUsers.SelectedIds.ToList();

                Conversation conversation = null;
                // Dealing with Single Person Conversation
                if (selectUsers.SelectedIds.Count() == 1)
                {
                    var otherUser = UnitOfWork.Users.GetAll().First(usr => usr.Id.Equals(selectedIds[0]));


                    try
                    {
                        var myConvs = UnitOfWork.Conversations.Find(cnv => cnv.ConversationUsers.Any(cu => cu.UserId.Equals(userAdmin.Id)));
                        var tmp =
                            myConvs.Where(cnv => cnv.ConversationUsers.Any(cu => cu.UserId.Equals(otherUser.Id)));
                        if (tmp != null)
                            conversation = tmp.FirstOrDefault(u => u.ConversationUsers.Count == 2);
                    }
                    catch (Exception)
                    {
                        // Just a precation.
                    }
                }

                if (conversation == null)
                {
                    if (selectUsers.Title == null)
                        selectUsers.Title = "";

                    List<User> usersInConv = UnitOfWork.Users.Find(usr => selectedIds.Contains(usr.Id)).ToList();
                    usersInConv.Add(userAdmin);

                    conversation = UnitOfWork.CreateNewConversation(userAdmin, usersInConv,selectUsers.Title);

                    MessageHub.SendNotification(new NotificationNode()
                    {
                        ConversationId = conversation.Id,
                        SenderName = userAdmin.FullName,
                        Time = conversation.StartDate.ToString("G"),
                        Title = conversation.Title
                    }, UnitOfWork.GetAllUsersPerConversation(conversation.Id).Select(usr=>usr.Id).ToList());
                }
                else
                {
                    Debug.WriteLine("User Conversation Already Exist.");
                }

                return RedirectToAction("Index", new { selectedConversationId = conversation.Id });
                // Redirect to User Conversation.
                // conversation
            }
            else
            {
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