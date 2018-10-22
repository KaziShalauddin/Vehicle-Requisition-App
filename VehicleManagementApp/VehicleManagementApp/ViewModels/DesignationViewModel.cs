using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class DesignationViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        [Remote("IsNameUnique", "Designation", HttpMethod = "POST", AdditionalFields = "DepartmentId", ErrorMessage = "Designation Already Exist, Try Another")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public IEnumerable<Department> Departments { get; set; } 


    }

   
}