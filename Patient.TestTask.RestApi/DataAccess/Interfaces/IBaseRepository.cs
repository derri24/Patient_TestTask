using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Patient.TestTask.RestApi.DataAccess;

namespace Prakle.ManagementWarehouse.Api.Base.DataAccess;

public interface IBaseRepository<TDocument> where TDocument : Document
{
    FilterDefinitionBuilder<TDocument> FilterDefinitionBuilder { get; }
    Task<List<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression);
    Task<IEnumerable<TDocument>> FilterByAsync(List<FilterDefinition<TDocument>> filters);
    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
    Task<TDocument> FindByIdAsync(Guid id);
    Task InsertOneAsync(TDocument document);
    Task InsertManyAsync(ICollection<TDocument> documents);
    Task ReplaceOneAsync(TDocument document);
    Task HardDeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

}