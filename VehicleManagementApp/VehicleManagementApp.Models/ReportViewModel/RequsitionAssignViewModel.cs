using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleManagementApp.Models.ReportViewModel
{
    public class RequsitionAssignViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Form { get; set; }
        public string VehicleName { get; set; }
        public string DriverName { get; set; }
        public DateTime JourneyStart { get; set; }
        public DateTime JouneyEnd { get; set; }
    }
}
