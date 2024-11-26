using Domain.Models;

namespace Application.Abstrsctions.Persistance.Repositories
{
    public interface IPatientsRepository
    {
        Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Patient patient, CancellationToken cancellationToken = default);
        Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
