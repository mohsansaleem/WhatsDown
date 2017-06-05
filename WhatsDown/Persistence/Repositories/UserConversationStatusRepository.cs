using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WhatsDown.Core.Domain;
using WhatsDown.Core.Repositories;

namespace WhatsDown.Persistence.Repositories
{
    public class UserConversationStatusRepository : Repository<UserConversationStatus>, IUserConversationStatusRepository
    {
        public UserConversationStatusRepository(WhatsDownDBContext context) 
            : base(context)
        {
            
        }

        public bool UpdateUserConversationLastActiveStatus(string userId, int conversationId)
        {
            var node = WhatsDownDbContext.UserConversations.FirstOrDefault(usr_cnv => usr_cnv.UserId.Equals(userId) && usr_cnv.ConversationId == conversationId);
            if (node != null)
            {
                node.LastActiveOnThread = DateTime.Now;

                WhatsDownDbContext.Entry(node).State = EntityState.Modified;

                WhatsDownDbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public bool UpdateUserConversationLastDeliveredStatus(string userId, int conversationId)
        {
            var node = WhatsDownDbContext.UserConversations.FirstOrDefault(usr_cnv => usr_cnv.UserId.Equals(userId) && usr_cnv.ConversationId == conversationId);
            if (node != null)
            {
                node.LastDelivered = DateTime.Now;

                WhatsDownDbContext.Entry(node).State = EntityState.Modified;

                WhatsDownDbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public WhatsDownDBContext WhatsDownDbContext
        {
            get { return Context as WhatsDownDBContext; }
        }
    }
}