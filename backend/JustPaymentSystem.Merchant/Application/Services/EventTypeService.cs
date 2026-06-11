using Application.Commands.EventType;
using Application.Dtos;
using Application.Interfaces.MappingProfiles;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using FluentValidation;

namespace Application.Services;

public class EventTypeService : IEventTypeService
{
    private readonly IEventTypeRepository _eventTypeRepository;
    private readonly IEventTypeMapper _mapper;
    private readonly IValidator<CreateEventTypeCommand> _createValidator;
    private readonly IValidator<UpdateEventTypeCommand> _updateValidator;
    private readonly IValidator<DeleteEventTypeCommand> _deleteValidator;
    private readonly IValidator<EventTypeIdCommand> _idValidator;

    public EventTypeService(
        IEventTypeRepository eventTypeRepository,
        IEventTypeMapper mapper,
        IValidator<CreateEventTypeCommand> createValidator,
        IValidator<UpdateEventTypeCommand> updateValidator,
        IValidator<DeleteEventTypeCommand> deleteValidator,
        IValidator<EventTypeIdCommand> idValidator)
    {
        _eventTypeRepository = eventTypeRepository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _deleteValidator = deleteValidator;
        _idValidator = idValidator;
    }

    public async Task<IReadOnlyList<EventTypeResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var eventTypes = await _eventTypeRepository.GetAllAsync(cancellationToken);
        return eventTypes.Select(_mapper.MapToResponse).ToList();
    }

    public async Task<EventTypeResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _idValidator.ValidateAndThrowAsync(new EventTypeIdCommand(id), cancellationToken);

        var eventType = await _eventTypeRepository.GetByIdAsync(id, trackChanges: false);
        return eventType is null ? null : _mapper.MapToResponse(eventType);
    }

    public async Task<EventTypeResponseDto> CreateAsync(CreateEventTypeDto request, CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(new CreateEventTypeCommand(request), cancellationToken);

        var eventType = _mapper.Map(request);

        await _eventTypeRepository.CreateAsync(eventType);
        await _eventTypeRepository.SaveChangesAsync();

        return _mapper.MapToResponse(eventType);
    }

    public async Task<EventTypeResponseDto?> UpdateAsync(Guid id, UpdateEventTypeDto request, CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(new UpdateEventTypeCommand(id, request), cancellationToken);

        var eventType = await _eventTypeRepository.GetByIdAsync(id, trackChanges: true);
        if (eventType is null)
        {
            return null;
        }

        _mapper.ApplyUpdate(eventType, request);

        _eventTypeRepository.Update(eventType);
        await _eventTypeRepository.SaveChangesAsync();

        return _mapper.MapToResponse(eventType);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _deleteValidator.ValidateAndThrowAsync(new DeleteEventTypeCommand(id), cancellationToken);

        var eventType = await _eventTypeRepository.GetByIdAsync(id, trackChanges: true);
        if (eventType is null)
        {
            return false;
        }

        _eventTypeRepository.Delete(eventType);
        await _eventTypeRepository.SaveChangesAsync();

        return true;
    }
}
