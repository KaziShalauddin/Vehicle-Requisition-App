using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class OrganaizationViewModels
    {
        public int Id { get; set; }

        [Required]
        [Remote("IsNameExist", "Organaization", HttpMethod = "POST", ErrorMessage = "Organaization Already Exist, Try Another")]
        public string Name { get; set; }

        public string Description { get; set; }
        public int OrganaizationId { get; set; }
        public Organaization Organaization { get; set; }
        public IEnumerable<Organaization> Organaizations { get; set; } 
    }
}