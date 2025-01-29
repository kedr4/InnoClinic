namespace DataAccess.Abstractions.Repositories;

public interface IBlobStorageRepository
{
    public Task UploadFileAsync(Guid id, Stream content, string contentType, CancellationToken cancellationToken);
    public Task<Stream> GetFileAsync(Guid id, CancellationToken cancellationToken);
    public Task DeleteFileAsync(Guid id, CancellationToken cancellationToken);
}
