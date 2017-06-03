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

        public User GetAuthorWithCourses(int id)
        {
            return null;//PlutoContext.Users.Include(a => a.c).SingleOrDefault(a => a.Id == id);
        }

        public WhatsDownDBContext PlutoContext
        {
            get { return Context as WhatsDownDBContext; }
        }
    }
}