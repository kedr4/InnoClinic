using Application.Abstrsctions.Persistance.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

public class ReceptionistsRepository(AuthDbContext context) : IReceptionistsRepository
{
    public async Task AddAsync(Receptionist receptionist, CancellationToken cancellationToken = default)
    {
        await context.Receptionists.AddAsync(receptionist, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var receptionist = await GetByIdAsync(id, cancellationToken);

        if (receptionist != null)
        {
            context.Receptionists.Remove(receptionist);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<Receptionist>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.Receptionists
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Receptionist> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Receptionists.FindAsync(id, cancellationToken);
    }

    public async Task UpdateAsync(Receptionist receptionist, CancellationToken cancellationToken = default)
    {
        context.Receptionists.Update(receptionist);
        await context.SaveChangesAsync(cancellationToken);
    }
}
