using LMS.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LMS.Repositories.Generic
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            dbSet = _context.Set<T>();
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disabledTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (disabledTracking)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        public T GetById(object id)
        {
            return dbSet.Find(id);
        }

        public T GetByIdAsync(Expression<Func<T, bool>> filter=null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include= null,
            bool disabledTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (disabledTracking)
            {
                query = query.AsNoTracking();
            }
            if(filter!= null)
            {
                query=query.Where(filter);
            }
            if(include != null)
            {
                query = include(query);
            }
            return query.FirstOrDefault();
        }

        
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public void AddRange(List<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<T> DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void DeleteRange(List<T> entityList)
        {
            dbSet.RemoveRange(entityList);
        }

        public bool Exists(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                return query.Any(filter);
            else
                return query.Any();
        }

        

    }
}