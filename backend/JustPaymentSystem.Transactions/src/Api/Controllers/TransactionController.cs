using Microsoft.AspNetCore.Mvc;
using Wolverine;
using System.Text.Encodings;
using System.Text;
using Application.Features.Transactions.Commands;
using System.Text.Json;
using Domain.Events;
using Application.Features.Transactions.Queries.DTOs;
using Application.Features.Transactions.Queries;
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
