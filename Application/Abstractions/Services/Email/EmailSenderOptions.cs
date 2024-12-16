using System.ComponentModel.DataAnnotations;

namespace Application.Abstractions.Services.Email;
public class EmailSenderOptions
{
    public string Sender { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmUrl { get; set; }
    public bool UseSsl { get; set; } = false;
}