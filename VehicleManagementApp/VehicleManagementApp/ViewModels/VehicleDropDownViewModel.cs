using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class VehicleDropDownViewModel
    {
        public int Id { get; set; }
        public string VehicleDetails { get; set; }
    }
}