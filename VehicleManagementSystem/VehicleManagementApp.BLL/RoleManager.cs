using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.BLL.Base;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Repository;
using VehicleManagementApp.Repository.Repository;

namespace VehicleManagementApp.BLL
{
    public class RoleManager:Manager<Role>,IRoleManager
    {
        public RoleManager() : base(new RoleRepository())
        {
        }
    }
}
