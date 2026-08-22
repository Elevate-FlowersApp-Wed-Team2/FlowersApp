namespace FlowersApp.Cart.Infrastructure.Persistence;

public class UnitOfWork(CartDbContext appDbContext)
{
    private readonly CartDbContext _appDbContext = appDbContext;
    public Task<int> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
