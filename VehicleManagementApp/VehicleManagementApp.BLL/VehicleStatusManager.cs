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
    public class VehicleStatusManager:Manager<VehicleStatus>,IVehicleStatusManager
    {
        private IVehicleStatusRepository vehicleStatusRepository;

        public VehicleStatusManager() : base(new VehicleStatusRepository())
        {
            this.vehicleStatusRepository = (VehicleStatusRepository)base.BaseRepository;
        }
        public VehicleStatusManager(IVehicleStatusRepository Repository) : base(Repository)
        {
            this.vehicleStatusRepository = Repository;
        }

       

       
    }
}
