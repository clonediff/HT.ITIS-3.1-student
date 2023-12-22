using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Behaviors;

public class PermissionCheckBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAdminRequest, IRequest<TResponse>
    where TResponse : Result
{
    private readonly IPermissionCheck<IAdminRequest> _permissionCheck;

    public PermissionCheckBehavior(IPermissionCheck<IAdminRequest> permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var permissionCheckResults = await _permissionCheck.CheckPermissionAsync(request);
        
        var errors = permissionCheckResults
            .Where(x => x.IsFailure)
            .Select(x => x.Error)
            .ToArray();

        if (errors.Length != 0)
            return $"Permission Check failed: \n{string.Join("\n", errors)}" as dynamic;

        return await next();
    }
}