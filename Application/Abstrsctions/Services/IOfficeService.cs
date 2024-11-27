namespace Application.Abstrsctions.Services;
public interface IOfficeService
{
    public Task<bool> IsOfficeExistsAsync(Guid officeId, CancellationToken cancellationToken);

}
