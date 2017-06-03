using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatsDown.Core.Domain
{
    public class UserConversation
    {
        [Key]
        public int Id
        {
            get; set;
        }

        //public int CId
        //{
        //    get; set;
        //}

        //public int UId
        //{
        //    get; set;
        //}

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