using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Validation;

namespace IYogaKoo.Dao
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected iyogakoodbEntities Context;
        protected DbSet<T> dbSet;
        public Repository()
        {
            this.Context = new iyogakoodbEntities();
            dbSet = Context.Set<T>();
        }
        public Repository(iyogakoodbEntities context)
        {
            this.Context = context;
            dbSet = Context.Set<T>();
        }
        public virtual T Get(object id)
        {
            return dbSet.Find(id);
        }
        public virtual T Add(T entity)
        {
            T t = dbSet.Add(entity);
            try
            {                
                Context.SaveChanges();
            }
            catch (DbEntityValidationException ex) { }
            return t;
        }
        public virtual void Delete(object id)
        {
            T entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }
        public virtual void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);

            }
            dbSet.Remove(entity);
        }
        public virtual void Update(T entityToUpdate)
        {

            dbSet.Attach(entityToUpdate);

            Context.Entry(entityToUpdate).State = EntityState.Modified;


        }
        public virtual IEnumerable<T> Get(
           Func<T, bool> exp)
        {
            return dbSet.Where(exp).ToList();
        }
        public virtual int Save()
        {
            return Context.SaveChanges();
        }
    }
}
