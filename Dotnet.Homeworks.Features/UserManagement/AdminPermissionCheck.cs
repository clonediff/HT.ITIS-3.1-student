using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.UserManagement;

public class AdminPermissionCheck : IPermissionCheck<IAdminRequest>
{
    private readonly HttpContext _httpContext;
    
    public AdminPermissionCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public Task<IEnumerable<PermissionResult>> CheckPermissionAsync(IAdminRequest request)
    {
        var isAdmin = _httpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == Roles.Admin.ToString());

        return Task.FromResult(new[] { new PermissionResult(isAdmin, "Access denied") }.AsEnumerable());
    }
}