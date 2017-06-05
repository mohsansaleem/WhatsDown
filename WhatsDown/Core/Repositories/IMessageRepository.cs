using System.Collections.Generic;
using WhatsDown.Core.Domain;

namespace WhatsDown.Core.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        IEnumerable<Message> GetAllMessagesForConversation(int conversationId);

        IEnumerable<Message> GetTopMessagesForConversation(int conversationId, int count);

        IEnumerable<Message> GetConversationMessagesPerPageForUser( int conversationId, int pageIndex, int pageSize);
    }
}