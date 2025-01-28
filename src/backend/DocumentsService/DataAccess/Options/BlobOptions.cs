using System.ComponentModel.DataAnnotations;

namespace DataAccess.Options;

public class BlobOptions
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Account name is required")]
    public string AccountName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Account key is required")]
    public string AccountKey { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Blob service url is required")]
    public string BlobServiceUrl { get; set; } = string.Empty;
}
