using Application.Abstractions.Services;

namespace Application.Services;

public class OfficeService : IOfficeService
{
    public Task<bool> IsOfficeExistsAsync(Guid officeId, CancellationToken cancellationToken) => Task.FromResult(true);
}