using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser.Dto;
using Dotnet.Homeworks.Features.Users.Commands.DeleteUser;
using Dotnet.Homeworks.Features.Users.Commands.UpdateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class UserManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("user")]
    public async Task<IActionResult> CreateUserAsync(RegisterUserDto userDto, CancellationToken cancellationToken)
    {
        var createUserResult = await _mediator.Send(new CreateUserCommand(userDto.Name, userDto.Email), cancellationToken);
        return createUserResult.IsSuccess ? Ok(createUserResult.Value) : BadRequest(createUserResult.Error);
    }

    [HttpGet("profile/{guid}")]
    public async Task<IActionResult> GetProfileAsync(Guid guid, CancellationToken cancellationToken)
    {
        var profileResult = await _mediator.Send(new GetUserQuery(guid), cancellationToken);
        return profileResult.IsSuccess ? Ok(profileResult.Value) : BadRequest(profileResult.Error);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserQuery(Guid.NewGuid()), cancellationToken);
        return user.IsSuccess ? Ok(user.Value) : BadRequest(user.Error);
    }

    [HttpDelete("profile/{guid:guid}")]
    public async Task<IActionResult> DeleteProfileAsync(Guid guid, CancellationToken cancellationToken)
    {
        var deleteResult = await _mediator.Send(new DeleteUserCommand(guid), cancellationToken);
        return deleteResult.IsSuccess ? Ok() : BadRequest(deleteResult.Error);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfileAsync(User user, CancellationToken cancellationToken)
    {
        var updateResult = await _mediator.Send(new UpdateUserCommand(user), cancellationToken);
        return updateResult.IsSuccess ? Ok() : BadRequest(updateResult.Error);
    }

    [HttpDelete("user/{guid:guid}")]
    public async Task<IActionResult> DeleteUserAsync(Guid guid, CancellationToken cancellationToken)
    {
        var deleteResult = await _mediator.Send(new DeleteUserCommand(guid), cancellationToken);
        return deleteResult.IsSuccess ? Ok() : BadRequest(deleteResult.Error);
    }
}