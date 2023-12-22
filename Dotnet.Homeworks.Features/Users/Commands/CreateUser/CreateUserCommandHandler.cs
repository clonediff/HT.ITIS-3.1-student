using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser.Dto;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser.Services;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : CqrsDecorator<CreateUserCommand, Result<CreateUserDto>>,
    ICommandHandler<CreateUserCommand, CreateUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegistrationService _registrationService;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, 
        IEnumerable<IValidator<CreateUserCommand>> validators,
        IPermissionCheck<IClientRequest>? permissionCheck, 
        IRegistrationService registrationService) 
        : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _registrationService = registrationService;
    }

    public override async Task<Result<CreateUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var baseResult = await base.Handle(request, cancellationToken);

        if (baseResult.IsFailure) return baseResult;
        
        var user = new User
        {
            Name = request.Name,
            Email = request.Email
        };
        
        await _userRepository.InsertUserAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _registrationService.RegisterAsync(new RegisterUserDto(user.Name, user.Email));
        
        return new CreateUserDto(user.Id);
    }
}