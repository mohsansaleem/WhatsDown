using System;
using System.Collections.Generic;
using WhatsDown.Core.Domain;
using WhatsDown.Core.Repositories;
using WhatsDown.Models;

namespace WhatsDown.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IMessageRepository Messages { get; }
        IUserRepository Users { get; }

        IConversationRepository Conversations
        {
            get; 
        }

        IUserConversationStatusRepository UsersConversationsStatus
        {
            get;
        }

        Message CreateNewMessage(User sender, int conversationId, string message);

        Conversation CreateNewConversation(User userAdmin, List<User> participantUsers, string title = "",
            string description = "");

        List<MessageNode> GetAllMessageNodesForConversation(int conversationId);

        List<ConversationNode> GetAllConversationNodesForUser(string userId);

        int Complete();
    }
}