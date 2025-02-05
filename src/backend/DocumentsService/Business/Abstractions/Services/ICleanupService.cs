namespace Business.Abstractions.Services;

public interface ICleanupService
{
    public Task CleanupAsync(CancellationToken cancellationToken = default);
}
