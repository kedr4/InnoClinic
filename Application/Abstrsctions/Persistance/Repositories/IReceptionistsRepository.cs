using Domain.Models;

namespace Application.Abstrsctions.Persistance.Repositories
{
    public interface IReceptionistsRepository
    {
        Task<Receptionist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Receptionist>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Receptionist receptionist, CancellationToken cancellationToken = default);
        Task UpdateAsync(Receptionist receptionist, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
