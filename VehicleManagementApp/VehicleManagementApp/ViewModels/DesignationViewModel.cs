using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class DesignationViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "Organaization")]
        public int OrganaizationId { get; set; }
        public Organaization Organaization { get; set; }
        public IEnumerable<Organaization> Organaizations { get; set; } 
    }
}