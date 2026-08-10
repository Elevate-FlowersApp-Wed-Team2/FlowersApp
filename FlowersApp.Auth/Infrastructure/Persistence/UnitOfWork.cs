namespace FlowersApp.Auth.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;
    public Task<int> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
