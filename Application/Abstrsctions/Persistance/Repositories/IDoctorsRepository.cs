using Domain.Models;

namespace Application.Abstrsctions.Persistance.Repositories
{
    public interface IDoctorsRepository
    {
        public Task<Doctor> GetByIdAsync(Guid doctorId, CancellationToken cancellationToken = default);
        public Task<List<Doctor>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        public Task AddAsync(Doctor doctor, CancellationToken cancellationToken = default);
        public Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default);
        public Task DeleteAsync(Guid doctorId, CancellationToken cancellationToken = default);
    }
}
