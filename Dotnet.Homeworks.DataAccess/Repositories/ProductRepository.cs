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
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (product is not null)
            _dbContext.Products.Remove(product);
    }

    public Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Products.Update(product);
        return Task.CompletedTask;
    }

    public Task<Guid> InsertProductAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Products.Add(product);
        return Task.FromResult(product.Id);
    }
}