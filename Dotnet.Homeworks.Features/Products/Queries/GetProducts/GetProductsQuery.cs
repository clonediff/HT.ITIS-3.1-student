using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;

namespace Dotnet.Homeworks.Features.Products.Queries.GetProducts;

public record GetProductsQuery : IQuery<GetProductsDto>;
