using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WhatsDown.Core.Domain;
using WhatsDown.Core.Repositories;

namespace WhatsDown.Persistence.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(WhatsDownDBContext context) 
            : base(context)
        {
        }

        public IEnumerable<Message> GetAllMessagesForConversation(int conversationId)
        {
            return WhatsDownDbContext.Messages.Where(msg => msg.ConversationId.Equals(conversationId)).OrderBy(msg=> msg.SendTime);
        }

        public IEnumerable<Message> GetTopMessagesForConversation(int conversationId, int count)
        {
            return WhatsDownDbContext.Messages.Where(msg => msg.ConversationId == conversationId).OrderBy(m => m.SendTime).Take(count).ToList();

            //from user in WhatsDownDbContext.Users
            //from cnv in WhatsDownDbContext.Conversations
            //from usr_cnv in WhatsDownDbContext.UserConversations

            //var result = (
            //// instance from context
            //from user in WhatsDownDbContext.Users
            //    // instance from navigation property
            //from message in user.Messages
            //    //join to bring useful data
            //join jmsg in WhatsDownDbContext.Messages on message.Id equals jmsg.Id
            //where user.Id == ""
            //select new// DTOGenericObject
            //{
            //    U = user.LastSeen,
            //    ID = jmsg.Id,
            //    //Name = jmsg.User.FirstName
            //}).ToList();

            //return result;

            
        }

        
        public IEnumerable<Message> GetConversationMessagesPerPageForUser(int conversationId, int pageIndex, int pageSize)
        {
            return WhatsDownDbContext.Messages.Where(msg => msg.ConversationId == conversationId).OrderBy(m => m.SendTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            //return
            //    (from g in WhatsDownDbContext..userGroups
            //     from u in ent.Users.Where(a => a.fk_user_id == u.userID).DefaultIfEmpty()
            //     select new
            //     {
            //         g,
            //         u,
            //     });

            
        }

        public WhatsDownDBContext WhatsDownDbContext
        {
            get { return Context as WhatsDownDBContext; }
        }
    }
}