namespace Application.Abstrsctions.Services;

public interface IAuthService
{
    public Task RegisterPatientAsync(string email, string password, string firstName, string lastName, string middleName,
       DateTimeOffset dateOfBirth, bool isLinkedToAccount, Guid accountId);
    public Task RegisterDoctorAsync(string email, string password, string firstName, string lastName, string middleName,
       DateTimeOffset dateOfBirth, Guid accountId, Guid specializationId, Guid officeId, int careerStartYear);
    public Task RegisterReceptionistAsync(string email, string password, string firstName, string lastName, string middleName,
         DateTimeOffset dateOfBirth, Guid accountId, Guid officeId);
    public Task LoginAsync(string email, string password);
    public Task LogoutAsync();
    public Task DeleteProfileAsync(Guid userId);
    public Task<string> RefreshAccessTokenAsync(string refreshToken);
}
