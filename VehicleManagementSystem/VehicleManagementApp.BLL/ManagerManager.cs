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
    public class ManagerManager:Manager<Manager>,IManagerManager
    {
        private IManagerRepository managerRepository;
        public ManagerManager() : base(new  ManagerRepository())
        {
            managerRepository = (IManagerRepository)base.BaseRepository;
        }

        public ManagerManager(IManagerRepository manager):base(manager)
        {
            managerRepository = manager;
        }
    }
}
