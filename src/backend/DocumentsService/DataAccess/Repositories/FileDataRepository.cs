using DataAccess.Abstractions.Services;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services;

public class FileDataRepository : IFileDataRepository
{
    private readonly DocumentsDbContext _context;

    public FileDataRepository(DocumentsDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddFileDataAsync(FileData filedata, CancellationToken cancellationToken)
    {
        await _context.FileDatas.AddAsync(filedata, cancellationToken);

        return filedata.Id;
    }

    public async Task DeleteFileDataAsync(Guid id, CancellationToken cancellationToken)
    {
        var fileData = await _context.FileDatas.FindAsync(id, cancellationToken);

        if (fileData is not null)
        {
            fileData.IsDeleted = true;
            _context.FileDatas.Update(fileData);
        }
    }

    public async Task<List<FileData>> GetDeletedFilesAsync(CancellationToken cancellationToken)
    {
        var deletedFiles = await _context.FileDatas.Where(file => file.IsDeleted).ToListAsync(cancellationToken);

        return deletedFiles;
    }

    public async Task<FileData?> GetFileDataAsync(Guid id, CancellationToken cancellationToken)
    {
        var fileData = await _context.FileDatas.FirstOrDefaultAsync(file => file.Id == id, cancellationToken);

        return fileData;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
