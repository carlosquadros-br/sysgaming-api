using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Persitence.Repositories;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : AbstractRepository<Transaction>, IBaseRepository<Transaction>, ITransactionRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public TransactionRepository(IAppDbContext appDbContext, IServiceProvider serviceProvider) : base(appDbContext)
        {
            _serviceProvider = serviceProvider;
        }

        override public async Task<Transaction> CreateAsync(Transaction entity)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<IAppDbContext>();
            await context.Set<Transaction>().AddAsync(entity);
            var result = await context.SaveChangesAsync();
            if (result == 0)
            {
                throw new Exception("Transaction not created");
            }
            return entity;
        }
    }
}
