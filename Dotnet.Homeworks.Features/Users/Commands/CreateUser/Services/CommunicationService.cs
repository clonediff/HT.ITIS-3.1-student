﻿using Dotnet.Homeworks.Shared.MessagingContracts.Email;
using MassTransit;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser.Services;

public class CommunicationService : ICommunicationService
{
    private readonly IBus _bus;

    public CommunicationService(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendEmailAsync(SendEmail sendEmailDto)
    {
        await _bus.Publish(sendEmailDto);
    }
}