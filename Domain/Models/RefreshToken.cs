namespace Domain.Models;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Token { get; set; }
    public DateTimeOffset AddedTime { get; set; }
    public DateTimeOffset ExpiryTime { get; set; }
}
