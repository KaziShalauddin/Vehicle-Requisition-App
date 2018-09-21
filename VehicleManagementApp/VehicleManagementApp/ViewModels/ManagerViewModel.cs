using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class ManagerViewModel
    {
        public int Id { get; set; }
        public string DriverName { get; set; }
        public string DriverNo { get; set; }
        //public string EmployeeName { get; set; }
        public string EmployeNumber { get; set; }
        public string Status { get; set; }

        [Display(Name = "Requsition")]
        public int RequsitionId { get; set; }
        public Requsition Requsition { get; set; }
        public IEnumerable<Requsition> Requsitions { get; set; }

        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }

        [Display(Name = "Driver")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public IEnumerable<Employee> Employees { get; set; }


    }
}