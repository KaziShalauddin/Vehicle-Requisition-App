using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.Repository.Repository;

namespace VehicleManagementApp.BLL.Base
{
    public abstract class Manager<T> where T:class
    {
        protected IRepository<T> BaseRepository;

        public Manager(IRepository<T> baseRepository )
        {
            BaseRepository = baseRepository;
        }

        public virtual bool Add(T entity)
        {
            return BaseRepository.Add(entity);
        }

        public virtual bool Update(T entity)
        {
            return BaseRepository.Update(entity);
        }

        public virtual bool Remove(IDeletable entity)
        {
            bool IsDeletable = entity is IDeletable;
            if (!IsDeletable)
            {
                throw new Exception("This Item Is Not Deletable");
            }
            return BaseRepository.Remove((IDeletable) entity);
        }

        public virtual bool Remove(ICollection<IDeletable> entites)
        {
            return BaseRepository.Remove(entites);
        }

        public virtual T GetById(int id)
        {
            return BaseRepository.GetById(id);
        }

        public virtual ICollection<T> GetAll(bool withDeleted = false)
        {
            return BaseRepository.GetAll(withDeleted);
        }

        public virtual ICollection<T> Get(Expression<Func<T, bool>> query)
        {
            return BaseRepository.Get(query);
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    
        //}
    }
}
