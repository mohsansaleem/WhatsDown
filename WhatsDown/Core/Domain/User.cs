using System.Collections.Generic;
using System.Data;
using  System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WhatsDown.Models;

namespace WhatsDown.Core.Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Quote { get; set; }

        public System.DateTime LastSeen
        {
            get; set;
        }

        public User()
        {
            this.ConversationUsers = new HashSet<UserConversationStatus>();
            this.Messages = new HashSet<Message>();
            LastSeen = DateTime.Now;
        }

        public virtual ICollection<Conversation> ConversationsHosting
        {
            get; set;
        }

        public virtual ICollection<UserConversationStatus> ConversationUsers
        {
            get; set;
        }

        public virtual ICollection<Message> Messages
        {
            get; set;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
