using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : CqrsDecorator<DeleteUserCommand, Result>,
    ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, 
        IEnumerable<IValidator<DeleteUserCommand>> validators,
        IPermissionCheck<IClientRequest>? permissionCheck) 
        : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var baseResult = await base.Handle(request, cancellationToken);

        if (baseResult.IsFailure) return baseResult;

        var user = await _userRepository.GetUserByGuidAsync(request.Guid, cancellationToken);
        if (user is null)
            return "User doesn't exist";
        
        await _userRepository.DeleteUserByGuidAsync(request.Guid, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new Result(true);
    }
}