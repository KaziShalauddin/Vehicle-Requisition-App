using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.BLL.Base;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Repository;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.Repository.Repository;

namespace VehicleManagementApp.BLL
{
    public class DepartmentManager:Manager<Department>,IDepartmentManager
    {
        private IDepartmentRepository departmentRepository;

        public DepartmentManager():base(new DepartmentRepository())
        {
           this.departmentRepository = (DepartmentRepository) base.BaseRepository;
        }

        public DepartmentManager(IDepartmentRepository Repository) : base(Repository)
        {
            this.departmentRepository = Repository;
        }

        public bool IsNameAlreadyExist(string name)
        {
            var department = departmentRepository.Get(c => c.Name.ToLower().Equals(name.ToLower()));
            if (department.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

   
}
