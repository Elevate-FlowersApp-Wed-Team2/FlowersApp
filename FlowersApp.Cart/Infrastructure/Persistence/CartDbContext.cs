using FlowersApp.Cart.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Cart.Infrastructure.Persistence;

public class CartDbContext :DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options)
        : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(CartDbContext).Assembly);
    }
}
