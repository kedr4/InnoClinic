using Domain.Models;

namespace Application.Abstrsctions.Persistance.Repositories;

public interface IReceptionistsRepository
{
    public Task<Receptionist> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<List<Receptionist>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    public Task AddAsync(Receptionist receptionist, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Receptionist receptionist, CancellationToken cancellationToken = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);

}
