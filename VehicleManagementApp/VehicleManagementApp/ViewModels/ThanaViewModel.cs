using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class ThanaViewModel
    {
        public int Id { get; set; }

        [Required]
        [Remote("IsNameExist", "Thana", HttpMethod = "POST", ErrorMessage = "Thana Already Exist, Try Another")]
        public string Name { get; set; }

        [Display(Name = "District Name")]
        public int DistrictId { get; set; }
        public District District { get; set; }
        public IEnumerable<District> Districts { get; set; }
    }
}