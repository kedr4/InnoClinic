using Business.Abstractions.Services;
using DataAccess.Abstractions.Repositories;
using DataAccess.Abstractions.Services;

namespace Business.Services;

public class CleanupService : ICleanupService
{
    private readonly IBlobStorageRepository _blobStorageRepository;
    private readonly IFileDataRepository _fileDataRepository;
    private readonly int chunkSize = 2;

    public CleanupService(IBlobStorageRepository blobStorageRepository, IFileDataRepository fileDataRepository)
    {
        _blobStorageRepository = blobStorageRepository;
        _fileDataRepository = fileDataRepository;
    }

    public async Task CleanupAsync(CancellationToken cancellationToken = default)
    {
        var filesToDelete = await _fileDataRepository.GetDeletedFilesAsync(cancellationToken);
        int deletedFilesCount = 0;

        foreach (var file in filesToDelete)
        {
            await _blobStorageRepository.DeleteFileAsync(file.Id, cancellationToken);
            await _fileDataRepository.HardDeleteFileDataAsync(file.Id, cancellationToken);
            await _fileDataRepository.SaveChangesAsync(cancellationToken);
            deletedFilesCount++;

            if (deletedFilesCount == chunkSize)
            {
                break;
            }
        }
    }
}
