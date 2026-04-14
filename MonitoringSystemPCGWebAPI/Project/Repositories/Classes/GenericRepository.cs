
using ApplicationContexts;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Utilities.Classes;
using static System.Net.WebRequestMethods;

namespace Repositories.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public ApplicationContext _context = new ApplicationContext();

        public virtual async Task<T?> InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T?> GetFirstOrDefaultAsync(
    T? filter = null,
    params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            // Apply Includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Apply Dynamic Filter
            if (filter != null)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression? combined = null;

                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(filter);

                    // Skip null values (IMPORTANT improvement)
                    if (value == null) continue;

                    var member = Expression.Property(parameter, property);
                    var constant = Expression.Constant(value, property.PropertyType);

                    var equalsCheck = Expression.Equal(member, constant);

                    combined = combined == null
                        ? equalsCheck
                        : Expression.AndAlso(combined, equalsCheck);
                }

                if (combined != null)
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
                    query = query.Where(lambda);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        //EXAMPLE
        //     return await _personnelRepository.GetAllAsync(
        //    filter,
        //    x => x.Rank,
        //    x => x.Department
        //);
        public virtual async Task<IEnumerable<T>> GetAllAsync(T? filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            // Apply Includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression? combined = null;

                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(filter);
                    var member = Expression.Property(parameter, property);
                    var constant = Expression.Constant(value, property.PropertyType);

                    var isNullCheck = Expression.Equal(constant, Expression.Constant(null, property.PropertyType));
                    var equalsCheck = Expression.Equal(member, constant);
                    var condition = Expression.OrElse(isNullCheck, equalsCheck);

                    combined = combined == null ? condition : Expression.AndAlso(combined, condition);
                }

                if (combined != null)
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
                    query = query.Where(lambda);
                }
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            var keyValue = GetKeyValueAsInt(entity);
            if (keyValue == null) return null;

            var retrievedEntity = await GetByIdAsync(keyValue.Value);
            if (retrievedEntity == null) return null;

            _context.Entry(retrievedEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return retrievedEntity; 
        }

        public virtual async Task<T?> DeleteByIdAsync(int id)
        {
            T? deletedData = await GetByIdAsync(id);
            _context.Set<T>().Remove(deletedData);
            await _context.SaveChangesAsync();
            return deletedData;
        }

        public virtual async Task<IEnumerable<T>> BulkUpdateAsync(List<T> list)
        {
            await _context.BulkUpdateAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public virtual async Task<IEnumerable<T>> BulkInsertAsync(List<T> list)
        {
            await _context.BulkInsertAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public virtual async Task<IEnumerable<T>> BulkUpsertAsync(List<T> entities)
        {
            var (entitiesToInsert, entitiesToUpdate) = SeparateEntities(entities);

            // Bulk Insert
            if (entitiesToInsert.Any())
            {
                await _context.BulkInsertAsync(entitiesToInsert);
            }

            // Bulk Update
            if (entitiesToUpdate.Any())
            {
                await _context.BulkUpdateAsync(entitiesToUpdate);
            }

            await _context.SaveChangesAsync();

            return entities;
        }

        public virtual async Task<IEnumerable<T>> BulkMergeAsync(List<T> entities, Expression<Func<T, bool>>? deleteFilter = null)
        {
            var keyProperty = GetKeyProperty();

            var keepIds = entities
                            .Select(x => keyProperty.GetValue(x))
                            .Where(id => id != null && Convert.ToInt32(id) > 0)
                            .Cast<int>()
                            .ToList();

            IQueryable<T> query = _context.Set<T>();

            if (deleteFilter != null)
            {
                query = query.Where(deleteFilter);
            }

            if (keepIds.Any())
            {
                var entitiesToDelete = await query
                    .Where(x => !keepIds.Contains(EF.Property<int>(x, keyProperty.Name)))
                    .ToListAsync();

                if (entitiesToDelete.Any())
                {
                    await _context.BulkDeleteAsync(entitiesToDelete);
                }
            }

            await BulkUpsertAsync(entities);

            return entities;
        }

        private PropertyInfo GetKeyProperty()
        {
            var keyProperty = typeof(T).GetProperties()
                                .FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Any());

            if (keyProperty == null)
                throw new InvalidOperationException($"No key property found for entity type {typeof(T).Name}");

            return keyProperty;
        }

        private (List<T> entitiesToInsert, List<T> entitiesToUpdate) SeparateEntities(List<T> entities)
        {
            var entitiesToInsert = new List<T>();
            var entitiesToUpdate = new List<T>();

            var keyProperty = GetKeyProperty();

            foreach (var entity in entities)
            {
                var keyValue = keyProperty.GetValue(entity);
                bool isDefaultKey = keyValue == null ||
                                   (keyValue is int intValue && intValue == 0) ||
                                   (keyValue is long longValue && longValue == 0);

                if (isDefaultKey)
                {
                    entitiesToInsert.Add(entity);
                }
                else
                {
                    entitiesToUpdate.Add(entity);
                }
            }
            return (entitiesToInsert, entitiesToUpdate);
        }

        private int? GetKeyValueAsInt(T entity)
        {
            var entityType = typeof(T);
            var keyProperty = GetKeyProperty();

            if (keyProperty == null)
            {
                throw new InvalidOperationException("No Key attribute found on properties.");
            }

            var keyValue = keyProperty.GetValue(entity);

            if (keyValue is int intValue)
            {
                return intValue;
            }

            throw new InvalidOperationException("Key value is not of type int.");
        }


        private T UpdateEntityProperties(T oldEntity, T newEntity)
        {
            var entityType = typeof(T);

            foreach (var property in entityType.GetProperties())
            {
                if (property.CanWrite && !Attribute.IsDefined(property, typeof(KeyAttribute)))
                {
                    var newValue = property.GetValue(newEntity);
                    property.SetValue(oldEntity, newValue);
                }
            }
            return newEntity;

        }

    }
}