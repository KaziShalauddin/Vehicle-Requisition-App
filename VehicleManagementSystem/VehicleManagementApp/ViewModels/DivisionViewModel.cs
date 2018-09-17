using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VehicleManagementApp.ViewModels
{
    public class DivisionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Division Name")]
        public string Name { get; set; }
    }
}