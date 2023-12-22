namespace Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

public interface IClientRequest : IPermissionCheckRequest
{
    public Guid Guid { get; }
}