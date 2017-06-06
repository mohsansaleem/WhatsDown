using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatsDown.Models
{
    public enum NotificationType
    {
        NewMessage = 1,
        NewConversation = 2
    }

    public class NotificationNode
    {
        public NotificationType Type { get; set; }

        public string Title { get; set; }

        public string BodyText { get; set; }

        public string Time { get; set; }

        public int ConversationId { get; set; }
        public int MessageId { get; set; }
        public string SenderName { get; set; }
    }

    public class MainViewModel
    {
        public MainViewModel()
        {
            CurrentUserName = "";
            Conversations = new List<ConversationNode>();
            SelectedConversationId = -1;
            SelectConversationMessages = new List<MessageNode>();
        }

        public string CurrentUserName { get; set; }

        public List<ConversationNode> Conversations { get; set; }

        public int SelectedConversationId { get; set; }

        public List<MessageNode> SelectConversationMessages { get; set; } 
        
    }

    public class MessageNode
    {
        public int MessageId { get; set; }

        public string SenderName { get; set; }

        public string MessageBody { get; set; }

        public DateTime MessageSentTime { get; set; }

        public string MessageSentTimeString
        {
            get { return MessageSentTime.ToString("u"); }
            set {
                ; }
        }
    }

    public class ConversationNode
    {
        public ConversationNode()
        {
            Title = "";
            ConversationInitialText = "";
            LastMessageTime = DateTime.MinValue;
        }

        public int ConversationId
        {
            get; set;
        }

        public int UnrealMessagesCount
        {
            get; set;
        }

        public string Title
        {
            get; set;
        }

        public string ConversationInitialText
        {
            get; set;
        }

        public DateTime LastMessageTime
        {
            get; set;
        }
    }

}