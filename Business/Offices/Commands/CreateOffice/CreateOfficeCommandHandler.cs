using Business.Repositories.Interfaces;
using ContractsLib;
using DataAccess.DTOs;
using DataAccess.Models;
using DataAccess.Repository.Interfaces;
using MassTransit;
using MediatR;

namespace Business.Offices.Commands.CreateOffice;

public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, Guid>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IBus _bus;


    public CreateOfficeCommandHandler(IOfficeRepository officeRepository, IStatusRepository statusRepository, IBus bus)
    {
        _officeRepository = officeRepository;
        _statusRepository = statusRepository;
        _bus = bus;
    }

    public async Task<Guid> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = new Office
        {
            Id = Guid.CreateVersion7(),
            City = request.City,
            Street = request.Street,
            HouseNumber = request.HouseNumber,
            OfficeNumber = request.OfficeNumber,
            Address = $"{request.City}, {request.Street} {request.HouseNumber}, office No.{request.OfficeNumber}",
            Photo = request.Photo.FileName,
            RegistryPhoneNumber = request.RegistryPhoneNumber,
            IsActive = request.IsActive

        };

        var officeStatus = new Status
        {
            Id = office.Id,
            StatusType = StatusType.Pending,
            ErrorMessage = null
        };


        if (request.Photo is not null)
        {
            using var memoryStream = new MemoryStream();
            await request.Photo.CopyToAsync(memoryStream, cancellationToken);
            var fileContent = memoryStream.ToArray();

            var endpoint = await _bus.GetSendEndpoint(new Uri("queue:file-upload-queue"));

            await endpoint.Send(new FileUploadRequest()
            {
                UserId = request.UserId,
                FileId = office.Id,
                FileName = request.Photo.FileName,
                FileContent = fileContent,
                ContentType = request.Photo.ContentType
            }, cancellationToken);
        }
        else
        {
            officeStatus.StatusType = StatusType.Failed;
        }

        await _officeRepository.AddAsync(office, cancellationToken);       
        await _statusRepository.AddOfficeStatusAsync(officeStatus, cancellationToken);

        return office.Id;
    }
}