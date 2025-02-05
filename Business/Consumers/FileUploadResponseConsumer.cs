using Business.Repositories.Interfaces;
using ContractsLib;
using DataAccess.DTOs;
using DataAccess.Models;
using DataAccess.Repository.Interfaces;
using MassTransit;
using Serilog;

namespace Business.Consumers;


public class FileUploadResponseConsumer : IConsumer<FileUploadResponse>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly ILogger _logger;

    public FileUploadResponseConsumer(IOfficeRepository officeRepository, IStatusRepository statusRepository, ILogger logger)
    {
        _officeRepository = officeRepository;
        _statusRepository = statusRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<FileUploadResponse> context)
    {
        var response = context.Message;

        if (!response.IsSuccess)
        {
            _logger.Error("Offices Service got failure response");

            await _officeRepository.DeleteAsync(response.FileId, CancellationToken.None);
            _logger.Information("Deleted office");

            var status = new Status
            {
                Id = response.FileId,
                StatusType = StatusType.Failed,
                ErrorMessage = response.Message
            };

            await _statusRepository.UpdateOfficeStatusAsync(status, CancellationToken.None);
            _logger.Information("Updated office status");

        }
        else
        {
            _logger.Information("Offices Service got success response");

            await _statusRepository.SetOfficeStatusAsync(response.FileId, StatusType.Success, CancellationToken.None);

            _logger.Information("Updated office status");
        }
    }
}
