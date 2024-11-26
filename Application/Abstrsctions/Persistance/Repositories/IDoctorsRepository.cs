using Domain.Models;

namespace Application.Abstrsctions.Persistance.Repositories
{
    public interface IDoctorsRepository
    {
        Task AddAsync(Doctor doctor, CancellationToken cancellationToken = default);
        Task<Doctor?> GetByIdAsync(Guid doctorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid doctorId, CancellationToken cancellationToken = default);



    }
}
