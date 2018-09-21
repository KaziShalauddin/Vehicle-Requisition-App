using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class VehicleTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vehicle Type")]
        [Remote("IsNameExist", "VehicleType", HttpMethod = "POST", ErrorMessage = "Vehicle Model Already Exist, Try Another")]
        public string TypeName { get; set; }

        public IEnumerable<VehicleType> VehicleTypes { get; set; } 
    }
}