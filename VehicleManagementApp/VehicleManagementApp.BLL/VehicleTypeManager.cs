﻿using System;
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
    public class VehicleTypeManager:Manager<VehicleType>,IVehicleTypeManager
    {
        private IVehicleTypeRepository vehicleTypeRepository;
        public VehicleTypeManager(IVehicleTypeRepository Repository) : base(Repository)
        {
            this.vehicleTypeRepository = Repository;
        }

        public bool IsTypeAlreadyExist(string typeName)
        {
            var type = vehicleTypeRepository.Get(c => c.TypeName.ToLower().Equals(typeName.ToLower()));
            if (type.Count > 0)
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
