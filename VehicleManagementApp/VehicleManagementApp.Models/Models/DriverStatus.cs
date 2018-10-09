using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Contracts;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.Models.Models
{
    //[Table("Employee")]
    public class DriverStatus:IModel,IDeletable
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

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
