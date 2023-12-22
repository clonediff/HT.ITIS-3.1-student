using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class PermissionCheckDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IPermissionCheck<IClientRequest> _permissionCheck;
    
    public PermissionCheckDecorator(IPermissionCheck<IClientRequest> permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }
    
    public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (request is not IClientRequest clientRequest)
            return true as dynamic;

        var permissionCheckResults = await _permissionCheck.CheckPermissionAsync(clientRequest);
        var errors = permissionCheckResults
            .Where(x => x.IsFailure)
            .Select(x => x.Error)
            .ToArray();

        if (errors.Length == 0)
            return true as dynamic;
        
        return $"Permission Check failed: \n{string.Join("\n", errors)}" as dynamic;
    }
}