using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VehicleManagementApp.ViewModels
{
    public class EditDivisionViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Division Name")]
        public string Name { get; set; }
    }
}