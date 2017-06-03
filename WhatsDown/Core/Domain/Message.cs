using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WhatsDown.Core.Domain
{
    public class Message
    {
        [Key]
        public int Id
        {
            get; set;
        }
        //public int ConversationId
        //{
        //    get; set;
        //}
        //public int SenderId
        //{
        //    get; set;
        //}

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
