using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    // MongoDB write operations (InsertOne, ReplaceOne) are atomic at the document level and are
    // committed immediately by the repositories, so there is no pending batch to flush. The unit
    // of work is kept to honor the application contract and as the single place to introduce
    // multi-document transactions (sessions) if they are ever required.
    public sealed class UnitOfWork : IUnitOfWork
    {
        public Task<int> Save()
        {
            return Task.FromResult(0);
        }
    }
}
