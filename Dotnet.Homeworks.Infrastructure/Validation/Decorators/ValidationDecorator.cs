using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class ValidationDecorator<TRequest, TResponse> : PermissionCheckDecorator<TRequest, TResponse> 
    where TRequest : IRequest<TResponse> 
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<IClientRequest> permissionCheck) 
        : base(permissionCheck)
    {
        _validators = validators;
    }

    public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var baseResult = await base.Handle(request, cancellationToken);

        if (baseResult.IsFailure) return baseResult;
        
        if (!_validators.Any())
            return true as dynamic;

        var validationResults =
            await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var errors = validationResults
            .Where(x => !x.IsValid)
            .SelectMany(x => x.Errors)
            .Select(x => $" -- {x.PropertyName}: {x.ErrorMessage} Severity: {x.Severity}")
            .ToArray();

        if (errors.Length == 0)
            return true as dynamic;

        return $"Validation failed:\n {string.Join("\n", errors)}" as dynamic;
    }
}