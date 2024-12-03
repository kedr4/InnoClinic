using Application.Abstrsctions.Persistance.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;
public class DoctorsRepository(AuthDbContext context) : IDoctorsRepository
{

    public async Task AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        await context.Doctors.AddAsync(doctor, cancellationToken);
        await context.SaveChangesAsync();  
    }

    public async Task DeleteAsync(Guid doctorId, CancellationToken cancellationToken = default)
    {
        var doctor = await GetByIdAsync(doctorId);

        if (doctor != null)
        {
            context.Doctors.Remove(doctor);
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<Doctor>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.Doctors
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
    }

    public async Task<Doctor> GetByIdAsync(Guid doctorId, CancellationToken cancellationToken = default)
    {
        return await context.Doctors.FindAsync(doctorId, cancellationToken);
    }

    public async Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        context.Doctors.Update(doctor);
        await context.SaveChangesAsync(cancellationToken);
    }
}
