using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class CountCommentViewModel
    {
        //public CountCommentViewModel commentsList;
        public int Id { get; set; }
        public string Status { get; set; }
      

        [Display(Name = "To")]
        public int? ReceiverEmployeeId { get; set; }
        public Employee ReceiverEmployee { get; set; }
     

        public int SenderEmployeeId { get; set; }
        public Employee SenderEmployee { get; set; }
    }
}