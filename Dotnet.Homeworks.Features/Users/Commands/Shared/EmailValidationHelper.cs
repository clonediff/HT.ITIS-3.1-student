using Dotnet.Homeworks.Domain.Abstractions.Repositories;

namespace Dotnet.Homeworks.Features.Users.Commands.Shared;

public static class EmailValidationHelper
{
    public static Func<string, CancellationToken, Task<bool>> CheckUserNotExists(IUserRepository userRepository)
    {
        return async (email, cancellationToken) =>
        {
            var users = await userRepository.GetUsersAsync(cancellationToken);
            return users.All(x => x.Email != email);
        };
    }

    public static Func<string, CancellationToken, Task<bool>> CheckUserExists(IUserRepository userRepository)
        => async (email, cancellationToken) => !await CheckUserNotExists(userRepository)(email, cancellationToken);
}