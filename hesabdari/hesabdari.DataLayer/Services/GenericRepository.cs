using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Context;
using Accounting.DataLayer;

namespace Accounting.DataLayer.Services
{
    public class GenericRepository<TEntity> : IDisposable where TEntity : class
    {
        private readonly Accounting_DBEntities _db;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(Accounting_DBEntities db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
            {
                query = query.Where(where);
            }

            return query.ToList();
        }

        public virtual TEntity GetById(object Id)
        {
            return _dbSet.Find(Id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Delete(object Id)
        {
            var entity = GetById(Id);
            Delete(entity);
        }

        public virtual void SaveChanges()
        {
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the error with more detail
                Console.WriteLine($"Error saving changes to the database: {ex.Message}");
                throw; // re-throw the original exception
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}