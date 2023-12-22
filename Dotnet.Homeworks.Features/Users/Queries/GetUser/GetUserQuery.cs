using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public record GetUserQuery(Guid Guid) : IClientRequest, IQuery<GetUserDto>;
