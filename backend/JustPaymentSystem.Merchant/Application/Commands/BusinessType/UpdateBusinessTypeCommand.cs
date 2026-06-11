using Application.Dtos;

namespace Application.Commands.BusinessType;

public sealed record UpdateBusinessTypeCommand(Guid Id, UpdateBusinessTypeDto Request);
