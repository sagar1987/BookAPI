using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain;
using ProductCatalog.Repositories;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.EFRepositories
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T:class
    {
        DbContext _context;
        DbSet<T> ds;
        ObjectCache cache = MemoryCache.Default;
        //If Caching is enabled the fetched data will be stored in cache for subsequent calls.
        public bool IsCachingEnabled;
        public GenericRepository(bool isCachingEnabled, DbContext context)
        {
            _context = context;
            ds = _context.Set<T>();
            IsCachingEnabled = isCachingEnabled;
        }

        public GenericRepository(DbContext context)
        {
            _context = context;
            ds = _context.Set<T>();
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<T> dsTemp = null;
            Type tp = typeof(T);
            string type = tp.ToString();
            if (IsCachingEnabled && cache[type] != null)
                dsTemp = cache[type] as IEnumerable<T>;
            if (dsTemp == null)
            {
                dsTemp = await ds.ToListAsync();
                if (IsCachingEnabled)
                    cache[type] = dsTemp;
            }
            return dsTemp;
        }
        public async virtual Task<T> GetDetails(int id)
        {
            return await ds.FindAsync(id);
        }

        public async virtual Task<T> Add(T entity)
        {
            ds.Add(entity);
            await _context.SaveChangesAsync();
            Type tp = typeof(T);
            string type = tp.ToString();
            cache.Remove(type);
            return entity;
        }
        public async virtual Task Update(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            Type tp = typeof(T);
            string type = tp.ToString();
            cache.Remove(type);
        }
        public async virtual Task Delete(int id)
        {
            T entity = ds.Find(id);
            ds.Remove(entity);
            await _context.SaveChangesAsync();
            Type tp = typeof(T);
            string type = tp.ToString();
            cache.Remove(type);
        }
        public virtual void Dispose()
        {
            _context.Dispose();
        }


    }
}
