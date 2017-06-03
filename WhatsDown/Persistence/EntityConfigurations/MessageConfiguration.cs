using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.ModelConfiguration;
using WhatsDown.Core.Domain;

namespace WhatsDown.Persistence.EntityConfigurations
{
    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            //Property(msg => msg.MessageBody)
            //    .IsRequired()
            //    .HasMaxLength(200);

            //HasRequired(msg => msg.SenderId)
            //    .WithMany(usr => usr.)
            //    .HasForeignKey(c => c.AuthorId)
            //    .WillCascadeOnDelete(false);

            //Property(msg => msg.MessageBody)
            //    .IsRequired()
            //    .HasMaxLength(200);

            //Property(msg => msg.MessageBody)
            //    .IsRequired()
            //    .HasMaxLength(200);
        }
    }
}