using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;
using Patient.TestTask.RestApi.Configuration;
using Prakle.ManagementWarehouse.Api.Base.DataAccess;

namespace Patient.TestTask.RestApi.DataAccess;

public class BaseRepository<TDocument> : IBaseRepository<TDocument> where TDocument : Document
{
    private readonly IMongoCollection<TDocument> _collection;
    public FilterDefinitionBuilder<TDocument> FilterDefinitionBuilder { get; }

    public BaseRepository(IDatabaseConfiguration settings)
    {
        var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        FilterDefinitionBuilder = new FilterDefinitionBuilder<TDocument>();
    }

    public virtual Task<List<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToListAsync();
    }
    
    public async Task<IEnumerable<TDocument>> FilterByAsync(List<FilterDefinition<TDocument>> filters)
    {
        if (!filters.Any())
            return new List<TDocument>();
        
        return (await _collection.FindAsync(FilterDefinitionBuilder.And(filters))).ToEnumerable();
    }
    public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).FirstOrDefaultAsync();
    }
    
    public virtual async Task<TDocument> FindByIdAsync(Guid id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public virtual async Task InsertOneAsync(TDocument document)
    {
        await _collection.InsertOneAsync(document);
    }
    
    public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
    {
        if (documents.Count > 0) await _collection.InsertManyAsync(documents);
    }

    public virtual async Task ReplaceOneAsync(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        await _collection.FindOneAndReplaceAsync(filter, document);
    }
    
    public virtual async Task HardDeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await _collection.FindOneAndDeleteAsync(filterExpression);
    }

    private string GetCollectionName(Type documentType)
    {
        return ((CollectionNameAttribute)documentType.GetCustomAttributes(
                typeof(CollectionNameAttribute),
                true)
            .FirstOrDefault())?.Name;
    }
}