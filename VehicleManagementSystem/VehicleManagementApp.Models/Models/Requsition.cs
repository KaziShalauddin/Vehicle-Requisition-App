using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Contracts;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.Models.Models
{
    public class Requsition:IModel,IDeletable
    {
        [Key]
        public int Id { get; set; }
        public string Form { get; set; }
        public string To { get; set; }
        public string Description { get; set; }
        public DateTime JourneyStart { get; set; }
        public DateTime JouneyEnd { get; set; }
        public string Status { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        
        //public int? ManagerId { get; set; }
        //public Manager Manager { get; set; }

        public bool IsDeleted { get; set; }
        public bool withDeleted()
        {
            return IsDeleted;
        }
    }
}
