using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class DriverDutyViewModel
    {
        //[Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Driver Name")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public IEnumerable<Employee> Employees { get; set; }

        public string RequsitionNumber { get; set; }

        [Display(Name = "Starting Place")]
        public string From { get; set; }

      
        [Display(Name = "Destination")]
        public string To { get; set; }
        
       
        [Display(Name = "Journey Start")]
        public DateTime JourneyStart { get; set; }
        
       
        [Display(Name = "Jouney End")]
        public DateTime JouneyEnd { get; set; }


        //public int RequsitionId { get; set; }
        //public Requsition Requsition { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public IEnumerable<DriverDutyViewModel> Duties { get; set; }
    }
}