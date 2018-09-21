using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Contracts;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.Repository.DatabaseContext;

namespace VehicleManagementApp.Repository.Repository
{
    public abstract class Repository<T>:IRepository<T> where T:class 
    {

        VehicleDatabaseContext db = new VehicleDatabaseContext();
        public VehicleDatabaseContext Context
        {
            get { return db; }
        }
        public virtual bool Add(T entity)
        {
            db.Set<T>().Add(entity);
            return db.SaveChanges() > 0;
        }
        public virtual bool Update(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        public virtual bool Remove(IDeletable entity)
        {
            entity.IsDeleted = true;
            return Update((T)entity);
        }

        public virtual bool Remove(ICollection<IDeletable> entities)
        {
            int removeCount = 0;

            foreach (var remove in entities)
            {
                var removeData = Remove(remove);
                if (removeData)
                {
                    removeCount++;
                }
            }
            return entities.Count == removeCount;
        }

        public virtual ICollection<T> GetAll(bool withDeleted = false)
        {
            return db.Set<T>().ToList();
        }
         
        public virtual T GetById(int id)
        {
            return db.Set<T>().Find(id);
        }

        public virtual ICollection<T> Get(Expression<Func<T, bool>> query)
        {
            return db.Set<T>().Where(query).ToList();
        }

    }

    public abstract class DeletableRepository<T>:Repository<T> where T:class,IDeletable
    {
        VehicleDatabaseContext db = new VehicleDatabaseContext();


        public override ICollection<T> GetAll(bool withDeleted = false)
        {
            return db.Set<T>().Where(x => x.IsDeleted == false || x.IsDeleted == withDeleted).AsNoTracking().ToList();
        }
    }
}
