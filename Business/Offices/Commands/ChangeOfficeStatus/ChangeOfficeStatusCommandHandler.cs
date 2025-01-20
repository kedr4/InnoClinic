using Business.Exceptions;
using Business.Repositories.Interfaces;
using MediatR;

namespace Business.Offices.Commands.ChangeOfficeStatus;

public class ChangeOfficeStatusCommandHandler : IRequestHandler<ChangeOfficeStatusCommand, Guid>
{
    private readonly IOfficeRepository _officeRepository;
    public ChangeOfficeStatusCommandHandler(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<Guid> Handle(ChangeOfficeStatusCommand command, CancellationToken cancellationToken)
    {
        if (await _officeRepository.GetByIdAsync(command.Id, cancellationToken) is null)
        {
            throw new OfficeNotFoundException(command.Id);
        }

        await _officeRepository.ChangeStatusAsync(command.Id, command.IsActive, cancellationToken);

        return command.Id;
    }
}
