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

namespace VehicleManagementApp.BLL
{
    public class DesignationManager:Manager<Designation>, IDesignationManager
    {
        private IDesignationRepository designationRepository;
        public DesignationManager(IDesignationRepository Repository) : base(Repository)
        {
            this.designationRepository = Repository;
        }
    }
}
