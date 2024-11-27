using Domain.Models;

namespace Application.Abstrsctions.Persistance.Repositories;

public interface IPatientsRepository
{
    public Task<Patient> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<List<Patient>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    public Task AddAsync(Patient patient, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

}
