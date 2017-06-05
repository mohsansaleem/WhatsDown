using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatsDown.Core.Domain
{
    public class UserConversationStatus
    {
        public UserConversationStatus()
        {
            LastActiveOnThread = DateTime.Now;
            LastDelivered = DateTime.Now;
        }

        [Key]
        public int Id
        {
            get; set;
        }

        public int ConversationId
        {
            get; set;
        }

        public string UserId
        {
            get; set;
        }

        public int Role
        {
            get; set;
        }

        public System.DateTime LastActiveOnThread
        {
            get; set;
        }

        public System.DateTime LastDelivered
        {
            get; set;
        }

        public virtual Conversation Conversation
        {
            get; set;
        }

        public virtual User User
        {
            get; set;
        }
    }
}