using System;
using WhatsDown.Core.Repositories;

namespace WhatsDown.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IMessageRepository Messages { get; }
        IUserRepository Users { get; }
        int Complete();
    }
}