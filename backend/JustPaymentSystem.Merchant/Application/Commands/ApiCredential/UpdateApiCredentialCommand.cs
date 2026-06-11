using Application.Dtos;

namespace Application.Commands.ApiCredential;

public sealed record UpdateApiCredentialCommand(Guid UserId, UpdateApiCredentialDto Request);
