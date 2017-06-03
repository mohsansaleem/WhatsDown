using WhatsDown.Core.Domain;

namespace WhatsDown.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetAuthorWithCourses(int id);
    }
}