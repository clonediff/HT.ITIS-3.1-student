﻿using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;

public class DeleteUserByAdminCommandHandler : ICommandHandler<DeleteUserByAdminCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserByAdminCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserByAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByGuidAsync(request.Guid, cancellationToken);
        if (user is null)
            return "User doesn't exist";
        
        await _userRepository.DeleteUserByGuidAsync(request.Guid, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new Result(true);
    }
}