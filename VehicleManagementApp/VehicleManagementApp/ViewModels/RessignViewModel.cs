using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class RessignViewModel
    {
        public int Id { get; set; }

        public string Status { get; set; }

        [Display(Name = "Requsition")]
        public int RequsitionId { get; set; }
        public Requsition Requsition { get; set; }

        //public IEnumerable<Requsition> Requsitions { get; set; }
        //public IEnumerable<RequsitionViewModel> RequsitionViewModels { get; set; }
        
        [Display(Name = "Present Driver")]
        public int PresentDriverId { get; set; }
        public Employee PresentDriver { get; set; }

        [Display(Name = "Present Vehicle")]
        public int PresentVehicleId { get; set; }
        public Vehicle PresentVehicle { get; set; }

        [Display(Name = "New Vehicle")]
        public int NewVehicleId { get; set; }
        public Vehicle NewVehicle { get; set; }

        [Display(Name = "New Driver")]
        public int NewDriverId { get; set; }
        public Employee NewDriver { get; set; }

        public IEnumerable<Employee> Drivers { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}