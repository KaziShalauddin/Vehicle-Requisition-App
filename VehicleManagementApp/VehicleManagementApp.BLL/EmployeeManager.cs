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
    public class EmployeeManager:Manager<Employee>,IEmployeeManager
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeManager():base(new EmployeeRepository())
        {
            _employeeRepository = (EmployeeRepository)base.BaseRepository;
        }

        public EmployeeManager(IEmployeeRepository repository):base(repository)
        {
            this._employeeRepository = repository;
        }

    }
}
