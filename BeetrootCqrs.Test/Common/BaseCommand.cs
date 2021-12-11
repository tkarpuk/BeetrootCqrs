using BeetrootCqrs.DAL.DB;
using System;

namespace BeetrootCqrs.Test.Common
{
    public abstract class BaseCommand : IDisposable
    {
        protected readonly AppDbContext _dbContext;

        public BaseCommand()
        {
            _dbContext = FakeDbContextFactory.Create();
        }

        public void Dispose()
        {
            FakeDbContextFactory.Destroy(_dbContext);
        }
    }
}
