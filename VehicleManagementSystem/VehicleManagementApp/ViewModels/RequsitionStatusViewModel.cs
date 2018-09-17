using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VehicleManagementApp.ViewModels
{
    public class RequsitionStatusViewModel
    {
        public int Id { get; set; }

        [Required]
        public string StatusName { get; set; }
    }
}