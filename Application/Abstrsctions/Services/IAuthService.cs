namespace Application.Abstrsctions.Services;

public interface IAuthService
{
    public Task RegisterPatientAsync(string email, string password, string firstName, string lastName, string middleName,
       DateTimeOffset dateOfBirth, bool isLinkedToAccount, Guid accountId);
    public Task RegisterDoctorAsync(string email, string password, string firstName, string lastName, string middleName,
       DateTimeOffset dateOfBirth, Guid accountId, Guid specializationId, Guid officeId, int careerStartYear);
    public Task RegisterReceptionistAsync(string email, string password, string firstName, string lastName, string middleName,
         DateTimeOffset dateOfBirth, Guid accountId, Guid officeId);
    public Task LoginPatientAsync(string email, string password);
    public Task LoginDoctorAsync(string email, string password);
    public Task LoginReceptionistAsync(string email, string password);
    public Task LogoutAsync(string refreshToken);
    public Task DeletePatientProfileAsync(Guid userId);
    public Task DeleteDoctorProfileAsync(Guid userId);
    public Task DeleteReceptionistProfileAsync(Guid userId);
    public Task<string> RefreshAccessTokenAsync(string refreshToken);
}
