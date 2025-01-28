using DataAccess.Models;

namespace Business.Abstractions.Services;

public interface IFilesService
{
    public Task<Guid> UploadFileAsync(BlobFile blobFile, CancellationToken cancellationToken);
    public Task DeleteFileAsync(Guid fileId, CancellationToken cancellationToken);
    public Task<(Stream fileStream, FileData data)> GetFileAsync(Guid fileId, CancellationToken cancellationToken);
}
