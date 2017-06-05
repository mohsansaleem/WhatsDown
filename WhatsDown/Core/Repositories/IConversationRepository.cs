using System.Collections.Generic;
using WhatsDown.Core.Domain;

namespace WhatsDown.Core.Repositories
{
    public interface IConversationRepository : IRepository<Conversation>
    {
        IEnumerable<Conversation> GetAllConversationsForUser(string userId);
    }
}