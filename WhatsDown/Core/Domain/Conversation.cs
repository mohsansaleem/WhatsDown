using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatsDown.Core.Domain
{
    public class Conversation
    {
        public Conversation()
        {
            this.ConversationUsers = new HashSet<UserConversation>();
            this.Messages = new HashSet<Message>();
        }

        [Key]
        public int Id
        {
            get; set;
        }

        public System.DateTime StartDate
        {
            get; set;
        }

        public string Title
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public virtual ICollection<UserConversation> ConversationUsers
        {
            get; set;
        }
        
        public virtual ICollection<Message> Messages
        {
            get; set;
        }
    }
}