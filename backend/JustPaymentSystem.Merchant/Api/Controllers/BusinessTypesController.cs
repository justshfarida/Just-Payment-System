using Application.Dtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusinessTypesController : ControllerBase
{
    private readonly IBusinessTypeService _businessTypeService;

    public BusinessTypesController(IBusinessTypeService businessTypeService)
    {
        _businessTypeService = businessTypeService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var businessTypes = await _businessTypeService.GetAllAsync(cancellationToken);
        return Ok(businessTypes);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var businessType = await _businessTypeService.GetByIdAsync(id, cancellationToken);
        return businessType is null ? NotFound() : Ok(businessType);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateBusinessTypeDto request, CancellationToken cancellationToken)
    {
        var businessType = await _businessTypeService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = businessType.Id }, businessType);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, UpdateBusinessTypeDto request, CancellationToken cancellationToken)
    {
        var businessType = await _businessTypeService.UpdateAsync(id, request, cancellationToken);
        return businessType is null ? NotFound() : Ok(businessType);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _businessTypeService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
