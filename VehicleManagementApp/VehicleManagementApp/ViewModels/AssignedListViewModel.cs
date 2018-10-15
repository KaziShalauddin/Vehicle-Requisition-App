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

        [Display(Name = "Requsition")]
        public int RequisitionId { get; set; }
        public Requsition Requisition { get; set; }
       

        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Request For")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Display(Name = "Driver")]
        public int DriverId { get; set; }
        public Employee Driver { get; set; }

        public string Status { get; set; }

    }
}