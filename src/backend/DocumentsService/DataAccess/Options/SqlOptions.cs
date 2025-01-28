using System.ComponentModel.DataAnnotations;

namespace DataAccess.Options;

public class SqlOptions
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Database connection string is required")]
    public string DefaultConnection { get; set; } = string.Empty;
}
