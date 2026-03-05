using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenericFunctions.EFNoSQL
{
    /// <summary>
    /// This interface is implemented by all MongoDB repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TDocument">Main Entity type this repository works on</typeparam>
    public interface INoSQLRepository<TDocument>
        where TDocument : class
    {
        /// <summary>
        /// Returns all the documents in the Collection
        /// </summary>
        /// <returns>List of <typeparamref name="TDocument"/></returns>
        Task<IList<TDocument>> GetAllAsync();

        /// <summary>
        /// Find all documents based on given <paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">Condition to filter documents</param>
        /// <returns></returns>
        Task<IList<TDocument>> FindAllAsync(Expression<Func<TDocument, bool>> filterExpression);
        
        /// <summary>
        /// Find all documents based on given <paramref name="predicate"/> and get projected values based on <paramref name="projectionExpression"/>
        /// </summary>
        /// <typeparam name="TProjected">Returned properties</typeparam>
        /// <param name="predicate">Condition to filter documents</param>
        /// <param name="projection">condition to get specified properties</param>
        /// <returns></returns>
        Task<IList<TProjected>> FindAllWithProjectionsAsync<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression);

        /// <summary>
        /// Deletes one document by function.
        /// Notice that: Only one entity that fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        TDocument Find(Expression<Func<TDocument, bool>> predicate);


        /// <summary>
        /// Deletes one document by function.
        /// Notice that: Only one entity that fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        Task<TDocument> FindAsync(Expression<Func<TDocument, bool>> predicate);


        /// <summary>
        /// Finds one document by its' id.
        /// Notice that: Find a document by its' key.
        /// </summary>
        /// <param name="id">Document Key Id</param>
        TDocument FindById(string id);


        /// <summary>
        /// Finds one document by its' id asynchronously.
        /// Notice that: Find a document by its' key.
        /// </summary>
        /// <param name="id">Document Key Id</param>
        Task<TDocument> FindByIdAsync(string id);


        /// <summary>
        /// Insert one document by function.
        /// Notice that: Only one entity that fits to given predicate are inserted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        void Insert(TDocument document);


        /// <summary>
        /// Insert one document by function asynchronously.
        /// Notice that: Only one entity that fits to given predicate are inserted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        Task InsertAsync(TDocument document);


        /// <summary>
        /// Insert documents by function.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        void InsertMany(ICollection<TDocument> documents);


        /// <summary>
        /// Insert documents by function asynchronously.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        Task InsertManyAsync(ICollection<TDocument> documents);


        /// <summary>
        /// Update one property of a document by the string property expression.
        /// Notice that: Only one entity that fits to given predicate are retrieved and updated.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="filterExpression">A condition to filter entities by a property</param>
        /// <param name="oldValue">Value to be checked with</param>
        /// <param name="newValue">Value to be updated with</param>
        void UpdateOne(Expression<Func<TDocument, string>> filterExpression, string oldValue, string newValue);


        /// <summary>
        /// Update one property of a document by the string property expression asynchronously.
        /// Notice that: Only one entity that fits to given predicate are retrieved and updated.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="filterExpression">A condition to filter entities by a property</param>
        /// <param name="oldValue">Value to be checked with</param>
        /// <param name="newValue">Value to be updated with</param>
        Task UpdateOneAsync(Expression<Func<TDocument, string>> filterExpression, string oldValue, string newValue);

        /// <summary>
        /// Update one document by function (Replace and create again).
        /// Notice that: Only one entity that fits to given predicate are retrieved and updated.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="filterExpression">A condition to filter entities by the document</param>
        /// <param name="document">Document to be updated with</param>
        void Update(Expression<Func<TDocument, TDocument>> filterExpression, TDocument document);


        /// <summary>
        /// Updates one document by function (Replace and create again) asynchronously.
        /// Notice that: Only one entity that fits to given predicate are retrieved and updated.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="filterExpression">A condition to filter entities by the document</param>
        /// <param name="document">Document to be updated with</param>
        Task UpdateAsync(Expression<Func<TDocument, TDocument>> filterExpression, TDocument document);

        /// <summary>
        /// Deletes one document by function.
        /// Notice that: Only one entity that fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        void DeleteOne(Expression<Func<TDocument, bool>> predicate);

        /// <summary>
        /// Deletes one document by function asynchronously.
        /// Notice that: Only one entity that fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> predicate);

        /// <summary>
        /// Deletes one document by its' id.
        /// Notice that: Delete a document by its' key.
        /// </summary>
        /// <param name="id">Document Key Id</param>
        void DeleteById(string id);

        /// <summary>
        /// Deletes one document by its' id asynchronously.
        /// Notice that: Delete a document by its' key.
        /// </summary>
        /// <param name="id">Document Key Id</param>
        Task DeleteByIdAsync(string id);

        /// <summary>
        /// Deletes all documents by function.
        /// Notice that: All entities that fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        void DeleteMany(Expression<Func<TDocument, bool>> predicate);

        /// <summary>
        /// Deletes all documents by function asynchronously.
        /// Notice that: All entities that fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> predicate);
    }
}
