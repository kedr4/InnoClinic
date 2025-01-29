using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DocumentsDbContext : DbContext
{
    public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options)
    { }

    public DbSet<FileData> FileDatas { get; set; }
}
