using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WhatsDown.Core.Domain;
using WhatsDown.Core.Repositories;

namespace WhatsDown.Persistence.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(WhatsDownDBContext context) 
            : base(context)
        {
        }

        public IEnumerable<Message> GetTopSellingCourses(int count)
        {
            return null; //PlutoContext.Courses.OrderByDescending(c => c.FullPrice).Take(count).ToList();
        }

        public IEnumerable<Message> GetCoursesWithAuthors(int pageIndex, int pageSize = 10)
        {
            return null;
            //return PlutoContext.Courses
            //    .Include(c => c.Author)
            //    .OrderBy(c => c.Name)
            //    .Skip((pageIndex - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();
        }

        public WhatsDownDBContext PlutoContext
        {
            get { return Context as WhatsDownDBContext; }
        }
    }
}