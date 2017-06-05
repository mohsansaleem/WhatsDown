using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WhatsDown.Core.Domain;
using WhatsDown.Core.Repositories;

namespace WhatsDown.Persistence.Repositories
{
    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        public ConversationRepository(WhatsDownDBContext context) 
            : base(context)
        {
            
        }

        public IEnumerable<Conversation> GetAllConversationsForUser(string userId)
        {
            var convs = new List<Conversation>();
            var lst = WhatsDownDbContext.Users.FirstOrDefault(usr => usr.Id.Equals(userId));
            if(lst != null && lst.ConversationUsers != null && lst.ConversationUsers.Count > 0)
                convs =lst.ConversationUsers.Select(convstatus => convstatus.Conversation)
                .ToList();
            return convs;
        }

        public WhatsDownDBContext WhatsDownDbContext
        {
            get { return Context as WhatsDownDBContext; }
        }
    }
}