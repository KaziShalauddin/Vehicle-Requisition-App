using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class NotificationViewModel
    {

        public int Id { get; set; }

        public string RequisitionNumber { get; set; }

        [Display(Name = "Starting Place")]
        public string From { get; set; }

        [Display(Name = "Destination")]
        public string To { get; set; }

        [Display(Name = "Journey Start")]
        [DataType(DataType.DateTime)]
        public DateTime JourneyStart { get; set; }


        [Display(Name = "Journey End")]
        [DataType(DataType.DateTime)]
        public DateTime JourneyEnd { get; set; }

        public string Status { get; set; }

        public bool IsEmployeeSeen { get; set; }


    }
}