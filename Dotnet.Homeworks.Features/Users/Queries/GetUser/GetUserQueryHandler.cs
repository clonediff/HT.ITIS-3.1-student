using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : CqrsDecorator<GetUserQuery, Result<GetUserDto>>, 
    IQueryHandler<GetUserQuery, GetUserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository, 
        IEnumerable<IValidator<GetUserQuery>> validators,
        IPermissionCheck<IClientRequest>? permissionCheck) 
        : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
    }

    public override async Task<Result<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var baseResult = await base.Handle(request, cancellationToken);

        if (baseResult.IsFailure) return baseResult;
        
        var user = await _userRepository.GetUserByGuidAsync(request.Guid, cancellationToken);
        if (user is null)
            return $"User with guid {request.Guid} not found";
        return new GetUserDto(user.Id, user.Name, user.Email);
    }
}