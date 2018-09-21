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

        public bool IsNameAlreadyExist(string name)
        {
            var designation = designationRepository.Get(c => c.Name.ToLower().Equals(name.ToLower()));
            if (designation.Count > 0)
            {
                return false;
            }else
            {
                return true;
            }
        }
    }
}
