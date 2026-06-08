using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Features.Transactions.Commands;
using Application.Features.Transactions.Commands.DTOs;
using Application.Features.Transactions.Queries;
using Application.Features.Transactions.Queries.DTOs;
using Domain.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Wolverine;
namespace Api.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly ICacheService _cacheService;
    private readonly IMerchantServiceClient _merchantServiceClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public TransactionController(IMessageBus bus, ICacheService cacheService, IMerchantServiceClient merchantServiceClient, IHttpClientFactory httpClientFactory)
    {
        _bus = bus;
        _cacheService = cacheService;
        _merchantServiceClient = merchantServiceClient;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("test")]
    public async Task<IActionResult> TestMerchant(string merchantId)
    {
        var res = await _merchantServiceClient.GetKeysAsync(merchantId);
        return Ok(res);
    }
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromForm] string data, [FromForm] string signature)
    {
        if (!string.IsNullOrEmpty(signature) || !VerifySignature(data, signature))
        {
            return Unauthorized("Invalid signature.");
        }

        try
        {
            byte[] binaryData = Convert.FromBase64String(data);

            var command = JsonSerializer.Deserialize<CreateTransactionCommand>(binaryData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (command == null)
            {
                return BadRequest("Payload could not be deserialized.");
            }

            var redirectUrl = await _bus.InvokeAsync<PaymentPageRedirectUrl>(command);

            return Ok(redirectUrl);
        }
        catch (FormatException)
        {
            return BadRequest("Invalid Base64 string format.");
        }
        catch (JsonException)
        {
            return BadRequest("Invalid JSON structure.");
        }
    }
    [HttpGet("{merchantId}/stats")]
    public async Task<IActionResult> GetStats(string merchantId)
    {
        var query = new GetTransactionStatsQuery(merchantId);
        var res = await _bus.InvokeAsync<TransactionStatsResponse>(query);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [BindRequired][Range(1, int.MaxValue)] int page,
        [BindRequired][Range(1, 50)] int pageSize,
        string? merchantId,
        string? currency,
        TransactionStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTransactionsQuery(page, pageSize, merchantId, currency, status);
        var res = await _bus.InvokeAsync<PagedResponse<TransactionResponse>>(query, cancellationToken);
        return Ok(res);
    }

    [Authorize]
    [HttpGet("by-id/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        TransactionResponse transaction = await _bus.InvokeAsync<TransactionResponse>(new GetTransactionByIdQuery(id));

        if (transaction == null)
        {
            return NotFound();
        }

        return Ok(transaction);
    }

    [HttpGet("{token}")]
    public async Task<IActionResult> GetByToken(string token)
    {
        TransactionSession? session = await _cacheService.GetAsync<TransactionSession>(token);
        if (session == null)
        {
            return NotFound();
        }

        if (session.CreatedAt.AddMinutes(5) < DateTime.UtcNow)
        {
            return BadRequest(new { Message = "Session is expired" });
        }

        return Ok(session);
    }

    [HttpPost("{token}/pay")]
    public async Task<IActionResult> Pay(PaymentRequest request, string token)
    {
        PayCommand command = new PayCommand(token, request);
        await _bus.InvokeAsync(command);
        return Ok();
    }



    private bool VerifySignature(string data, string signature)
    {

        //SignatureHelpers.ComputeHmacSha256(data, );
        return true;
    }
}
