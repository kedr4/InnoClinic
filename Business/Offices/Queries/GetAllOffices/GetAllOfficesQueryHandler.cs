using Business.DTOs;
using Business.Repositories.Interfaces;
using DataAccess.Models;
using MediatR;

namespace Business.Offices.Queries.GetAllOffices;

public class GetAllOfficesQueryHandler : IRequestHandler<GetAllOfficesQuery, List<OfficeDTO>>
{
    private readonly IOfficeRepository _officeRepository;

    public GetAllOfficesQueryHandler(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<List<OfficeDTO>> Handle(GetAllOfficesQuery request, CancellationToken cancellationToken)
    {
        List<Office> offices;

        switch (request.IsActive)
        {
            case null:
                offices = await _officeRepository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
                break;

            case true:
                offices = await _officeRepository.GetActiveAsync(request.Page, request.PageSize, cancellationToken);
                break;

            case false:
                offices = await _officeRepository.GetInactiveAsync(request.Page, request.PageSize, cancellationToken);
                break;
        }

        var officeDtos = offices.Select(OfficeDTO.MapFromModel).ToList();

        return officeDtos;
    }
}
