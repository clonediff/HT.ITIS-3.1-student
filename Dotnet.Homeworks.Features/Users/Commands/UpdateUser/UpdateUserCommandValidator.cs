using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Users.Commands.Shared;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Guid).NotEmpty();
        RuleFor(x => x.User).NotNull();
        RuleFor(x => x.User.Name).NotEmpty();
        RuleFor(x => x.User.Email)
            .MustAsync(EmailValidationHelper.CheckUserExists(userRepository))
            .EmailAddress().WithMessage("Email address has wrong format");
    }
}