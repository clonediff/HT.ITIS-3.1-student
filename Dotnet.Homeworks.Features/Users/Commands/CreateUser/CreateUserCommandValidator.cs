using System.Text.RegularExpressions;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Users.Commands.Shared;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email)
            .MustAsync(EmailValidationHelper.CheckUserNotExists(userRepository))
            .NotNull()
            .Matches(@"\w+@\w+\.\w+").WithMessage("Email address has wrong format");
    }
}