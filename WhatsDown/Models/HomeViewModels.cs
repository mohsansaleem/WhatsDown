using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatsDown.Models
{

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