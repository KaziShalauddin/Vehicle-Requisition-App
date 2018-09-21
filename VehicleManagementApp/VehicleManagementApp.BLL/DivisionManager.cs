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
    public class DivisionManager:Manager<Division>,IDivisionManager
    {
        private IDivisionRepository _iDivisionRepository;
        public DivisionManager(IDivisionRepository divisionRepository) : base(divisionRepository)
        {
            this._iDivisionRepository = divisionRepository;
        }

        public bool IsNameAlreadyExist(string name)
        {
            var division = _iDivisionRepository.Get(c => c.Name.ToLower().Equals(name.ToLower()));
            if (division.Count > 0)
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
