using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiCredentialsController : ControllerBase
{
    private readonly IApiCredentialService _apiCredentialService;

    public ApiCredentialsController(IApiCredentialService apiCredentialService)
    {
        _apiCredentialService = apiCredentialService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var apiCredentials = await _apiCredentialService.GetByUserIdAsync(userId, cancellationToken);

        return apiCredentials is null ? NotFound() : Ok(apiCredentials);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var apiCredentials = await _apiCredentialService.CreateAsync(userId, cancellationToken);

        return CreatedAtAction(nameof(Get), apiCredentials);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(UpdateApiCredentialDto request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var apiCredentials = await _apiCredentialService.UpdateAsync(userId, request, cancellationToken);

        return apiCredentials is null ? NotFound() : Ok(apiCredentials);
    }

    [HttpPatch("secret-key")]
    [Authorize]
    public async Task<IActionResult> RegenerateSecretKey(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _apiCredentialService.RegenerateSecretKeyAsync(userId, cancellationToken);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var deleted = await _apiCredentialService.DeleteAsync(userId, cancellationToken);

        return deleted ? NoContent() : NotFound();
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User ID claim not found."));
    }
}
