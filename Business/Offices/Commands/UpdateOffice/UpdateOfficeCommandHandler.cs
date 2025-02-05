using Business.Exceptions;
using Business.Repositories.Interfaces;
using DataAccess.Models;
using MediatR;

namespace Business.Offices.Commands.UpdateOffice;

public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand, Guid>
{
    private readonly IOfficeRepository _officeRepository;

    public UpdateOfficeCommandHandler(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<Guid> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
    {
        if (await _officeRepository.GetByIdAsync(request.Id, cancellationToken) is null)
        {
            throw new OfficeNotFoundException(request.Id);
        }

        var office = new Office
        {
            Id = request.Id,
            City = request.City,
            Street = request.Street,
            HouseNumber = request.HouseNumber,
            OfficeNumber = request.OfficeNumber,
            Address = $"{request.City}, {request.Street} {request.HouseNumber}, office No.{request.OfficeNumber}",
            Photo = request.Photo,
            RegistryPhoneNumber = request.RegistryPhoneNumber,
            IsActive = request.IsActive
        };

        await _officeRepository.UpdateAsync(office, cancellationToken);

        return office.Id;
    }
}
