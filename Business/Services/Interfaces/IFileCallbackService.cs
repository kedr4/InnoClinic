namespace Business.Services.Interfaces;

public interface IFileCallbackService
{
    public Task SendCallback(string url, CancellationToken cancellationToken);
}
