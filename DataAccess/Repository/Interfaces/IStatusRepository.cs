using DataAccess.DTOs;
using DataAccess.Models;

namespace DataAccess.Repository.Interfaces;

public interface IStatusRepository
{
    public Task<Status?> GetOfficeStatusAsync(Guid id, CancellationToken cancellationToken);
    public Task<Guid> AddOfficeStatusAsync(Status status, CancellationToken cancellationToken);
    public Task UpdateOfficeStatusAsync(Status status, CancellationToken cancellationToken);

    public Task<Status> SetOfficeStatusAsync(Guid id, StatusType statusType, CancellationToken cancellationToken);
}
