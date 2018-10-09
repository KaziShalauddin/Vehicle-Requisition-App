using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class DriverStatusViewModel
    {
        //[Key]
        //public int Id { get; set; }
        [Required]
        [Display(Name = "Driver Name")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public IEnumerable<Employee> Employees { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = " End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }


        [Required]
        [Display(Name = " End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        public string Status { get; set; }
    }
}