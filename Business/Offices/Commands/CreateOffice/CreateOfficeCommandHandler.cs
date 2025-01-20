using Business.Repositories.Interfaces;
using Business.Services.Interfaces;
using DataAccess.Models;
using MediatR;

namespace Business.Offices.Commands.CreateOffice;

public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, Guid>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IFileCallbackService _fileCallbackService;

    public CreateOfficeCommandHandler(IOfficeRepository officeRepository, IFileCallbackService fileCallbackService)
    {
        _officeRepository = officeRepository;
        _fileCallbackService = fileCallbackService;
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

        //if (!string.IsNullOrWhiteSpace(request.PhotoUrl))
        //{
        //    await _fileCallbackService.SendCallback(request.PhotoUrl, cancellationToken);
        //}

        return office.Id;
    }
}