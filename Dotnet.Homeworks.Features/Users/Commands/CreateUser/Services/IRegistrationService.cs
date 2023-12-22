﻿using Dotnet.Homeworks.Features.Users.Commands.CreateUser.Dto;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser.Services;

public interface IRegistrationService
{
    public Task RegisterAsync(RegisterUserDto userDto);
}