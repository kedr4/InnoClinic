using Business.Abstractions.Services;
using ContractsLib;
using DataAccess.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ILogger = Serilog.ILogger;

namespace Business.Services;

public class FileUploadRequestConsumer : IConsumer<FileUploadRequest>
{
    private readonly IFilesService _fileService;
    private readonly ILogger _logger;

    public FileUploadRequestConsumer(IFilesService fileService, ILogger logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<FileUploadRequest> context)
    {
        _logger.Warning("Consumer Activated");
        var fileRequest = context.Message;
        var fileStream = new MemoryStream(fileRequest.FileContent);

        var endpoint = await context.GetSendEndpoint(new Uri("queue:file-upload-response"));

        try
        {
            var file = new BlobFile
            {

                Uri = fileRequest.FileName,
                Id = fileRequest.FileId,
                Content = fileStream,
                ContentType = fileRequest.ContentType,
            };

            var fileId = await _fileService.UploadFileWithUserIdAsync(file, fileRequest.UserId, CancellationToken.None);

            _logger.Warning("File uploaded. Sending success response");

            await endpoint.Send(new FileUploadResponse
            {
                FileId = fileId,
                IsSuccess = true,
                Message = null,
                StackTrace = null
            });

            _logger.Warning("Success response sent");
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed saving to DB {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}", ex);
            _logger.Warning ("Sending failure response");
            
            await endpoint.Send(new FileUploadResponse
            {
                FileId = fileRequest.FileId,
                IsSuccess = false,
                Message = ex.Message,
                StackTrace = ex.StackTrace
            });

            _logger.Warning("Failure response sent");
        }
    }
}
 