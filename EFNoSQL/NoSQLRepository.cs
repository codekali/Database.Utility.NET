using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Codekali.Net.Persistence.EFNoSQL
{
    public class NoSQLRepository<TDocument> : INoSQLRepository<TDocument>
            where TDocument : class
    {
        private readonly IMongoCollection<TDocument> collection;

        public NoSQLRepository()
        {
            var client = new MongoClient(IMongoDbSettings.ConnectionString);
            var database = client.GetDatabase(IMongoDbSettings.DatabaseName);
            collection = database.GetCollection<TDocument>(typeof(TDocument).Name);
        }
        public async Task<IList<TDocument>> GetAllAsync()
        {
            return await Task.FromResult(collection.AsQueryable().ToList());
        }

        public IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return collection.Find(filterExpression).ToEnumerable();
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public TDocument Find(Expression<Func<TDocument, bool>> filterExpression)
        {
            return collection.Find(filterExpression).FirstOrDefault();
        }

        public async Task<TDocument> FindAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq("Id", objectId);
            return collection.Find(filter).SingleOrDefault();
        }

        public Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq("Id", objectId);
                return collection.Find(filter).SingleOrDefaultAsync();
            });
        }


        public void Insert(TDocument document)
        {
            collection.InsertOne(document);
        }

        public Task InsertAsync(TDocument document)
        {
            return Task.Run(() => collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            collection.InsertMany(documents);
        }


        public async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await collection.InsertManyAsync(documents);
        }

        public void UpdateOne(Expression<Func<TDocument, string>> filterExpression, string oldValue, string newValue)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterExpression, oldValue);
            var update = Builders<TDocument>.Update.Set(filterExpression, newValue);
            collection.UpdateOne(filter, update);
        }

        public async Task UpdateOneAsync(Expression<Func<TDocument, string>> filterExpression, string oldValue, string newValue)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterExpression, oldValue);
            var update = Builders<TDocument>.Update.Set(filterExpression, newValue);
            await collection.UpdateOneAsync(filter, update);
        }

        public void Update(Expression<Func<TDocument, TDocument>> filterExpression, TDocument document)
        {

            var filter = Builders<TDocument>.Filter.Eq(filterExpression, document);
            collection.FindOneAndReplace(filter, document);
        }

        public async Task UpdateAsync(Expression<Func<TDocument, TDocument>> filterExpression, TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterExpression, document);
            await collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq("Id", objectId);
            collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq("Id", objectId);
                collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => collection.DeleteManyAsync(filterExpression));
        }

        public async Task<IList<TDocument>> FindAllAsync(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return await collection.Find(filterExpression).ToListAsync();
        }

        public async Task<IList<TProjected>> FindAllWithProjectionsAsync<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return await collection.Find(filterExpression).Project(projectionExpression).ToListAsync();
        }
    }
}

