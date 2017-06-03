using System.Collections.Generic;
using WhatsDown.Core.Domain;

namespace WhatsDown.Core.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        IEnumerable<Message> GetTopSellingCourses(int count);
        IEnumerable<Message> GetCoursesWithAuthors(int pageIndex, int pageSize);
    }
}