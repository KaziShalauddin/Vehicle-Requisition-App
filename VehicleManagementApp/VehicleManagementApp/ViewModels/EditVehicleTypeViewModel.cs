using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class EditVehicleTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vehicle Type")]
        public string TypeName { get; set; }

        public IEnumerable<VehicleType> VehicleTypes { get; set; } 
    }
}