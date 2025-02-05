using Business.DTOs;
using Business.Exceptions;
using Business.Repositories.Interfaces;
using DataAccess.Repository.Interfaces;
using MediatR;

namespace Business.Offices.Queries.GetOfficeStatus;
public class GetOfficeStatusQueryHandler : IRequestHandler<GetOfficeStatusQuery, StatusDTO>
{
    private readonly IStatusRepository _statusRepository;
    private readonly IOfficeRepository _officeRepository;


    public GetOfficeStatusQueryHandler(IStatusRepository statusRepository, IOfficeRepository officeRepository)
    {
        _statusRepository = statusRepository;
        _officeRepository = officeRepository;
    }

    public async Task<StatusDTO> Handle(GetOfficeStatusQuery request, CancellationToken cancellationToken)
    {
        var status = await _statusRepository.GetOfficeStatusAsync(request.Id, cancellationToken);

        if (status is null)
        {
            throw new StatusNotFountException(request.Id);
        }

        StatusDTO statusDto = new StatusDTO();

        if (status.StatusType == DataAccess.Models.StatusType.Failed)
        {
            statusDto = StatusDTO.MapFromModel(status, null as OfficeDTO);
        }
        else
        {
            var office = await _officeRepository.GetByIdAsync(request.Id, cancellationToken);

            statusDto = StatusDTO.MapFromModel(status, office);
        }

        return statusDto;
    }
}
