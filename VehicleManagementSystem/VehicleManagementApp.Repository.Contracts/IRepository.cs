using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.Repository.Contracts
{
    public interface IRepository<T> where T:class
    {
        bool Add(T entity);
        bool Update(T entity);
        bool Remove(IDeletable entity);
        bool Remove(ICollection<IDeletable> entites);
        T GetById(int id);
        ICollection<T> GetAll(bool withDeleted = false);
        ICollection<T> Get(Expression<Func<T, bool>> query);
    }
}
