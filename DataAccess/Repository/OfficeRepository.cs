using Business.Repositories.Interfaces;
using DataAccess.Models;
using DataAccess.Setup;
using MongoDB.Driver;

namespace DataAccess.Repository;

public class OfficeRepository : IOfficeRepository
{
    private readonly IMongoCollection<Office> _officeCollection;

    public OfficeRepository(MongoDBClient mongoDBClient)
    {
        _officeCollection = mongoDBClient.GetOfficeCollection();
    }

    public async Task AddAsync(Office office, CancellationToken cancellationToken = default)
    {
        await _officeCollection.InsertOneAsync(office);
    }

    public async Task ChangeStatusAsync(Office office, CancellationToken cancellationToken = default)
    {
        var update = Builders<Office>.Update.Set(o => o.IsActive, office.IsActive);
        await _officeCollection.UpdateOneAsync(o => o.Id == office.Id, update);

    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _officeCollection.DeleteOneAsync(o => o.Id == id);
    }

    public async Task<List<Office>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _officeCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Office> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _officeCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Office office, CancellationToken cancellationToken = default)
    {
        await _officeCollection.ReplaceOneAsync(o => o.Id == office.Id, office);
    }

}
