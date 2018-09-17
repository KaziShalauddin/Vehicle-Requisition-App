using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class VehicleTypeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Vehicle Type")]
        public string TypeName { get; set; }

        public IEnumerable<VehicleType> VehicleTypes { get; set; } 
    }
}