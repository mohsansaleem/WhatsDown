using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Mvc;

namespace WhatsDown.Core.Domain
{
    public class Message
    {
        public Message()
        {
            SendTime = DateTime.UtcNow;
            MessageTitle = "";
            MessageBody = "";

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

        public string MessageTitle
        {
            get; set;
        }
        public string MessageBody
        {
            get; set;
        }
        public System.DateTime SendTime
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
