using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class EditThanaViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Display(Name = "District Name")]
        public int DistrictId { get; set; }
        public District District { get; set; }
        public IEnumerable<District> Districts { get; set; }
    }
}