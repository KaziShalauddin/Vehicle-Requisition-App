using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Contracts;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.Models.Models
{
    public class Vehicle:IModel,IDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VehicleName { get; set; }
        public string VModel {get;set;}
        public string VRegistrationNo { get; set; }
        public string VChesisNo { get; set; }
        public string VCapacity { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public bool IsDeleted { get; set; }
        public bool withDeleted()
        {
            return IsDeleted;
        }
    }
}
