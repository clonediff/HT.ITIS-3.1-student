using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public interface IPermissionCheck<TRequest>
    where TRequest : IPermissionCheckRequest
{
    Task<IEnumerable<PermissionResult>> CheckPermissionAsync(TRequest request);
}