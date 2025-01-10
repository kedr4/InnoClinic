using DataAccess.Models;

namespace Business.Repositories.Interfaces;

public interface IOfficeRepository
{
    public Task<Office> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<Office>> GetAllAsync(CancellationToken cancellationToken);
    public Task AddAsync(Office office, CancellationToken cancellationToken);
    public Task UpdateAsync(Office office, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    public Task ChangeStatusAsync(Office office, CancellationToken cancellationToken);
}
