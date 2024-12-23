using System.ComponentModel.DataAnnotations;

namespace Application.Options;
public class EmailSenderOptions
{
    [Required(ErrorMessage = "Sender email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Sender { get; set; }

    [Required(ErrorMessage = "SMTP Server is required.")]
    public string SmtpServer { get; set; }

    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535.")]
    public int Port { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirmation URL is required.")]
    [Url(ErrorMessage = "Invalid URL format.")]
    public string ConfirmUrl { get; set; }

    public bool UseSsl { get; set; } = false;
}