using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using WebGrease.Css.Extensions;
using WhatsDown.Core;
using WhatsDown.Core.Domain;
using WhatsDown.Core.Repositories;
using WhatsDown.Models;
using WhatsDown.Persistence.Repositories;

namespace WhatsDown.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WhatsDownDBContext _context;

        public UnitOfWork(WhatsDownDBContext context)
        {
            _context = context;
            Messages = new MessageRepository(_context);
            Users = new UserRepository(_context);
            Conversations = new ConversationRepository(_context);
            UsersConversationsStatus = new UserConversationStatusRepository(_context);
        }

        public IMessageRepository Messages { get; private set; }

        public IUserRepository Users { get; private set; }

        public IConversationRepository Conversations { get; private set; }

        public IUserConversationStatusRepository UsersConversationsStatus { get; private set; }

        public Message CreateNewMessage(User sender, int conversationId, string message)
        {
            Message msg = new Message()
            {
                ConversationId = conversationId,
                MessageBody = message,
                UserId = sender.Id,
                SendTime = DateTime.UtcNow,
                User = sender,
                Conversation = Conversations.Find(cnv=> cnv.Id== conversationId).First()
            };

            Messages.Add(msg);

            UpdateLastActiveStatusForUser(sender.Id, conversationId);

            Complete();

            return msg;
        }

        public void UpdateLastActiveStatusForUser(string userId, int conversationId)
        {
            var user = Users.Find(usr => usr.Id.Equals(userId)).First();
            Conversations.Find(conv => conv.Id == conversationId).First()
                .ConversationUsers.First(usr => usr.UserId.Equals(user.Id))
                .LastActiveOnThread = DateTime.UtcNow;
        }

        public Conversation CreateNewConversation( User userAdmin, List<User> participantUsers, string title = "", string description = ""   )
        {
            Conversation conversation = new Conversation()
            {
                Admin = userAdmin,
                Description = description,
                Title = title,
                //AdminId = userAdmin.Id
            };

            if(conversation.ConversationUsers == null)
                conversation.ConversationUsers = new List<UserConversationStatus>();

            participantUsers.ForEach(usr =>
            {
                conversation.ConversationUsers.Add(new UserConversationStatus()
                {
                    Conversation = conversation,
                    User = usr,
                    UserId = usr.Id,
                    ConversationId = conversation.Id
                });
            });
            
            Conversations.Add(conversation);
            conversation.ConversationUsers.ForEach(cnvUSr => UsersConversationsStatus.Add(cnvUSr));

            var val = this.Conversations.SaveChanges();

            return conversation;
        }

        public List<User> GetAllUsersPerConversation(int conversationId)
        {
            List<User> users = Conversations.Find(cnv => cnv.Id == conversationId).First().ConversationUsers.Select(cnvusr => cnvusr.User).ToList();

            return users;
        }

        public List<MessageNode> GetAllMessageNodesForConversation(int conversationId)
        {
            var msgs = Messages.GetAllMessagesForConversation(conversationId).ToList();

            List<MessageNode> messages = msgs.Select(msg => new MessageNode()
            {
                MessageBody = msg.MessageBody, MessageSentTime = msg.SendTime, SenderName = Users.Find(usr=> usr.Id.Equals(msg.UserId)).First().FullName
            }).ToList();

            return messages;
        }

        public List<ConversationNode> GetAllConversationNodesForUser( string userId)
        {
            List<ConversationNode> conversations = new List<ConversationNode>();

            var cnvs = Conversations.GetAllConversationsForUser(userId).ToList();

            foreach (var cnv in cnvs)
            {
                var cnvNode = new ConversationNode();

                if (cnv.Title.IsEmpty())
                    cnvNode.Title = cnv.ConversationUsers.Select(cnvusr => cnvusr.User.FirstName).Aggregate((i, j) => i + ", " + j);
                else
                    cnvNode.Title = cnv.Title;
                
                var latestMsg = cnv.Messages.OrderByDescending(msg => msg.SendTime).FirstOrDefault();

                if (latestMsg != null)
                {
                    cnvNode.LastMessageTime = latestMsg.SendTime;

                    if (latestMsg.MessageBody.Length > 15)
                        cnvNode.ConversationInitialText = latestMsg.MessageBody.Substring(0, 15);
                    else
                        cnvNode.ConversationInitialText = latestMsg.MessageBody;
                }
                else
                {
                    cnvNode.LastMessageTime = cnv.StartDate;
                }

                var cnv_usr_status = cnv.ConversationUsers.First(c => c.UserId.Equals(userId));

                cnvNode.UnrealMessagesCount = cnv.Messages.Count(msg => msg.SendTime > cnv_usr_status.LastActiveOnThread);

                cnvNode.ConversationId = cnv.Id;

                conversations.Add(cnvNode);
            }
            
            return conversations.OrderByDescending(cnv => cnv.LastMessageTime).ToList();
        }
             

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}