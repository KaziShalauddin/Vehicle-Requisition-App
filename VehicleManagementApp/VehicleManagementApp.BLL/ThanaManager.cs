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
    public class ThanaManager:Manager<Thana>,IThanaManager
    {
        private IThanaRepository _thanaRepository;

        public ThanaManager() : base(new ThanaRepository())
        {
            this._thanaRepository = (ThanaRepository)base.BaseRepository;
           
        }

        public ThanaManager(IThanaRepository thana) : base(thana)
        {
            this._thanaRepository = thana;
        }

        public bool IsThanaAlreadyExist(string name)
        {
            var thana = _thanaRepository.Get(c => c.Name.ToLower().Equals(name.ToLower()));
            if (thana.Count > 0)
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
