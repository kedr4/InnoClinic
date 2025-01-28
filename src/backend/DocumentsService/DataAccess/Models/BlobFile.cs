namespace DataAccess.Models;

public class BlobFile
{
    public string Uri { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public Stream Content { get; set; }
    public string ContentType { get; set; } = string.Empty;
}
