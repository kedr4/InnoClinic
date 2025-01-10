using Business.Repositories.Interfaces;
using DataAccess.Models;
using MediatR;

namespace Business.Commands.CreateOffice;

public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, Guid>
{
    private readonly IOfficeRepository _officeRepository;

    public CreateOfficeCommandHandler(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<Guid> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = new Office
        {
            Id = Guid.NewGuid(),
            City = request.City,
            Street = request.Street,
            HouseNumber = request.HouseNumber,
            OfficeNumber = request.OfficeNumber,
            Address = $"{request.City}, {request.Street} {request.HouseNumber}, office No.{request.OfficeNumber}",
            PhotoUrl = request.PhotoUrl,
            RegistryPhoneNumber = request.RegistryPhoneNumber,
            IsActive = request.IsActive

        };

        await _officeRepository.AddAsync(office, cancellationToken);

        return office.Id;
    }
}