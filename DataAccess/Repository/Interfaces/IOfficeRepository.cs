using DataAccess.Models;

namespace Business.Repositories.Interfaces;

public interface IOfficeRepository
{
    public Task<Office> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<Office>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
    public Task<List<Office>> GetActiveAsync(int page, int pageSize, CancellationToken cancellationToken);
    public Task<List<Office>> GetInactiveAsync(int page, int pageSize, CancellationToken cancellationToken);
    public Task SetOfficePhotoToNull(Guid id, CancellationToken cancellationToken = default);
    public Task AddAsync(Office office, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Office office, CancellationToken cancellationToken);
    public Task ChangeStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken);
}
