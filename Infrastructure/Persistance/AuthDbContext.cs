using Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistance;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Receptionist> Receptionists { get; set; }

}
