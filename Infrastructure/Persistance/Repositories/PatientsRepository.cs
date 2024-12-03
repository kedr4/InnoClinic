using Application.Abstrsctions.Persistance.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

public class PatientsRepository(AuthDbContext context) : IPatientsRepository
{
    public async Task<Patient> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Patients.FindAsync(id, cancellationToken);
    }

    public async Task<List<Patient>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.Patients
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        await context.Patients.AddAsync(patient, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        context.Patients.Update(patient);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var patient = await GetByIdAsync(id, cancellationToken);

        if (patient != null)
        {
            context.Patients.Remove(patient);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}