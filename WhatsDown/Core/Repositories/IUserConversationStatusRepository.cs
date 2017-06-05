using System.Collections.Generic;
using WhatsDown.Core.Domain;

namespace WhatsDown.Core.Repositories
{
    public interface IUserConversationStatusRepository : IRepository<UserConversationStatus>
    {
        bool UpdateUserConversationLastActiveStatus(string userId, int conversationId);

        bool UpdateUserConversationLastDeliveredStatus(string userId, int conversationId);
    }
}