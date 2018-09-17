using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organaization Name")]
        public int OrganaizationId { get; set; }
        public Organaization Organaization { get; set; }
        public IEnumerable<Organaization> Organaizations { get; set; }
    }
}