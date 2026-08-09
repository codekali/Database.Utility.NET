using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Codekali.Net.Persistence.Repository
{

    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public IQueryable<TEntity> DbSet
        {
            get
            {
                return GetAll();
            }
        }

        public abstract DbSet<TEntity> DbSetSqlRaw { get; }

        public abstract void DetachLocal(TEntity t, string entryId);

        public abstract IQueryable<TEntity> GetAll();

        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetAll().AsNoTracking();
        }

        public virtual IQueryable<TEntity> GetAllIncluding(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            return GetAll().AsNoTracking();
        }

        public virtual IList<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public virtual Task<IList<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(GetAll().AsEnumerable<TEntity>());
        }

        public virtual IList<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public virtual Task<IList<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public virtual Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllListAsync(predicate);
        }

        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return FirstOrDefaultAsync(predicate);
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().AsNoTracking().FirstOrDefault(predicate);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }


        public abstract TEntity Insert(TEntity entity);

        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public abstract TEntity Update(TEntity entity);

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public abstract Task<TEntity> UpdateFieldsAsync(TEntity entity, params string[] includeProperties);

        public virtual Task<TEntity> UpdateFieldsAsync(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            if (updatedProperties.Any())
            {
                return UpdateFieldsAsync(entity, updatedProperties.Select(x => x.Type.Name).ToArray());
            }

            return UpdateAsync(entity);
        }

        public abstract TEntity UpdateSingleField(TEntity entity, string property, object value);

        public virtual TEntity UpdateSingleField(TEntity entity, Expression<Func<TEntity, object>> updatedProperty, object value)
        {
            return UpdateSingleField(entity, updatedProperty.Type.Name, value);
        }
        public abstract TEntity UpdateMultipleField(TEntity entity, IDictionary<string, object> keyValue);

        public abstract void Delete(TEntity entity);

        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(predicate);
            return Task.FromResult(0);
        }

        public virtual int Count()
        {
            return GetAll().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        public virtual long LongCount()
        {
            return GetAll().LongCount();
        }

        public virtual Task<long> LongCountAsync()
        {
            return Task.FromResult(LongCount());
        }

        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LongCount(predicate));
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Any(predicate));
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().AsNoTracking().Any(predicate);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public abstract List<TEntity> CreateList(List<TEntity> items);

        public async Task<List<TEntity>> CreateListAsync(List<TEntity> items)
        {
            return await Task.FromResult(CreateList(items));
        }

        public abstract int DeleteList(List<TEntity> items);

        public async Task<int> DeleteListAsync(List<TEntity> items)
        {
            return await Task.FromResult(DeleteList(items));
        }

        public abstract IQueryable<TEntity> FindAllInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        public bool IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var count = await CountAsync(predicate);
            return count > 0;
        }


        public abstract Task<TEntity> FindIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        public abstract Task<int> CreateOrUpdateAsync(List<TEntity> items, string keyId = "Id");
        public abstract Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        public abstract Task<int> ExecuteSqlCommandAsync(string sqlCommand, params object[] parameters);
    }
}
