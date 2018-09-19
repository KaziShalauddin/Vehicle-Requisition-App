using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.BLL.Base;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.BLL
{
    public class ThanaManager:Manager<Thana>,IThanaManager
    {
        private IThanaRepository _thanaRepository;
        public ThanaManager(IThanaRepository thana) : base(thana)
        {
            this._thanaRepository = thana;
        }
    }
}
