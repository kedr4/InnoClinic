using Business.Abstractions.Services;
using Business.Exceptions;
using DataAccess.Abstractions.Repositories;
using DataAccess.Abstractions.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Business.Services;

public class FilesService : IFilesService
{
    private readonly IBlobStorageRepository _blobStorageRepository;
    private readonly IFileDataRepository _fileDataRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string[] _allowedContentTypes = { "image/jpeg", "image/png" };

    public FilesService(IBlobStorageRepository blobstorageRepository, IFileDataRepository fileDataRepository, IHttpContextAccessor httpContextAccessor)
    {
        _blobStorageRepository = blobstorageRepository;
        _fileDataRepository = fileDataRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task DeleteFileAsync(Guid fileId, CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromAccessToken();

        if (userId == Guid.Empty)
        {
            throw new UnauthorizedException();
        }

        var fileToDelete = await _fileDataRepository.GetFileDataAsync(fileId, cancellationToken);

        if (fileToDelete is null)
        {
            throw new FileDataNotFoundException(fileId);
        }

        if (fileToDelete.IsDeleted)
        {
            throw new FileAlreadyDeletedException(fileId);
        }

        if (userId != fileToDelete.UserId)
        {
            throw new UnauthorizedException();
        }

        await _fileDataRepository.SoftDeleteFileDataAsync(fileId, cancellationToken);
        await _fileDataRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<(Stream fileStream, FileData data)> GetFileAsync(Guid fileId, CancellationToken cancellationToken)
    {
        var desiredFile = await _fileDataRepository.GetFileDataAsync(fileId, cancellationToken);

        if (desiredFile is null || desiredFile.IsDeleted)
        {
            throw new FileDataNotFoundException(fileId);
        }

        var fileStream = await _blobStorageRepository.GetFileAsync(desiredFile.Id, cancellationToken);

        return (fileStream, desiredFile);
    }

    public async Task<Guid> UploadFileAsync(BlobFile blobFile, CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromAccessToken();


        if (blobFile.Content is null || blobFile.Content.Length == 0)
        {
            throw new EmptyFileException();
        }

        if (!_allowedContentTypes.Contains(blobFile.ContentType))
        {
            throw new InvalidFileTypeException();
        }

        var fileId = Guid.CreateVersion7();
        await _blobStorageRepository.UploadFileAsync(fileId, blobFile.Content, blobFile.ContentType, cancellationToken);

        var fileData = new FileData
        {
            Id = fileId,
            FileName = blobFile.Uri,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        await _fileDataRepository.AddFileDataAsync(fileData, cancellationToken);
        await _fileDataRepository.SaveChangesAsync(cancellationToken);

        return fileId;
    }

    private Guid GetUserIdFromAccessToken()
    {
        var user = _httpContextAccessor.HttpContext.User;

        if (user is null)
        {
            throw new UnauthorizedException();
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = Guid.TryParse(userId, out var parsedGuid);

        if (!result || parsedGuid == Guid.Empty)
        {
            throw new UnauthorizedException();
        }

        return parsedGuid;
    }
}
