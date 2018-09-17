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
    public class OrganaizationManager:Manager<Organaization>,IOrganaizationManager
    {
        private IOrganaizationRepository _repository;

        public OrganaizationManager() : base(new OrganaizationRepository())
        {
            _repository = (OrganaizationRepository)base.BaseRepository;
        }

        public OrganaizationManager(IOrganaizationRepository baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }


        public bool IsExistsByName(string name)
        {
            var organization = _repository.Get(c => c.Name.ToLower().Equals(name.ToLower()));
            if (organization.Count > 0)
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
