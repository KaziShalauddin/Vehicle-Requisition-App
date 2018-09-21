using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.BLL.Base;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.BLL
{
    public class DistrictManager:Manager<District>,IDistrictManager
    {
        private IDristictRepository _dristictRepository;
        public DistrictManager(IDristictRepository manageRepository) : base(manageRepository)
        {
            this._dristictRepository = manageRepository;
        }

        public bool IsNameAlreadyExist(string name)
        {
            var district = _dristictRepository.Get(c => c.Name.ToLower().Equals(name.ToLower()));
            if (district.Count > 0)
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
