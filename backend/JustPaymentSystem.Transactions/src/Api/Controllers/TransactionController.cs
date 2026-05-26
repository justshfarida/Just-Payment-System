using Application.Common.Models;
using Application.Features.Transactions.Commands;
using Application.Features.Transactions.Queries;
using Application.Features.Transactions.Queries.DTOs;
using Domain.Events;
using Domain.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Wolverine;
namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IMessageBus _bus;

    public TransactionController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromForm] string data, [FromForm] string signature)
    {
        if (!string.IsNullOrEmpty(signature) && !VerifySignature(data, signature))
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

            var res = await _bus.InvokeAsync<TransactionCreated>(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = res.TransactionId },
                res
            );
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

    [HttpGet]
    public async Task<IActionResult> GetAll(int page, int pageSize, string? merchantId, string? currency, TransactionStatus status, CancellationToken cancellationToken =  default)
    {
        var query = new GetTransactionsQuery(page, pageSize, merchantId, currency, status);
        var res = await _bus.InvokeAsync<PagedResponse<TransactionResponse>>(query, cancellationToken);
        return Ok(res);
    }
   
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        TransactionResponse transaction = await _bus.InvokeAsync<TransactionResponse>(new GetTransactionByIdQuery(id));

        if (transaction == null)
        {
            return NotFound();
        }

        return Ok(transaction);
    }

    private bool VerifySignature(string data, string signature)
    {
        return true;
    }
}
