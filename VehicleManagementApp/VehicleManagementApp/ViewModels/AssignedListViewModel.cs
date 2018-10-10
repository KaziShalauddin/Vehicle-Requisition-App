using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class AssignedListViewModel
    {
        public int Id { get; set; }

        //public string DriverName { get; set; }
        //public string DriverNo { get; set; }
        //public string EmployeeName { get; set; }
        //public string EmployeNumber { get; set; }
       
        //public DateTime JourneyStart { get; set; }
        //public DateTime JourneyEnd { get; set; }
        //public string VehicleModel { get; set; }
        //public string To { get; set; }
        
        //public string Status { get; set; }
        [Display(Name = "Requsition")]
        public int RequsitionId { get; set; }
        public Requsition Requsition { get; set; }
       

        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Request For")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Display(Name = "Driver")]
        public int DriverId { get; set; }
        public Employee Driver { get; set; }

        //public string Description { get; set; }
        //public string Designation { get; set; }

    }
}