namespace FlowersApp.Catalog.Infrastructure.Persistence;

public class UnitOfWork(CatalogDbContext appDbContext)
{
    private readonly CatalogDbContext _appDbContext = appDbContext;
    public Task<int> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
