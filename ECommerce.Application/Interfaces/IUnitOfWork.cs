namespace ECommerce.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IRepositoryCart Carts { get; }
        IRepositoryCategory Categorys { get; }
        IRepositoryOrder Orders { get; }
        IRepositoryProduct Products { get; }
        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
