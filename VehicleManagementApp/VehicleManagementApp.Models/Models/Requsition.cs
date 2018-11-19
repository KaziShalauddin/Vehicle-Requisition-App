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
        public string RequsitionNumber { get; set; }

        [Display(Name = "Request For")]
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public string RequestType { get; set; }

        [Display(Name = "Requested By")]
        public int? RequestedBy { get; set; }
        //public int? ManagerId { get; set; }
        //public Manager Manager { get; set; }

        public bool IsDeleted { get; set; }
        public bool withDeleted()
        {
            return IsDeleted;
        }

        public bool IsEmployeeSeen { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsHold { get; set; }
        public bool IsReAssigned { get; set; }
    }
}
