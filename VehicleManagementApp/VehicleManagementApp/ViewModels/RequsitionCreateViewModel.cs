using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class RequsitionCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Starting Place")]
        public string Form { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string To { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime JourneyStartDate { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime JourneyStartTime { get; set; }

        [Required]
        [Display(Name = " End Date")]
        [DataType(DataType.Date)]
        public DateTime JouneyEndDate { get; set; }


        [Required]
        [Display(Name = " End Time")]
        [DataType(DataType.Time)]
        public DateTime JouneyEndTime { get; set; }

        //[DataType(DataType.Time)]
        //public DateTime Time { get; set; }

        [Display(Name = "Request For Other")]
        public bool RequestForOther { get; set; }

        [Display(Name = "Employee Name")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public IEnumerable<RequsitionViewModel> RequsitionViewModels { get; set; }

        //[Display(Name = "Employee")]
        //public int EmployeeId { get; set; }
        //public virtual Employee Employee { get; set; }
        //public IEnumerable<Employee> Employees { get; set; }
        //public IEnumerable<Employee> Employees { get; set; }


        //[Display(Name = "Vehicle Name")]
        //public int VehicleId { get; set; }
        //public Vehicle Vehicle { get; set; }
        //public IEnumerable<Vehicle> Vehicles { get; set; }

        //[Display(Name = "Vehicle Name")]
        //public int ManagerId { get; set; }
        //public Manager Manager { get; set; }
        //public IEnumerable<Manager> Managers { get; set; }

        //public int EmployeeViewModelId { get; set; }
        //public EmployeeViewModel EmployeeViewModel { get; set; }
    }
}