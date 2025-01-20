using Business.Repositories.Interfaces;
using DataAccess.Models;
using DataAccess.Setup;
using MongoDB.Driver;

namespace DataAccess.Repository;

public class OfficeRepository : IOfficeRepository
{
    private readonly IMongoCollection<Office> _officeCollection;
    private readonly UpdateDefinitionBuilder<Office> _updateDefinitionBuilder;
    private readonly FilterDefinitionBuilder<Office> _filterDefinitionBuilder;

    public OfficeRepository(MongoDBClient mongoDBClient)
    {
        _officeCollection = mongoDBClient.GetOfficeCollection();
        _updateDefinitionBuilder = Builders<Office>.Update;
        _filterDefinitionBuilder = Builders<Office>.Filter;
    }

    public async Task AddAsync(Office office, CancellationToken cancellationToken = default)
    {
        await _officeCollection.InsertOneAsync(office, null, cancellationToken);
    }

    public async Task ChangeStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default)
    {
        var filter = _filterDefinitionBuilder.Eq(o => o.Id, id);
        var update = _updateDefinitionBuilder.Set(o => o.IsActive, isActive);

        await _officeCollection.UpdateOneAsync(filter, update, null, cancellationToken);
    }

    public Task<List<Office>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return _officeCollection
            .Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Office>> GetActiveAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return _officeCollection
            .Find(o => o.IsActive)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Office>> GetInactiveAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return _officeCollection
            .Find(o => !o.IsActive)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Office> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _officeCollection.Find(o => o.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(Office office, CancellationToken cancellationToken = default)
    {
        await _officeCollection.ReplaceOneAsync(o => o.Id == office.Id, office, new ReplaceOptions(), cancellationToken);
    }
}
