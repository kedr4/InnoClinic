using Business.DTOs;
using Business.Exceptions;
using Business.Repositories.Interfaces;
using MediatR;

namespace Business.Offices.Queries.GetOfficeById;

public class GetOfficeByIdQueryHandler : IRequestHandler<GetOfficeByIdQuery, OfficeDTO>
{
    private readonly IOfficeRepository _officeRepository;

    public GetOfficeByIdQueryHandler(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<OfficeDTO> Handle(GetOfficeByIdQuery request, CancellationToken cancellationToken)
    {
        var office = await _officeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (office is null)
        {
            throw new OfficeNotFoundException(request.Id);
        }

        var officeDto = new OfficeDTO
        {
            Id = office.Id,
            City = office.City,
            Street = office.Street,
            HouseNumber = office.HouseNumber,
            OfficeNumber = office.OfficeNumber,
            Address = office.Address,
            PhotoUrl = office.PhotoUrl,
            RegistryPhoneNumber = office.RegistryPhoneNumber,
            IsActive = office.IsActive
        };

        return officeDto;
    }
}
