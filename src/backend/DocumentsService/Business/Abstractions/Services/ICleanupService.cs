namespace Business.Abstractions.Services;

public interface ICleanupService
{
    Task CleanupAsync(CancellationToken cancellationToken = default);
}
