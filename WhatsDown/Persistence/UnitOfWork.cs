using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
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
                    //Conversation = conversation,
                    //User = usr,
                    UserId = usr.Id,
                    ConversationId = conversation.Id
                });
            });

            Conversations.Add(conversation);

            var val = this.Conversations.SaveChanges();

            return conversation;
        }

        public List<ConversationNode> GetAllConversationNodesForUser( string userId)
        {
            List<ConversationNode> conversations = new List<ConversationNode>();

            var cnvs = Conversations.GetAllConversationsForUser(userId);

            foreach (var cnv in cnvs)
            {
                var cnvNode = new ConversationNode();

                if (cnv.Title.IsEmpty())
                    cnvNode.Title = cnv.ConversationUsers.Select(cnvusr => cnvusr.User.FirstName).Aggregate((i, j) => i + ", " + j);

                var latestMsg = cnv.Messages.OrderByDescending(msg => msg.SendTime).FirstOrDefault();

                if (latestMsg != null)
                {
                    cnvNode.LastMessageTime = latestMsg.SendTime;

                    if (latestMsg.MessageBody.Length < 15)
                        cnvNode.ConversationInitialText = latestMsg.MessageBody.Substring(0, 15);
                    else
                        cnvNode.ConversationInitialText = latestMsg.MessageBody;
                }

                var cnv_usr_status = cnv.ConversationUsers.First(c => c.UserId.Equals(userId));

                cnvNode.UnrealMessagesCount = cnv.Messages.Count(msg => msg.SendTime > cnv_usr_status.LastActiveOnThread);
                
                conversations.Add(cnvNode);
            }
            return conversations;
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