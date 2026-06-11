using Application.Dtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventTypesController : ControllerBase
{
    private readonly IEventTypeService _eventTypeService;

    public EventTypesController(IEventTypeService eventTypeService)
    {
        _eventTypeService = eventTypeService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var eventTypes = await _eventTypeService.GetAllAsync(cancellationToken);
        return Ok(eventTypes);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var eventType = await _eventTypeService.GetByIdAsync(id, cancellationToken);
        return eventType is null ? NotFound() : Ok(eventType);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateEventTypeDto request, CancellationToken cancellationToken)
    {
        var eventType = await _eventTypeService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = eventType.Id }, eventType);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, UpdateEventTypeDto request, CancellationToken cancellationToken)
    {
        var eventType = await _eventTypeService.UpdateAsync(id, request, cancellationToken);
        return eventType is null ? NotFound() : Ok(eventType);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _eventTypeService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
