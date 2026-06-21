using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Infrastructure.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
       
        private readonly E_commerceDbcontext dbcontext;
        private IDbContextTransaction? transaction;

        public UnitOfWork(E_commerceDbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
            
        }

        public async Task BeginTransactionAsync()
        {
            transaction = await dbcontext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (transaction is not null)
            {
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
                transaction = null;
                
            }

        }

        public async Task RollbackAsync()
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;

            }
        }

        public async Task SaveAsync()
        {
            await dbcontext.SaveChangesAsync() ;
        }
    }
}
