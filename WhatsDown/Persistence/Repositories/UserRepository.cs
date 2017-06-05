using System.Data.Entity;
using System.Linq;
using WhatsDown.Core.Domain;
using WhatsDown.Core.Repositories;

namespace WhatsDown.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(WhatsDownDBContext context) : base(context)
        {
        }

        public User GetSenderFromMessage(int messageId)
        {
            return null;//WhatsDownDbContext.Users.Include(a => a.c).SingleOrDefault(a => a.Id == id);
        }

        public WhatsDownDBContext WhatsDownDbContext
        {
            get { return Context as WhatsDownDBContext; }
        }
    }
}