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
    public class VehicleManager:Manager<Vehicle>,IVehicleManager
    {
        private IVehicleRepository vehicleRepository;
        public VehicleManager(IVehicleRepository Repository) : base(Repository)
        {
            this.vehicleRepository = Repository;
        }

        public bool IsNameAlreadyExist(string vModel)
        {
            var vehicle = vehicleRepository.Get(c => c.VModel == vModel);
            if (vehicle.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsRegistrationAlreadyExist(string vRegistrationNo)
        {
            var registration = vehicleRepository.Get(c => c.VRegistrationNo == vRegistrationNo);
            if (registration.Count > 0)
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
