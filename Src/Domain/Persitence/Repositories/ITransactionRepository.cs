using System.Transactions;
using SysgamingApi.Src.Domain.Common;

namespace SysgamingApi.Src.Domain.Persitence.Repositories
{
    interface ITransactionRepository : IBaseRepository<Transaction>
    {
    }
}
