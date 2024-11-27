using Application.Abstrsctions.Services;

namespace Application.Services;
public class OfficeService : IOfficeService
{
    public Task<bool> IsOfficeExistsAsync(Guid officeId, CancellationToken cancellationToken) => Task.FromResult(true);
}