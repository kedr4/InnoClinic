using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DataAccess.Abstractions.Repositories;
using DataAccess.Options;
using Microsoft.Extensions.Options;

namespace DataAccess.Repositories;

public class BlobStorageRepository : IBlobStorageRepository
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "documents";

    public BlobStorageRepository(IOptions<BlobOptions> blobOptions)
    {
        var options = blobOptions.Value;
        _blobServiceClient = new BlobServiceClient(
                   new Uri(options.BlobServiceUrl),
                   new Azure.Storage.StorageSharedKeyCredential(options.AccountName, options.AccountKey)
               );
    }

    public async Task DeleteFileAsync(Guid id, CancellationToken cancellationToken)
    {
        var containerClient = await GetContainerClientAsync();
        var blobClient = containerClient.GetBlobClient(id.ToString());

        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<Stream> GetFileAsync(Guid id, CancellationToken cancellationToken)
    {
        var containerClient = await GetContainerClientAsync();
        var blobClient = containerClient.GetBlobClient(id.ToString());
        var response = await blobClient.DownloadAsync(cancellationToken);

        return response.Value.Content;
    }

    public async Task UploadFileAsync(Guid id, Stream content, string contentType, CancellationToken cancellationToken)
    {
        var containerClient = await GetContainerClientAsync();
        var blobClient = containerClient.GetBlobClient(id.ToString());
        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

        await blobClient.UploadAsync(content, blobHttpHeaders);
    }

    private async Task<BlobContainerClient> GetContainerClientAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

        return containerClient;
    }
}
