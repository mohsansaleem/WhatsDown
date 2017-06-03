using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WhatsDown.Core.Domain;
using WhatsDown.Persistence.EntityConfigurations;

namespace WhatsDown.Persistence
{
    public class WhatsDownDBContext : IdentityDbContext<User>
    {
        public WhatsDownDBContext() : base("WhatsDownConnection", throwIfV1Schema: false)
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }
        public static WhatsDownDBContext Create()
        {
            return new WhatsDownDBContext();
        }
        
        public virtual DbSet<Message> Messages { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MessageConfiguration());
        }
    }
}
