using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class FilteringSearchViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Startdate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int DesignationId { get; set; }
        public Designation Designation { get; set; }
        public IEnumerable<ManagerViewModel> ManagerViewModels { get; set; }
        public IEnumerable<RequsitionViewModel> RequsitionViewModels { get; set; }
        public IEnumerable<Designation> Designations { get; set; }
    }
}