using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class EditVehicleViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string VehicleName { get; set; }
        [Required]
        [Display(Name = "Model")]
        public string VModel { get; set; }
        [Required]
        [Display(Name = "Registration No")]
        public string VRegistrationNo { get; set; }
        [Required]
        [Display(Name = "Chesis No")]
        public string VChesisNo { get; set; }
        [Required]
        [Display(Name = "Capacity")]
        public string VCapacity { get; set; }

        [Required]
        public string Description { get; set; }
        public string Status { get; set; }

        [Required]
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public IEnumerable<VehicleType> VehicleTypes { get; set; } 
    }
}