using Business.Services.Interfaces;

namespace Business.Services;

public class FileCallbackService(HttpClient httpClient) : IFileCallbackService
{

    public Task SendCallback(string url, CancellationToken cancellationToken)
    {
        return httpClient.PostAsync(url, null, cancellationToken);
    }
}
