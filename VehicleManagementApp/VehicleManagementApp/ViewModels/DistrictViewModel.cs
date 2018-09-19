using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class DistrictViewModel
    {
        public int Id { get; set; }

        [Display(Name = "District Name")]
        public string Name { get; set; }

        [Display(Name = "Division")]
        public int DivisionId { get; set; }
        public Division Division { get; set; }
        public IEnumerable<Division> Divisions { get; set; }
    }
}