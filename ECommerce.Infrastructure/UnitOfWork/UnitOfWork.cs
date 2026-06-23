using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Infrastructure.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
       
        private readonly E_commerceDbcontext dbcontext;
        

        public IRepositoryCart Carts { get; }

        public IRepositoryCategory Categorys { get; }

        public IRepositoryOrder Orders { get; }

        public IRepositoryProduct Products { get; }

        private IDbContextTransaction? transaction;

        public UnitOfWork(E_commerceDbcontext dbcontext , IRepositoryCart Carts, IRepositoryCategory Categorys, IRepositoryOrder Orders, IRepositoryProduct Products)
        {
            this.dbcontext = dbcontext;
            this.Carts = Carts;
            this.Categorys = Categorys;
            this.Orders = Orders;
            this.Products = Products;
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
