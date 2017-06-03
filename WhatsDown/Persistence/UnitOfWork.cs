using WhatsDown.Core;
using WhatsDown.Core.Repositories;
using WhatsDown.Persistence.Repositories;

namespace WhatsDown.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WhatsDownDBContext _context;

        public UnitOfWork(WhatsDownDBContext context)
        {
            _context = context;
            Messages = new MessageRepository(_context);
            Users = new UserRepository(_context);
        }

        public IMessageRepository Messages { get; private set; }
        public IUserRepository Users { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}