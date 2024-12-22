using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Options;

public class DatabaseOptions()
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Database connection string is required")]
    public string ConnectionString { get; set; } = string.Empty;
}
