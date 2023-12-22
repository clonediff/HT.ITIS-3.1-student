using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class CqrsDecorator<TRequest, TResponse> : ValidationDecorator<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    protected CqrsDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<IClientRequest>? permissionCheck) 
        : base(validators, permissionCheck)
    {
    }
}