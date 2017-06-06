using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WhatsDown.Core.Domain
{
    public class Conversation
    {
        public Conversation()
        {
            StartDate = DateTime.UtcNow;
            this.ConversationUsers = new HashSet<UserConversationStatus>();
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

        public string AdminId { get; set; }

        public virtual User Admin { get; set; }

        public virtual ICollection<UserConversationStatus> ConversationUsers
        {
            get; set;
        }
        
        public virtual ICollection<Message> Messages
        {
            get; set;
        }
    }
}