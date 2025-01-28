using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : DbContext(options)
{
    public DbSet<FileData> FileDatas { get; set; }
}

