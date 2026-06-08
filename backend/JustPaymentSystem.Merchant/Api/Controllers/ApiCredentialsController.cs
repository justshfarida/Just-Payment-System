using Application.Helpers;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiCredentialsController : ControllerBase
{
    private readonly MerchantDbContext _db;

    public ApiCredentialsController(MerchantDbContext merchantDbContext)
    {
        _db = merchantDbContext;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("User ID claim not found."));
        var apiCredentials = await _db.ApiCredentials
            .Include(c => c.Merchant)
            .AsNoTracking()
            .Where(c => c.Merchant != null && c.Merchant!.UserId == userId)
            .Select(c => new 
            {
                merchant_id = c.Merchant!.UserId,
                secret_key = c.SecretKeyHash
            })
            .FirstOrDefaultAsync();

        return apiCredentials is null ? NotFound() : Ok(apiCredentials);
    }

    [HttpPatch("/secret-key")]
    [Authorize]
    public async Task<IActionResult> GenerateSecretKey()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("User ID claim not found."));
        var apiCredentials = await _db.ApiCredentials
            .Include(c => c.Merchant)
            .Where(c => c.Merchant != null && c.Merchant!.UserId == userId)
            .FirstOrDefaultAsync();

        if(apiCredentials == null)
        {
            return NotFound();
        }

        string secretKeyHash = SecretKeyGeneratorHelper.GenerateSecretKey();
        apiCredentials.SecretKeyHash = secretKeyHash;
        await _db.SaveChangesAsync();
        return Ok(secretKeyHash);
    }

}
