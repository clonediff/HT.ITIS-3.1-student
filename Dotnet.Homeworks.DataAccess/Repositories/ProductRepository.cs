using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_dbContext.Products.AsEnumerable());
    }

    public async Task DeleteProductByGuidAsync(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => x.Id == product.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, product.Name), cancellationToken);
    }

    public Task<Guid> InsertProductAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Add(product);
        return Task.FromResult(product.Id);
    }
}