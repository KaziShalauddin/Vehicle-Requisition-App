using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Contracts;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.Models.Models
{
    
    public class VehicleStatus : IModel, IDeletable
    {
        [Key]
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Status { get; set; }

        public bool IsDeleted { get; set; }
        public bool withDeleted()
        {
            return IsDeleted;
        }
    }
}
