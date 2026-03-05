using GenericFunctions.Extensions;
using GenericFunctions.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenericFunctions.EFCore
{
    /// <summary>
    /// Implements IRepository for Entity Framework.
    /// </summary>
    /// <typeparam name="TDbContext">DbContext which contains <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    public class EFCoreRepository<TDbContext, TEntity> : Repository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public TDbContext Context => _dbContextProvider.GetDbContext();

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();
        public override DbSet<TEntity> DbSetSqlRaw => Context.Set<TEntity>();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public EFCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public override IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public override IQueryable<TEntity> GetAllIncluding(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            var query = Table.AsNoTracking().AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            return query;
        }

        public override async Task<IList<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public override async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Table.AsNoTracking().ToListAsync();
        }

        public override async Task<IList<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public override async Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAllListAsync(predicate);
        }

        public override async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Table.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity = Update(entity);
            return Task.FromResult(entity);
        }

        protected virtual EntityEntry<TEntity> AttachIfNot(TEntity entity)
        {
            var key = Context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey();
            var keyValues = key.Properties.Select(p => p.PropertyInfo.GetValue(entity)).ToArray();

            var tracked = Context.ChangeTracker.Entries<TEntity>()
                .FirstOrDefault(e =>
                    key.Properties
                        .Select(p => p.PropertyInfo.GetValue(e.Entity))
                        .SequenceEqual(keyValues)
                );
            if (tracked != null)
            {
                tracked.State = EntityState.Detached;
            }

            return Table.Attach(entity);
        }

        public override Task<TEntity> UpdateFieldsAsync(TEntity entity, params string[] updatedProperties)
        {
            if (updatedProperties.Any())
            {
                var dbEntityEntry = AttachIfNot(entity);

                foreach (var property in updatedProperties)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
            }

            return Task.FromResult(entity);
        }

        public override Task<TEntity> UpdateFieldsAsync(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            if (updatedProperties.Any())
            {
                var properties = updatedProperties.Select(x =>
                {
                    var expression = (MemberExpression)x.Body;
                    return expression.Member.Name;
                }).ToArray();

                return UpdateFieldsAsync(entity, properties);
            }

            return Task.FromResult(entity);
        }

        public override TEntity UpdateSingleField(TEntity entity, string property, object value)
        {
            if (string.IsNullOrEmpty(property))
                return entity;

            var dbEntityEntry = AttachIfNot(entity);

            dbEntityEntry.Property(property).IsModified = true;
            dbEntityEntry.Property(property).CurrentValue = value;

            return dbEntityEntry.Entity;
        }

        public override TEntity UpdateSingleField(TEntity entity, Expression<Func<TEntity, object>> updatedProperty, object value)
        {
            var expression = (MemberExpression)updatedProperty.Body;
            string name = expression.Member.Name;
            return UpdateSingleField(entity, name, value);
        }

        public override TEntity UpdateMultipleField(TEntity entity, IDictionary<string, object> keyValue)
        {
            if (keyValue == null)
                return entity;

            var dbEntityEntry = AttachIfNot(entity);

            foreach (var key in keyValue.Keys)
            {
                dbEntityEntry.Property(key).IsModified = true;
                dbEntityEntry.Property(key).CurrentValue = entity.ConvertPropertyTypeValue(key, keyValue[key]);
            }

            return dbEntityEntry.Entity;
        }


        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public override async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        public override async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        public override async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().AsNoTracking().AnyAsync(predicate);
        }

        public override bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().AsNoTracking().Any(predicate);
        }

        public override List<TEntity> CreateList(List<TEntity> items)
        {
            Table.AddRange(items);
            return items;
        }

        public override int DeleteList(List<TEntity> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            try
            {
                Table.RemoveRange(items);
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override IQueryable<TEntity> FindAllInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate
                (DbSet.AsNoTracking().AsQueryable().Where(predicate), (current, includeProperty) => current.Include(includeProperty));
        }

        public override async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }

        public override async Task<int> CreateOrUpdateAsync(List<TEntity> items, string keyId)
        {

            try
            {
                foreach (var item in items)
                {
                    var keyFieldId = item.GetType().GetProperty(keyId).GetValue(item);
                    if ((int)keyFieldId > 0)
                    {
                        var entry = Context.Entry(item);
                        Table.Attach(item);
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        Table.Add(item);
                    }

                }
                await Context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public override async Task<TEntity> FindIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await includeProperties.Aggregate
                (DbSet.AsNoTracking().AsQueryable(), (current, includeProperty) => current.Include(includeProperty)).FirstOrDefaultAsync(predicate);
        }

        public override void DetachLocal(TEntity t, string entryId)
        {
            var local = Context.Set<TEntity>()
                .Local
                .FirstOrDefault(entry => entryId.Equals(entryId));
            if (local != null)
            {
                Context.Entry(local).State = EntityState.Detached;
            }

            Context.Entry(t).State = EntityState.Modified;
        }
        public override async Task<int> ExecuteSqlCommandAsync(string sqlCommand, params object[] parameters)
        {
            return await Context.Database.ExecuteSqlRawAsync(sqlCommand, parameters);
        }


    }
}
