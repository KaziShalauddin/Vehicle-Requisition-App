using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [DisplayName("Department_Name")]
        [Required]
        [Remote("IsNameExist", "Department", HttpMethod = "POST", ErrorMessage = "Organaization Already Exist, Try Another")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Organaization_Name")]
        public int OrganaizationId { get; set; }
        
        public Organaization Organaization { get; set; }
        
        public IEnumerable<Organaization> Organaizations { get; set; }
    }
}