using DataAccess.DTOs;
using DataAccess.Models;
using DataAccess.Repository.Interfaces;
using DataAccess.Setup;
using MongoDB.Driver;

namespace DataAccess.Repository;

public class StatusRepository : IStatusRepository
{
    private readonly IMongoCollection<Status> _statusCollection;

    public StatusRepository(MongoDBClient mongoDBClient)
    {
        _statusCollection = mongoDBClient.GetStatusCollection();
    }

    public async Task<Guid> AddOfficeStatusAsync(Status status, CancellationToken cancellationToken)
    {
        await _statusCollection.InsertOneAsync(status, null, cancellationToken);

        return status.Id;
    }

    public async Task UpdateOfficeStatusAsync(Status status, CancellationToken cancellationToken)
    {
        await _statusCollection.FindOneAndReplaceAsync(s => s.Id == status.Id, status, null, cancellationToken);
    }

    public async Task<Status?> GetOfficeStatusAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _statusCollection.Find(s => s.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Status> SetOfficeStatusAsync(Guid id, StatusType statusType, CancellationToken cancellationToken)
    {
        var status = await _statusCollection.Find(s => s.Id == id).FirstOrDefaultAsync(cancellationToken);
        status.StatusType = statusType;
        await _statusCollection.ReplaceOneAsync(s => s.Id == id, status, new ReplaceOptions(), cancellationToken);

        return status;
    }
}
