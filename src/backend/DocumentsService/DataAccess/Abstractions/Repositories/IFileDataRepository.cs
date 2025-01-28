using DataAccess.Models;

namespace DataAccess.Abstractions.Services;

public interface IFileDataRepository
{
    public Task<Guid> AddFileDataAsync(FileData filedata, CancellationToken cancellationToken);
    public Task<FileData?> GetFileDataAsync(Guid id, CancellationToken cancellationToken);
    public Task DeleteFileDataAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<FileData>> GetDeletedFilesAsync(CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
